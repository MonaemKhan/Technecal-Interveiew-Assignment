using System.ComponentModel.DataAnnotations;

namespace Medical_Appoinment_System_API.Model
{
    public class AppointmentDto
    {
        [Required]
        public string AppointmentNo { get; set; } = string.Empty;
        [Required]
        public int PatientId { get; set; }


        [Required]
        public int DoctorId { get; set; }


        [Required]
        public string AppointmentDate { get; set; } = string.Empty;


        [Required]
        public string VisitType { get; set; } = string.Empty;


        public string? Notes { get; set; }
        public string? Diagnosis { get; set; }
    }
}
