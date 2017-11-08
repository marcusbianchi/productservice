using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace productservice.Migrations
{
    public partial class IntitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    childrenProductsIds = table.Column<int[]>(type: "integer[]", nullable: true),
                    parentProducId = table.Column<int>(type: "int4", nullable: true),
                    producCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    producDescription = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    producName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    productGTIN = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
