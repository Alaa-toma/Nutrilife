using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nutrilife.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class app_sub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Subscriptions_SubscriptionId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SubscriptionId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SubscriptioId",
                table: "Appointments",
                column: "SubscriptioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Subscriptions_SubscriptioId",
                table: "Appointments",
                column: "SubscriptioId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Subscriptions_SubscriptioId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SubscriptioId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SubscriptionId",
                table: "Appointments",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Subscriptions_SubscriptionId",
                table: "Appointments",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
