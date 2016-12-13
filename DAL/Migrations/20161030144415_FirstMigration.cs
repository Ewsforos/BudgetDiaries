using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    PK_ID = table.Column<Guid>(nullable: false),
                    Balance = table.Column<double>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.PK_ID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    PK_ID = table.Column<Guid>(nullable: false),
                    IconPath = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.PK_ID);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    PK_ID = table.Column<Guid>(nullable: false),
                    AccountPK_ID = table.Column<Guid>(nullable: true),
                    CategoryPK_ID = table.Column<Guid>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    DateFrom = table.Column<DateTime>(nullable: true),
                    DateTo = table.Column<DateTime>(nullable: true),
                    FK_Account = table.Column<Guid>(nullable: false),
                    FK_Categoty = table.Column<Guid>(nullable: false),
                    IsRepeatable = table.Column<bool>(nullable: false, defaultValue: false),
                    Note = table.Column<string>(nullable: true),
                    RepeatType = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.PK_ID);
                    table.ForeignKey(
                        name: "FK_Transaction_Accounts_AccountPK_ID",
                        column: x => x.AccountPK_ID,
                        principalTable: "Accounts",
                        principalColumn: "PK_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Categories_CategoryPK_ID",
                        column: x => x.CategoryPK_ID,
                        principalTable: "Categories",
                        principalColumn: "PK_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountPK_ID",
                table: "Transaction",
                column: "AccountPK_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CategoryPK_ID",
                table: "Transaction",
                column: "CategoryPK_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
