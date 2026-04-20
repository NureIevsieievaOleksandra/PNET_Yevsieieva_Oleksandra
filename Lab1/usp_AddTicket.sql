USE Cinema;
GO

IF OBJECT_ID('dbo.usp_AddTicket', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_AddTicket;
GO

CREATE PROCEDURE dbo.usp_AddTicket
    @FilmTitle   VARCHAR(100),
    @Booking_No  INT = NULL,
    @Quantity    INT = 1
AS
BEGIN
    DECLARE @ScreeningID INT;

    SET @ScreeningID = (
        SELECT TOP 1 s.Screening_ID
        FROM Screenings s
        INNER JOIN Films f ON s.Film_ID = f.Film_ID
        WHERE f.Title = @FilmTitle
        ORDER BY f.Title ASC, s.StartTime ASC
    );

    IF @ScreeningID IS NULL
    BEGIN
        PRINT 'Фільм «' + @FilmTitle + '» не знайдено.';
        RETURN;
    END

    IF @Booking_No IS NULL
    BEGIN
        SELECT @Booking_No = ISNULL(MAX(Booking_No), 0) + 1
        FROM Tickets;
    END

    INSERT INTO Tickets (Screening_ID, Customer_ID, SeatNumber, PurchaseDate, Quantity, Booking_No)
    VALUES (@ScreeningID, NULL, NULL, GETDATE(), @Quantity, @Booking_No);

    PRINT 'Квиток додано. Бронювання №' + CAST(@Booking_No AS VARCHAR(10)) +
          ', кількість: ' + CAST(@Quantity AS VARCHAR(10));
END
GO


EXEC dbo.usp_AddTicket @FilmTitle = 'Inception', @Booking_No = 2001, @Quantity = 3;
EXEC dbo.usp_AddTicket @FilmTitle = 'Inception';
EXEC dbo.usp_AddTicket @FilmTitle = 'Якийсь фільм';