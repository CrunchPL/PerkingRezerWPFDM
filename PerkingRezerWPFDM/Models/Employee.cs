using System.ComponentModel.DataAnnotations;

namespace PerkingRezerWPFDM.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string RegistrationNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Department { get; set; }
    }
}
