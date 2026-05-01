using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    [Migration("20260430224500_AddStockProduit")]
    public class AddStockProduit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STOCK_PRODUIT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NumeroStock = table.Column<int>(type: "int", nullable: false),
                    CodeProduit = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STOCK_PRODUIT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_STOCK_PRODUIT_PRODUIT_CodeProduit",
                        column: x => x.CodeProduit,
                        principalTable: "PRODUIT",
                        principalColumn: "CodeProduit",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_STOCK_PRODUIT_STOCK_NumeroStock",
                        column: x => x.NumeroStock,
                        principalTable: "STOCK",
                        principalColumn: "NumeroStock",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_STOCK_PRODUIT_CodeProduit",
                table: "STOCK_PRODUIT",
                column: "CodeProduit");

            migrationBuilder.CreateIndex(
                name: "IX_STOCK_PRODUIT_NumeroStock",
                table: "STOCK_PRODUIT",
                column: "NumeroStock");

            migrationBuilder.CreateIndex(
                name: "IX_STOCK_PRODUIT_NumeroStock_CodeProduit",
                table: "STOCK_PRODUIT",
                columns: new[] { "NumeroStock", "CodeProduit" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STOCK_PRODUIT");
        }
    }
}
