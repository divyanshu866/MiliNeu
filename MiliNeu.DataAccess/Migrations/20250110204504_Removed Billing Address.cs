using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiliNeu.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedBillingAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderAddress_BillingAddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BillingAddressId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedDeliveryBy",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedDeliveryBy",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "BillingAddressId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "ApartmentSuite", "City", "Country", "Email", "FirstName", "IsPrimary", "LastName", "PhoneNumber", "PostalCode", "State", "StreetAddress", "UserId" },
                values: new object[,]
                {
                    { 1, "4B", "New York", "USA", "johndoe@gmail.com", "John", true, "Doe", "123-456-7890", "10001", "NY", "123 Main St", "fddc1dd1-9bfb-45e6-971f-67a18db045e3" },
                    { 2, "101", "Los Angeles", "USA", "janesmith@hotmail.com", "Jane", false, "Smith", "987-654-3210", "90001", "CA", "456 Oak Avenue", "fddc1dd1-9bfb-45e6-971f-67a18db045e3" },
                    { 3, "304", "Chicago", "USA", "RaneBrown@yahoo.com", "Mark", false, "Taylor", "642-987-8764", "60601", "IL", "789 Elm Street", "fddc1dd1-9bfb-45e6-971f-67a18db045e3" }
                });

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2439));

            migrationBuilder.UpdateData(
                table: "Collections",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2444));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2946));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2953));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2958));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2963));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2968));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2972));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2978));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2983));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2988));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ReleaseDate",
                value: new DateTime(2025, 1, 6, 10, 41, 4, 556, DateTimeKind.Local).AddTicks(2993));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillingAddressId",
                table: "Orders",
                column: "BillingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderAddress_BillingAddressId",
                table: "Orders",
                column: "BillingAddressId",
                principalTable: "OrderAddress",
                principalColumn: "Id");
        }
    }
}
