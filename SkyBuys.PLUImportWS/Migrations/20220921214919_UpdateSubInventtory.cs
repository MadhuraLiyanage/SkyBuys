using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyBuys.PLUImportWS.Migrations
{
    public partial class UpdateSubInventtory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubInventory",
                columns: table => new
                {
                    SubInventoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubInventoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubInventory", x => x.SubInventoryCode);
                });

            migrationBuilder.InsertData(
                table: "SubInventory",
                columns: new[] { "SubInventoryCode", "SubInventoryDescription" },
                values: new object[,]
                {
                    { "DCS", "Nadi Cuppabula Convenience Store - Arrival" },
                    { "DFA", "Nadi Tappoo Shop - Arrival" },
                    { "DFD", "Nadi Tappoo Shop - Departure" },
                    { "DFV", "Nadi Tappoo Vodafone Shop - Arrival" },
                    { "DNK", "Nadi Tappoo Nike Shop - Departure" },
                    { "DWS", "Nadi Tappoo W/Smith - Departure" },
                    { "NSD", "Nausori Tappoo Shop - Departure" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubInventory");
        }
    }
}
