﻿using Microsoft.EntityFrameworkCore;
using workshop.wwwapi.Data;
using workshop.wwwapi.Models;

namespace workshop.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DatabaseContext _databaseContext;
        public Repository(DatabaseContext db)
        {
            _databaseContext = db;
        }
        public async Task<IEnumerable<Patient>> GetPatients()
        {
            List<Patient> patients = await _databaseContext.Patients.ToListAsync();
            return patients;
        }

        public async Task<Patient>GetPatientById(int id)
        {
            var patient = await _databaseContext.Patients.FirstOrDefaultAsync(x => x.Id == id);
            return patient;
        }
        public async Task<IEnumerable<Doctor>> GetDoctors()
        {
            return await _databaseContext.Doctors.ToListAsync();
        }
        public async Task<Doctor> GetDoctorById(int id)
        {
            var doctor = await _databaseContext.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            return doctor;
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctor(int id)
        {
            return await _databaseContext.Appointments.Where(a => a.DoctorId==id).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointments()
        {
            return await _databaseContext.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            return await _databaseContext.Appointments.FirstOrDefaultAsync(x => x.Appointment_Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentByDoctorId(int id)
        {
            return await _databaseContext.Appointments.Where(x => x.DoctorId == id).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentByPatientId(int id)
        {
            return await _databaseContext.Appointments.Where(x => x.PatientId == id).ToListAsync();
        }
    }
}
