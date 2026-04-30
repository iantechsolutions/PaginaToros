ALTER TABLE `TOROSUNI`
    ADD COLUMN IF NOT EXISTS `establecimientoId` int(10) NULL AFTER `ESTCOD`;

ALTER TABLE `TOROSUNI`
    ADD CONSTRAINT `FK_TOROSUNI_ESTABLE_establecimientoId`
    FOREIGN KEY (`establecimientoId`) REFERENCES `ESTABLE` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT;

CREATE INDEX `IX_TOROSUNI_establecimientoId`
    ON `TOROSUNI` (`establecimientoId`);

SELECT
    t.`id`,
    t.`CRIADOR`,
    s.`CODPOS2` AS `NumeroSocio`,
    s.`NOMBRE` AS `SocioNombre`,
    t.`NOM_DAD`,
    t.`HBA`,
    t.`TATPART`,
    t.`ESTCOD`
FROM `TOROSUNI` t
LEFT JOIN `SOCIOS` s ON s.`SCOD` = t.`CRIADOR`
WHERE t.`establecimientoId` IS NULL
ORDER BY s.`NOMBRE`, t.`NOM_DAD`, t.`id`;

SELECT
    s.`id` AS `SocioId`,
    s.`CODPOS2` AS `NumeroSocio`,
    s.`NOMBRE` AS `SocioNombre`,
    e.`id` AS `EstablecimientoId`,
    COALESCE(e.`NOMBRE`, 'Sin establecimiento') AS `Establecimiento`,
    COUNT(*) AS `CantidadToros`
FROM `TOROSUNI` t
LEFT JOIN `SOCIOS` s ON s.`SCOD` = t.`CRIADOR`
LEFT JOIN `ESTABLE` e ON e.`id` = t.`establecimientoId`
GROUP BY
    s.`id`,
    s.`CODPOS2`,
    s.`NOMBRE`,
    e.`id`,
    e.`NOMBRE`
ORDER BY s.`NOMBRE`, `Establecimiento`;
