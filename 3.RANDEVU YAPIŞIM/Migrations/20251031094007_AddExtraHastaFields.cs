using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3.RANDEVU_YAPIŞIM.Migrations
{
    /// <inheritdoc />
    public partial class AddExtraHastaFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Hastalar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DogumTarihi",
                table: "Hastalar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TcKimlik",
                table: "Hastalar",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Hastalar");

            migrationBuilder.DropColumn(
                name: "DogumTarihi",
                table: "Hastalar");

            migrationBuilder.DropColumn(
                name: "TcKimlik",
                table: "Hastalar");
        }
    }
}
