using AutoMapper;
using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using dentist_panel_api.Entities.DTOs;

namespace dentist_panel_api.Utils
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<SignUpDTO, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserDTO>();

            CreateMap<Patient, PatientDTO>().ReverseMap();
            CreateMap<PatientCreationDTO, Patient>().ReverseMap();

            CreateMap<HealthCoverage, HealthCoverageDTO>();
            CreateMap<HealthCoverageCreationDTO, HealthCoverage>();

            CreateMap<Appointment, AppointmentDTO>();
            CreateMap<AppointmentPutDTO, Appointment>();
            CreateMap<AppointmentCreationDTO, Appointment>();

            CreateMap<ConsultCreationDTO, Consult>();
            CreateMap<Consult, ConsultDTO>();

            CreateMap<Doctor, DoctorDTO>();

            CreateMap<Odontogram, OdontogramDTO>();
            CreateMap<OdontogramCreationDTO, Odontogram>();
            CreateMap<OdontogramProsthesis, OdontogramProsthesisDTO>();
            CreateMap<OdontogramProsthesisCreationDTO, OdontogramProsthesis>();

            CreateMap<Decay, DecayDTO>();
            CreateMap<DecayCreationDTO, Decay>();

            CreateMap<TipoDeTratamiento, TipoDeTratamientoDTO>();
            CreateMap<TipoDeTratamientoCreationDTO, TipoDeTratamiento>();
        }
    }
}
