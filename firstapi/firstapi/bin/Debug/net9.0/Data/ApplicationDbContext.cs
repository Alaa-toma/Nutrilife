using firstapi.Models;
using Microsoft.EntityFrameworkCore;

namespace firstapi.Data
{
    public class ApplicationDbContext: DbContext 
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(
                "Server=.;Database=Aap13;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;"
            );
        }
    }
}
