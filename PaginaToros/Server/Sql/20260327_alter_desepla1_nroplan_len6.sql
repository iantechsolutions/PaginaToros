-- Ajusta el tamaño de NROPLAN para admitir códigos de 6 caracteres
ALTER TABLE `DESEPLA1`
    MODIFY COLUMN `NROPLAN` VARCHAR(6) NULL;
