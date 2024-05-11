using FluentMigrator;

namespace RtfmAPI.Migrator.Migrations;

[Migration(1, "Create First Table.")]
public class M000001_CreateFirstTable : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table("users")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("login").AsString(255).Nullable()
            .WithColumn("password_hash").AsString(255).Nullable()
            .WithColumn("email").AsString(255).Nullable()
            .WithColumn("registration_date").AsDateTime().Nullable()
            .WithColumn("authorization_date").AsDateTime().Nullable()
            .WithColumn("vk_user_id").AsGuid().Nullable();
    }
}