USE Cinema;
GO

IF OBJECT_ID('dbo.fn_EveryIthFilmSales', 'TF') IS NOT NULL
    DROP FUNCTION dbo.fn_EveryIthFilmSales;
GO

CREATE FUNCTION dbo.fn_EveryIthFilmSales
(
    @StudioName VARCHAR(50)
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        f.Title         AS FilmTitle,
        f.Genre         AS Genre,
        s.Hall          AS Hall,
        s.StartTime     AS StartTime,
        s.TicketPrice   AS TicketPrice,
        t.Ticket_ID     AS Ticket_ID,
        t.Quantity      AS Quantity,
        t.PurchaseDate  AS PurchaseDate
    FROM Tickets t
    INNER JOIN Screenings s ON t.Screening_ID = s.Screening_ID
    INNER JOIN Films f      ON s.Film_ID = f.Film_ID
    INNER JOIN Studios st   ON f.Studio_ID = st.Studio_ID
    WHERE st.Name = @StudioName
      AND f.Film_ID IN (
            SELECT Film_ID
            FROM (
                SELECT
                    Film_ID,
                    ROW_NUMBER() OVER (ORDER BY Film_ID ASC) AS RowNum
                FROM Films f2
                INNER JOIN Studios st2 ON f2.Studio_ID = st2.Studio_ID
                WHERE st2.Name = @StudioName
            ) numbered
            WHERE RowNum % 7 = 0
      )
);
GO


SELECT * FROM dbo.fn_EveryIthFilmSales('Warner Bros');
SELECT * FROM dbo.fn_EveryIthFilmSales('Назва якої немає');