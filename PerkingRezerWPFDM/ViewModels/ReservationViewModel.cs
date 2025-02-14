using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PerkingRezerWPFDM.Database;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.Views;

namespace PerkingRezerWPFDM.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        private readonly User _loggedInUser;
        private ReservationDisplayModel _selectedReservation;
        private string _selectedZone;
        private int _selectedNumber;
        private Vehicle _selectedVehicle;
        private DateTime _selectedDate = DateTime.Today;
        private int _selectedStartHour;
        private int _selectedStartMinute;
        private int _selectedEndHour;
        private int _selectedEndMinute;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<string> ParkingZones { get; set; } = new ObservableCollection<string> { "A", "B", "C", "D", "E" };
        public ObservableCollection<int> ParkingNumbers { get; set; } = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public ObservableCollection<Vehicle> UserVehicles { get; set; } = new ObservableCollection<Vehicle>();
        public ObservableCollection<ReservationDisplayModel> Reservations { get; set; } = new ObservableCollection<ReservationDisplayModel>();

        public string SelectedZone
        {
            get => _selectedZone;
            set { _selectedZone = value; OnPropertyChanged(nameof(SelectedZone)); }
        }

        public int SelectedNumber
        {
            get => _selectedNumber;
            set { _selectedNumber = value; OnPropertyChanged(nameof(SelectedNumber)); }
        }

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set { _selectedVehicle = value; OnPropertyChanged(nameof(SelectedVehicle)); }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
        }

        public ObservableCollection<int> AvailableHours { get; set; } = new ObservableCollection<int>(Enumerable.Range(6, 17));
        public ObservableCollection<int> AvailableMinutes { get; set; } = new ObservableCollection<int> { 0, 10, 20, 30, 40, 50 };

        public int SelectedStartHour
        {
            get => _selectedStartHour;
            set { _selectedStartHour = value; OnPropertyChanged(nameof(SelectedStartHour)); }
        }

        public int SelectedStartMinute
        {
            get => _selectedStartMinute;
            set { _selectedStartMinute = value; OnPropertyChanged(nameof(SelectedStartMinute)); }
        }

        public int SelectedEndHour
        {
            get => _selectedEndHour;
            set { _selectedEndHour = value; OnPropertyChanged(nameof(SelectedEndHour)); }
        }

        public int SelectedEndMinute
        {
            get => _selectedEndMinute;
            set { _selectedEndMinute = value; OnPropertyChanged(nameof(SelectedEndMinute)); }
        }

        public string ReservationDuration
        {
            get
            {
                int startTime = (SelectedStartHour * 60) + SelectedStartMinute;
                int endTime = (SelectedEndHour * 60) + SelectedEndMinute;
                int duration = endTime - startTime;
                return duration > 0 ? $"{duration} minut" : "Błąd wyboru czasu";
            }
        }

        public ReservationDisplayModel SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
                ((RelayCommand)EditReservationCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteReservationCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand ReserveSpotCommand { get; }
        public ICommand EditReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }

        public ReservationViewModel(User loggedInUser)
        {
            _loggedInUser = loggedInUser;
            LoadUserVehicles();
            LoadUserReservations();
            ReserveSpotCommand = new RelayCommand(ReserveSpot);
            EditReservationCommand = new RelayCommand(EditReservation, () => SelectedReservation != null);
            DeleteReservationCommand = new RelayCommand(DeleteReservation, () => SelectedReservation != null);
        }

        private void LoadUserVehicles()
        {
            using (var db = new ParkingDbContext())
            {
                var vehicles = db.Vehicles.Where(v => v.OwnerId == _loggedInUser.Id).ToList();
                UserVehicles.Clear();
                foreach (var vehicle in vehicles) { UserVehicles.Add(vehicle); }
            }
        }

        private void LoadUserReservations()
        {
            using (var db = new ParkingDbContext())
            {
                var userReservations = db.Reservations
                    .Where(r => r.UserId == _loggedInUser.Id)
                    .Select(r => new ReservationDisplayModel
                    {
                        ReservationId = r.Id,
                        ParkingSpot = r.ParkingSpotId,
                        LicensePlate = db.Vehicles.Where(v => v.Id == r.VehicleId).Select(v => v.LicensePlate).FirstOrDefault(),
                        ReservationDate = r.ReservationDate.ToString("dd.MM.yyyy"),
                        StartTime = $"{r.StartHour:D2}:{r.StartMinute:D2}",
                        EndTime = $"{r.EndHour:D2}:{r.EndMinute:D2}",
                        TotalDuration = $"{(r.EndHour * 60 + r.EndMinute) - (r.StartHour * 60 + r.StartMinute)} min"
                    })
                    .ToList();

                Reservations.Clear();
                foreach (var res in userReservations)
                {
                    Reservations.Add(res);
                }
            }
        }

        private void ReserveSpot()
        {
            if (SelectedVehicle == null)
            {
                MessageBox.Show("Proszę wybrać pojazd przed rezerwacją.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string spot = $"{SelectedZone}{SelectedNumber}";
            using (var db = new ParkingDbContext())
            {
                var newReservation = new Reservation
                {
                    ParkingSpotId = spot,
                    ReservationDate = SelectedDate,
                    StartHour = SelectedStartHour,
                    StartMinute = SelectedStartMinute,
                    EndHour = SelectedEndHour,
                    EndMinute = SelectedEndMinute,
                    VehicleId = SelectedVehicle.Id,
                    UserId = _loggedInUser.Id
                };

                db.Reservations.Add(newReservation);
                db.SaveChanges();
                LoadUserReservations();
                MessageBox.Show($"Zarezerwowano miejsce {spot}.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditReservation()
        {
            if (SelectedReservation == null) return;

            var editWindow = new EditReservationWindow(SelectedReservation);
            if (editWindow.ShowDialog() == true)
            {
                using (var db = new ParkingDbContext())
                {
                    var reservation = db.Reservations.Find(SelectedReservation.ReservationId);
                    if (reservation != null)
                    {
                        reservation.StartHour = editWindow.NewStartHour;
                        reservation.StartMinute = editWindow.NewStartMinute;
                        reservation.EndHour = editWindow.NewEndHour;
                        reservation.EndMinute = editWindow.NewEndMinute;
                        db.SaveChanges();
                    }
                }

                LoadUserReservations();
                MessageBox.Show("Rezerwacja została zaktualizowana.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteReservation()
        {
            if (SelectedReservation == null) return;

            using (var db = new ParkingDbContext())
            {
                var reservation = db.Reservations.Find(SelectedReservation.ReservationId);
                if (reservation != null)
                {
                    db.Reservations.Remove(reservation);
                    db.SaveChanges();
                }
            }

            LoadUserReservations();
            MessageBox.Show("Rezerwacja została usunięta.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
