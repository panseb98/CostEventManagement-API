using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CostEventManegement.DatabaseModule.Migrations
{
    /// <inheritdoc />
    public partial class AddRestObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DefaultCurrencyId = table.Column<int>(type: "integer", nullable: false),
                    AutomaticConvert = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Currencies_DefaultCurrencyId",
                        column: x => x.DefaultCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Costs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    PayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Costs_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Costs_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Costs_Users_PayerId",
                        column: x => x.PayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventUsers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    CostId = table.Column<int>(type: "integer", nullable: false),
                    PayerId = table.Column<int>(type: "integer", nullable: false),
                    DebtorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCosts_Costs_CostId",
                        column: x => x.CostId,
                        principalTable: "Costs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCosts_Users_DebtorId",
                        column: x => x.DebtorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCosts_Users_PayerId",
                        column: x => x.PayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Costs_CurrencyId",
                table: "Costs",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_EventId",
                table: "Costs",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_PayerId",
                table: "Costs",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_DefaultCurrencyId",
                table: "Events",
                column: "DefaultCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventUsers_EventId",
                table: "EventUsers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventUsers_UserId",
                table: "EventUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCosts_CostId",
                table: "UserCosts",
                column: "CostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCosts_DebtorId",
                table: "UserCosts",
                column: "DebtorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCosts_PayerId",
                table: "UserCosts",
                column: "PayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventUsers");

            migrationBuilder.DropTable(
                name: "UserCosts");

            migrationBuilder.DropTable(
                name: "Costs");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
