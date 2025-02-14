using System.Configuration;
using System.Data;
using System.Windows;
using PerkingRezerWPFDM.Database;
using Microsoft.EntityFrameworkCore;

namespace PerkingRezerWPFDM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new ParkingDbContext())
            {
                db.Database.Migrate();
            }

                LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }

}
