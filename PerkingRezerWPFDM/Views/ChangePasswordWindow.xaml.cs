using System;
using System.Linq;
using System.Windows;
using System.Security.Cryptography;
using System.Text;
using PerkingRezerWPFDM.Database;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.Views
{
    public partial class ChangePasswordWindow : Window
    {
        private User _loggedInUser;

        public ChangePasswordWindow(User loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Hasło nie może być puste.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Hasła nie są zgodne.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new ParkingDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == _loggedInUser.Id);
                if (user != null)
                {
                    user.PasswordHash = HashPassword(newPassword); // ✅ Haszowanie nowego hasła
                    db.SaveChanges(); // ✅ Zapisanie zmian do bazy danych

                    // 🔄 Aktualizacja danych użytkownika w aplikacji
                    _loggedInUser.PasswordHash = user.PasswordHash;

                    MessageBox.Show("Hasło zostało zmienione.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("Błąd zmiany hasła. Użytkownik nie został znaleziony.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
