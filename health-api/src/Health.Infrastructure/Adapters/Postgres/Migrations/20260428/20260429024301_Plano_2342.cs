using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health.Infrastructure.Adapters.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Plano_2342 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "hth_beneficiarios");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "hth_beneficiarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "hth_beneficiarios");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "hth_beneficiarios",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
