using System.Drawing;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Models;
using workshop.wwwapi.Repository;

namespace workshop.wwwapi.Endpoints
{
    public static class SurgeryEndpoint
    {
        //TODO:  add additional endpoints in here according to the requirements in the README.md 
        public static void ConfigurePatientEndpoint(this WebApplication app)
        {
            var surgeryGroup = app.MapGroup("surgery");

            surgeryGroup.MapGet("/patients", GetPatients);
            surgeryGroup.MapGet("/patients/{id}", GetPatientById);
            surgeryGroup.MapGet("/doctors", GetDoctors);
            surgeryGroup.MapGet("/appointmentsbydoctor/{id}", GetAppointmentsByDoctor);
            surgeryGroup.MapGet("doctors/{id}", GetDoctorById);
            surgeryGroup.MapGet("/appointments", GetAppointments);
            surgeryGroup.MapGet("/appointments/{id}", GetAppointmentById);
            surgeryGroup.MapGet("/appointment/{doctorId}", GetAppointmentsByDoctorId);
            surgeryGroup.MapGet("/appointmen/{patientId}", GetAppointmentsByPatientId);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetPatients(IRepository repository)
        {
            List<PatientDTO> dtos = new List<PatientDTO>();

            var patients = await repository.GetPatients();

            foreach (Patient patient in patients)
            {

                PatientDTO dto = new PatientDTO
                {
                    Id = patient.Id,
                    FullName = patient.FullName,
                    Appointments = new List<AppointmentDTOwithName>()
                };
                IEnumerable<Appointment> appointments = await repository.GetAppointmentByPatientId(patient.Id);

                foreach (Appointment appointment in appointments)
                {
                    AppointmentDTOwithName dtoName = new AppointmentDTOwithName();
                    Doctor doc = await repository.GetDoctorById(appointment.DoctorId);
                    dtoName.DoctorName = doc.FullName;
                    dtoName.DoctorId = appointment.DoctorId;
                    dtoName.Booking = appointment.Booking;

                    dto.Appointments.Add(dtoName);
                }

                dtos.Add(dto);
            }

            return TypedResults.Ok(dtos);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetPatientById(IRepository repository, int id)
        {
            var patients = await repository.GetPatientById(id);

            PatientDTO dto = new PatientDTO()
            {
                Id = patients.Id,
                FullName = patients.FullName,
                Appointments = new List<AppointmentDTOwithName>()
            };

            IEnumerable<Appointment> appointments = await repository.GetAppointmentByPatientId(patients.Id);
            
            foreach(Appointment appointment in appointments)
            {
                AppointmentDTOwithName dtoName = new AppointmentDTOwithName();
                Doctor doc = await repository.GetDoctorById(appointment.DoctorId);
                dtoName.DoctorName = doc.FullName;
                dtoName.DoctorId = appointment.DoctorId;
                dtoName.Booking = appointment.Booking;

                dto.Appointments.Add(dtoName);
            }
                

            return TypedResults.Ok(dto);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetDoctors(IRepository repository)
        {
            List<DoctorDTO> dtos = new List<DoctorDTO>();

            var doctors = await repository.GetDoctors();

            foreach (Doctor doctor in doctors)
            {

                DoctorDTO dto = new DoctorDTO
                {
                    Id = doctor.Id,
                    FullName = doctor.FullName,
                    Appointments = new List<AppointmentDTODoctor>()
                };

                IEnumerable<Appointment> appointments = await repository.GetAppointmentByDoctorId(doctor.Id);

                foreach (Appointment appointment in appointments)
                {
                    AppointmentDTODoctor dtoName = new AppointmentDTODoctor();
                    Patient patient = await repository.GetPatientById(appointment.PatientId);
                    dtoName.Name = patient.FullName;
                    dtoName.PatientId = appointment.PatientId;
                    dtoName.Booking = appointment.Booking;

                    dto.Appointments.Add(dtoName);
                }
                dtos.Add(dto);
            }

            return TypedResults.Ok(dtos);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetDoctorById(IRepository repository, int id)
        {
            var doctors = await repository.GetDoctorById(id);

            DoctorDTO dto = new DoctorDTO()
            {
                Id = doctors.Id,
                FullName = doctors.FullName,
                Appointments = new List<AppointmentDTODoctor>()
            };

            IEnumerable<Appointment> appointments = await repository.GetAppointmentByDoctorId(doctors.Id);

            foreach (Appointment appointment in appointments)
            {
                AppointmentDTODoctor dtoName = new AppointmentDTODoctor();
                Patient patient = await repository.GetPatientById(appointment.PatientId);
                dtoName.Name = patient.FullName;
                dtoName.PatientId = appointment.PatientId;
                dtoName.Booking = appointment.Booking;

                dto.Appointments.Add(dtoName);
            }

            return TypedResults.Ok(dto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        
        public static async Task<IResult> GetAppointmentsByDoctor(IRepository repository, int id)
        {
            return TypedResults.Ok(await repository.GetAppointmentsByDoctor(id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAppointments(IRepository repository)
        {
            List<AppointmentDTO> dtos = new List<AppointmentDTO>();

            var appointments = await repository.GetAppointments();

            foreach (Appointment appointment in appointments)
            {
                Doctor doc = await repository.GetDoctorById(appointment.DoctorId);
                Patient pat = await repository.GetPatientById(appointment.PatientId);

                AppointmentDTO dto = new AppointmentDTO()
                {
                    Id = appointment.Appointment_Id,
                    Booking = appointment.Booking,
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId,
                    
                    DoctorName = doc.FullName,
                    PatientName = pat.FullName
                };

                dtos.Add(dto);
            }

            return TypedResults.Ok(dtos);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAppointmentById(IRepository repository, int id)
        {
            var appointment = await repository.GetAppointmentById(id);
            Doctor doc = await repository.GetDoctorById(appointment.DoctorId);
            Patient pat = await repository.GetPatientById(appointment.PatientId);
           
            AppointmentDTO dto = new AppointmentDTO()
            {
                Id = appointment.Appointment_Id,
                Booking = appointment.Booking,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                DoctorName = doc.FullName,
                PatientName = pat.FullName
            };

            return TypedResults.Ok(dto);
        }
        

        public static async Task<IResult> GetAppointmentsByDoctorId(IRepository repository, int doctor_id)
        {
            IEnumerable<Appointment> appointments = await repository.GetAppointmentByDoctorId(doctor_id);
            List<AppointmentDTO> appointmentsDTO = new List<AppointmentDTO>();
            
            
            foreach (Appointment appointment in appointments)
            {
                Doctor doc = await repository.GetDoctorById(appointment.DoctorId);
                Patient pat = await repository.GetPatientById(appointment.PatientId);
                AppointmentDTO dto = new AppointmentDTO()
                {
                    Id = appointment.Appointment_Id,
                    Booking = appointment.Booking,
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId,
                    DoctorName = doc.FullName,
                    PatientName = pat.FullName

                };
                appointmentsDTO.Add(dto);
            }

            return TypedResults.Ok(appointmentsDTO);
        }

        public static async Task<IResult> GetAppointmentsByPatientId(IRepository repository, int patient_id)
        {
            IEnumerable<Appointment> appointments = await repository.GetAppointmentByPatientId(patient_id);
            List<AppointmentDTO> appointmentsDTO = new List<AppointmentDTO>();
            foreach (Appointment appointment in appointments)
            {
                Doctor doc = await repository.GetDoctorById(appointment.DoctorId);
                Patient pat = await repository.GetPatientById(appointment.PatientId);
                AppointmentDTO dto = new AppointmentDTO()
                {
                    Id = appointment.Appointment_Id,
                    Booking = appointment.Booking,
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId,
                    DoctorName = doc.FullName,
                    PatientName = pat.FullName
                };
                appointmentsDTO.Add(dto);
            }

            return TypedResults.Ok(appointmentsDTO);
        }


    }
}
