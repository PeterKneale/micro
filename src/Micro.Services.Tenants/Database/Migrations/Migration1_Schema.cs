using FluentMigrator;
using System;

namespace Micro.Services.Tenants.Database.Migrations
{
    [Migration(1, "Schema")]
    public class Migration1_Schema : Migration
    {
        public override void Up()
        {
            Execute.Script($"{AppDomain.CurrentDomain.BaseDirectory}\\Database\\Scripts\\Migration1_Up.sql");
        }

        public override void Down()
        {
            Execute.Script($"{AppDomain.CurrentDomain.BaseDirectory}\\Database\\Scripts\\Migration1_Down.sql");
        }
    }
}
