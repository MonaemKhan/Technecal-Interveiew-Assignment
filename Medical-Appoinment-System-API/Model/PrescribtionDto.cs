using System.ComponentModel.DataAnnotations;

namespace Medical_Appoinment_System_API.Model
{
    public class PrescriptionDto
    {
        public int Id { get; set; }

        public int MedicineId { get; set; }

        public string Dosage { get; set; } = string.Empty;

        public string StartDate { get; set; } = string.Empty;

        public string EndDate { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;


        public string AppointmentNo { get; set; } = string.Empty;
    }
}
