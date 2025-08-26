using Medical_Appoinment_System_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appoinment_System_API.DBConnectionContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Prescription> Prescribtions { get; set; }

        public DbSet<Medicine> Medicines { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<Prescription>().ToTable("Prescribtion");
            modelBuilder.Entity<Medicine>().ToTable("Medecine");

            modelBuilder.Entity<Patient>().HasData(
                    new Patient { Id = 1, Name = "Alex" },
                    new Patient { Id = 2, Name = "Bob" },
                    new Patient { Id = 3, Name = "Tom" },
                    new Patient { Id = 4, Name = "Alice" }
                    );
            modelBuilder.Entity<Doctor>().HasData(
                        new Doctor { Id = 1, Name = "Dr. Jhon Don" },
                        new Doctor { Id = 2, Name = "Dr. Kate" },
                        new Doctor { Id = 3, Name = "Dr. Eve" },
                        new Doctor { Id = 4, Name = "Dr. Emma" }
                        );

            modelBuilder.Entity<Medicine>().HasData(
                    new Doctor { Id = 1, Name = "Napa" },
                    new Doctor { Id = 2, Name = "Paracetamal" },
                    new Doctor { Id = 3, Name = "Progut Mups" },
                    new Doctor { Id = 4, Name = "Antazol" }
                    );
        }
    }
}
