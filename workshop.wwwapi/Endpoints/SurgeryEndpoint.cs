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
                    FullName = patient.FullName 
                };

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
                FullName = patients.FullName
            };

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
                    FullName = doctor.FullName
                };

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
                FullName = doctors.FullName
            };

            return TypedResults.Ok(dto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAppointmentsByDoctor(IRepository repository, int id)
        {
            return TypedResults.Ok(await repository.GetAppointmentsByDoctor(id));
        }
    }
}
