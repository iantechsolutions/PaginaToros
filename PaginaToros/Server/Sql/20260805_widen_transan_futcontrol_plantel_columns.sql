-- =====================================================================
-- 20260805_widen_transan_futcontrol_plantel_columns.sql
--
-- Motivo:
--   TRANSAN.PLANT / TRANSAN.NVO_PLA estan definidas como varchar(4),
--   pero PLANTEL.PLACOD (el valor que se guarda ahi) es varchar(10).
--   Cualquier plantel con codigo de 5+ caracteres (ej: 5R141, 5RB52)
--   hace fallar el INSERT con "Data too long for column 'NVO_PLA'".
--
--   Lo mismo pasa en FUT_CONTROL.PLANTEL (varchar(4)) y, ademas,
--   FUT_CONTROL.VNOM / CNOM son mas cortas que SOCIOS.NOMBRE (varchar(100)),
--   por lo que razones sociales largas tambien rompen el guardado.
--
--   TRANSAN.TIPHAC es varchar(4) pero el valor 'PHVIP' ocupa 5.
--
-- Naturaleza del cambio:
--   Solo ensancha columnas (VARCHAR mas grande). No borra ni transforma
--   datos existentes y es compatible hacia atras.
--
-- Requisito: correr DESPUES de tener backup y ANTES de desplegar el
--   codigo nuevo (el codigo viejo sigue funcionando con estas columnas).
-- =====================================================================


-- ---------------------------------------------------------------------
-- PASO 0 - Diagnostico previo (solo lectura, no modifica nada).
--          Corrélo primero y guardá la salida como referencia.
-- ---------------------------------------------------------------------
SELECT TABLE_NAME, COLUMN_NAME, COLUMN_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND (
        (TABLE_NAME = 'TRANSAN'     AND COLUMN_NAME IN ('PLANT', 'NVO_PLA', 'TIPHAC'))
     OR (TABLE_NAME = 'FUT_CONTROL' AND COLUMN_NAME IN ('PLANTEL', 'PLANT_DEST', 'VNOM', 'CNOM'))
     OR (TABLE_NAME = 'PLANTEL'     AND COLUMN_NAME = 'PLACOD')
     OR (TABLE_NAME = 'SOCIOS'      AND COLUMN_NAME = 'NOMBRE')
      )
ORDER BY TABLE_NAME, COLUMN_NAME;

-- Cuantos planteles hoy NO pueden transferirse por el limite actual:
SELECT COUNT(*) AS planteles_con_codigo_mayor_a_4
FROM `PLANTEL`
WHERE CHAR_LENGTH(`PLACOD`) > 4;


-- ---------------------------------------------------------------------
-- PASO 1 - TRANSAN (transferencias de animales)
-- ---------------------------------------------------------------------
ALTER TABLE `TRANSAN`
    MODIFY COLUMN `PLANT`   VARCHAR(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
    MODIFY COLUMN `NVO_PLA` VARCHAR(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
    MODIFY COLUMN `TIPHAC`  VARCHAR(8)  CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL;


-- ---------------------------------------------------------------------
-- PASO 2 - FUT_CONTROL (transferencias futuras)
-- ---------------------------------------------------------------------
ALTER TABLE `FUT_CONTROL`
    MODIFY COLUMN `PLANTEL`    VARCHAR(10)  CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
    MODIFY COLUMN `PLANT_DEST` VARCHAR(10)  CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
    MODIFY COLUMN `VNOM`       VARCHAR(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
    MODIFY COLUMN `CNOM`       VARCHAR(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL;


-- ---------------------------------------------------------------------
-- PASO 3 - Verificacion posterior.
--          Esperado:
--            TRANSAN.PLANT            varchar(10)
--            TRANSAN.NVO_PLA          varchar(10)
--            TRANSAN.TIPHAC           varchar(8)
--            FUT_CONTROL.PLANTEL      varchar(10)
--            FUT_CONTROL.PLANT_DEST   varchar(10)
--            FUT_CONTROL.VNOM         varchar(100)
--            FUT_CONTROL.CNOM         varchar(100)
-- ---------------------------------------------------------------------
SELECT TABLE_NAME, COLUMN_NAME, COLUMN_TYPE
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND (
        (TABLE_NAME = 'TRANSAN'     AND COLUMN_NAME IN ('PLANT', 'NVO_PLA', 'TIPHAC'))
     OR (TABLE_NAME = 'FUT_CONTROL' AND COLUMN_NAME IN ('PLANTEL', 'PLANT_DEST', 'VNOM', 'CNOM'))
      )
ORDER BY TABLE_NAME, COLUMN_NAME;
