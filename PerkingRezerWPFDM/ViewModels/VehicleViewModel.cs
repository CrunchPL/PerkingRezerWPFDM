using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.Database;
using PerkingRezerWPFDM.Views;

namespace PerkingRezerWPFDM.ViewModels
{
    public class VehicleViewModel : INotifyPropertyChanged
    {
        private Vehicle _selectedVehicle;
        private readonly User _loggedInUser;

        public ObservableCollection<Vehicle> Vehicles { get; set; }

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                _selectedVehicle = value;
                Console.WriteLine($"Wybrano pojazd: {_selectedVehicle?.LicensePlate}");
                OnPropertyChanged(nameof(SelectedVehicle));

                // Upewniamy się, że przyciski odświeżają stan (Edytuj/Usuń)
                (EditVehicleCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteVehicleCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public List<string> VehicleTypes { get; } = new List<string>
        {
            "Czterokołowiec", "Motocykl", "Ciężarówka"
        };

        public List<string> FuelTypes { get; } = new List<string>
        {
            "Benzyna", "Benzyna+LPG", "Diesel", "Elektryczny", "Hybrydowy"
        };

        public ICommand AddVehicleCommand { get; }
        public ICommand EditVehicleCommand { get; }
        public ICommand DeleteVehicleCommand { get; }

        public VehicleViewModel(User loggedInUser)
        {
            _loggedInUser = loggedInUser ?? throw new ArgumentNullException(nameof(loggedInUser));
            Vehicles = new ObservableCollection<Vehicle>();
            LoadUserVehicles();

            AddVehicleCommand = new RelayCommand(AddVehicle);
            EditVehicleCommand = new RelayCommand(EditVehicle, () => SelectedVehicle != null);
            DeleteVehicleCommand = new RelayCommand(DeleteVehicle, () => SelectedVehicle != null);
        }

        public void LoadUserVehicles()
        {
            using (var db = new ParkingDbContext())
            {
                Console.WriteLine("Ładowanie pojazdów dla użytkownika: " + _loggedInUser.Id);
                var userVehicles = db.Vehicles.Where(v => v.OwnerId == _loggedInUser.Id).ToList();

                Vehicles.Clear();
                foreach (var vehicle in userVehicles)
                {
                    Console.WriteLine($"Załadowano: {vehicle.LicensePlate} {vehicle.Brand} {vehicle.Model}");
                    Vehicles.Add(vehicle);
                }

                // Upewniamy się, że po załadowaniu lista jest poprawnie zaktualizowana
                OnPropertyChanged(nameof(Vehicles));
            }
        }

        private void AddVehicle()
        {
            var addVehicleWindow = new AddVehicleWindow(_loggedInUser.Id, this);
            bool? result = addVehicleWindow.ShowDialog(); // Czekamy na zamknięcie okna

            if (result == true) // Jeżeli dodano pojazd, odśwież listę
            {
                LoadUserVehicles();
            }
        }

        private void EditVehicle()
        {
            if (SelectedVehicle != null)
            {
                var editWindow = new EditVehicleWindow(SelectedVehicle, VehicleTypes, FuelTypes, this);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadUserVehicles(); // Odśwież listę po edycji
                }
            }
        }



        private void DeleteVehicle()
        {
            if (SelectedVehicle != null)
            {
                var result = MessageBox.Show($"Czy na pewno chcesz usunąć pojazd {SelectedVehicle.LicensePlate}?",
                    "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new ParkingDbContext())
                    {
                        var vehicleToDelete = db.Vehicles.FirstOrDefault(v => v.Id == SelectedVehicle.Id);
                        if (vehicleToDelete != null)
                        {
                            db.Vehicles.Remove(vehicleToDelete);
                            db.SaveChanges();
                        }
                    }
                    LoadUserVehicles();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
