using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerkingRezerWPFDM.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LicensePlate { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public string VehicleType { get; set; }
        public string FuelType { get; set; }

        public int Year { get; set; }

        [Required]
        public int OwnerId { get; set; }  // Poprawiona nazwa z UserId na OwnerId
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }  // Powiązany użytkownik
    }
}
