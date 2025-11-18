using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3.RANDEVU_YAPIŞIM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHastaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ad",
                table: "Hastalar");

            migrationBuilder.RenameColumn(
                name: "TC",
                table: "Hastalar",
                newName: "Sifre");

            migrationBuilder.RenameColumn(
                name: "Soyad",
                table: "Hastalar",
                newName: "AdSoyad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sifre",
                table: "Hastalar",
                newName: "TC");

            migrationBuilder.RenameColumn(
                name: "AdSoyad",
                table: "Hastalar",
                newName: "Soyad");

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "Hastalar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
