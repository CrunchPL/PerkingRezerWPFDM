using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PerkingRezerWPFDM.ViewModels;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.Database;

namespace PerkingRezerWPFDM
{
        public partial class LoginWindow : Window
        {
            private readonly UserViewModel _userViewModel;

            public LoginWindow()
            {
                InitializeComponent();
                this.DataContext = new UserViewModel();
            }

            private string HashPassword(string password)
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(bytes);
                }
            }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;
            var hashedPassword = HashPassword(password);

            using (var db = new ParkingDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hashedPassword);

                if (user != null)
                {
                    MainWindow mainWindow = new MainWindow(user); // Przekazujemy użytkownika
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ErrorText.Text = "Niepoprawny login lub hasło!";
                    ErrorText.Visibility = Visibility.Visible;
                }
            }
        }



    }
}
