using Microsoft.Extensions.DependencyInjection;
using System;
using FluentMigrator.Runner;

namespace Micro.Services.Tenants.Database
{
    public interface IDatabaseMigrator
    {
        void ReCreate();
        void MigrateUp();
        void MigrateDown();
    }

    public class DatabaseMigrator : IDatabaseMigrator
    {
        private readonly string _connection;

        public DatabaseMigrator(string connection)
        {
            _connection = connection;
        }

        public void ReCreate()
        {
            MigrateDown();
            MigrateUp();
        }

        public void MigrateUp()
        {
            Migrate(x => { x.MigrateUp(); });
        }

        public void MigrateDown()
        {
            Migrate(x => { x.MigrateDown(0); });
        }

        public void Migrate(Action<IMigrationRunner> action)
        {
            var serviceProvider = CreateServices();
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                System.Diagnostics.Trace.WriteLine($"Migrating {action.GetType().Name}");
                action(runner);
            }
        }

        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(_connection)
                    .ScanIn(typeof(Migration1_Schema).Assembly).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}
