using System.ComponentModel.DataAnnotations;

namespace IncidentAPI_imen.Models
{
    public class Incident
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength =3, ErrorMessage ="erreur")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        /*[EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;*/

        [Required]
        [RegularExpression("LOW|MEDIUM|HIGH|CRITICAL", ErrorMessage = "Invalid severity")]

        public string Severity { get; set; } = string.Empty;
        

        public string Status { get; set; } = string.Empty;
        [Required]

        public DateTime CreatedAt { get; set; }


    }
}
