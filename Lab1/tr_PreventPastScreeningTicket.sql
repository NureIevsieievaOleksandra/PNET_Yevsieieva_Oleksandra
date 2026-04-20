USE Cinema;
GO

IF OBJECT_ID('dbo.tr_PreventPastScreeningTicket', 'TR') IS NOT NULL
    DROP TRIGGER dbo.tr_PreventPastScreeningTicket;
GO

CREATE TRIGGER dbo.tr_PreventPastScreeningTicket
ON dbo.Tickets
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN Screenings s ON i.Screening_ID = s.Screening_ID
        WHERE s.StartTime < GETDATE()
    )
    BEGIN
        ROLLBACK TRANSACTION;

        RAISERROR('Неможливо придбати квиток на сеанс який вже відбувся.', 16, 1);
    END
END
GO

INSERT INTO Tickets (Screening_ID, Customer_ID, SeatNumber, PurchaseDate, Quantity, Booking_No)
VALUES (1, 1, 30, GETDATE(), 2, 4001);

