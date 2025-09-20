using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dentist_panel_api.Migrations
{
    public partial class AddTipoDeTratamientoToConsult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TipoDeTratamientoId",
                table: "Consults",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Consults_TipoDeTratamientoId",
                table: "Consults",
                column: "TipoDeTratamientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consults_TiposDeTratamiento_TipoDeTratamientoId",
                table: "Consults",
                column: "TipoDeTratamientoId",
                principalTable: "TiposDeTratamiento",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consults_TiposDeTratamiento_TipoDeTratamientoId",
                table: "Consults");

            migrationBuilder.DropIndex(
                name: "IX_Consults_TipoDeTratamientoId",
                table: "Consults");

            migrationBuilder.DropColumn(
                name: "TipoDeTratamientoId",
                table: "Consults");
        }
    }
}
