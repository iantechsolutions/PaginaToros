-- =============================================================================
-- ESTABLECIMIENTO COMO DIMENSION OPERATIVA: PLANTEL (stock de hembras) y
-- CERTIFSEMEN (stock de dosis de semen). Complemento del backfill de TOROSUNI.
-- =============================================================================
--
-- Mismo contenido que la migracion EF
--   Server/Migrations/hereford_pr/20261006033255_AddEstablecimientoToPlantelAndCertifsemen.cs
-- para correrlo a mano, porque Program.cs no aplica migraciones al arrancar.
--
-- Todo el DDL es idempotente (IF NOT EXISTS). Los UPDATE de backfill tambien pueden repetirse:
-- solo tocan filas con establecimientoId IS NULL.
--
-- REGLA DEL BACKFILL: se asigna establecimiento UNICAMENTE cuando es inequivoco.
--   (1) TOROSUNI con ESTCOD legacy que coincide con exactamente un ESTABLE.ECOD del mismo CRIADOR.
--   (2) PLANTEL / CERTIFSEMEN / TOROSUNI de socios que tienen UN SOLO establecimiento cargado
--       (contando activos e inactivos: no hay otra opcion posible).
-- Lo demas queda NULL y se lista en el BLOQUE C para definirlo a mano. NO inventar asociaciones.
--
-- Orden: 0 (backup) -> A (DDL) -> A2 (ancho de columnas de codigo de plantel) -> B (backfill)
--        -> C (auditoria de lo que quedo pendiente).
-- Varias consolas (phpMyAdmin, cPanel) no aceptan mas de una sentencia por ejecucion: correr
-- cada una por separado.
-- =============================================================================


-- =============================================================================
-- BLOQUE 0 -- BACKUP
-- =============================================================================
--   mysqldump -u <usuario> -p <base> PLANTEL CERTIFSEMEN TOROSUNI ESTABLE \
--     RESIN1 RESIN8 DESEPLA1 REMANSOL retenidas FUT_CONTROL \
--     > backup_establecimiento_$(date +%F_%H%M).sql


-- =============================================================================
-- BLOQUE A -- DDL
-- =============================================================================

ALTER TABLE `PLANTEL` ADD COLUMN IF NOT EXISTS `establecimientoId` INT(10) NULL;
CREATE INDEX IF NOT EXISTS `IX_PLANTEL_establecimientoId` ON `PLANTEL` (`establecimientoId`);
CREATE INDEX IF NOT EXISTS `IX_PLANTEL_nrocri_anioex_establecimiento` ON `PLANTEL` (`NROCRI`, `ANIOEX`, `establecimientoId`);
ALTER TABLE `PLANTEL`
    ADD CONSTRAINT `FK_PLANTEL_ESTABLE_establecimientoId`
    FOREIGN KEY IF NOT EXISTS (`establecimientoId`) REFERENCES `ESTABLE` (`id`)
    ON DELETE RESTRICT;

ALTER TABLE `CERTIFSEMEN` ADD COLUMN IF NOT EXISTS `establecimientoId` INT(10) NULL;
CREATE INDEX IF NOT EXISTS `IX_CERTIFSEMEN_establecimientoId` ON `CERTIFSEMEN` (`establecimientoId`);
ALTER TABLE `CERTIFSEMEN`
    ADD CONSTRAINT `FK_CERTIFSEMEN_ESTABLE_establecimientoId`
    FOREIGN KEY IF NOT EXISTS (`establecimientoId`) REFERENCES `ESTABLE` (`id`)
    ON DELETE RESTRICT;

-- TOROSUNI.establecimientoId ya existe (20260429). Si la base no lo tuviera:
-- ALTER TABLE `TOROSUNI` ADD COLUMN IF NOT EXISTS `establecimientoId` INT(10) NULL;
-- (ver Server/Sql/20260429_toros_establecimiento_filters.sql)


