USE Cinema;
GO

DECLARE @StudioName  VARCHAR(50);
DECLARE @StudioID    INT;
DECLARE @TotalTickets INT;

DECLARE studio_cursor CURSOR FOR
    SELECT Studio_ID, Name
    FROM Studios
    ORDER BY Name ASC;

OPEN studio_cursor;

FETCH NEXT FROM studio_cursor
INTO @StudioID, @StudioName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @TotalTickets = ISNULL(SUM(t.Quantity), 0)
    FROM Tickets t
    INNER JOIN Screenings s ON t.Screening_ID = s.Screening_ID
    INNER JOIN Films f      ON s.Film_ID = f.Film_ID
    WHERE f.Studio_ID = @StudioID;

    PRINT 'Студія: ' + @StudioName +
          ' | Продано квитків: ' + CAST(@TotalTickets AS VARCHAR(10));

    FETCH NEXT FROM studio_cursor
    INTO @StudioID, @StudioName;
END

CLOSE studio_cursor;
DEALLOCATE studio_cursor;
GO