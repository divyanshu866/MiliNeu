using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiliNeu.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedSubscriberModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscribedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(5922));

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(5926));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6501));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6510));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6516));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6521));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6527));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6532));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6536));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6542));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6546));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 12, 16, 39, 21, 353, DateTimeKind.Local).AddTicks(6553));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(1621));

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(1626));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2207));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2213));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2219));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2224));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2229));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2233));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2238));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2243));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2248));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 11, 10, 27, 34, 547, DateTimeKind.Local).AddTicks(2254));
        }
    }
}
