using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class UpdateProfessionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_role",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Professions");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "Professions",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Professions",
                newName: "NameRu");

            migrationBuilder.AddColumn<int>(
                name: "_role",
                table: "Roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Professions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
