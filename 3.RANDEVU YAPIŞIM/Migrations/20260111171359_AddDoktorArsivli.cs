using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3.RANDEVU_YAPIŞIM.Migrations
{
    /// <inheritdoc />
    public partial class AddDoktorArsivli : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Arsivli",
                table: "Doktorlar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arsivli",
                table: "Doktorlar");
        }
    }
}
