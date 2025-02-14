using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.ViewModels;

namespace PerkingRezerWPFDM.Views
{
    public partial class AddUserWindow : Window
    {
        private AddUserViewModel _viewModel;

        public AddUserWindow(ObservableCollection<User> users)
        {
            InitializeComponent();
            _viewModel = new AddUserViewModel(users);
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.NewUser.Password = ((PasswordBox)sender).Password;
        }
    }

}
