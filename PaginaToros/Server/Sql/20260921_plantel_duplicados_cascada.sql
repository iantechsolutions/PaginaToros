-- =============================================================================
-- LIMPIEZA: planteles duplicados en cascada (tabla PLANTEL)
-- =============================================================================
--
-- SINTOMA: un mismo socio quedo con varios planteles del mismo ANIOEX, con codigos que se
-- deforman uno a partir del otro. Verificado 2026-09-21 en produccion:
--
--   NROCRI 5734   / 2026 -> 6RJ01, 6RJ011, 6J011, 6J0111, 60111   (sobrevive id 10276 = 6RJ01)
--   NROCRI 005351 / 2026 -> 6RG041, 6G041                          (sobrevive id 10197 = 6RG041)
--
-- CAUSA: bucle de realimentacion en updatePlantel (Client/Pages/Inspecciones/AddReporte.razor).
-- Con el tilde "Crear nuevo plantel" prendido, cada vuelta calculaba el codigo como
-- ultimoDigitoDelAnio + DerivarTailCodigo(Plantelseleccionado.Placod). Si ya existia, un bucle
-- anticolision le agregaba sufijo y creaba OTRO plantel; ese pasaba a ser Plantelseleccionado y en
-- la vuelta siguiente el recorte daba otro codigo libre. Las vueltas las daba agregarMov: mientras
-- el reporte no estaba creado el tilde nunca se apagaba, asi que cada movimiento cargado creaba un
-- plantel.  5RJ01 -> 6RJ01 -> 6RJ011 -> 6J011 -> 6J0111 -> 60111
--
-- Ademas los totales de esas filas estan contaminados: cada vuelta recalculaba la existencia
-- anterior contra el plantel que acababa de crear, asi que el arrastre quedo mal. Por eso hay
-- negativos (VQCSRP = -77 en 60111). NO alcanza con borrar las sobrantes: hay que recalcular los
-- totales del sobreviviente (bloque D).
--
-- ARREGLADO EN CODIGO (pendiente de publicar): updatePlantel reutiliza el plantel cuando el codigo
-- calculado ya es del mismo socio y año, deriva el codigo del plantel de ORIGEN y no del destino,
-- y el volcado al plantel ocurre una sola vez, con el reporte ya creado.
--
-- -----------------------------------------------------------------------------
-- COMO CORRERLO
-- -----------------------------------------------------------------------------
-- Orden obligatorio: 0 -> A -> B -> C -> D -> E -> F. No saltear el backup ni la auditoria.
-- Varias consolas (phpMyAdmin, cPanel, Adminer) no aceptan mas de una sentencia por ejecucion:
-- correr cada consulta POR SEPARADO y leer el resultado antes de seguir.
-- Los UPDATE y DELETE estan comentados a proposito. Descomentar recien despues de verificar.
-- =============================================================================


-- =============================================================================
-- BLOQUE 0 -- BACKUP. Sin esto no se sigue.
-- =============================================================================
-- Desde la shell del servidor (NO desde la consola SQL):
--
--   mysqldump -u <usuario> -p <base> PLANTEL RESIN1 RESIN2 RESIN3 RESIN8 DESEPLA1 REMANSOL retenidas \
--     > backup_planteles_$(date +%F_%H%M).sql
--
-- Si no hay acceso a shell, al menos una copia de la tabla dentro de la misma base:
-- CREATE TABLE PLANTEL_backup_20260921 AS SELECT * FROM PLANTEL;


-- =============================================================================
-- BLOQUE A -- AUDITORIA (solo lectura)
-- =============================================================================

-- A1. Todos los grupos afectados, sin filtrar por año (puede haber de 2025 o antes).
--     El de id mas bajo de cada grupo es el que tiene el codigo correcto: el derivado del plantel
--     del año anterior. Los demas son basura de la cascada.
SELECT NROCRI, ANIOEX, COUNT(*) AS cant,
       GROUP_CONCAT(PLACOD ORDER BY id) AS codigos_en_orden_de_creacion,
       MIN(id) AS id_sobreviviente
FROM PLANTEL
GROUP BY NROCRI, ANIOEX
HAVING cant > 1
ORDER BY cant DESC, NROCRI;

-- A2. Detalle y fecha de alta. FECING dice el dia en que se creo cada fila: sirve para saber si la
--     cascada ocurrio antes o despues de publicar el arreglo.
SELECT id, PLACOD, ANIOEX, NROCRI, FECING, FCH_USU,
       VAREDE, VQCSRD, VQSSRD, VAREPR, VQCSRP, VQSSRP
FROM PLANTEL
WHERE ANIOEX = '2026' AND NROCRI IN ('5734', '005351')
ORDER BY NROCRI, id;

