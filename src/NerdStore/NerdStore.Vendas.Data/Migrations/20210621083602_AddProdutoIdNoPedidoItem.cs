using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NerdStore.Vendas.Data.Migrations
{
    public partial class AddProdutoIdNoPedidoItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProdutoId",
                table: "PedidoItens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "PedidoItens");
        }
    }
}
