using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nutrilife.DataAccessLayer.Data;
using Nutrilife.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{

    // يفحص كل 24 ساعة , اذا في اشتراك حالته اكتف بس التاريخ تبعه انتهى فيحول حالته الى فنشد
    public class SubscriptionExpiryService : BackgroundService
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public SubscriptionExpiryService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckExpiredSubscriptions();

                // Run every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckExpiredSubscriptions()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var expiredSubscriptions = await context.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active &&
                            s.EndDate < DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();

            foreach (var subscription in expiredSubscriptions)
            {
                subscription.Status = SubscriptionStatus.Finished;
            }

            if (expiredSubscriptions.Any())
                await context.SaveChangesAsync();
        }


    }
}
