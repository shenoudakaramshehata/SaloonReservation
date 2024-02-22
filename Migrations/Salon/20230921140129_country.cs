using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaloonReservation.Migrations.Salon
{
    /// <inheritdoc />
    public partial class country : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CityTLEn",
                table: "Cities",
                newName: "CityTlEn");

            migrationBuilder.RenameColumn(
                name: "CityTLAr",
                table: "Cities",
                newName: "CityTlAr");

            migrationBuilder.RenameColumn(
                name: "AreaTLEn",
                table: "Areas",
                newName: "AreaTlEn");

            migrationBuilder.RenameColumn(
                name: "AreaTLAr",
                table: "Areas",
                newName: "AreaTlAr");

            migrationBuilder.AlterColumn<string>(
                name: "CityTlEn",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CityTlAr",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "CityIsActive",
                table: "Cities",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityOrderIndex",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "AreaTlEn",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "AreaTlAr",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AreaIsActive",
                table: "Areas",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AreaOrderIndex",
                table: "Areas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryTlAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryTlEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryIsActive = table.Column<bool>(type: "bit", nullable: true),
                    CountryOrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CountryId",
                table: "Customers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Countries_CountryId",
                table: "Customers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Countries_CountryId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CountryId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityIsActive",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityOrderIndex",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "AreaIsActive",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "AreaOrderIndex",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "CityTlEn",
                table: "Cities",
                newName: "CityTLEn");

            migrationBuilder.RenameColumn(
                name: "CityTlAr",
                table: "Cities",
                newName: "CityTLAr");

            migrationBuilder.RenameColumn(
                name: "AreaTlEn",
                table: "Areas",
                newName: "AreaTLEn");

            migrationBuilder.RenameColumn(
                name: "AreaTlAr",
                table: "Areas",
                newName: "AreaTLAr");

            migrationBuilder.AlterColumn<string>(
                name: "CityTLEn",
                table: "Cities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CityTLAr",
                table: "Cities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AreaTLEn",
                table: "Areas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AreaTLAr",
                table: "Areas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
