using System.ComponentModel.DataAnnotations;

namespace Medical_Appoinment_System_API.Model
{
    public class Doctor
    {
        public int Id { get; set; }


        [Required, MaxLength(120)]
        public string Name { get; set; } = string.Empty;
    }
}
