-- Planteles duplicados en cascada (PLANTEL) generados por AddReporte.razor.
--
-- SINTOMA: un mismo socio quedo con varios planteles del mismo ANIOEX, con codigos que se
-- van deformando uno a partir del otro. Verificado 2026-09-21 en produccion:
--
--   NROCRI 5734   / 2026 -> 6RJ01, 6RJ011, 6J011, 6J0111, 60111   (id_original 10276)
--   NROCRI 005351 / 2026 -> 6RG041, 6G041                          (id_original 10197)
--
-- CAUSA: bucle de realimentacion en updatePlantel (Client/Pages/Inspecciones/AddReporte.razor).
-- Con el tilde "Crear nuevo plantel" prendido, cada vuelta del circuito calculaba el codigo como
-- ultimoDigitoDelAnio + DerivarTailCodigo(Plantelseleccionado.Placod); si ese codigo ya existia,
-- un bucle anticolision le agregaba sufijo y creaba OTRO plantel. Ese plantel nuevo pasaba a ser
-- Plantelseleccionado, y en la vuelta siguiente DerivarTailCodigo lo recortaba distinto (le saca
-- el primer digito y se queda con los ultimos 4), produciendo otro codigo libre. Un plantel por
-- vuelta. Las vueltas las daba agregarMov: mientras el reporte no estaba creado (RESIN1.NRORES
-- vacio) el tilde nunca se apagaba, asi que cada movimiento cargado creaba un plantel.
--
--   5RJ01 -> 6RJ01 -> 6RJ011 -> 6J011 -> 6J0111 -> 60111
--   5RI27 -> 6RI27 -> 6RI271 -> 6I271 -> 6I2711 -> 62711   (caso anterior, GERMAN GARDIEN)
--
-- ARREGLO EN CODIGO: updatePlantel ahora reutiliza el plantel cuando el codigo calculado ya
-- pertenece al mismo socio y al mismo anio, en vez de inventarle un codigo libre. El bucle
-- anticolision quedo solo para choques con planteles de OTRO socio u OTRO anio.
--
-- IMPORTANTE: varias consolas SQL (phpMyAdmin, cPanel, Adminer) no aceptan mas de una sentencia
-- por ejecucion. Correr cada bloque POR SEPARADO, de a uno, y leer el resultado antes de seguir.


-- ---------------------------------------------------------------------------
-- BLOQUE A -- AUDITORIA (solo lectura). Correr entero antes de tocar nada.
-- ---------------------------------------------------------------------------

-- A1. Todos los grupos afectados. El de id mas bajo de cada grupo es el que tiene el codigo
--     correcto (el derivado del plantel del anio anterior); los demas son basura de la cascada.
SELECT NROCRI, ANIOEX, COUNT(*) AS cant,
       GROUP_CONCAT(PLACOD ORDER BY id) AS codigos_en_orden_de_creacion,
       MIN(id) AS id_sobreviviente
FROM PLANTEL
GROUP BY NROCRI, ANIOEX
HAVING cant > 1
ORDER BY cant DESC, NROCRI;

-- A2. Detalle y fecha de alta de los dos grupos ya detectados.
--     FECING dice el dia en que se creo cada fila: sirve para saber si la cascada ocurrio antes
--     o despues del deploy del arreglo.
SELECT id, PLACOD, ANIOEX, NROCRI, FECING, FCH_USU,
       VAREDE, VQCSRD, VQSSRD, VAREPR, VQCSRP, VQSSRP
FROM PLANTEL
WHERE ANIOEX = '2026' AND NROCRI IN ('5734', '005351')
ORDER BY NROCRI, id;

-- A3. Que quedo colgando de los planteles sobrantes. Cinco tablas referencian el codigo de
--     plantel por texto (no hay FK). Si alguna devuelve filas, hay que repuntarlas (bloque B)
--     ANTES de borrar nada.
SELECT 'RESIN1' AS tabla, r.id, r.NROPLA AS codigo_plantel, r.NRORES AS referencia
FROM RESIN1 r
WHERE r.NROPLA IN ('6RJ011','6J011','6J0111','60111','6G041')
UNION ALL
SELECT 'RESIN8', r.id, r.NROPLA, r.NRORES FROM RESIN8 r
WHERE r.NROPLA IN ('6RJ011','6J011','6J0111','60111','6G041')
UNION ALL
SELECT 'DESEPLA1', d.id, d.NROPLAN, NULL FROM DESEPLA1 d
WHERE d.NROPLAN IN ('6RJ011','6J011','6J0111','60111','6G041')
UNION ALL
SELECT 'REMANSOL', m.id, m.NROPLAN, NULL FROM REMANSOL m
WHERE m.NROPLAN IN ('6RJ011','6J011','6J0111','60111','6G041')
UNION ALL
SELECT 'retenidas', t.id, t.NROPLAN, NULL FROM retenidas t
WHERE t.NROPLAN IN ('6RJ011','6J011','6J0111','60111','6G041');


