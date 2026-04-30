using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaToros.Server.Migrations.hereford_pr
{
    public partial class AddTorosEstablecimiento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "establecimientoId",
                table: "TOROSUNI",
                type: "int(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TOROSUNI_establecimientoId",
                table: "TOROSUNI",
                column: "establecimientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOROSUNI_ESTABLE_establecimientoId",
                table: "TOROSUNI",
                column: "establecimientoId",
                principalTable: "ESTABLE",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOROSUNI_ESTABLE_establecimientoId",
                table: "TOROSUNI");

            migrationBuilder.DropIndex(
                name: "IX_TOROSUNI_establecimientoId",
                table: "TOROSUNI");

            migrationBuilder.DropColumn(
                name: "establecimientoId",
                table: "TOROSUNI");
        }
    }
}
