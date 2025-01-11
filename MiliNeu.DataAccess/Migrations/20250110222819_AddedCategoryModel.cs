using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiliNeu.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Women" },
                    { 2, "Unisex" }
                });

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(1757));

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(1762));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2332) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2338) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2343) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2348) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2353) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2357) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2362) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2366) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2371) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CategoryId", "ReleaseDate" },
                values: new object[] { 1, new DateTime(2025, 1, 10, 22, 28, 17, 477, DateTimeKind.Local).AddTicks(2377) });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(1645));

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(1651));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Casual Wear", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2465) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Casual Wear", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2476) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Formal Wear", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2486) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Formal Wear", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2495) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Evening Wear", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2500) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Evening Wear", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2526) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "SumAthleisuremer", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2574) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Athleisure", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Party Wear", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2585) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "ReleaseDate" },
                values: new object[] { "Party Wear", new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2590) });
        }
    }
}
