using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiliNeu.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedBillingAddressToOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingAddressId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillingAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Line1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Line2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VATNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingAddresses", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(3481));

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(3485));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(3996));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4001));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4007));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4011));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4015));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4019));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4023));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4028));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4032));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 39, 55, 143, DateTimeKind.Local).AddTicks(4037));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillingAddressId",
                table: "Orders",
                column: "BillingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_BillingAddresses_BillingAddressId",
                table: "Orders",
                column: "BillingAddressId",
                principalTable: "BillingAddresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_BillingAddresses_BillingAddressId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "BillingAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BillingAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5077));

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5082));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5593));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5598));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5604));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5608));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5612));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5620));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5624));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5628));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5632));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 13, 22, 30, 51, 144, DateTimeKind.Local).AddTicks(5637));
        }
    }
}
