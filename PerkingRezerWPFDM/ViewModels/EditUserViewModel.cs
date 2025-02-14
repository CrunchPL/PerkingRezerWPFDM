using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.Database;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PerkingRezerWPFDM.ViewModels
{
    public class EditUserViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;

        private string _firstName;
        private string _lastName;
        private string _role;
        private string _password;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Username { get; }  // Login użytkownika - nie można zmieniać

        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public string Password
        {
            private get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand SaveUserCommand { get; }
        public ICommand CancelCommand { get; }

        public EditUserViewModel(Window window, User user)
        {
            _window = window;

            // Wczytanie danych użytkownika do pól
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Role = user.Role;
            Password = ""; // Pole hasła puste domyślnie

            SaveUserCommand = new RelayCommand(SaveUser);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void SaveUser()
        {
            using (var db = new ParkingDbContext())
            {
                var userToEdit = db.Users.FirstOrDefault(u => u.Username == Username);
                if (userToEdit != null)
                {
                    userToEdit.FirstName = FirstName;
                    userToEdit.LastName = LastName;
                    userToEdit.Role = Role;

                    // Jeśli wpisano nowe hasło, aktualizujemy je po zahashowaniu
                    if (!string.IsNullOrWhiteSpace(Password))
                    {
                        userToEdit.PasswordHash = HashPassword(Password);
                    }

                    // Oznaczamy encję jako zmodyfikowaną, aby wymusić aktualizację
                    db.Entry(userToEdit).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            // Odświeżenie listy użytkowników w głównym oknie
            var userViewModel = Application.Current.Windows
                .OfType<MainWindow>().FirstOrDefault()?.DataContext as UserViewModel;
            userViewModel?.LoadUsersFromDatabase();

            MessageBox.Show($"Zapisano zmiany dla użytkownika: {Username}");
            _window.Close();
        }

        private void Cancel()
        {
            _window.Close();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