-- =============================================================================
-- BLOQUE A2 -- ANCHO DE LAS COLUMNAS QUE REFERENCIAN EL CODIGO DE PLANTEL
-- =============================================================================
-- PLANTEL.PLACOD es varchar(10). Hasta ahora los codigos tenian 5 caracteres (ultimo digito del
-- año + cola de 4). Con un plantel por establecimiento, el segundo plantel del mismo año de la
-- misma razon social lleva sufijo numerico (6RA01 -> 6RA011, 6RA012...): 6 caracteres o mas.
-- Varias columnas que guardan ese codigo son mas angostas que PLACOD y, con sql_mode estricto,
-- un codigo largo da "Data too long for column" (no se trunca, falla la operacion):
--   RESIN8.NROPLA    varchar(4)  -> 15 (igual que RESIN1.NROPLA, con la que se relaciona por NRORES;
--                                       solo la escribe la consolidacion de planteles duplicados)
--   RESIN1.NROPLA    varchar(15) -> 15 (sin cambio; se deja explicito por si alguna base la tiene angosta)
--   DESEPLA1.NROPLAN varchar(6)  -> 15 (la escribe AddDeclaracion; el script 20260327 la habia dejado en 6)
--   REMANSOL.NROPLAN varchar(5)  -> 10 (consolidacion de planteles duplicados)
--   retenidas.NROPLAN varchar(5) -> 10 (consolidacion de planteles duplicados)
--   FUT_CONTROL.PLANT_DEST varchar(5) -> 10 (la escribe AddTransferenciaFu; el script 20260805 ya lo
--                                       pedia pero hay bases donde no se aplico ese paso)
-- Solo se ensancha (VARCHAR mas grande): no borra ni transforma datos y es compatible hacia atras.
-- El charset/collation se repite explicitamente para que MODIFY no los cambie.
-- La app ademas valida el largo (EstablecimientoPlantelHelper.CodigoMaxLength = 10) y rechaza en
-- vez de truncar.

-- A2.0 Diagnostico previo (solo lectura).
SELECT TABLE_NAME, COLUMN_NAME, COLUMN_TYPE, CHARACTER_SET_NAME, COLLATION_NAME
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND ((TABLE_NAME = 'RESIN8'      AND COLUMN_NAME = 'NROPLA')
    OR (TABLE_NAME = 'RESIN1'      AND COLUMN_NAME = 'NROPLA')
    OR (TABLE_NAME = 'DESEPLA1'    AND COLUMN_NAME = 'NROPLAN')
    OR (TABLE_NAME = 'REMANSOL'    AND COLUMN_NAME = 'NROPLAN')
    OR (TABLE_NAME = 'retenidas'   AND COLUMN_NAME = 'NROPLAN')
    OR (TABLE_NAME = 'FUT_CONTROL' AND COLUMN_NAME IN ('PLANTEL', 'PLANT_DEST'))
    OR (TABLE_NAME = 'PLANTEL'     AND COLUMN_NAME = 'PLACOD'))
ORDER BY TABLE_NAME, COLUMN_NAME;

