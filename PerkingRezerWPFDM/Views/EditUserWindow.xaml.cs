using System.Windows;
using PerkingRezerWPFDM.ViewModels;
using PerkingRezerWPFDM.Models;
using PerkingRezerWPFDM.ViewModels;

namespace PerkingRezerWPFDM.Views
{
    public partial class EditUserWindow : Window
    {
        public EditUserWindow(User user)
        {
            InitializeComponent();
            DataContext = new EditUserViewModel(this, user);
        }
    }
}
