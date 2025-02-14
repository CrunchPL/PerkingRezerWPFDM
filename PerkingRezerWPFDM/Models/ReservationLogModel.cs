using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerkingRezerWPFDM.Models
{
    public class ReservationLogModel
    {
        public int ReservationId { get; set; }
        public string Username { get; set; }
        public string ParkingSpot { get; set; }
        public string ReservationDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Vehicle { get; set; }
    }
}

