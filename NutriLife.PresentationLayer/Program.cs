
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nutrilife.DataAccessLayer.Data;
using Nutrilife.DataAccessLayer.Models;
using Nutrilife.DataAccessLayer.Repository;
using Nutrilife.DataAccessLayer.utilities;
using Nutrilife.LogicLayer.Mapping;
using Nutrilife.LogicLayer.Service;
using System;
using System.Text;
using System.Text.Json.Serialization;
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
                options.UseSqlServer("Server=db44885.public.databaseasp.net;Database=db44885;User Id=db44885;Password=8Wj+Td@93Rf?;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");
            });

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";



            //which front end can use my apis?
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                  });
            });




            //objects ................
            // builder.Services.AddScoped< interface, class   >(); ... for interfaces and creating objects 

            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<INutritionistRepository, NutritionistRepository>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<ISeedData, RoleSeedData>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddHostedService<SubscriptionExpiryService>();



            MappsterConfig.MappsterConfigRegister();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.User.RequireUniqueEmail = true;

                Options.Password.RequireDigit = true;
                Options.Password.RequireLowercase = true;
                Options.Password.RequireUppercase = true;
                Options.Password.RequiredLength = 8;

                Options.Lockout.MaxFailedAccessAttempts = 4; // 4 محاولات خطأ ينعمل حظر لليوزر مدة معينة
                Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(4); // مدة الحظر
            })
               .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            // ................

            // with enums, to  show as string..
            builder.Services.AddControllers()
         .AddJsonOptions(options =>
         {
             options.JsonSerializerOptions.Converters
                 .Add(new JsonStringEnumConverter()); // ← allows "Electronic" instead of 0
         });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                        };
                    });


            var app = builder.Build();


            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = contextFeature.Error.Message,
                            inner = contextFeature.Error.InnerException?.Message, // ← add this
                            stackTrace = contextFeature.Error.StackTrace // ← add this
                        });
                    }
                });
            });

            app.UseCors(MyAllowSpecificOrigins);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();   
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // to render ..
            var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


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
