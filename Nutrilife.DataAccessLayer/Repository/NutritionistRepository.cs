using Nutrilife.DataAccessLayer.Data;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Repository
{
    public class NutritionistRepository :GenericRepository<Nutritioist>, INutritionistRepository
    {
        public NutritionistRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
