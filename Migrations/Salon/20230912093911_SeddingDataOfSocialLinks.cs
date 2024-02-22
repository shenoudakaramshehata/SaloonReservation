using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaloonReservation.Migrations.Salon
{
    /// <inheritdoc />
    public partial class SeddingDataOfSocialLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SoicialMidiaLinks",
                columns: new[] { "SoicialMidiaLinkId", "Facebook", "Instgram", "LinkedIn", "Twitter", "WhatsApp", "Youtube" },
                values: new object[] { 1, "https://www.facebook.com/", "https://www.instagram.com/", "https://www.linkedin.com/", "https://twitter.com/", "https://web.whatsapp.com/", "https://youtube.com/" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SoicialMidiaLinks",
                keyColumn: "SoicialMidiaLinkId",
                keyValue: 1);
        }
    }
}