-- A2.1 Ensanchar.
ALTER TABLE `RESIN8`      MODIFY COLUMN `NROPLA`     VARCHAR(15) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL;
ALTER TABLE `RESIN1`      MODIFY COLUMN `NROPLA`     VARCHAR(15) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL;
ALTER TABLE `DESEPLA1`    MODIFY COLUMN `NROPLAN`    VARCHAR(15) CHARACTER SET utf8   COLLATE utf8_general_ci   NULL;
ALTER TABLE `REMANSOL`    MODIFY COLUMN `NROPLAN`    VARCHAR(10) CHARACTER SET utf8   COLLATE utf8_general_ci   NULL;
ALTER TABLE `retenidas`   MODIFY COLUMN `NROPLAN`    VARCHAR(10) CHARACTER SET utf8   COLLATE utf8_general_ci   NULL;
ALTER TABLE `FUT_CONTROL` MODIFY COLUMN `PLANT_DEST` VARCHAR(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL;

-- A2.2 Verificacion: ninguna columna que referencia el codigo de plantel puede ser mas angosta
--      que PLANTEL.PLACOD. Esperado: 0 filas.
SELECT c.TABLE_NAME, c.COLUMN_NAME, c.CHARACTER_MAXIMUM_LENGTH
FROM information_schema.COLUMNS c
WHERE c.TABLE_SCHEMA = DATABASE()
  AND ((c.TABLE_NAME = 'RESIN8'      AND c.COLUMN_NAME = 'NROPLA')
    OR (c.TABLE_NAME = 'RESIN1'      AND c.COLUMN_NAME = 'NROPLA')
    OR (c.TABLE_NAME = 'DESEPLA1'    AND c.COLUMN_NAME = 'NROPLAN')
    OR (c.TABLE_NAME = 'REMANSOL'    AND c.COLUMN_NAME = 'NROPLAN')
    OR (c.TABLE_NAME = 'retenidas'   AND c.COLUMN_NAME = 'NROPLAN')
    OR (c.TABLE_NAME = 'FUT_CONTROL' AND c.COLUMN_NAME IN ('PLANTEL', 'PLANT_DEST'))
    OR (c.TABLE_NAME = 'TRANSAN'     AND c.COLUMN_NAME IN ('PLANT', 'NVO_PLA')))
  AND c.CHARACTER_MAXIMUM_LENGTH < (SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.COLUMNS
                                     WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'PLANTEL' AND COLUMN_NAME = 'PLACOD');


-- =============================================================================
-- BLOQUE B -- BACKFILL (solo asociaciones ciertas)
-- =============================================================================

-- B1. Toros: ESTCOD legacy -> establecimientoId (solo si hay un unico ESTABLE con ese ECOD para
--     ese socio).
UPDATE `TOROSUNI` t
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
  AND t.`ESTCOD` IS NOT NULL AND TRIM(t.`ESTCOD`) <> '';

-- B2. Planteles de socios con un unico establecimiento.
UPDATE `PLANTEL` p
JOIN (
    SELECT TRIM(e.`CODSOC`) AS codsoc, MIN(e.`id`) AS estab_id
    FROM `ESTABLE` e
    WHERE e.`CODSOC` IS NOT NULL AND TRIM(e.`CODSOC`) <> ''
    GROUP BY TRIM(e.`CODSOC`)
    HAVING COUNT(*) = 1
) u ON u.codsoc = TRIM(p.`NROCRI`)
SET p.`establecimientoId` = u.estab_id
WHERE p.`establecimientoId` IS NULL;

-- B3. Certificados de semen de socios con un unico establecimiento.
UPDATE `CERTIFSEMEN` c
JOIN (
    SELECT TRIM(e.`CODSOC`) AS codsoc, MIN(e.`id`) AS estab_id
    FROM `ESTABLE` e
    WHERE e.`CODSOC` IS NOT NULL AND TRIM(e.`CODSOC`) <> ''
    GROUP BY TRIM(e.`CODSOC`)
    HAVING COUNT(*) = 1
) u ON u.codsoc = TRIM(c.`NROCRI`)
SET c.`establecimientoId` = u.estab_id
WHERE c.`establecimientoId` IS NULL;

-- B4. Toros de socios con un unico establecimiento.
UPDATE `TOROSUNI` t
JOIN (
    SELECT TRIM(e.`CODSOC`) AS codsoc, MIN(e.`id`) AS estab_id
    FROM `ESTABLE` e
    WHERE e.`CODSOC` IS NOT NULL AND TRIM(e.`CODSOC`) <> ''
    GROUP BY TRIM(e.`CODSOC`)
    HAVING COUNT(*) = 1
) u ON u.codsoc = TRIM(t.`CRIADOR`)
SET t.`establecimientoId` = u.estab_id
WHERE t.`establecimientoId` IS NULL;


-- =============================================================================
-- BLOQUE C -- AUDITORIA: lo que quedo sin establecimiento y requiere definicion manual
-- (solo lectura)
-- =============================================================================

-- C1. Resumen: cuantos registros quedaron sin establecimiento, separados por causa.
SELECT 'PLANTEL' AS tabla,
       CASE WHEN e.cant IS NULL THEN 'socio sin establecimientos'
            WHEN e.cant > 1 THEN 'socio con varios establecimientos'
            ELSE 'otro (revisar)' END AS causa,
       COUNT(*) AS registros
FROM PLANTEL p
LEFT JOIN (SELECT TRIM(CODSOC) codsoc, COUNT(*) cant FROM ESTABLE GROUP BY TRIM(CODSOC)) e ON e.codsoc = TRIM(p.NROCRI)
WHERE p.establecimientoId IS NULL
GROUP BY CASE WHEN e.cant IS NULL THEN 'socio sin establecimientos'
            WHEN e.cant > 1 THEN 'socio con varios establecimientos'
            ELSE 'otro (revisar)' END
UNION ALL
SELECT 'CERTIFSEMEN',
       CASE WHEN e.cant IS NULL THEN 'socio sin establecimientos'
            WHEN e.cant > 1 THEN 'socio con varios establecimientos'
            ELSE 'otro (revisar)' END,
       COUNT(*)
FROM CERTIFSEMEN c
LEFT JOIN (SELECT TRIM(CODSOC) codsoc, COUNT(*) cant FROM ESTABLE GROUP BY TRIM(CODSOC)) e ON e.codsoc = TRIM(c.NROCRI)
WHERE c.establecimientoId IS NULL
GROUP BY CASE WHEN e.cant IS NULL THEN 'socio sin establecimientos'
            WHEN e.cant > 1 THEN 'socio con varios establecimientos'
            ELSE 'otro (revisar)' END
UNION ALL
SELECT 'TOROSUNI',
       CASE WHEN e.cant IS NULL THEN 'socio sin establecimientos'
            WHEN e.cant > 1 THEN 'socio con varios establecimientos'
            ELSE 'otro (revisar)' END,
       COUNT(*)
FROM TOROSUNI t
LEFT JOIN (SELECT TRIM(CODSOC) codsoc, COUNT(*) cant FROM ESTABLE GROUP BY TRIM(CODSOC)) e ON e.codsoc = TRIM(t.CRIADOR)
WHERE t.establecimientoId IS NULL
GROUP BY CASE WHEN e.cant IS NULL THEN 'socio sin establecimientos'
            WHEN e.cant > 1 THEN 'socio con varios establecimientos'
            ELSE 'otro (revisar)' END;

-- C2. Planteles ambiguos (socio con varios establecimientos) con la SUGERENCIA que sale de sus
--     reportes de inspeccion: si todos los RESIN1 del plantel se hicieron en el mismo ESTCOD,
--     ese es el candidato. Es una sugerencia, NO se aplica sola: en el sistema anterior el
--     establecimiento era solo el lugar de la inspeccion y el plantel era de toda la razon social.
SELECT p.id, p.PLACOD, p.ANIOEX, p.NROCRI, s.NOMBRE AS socio,
       e.cant AS establecimientos_del_socio,
       (SELECT GROUP_CONCAT(DISTINCT r.ESTCOD) FROM RESIN1 r
         WHERE r.NROPLA = p.PLACOD AND TRIM(r.SCOD) = TRIM(p.NROCRI)
           AND r.ESTCOD IS NOT NULL AND TRIM(r.ESTCOD) <> '') AS estcod_inspecciones,
       (SELECT COUNT(DISTINCT r.ESTCOD) FROM RESIN1 r
         WHERE r.NROPLA = p.PLACOD AND TRIM(r.SCOD) = TRIM(p.NROCRI)
           AND r.ESTCOD IS NOT NULL AND TRIM(r.ESTCOD) <> '') AS cant_estcod_distintos,
       p.VAREDE, p.VQCSRD, p.VQSSRD, p.VAREPR, p.VQCSRP, p.VQSSRP
FROM PLANTEL p
JOIN (SELECT TRIM(CODSOC) codsoc, COUNT(*) cant FROM ESTABLE GROUP BY TRIM(CODSOC)) e ON e.codsoc = TRIM(p.NROCRI)
LEFT JOIN SOCIOS s ON TRIM(s.SCOD) = TRIM(p.NROCRI)
WHERE p.establecimientoId IS NULL AND e.cant > 1
ORDER BY p.NROCRI, p.ANIOEX DESC, p.PLACOD;

-- C3. Establecimientos de cada socio ambiguo, para decidir a mano.
SELECT e.CODSOC, s.NOMBRE AS socio, e.id AS establecimiento_id, e.ECOD, e.NOMBRE AS establecimiento, e.ACTIVO, e.LOCALI
FROM ESTABLE e
JOIN (SELECT TRIM(CODSOC) codsoc FROM ESTABLE GROUP BY TRIM(CODSOC) HAVING COUNT(*) > 1) m ON m.codsoc = TRIM(e.CODSOC)
LEFT JOIN SOCIOS s ON TRIM(s.SCOD) = TRIM(e.CODSOC)
ORDER BY e.CODSOC, e.ACTIVO DESC, e.id;

-- C4. Certificados ambiguos (socio con varios establecimientos), con stock remanente.
SELECT c.id, c.NRO_CERT, c.HBA, c.NOM_DAD, c.NROCRI, s.NOMBRE AS socio, c.NROCEN, c.NR_DOSI
FROM CERTIFSEMEN c
JOIN (SELECT TRIM(CODSOC) codsoc, COUNT(*) cant FROM ESTABLE GROUP BY TRIM(CODSOC)) e ON e.codsoc = TRIM(c.NROCRI)
LEFT JOIN SOCIOS s ON TRIM(s.SCOD) = TRIM(c.NROCRI)
WHERE c.establecimientoId IS NULL AND e.cant > 1
ORDER BY (c.NR_DOSI > 0) DESC, c.NROCRI, c.id;

-- C5. Toros ambiguos activos (socio con varios establecimientos y sin ESTCOD utilizable).
SELECT t.id, t.HBA, t.TATPART, t.NOM_DAD, t.CRIADOR, s.NOMBRE AS socio, t.ESTCOD, t.CodEstado
FROM TOROSUNI t
JOIN (SELECT TRIM(CODSOC) codsoc, COUNT(*) cant FROM ESTABLE GROUP BY TRIM(CODSOC)) e ON e.codsoc = TRIM(t.CRIADOR)
LEFT JOIN SOCIOS s ON TRIM(s.SCOD) = TRIM(t.CRIADOR)
WHERE t.establecimientoId IS NULL AND e.cant > 1
ORDER BY (t.CodEstado = '1') DESC, t.CRIADOR, t.id;

-- C6. Asignacion manual (plantilla). Reemplazar los ? despues de decidir con C2/C3.
-- UPDATE PLANTEL     SET establecimientoId = ? WHERE id = ?;
-- UPDATE CERTIFSEMEN SET establecimientoId = ? WHERE id = ?;
-- UPDATE TOROSUNI    SET establecimientoId = ?, ESTCOD = (SELECT ECOD FROM ESTABLE WHERE id = ?) WHERE id = ?;

-- C7. Verificacion: ningun registro apunta a un establecimiento de OTRO socio.
SELECT 'PLANTEL' tabla, p.id, p.NROCRI AS socio_registro, e.CODSOC AS socio_establecimiento
FROM PLANTEL p JOIN ESTABLE e ON e.id = p.establecimientoId
WHERE TRIM(e.CODSOC) <> TRIM(p.NROCRI)
UNION ALL
SELECT 'CERTIFSEMEN', c.id, c.NROCRI, e.CODSOC
FROM CERTIFSEMEN c JOIN ESTABLE e ON e.id = c.establecimientoId
WHERE TRIM(e.CODSOC) <> TRIM(c.NROCRI)
UNION ALL
SELECT 'TOROSUNI', t.id, t.CRIADOR, e.CODSOC
FROM TOROSUNI t JOIN ESTABLE e ON e.id = t.establecimientoId
WHERE TRIM(e.CODSOC) <> TRIM(t.CRIADOR);
