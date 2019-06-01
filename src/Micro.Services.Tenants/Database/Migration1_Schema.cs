using FluentMigrator;

namespace Micro.Services.Tenants.Database
{
    [Migration(1, "Schema")]
    public class Migration1_Schema : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript($"{GetType().Namespace}.Scripts.{GetType().Name}_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript($"{GetType().Namespace}.Scripts.{GetType().Name}_Down.sql");
        }
    }
}
