using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaToros.Server.Migrations.hereford_pr
{
    /// <summary>
    /// Incorpora el establecimiento como dimension operativa del stock de hembras (PLANTEL) y del
    /// stock de dosis de semen (CERTIFSEMEN): columna nullable `establecimientoId` con FK a ESTABLE.id,
    /// mas los indices que usan las busquedas por socio + año + establecimiento.
    ///
    /// Tambien hace el backfill que puede determinarse con certeza:
    ///   - TOROSUNI: ESTCOD legacy que coincide con un ESTABLE.ECOD del mismo CRIADOR.
    ///   - PLANTEL / CERTIFSEMEN / TOROSUNI: socios con UN SOLO establecimiento cargado.
    /// Los registros de socios con mas de un establecimiento quedan en NULL a proposito y se listan
    /// con las consultas de auditoria de Server/Sql/20261006_establecimiento_plantel_certifsemen.sql.
    ///
    /// Todo el DDL es SQL directo e idempotente (IF NOT EXISTS) porque produccion no aplica
    /// migraciones por EF: el esquema se mantiene con los .sql de Server/Sql/. El Designer y el
    /// snapshot de esta migracion se generaron con `dotnet ef migrations add`; el snapshot anterior
    /// estaba desactualizado respecto del modelo (columnas PLANT_*_ID de TRANSAN, anchos de
    /// FUT_CONTROL, tablas TRANSAN_MAIL_OUTBOX y TRANSAN_TRANSFER_AUDITS), asi que el scaffolding
    /// tambien incluia esas diferencias. Se quitaron de Up()/Down() porque ya estan aplicadas en la
    /// base por los scripts 20260604/20260805; esta migracion solo toca lo que corresponde a
    /// establecimiento. El snapshot queda consistente con el modelo actual.
    ///
    /// Ancho de las columnas que referencian el codigo de plantel (bloque A2 del .sql): con un
    /// plantel por establecimiento, el segundo plantel del mismo año lleva sufijo (6RA01 -> 6RA011)
    /// y ya no entra en RESIN8.NROPLA varchar(4), DESEPLA1.NROPLAN varchar(6), REMANSOL.NROPLAN y
    /// retenidas.NROPLAN varchar(5) ni FUT_CONTROL.PLANT_DEST varchar(5). Con sql_mode estricto eso
    /// es "Data too long" (falla la operacion); la app ademas valida el largo y rechaza en vez de
    /// truncar (EstablecimientoPlantelHelper.CodigoMaxLength). Solo se ensancha: sin perdida de datos.
    /// </summary>
    public partial class AddEstablecimientoToPlantelAndCertifsemen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- PLANTEL ---------------------------------------------------------------------
            migrationBuilder.Sql("ALTER TABLE `PLANTEL` ADD COLUMN IF NOT EXISTS `establecimientoId` INT(10) NULL;");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS `IX_PLANTEL_establecimientoId` ON `PLANTEL` (`establecimientoId`);");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS `IX_PLANTEL_nrocri_anioex_establecimiento` ON `PLANTEL` (`NROCRI`, `ANIOEX`, `establecimientoId`);");
            migrationBuilder.Sql(@"ALTER TABLE `PLANTEL`
    ADD CONSTRAINT `FK_PLANTEL_ESTABLE_establecimientoId`
    FOREIGN KEY IF NOT EXISTS (`establecimientoId`) REFERENCES `ESTABLE` (`id`)
    ON DELETE RESTRICT;");

            // --- CERTIFSEMEN -----------------------------------------------------------------
            migrationBuilder.Sql("ALTER TABLE `CERTIFSEMEN` ADD COLUMN IF NOT EXISTS `establecimientoId` INT(10) NULL;");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS `IX_CERTIFSEMEN_establecimientoId` ON `CERTIFSEMEN` (`establecimientoId`);");
            migrationBuilder.Sql(@"ALTER TABLE `CERTIFSEMEN`
    ADD CONSTRAINT `FK_CERTIFSEMEN_ESTABLE_establecimientoId`
    FOREIGN KEY IF NOT EXISTS (`establecimientoId`) REFERENCES `ESTABLE` (`id`)
    ON DELETE RESTRICT;");

            // --- Columnas que referencian el codigo de plantel: al menos el ancho de PLACOD (10) ---
            // Se repite charset/collation para que MODIFY no los cambie (coinciden con el default de
            // cada tabla; verificado contra information_schema). RESIN1.NROPLA ya es varchar(15):
            // se deja explicito por si alguna base la tiene angosta. FUT_CONTROL.PLANT_DEST ya lo
            // pedia el script 20260805, pero hay bases donde ese paso no se aplico.
            migrationBuilder.Sql("ALTER TABLE `RESIN8` MODIFY COLUMN `NROPLA` VARCHAR(15) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL;");
            migrationBuilder.Sql("ALTER TABLE `RESIN1` MODIFY COLUMN `NROPLA` VARCHAR(15) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL;");
            migrationBuilder.Sql("ALTER TABLE `DESEPLA1` MODIFY COLUMN `NROPLAN` VARCHAR(15) CHARACTER SET utf8 COLLATE utf8_general_ci NULL;");
            migrationBuilder.Sql("ALTER TABLE `REMANSOL` MODIFY COLUMN `NROPLAN` VARCHAR(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL;");
            migrationBuilder.Sql("ALTER TABLE `retenidas` MODIFY COLUMN `NROPLAN` VARCHAR(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL;");
            migrationBuilder.Sql("ALTER TABLE `FUT_CONTROL` MODIFY COLUMN `PLANT_DEST` VARCHAR(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL;");

            // --- Backfill: solo asociaciones ciertas -----------------------------------------
            // 1) Toros: el ESTCOD legacy identifica al establecimiento si es un ECOD del mismo criador
            //    (y solo si hay exactamente un ESTABLE con ese ECOD para ese socio).
            migrationBuilder.Sql(@"UPDATE `TOROSUNI` t
JOIN (
    SELECT TRIM(e.`ECOD`) AS ecod, TRIM(e.`CODSOC`) AS codsoc, MIN(e.`id`) AS estab_id
    FROM `ESTABLE` e
    WHERE e.`ECOD` IS NOT NULL AND TRIM(e.`ECOD`) <> ''
      AND e.`CODSOC` IS NOT NULL AND TRIM(e.`CODSOC`) <> ''
    GROUP BY TRIM(e.`ECOD`), TRIM(e.`CODSOC`)
    HAVING COUNT(*) = 1
) u ON u.ecod = TRIM(t.`ESTCOD`) AND u.codsoc = TRIM(t.`CRIADOR`)
SET t.`establecimientoId` = u.estab_id
WHERE t.`establecimientoId` IS NULL
  AND t.`ESTCOD` IS NOT NULL AND TRIM(t.`ESTCOD`) <> '';");

            // 2) Socios con un unico establecimiento (contando activos e inactivos): no hay otra
            //    opcion posible, asi que la asignacion es inequivoca.
            const string unicoEstablecimientoPorSocio = @"(
    SELECT TRIM(e.`CODSOC`) AS codsoc, MIN(e.`id`) AS estab_id
    FROM `ESTABLE` e
    WHERE e.`CODSOC` IS NOT NULL AND TRIM(e.`CODSOC`) <> ''
    GROUP BY TRIM(e.`CODSOC`)
    HAVING COUNT(*) = 1
)";

            migrationBuilder.Sql($@"UPDATE `PLANTEL` p
JOIN {unicoEstablecimientoPorSocio} u ON u.codsoc = TRIM(p.`NROCRI`)
SET p.`establecimientoId` = u.estab_id
WHERE p.`establecimientoId` IS NULL;");

            migrationBuilder.Sql($@"UPDATE `CERTIFSEMEN` c
JOIN {unicoEstablecimientoPorSocio} u ON u.codsoc = TRIM(c.`NROCRI`)
SET c.`establecimientoId` = u.estab_id
WHERE c.`establecimientoId` IS NULL;");

            migrationBuilder.Sql($@"UPDATE `TOROSUNI` t
JOIN {unicoEstablecimientoPorSocio} u ON u.codsoc = TRIM(t.`CRIADOR`)
SET t.`establecimientoId` = u.estab_id
WHERE t.`establecimientoId` IS NULL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // El backfill no se revierte: TOROSUNI.establecimientoId ya existia antes de esta
            // migracion y las asignaciones de PLANTEL / CERTIFSEMEN desaparecen con la columna.
            // Los anchos de columna tampoco se revierten: volver a achicarlos truncaria codigos de
            // plantel ya grabados (ya habia PLACOD de 6 caracteres antes de esta migracion).
            migrationBuilder.Sql("ALTER TABLE `CERTIFSEMEN` DROP FOREIGN KEY IF EXISTS `FK_CERTIFSEMEN_ESTABLE_establecimientoId`;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS `IX_CERTIFSEMEN_establecimientoId` ON `CERTIFSEMEN`;");
            migrationBuilder.Sql("ALTER TABLE `CERTIFSEMEN` DROP COLUMN IF EXISTS `establecimientoId`;");

            migrationBuilder.Sql("ALTER TABLE `PLANTEL` DROP FOREIGN KEY IF EXISTS `FK_PLANTEL_ESTABLE_establecimientoId`;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS `IX_PLANTEL_nrocri_anioex_establecimiento` ON `PLANTEL`;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS `IX_PLANTEL_establecimientoId` ON `PLANTEL`;");
            migrationBuilder.Sql("ALTER TABLE `PLANTEL` DROP COLUMN IF EXISTS `establecimientoId`;");
        }
    }
}
