using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using PerkingRezerWPFDM.Database;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.ViewModels
{
    public class ReservationLogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<ReservationLogModel> AllReservations { get; set; } = new ObservableCollection<ReservationLogModel>();

        public ReservationLogViewModel()
        {
            LoadAllReservations();
        }

        private void LoadAllReservations()
        {
            using (var db = new ParkingDbContext())
            {
                var reservations = db.Reservations
                    .Select(r => new ReservationLogModel
                    {
                        ReservationId = r.Id,
                        Username = db.Users.Where(u => u.Id == r.UserId).Select(u => u.Username).FirstOrDefault(),
                        ParkingSpot = r.ParkingSpotId,
                        ReservationDate = r.ReservationDate.ToString("dd.MM.yyyy"),
                        StartTime = $"{r.StartHour:D2}:{r.StartMinute:D2}",
                        EndTime = $"{r.EndHour:D2}:{r.EndMinute:D2}",
                        Vehicle = db.Vehicles.Where(v => v.Id == r.VehicleId).Select(v => v.LicensePlate).FirstOrDefault()
                    })
                    .ToList();

                AllReservations.Clear();
                foreach (var res in reservations)
                {
                    AllReservations.Add(res);
                }
            }
        }
    }
}
