using FluentMigrator;
using System;

namespace Micro.Services.Tenants.Database.Migrations
{
    [Migration(2, "Data")]
    public class Migration2_Schema : Migration
    {
        public override void Up()
        {
            Execute.Script($"{AppDomain.CurrentDomain.BaseDirectory}\\Database\\Scripts\\Migration2_Up.sql");
        }

        public override void Down()
        {
            Execute.Script($"{AppDomain.CurrentDomain.BaseDirectory}\\Database\\Scripts\\Migration2_Down.sql");
        }
    }
}
