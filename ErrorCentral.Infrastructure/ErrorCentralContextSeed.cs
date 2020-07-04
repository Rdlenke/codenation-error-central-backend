using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErrorCentral.Infrastructure
{
    public class ErrorCentralContextSeed
    {
        public static async Task SeedAsync(ErrorCentralContext context, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                // context.Database.Migrate();
                if (!await context.Users.AnyAsync())
                {
                    await context.Users.AddRangeAsync(
                        GetPreconfiguredUsers());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    await SeedAsync(context, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<User> GetPreconfiguredUsers()
        {
            return new List<User>()
            {
            };
        }
    }
}