-- A3. Que reportes y declaraciones cuelgan de los planteles sobrantes. Cinco tablas referencian el
--     codigo de plantel por texto, sin FK. Lo que aparezca aca hay que repuntarlo (bloque B) ANTES
--     de borrar nada.
SELECT 'RESIN1' AS tabla, id, NROPLA AS codigo_plantel, NRORES AS referencia FROM RESIN1
WHERE NROPLA IN ('6RJ011','6J011','6J0111','60111','6G041')
UNION ALL SELECT 'RESIN8', id, NROPLA, NRORES FROM RESIN8
WHERE NROPLA IN ('6RJ011','6J011','6J0111','60111','6G041')
UNION ALL SELECT 'DESEPLA1', id, NROPLAN, NULL FROM DESEPLA1
WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111','6G041')
UNION ALL SELECT 'REMANSOL', id, NROPLAN, NULL FROM REMANSOL
WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111','6G041')
UNION ALL SELECT 'retenidas', id, NROPLAN, NULL FROM retenidas
WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111','6G041');

-- A4. Historicos de existencia anterior duplicados. El mismo bug de la pantalla creaba un RESIN2
--     por cada guardado. Importa porque el bloque D recalcula a partir de RESIN2: si hay mas de
--     uno por NRORES, hay que quedarse con el de id mas bajo (el original).
SELECT NRORES, COUNT(*) AS cant, GROUP_CONCAT(id ORDER BY id) AS ids
FROM RESIN2
GROUP BY NRORES
HAVING cant > 1
ORDER BY cant DESC;


-- =============================================================================
-- BLOQUE B -- REPUNTAR REFERENCIAS al plantel que sobrevive
-- Correr SOLO las sentencias cuyas tablas hayan devuelto filas en A3.
-- Sobrevivientes: NROCRI 5734 -> '6RJ01'   |   NROCRI 005351 -> '6RG041'
-- =============================================================================

-- UPDATE RESIN1    SET NROPLA  = '6RJ01'  WHERE NROPLA  IN ('6RJ011','6J011','6J0111','60111');
-- UPDATE RESIN8    SET NROPLA  = '6RJ01'  WHERE NROPLA  IN ('6RJ011','6J011','6J0111','60111');
-- UPDATE DESEPLA1  SET NROPLAN = '6RJ01'  WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111');
-- UPDATE REMANSOL  SET NROPLAN = '6RJ01'  WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111');
-- UPDATE retenidas SET NROPLAN = '6RJ01'  WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111');

-- UPDATE RESIN1    SET NROPLA  = '6RG041' WHERE NROPLA  = '6G041';
-- UPDATE RESIN8    SET NROPLA  = '6RG041' WHERE NROPLA  = '6G041';
-- UPDATE DESEPLA1  SET NROPLAN = '6RG041' WHERE NROPLAN = '6G041';
-- UPDATE REMANSOL  SET NROPLAN = '6RG041' WHERE NROPLAN = '6G041';
-- UPDATE retenidas SET NROPLAN = '6RG041' WHERE NROPLAN = '6G041';


-- =============================================================================
-- BLOQUE C -- BORRAR LAS SOBRANTES (ya sin nada colgando)
-- Repetir A3 antes: tiene que devolver cero filas.
-- =============================================================================

-- DELETE FROM PLANTEL WHERE ANIOEX = '2026' AND NROCRI = '5734'   AND PLACOD IN ('6RJ011','6J011','6J0111','60111');
-- DELETE FROM PLANTEL WHERE ANIOEX = '2026' AND NROCRI = '005351' AND PLACOD = '6G041';


-- =============================================================================
-- BLOQUE D -- RECALCULAR LOS TOTALES DEL SOBREVIVIENTE
--
-- Los totales guardados no sirven: los de la ultima fila de la cascada estan contaminados (de ahi
-- los negativos) y los de la primera son un estado intermedio. Se recomponen desde la formula real
-- del circuito:   existencia final = existencia anterior (RESIN2) - salidas + entradas (RESIN3)
--
-- Esta consulta los calcula. Verificar los numeros contra el reporte en pantalla ANTES de grabar.
-- =============================================================================

