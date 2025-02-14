using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using PerkingRezerWPFDM.Database;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.ViewModels
{
    public class ParkingSpotViewModel : INotifyPropertyChanged
    {
        private ParkingSpot _selectedParkingSpot;
        private Vehicle _selectedVehicle;

        public ObservableCollection<ParkingSpot> ParkingSpots { get; set; }
        public ObservableCollection<Vehicle> UserVehicles { get; set; }

        public ParkingSpot SelectedParkingSpot
        {
            get => _selectedParkingSpot;
            set
            {
                _selectedParkingSpot = value;
                OnPropertyChanged(nameof(SelectedParkingSpot));
            }
        }

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged(nameof(SelectedVehicle));
            }
        }

        public ICommand AssignVehicleCommand { get; }

        public ParkingSpotViewModel(int userId)
        {
            using (var db = new ParkingDbContext())
            {
                // Wczytujemy miejsca parkingowe
                ParkingSpots = new ObservableCollection<ParkingSpot>(db.ParkingSpots.ToList());

                // Wczytujemy pojazdy zalogowanego użytkownika
                UserVehicles = new ObservableCollection<Vehicle>(db.Vehicles.Where(v => v.OwnerId == userId).ToList());
            }

            AssignVehicleCommand = new RelayCommand(AssignVehicle, CanAssignVehicle);
        }

        private bool CanAssignVehicle()
        {
            return SelectedParkingSpot != null && SelectedVehicle != null && !SelectedParkingSpot.IsOccupied;
        }

        private void AssignVehicle()
        {
            if (SelectedParkingSpot == null || SelectedVehicle == null) return;

            using (var db = new ParkingDbContext())
            {
                var spot = db.ParkingSpots.FirstOrDefault(s => s.Id == SelectedParkingSpot.Id);
                if (spot != null && !spot.IsOccupied)
                {
                    spot.IsOccupied = true;
                    spot.VehicleId = SelectedVehicle.Id;  // Przypisanie pojazdu do miejsca
                    db.SaveChanges();
                }
            }

            // Odświeżenie widoku
            OnPropertyChanged(nameof(ParkingSpots));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
