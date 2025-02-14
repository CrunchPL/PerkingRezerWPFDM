using System.Windows;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.ViewModels;
using PerkingRezerWPFDM.Views;

namespace PerkingRezerWPFDM
{
    public partial class MainWindow : Window
    {
        private User _loggedInUser;
        private ReservationViewModel _reservationViewModel;
        private ReservationLogViewModel _reservationLogViewModel;

        public MainWindow(User loggedUser)
        {
            InitializeComponent();
            _loggedInUser = loggedUser;

            // 🚀 Stworzenie instancji ViewModeli
            var userViewModel = new UserViewModel();
            var vehicleViewModel = new VehicleViewModel(_loggedInUser);
            _reservationViewModel = new ReservationViewModel(_loggedInUser);
            _reservationLogViewModel = new ReservationLogViewModel();
            var mainViewModel = new MainViewModel(userViewModel, vehicleViewModel, _reservationViewModel);

            // 🚀 Ustawienie kontekstu danych
            DataContext = mainViewModel;
            ReservationsTab.DataContext = _reservationViewModel;
            AdminReservationsTab.DataContext = _reservationLogViewModel;

            // Ukrywanie zakładek dla zwykłych użytkowników
            if (_loggedInUser.Role != "Admin")
            {
                UsersTab.Visibility = Visibility.Collapsed;
                AdminReservationsTab.Visibility = Visibility.Collapsed;
            }

            // Ustawienie domyślnej zakładki
            MainTabControl.SelectedItem = ReservationsTab;
        }

        // 🚀 Obsługa przycisku "Wyloguj"
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        // 🚀 Obsługa przycisku "Zmień hasło"
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var changePasswordWindow = new ChangePasswordWindow(_loggedInUser);
            changePasswordWindow.ShowDialog();
        }
    }
}
