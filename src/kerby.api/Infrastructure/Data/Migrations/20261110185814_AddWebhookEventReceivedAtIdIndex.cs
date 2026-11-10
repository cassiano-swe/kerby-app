using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kerby.api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWebhookEventReceivedAtIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WebhookEvents_ReceivedAt_Id",
                table: "WebhookEvents",
                columns: new[] { "ReceivedAt", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebhookEvents_ReceivedAt_Id",
                table: "WebhookEvents");
        }
    }
}
