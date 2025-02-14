using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public UserViewModel UserViewModel { get; }
        public VehicleViewModel VehicleViewModel { get; }
        public ReservationViewModel ReservationViewModel { get; }

        // 🔹 Przekierowanie dla użytkowników
        public ObservableCollection<User> Users => UserViewModel.Users;
        public User SelectedUser
        {
            get => UserViewModel.SelectedUser;
            set
            {
                UserViewModel.SelectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        // 🔹 Przekierowanie dla pojazdów
        public ObservableCollection<Vehicle> Vehicles => VehicleViewModel.Vehicles;
        public Vehicle SelectedVehicle
        {
            get => VehicleViewModel.SelectedVehicle;
            set
            {
                VehicleViewModel.SelectedVehicle = value;
                OnPropertyChanged(nameof(SelectedVehicle));
            }
        }

        // 🔹 Przekierowanie dla rezerwacji
        public DateTime SelectedDate
        {
            get => ReservationViewModel.SelectedDate;
            set
            {
                ReservationViewModel.SelectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public ObservableCollection<int> AvailableHours => ReservationViewModel.AvailableHours;
        public ObservableCollection<int> AvailableMinutes => ReservationViewModel.AvailableMinutes;

        public int SelectedStartHour
        {
            get => ReservationViewModel.SelectedStartHour;
            set
            {
                ReservationViewModel.SelectedStartHour = value;
                OnPropertyChanged(nameof(SelectedStartHour));
            }
        }

        public int SelectedStartMinute
        {
            get => ReservationViewModel.SelectedStartMinute;
            set
            {
                ReservationViewModel.SelectedStartMinute = value;
                OnPropertyChanged(nameof(SelectedStartMinute));
            }
        }

        public int SelectedEndHour
        {
            get => ReservationViewModel.SelectedEndHour;
            set
            {
                ReservationViewModel.SelectedEndHour = value;
                OnPropertyChanged(nameof(SelectedEndHour));
            }
        }

        public int SelectedEndMinute
        {
            get => ReservationViewModel.SelectedEndMinute;
            set
            {
                ReservationViewModel.SelectedEndMinute = value;
                OnPropertyChanged(nameof(SelectedEndMinute));
            }
        }

        public string ReservationDuration => ReservationViewModel.ReservationDuration;

        // 🔹 Nowe pola związane z rezerwacją
        public ObservableCollection<string> ParkingZones => ReservationViewModel.ParkingZones;
        public string SelectedZone
        {
            get => ReservationViewModel.SelectedZone;
            set
            {
                ReservationViewModel.SelectedZone = value;
                OnPropertyChanged(nameof(SelectedZone));
            }
        }

        public ObservableCollection<int> ParkingNumbers => ReservationViewModel.ParkingNumbers;
        public int SelectedNumber
        {
            get => ReservationViewModel.SelectedNumber;
            set
            {
                ReservationViewModel.SelectedNumber = value;
                OnPropertyChanged(nameof(SelectedNumber));
            }
        }

        public ObservableCollection<Vehicle> UserVehicles => ReservationViewModel.UserVehicles;
        public Vehicle SelectedVehicleForReservation
        {
            get => ReservationViewModel.SelectedVehicle;
            set
            {
                ReservationViewModel.SelectedVehicle = value;
                OnPropertyChanged(nameof(SelectedVehicleForReservation));
            }
        }

        // 🔥 Konstruktor
        public MainViewModel(UserViewModel userViewModel, VehicleViewModel vehicleViewModel, ReservationViewModel reservationViewModel)
        {
            UserViewModel = userViewModel;
            VehicleViewModel = vehicleViewModel;
            ReservationViewModel = reservationViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
