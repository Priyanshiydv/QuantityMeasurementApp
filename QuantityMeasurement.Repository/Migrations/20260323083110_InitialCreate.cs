using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurement.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuantityMeasurementEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstOperand = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SecondOperand = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OperationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HasError = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MeasurementType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityMeasurementEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HasError",
                table: "QuantityMeasurementEntity",
                column: "HasError");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementType",
                table: "QuantityMeasurementEntity",
                column: "MeasurementType");

            migrationBuilder.CreateIndex(
                name: "IX_OperationType",
                table: "QuantityMeasurementEntity",
                column: "OperationType");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp",
                table: "QuantityMeasurementEntity",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuantityMeasurementEntity");
        }
    }
}
