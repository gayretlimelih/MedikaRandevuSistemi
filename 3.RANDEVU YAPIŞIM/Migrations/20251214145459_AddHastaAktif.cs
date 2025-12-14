using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3.RANDEVU_YAPIŞIM.Migrations
{
    /// <inheritdoc />
    public partial class AddHastaAktif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktif",
                table: "Hastalar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktif",
                table: "Hastalar");
        }
    }
}
