-- Local test data sanitization for socios.
-- Use this only in a local or disposable environment.
-- It keeps the original address readable, but makes it invalid for accidental delivery.

UPDATE `SOCIOS`
SET
    `mail` = CASE
        WHEN `mail` IS NULL OR `mail` = '' THEN `mail`
        WHEN `mail` LIKE 'ZZZ%' THEN `mail`
        ELSE CONCAT('ZZZ', `mail`)
    END,
    `mailreg` = CASE
        WHEN `mailreg` IS NULL OR `mailreg` = '' THEN `mailreg`
        WHEN `mailreg` LIKE 'ZZZ%' THEN `mailreg`
        ELSE CONCAT('ZZZ', `mailreg`)
    END;

-- If you prefer to remove the emails entirely instead of prefixing them,
-- use this statement instead of the UPDATE above:
-- UPDATE `SOCIOS` SET `mail` = NULL, `mailreg` = NULL;
