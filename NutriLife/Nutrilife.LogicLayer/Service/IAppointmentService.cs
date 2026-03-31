using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public interface IAppointmentService
    {
        Task<AppointmentResponse> CreateAppointmentAsync(AppointmentRequest request);
        Task<List<Appointment>> GetClientAppointmentsAsync(string ClientId);
        Task<List<Appointment>> GetNutritionistAppointmentsAsync(string NutriId);
         Task<AppointmentResponse> RejectAppointmentAsync(int appointmentId);
         Task<AppointmentResponse> ApproveAppointmentAsync(int appointmentId);

    }
}
