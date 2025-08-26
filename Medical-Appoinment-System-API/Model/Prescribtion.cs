using System.ComponentModel.DataAnnotations;

namespace Medical_Appoinment_System_API.Model
{
    public class Prescription
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }

        [Required, MaxLength(100)]
        public string Dogaes { get; set; } = string.Empty;

        [Required]
        public string StartDate { get; set; } = string.Empty;

        [Required]
        public string EndDate { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Notes { get; set; } = string.Empty;

        [Required]
        public string AppointmentNo { get; set; } = string.Empty;
    }
}
