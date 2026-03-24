using Microsoft.EntityFrameworkCore;
using Nutrilife.DataAccessLayer.Data;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Repository
{
    public class AppointmentRepository :GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       

        public async Task<List<Appointment>> GetClientAppointments(string ClientrId)
        {
            return await _context.Appointments.Where(a => a.Subscription.ClientId == ClientrId).ToListAsync();
        }
        public async Task<List<Appointment>> GetNutritionistAppointments(string NutritionistId)
        {
            return await _context.Appointments.Where(a => a.Subscription.NutritionistId == NutritionistId).ToListAsync();
        }
        public async Task<bool> ISConflict(Subscription subscription, String UserId, DateTime AppointmentDate )
        {
            return await _context.Appointments.AnyAsync(a => ( a.status != AppointmentStatus.Cancelled ) 
            && (a.Subscription.ClientId == UserId)
            && ((a.appointment_date.Date == AppointmentDate.Date) )  ); // if there an conflict return true.
        }




    }
}
