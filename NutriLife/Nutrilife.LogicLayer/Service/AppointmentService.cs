
using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using Nutrilife.DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ISubscriptionRepository _SubscriptionRepository;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser>  _UserManager;
        private readonly IMeetingLinkCreation _meetingLinkCreation;

        public AppointmentService(IAppointmentRepository appointmentRepository,
            ISubscriptionRepository subscriptionRepository, IHttpContextAccessor httpContextAccessor, 
            IEmailSender emailSender, UserManager<ApplicationUser> UserManager, 
            IMeetingLinkCreation meetingLinkCreation)
        {
            _appointmentRepository = appointmentRepository;
            _httpContextAccessor = httpContextAccessor;
            _SubscriptionRepository = subscriptionRepository;
            _emailSender = emailSender;
            _UserManager = UserManager;
            _meetingLinkCreation = meetingLinkCreation;
        }


        public async Task<AppointmentResponse> CreateAppointmentAsync(AppointmentRequest request)
        {
            var subscription = await _SubscriptionRepository.GetByIdAsync(request.SubscriptioId);

            if (subscription == null)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = "You need to subscribe with the Nutritionist before booking an appointment."
                };
            } // client has subscription
            if (request.appointment_date <= DateTime.UtcNow)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = "You book Appointment in The Past! Check Your Appointment Time"
                };
            } // time not in the past

            var UserId = GetCurrentUserId();
            var hasConflict = await _appointmentRepository.ISConflict(subscription, UserId, request.appointment_date);
            if (hasConflict)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = "Conflict Appointment! you Already sent an appointment request for this day.."
                };
            }

            // adapt to appointment object
            var appointment = request.Adapt<Appointment>();
            appointment.Subscription = subscription;
            appointment.status = AppointmentStatus.Pending;

            // create and save changes
            var created = await _appointmentRepository.CreateAsync(appointment);


            return new AppointmentResponse()
            {
                Confirmd = true,
                message = "Your Appointment Request Send Successfully..",
                AppointmentId = created.Id
            };

        }

        public async Task<List<Appointment>> GetClientAppointmentsAsync(string ClientId)
        {
            return await _appointmentRepository.GetClientAppointments(ClientId);
        }

        public async Task<List<Appointment>> GetNutritionistAppointmentsAsync(string NutriId)
        {
            return await _appointmentRepository.GetNutritionistAppointments(NutriId);
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<AppointmentResponse> RejectAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = "Appointment Not Found!"
                };
            }

            var hasSubscription = await _SubscriptionRepository.GetByIdAsync(appointment.SubscriptioId);
            if (hasSubscription == null)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = " You Should Make a Subscription First!"
                };
            }

            if (hasSubscription.NutritionistId != GetCurrentUserId())
                throw new UnauthorizedAccessException("You cannot reject this subscription");

            if (appointment.status != AppointmentStatus.Pending)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = "Only pending Appointments can be rejected"
                };
            }

            appointment.status = AppointmentStatus.Cancelled;
            var updated = await _appointmentRepository.UpdateAsync(appointment);

            return new AppointmentResponse()
            {
                Confirmd = true,
                message = "Appointment Is Rejected Succfully..",
                AppointmentId = appointmentId
            };
        }

        public async Task<AppointmentResponse> ApproveAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = "Appointment Not Found!"
                };
            }

            var hasSubscription = await _SubscriptionRepository.GetByIdAsync(appointment.SubscriptioId);
            if (hasSubscription == null)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = " You Should Make a Subscription First!"
                };
            }

            if (hasSubscription.NutritionistId != GetCurrentUserId())
                throw new UnauthorizedAccessException("You cannot Approve this subscription");

            if (appointment.status != AppointmentStatus.Pending)
            {
                return new AppointmentResponse()
                {
                    Confirmd = false,
                    message = "Only pending Appointments can be rejected"
                };
            }

            appointment.status = AppointmentStatus.Confirmed;


            var client = await _UserManager.FindByIdAsync(hasSubscription.ClientId);

            string meetLink = "";
            if (appointment.type == AppointmentType.online)
            {
                 meetLink = await _meetingLinkCreation.CreateMeetingLinkAsync(
               title: $"Nutrition Consultation - {client.UserName}",
               start: appointment.appointment_date,
               end: appointment.appointment_date.AddMinutes(45)
           );

                await _emailSender.SendEmailAsync(
                    client.Email!,
                    "Your Appointment is Scheduled 📅",
                    $@"
                <h2>Hello {client.UserName},</h2>
                <p>Your nutrition consultation has been scheduled.</p>
                <p><strong>Date:</strong> day-{appointment.appointment_date.Day} : month-{appointment.appointment_date.Month}  year-{appointment.appointment_date.Year} </p>
                <p><strong>Time:</strong> {appointment.appointment_date.TimeOfDay} </p>
                <br/>
                <a href='{meetLink}'
                   style='background:#1a73e8; color:white; padding:12px 24px;
                          text-decoration:none; border-radius:6px; font-size:16px;'>
                   Join Google Meet 🎥
                </a>
                <br/><br/>
                <p>Or copy this link: <a href='{meetLink}'>{meetLink}</a></p>
                "
                );
            }
            else
            {
                var nutri = await _UserManager.FindByIdAsync(hasSubscription.NutritionistId);
                await _emailSender.SendEmailAsync(
                    client.Email!,
                    "Your Appointment is Scheduled 📅",
                    $@"
                <h2>Hello {client.UserName},</h2>
                <p>Your nutrition consultation has been scheduled.</p>
                <p><strong>Date:</strong> day-{appointment.appointment_date.Day} : month-{appointment.appointment_date.Month}  year-{appointment.appointment_date.Year} </p>
                <p><strong>Time:</strong> {appointment.appointment_date.TimeOfDay} </p>
                <br/>
                 <p><strong>Location:</strong> In {nutri.FullName} clinic  </p>
                "
                );
            }

                var updated = await _appointmentRepository.UpdateAsync(appointment);

            return new AppointmentResponse()
            {
                Confirmd = true,
                message = "Appointment Is Approved Succfully.. ",
                AppointmentId = appointmentId,
                MeetingLink = meetLink
            };
        }




    }
}
