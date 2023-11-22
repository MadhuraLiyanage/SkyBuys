using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyBuys.PLUImportWS.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "itemDefinitions",
                columns: table => new
                {
                    RecId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemSize = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemDefinitions", x => x.RecId);
                });

            migrationBuilder.CreateTable(
                name: "Soh",
                columns: table => new
                {
                    RecId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubinventoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PrimaryQuantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Soh", x => x.RecId);
                });

            migrationBuilder.InsertData(
                table: "Soh",
                columns: new[] { "RecId", "ItemNumber", "OrganizationCode", "PrimaryQuantity", "SubinventoryCode" },
                values: new object[] { -1L, "1111111", "NADAPR", 10.0, "NAD" });

            migrationBuilder.InsertData(
                table: "itemDefinitions",
                columns: new[] { "RecId", "Brand", "ItemNumber", "ItemSize", "LongDescription", "MainCategory", "ShortDescription", "SubCategory" },
                values: new object[] { -1L, "BRAND", "ITM1", "SIZE", "TEST ITEM SHORT DESCRIPTION", "MAIN CATEGORY", "TEST ITEM 1", "SUB CATEGORY" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "itemDefinitions");

            migrationBuilder.DropTable(
                name: "Soh");
        }
    }
}
