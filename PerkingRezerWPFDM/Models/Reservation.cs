using PerkingRezerWPFDM.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Reservation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ParkingSpotId { get; set; }

    [Required]
    public DateTime ReservationDate { get; set; }

    public int GetDuration()
    {
        return (EndHour - StartHour) * 60;  // Minuty
    }

    [Required]
    public int VehicleId { get; set; }
    [Required]
    public int StartHour { get; set; }

    public int StartMinute { get; set; }
    [Required]
    public int EndHour { get; set; }
    public int EndMinute { get; set; }
    [ForeignKey("VehicleId")]
    public Vehicle Vehicle { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

}
