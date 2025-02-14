using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.ViewModels;
using PerkingRezerWPFDM.Database;

namespace PerkingRezerWPFDM.Views
{
    public partial class EditVehicleWindow : Window
    {
        private VehicleViewModel _viewModel;
        public Vehicle Vehicle { get; set; }

        // Dodajemy listy do powiązania z ComboBox
        public List<string> VehicleTypes { get; set; }
        public List<string> FuelTypes { get; set; }

        public ICommand SaveCommand { get; }

        public EditVehicleWindow(Vehicle vehicle, List<string> vehicleTypes, List<string> fuelTypes, VehicleViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            // Ustawienie list dla ComboBox
            VehicleTypes = vehicleTypes;
            FuelTypes = fuelTypes;

            Vehicle = new Vehicle
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                VehicleType = vehicle.VehicleType,
                FuelType = vehicle.FuelType,
                Year = vehicle.Year
            };

            SaveCommand = new RelayCommand(SaveVehicle);
            DataContext = this; // Ustawienie kontekstu danych
        }

        private void SaveVehicle()
        {
            using (var db = new ParkingDbContext())
            {
                var vehicleToUpdate = db.Vehicles.Find(Vehicle.Id);
                if (vehicleToUpdate != null)
                {
                    vehicleToUpdate.LicensePlate = Vehicle.LicensePlate;
                    vehicleToUpdate.Brand = Vehicle.Brand;
                    vehicleToUpdate.Model = Vehicle.Model;
                    vehicleToUpdate.VehicleType = Vehicle.VehicleType;
                    vehicleToUpdate.FuelType = Vehicle.FuelType;
                    vehicleToUpdate.Year = Vehicle.Year;

                    db.SaveChanges();
                }
            }

            _viewModel.LoadUserVehicles(); // Odśwież listę pojazdów
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
