using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nutrilife.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class meetingLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingLink",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingLink",
                table: "Appointments");
        }
    }
}
