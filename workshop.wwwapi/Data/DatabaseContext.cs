using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using workshop.wwwapi.Models;

namespace workshop.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Appointment Key etc.. Add Here
            modelBuilder.Entity<Appointment>().HasKey(a => new { a.PatientId, a.DoctorId });


            //TODO: Seed Data Here
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, FullName = "Axel" },
                new Patient { Id = 2, FullName = "Rob" },
                new Patient { Id = 3, FullName = "Nick" }
    );
            modelBuilder.Entity<Doctor>().HasData(
               new Doctor { Id = 1, FullName = "Kris"},
               new Doctor { Id = 2, FullName = "Coke"},
               new Doctor { Id = 3, FullName = "Pope"});

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { Booking = DateTime.UtcNow, DoctorId = 1, PatientId = 2 },
                new Appointment { Booking = DateTime.UtcNow, DoctorId = 1, PatientId = 3 },
                new Appointment { Booking = DateTime.UtcNow, DoctorId = 2, PatientId = 2 },
                new Appointment { Booking = DateTime.UtcNow, DoctorId = 2, PatientId = 3 },
                new Appointment { Booking = DateTime.UtcNow, DoctorId = 3, PatientId = 1 },
                new Appointment { Booking = DateTime.UtcNow, DoctorId = 3, PatientId = 2 }

                );

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message)); //see the sql EF using in the console
            
        }


        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
