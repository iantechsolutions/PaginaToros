using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaToros.Server.Migrations.hereford_pr
{
    public partial class AddTorosCreatedAtAndIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "TOROSUNI",
                type: "datetime",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.Sql(@"
UPDATE `TOROSUNI`
SET `createdAt` = COALESCE(
    `FCH_USU`,
    `FECHA`,
    STR_TO_DATE(NULLIF(`FECING`, ''), '%Y/%m/%d'),
    STR_TO_DATE(NULLIF(`FECING`, ''), '%d/%m/%Y'),
    NOW()
)
WHERE `createdAt` IS NULL;
");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "TOROSUNI",
                type: "datetime",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateIndex(
                name: "IX_TOROSUNI_estado_createdAt",
                table: "TOROSUNI",
                columns: new[] { "CodEstado", "createdAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TOROSUNI_CRIADOR",
                table: "TOROSUNI",
                column: "CRIADOR");

            migrationBuilder.CreateIndex(
                name: "IX_TOROSUNI_HBA",
                table: "TOROSUNI",
                column: "HBA");

            migrationBuilder.CreateIndex(
                name: "IX_TOROSUNI_TATPART",
                table: "TOROSUNI",
                column: "TATPART");

            migrationBuilder.CreateIndex(
                name: "IX_TOROSUNI_NOM_DAD",
                table: "TOROSUNI",
                column: "NOM_DAD");

            migrationBuilder.CreateIndex(
                name: "IX_TOROSUNI_criador_establecimiento_estado",
                table: "TOROSUNI",
                columns: new[] { "CRIADOR", "establecimientoId", "CodEstado" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TOROSUNI_estado_createdAt",
                table: "TOROSUNI");

            migrationBuilder.DropIndex(
                name: "IX_TOROSUNI_CRIADOR",
                table: "TOROSUNI");

            migrationBuilder.DropIndex(
                name: "IX_TOROSUNI_HBA",
                table: "TOROSUNI");

            migrationBuilder.DropIndex(
                name: "IX_TOROSUNI_TATPART",
                table: "TOROSUNI");

            migrationBuilder.DropIndex(
                name: "IX_TOROSUNI_NOM_DAD",
                table: "TOROSUNI");

            migrationBuilder.DropIndex(
                name: "IX_TOROSUNI_criador_establecimiento_estado",
                table: "TOROSUNI");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "TOROSUNI");
        }
    }
}
