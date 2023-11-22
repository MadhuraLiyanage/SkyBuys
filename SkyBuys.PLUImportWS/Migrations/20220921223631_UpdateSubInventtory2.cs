using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyBuys.PLUImportWS.Migrations
{
    public partial class UpdateSubInventtory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "SubInventory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SubInventory",
                keyColumn: "SubInventoryCode",
                keyValue: "DCS",
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "SubInventory",
                keyColumn: "SubInventoryCode",
                keyValue: "DFA",
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "SubInventory",
                keyColumn: "SubInventoryCode",
                keyValue: "DFD",
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "SubInventory",
                keyColumn: "SubInventoryCode",
                keyValue: "DFV",
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "SubInventory",
                keyColumn: "SubInventoryCode",
                keyValue: "DNK",
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "SubInventory",
                keyColumn: "SubInventoryCode",
                keyValue: "DWS",
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "SubInventory",
                keyColumn: "SubInventoryCode",
                keyValue: "NSD",
                column: "Active",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "SubInventory");
        }
    }
}
