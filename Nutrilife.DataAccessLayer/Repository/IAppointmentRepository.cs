using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Repository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<List<Appointment>> GetClientAppointments(string ClientrId);
        Task<List<Appointment>> GetNutritionistAppointments(string NutritionistId);
        Task<bool> ISConflict(Subscription subscription, String UserId, DateTime AppointmentDate);


        }
}
