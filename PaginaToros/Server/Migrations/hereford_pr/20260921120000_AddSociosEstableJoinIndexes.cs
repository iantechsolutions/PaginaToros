using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaToros.Server.Migrations.hereford_pr
{
    /// <summary>
    /// Indices sobre las claves de negocio con las que se joinean SOCIOS y ESTABLE.
    ///
    /// SOCIOS.SCOD solo existia como segunda columna del PK compuesto (id, SCOD) y ESTABLE.ECOD
    /// no tenia ningun indice, asi que todo JOIN por clave de negocio terminaba en full scan con
    /// block nested loop. En la busqueda de Reportes de Inspeccion eso daba ~14,5 s por consulta
    /// y en produccion superaba el command timeout devolviendo 500.
    ///
    /// Se usa SQL directo (no migrationBuilder.CreateIndex) porque estos indices no estan
    /// declarados en el modelo y hay bases donde ya se crearon a mano; con IF NOT EXISTS la
    /// migracion es idempotente. Son NO UNIQUE a proposito: SOCIOS.SCOD tiene valores repetidos
    /// en los datos historicos, un indice unico fallaria al crearse.
    ///
    /// El mismo contenido esta en Server/Sql/20260921_socios_estable_join_indexes.sql para
    /// correrlo a mano, porque Program.cs no aplica migraciones al arrancar.
    /// </summary>
    public partial class AddSociosEstableJoinIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS `IX_SOCIOS_SCOD` ON `SOCIOS` (`SCOD`);");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS `IX_ESTABLE_ECOD` ON `ESTABLE` (`ECOD`);");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS `IX_ESTABLE_CODSOC` ON `ESTABLE` (`CODSOC`);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS `IX_ESTABLE_CODSOC` ON `ESTABLE`;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS `IX_ESTABLE_ECOD` ON `ESTABLE`;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS `IX_SOCIOS_SCOD` ON `SOCIOS`;");
        }
    }
}
