USE Cinema;
GO

IF OBJECT_ID('dbo.fn_MaxTicketsByFilm', 'TF') IS NOT NULL
    DROP FUNCTION dbo.fn_MaxTicketsByFilm;
GO

CREATE FUNCTION dbo.fn_MaxTicketsByFilm
(
    @FilmTitle VARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        f.Title          AS FilmTitle,
        s.Hall           AS Hall,
        s.StartTime      AS StartTime,
        s.TicketPrice    AS TicketPrice,
        t.Ticket_ID      AS Ticket_ID,
        t.Customer_ID    AS Customer_ID,
        t.SeatNumber     AS SeatNumber,
        t.PurchaseDate   AS PurchaseDate,
        t.Quantity       AS Quantity
    FROM Tickets t
    INNER JOIN Screenings s ON t.Screening_ID = s.Screening_ID
    INNER JOIN Films f      ON s.Film_ID = f.Film_ID
    WHERE f.Title = @FilmTitle
      AND t.Quantity = (
            SELECT MAX(t2.Quantity)
            FROM Tickets t2
            INNER JOIN Screenings s2 ON t2.Screening_ID = s2.Screening_ID
            INNER JOIN Films f2      ON s2.Film_ID = f2.Film_ID
            WHERE f2.Title = @FilmTitle
      )

    UNION ALL

    SELECT
        CASE
            WHEN NOT EXISTS (SELECT 1 FROM Films WHERE Title = @FilmTitle)
                THEN 'Відсутні замовлення фільму «' + @FilmTitle + '»'
            ELSE 'Відсутні замовлення фільму «' + @FilmTitle + '»'
        END,
        NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
    WHERE NOT EXISTS (
        SELECT 1
        FROM Tickets t
        INNER JOIN Screenings s ON t.Screening_ID = s.Screening_ID
        INNER JOIN Films f      ON s.Film_ID = f.Film_ID
        WHERE f.Title = @FilmTitle
    )
);
GO


SELECT * FROM dbo.fn_MaxTicketsByFilm('Inception');
SELECT * FROM dbo.fn_MaxTicketsByFilm('Назва якої немає');