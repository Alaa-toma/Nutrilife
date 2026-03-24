using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.DTO.Response
{
    public class NutritionistResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
        public string Bio { get; set; }
        public decimal? ConsultationFee { get; set; }
    }
}