SELECT
    r.NROPLA                                                      AS plantel,
    r.NRORES                                                      AS reporte,
    h.EA1 - COALESCE(sal.RDVAC, 0)   + COALESCE(ent.RDVAC, 0)     AS VAREDE_nuevo,
    h.EA2 - COALESCE(sal.RDVAQCS, 0) + COALESCE(ent.RDVAQCS, 0)   AS VQCSRD_nuevo,
    h.EA3 - COALESCE(sal.RDVAQSS, 0) + COALESCE(ent.RDVAQSS, 0)   AS VQSSRD_nuevo,
    h.EA4 - COALESCE(sal.RPVAC, 0)   + COALESCE(ent.RPVAC, 0)     AS VAREPR_nuevo,
    h.EA5 - COALESCE(sal.RPVAQCS, 0) + COALESCE(ent.RPVAQCS, 0)   AS VQCSRP_nuevo,
    h.EA6 - COALESCE(sal.RPVAQSS, 0) + COALESCE(ent.RPVAQSS, 0)   AS VQSSRP_nuevo,
    p.VAREDE AS VAREDE_actual, p.VQCSRD AS VQCSRD_actual, p.VQSSRD AS VQSSRD_actual,
    p.VAREPR AS VAREPR_actual, p.VQCSRP AS VQCSRP_actual, p.VQSSRP AS VQSSRP_actual
FROM RESIN1 r
JOIN PLANTEL p
      ON p.PLACOD = r.NROPLA
-- El historico original, por si A4 mostro duplicados para ese NRORES.
JOIN RESIN2 h
      ON h.NRORES = r.NRORES
     AND h.id = (SELECT MIN(h2.id) FROM RESIN2 h2 WHERE h2.NRORES = r.NRORES)
LEFT JOIN (
    SELECT NRORES,
           SUM(RDVAC) RDVAC, SUM(RDVAQCS) RDVAQCS, SUM(RDVAQSS) RDVAQSS,
           SUM(RPVAC) RPVAC, SUM(RPVAQCS) RPVAQCS, SUM(RPVAQSS) RPVAQSS
    FROM RESIN3 WHERE TIPMOV = 'S' GROUP BY NRORES
) sal ON sal.NRORES = r.NRORES
LEFT JOIN (
    SELECT NRORES,
           SUM(RDVAC) RDVAC, SUM(RDVAQCS) RDVAQCS, SUM(RDVAQSS) RDVAQSS,
           SUM(RPVAC) RPVAC, SUM(RPVAQCS) RPVAQCS, SUM(RPVAQSS) RPVAQSS
    FROM RESIN3 WHERE TIPMOV <> 'S' OR TIPMOV IS NULL GROUP BY NRORES
) ent ON ent.NRORES = r.NRORES
WHERE r.NROPLA IN ('6RJ01', '6RG041');

-- Con los numeros de arriba ya verificados, grabar (reemplazar los ? por los valores calculados):
--
-- UPDATE PLANTEL SET VAREDE = ?, VQCSRD = ?, VQSSRD = ?, VAREPR = ?, VQCSRP = ?, VQSSRP = ?
-- WHERE PLACOD = '6RJ01' AND ANIOEX = '2026' AND NROCRI = '5734';
--
-- UPDATE PLANTEL SET VAREDE = ?, VQCSRD = ?, VQSSRD = ?, VAREPR = ?, VQCSRP = ?, VQSSRP = ?
-- WHERE PLACOD = '6RG041' AND ANIOEX = '2026' AND NROCRI = '005351';


-- =============================================================================
-- BLOQUE E -- HISTORICOS DUPLICADOS (solo si A4 devolvio filas)
-- Se conserva el de id mas bajo, que es el snapshot original.
-- =============================================================================

-- DELETE h FROM RESIN2 h
-- JOIN (SELECT NRORES, MIN(id) AS conservar FROM RESIN2 GROUP BY NRORES HAVING COUNT(*) > 1) d
--   ON d.NRORES = h.NRORES
-- WHERE h.id > d.conservar;


-- =============================================================================
-- BLOQUE F -- VERIFICACION FINAL. Las tres tienen que devolver cero filas.
-- =============================================================================

-- F1. No quedan planteles duplicados por socio y año.
SELECT NROCRI, ANIOEX, COUNT(*) AS cant FROM PLANTEL
GROUP BY NROCRI, ANIOEX HAVING cant > 1;

-- F2. No quedan existencias negativas.
SELECT id, PLACOD, ANIOEX, NROCRI, VAREDE, VQCSRD, VQSSRD, VAREPR, VQCSRP, VQSSRP
FROM PLANTEL
WHERE VAREDE < 0 OR VQCSRD < 0 OR VQSSRD < 0 OR VAREPR < 0 OR VQCSRP < 0 OR VQSSRP < 0;

-- F3. Ningun reporte quedo apuntando a un plantel inexistente.
SELECT r.id, r.NRORES, r.NROPLA
FROM RESIN1 r
LEFT JOIN PLANTEL p ON p.PLACOD = r.NROPLA
WHERE r.NROPLA IS NOT NULL AND TRIM(r.NROPLA) <> '' AND p.id IS NULL;
