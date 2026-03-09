
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nutrilife.DataAccessLayer.Data;
using Nutrilife.DataAccessLayer.Models;
using Nutrilife.DataAccessLayer.Repository;
using Nutrilife.DataAccessLayer.utilities;
using Nutrilife.LogicLayer.Service;
using System.Threading.Tasks;




namespace NutriLife.PresentationLayer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });


            //objects ................
            // builder.Services.AddScoped< interface, class   >(); ... for interfaces and creating objects 

            builder.Services.AddScoped<IClientService, ClientSrevice>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ISeedData, RoleSeedData>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();



            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            // ................

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();   
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeder = services.GetServices<ISeedData>(); 
                // نعمل هاي الخطوة لما بدنا نعمل اوبجكت من كلاس لمرة واحدة عشان يشتغل, وخذا الاوبجكت ما رح نستخدمه في الكود
                
                foreach(var seed in seeder)
                {
                   await seed.DataSeed(); // لانه في اكثر من رول فنعمل اكثرمن اوبجكت
                }
            
            }

            app.Run();
        }
    }
}
