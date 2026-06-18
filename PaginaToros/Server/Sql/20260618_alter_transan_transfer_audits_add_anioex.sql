ALTER TABLE `TRANSAN_TRANSFER_AUDITS`
    ADD COLUMN IF NOT EXISTS `plantel_origen_codigo` VARCHAR(20) NULL,
    ADD COLUMN IF NOT EXISTS `plantel_destino_codigo` VARCHAR(20) NULL,
    ADD COLUMN IF NOT EXISTS `plantel_origen_anioex` VARCHAR(4) NULL,
    ADD COLUMN IF NOT EXISTS `plantel_destino_anioex` VARCHAR(4) NULL;
