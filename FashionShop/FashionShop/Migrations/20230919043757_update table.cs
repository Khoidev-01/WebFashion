using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionShop.Migrations
{
    public partial class updatetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoucherID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherID",
                table: "Orders",
                column: "VoucherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vouchers_VoucherID",
                table: "Orders",
                column: "VoucherID",
                principalTable: "Vouchers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vouchers_VoucherID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VoucherID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "Orders");
        }
    }
}