-- ---------------------------------------------------------------------------
-- BLOQUE B -- REPUNTAR REFERENCIAS al plantel que sobrevive.
-- Correr SOLO las sentencias cuyas tablas hayan devuelto filas en A3.
-- Sobrevivientes: NROCRI 5734 -> '6RJ01'   |   NROCRI 005351 -> '6RG041'
-- ---------------------------------------------------------------------------

-- UPDATE RESIN1   SET NROPLA  = '6RJ01'  WHERE NROPLA  IN ('6RJ011','6J011','6J0111','60111');
-- UPDATE RESIN8   SET NROPLA  = '6RJ01'  WHERE NROPLA  IN ('6RJ011','6J011','6J0111','60111');
-- UPDATE DESEPLA1 SET NROPLAN = '6RJ01'  WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111');
-- UPDATE REMANSOL SET NROPLAN = '6RJ01'  WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111');
-- UPDATE retenidas SET NROPLAN = '6RJ01' WHERE NROPLAN IN ('6RJ011','6J011','6J0111','60111');

-- UPDATE RESIN1   SET NROPLA  = '6RG041' WHERE NROPLA  = '6G041';
-- UPDATE RESIN8   SET NROPLA  = '6RG041' WHERE NROPLA  = '6G041';
-- UPDATE DESEPLA1 SET NROPLAN = '6RG041' WHERE NROPLAN = '6G041';
-- UPDATE REMANSOL SET NROPLAN = '6RG041' WHERE NROPLAN = '6G041';
-- UPDATE retenidas SET NROPLAN = '6RG041' WHERE NROPLAN = '6G041';


-- ---------------------------------------------------------------------------
-- BLOQUE C -- CORREGIR LOS TOTALES DEL SOBREVIVIENTE.
--
-- NO se pueden copiar los totales de la ultima fila de la cascada: estan contaminados. Cada
-- vuelta recalculaba "existencia anterior" contra el plantel que acababa de crear, asi que el
-- arrastre quedo mal. Se nota a simple vista en los negativos (VQCSRP = -77 en 60111,
-- VQSSRD = -34 y VAREPR = -3 en 62711): no existen categorias con cantidad negativa.
--
-- Lo correcto es recomponerlos como: totales del plantel del anio ANTERIOR del mismo socio,
-- menos salidas, mas entradas de los movimientos (RESIN3) del reporte. Esta consulta muestra
-- los insumos para hacerlo a mano; el UPDATE final conviene escribirlo con los numeros ya
-- verificados contra el reporte en pantalla.
-- ---------------------------------------------------------------------------

SELECT p.NROCRI, p.ANIOEX, p.PLACOD,
       p.VAREDE, p.VQCSRD, p.VQSSRD, p.VAREPR, p.VQCSRP, p.VQSSRP
FROM PLANTEL p
WHERE p.NROCRI IN ('5734', '005351')
  AND p.ANIOEX IN ('2025', '2026')
ORDER BY p.NROCRI, p.ANIOEX, p.id;


-- ---------------------------------------------------------------------------
-- BLOQUE D -- BORRAR LAS SOBRANTES. Recien despues de A3 en cero (o de B aplicado) y C corregido.
-- ---------------------------------------------------------------------------

-- DELETE FROM PLANTEL WHERE ANIOEX = '2026' AND NROCRI = '5734'   AND PLACOD IN ('6RJ011','6J011','6J0111','60111');
-- DELETE FROM PLANTEL WHERE ANIOEX = '2026' AND NROCRI = '005351' AND PLACOD = '6G041';

-- Verificacion final: no deberia devolver ninguna fila.
-- SELECT NROCRI, ANIOEX, COUNT(*) AS cant FROM PLANTEL GROUP BY NROCRI, ANIOEX HAVING cant > 1;
