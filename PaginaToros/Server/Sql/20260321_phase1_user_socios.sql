-- Ejecutar sobre una copia local de la base antes de probar la primera etapa
-- del refactor multi-socio por usuario.

ALTER TABLE `User`
    ADD COLUMN IF NOT EXISTS `IdentityUserId` varchar(450) NULL AFTER `Created`;

CREATE TABLE IF NOT EXISTS `user_socios` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `user_id` int(11) NOT NULL,
    `socio_id` int(11) NOT NULL,
    `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    UNIQUE KEY `UX_user_socios_user_socio` (`user_id`, `socio_id`),
    KEY `IX_user_socios_user_id` (`user_id`),
    KEY `IX_user_socios_socio_id` (`socio_id`),
    CONSTRAINT `FK_user_socios_user` FOREIGN KEY (`user_id`) REFERENCES `User` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_user_socios_socios` FOREIGN KEY (`socio_id`) REFERENCES `SOCIOS` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

INSERT INTO `AspNetRoles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`)
SELECT 'Socio', 'Socio', 'SOCIO', UUID()
WHERE NOT EXISTS (
    SELECT 1
    FROM `AspNetRoles`
    WHERE UPPER(COALESCE(`NormalizedName`, '')) = 'SOCIO'
);

INSERT INTO `user_socios` (`user_id`, `socio_id`, `created_at`)
SELECT u.`Id`, u.`SocioId`, UTC_TIMESTAMP()
FROM `User` u
LEFT JOIN `user_socios` us
    ON us.`user_id` = u.`Id`
   AND us.`socio_id` = u.`SocioId`
WHERE u.`SocioId` IS NOT NULL
  AND u.`SocioId` > 0
  AND us.`id` IS NULL;
