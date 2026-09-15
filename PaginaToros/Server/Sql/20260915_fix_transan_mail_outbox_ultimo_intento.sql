-- =====================================================================
-- 20260915_fix_transan_mail_outbox_ultimo_intento.sql
--
-- Motivo:
--   En produccion (hereford_pr) la tabla TRANSAN_MAIL_OUTBOX NO fue creada
--   con el script del repo (20260604_add_transan_plantel_ids_and_mail_outbox.sql)
--   sino a mano, con nombres distintos. El resultado:
--
--     * FALTA la columna `ultimo_intento` (produccion tiene `fecha_ultimo_intento`).
--     * Sobran `mensaje_error`, `fecha_actualizacion` y `transferencia_id`.
--
--   El modelo EF (hereford_prContext -> TransanMailOutbox.UltimoIntento)
--   mapea a `ultimo_intento`, asi que el INSERT de la cola de mails falla con
--   "Unknown column 'ultimo_intento' in 'field list'".
--
--   Ese INSERT ocurre DENTRO de la transaccion de TransanController.Guardar /
--   Editar, por lo que TODA la transferencia se revierte y el usuario ve el
--   mensaje generico "No se pudo guardar la transferencia por un error de
--   datos. Revisa los campos ingresados."
--
--   Sintoma verificable: TRANSAN_MAIL_OUTBOX.AUTO_INCREMENT = 1 con 0 filas
--   (nunca se inserto una fila) y TRANSAN_TRANSFER_AUDITS con AUTO_INCREMENT
--   avanzado y 0 filas (cada intento se inserto y se revirtio).
--
--   El worker TransanMailOutboxWorker tambien lee/escribe esa columna, asi que
--   ademas nunca pudo procesar la cola.
--
-- Naturaleza del cambio:
--   Renombra la columna existente (o la agrega si no existe). No borra datos.
--   Es idempotente: se puede correr mas de una vez.
--
-- Requisito: correr ANTES de volver a intentar una transferencia.
-- =====================================================================


-- ---------------------------------------------------------------------
-- PASO 0 - Diagnostico previo (solo lectura). Guarda la salida.
-- ---------------------------------------------------------------------
SELECT COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME = 'TRANSAN_MAIL_OUTBOX'
ORDER BY ORDINAL_POSITION;

-- Cuantas filas hay realmente en la cola (TABLE_ROWS de InnoDB es estimado):
SELECT COUNT(*) AS filas_en_outbox FROM `TRANSAN_MAIL_OUTBOX`;


-- ---------------------------------------------------------------------
-- PASO 1 - Dejar la columna `ultimo_intento` que espera el modelo EF.
--
--   1.a Si existe `fecha_ultimo_intento` y NO existe `ultimo_intento`,
--       se renombra (conserva los datos que hubiera).
--   1.b Si no existe ninguna de las dos, se crea `ultimo_intento`.
--   1.c Si `ultimo_intento` ya existe, no se hace nada.
-- ---------------------------------------------------------------------
SET @rename_sql = (
    SELECT IF(
        EXISTS (SELECT 1 FROM information_schema.COLUMNS
                 WHERE TABLE_SCHEMA = DATABASE()
                   AND TABLE_NAME = 'TRANSAN_MAIL_OUTBOX'
                   AND COLUMN_NAME = 'fecha_ultimo_intento')
        AND NOT EXISTS (SELECT 1 FROM information_schema.COLUMNS
                         WHERE TABLE_SCHEMA = DATABASE()
                           AND TABLE_NAME = 'TRANSAN_MAIL_OUTBOX'
                           AND COLUMN_NAME = 'ultimo_intento'),
        'ALTER TABLE `TRANSAN_MAIL_OUTBOX` CHANGE COLUMN `fecha_ultimo_intento` `ultimo_intento` DATETIME NULL',
        'DO 0'
    )
);
PREPARE stmt FROM @rename_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

ALTER TABLE `TRANSAN_MAIL_OUTBOX`
    ADD COLUMN IF NOT EXISTS `ultimo_intento` DATETIME NULL AFTER `fecha_creacion`;


-- ---------------------------------------------------------------------
-- PASO 2 - Verificacion posterior.
--          Esperado: aparece `ultimo_intento` datetime NULL y ya NO aparece
--          `fecha_ultimo_intento`.
-- ---------------------------------------------------------------------
SELECT COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME = 'TRANSAN_MAIL_OUTBOX'
ORDER BY ORDINAL_POSITION;


-- ---------------------------------------------------------------------
-- PASO 3 - OPCIONAL (no lo corras sin leer).
--
--   Produccion tiene 3 columnas huerfanas que el codigo no usa nunca:
--   `mensaje_error`, `fecha_actualizacion` y `transferencia_id`.
--   Son NULLABLE, asi que no rompen el INSERT; quedan solo como ruido.
--   Si queres alinear el esquema al script del repo, y SOLO si el PASO 0
--   confirmo 0 filas en la tabla:
--
-- ALTER TABLE `TRANSAN_MAIL_OUTBOX`
--     DROP COLUMN `mensaje_error`,
--     DROP COLUMN `fecha_actualizacion`,
--     DROP COLUMN `transferencia_id`;
--
--   Nota: los largos tambien difieren del script del repo
--   (accion varchar(100) vs 32, asunto varchar(500) vs 200,
--    destinatarios text vs longtext, estado varchar(50) vs 32).
--   Son MAS grandes que lo que pide el modelo, asi que no rompen nada.
-- ---------------------------------------------------------------------
