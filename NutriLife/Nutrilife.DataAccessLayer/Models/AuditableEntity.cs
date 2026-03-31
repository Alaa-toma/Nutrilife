using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Models
{
    public class AuditableEntity
    {
        public string? createdById { get; set; }
        public string? updatedById { get; set; }

        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }

        public ApplicationUser createdBy { get; set; }
        public ApplicationUser updatedBy { get; set; }
    }
}
