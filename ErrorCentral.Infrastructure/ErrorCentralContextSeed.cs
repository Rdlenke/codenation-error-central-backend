using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErrorCentral.Infrastructure
{
    public class ErrorCentralContextSeed
    {
        public static async Task SeedAsync(ErrorCentralContext context, ILoggerFactory loggerFactory, int? retry = 0)
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

                if (!await context.LogErrors.AnyAsync())
                {
                    await context.LogErrors.AddRangeAsync(
                        GetPreconfiguredLogErros());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<ErrorCentralContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<LogError> GetPreconfiguredLogErros()
        {
            return new List<LogError>()
            {
                new LogError(1, "Compilation error", "Compilation error (line 7, col 21): The name 'vars' does not exist in the current context", "http://localhost:8080/", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Compilation error", "Compilation error (line 20, col 21): The name 'vars' does not exist in the current context", "http://homolog.com/", ELevel.Error, EEnvironment.Homologation),
                new LogError(1, "Compilation error", "Compilation error (line 20, col 21): The name 'vars' does not exist in the current context", "http://production.com/", ELevel.Error, EEnvironment.Production),
                new LogError(2, "Run-time exception (line 11): Index was outside the bounds of the array.", "[System.IndexOutOfRangeException: Index was outside the bounds of the array.] \nat Program.Main() :line 11", "http://localhost:8080/", ELevel.Debug, EEnvironment.Development),
                new LogError(2, "Run-time exception (line 11): Index was outside the bounds of the array.", "[System.IndexOutOfRangeException: Index was outside the bounds of the array.] \nat Program.Main() :line 11", "http://homolog.com/", ELevel.Debug, EEnvironment.Homologation),
                new LogError(2, "Run-time exception (line 11): Index was outside the bounds of the array.", "[System.IndexOutOfRangeException: Index was outside the bounds of the array.] \nat Program.Main() :line 11", "http://production.com/", ELevel.Debug, EEnvironment.Production),
                new LogError(3, "Run-time exception (line 8): Attempted to divide by zero.", "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8", "http://localhost:8080/", ELevel.Warning, EEnvironment.Development),
                new LogError(3, "Run-time exception (line 8): Attempted to divide by zero.", "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8", "http://homolog.com/", ELevel.Warning, EEnvironment.Homologation),
                new LogError(3, "Run-time exception (line 8): Attempted to divide by zero.", "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8", "http://production.com/", ELevel.Warning, EEnvironment.Production),
            };
        }

        static IEnumerable<User> GetPreconfiguredUsers()
        {
            return new List<User>()
            {
                new User("João", "Alcantara", "joao@email.com", "123abc"),
                new User("Maria", "Antônia", "maria@email.com", "ma123abc"),
                new User("Sebastião", "Alves", "sebinha@email.com", "sebinha123"),
                new User("Julia", "Magualhoes", "ju@email.com", "julinha@@"),
            };
        }
    }
}
