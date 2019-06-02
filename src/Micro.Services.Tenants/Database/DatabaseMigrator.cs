using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<DatabaseMigrator> _log;
        private readonly IMigrationRunner _runner;

        public DatabaseMigrator(ILogger<DatabaseMigrator> log , IMigrationRunner runner)
        {
            _log = log;
            _runner = runner;
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
            _log.LogInformation($"Migrating {action.GetType().Name}");
            action(_runner);
        }
    }
}
