ALTER TABLE `TRANSAN`
    ADD COLUMN IF NOT EXISTS `PLANT_ORIGEN_ID` INT(11) NULL AFTER `NVO_PLA`,
    ADD COLUMN IF NOT EXISTS `PLANT_DESTINO_ID` INT(11) NULL AFTER `PLANT_ORIGEN_ID`,
    ADD COLUMN IF NOT EXISTS `PLANT_ORIGEN_CODIGO` VARCHAR(20) NULL AFTER `PLANT_DESTINO_ID`,
    ADD COLUMN IF NOT EXISTS `PLANT_ORIGEN_ANIOEX` VARCHAR(4) NULL AFTER `PLANT_ORIGEN_CODIGO`,
    ADD COLUMN IF NOT EXISTS `PLANT_DESTINO_CODIGO` VARCHAR(20) NULL AFTER `PLANT_ORIGEN_ANIOEX`,
    ADD COLUMN IF NOT EXISTS `PLANT_DESTINO_ANIOEX` VARCHAR(4) NULL AFTER `PLANT_DESTINO_CODIGO`;

CREATE TABLE IF NOT EXISTS `TRANSAN_MAIL_OUTBOX` (
    `id` INT(11) NOT NULL AUTO_INCREMENT,
    `transan_id` INT(11) NOT NULL,
    `accion` VARCHAR(32) NOT NULL,
    `asunto` VARCHAR(200) NOT NULL,
    `cuerpo_html` LONGTEXT NOT NULL,
    `destinatarios` LONGTEXT NOT NULL,
    `mail_vendedor` VARCHAR(255) NULL,
    `mail_comprador` VARCHAR(255) NULL,
    `estado` VARCHAR(32) NOT NULL DEFAULT 'Pendiente',
    `intentos` INT(11) NOT NULL DEFAULT 0,
    `ultimo_error` LONGTEXT NULL,
    `fecha_creacion` DATETIME NOT NULL,
    `ultimo_intento` DATETIME NULL,
    `fecha_envio` DATETIME NULL,
    `proximo_intento` DATETIME NULL,
    `plantel_origen_id` INT(11) NULL,
    `plantel_destino_id` INT(11) NULL,
    `plantel_origen_codigo` VARCHAR(20) NULL,
    `plantel_destino_codigo` VARCHAR(20) NULL,
    PRIMARY KEY (`id`),
    KEY `idx_transan_mail_outbox_estado` (`estado`),
    KEY `idx_transan_mail_outbox_proximo_intento` (`proximo_intento`),
    KEY `idx_transan_mail_outbox_transan_id` (`transan_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
