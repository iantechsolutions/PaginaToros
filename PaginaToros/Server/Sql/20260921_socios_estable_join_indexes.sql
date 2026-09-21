-- Indices sobre las claves de negocio con las que se joinean SOCIOS y ESTABLE.
--
-- Problema: SOCIOS.SCOD solo existia como segunda columna del PK compuesto (id, SCOD) y
-- ESTABLE.ECOD no tenia ningun indice. Como ademas RESIN1.SCOD / RESIN1.ESTCOD son latin1
-- y SOCIOS.SCOD / ESTABLE.ECOD son utf8, MariaDB tiene que convertir en cada comparacion.
-- Resultado: EXPLAIN mostraba tres full scans con block nested loop y la busqueda de
-- Reportes de Inspeccion tardaba ~14,5 s, superando el command timeout en produccion (500).
--
-- Los indices son NO UNIQUE a proposito: SOCIOS.SCOD tiene valores repetidos en los datos
-- historicos (ver la verificacion 2 al final del script).
--
-- IMPORTANTE: varias consolas SQL (phpMyAdmin, cPanel, Adminer) no aceptan mas de una
-- sentencia por ejecucion. Correr cada sentencia POR SEPARADO, de a una.
--
-- Las tres son idempotentes: si el indice ya existe devuelven el aviso
-- "Note (Code 1061): Duplicate key name" en vez de fallar.

CREATE INDEX IF NOT EXISTS `IX_SOCIOS_SCOD` ON `SOCIOS` (`SCOD`);

CREATE INDEX IF NOT EXISTS `IX_ESTABLE_ECOD` ON `ESTABLE` (`ECOD`);

CREATE INDEX IF NOT EXISTS `IX_ESTABLE_CODSOC` ON `ESTABLE` (`CODSOC`);


-- ---------------------------------------------------------------------------
-- Verificaciones (tambien de a una)
-- ---------------------------------------------------------------------------

-- 1) Los tres indices tienen que aparecer, con non_unique = 1.
SELECT table_name, index_name, non_unique, seq_in_index, column_name
FROM information_schema.statistics
WHERE table_schema = DATABASE()
  AND index_name IN ('IX_SOCIOS_SCOD', 'IX_ESTABLE_ECOD', 'IX_ESTABLE_CODSOC')
ORDER BY table_name, index_name, seq_in_index;

-- 2) SOCIOS.SCOD duplicados: por eso el indice NO puede ser UNIQUE.
SELECT SCOD, COUNT(*) AS repeticiones
FROM SOCIOS
GROUP BY SCOD
HAVING repeticiones > 1;

-- 3) El plan del join tiene que pasar de "ALL" a "ref".
-- Esperado: r -> ALL (scan de RESIN1), s -> ref IX_SOCIOS_SCOD, e -> ref IX_ESTABLE_ECOD.
EXPLAIN
SELECT COUNT(*)
FROM RESIN1 r
INNER JOIN SOCIOS s ON r.SCOD = s.SCOD
LEFT JOIN ESTABLE e ON r.ESTCOD = e.ECOD
WHERE r.NRORES LIKE '%009179%';
