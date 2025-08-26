using System.ComponentModel.DataAnnotations;

namespace Medical_Appoinment_System_API.Model
{
    public class Appointment
    {
        public int Id { get; set; }


        [Required, MaxLength(50)]
        public string AppointmentNo { get; set; } = string.Empty;


        [Required]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }


        [Required]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }


        [Required]
        public string AppointmentDate { get; set; } = string.Empty;


        [Required]
        public string VisitType { get; set; } = string.Empty;


        [MaxLength(1000)]
        public string? Notes { get; set; }


        [MaxLength(1000)]
        public string? Diagnosis { get; set; }

        public string isAppointmentVIsited { get; set; } = "0";
    }
}
