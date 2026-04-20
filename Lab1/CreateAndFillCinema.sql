USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'Cinema')
    DROP DATABASE Cinema;
GO

CREATE DATABASE Cinema;
GO

USE Cinema;
GO

CREATE TABLE Studios (
    Studio_ID INT NOT NULL IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL,
    Country VARCHAR(30) NULL,
    Founded INT NULL,
    Info VARCHAR(100) NULL,

    CONSTRAINT PK_Studios PRIMARY KEY (Studio_ID)
);
GO

CREATE TABLE Films (
    Film_ID INT NOT NULL IDENTITY(1,1),
    Title VARCHAR(100) NOT NULL,
    Genre VARCHAR(30) NULL,
    ReleaseYear INT NULL,
    Duration INT NULL,
    Rating FLOAT NULL,
    Studio_ID INT NOT NULL,
    Description NVARCHAR(200) NULL,

    CONSTRAINT PK_Films PRIMARY KEY (Film_ID),
    CONSTRAINT FK_Films_Studios
        FOREIGN KEY (Studio_ID) REFERENCES Studios(Studio_ID)
);
GO

CREATE TABLE Customers (
    Customer_ID INT NOT NULL IDENTITY(1,1),
    FirstName VARCHAR(30) NOT NULL,
    LastName VARCHAR(30) NOT NULL,
    Phone VARCHAR(20) NULL,
    Email VARCHAR(50) NULL,

    CONSTRAINT PK_Customers PRIMARY KEY (Customer_ID)
);
GO

CREATE TABLE Screenings (
    Screening_ID INT NOT NULL IDENTITY(1,1),
    Film_ID INT NOT NULL,
    Hall VARCHAR(20) NULL,
    StartTime DATETIME NOT NULL,
    TicketPrice FLOAT NOT NULL,
    TotalSeats INT NOT NULL DEFAULT 100,

    CONSTRAINT PK_Screenings PRIMARY KEY (Screening_ID),
    CONSTRAINT FK_Screenings_Films
        FOREIGN KEY (Film_ID) REFERENCES Films(Film_ID)
);
GO

CREATE TABLE Tickets (
    Ticket_ID INT NOT NULL IDENTITY(1,1),
    Screening_ID INT NOT NULL,
    Customer_ID INT NOT NULL,
    SeatNumber INT NULL,
    Booking_No INT NULL,
    PurchaseDate DATE NOT NULL DEFAULT GETDATE(),
    Quantity INT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Tickets PRIMARY KEY (Ticket_ID),
    CONSTRAINT FK_Tickets_Screenings
        FOREIGN KEY (Screening_ID) REFERENCES Screenings(Screening_ID),
    CONSTRAINT FK_Tickets_Customers
        FOREIGN KEY (Customer_ID) REFERENCES Customers(Customer_ID)
);
GO

CREATE TABLE TicketsLog (
    Log_ID INT NOT NULL IDENTITY(1,1),
    Ticket_ID INT NOT NULL,
    ModifyDate DATETIME NOT NULL DEFAULT GETDATE(),
    Action VARCHAR(10) NOT NULL,

    CONSTRAINT PK_TicketsLog PRIMARY KEY (Log_ID)
);
GO


INSERT INTO Studios (Name, Country, Founded, Info) VALUES
('Warner Bros', 'США', 1923, NULL),
('Universal', 'США', 1912, NULL),
('Paramount', 'США', 1912, NULL),
('Marvel Studios',  'США', 1996, NULL),
('A24', 'США', 2012, NULL),
('Film.UA', 'Україна', 2004, NULL);
GO

INSERT INTO Films (Title, Genre, ReleaseYear, Duration, Rating, Studio_ID, Description) VALUES
('Inception', 'Фантастика', 2010, 148, 8.8, 1, 'Крадіжка ідей уві сні'),
('The Dark Knight', 'Бойовик', 2008, 152, 9.0, 1, 'Бетмен проти Джокера'),
('Oppenheimer', 'Драма', 2023, 180, 8.6, 3, 'Батько атомної бомби'),
('Everything Everywhere', 'Комедія', 2022, 139, 7.8, 5, 'Мультивсесвіт'),
('Avengers Endgame', 'Бойовик', 2019, 181, 8.4, 4, 'Кінець епохи'),
('Interstellar', 'Фантастика', 2014, 169, 8.6, 1, 'Подорож крізь червоточину'),
('The Matrix', 'Фантастика', 1999, 136, 8.7, 1, 'Симуляція реальності'),
('Joker', 'Драма', 2019, 122, 8.4, 1, 'Походження Джокера'),
('Dune', 'Фантастика', 2021, 155, 8.0, 1, 'Пустельна планета'),
('Aquaman', 'Бойовик', 2018, 143, 6.9, 1, 'Король морів'),
('The Conjuring', 'Жахи', 2013, 112, 7.5, 1, 'Справжня історія'),
('Wonka', 'Комедія', 2023, 116, 7.2, 1, 'Молодий Віллі Вонка'),
('Мирослава', 'Драма', 2023, 110, 7.2, 6, 'Українська драма');
GO

INSERT INTO Customers (FirstName, LastName, Phone, Email) VALUES
('Олег', 'Іваненко', '+380501234567', 'oleg@mail.com'),
('Марія', 'Коваленко', '+380671234567', 'maria@mail.com'),
('Іван', 'Петренко', '+380931234567', 'ivan@mail.com'),
('Анна', 'Сидоренко', '+380661234567', 'anna@mail.com'),
('Дмитро', 'Бондаренко', '+380991234567', 'dmytro@mail.com');
GO

INSERT INTO Screenings (Film_ID, Hall, StartTime, TicketPrice, TotalSeats) VALUES
(1, 'Зал 1', '2024-03-01 10:00', 150.0, 100),
(1, 'Зал 2', '2024-03-01 14:00', 180.0, 80),
(2, 'Зал 1', '2024-03-02 16:00', 150.0, 100),
(3, 'Зал 3', '2024-03-03 18:00', 200.0, 120),
(5, 'Зал 2', '2024-03-04 20:00', 220.0, 80),
(6, 'Зал 1', '2024-03-05 12:00', 120.0, 100);
GO

INSERT INTO Tickets (Screening_ID, Customer_ID, SeatNumber, PurchaseDate, Quantity) VALUES
(1, 1, 12, '2024-03-01', 2),
(1, 2,  5, '2024-03-01', 1),
(2, 3, 20, '2024-03-01', 3),
(3, 1,  8, '2024-03-02', 1),
(4, 4, 15, '2024-03-03', 2),
(5, 5,  3, '2024-03-04', 4),
(6, 2, 10, '2024-03-05', 1);
GO