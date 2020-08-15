using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PRM.Infrastructure.Persistence.MySQL.Migrations
{
    public partial class Entities_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RenterRentalHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenterRentalHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Renters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PersonImage = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    GovernmentRegistrationDocumentCode = table.Column<string>(nullable: false),
                    RenterRentalHistoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Renters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Renters_RenterRentalHistory_RenterRentalHistoryId",
                        column: x => x.RenterRentalHistoryId,
                        principalTable: "RenterRentalHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RenterRentalHistoryId = table.Column<Guid>(nullable: true),
                    RenterId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    DailyPrice = table.Column<decimal>(nullable: false),
                    DailyLateFee = table.Column<decimal>(nullable: false),
                    WasProductDamaged = table.Column<bool>(nullable: false),
                    DamageFee = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rents_RenterRentalHistory_RenterRentalHistoryId",
                        column: x => x.RenterRentalHistoryId,
                        principalTable: "RenterRentalHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductRentalHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RentId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRentalHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRentalHistory_Rents_RentId1",
                        column: x => x.RentId1,
                        principalTable: "Rents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductRentalHistoryId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 0),
                    RentDailyPrice = table.Column<decimal>(nullable: false),
                    RentDailyLateFee = table.Column<decimal>(nullable: false),
                    RentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductRentalHistory_ProductRentalHistoryId",
                        column: x => x.ProductRentalHistoryId,
                        principalTable: "ProductRentalHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Rents_RentId",
                        column: x => x.RentId,
                        principalTable: "Rents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRentalHistory_RentId1",
                table: "ProductRentalHistory",
                column: "RentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductRentalHistoryId",
                table: "Products",
                column: "ProductRentalHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RentId",
                table: "Products",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_Renters_RenterRentalHistoryId",
                table: "Renters",
                column: "RenterRentalHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_RenterRentalHistoryId",
                table: "Rents",
                column: "RenterRentalHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Renters");

            migrationBuilder.DropTable(
                name: "ProductRentalHistory");

            migrationBuilder.DropTable(
                name: "Rents");

            migrationBuilder.DropTable(
                name: "RenterRentalHistory");
        }
    }
}
