using Microsoft.EntityFrameworkCore;
using Nutrilife.DataAccessLayer.Data;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Repository
{
    public class NutritionistRepository : INutritionistRepository
    {
        private readonly ApplicationDbContext _context;

        public NutritionistRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Nutritionist> GetByIdAsync(string nutritionistId)
        {
            return await _context.Users
                .OfType<Nutritionist>()
                .FirstOrDefaultAsync(n => n.Id == nutritionistId);
        }

        public async Task<List<Nutritionist>> GetAllAsync()
        {
            return await _context.Users
                .OfType<Nutritionist>()
                .ToListAsync();
        }
    }
}
