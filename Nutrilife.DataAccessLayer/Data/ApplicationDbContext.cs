using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessror;

        public DbSet<Client> Clients { get; set; }
        public DbSet<Nutritionist> Nutritionists { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Appointment> Appointments { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessror)
       : base(options)
        {
            _httpContextAccessror = httpContextAccessror;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users")
             .HasDiscriminator<string>("UserType")
             .HasValue<Client>("Client")
             .HasValue<Nutritionist>("Nutritionist");

            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            


            // *********   subscription **************

            // Fix decimal precision warnings
            builder.Entity<Nutritionist>()
                .Property(n => n.ConsultationFee)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Subscription>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");



            // Fluent API, “A Subscription belongs to one Client, and a Client can have many Subscriptions.” 
            // control (and prevent) cascade delete behavior.
            builder.Entity<Subscription>()
             .HasOne(s => s.Client)
             .WithMany(c => c.Subscriptions)
             .HasForeignKey(s => s.ClientId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Subscription>()
                .HasOne(s => s.Nutritionist)
                .WithMany(n => n.Subscriptions)
                .HasForeignKey(s => s.NutritionistId)
                .OnDelete(DeleteBehavior.Restrict);

            // *********   subscription **************
        }



        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessror.HttpContext != null)
            {

                var currentUserId = _httpContextAccessror.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var entries = ChangeTracker.Entries<AuditableEntity>();
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(x => x.createdById).CurrentValue = currentUserId;
                        entry.Property(x => x.createdOn).CurrentValue = DateTime.UtcNow;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property(x => x.updatedById).CurrentValue = currentUserId;
                        entry.Property(x => x.updatedOn).CurrentValue = DateTime.UtcNow;
                    }
                }

            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}
