using Microsoft.EntityFrameworkCore;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Client> Clients { get; set; }
        public DbSet<Nutritioist> Nutritioists { get; set; }
        public DbSet<Subscribtion> Subscribtions { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

    }
}
