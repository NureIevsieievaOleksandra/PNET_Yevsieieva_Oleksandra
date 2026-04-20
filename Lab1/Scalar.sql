USE Cinema;
GO

IF OBJECT_ID('dbo.fn_MostExpensiveFilmByDate', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_MostExpensiveFilmByDate;
GO

CREATE FUNCTION dbo.fn_MostExpensiveFilmByDate
(
    @Date DATE
)
RETURNS NVARCHAR(200)
AS
BEGIN
    DECLARE @Result NVARCHAR(200);

    SELECT TOP 1 @Result = f.Title
    FROM Tickets t
    INNER JOIN Screenings s ON t.Screening_ID = s.Screening_ID
    INNER JOIN Films f      ON s.Film_ID = f.Film_ID
    WHERE CAST(t.PurchaseDate AS DATE) = @Date
    ORDER BY s.TicketPrice DESC;

    IF @Result IS NULL
        SET @Result = 'Квитки не продавались ' + CONVERT(VARCHAR(10), @Date, 103);

    RETURN @Result;
END
GO

SELECT dbo.fn_MostExpensiveFilmByDate('2024-03-03') AS Result;
SELECT dbo.fn_MostExpensiveFilmByDate('2000-01-01') AS Result;