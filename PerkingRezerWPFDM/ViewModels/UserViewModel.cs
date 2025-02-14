using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.Views;
using PerkingRezerWPFDM.Database;

namespace PerkingRezerWPFDM.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private User _selectedUser;
        public ObservableCollection<User> Users { get; set; }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                ((RelayCommand)EditUserCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteUserCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand OpenAddUserWindowCommand { get; }

        public UserViewModel()
        {
            Users = new ObservableCollection<User>();

            LoadUsersFromDatabase();
            EnsureAdminAccountExists();

            EditUserCommand = new RelayCommand(EditUser, () => SelectedUser != null);
            DeleteUserCommand = new RelayCommand(DeleteUser, () => SelectedUser != null);
            OpenAddUserWindowCommand = new RelayCommand(OpenAddUserWindow);
        }

        public void LoadUsersFromDatabase()
        {
            using (var db = new ParkingDbContext())
            {
                var usersFromDb = db.Users.ToList();
                Users.Clear();
                foreach (var user in usersFromDb)
                {
                    Users.Add(user);
                }
            }
        }

        private void EnsureAdminAccountExists()
        {
            using (var db = new ParkingDbContext())
            {
                if (!db.Users.Any(u => u.Username == "admin"))
                {
                    var adminUser = new User
                    {
                        Username = "admin",
                        FirstName = "Admin",
                        LastName = "Systemowy",
                        Role = "Admin",
                        Password = "admin",
                        PasswordHash = HashPassword("admin")
                    };

                    db.Users.Add(adminUser);
                    db.SaveChanges();
                    Users.Add(adminUser);
                }
            }
        }

        private void EditUser()
        {
            if (SelectedUser == null) return;
            var editWindow = new EditUserWindow(SelectedUser);
            editWindow.ShowDialog();
            LoadUsersFromDatabase();
        }

        private void DeleteUser()
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show($"Czy na pewno chcesz usunąć użytkownika {SelectedUser.Username}?",
                                         "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new ParkingDbContext())
                {
                    var userToRemove = db.Users.FirstOrDefault(u => u.Username == SelectedUser.Username);
                    if (userToRemove != null)
                    {
                        db.Users.Remove(userToRemove);
                        db.SaveChanges();
                    }
                }
                Users.Remove(SelectedUser);
            }
        }

        private void OpenAddUserWindow()
        {
            var addUserWindow = new AddUserWindow(Users);
            addUserWindow.ShowDialog();
            LoadUsersFromDatabase();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
