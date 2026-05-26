-- Auditoria previa:
-- Devuelve los grupos de certificados que colisionan por NRO_CERT + HBA.
SELECT
    COALESCE(TRIM(NRO_CERT), '') AS NRO_CERT,
    COALESCE(TRIM(HBA), '') AS HBA,
    COUNT(*) AS total,
    GROUP_CONCAT(id ORDER BY COALESCE(FCH_USU, '1900-01-01') DESC, id DESC) AS ids
FROM CERTIFSEMEN
GROUP BY COALESCE(TRIM(NRO_CERT), ''), COALESCE(TRIM(HBA), '')
HAVING COUNT(*) > 1;

-- Limpieza:
-- Conserva el registro mas nuevo por FCH_USU y, si empata, el de mayor Id.
DELETE c
FROM CERTIFSEMEN c
INNER JOIN (
    SELECT id
    FROM (
        SELECT
            id,
            ROW_NUMBER() OVER (
                PARTITION BY COALESCE(TRIM(NRO_CERT), ''), COALESCE(TRIM(HBA), '')
                ORDER BY COALESCE(FCH_USU, '1900-01-01') DESC, id DESC
            ) AS rn
        FROM CERTIFSEMEN
    ) ranked
    WHERE ranked.rn > 1
) duplicates ON duplicates.id = c.id;

-- Restriccion de base: no permitir volver a crear duplicados por NRO_CERT + HBA.
ALTER TABLE CERTIFSEMEN
    ADD UNIQUE INDEX UX_CERTIFSEMEN_NRO_CERT_HBA (NRO_CERT, HBA);
