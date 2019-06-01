using FluentMigrator;

namespace Micro.Services.Tenants.Database
{
    [Migration(2, "Data")]
    public class Migration2_Data : Migration
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
