using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PerkingRezerWPFDM.Database;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.ViewModels
{
    public class AddUserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _users;
        public User NewUser { get; set; }
        public ObservableCollection<string> Roles { get; set; }
        public ICommand AddUserCommand { get; }

        public AddUserViewModel(ObservableCollection<User> users)
        {
            _users = users;
            NewUser = new User();
            Roles = new ObservableCollection<string> { "Admin", "Użytkownik" };
            AddUserCommand = new RelayCommand(AddUser);
        }

        private void AddUser()
        {
            if (string.IsNullOrEmpty(NewUser.Username) || string.IsNullOrEmpty(NewUser.Role) || string.IsNullOrEmpty(NewUser.Password))
            {
                MessageBox.Show("Wszystkie pola są wymagane!");
                return;
            }

            // Hashowanie hasła przed zapisaniem
            NewUser.PasswordHash = HashPassword(NewUser.Password);

            // ✅ Zapisanie użytkownika do bazy danych
            using (var db = new ParkingDbContext())
            {
                db.Users.Add(NewUser);
                db.SaveChanges();
            }

            _users.Add(NewUser); // Dodanie do ObservableCollection (widok się odświeży)

            MessageBox.Show($"Dodano użytkownika: {NewUser.Username}");
            Application.Current.Windows[1].Close(); // Zamknięcie okna po dodaniu
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
