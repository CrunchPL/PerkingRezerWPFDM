namespace PerkingRezerWPFDM.Models
{
    public class ReservationDisplayModel
    {
        public int ReservationId { get; set; }
        public string ParkingSpot { get; set; }
        public string LicensePlate { get; set; }
        public string ReservationDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TotalDuration { get; set; }
    }
}
