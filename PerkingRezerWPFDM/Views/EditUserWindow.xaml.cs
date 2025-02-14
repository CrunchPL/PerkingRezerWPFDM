using System;
using System.Linq;
using System.Windows;
using PerkingRezerWPFDM.Database;
using PerkingRezerWPFDM.Models;
using System.Security.Cryptography;
using System.Text;

namespace PerkingRezerWPFDM.Views
{
    public partial class EditUserWindow : Window
    {
        private User _userToEdit;

        public EditUserWindow(User user)
        {
            InitializeComponent();
            _userToEdit = user;

            // Wypełnienie pól edycji danymi użytkownika
            UsernameBox.Text = _userToEdit.Username;
            FirstNameBox.Text = _userToEdit.FirstName;
            LastNameBox.Text = _userToEdit.LastName;
            RoleBox.SelectedIndex = _userToEdit.Role == "Admin" ? 0 : 1; // Admin = index 0, User = index 1
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == _userToEdit.Id);
                if (user != null)
                {
                    // Aktualizacja danych użytkownika
                    user.Username = UsernameBox.Text;
                    user.FirstName = FirstNameBox.Text;
                    user.LastName = LastNameBox.Text;
                    user.Role = RoleBox.SelectedIndex == 0 ? "Admin" : "User"; // Przypisanie roli

                    if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
                    {
                        // Jeśli wpisano nowe hasło, aktualizujemy je w bazie z haszowaniem
                        user.PasswordHash = HashPassword(PasswordBox.Password);
                    }

                    db.SaveChanges(); // Zapisanie zmian w bazie

                    MessageBox.Show("Dane użytkownika zostały zapisane.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nie znaleziono użytkownika w bazie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
