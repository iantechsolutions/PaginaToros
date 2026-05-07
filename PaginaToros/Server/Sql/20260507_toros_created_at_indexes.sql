ALTER TABLE `TOROSUNI`
    ADD COLUMN IF NOT EXISTS `createdAt` datetime NULL DEFAULT CURRENT_TIMESTAMP;

UPDATE `TOROSUNI`
SET `createdAt` = COALESCE(
    `FCH_USU`,
    `FECHA`,
    STR_TO_DATE(NULLIF(`FECING`, ''), '%Y/%m/%d'),
    STR_TO_DATE(NULLIF(`FECING`, ''), '%d/%m/%Y'),
    NOW()
)
WHERE `createdAt` IS NULL;

ALTER TABLE `TOROSUNI`
    MODIFY COLUMN `createdAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP;

SET @index_exists := (
    SELECT COUNT(1)
    FROM information_schema.statistics
    WHERE table_schema = DATABASE()
      AND table_name = 'TOROSUNI'
      AND index_name = 'IX_TOROSUNI_estado_createdAt'
);
SET @sql := IF(@index_exists = 0,
    'CREATE INDEX `IX_TOROSUNI_estado_createdAt` ON `TOROSUNI` (`CodEstado`, `createdAt`)',
    'SELECT ''IX_TOROSUNI_estado_createdAt already exists''');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @index_exists := (
    SELECT COUNT(1)
    FROM information_schema.statistics
    WHERE table_schema = DATABASE()
      AND table_name = 'TOROSUNI'
      AND index_name = 'IX_TOROSUNI_CRIADOR'
);
SET @sql := IF(@index_exists = 0,
    'CREATE INDEX `IX_TOROSUNI_CRIADOR` ON `TOROSUNI` (`CRIADOR`)',
    'SELECT ''IX_TOROSUNI_CRIADOR already exists''');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @index_exists := (
    SELECT COUNT(1)
    FROM information_schema.statistics
    WHERE table_schema = DATABASE()
      AND table_name = 'TOROSUNI'
      AND index_name = 'IX_TOROSUNI_HBA'
);
SET @sql := IF(@index_exists = 0,
    'CREATE INDEX `IX_TOROSUNI_HBA` ON `TOROSUNI` (`HBA`)',
    'SELECT ''IX_TOROSUNI_HBA already exists''');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @index_exists := (
    SELECT COUNT(1)
    FROM information_schema.statistics
    WHERE table_schema = DATABASE()
      AND table_name = 'TOROSUNI'
      AND index_name = 'IX_TOROSUNI_TATPART'
);
SET @sql := IF(@index_exists = 0,
    'CREATE INDEX `IX_TOROSUNI_TATPART` ON `TOROSUNI` (`TATPART`)',
    'SELECT ''IX_TOROSUNI_TATPART already exists''');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @index_exists := (
    SELECT COUNT(1)
    FROM information_schema.statistics
    WHERE table_schema = DATABASE()
      AND table_name = 'TOROSUNI'
      AND index_name = 'IX_TOROSUNI_NOM_DAD'
);
SET @sql := IF(@index_exists = 0,
    'CREATE INDEX `IX_TOROSUNI_NOM_DAD` ON `TOROSUNI` (`NOM_DAD`)',
    'SELECT ''IX_TOROSUNI_NOM_DAD already exists''');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @index_exists := (
    SELECT COUNT(1)
    FROM information_schema.statistics
    WHERE table_schema = DATABASE()
      AND table_name = 'TOROSUNI'
      AND index_name = 'IX_TOROSUNI_criador_establecimiento_estado'
);
SET @sql := IF(@index_exists = 0,
    'CREATE INDEX `IX_TOROSUNI_criador_establecimiento_estado` ON `TOROSUNI` (`CRIADOR`, `establecimientoId`, `CodEstado`)',
    'SELECT ''IX_TOROSUNI_criador_establecimiento_estado already exists''');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SELECT
    COUNT(*) AS total_toros,
    SUM(CASE WHEN `createdAt` IS NULL THEN 1 ELSE 0 END) AS sin_createdAt
FROM `TOROSUNI`;
