using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Customer_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Customers", x => x.Customer_ID); });

            migrationBuilder.CreateTable(
                name: "Studios",
                columns: table => new
                {
                    Studio_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Founded = table.Column<int>(type: "int", nullable: true),
                    Info = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Studios", x => x.Studio_ID); });

            migrationBuilder.CreateTable(
                name: "TicketsLog",
                columns: table => new
                {
                    Log_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticket_ID = table.Column<int>(type: "int", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Action = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_TicketsLog", x => x.Log_ID); });

            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Film_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    Studio_ID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Film_ID);
                    table.ForeignKey(name: "FK_Films_Studios_Studio_ID", column: x => x.Studio_ID,
                        principalTable: "Studios", principalColumn: "Studio_ID", onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Screenings",
                columns: table => new
                {
                    Screening_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Film_ID = table.Column<int>(type: "int", nullable: false),
                    Hall = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TicketPrice = table.Column<double>(type: "float", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false, defaultValue: 100)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenings", x => x.Screening_ID);
                    table.ForeignKey(name: "FK_Screenings_Films_Film_ID", column: x => x.Film_ID,
                        principalTable: "Films", principalColumn: "Film_ID", onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Ticket_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Screening_ID = table.Column<int>(type: "int", nullable: false),
                    Customer_ID = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: true),
                    Booking_No = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Ticket_ID);
                    table.ForeignKey(name: "FK_Tickets_Customers_Customer_ID", column: x => x.Customer_ID,
                        principalTable: "Customers", principalColumn: "Customer_ID", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(name: "FK_Tickets_Screenings_Screening_ID", column: x => x.Screening_ID,
                        principalTable: "Screenings", principalColumn: "Screening_ID", onDelete: ReferentialAction.Restrict);
                });

            // Seed data - Studios
            migrationBuilder.InsertData("Studios", new[] { "Studio_ID","Name","Country","Founded","Info" }, new object[,]
            {
                { 1, "Warner Bros", "США", 1923, null },
                { 2, "Universal", "США", 1912, null },
                { 3, "Paramount", "США", 1912, null },
                { 4, "Marvel Studios", "США", 1996, null },
                { 5, "A24", "США", 2012, null },
                { 6, "Film.UA", "Україна", 2004, null }
            });

            // Seed Films
            migrationBuilder.InsertData("Films", new[] { "Film_ID","Title","Genre","ReleaseYear","Duration","Rating","Studio_ID","Description" }, new object[,]
            {
                { 1, "Inception", "Фантастика", 2010, 148, 8.8, 1, "Крадіжка ідей уві сні" },
                { 2, "The Dark Knight", "Бойовик", 2008, 152, 9.0, 1, "Бетмен проти Джокера" },
                { 3, "Oppenheimer", "Драма", 2023, 180, 8.6, 3, "Батько атомної бомби" },
                { 4, "Everything Everywhere", "Комедія", 2022, 139, 7.8, 5, "Мультивсесвіт" },
                { 5, "Avengers Endgame", "Бойовик", 2019, 181, 8.4, 4, "Кінець епохи" },
                { 6, "Interstellar", "Фантастика", 2014, 169, 8.6, 1, "Подорож крізь червоточину" },
                { 7, "The Matrix", "Фантастика", 1999, 136, 8.7, 1, "Симуляція реальності" },
                { 8, "Joker", "Драма", 2019, 122, 8.4, 1, "Походження Джокера" },
                { 9, "Dune", "Фантастика", 2021, 155, 8.0, 1, "Пустельна планета" },
                { 10, "Aquaman", "Бойовик", 2018, 143, 6.9, 1, "Король морів" },
                { 11, "The Conjuring", "Жахи", 2013, 112, 7.5, 1, "Справжня історія" },
                { 12, "Wonka", "Комедія", 2023, 116, 7.2, 1, "Молодий Віллі Вонка" },
                { 13, "Мирослава", "Драма", 2023, 110, 7.2, 6, "Українська драма" }
            });

            // Seed Customers
            migrationBuilder.InsertData("Customers", new[] { "Customer_ID","FirstName","LastName","Phone","Email" }, new object[,]
            {
                { 1, "Олег", "Іваненко", "+380501234567", "oleg@mail.com" },
                { 2, "Марія", "Коваленко", "+380671234567", "maria@mail.com" },
                { 3, "Іван", "Петренко", "+380931234567", "ivan@mail.com" },
                { 4, "Анна", "Сидоренко", "+380661234567", "anna@mail.com" },
                { 5, "Дмитро", "Бондаренко", "+380991234567", "dmytro@mail.com" }
            });

            // Seed Screenings
            migrationBuilder.InsertData("Screenings", new[] { "Screening_ID","Film_ID","Hall","StartTime","TicketPrice","TotalSeats" }, new object[,]
            {
                { 1, 1, "Зал 1", new DateTime(2025,6,1,10,0,0), 150.0, 100 },
                { 2, 1, "Зал 2", new DateTime(2025,6,1,14,0,0), 180.0, 80 },
                { 3, 2, "Зал 1", new DateTime(2025,6,2,16,0,0), 150.0, 100 },
                { 4, 3, "Зал 3", new DateTime(2025,6,3,18,0,0), 200.0, 120 },
                { 5, 5, "Зал 2", new DateTime(2025,6,4,20,0,0), 220.0, 80 },
                { 6, 6, "Зал 1", new DateTime(2025,6,5,12,0,0), 120.0, 100 }
            });

            migrationBuilder.CreateIndex(name: "IX_Films_Studio_ID", table: "Films", column: "Studio_ID");
            migrationBuilder.CreateIndex(name: "IX_Screenings_Film_ID", table: "Screenings", column: "Film_ID");
            migrationBuilder.CreateIndex(name: "IX_Tickets_Customer_ID", table: "Tickets", column: "Customer_ID");
            migrationBuilder.CreateIndex(name: "IX_Tickets_Screening_ID", table: "Tickets", column: "Screening_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Tickets");
            migrationBuilder.DropTable(name: "TicketsLog");
            migrationBuilder.DropTable(name: "Screenings");
            migrationBuilder.DropTable(name: "Films");
            migrationBuilder.DropTable(name: "Studios");
            migrationBuilder.DropTable(name: "Customers");
        }
    }
}
