using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
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

        public AppointmentService(IAppointmentRepository appointmentRepository,
            ISubscriptionRepository subscriptionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentRepository = appointmentRepository;
            _httpContextAccessor = httpContextAccessor;
            _SubscriptionRepository = subscriptionRepository;
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
            if(request.appointment_date <= DateTime.UtcNow)
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
                message = "Your Appointment Request Send Successfully.."
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


    }
}
