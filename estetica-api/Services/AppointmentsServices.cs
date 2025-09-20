using AutoMapper;
using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dentist_panel_api.Services
{
    public class AppointmentsServices : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailServices _emailServices;
        private readonly UserServices _userServices;
        private readonly IMapper _mapper;
        public readonly MessagesServices messagesServices;

        public AppointmentsServices(ApplicationDbContext context, IMapper mapper, EmailServices emailServices, UserServices userServices, MessagesServices messagesServices)
        {
            this._context = context;
            _emailServices = emailServices;
            this._mapper = mapper;
            this._userServices = userServices;
            this.messagesServices = messagesServices;
        }

        public async Task<ActionResult<List<AppointmentDTO>>> Get(AppointmentsFilter filter, ClaimsPrincipal claims)
        {
            ApplicationUser user = await _userServices.GetCurrentUser(claims);
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Owner.Id == user.Id && a.StartDate > filter.StartDate.ToUniversalTime() && a.StartDate < filter.EndDate.ToUniversalTime())
                .ToListAsync();
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<ActionResult> Create(AppointmentCreationDTO dto)
        {
            var patient = await _context.Patients.FindAsync(dto.PatientId);
            if (patient == null) return NotFound();
            
            var appointment = _mapper.Map<Appointment>(dto);
            _context.Add(appointment);
            await _context.SaveChangesAsync();
            
            await _emailServices.SendHtmlEmail(appointment.Patient.Email, 
                appointment.Patient.Name, 
                "Turno agendado", 
                $"Hola {patient.Name}, agendamos un nuevo turno en el consultorio. " +
                $"Te esperamos el día {appointment.StartDate.ToString("dd/MM/yy")} a las {appointment.StartDate.ToString("HH:mm")} hs.");

            return NoContent();
        }
        public async Task<ActionResult> SendDailyReminders()
        {
            var appointmentsOfTheDay = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Owner)
                .Where(a => a.StartDate.Date == DateTime.Now.Date)
                .ToListAsync();

            foreach (var appointment in appointmentsOfTheDay)
            {
                await _emailServices.SendHtmlEmail(appointment.Patient.Email, 
                    appointment.Patient.Name, 
                    "Recordatorio de turno", 
                    $"Hola {appointment.Patient.Name}, te recordamos que " +
                    $"tenés un turno odontológico para el dia de hoy " +
                    $"a las {appointment.StartDate.ToString("HH:mm")} hs.");
                if (appointment.Owner.PlanType==1 && appointment.Owner.CodePhoneId!=null && validPhoneNumber(appointment.Patient.PhoneNumber))
                    messagesServices.SendMessage(appointment);                
            }

            return NoContent();
        }
        
        private Boolean validPhoneNumber(string phoneNumber)        
        {
            return phoneNumber != null && phoneNumber != "";
        }
    }
}
