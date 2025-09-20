using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dentist_panel_api.Migrations
{
    public partial class AddAbdomenToConsult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Abdomen",
                table: "Consults",
                type: "decimal(65,30)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abdomen",
                table: "Consults");
        }
    }
}
