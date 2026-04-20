USE Cinema;
GO

IF OBJECT_ID('dbo.tr_StudioSalesInfo', 'TR') IS NOT NULL
    DROP TRIGGER dbo.tr_StudioSalesInfo;
GO

CREATE TRIGGER dbo.tr_StudioSalesInfo
ON dbo.Tickets
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StudioID INT;

    SELECT @StudioID = st.Studio_ID
    FROM inserted i
    INNER JOIN Screenings s  ON i.Screening_ID = s.Screening_ID
    INNER JOIN Films f       ON s.Film_ID = f.Film_ID
    INNER JOIN Studios st    ON f.Studio_ID = st.Studio_ID;

    DECLARE @UniqueFilms INT;

    SELECT @UniqueFilms = COUNT(DISTINCT f.Film_ID)
    FROM Tickets t
    INNER JOIN Screenings s  ON t.Screening_ID = s.Screening_ID
    INNER JOIN Films f       ON s.Film_ID = f.Film_ID
    WHERE f.Studio_ID = @StudioID;

    IF @UniqueFilms = 10
    BEGIN
        UPDATE Studios
        SET Info = CASE
            WHEN Info IS NULL
                THEN 'У студії продано квитки на 10 різних фільмів'
            WHEN Info NOT LIKE '%У студії продано квитки на 10 різних фільмів%'
                THEN Info + ' | У студії продано квитки на 10 різних фільмів'
            ELSE Info
        END
        WHERE Studio_ID = @StudioID;
    END
END
GO

INSERT INTO Tickets (Screening_ID, Customer_ID, SeatNumber, PurchaseDate, Quantity, Booking_No)
VALUES (1, 1, 25, GETDATE(), 2, 3001);

SELECT Studio_ID, Name, Info FROM Studios;