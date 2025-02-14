using System;
using System.Linq;
using System.Windows;
using PerkingRezerWPFDM.Database;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.ViewModels;

namespace PerkingRezerWPFDM.Views
{
    public partial class AddVehicleWindow : Window
    {
        private readonly int _loggedUserId;
        private readonly VehicleViewModel _vehicleViewModel;

        public AddVehicleWindow(int loggedUserId, VehicleViewModel vehicleViewModel)
        {
            InitializeComponent();
            _loggedUserId = loggedUserId;
            _vehicleViewModel = vehicleViewModel;

            // Ustawienie DataContext dla ComboBox
            VehicleTypeComboBox.ItemsSource = vehicleViewModel.VehicleTypes;
            FuelTypeComboBox.ItemsSource = vehicleViewModel.FuelTypes;
        }

        private void AddVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LicensePlateTextBox.Text) ||
                string.IsNullOrWhiteSpace(BrandTextBox.Text) ||
                string.IsNullOrWhiteSpace(ModelTextBox.Text) ||
                VehicleTypeComboBox.SelectedItem == null ||
                FuelTypeComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(YearTextBox.Text))
            {
                MessageBox.Show("Wypełnij wszystkie pola!");
                return;
            }

            if (!int.TryParse(YearTextBox.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
            {
                MessageBox.Show("Podaj poprawny rok (np. 2023).");
                return;
            }

            using (var db = new ParkingDbContext())
            {
                var newVehicle = new Vehicle
                {
                    LicensePlate = LicensePlateTextBox.Text,
                    Brand = BrandTextBox.Text,
                    Model = ModelTextBox.Text,
                    VehicleType = VehicleTypeComboBox.SelectedItem.ToString(),
                    FuelType = FuelTypeComboBox.SelectedItem.ToString(),
                    Year = year,
                    OwnerId = _loggedUserId
                };

                db.Vehicles.Add(newVehicle);
                db.SaveChanges();
            }

            _vehicleViewModel.LoadUserVehicles(); // 🚀 Odświeżenie listy
            DialogResult = true; // ✅ Informujemy, że pojazd został dodany
            Close();
        }

    }
}
