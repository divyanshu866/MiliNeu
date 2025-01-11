using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiliNeu.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderStatusEnumToOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2465));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2486));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2495));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2500));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2526));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2574));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2580));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2585));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 21, 36, 17, 760, DateTimeKind.Local).AddTicks(2590));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2328));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2815));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2820));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2825));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2830));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2835));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2839));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2844));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2849));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2853));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 10, 20, 45, 2, 388, DateTimeKind.Local).AddTicks(2858));
        }
    }
}
