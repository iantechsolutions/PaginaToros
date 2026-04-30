CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    ALTER DATABASE CHARACTER SET latin1;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `actualizacion` (
        `socios` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `torosp` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `torospr` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `establec` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `certsemen` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `centrosia` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT '',
        `solicinspecc` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `archrural` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `decserv` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `resultsol` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `inspect` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `transfsb` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `transfanim` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `transfafut` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `zonas` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `planteles` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `archrural` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `TAT` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `HBA` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `TIPO` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NOMBRE` varchar(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NAC` datetime(6) NULL,
        CONSTRAINT `PK_archrural` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AspNetRoles` (
        `Id` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `Name` varchar(256) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NormalizedName` varchar(256) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `ConcurrencyStamp` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AspNetUsers` (
        `Id` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `UserName` varchar(256) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NormalizedUserName` varchar(256) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `Email` varchar(256) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NormalizedEmail` varchar(256) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `EmailConfirmed` tinyint(1) NOT NULL,
        `PasswordHash` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `SecurityStamp` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `ConcurrencyStamp` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `PhoneNumber` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `PhoneNumberConfirmed` tinyint(1) NOT NULL,
        `TwoFactorEnabled` tinyint(1) NOT NULL,
        `LockoutEnd` datetime(6) NULL,
        `LockoutEnabled` tinyint(1) NOT NULL,
        `AccessFailedCount` int(11) NOT NULL,
        CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AUTORIZA` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NROSOL` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CANTOR` double NULL DEFAULT '0',
        `CANVPR` double NULL DEFAULT '0',
        `BASVAC` double NULL DEFAULT '0',
        `BASVAQ` double NULL DEFAULT '0',
        `PROCRE` double NULL,
        `ANIOPR` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `VACSER` double NULL,
        `ANIOSE` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FREALI` datetime NULL,
        `FAUTOR` datetime NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL,
        `nrosolanual` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT '1',
        CONSTRAINT `PK_AUTORIZA` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `CENTROSIA` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NROCEN` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `NOMBRE` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NRO_C_SAYG` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        CONSTRAINT `PK_CENTROSIA` PRIMARY KEY (`id`),
        CONSTRAINT `AK_CENTROSIA_NROCEN` UNIQUE (`NROCEN`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `DESE_USADAS` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRODEC` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CERTSEMEN` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DOSIS_USADA` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_DESE_USADAS` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `DESEPLA2` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `TATPART` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `HARDB` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NRODEC` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DESDE` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `HASTA` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `APODO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_DESEPLA2` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `DESEPLA3` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `TATPART` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `HARDB` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CANTV` double NULL,
        `NRODEC` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TIPO` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `SERVICIO` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DESDE` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `HASTA` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `APODO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_DESEPLA3` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `DESEPLA4 - No Usada` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `CTST` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CANTV` double NULL,
        `NRODEC` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_DESEPLA4 - No Usada` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `DESEPLA5 - No Usada` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRODEC` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `E_NOM` varchar(70) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_DESEPLA5 - No Usada` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `FUT_CONTROL` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRO_TRANS` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FECTRANS` datetime NULL,
        `SVEN` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CATEG_SV` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `VNOM` varchar(35) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `SCOM` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CATEG_SC` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CNOM` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `PLANTEL` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `EDAD_CRIAS` int(11) NULL,
        `CANT_HEM` int(11) NULL,
        `CANT_MACH` int(11) NULL,
        `PLANT_DEST` varchar(5) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `INCORP` tinyint(4) NULL,
        `HEMSTA` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL,
        CONSTRAINT `PK_FUT_CONTROL` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `opcdecserv` (
        `natmin` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `natdosis` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `natmax` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `natcorrmin` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `natcorrdosis` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `natcorrmax` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `iamin` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `iadosis` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `iamax` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia1min` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia1dosis` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia1max` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia2min` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia2dosis` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia2max` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia3min` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia3dosis` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ia3max` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `hedadmin` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `hedadmax` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `medadmin` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `medadmax` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `cotamin` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `cotamax` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `unidadmes` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `PEG` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRODEC` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `NROSOCIO` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `SOCIO` varchar(200) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `HBA` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `DESDE` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `HASTA` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `CVIENTRES` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `SERVICIO` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `APODO` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_PEG` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `PROVIN` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `PCOD` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `NOMBRE` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        CONSTRAINT `PK_PROVIN` PRIMARY KEY (`id`),
        CONSTRAINT `AK_PROVIN_PCOD` UNIQUE (`PCOD`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `REMANSOL` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRODEC` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `NROPLAN` varchar(5) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NROSOL` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `REMANTOROPR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `REMANVQPR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `REMANVQVIP` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `nrosolanual` int(10) NOT NULL DEFAULT '1',
        `ANIO` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DISPTOROPR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DISPVQPR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DISPVQVIP` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `AUTTOROPR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `AUTVQPR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `AUTVQVIP` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `PEDTOROPR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `PEDVQPR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `PEDVQVIP` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_REMANSOL` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `RESIN2` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `EA1` int(10) NOT NULL,
        `EA2` double NULL,
        `EA3` double NULL,
        `EA4` double NULL,
        `EA5` double NULL,
        `EA6` double NULL,
        `NRORES` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_RESIN2` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `RESIN3` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `RDVAC` double NULL,
        `RDVAQCS` double NULL,
        `RDVAQSS` double NULL,
        `RPVAC` double NULL,
        `RPVAQCS` double NULL,
        `RPVAQSS` double NULL,
        `CTOMOV` varchar(2) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `TIPMOV` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NRORES` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_RESIN3` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `RESIN4` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `PEDAD` double NULL,
        `PPESO` double NULL,
        `MEDAD` double NULL,
        `MPESO` double NULL,
        `IEDAD` double NULL,
        `IPESO` double NULL,
        `PDL` double NULL,
        `P2D` double NULL,
        `P4D` double NULL,
        `SEXO` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NRORES` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_RESIN4` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `RESIN8` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRORES` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NROPLA` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `MOTIVO_RECHAZO` int(11) NULL,
        `HEMBRAS` int(11) NULL,
        `MACHOS` int(11) NULL,
        `FCH_REALIZADA` datetime NULL,
        CONSTRAINT `PK_RESIN8` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `RESIN9` (
        `MOTIVO_RECHAZO` int(11) NOT NULL,
        `DESCRIPCION` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`MOTIVO_RECHAZO`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `retenidas` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRODEC` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `NROPLAN` varchar(5) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DESDE` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `HASTA` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FECDECL` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCHRECEPCION` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TIPSE` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `SEMEN` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CANTV` double NULL,
        `CANTB` double NULL,
        `REMBA` double NULL,
        `REMPR` double NULL,
        `SEMPROP` tinyint(1) NULL DEFAULT '0',
        `CANTOR` double NULL,
        `REMMPR` double NULL,
        `CTRLU` tinyint(1) NULL DEFAULT '0',
        `CTRLM` tinyint(1) NULL DEFAULT '0',
        `COEF_AUTO_SN` double NULL DEFAULT '0',
        `COEF_AUTO_IA` double NULL DEFAULT '0',
        `COEF_AUTO_IAR` double NULL DEFAULT '0',
        `IA_SINCRO` float NULL DEFAULT '0',
        `PASTILLAS_SINCRO` int(11) NULL DEFAULT '0',
        `FECRET` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_FOLIO` varchar(9) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `reten` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
        CONSTRAINT `PK_retenidas` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `SOLICI4` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NROSOL` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CODPLA` varchar(5) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_SOLICI4` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `SOLICI6` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NROSOL` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NRODEC` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `IND_HEM_MAC` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_SOLICI6` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `TOROSPR` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `APODO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NOMBRE` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `EST_DOC` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `RES_INSP` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `SBCOD` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TIP_TORO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TATPART` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NOM_DAD` varchar(35) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_INSC` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_TSAN` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_INSD` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FECHA` datetime NULL,
        `HBA` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `VARIEDAD` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CRIADOR` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CATEGO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `PLANTEL` varchar(5) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `ESTCOD` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_BAJA` datetime NULL,
        `ACTIVO` tinyint(1) NULL DEFAULT '0',
        `MOTIVO_BAJ` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NACIDO` datetime NULL,
        `ACTUALIZADO` tinyint(1) NULL DEFAULT '0',
        `CIRC_ESCROTAL` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CodEstado` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0',
        `Id_tipo` int(11) NULL DEFAULT '0',
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `FECING` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        CONSTRAINT `PK_TOROSPR` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `TorosRural` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `hba` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `tatpart` varchar(8) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `nombre` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `fec_nac` datetime NULL,
        `procesado` tinyint(4) NULL,
        `tip_toro` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `fch_usu` datetime NULL,
        `cod_usu` int(11) NULL,
        CONSTRAINT `PK_TorosRural` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `TOROSUNIestados` (
        `id` int(5) NOT NULL AUTO_INCREMENT,
        `CodEstado` varchar(10) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL,
        CONSTRAINT `PK_TOROSUNIestados` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `TRANSAN` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRO_CERT` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FECVTA` datetime NULL,
        `SVEN` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CATEG_SV` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `VNOM` varchar(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `SCOM` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CATEG_SC` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CNOM` varchar(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `PLANT` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NVO_PLA` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CANT_HEM` int(11) NULL,
        `CANT_MACH` int(11) NULL,
        `TIPHAC` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `HEMSTA` varchar(3) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `TIPANI` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `INCORP` tinyint(4) NULL,
        `TIPOHEM` varchar(2) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CANT_CHEM` int(11) NULL,
        `CANT_CMACH` int(11) NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL,
        CONSTRAINT `PK_TRANSAN` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `TRANSEM` (
        `NRO_CERT` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NROCEN` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FECVTA` datetime NULL,
        `NVEN` varchar(25) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NROCRI` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CATEG_SC` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `SCOD` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `TATPART` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `HBA` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NOM_DAD` varchar(35) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NR_INSC` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NR_TSAN` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NR_INSD` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NR_DOSI` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NR_DOSI_OR` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `TIP_ENV` varchar(8) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `VARIEDAD` varchar(2) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `usuarios` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `user` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `pass` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `passmd5` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `nrousu` varchar(10) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ultimavis` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ultimahora` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        CONSTRAINT `PK_usuarios` PRIMARY KEY (`id`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AspNetRoleClaims` (
        `Id` int(11) NOT NULL AUTO_INCREMENT,
        `RoleId` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ClaimType` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `ClaimValue` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
        CONSTRAINT `AspNetRoleClaims_ibfk_1` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AspNetUserClaims` (
        `Id` int(11) NOT NULL AUTO_INCREMENT,
        `UserId` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ClaimType` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `ClaimValue` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AspNetUserLogins` (
        `LoginProvider` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ProviderKey` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `ProviderDisplayName` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `UserId` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
        CONSTRAINT `AspNetUserLogins_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AspNetUserRole` (
        `UserId` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `RoleId` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        CONSTRAINT `PK_AspNetUserRole` PRIMARY KEY (`UserId`, `RoleId`),
        CONSTRAINT `FK_AspNetUserRole_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_AspNetUserRole_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AspNetUserRoles` (
        `UserId` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `RoleId` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`UserId`, `RoleId`),
        CONSTRAINT `AspNetUserRoles_ibfk_1` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `AspNetUserRoles_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `AspNetUserTokens` (
        `UserId` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `LoginProvider` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `Name` varchar(450) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `Value` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
        CONSTRAINT `AspNetUserTokens_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `INSPECT` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `ICOD` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `NOMBRE` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DIRECC` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `LOCALI` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPOS` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPRO` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TELEFO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `mail` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_INSPECT` PRIMARY KEY (`id`),
        CONSTRAINT `AK_INSPECT_ICOD` UNIQUE (`ICOD`),
        CONSTRAINT `FK_INSPECT_PROVIN_CODPRO` FOREIGN KEY (`CODPRO`) REFERENCES `PROVIN` (`PCOD`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `SOCIOS` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `SCOD` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
        `CATEGO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CUENTA` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `PRENOM` varchar(33) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NOMBRE` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `POSNOM` varchar(33) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DIRECC1` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `LOCALI1` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPOS1` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPRO1` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `ORDALF` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TELEFO1` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `mail` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CRIADOR` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DIRECC2` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `LOCALI2` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPOS2` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPRO2` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TELEFO2` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FECING` datetime NULL,
        `VTOSUS` datetime NULL,
        `ENVIO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `PLACOD` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `mailreg` varchar(101) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `diaregautog` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`id`),
        CONSTRAINT `AK_SOCIOS_SCOD` UNIQUE (`SCOD`),
        CONSTRAINT `FK_SOCIOS_PROVIN_CODPRO1` FOREIGN KEY (`CODPRO1`) REFERENCES `PROVIN` (`PCOD`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `ZONAS` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `ZCOD` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `MESES` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `INSPEC` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPRO` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `LOCALI` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `FCH_USU` datetime NULL,
        CONSTRAINT `PK_ZONAS` PRIMARY KEY (`id`),
        CONSTRAINT `AK_ZONAS_ZCOD` UNIQUE (`ZCOD`),
        CONSTRAINT `FK_ZONAS_INSPECT_INSPEC` FOREIGN KEY (`INSPEC`) REFERENCES `INSPECT` (`ICOD`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `CERTIFSEMEN` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `TIPO_CERT` varchar(9) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NRO_CONST` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NRO_CERT` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NROCEN` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FECVTA` datetime(6) NULL,
        `FCH_CONST` datetime NULL,
        `NVEN` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NROCRI` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CATEG_SC` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `SCOD` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TATPART` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `HBA` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NOM_DAD` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_INSC` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_TSAN` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_INSD` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_DOSI` int(11) NULL DEFAULT '0',
        `NR_DOSI_OR` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TIP_ENV` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `VARIEDAD` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `APODO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `Apodacion` varchar(250) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_CERTIFSEMEN` PRIMARY KEY (`id`),
        CONSTRAINT `FK_CERTIFSEMEN_CENTROSIA_NROCEN` FOREIGN KEY (`NROCEN`) REFERENCES `CENTROSIA` (`NROCEN`),
        CONSTRAINT `FK_CERTIFSEMEN_SOCIOS_NROCRI` FOREIGN KEY (`NROCRI`) REFERENCES `SOCIOS` (`SCOD`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `PLANTEL` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `PLACOD` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `ANIOEX` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `VAREDE` double NULL DEFAULT '0',
        `VQCSRD` double NULL DEFAULT '0',
        `VQSSRD` double NULL DEFAULT '0',
        `VAREPR` double NULL DEFAULT '0',
        `VQCSRP` double NULL DEFAULT '0',
        `VQSSRP` double NULL DEFAULT '0',
        `FEULTI` datetime NULL,
        `NROINS` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NROCRI` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CATEGO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `ANIOPA` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `UREIN` datetime NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `comment` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `ESTADO` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'S',
        `FECING` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_PLANTEL` PRIMARY KEY (`id`),
        CONSTRAINT `AK_PLANTEL_PLACOD` UNIQUE (`PLACOD`),
        CONSTRAINT `FK_PLANTEL_SOCIOS_NROCRI` FOREIGN KEY (`NROCRI`) REFERENCES `SOCIOS` (`SCOD`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `TOROSUNI` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `APODO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NOMBRE` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `EST_DOC` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `RES_INSP` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `SBCOD_OLD` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `SBCOD` int(50) NULL,
        `TIP_TORO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TATPART` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NOM_DAD` varchar(35) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_INSC` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_TSAN` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_INSD` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FECHA` datetime NULL,
        `HBA` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `VARIEDAD` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CRIADOR` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CATEGO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `PLANTEL` varchar(5) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `ESTCOD` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_BAJA` datetime NULL,
        `ACTIVO` tinyint(1) NULL DEFAULT '0',
        `MOTIVO_BAJ` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NACIDO` datetime NULL,
        `ACTUALIZADO` tinyint(1) NULL DEFAULT '0',
        `CIRC_ESCROTAL` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CodEstado` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0',
        `Id_tipo` int(11) NULL DEFAULT '0',
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `FECING` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `fechasba` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `pnac` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `pajudest` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `pajufinal` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `gdpostdest` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `indicedest` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `cescrot` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `otros1` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `promgrupo1` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `promgrupo2` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `gdvida` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `indicefinal` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `frame` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `otros2` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `COMENTARIO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_TOROSUNI` PRIMARY KEY (`id`),
        CONSTRAINT `FK_TOROSUNI_SOCIOS_CRIADOR` FOREIGN KEY (`CRIADOR`) REFERENCES `SOCIOS` (`SCOD`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `User` (
        `Id` int(11) NOT NULL AUTO_INCREMENT,
        `Names` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `LastNames` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `DNI` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `Phone` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `Email` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `Rol` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `Status` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `Created` datetime NOT NULL,
        `IdentityUserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `SocioId` int(10) NULL,
        CONSTRAINT `PK_User` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_User_SOCIOS_SocioId` FOREIGN KEY (`SocioId`) REFERENCES `SOCIOS` (`id`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `ESTABLE` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `ECOD` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `CODSOC` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CATEGO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NOMBRE` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DIRECC` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `LOCALI` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPOS` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODPRO` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `PLANO` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CODZON` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `ACTIVO` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `FECING` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `fechacreacion` varchar(4) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `ENCARGADO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `TEL` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        CONSTRAINT `PK_ESTABLE` PRIMARY KEY (`id`),
        CONSTRAINT `AK_ESTABLE_ECOD` UNIQUE (`ECOD`),
        CONSTRAINT `FK_ESTABLE_PROVIN_CODPRO` FOREIGN KEY (`CODPRO`) REFERENCES `PROVIN` (`PCOD`),
        CONSTRAINT `FK_ESTABLE_SOCIOS_CODSOC` FOREIGN KEY (`CODSOC`) REFERENCES `SOCIOS` (`SCOD`),
        CONSTRAINT `FK_ESTABLE_ZONAS_CODZON` FOREIGN KEY (`CODZON`) REFERENCES `ZONAS` (`ZCOD`)
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `DESEPLA1` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRODEC` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        `NROPLAN` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `DESDE` datetime NULL,
        `HASTA` datetime NULL,
        `FECDECL` datetime NULL,
        `FCHRECEPCION` datetime NULL,
        `TIPSE` varchar(5) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `SEMEN` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `CANTV` double NULL,
        `CANTB` double NULL,
        `REMBA` double NULL,
        `REMPR` double NULL,
        `SEMPROP` tinyint(1) NULL DEFAULT '0',
        `CANTOR` double NULL,
        `REMMPR` double NULL,
        `CTRLU` tinyint(1) NULL DEFAULT '0',
        `CTRLM` tinyint(1) NULL DEFAULT '0',
        `COEF_AUTO_SN` double NULL DEFAULT '0',
        `COEF_AUTO_IA` double NULL DEFAULT '0',
        `COEF_AUTO_IAR` double NULL DEFAULT '0',
        `IA_SINCRO` float NULL DEFAULT '0',
        `PASTILLAS_SINCRO` int(11) NULL DEFAULT '0',
        `FECRET` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
        `NR_FOLIO` int(9) NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL DEFAULT '0',
        `reten` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
        `edicion` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
        `NROCRI` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
        CONSTRAINT `PK_DESEPLA1` PRIMARY KEY (`id`),
        CONSTRAINT `FK_DESEPLA1_PLANTEL_NROPLAN` FOREIGN KEY (`NROPLAN`) REFERENCES `PLANTEL` (`PLACOD`),
        CONSTRAINT `FK_DESEPLA1_SOCIOS_NROCRI` FOREIGN KEY (`NROCRI`) REFERENCES `SOCIOS` (`SCOD`) ON DELETE CASCADE
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `user_socios` (
        `id` int(11) NOT NULL AUTO_INCREMENT,
        `user_id` int(11) NOT NULL,
        `socio_id` int(11) NOT NULL,
        `created_at` datetime NOT NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`id`),
        CONSTRAINT `FK_user_socios_SOCIOS_socio_id` FOREIGN KEY (`socio_id`) REFERENCES `SOCIOS` (`id`) ON DELETE CASCADE,
        CONSTRAINT `FK_user_socios_User_user_id` FOREIGN KEY (`user_id`) REFERENCES `User` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `RESIN1` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `SCOD` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '',
        `NRORES` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `NROPLA` varchar(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FREALI` datetime NULL,
        `OBSERV` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `PPAJUST` int(11) NULL,
        `EPROMEDIO` int(11) NULL,
        `EMAX` int(11) NULL,
        `EMIN` int(11) NULL,
        `TORTOT` int(11) NULL,
        `TORSB` int(11) NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL,
        `editar` int(1) NOT NULL,
        `ICOD` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
        `ESTCOD` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`id`, `SCOD`),
        CONSTRAINT `AK_RESIN1_NRORES` UNIQUE (`NRORES`),
        CONSTRAINT `FK_RESIN1_ESTABLE_ESTCOD` FOREIGN KEY (`ESTCOD`) REFERENCES `ESTABLE` (`ECOD`),
        CONSTRAINT `FK_RESIN1_SOCIOS_SCOD` FOREIGN KEY (`SCOD`) REFERENCES `SOCIOS` (`SCOD`) ON DELETE CASCADE
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `SOLICI1` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `CODEST` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `NROSOL` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FECSOL` datetime NULL,
        `LUGAR` varchar(25) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CANTOR` double NULL DEFAULT '0',
        `CANTVQ` double NULL DEFAULT '0',
        `PRODUC` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `REINSP` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CANVAC` double NULL DEFAULT '0',
        `CANVAQ` double NULL DEFAULT '0',
        `EDAD_MIN_MAC` int(11) NULL,
        `EDAD_MAX_HEM` int(11) NULL,
        `EDAD_MIN_HEM` int(11) NULL,
        `EDAD_MAX_MAC` int(11) NULL,
        `TYNCTE` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `BANCO` varchar(18) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `IMPORT` double NULL,
        `FECINS` datetime NULL,
        `CTRL1` tinyint(4) NULL,
        `CTRL2` tinyint(4) NULL,
        `FECRET` datetime NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL,
        `Anio` varchar(200) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_SOLICI1` PRIMARY KEY (`id`),
        CONSTRAINT `FK_SOLICI1_ESTABLE_CODEST` FOREIGN KEY (`CODEST`) REFERENCES `ESTABLE` (`ECOD`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `TRANSSB` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `NRO_TRANS` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
        `FECTRANS` datetime NULL,
        `NRO_ORDEN` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `SVEN` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CATEG_SV` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `VNOM` varchar(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `SCOM` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CATEG_SC` varchar(1) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CNOM` varchar(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `ECOD` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `FCH_USU` datetime NULL,
        `COD_USU` int(11) NULL,
        `TOROVENDIDO` int(11) NULL,
        CONSTRAINT `PK_TRANSSB` PRIMARY KEY (`id`),
        CONSTRAINT `FK_TRANSSB_ESTABLE_ECOD` FOREIGN KEY (`ECOD`) REFERENCES `ESTABLE` (`ECOD`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `RESIN6` (
        `id` int(10) NOT NULL AUTO_INCREMENT,
        `HDP` double NULL DEFAULT '0',
        `HDP_M` double NULL DEFAULT '0',
        `HDP_AS` double NULL DEFAULT '0',
        `HDT` double NULL DEFAULT '0',
        `HDB` double NULL DEFAULT '0',
        `HPP` double NULL DEFAULT '0',
        `HPP_M` int(11) NULL DEFAULT '0',
        `HPP_AS` int(11) NULL DEFAULT '0',
        `HPT` double NULL DEFAULT '0',
        `HPB` double NULL DEFAULT '0',
        `HGVP` double NULL DEFAULT '0',
        `HGVB` double NULL DEFAULT '0',
        `HGQP` double NULL DEFAULT '0',
        `HGQB` double NULL DEFAULT '0',
        `MCP` double NULL DEFAULT '0',
        `MCP_M` double NULL DEFAULT '0',
        `MCP_AS` double NULL DEFAULT '0',
        `MCT` double NULL DEFAULT '0',
        `MSP` double NULL DEFAULT '0',
        `MSP_M` double NULL DEFAULT '0',
        `MSP_AS` double NULL DEFAULT '0',
        `MST` double NULL DEFAULT '0',
        `MSPSB` double NULL DEFAULT '0',
        `NRORES` varchar(6) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        CONSTRAINT `PK_RESIN6` PRIMARY KEY (`id`),
        CONSTRAINT `FK_RESIN6_RESIN1_NRORES` FOREIGN KEY (`NRORES`) REFERENCES `RESIN1` (`NRORES`)
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE TABLE `SOLICI1AUX` (
        `id` int(11) NOT NULL AUTO_INCREMENT,
        `IdSoli` int(11) NOT NULL,
        `Anio` varchar(200) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
        `CANTOR` double NULL DEFAULT '0',
        `CANTVQ` double NULL DEFAULT '0',
        `CANVAC` double NULL DEFAULT '0',
        `CANVAQ` double NULL DEFAULT '0',
        CONSTRAINT `PK_SOLICI1AUX` PRIMARY KEY (`id`),
        CONSTRAINT `FK_SOLICI1AUX_SOLICI1_IdSoli` FOREIGN KEY (`IdSoli`) REFERENCES `SOLICI1` (`id`) ON DELETE CASCADE
    ) CHARACTER SET=latin1 COLLATE=latin1_swedish_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `FK_AspNetRoleClaims_AspNetRoles_RoleId` ON `AspNetRoleClaims` (`RoleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `FK_AspNetUserClaims_AspNetUserUserId` ON `AspNetUserClaims` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `UserId` ON `AspNetUserLogins` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_AspNetUserRole_RoleId` ON `AspNetUserRole` (`RoleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE UNIQUE INDEX `IX_AspNetUserRole_UserId` ON `AspNetUserRole` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `RoleId` ON `AspNetUserRoles` (`RoleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_CERTIFSEMEN_NROCEN` ON `CERTIFSEMEN` (`NROCEN`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_CERTIFSEMEN_NROCRI` ON `CERTIFSEMEN` (`NROCRI`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_DESEPLA1_NROCRI` ON `DESEPLA1` (`NROCRI`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_DESEPLA1_NROPLAN` ON `DESEPLA1` (`NROPLAN`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_ESTABLE_CODPRO` ON `ESTABLE` (`CODPRO`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_ESTABLE_CODSOC` ON `ESTABLE` (`CODSOC`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_ESTABLE_CODZON` ON `ESTABLE` (`CODZON`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_INSPECT_CODPRO` ON `INSPECT` (`CODPRO`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_PLANTEL_NROCRI` ON `PLANTEL` (`NROCRI`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_RESIN1_ESTCOD` ON `RESIN1` (`ESTCOD`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_RESIN1_SCOD` ON `RESIN1` (`SCOD`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_RESIN6_NRORES` ON `RESIN6` (`NRORES`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_SOCIOS_CODPRO1` ON `SOCIOS` (`CODPRO1`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_SOLICI1_CODEST` ON `SOLICI1` (`CODEST`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_SOLICI1AUX_IdSoli` ON `SOLICI1AUX` (`IdSoli`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_TOROSUNI_CRIADOR` ON `TOROSUNI` (`CRIADOR`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_TRANSSB_ECOD` ON `TRANSSB` (`ECOD`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_User_SocioId` ON `User` (`SocioId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_user_socios_socio_id` ON `user_socios` (`socio_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_user_socios_user_id` ON `user_socios` (`user_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE UNIQUE INDEX `UX_user_socios_user_socio` ON `user_socios` (`user_id`, `socio_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    CREATE INDEX `IX_ZONAS_INSPEC` ON `ZONAS` (`INSPEC`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260327155818_AlterDesepla1NroplanLen6') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260327155818_AlterDesepla1NroplanLen6', '6.0.7');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260429194240_AddTorosEstablecimiento') THEN

    ALTER TABLE `TOROSUNI` ADD `establecimientoId` int(10) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260429194240_AddTorosEstablecimiento') THEN

    CREATE INDEX `IX_TOROSUNI_establecimientoId` ON `TOROSUNI` (`establecimientoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260429194240_AddTorosEstablecimiento') THEN

    ALTER TABLE `TOROSUNI` ADD CONSTRAINT `FK_TOROSUNI_ESTABLE_establecimientoId` FOREIGN KEY (`establecimientoId`) REFERENCES `ESTABLE` (`id`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260429194240_AddTorosEstablecimiento') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260429194240_AddTorosEstablecimiento', '6.0.7');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

