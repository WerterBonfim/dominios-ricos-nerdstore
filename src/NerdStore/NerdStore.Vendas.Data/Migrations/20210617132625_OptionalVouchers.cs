using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NerdStore.Vendas.Data.Migrations
{
    public partial class OptionalVouchers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Vouchers_VoucherId",
                table: "Pedidos");

            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherId",
                table: "Pedidos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Vouchers_VoucherId",
                table: "Pedidos",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Vouchers_VoucherId",
                table: "Pedidos");

            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherId",
                table: "Pedidos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Vouchers_VoucherId",
                table: "Pedidos",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
