using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerkingRezerWPFDM.Models
{
    public class ParkingSpot
    {
        [Key]
        public int Id { get; set; }  // Klucz główny

        [Required]
        public string SpotNumber { get; set; }  // Poprawiona nazwa zamiast SpotId

        [Required]
        public bool IsOccupied { get; set; }  // Poprawiona nazwa zamiast IsAvailable

        public int? VehicleId { get; set; }  // ID pojazdu, jeśli miejsce jest zajęte
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }  // Powiązany pojazd (może być null)
        public bool IsSelected { get; set; } // True = wybrane przez użytkownika
    }
}
