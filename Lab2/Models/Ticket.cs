using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models;

public class Ticket
{
    public int Ticket_ID { get; set; }

    [Required(ErrorMessage = "Сеанс обов'язковий")]
    [Display(Name = "Сеанс")]
    public int Screening_ID { get; set; }

    [Required(ErrorMessage = "Клієнт обов'язковий")]
    [Display(Name = "Клієнт")]
    public int Customer_ID { get; set; }

    [Required(ErrorMessage = "Поле обов'язкове")]
    [Range(1, 1000, ErrorMessage = "Номер місця від 1 до 1000")]
    [Display(Name = "Номер місця")]
    public int? SeatNumber { get; set; }

    [Display(Name = "Номер бронювання")]
    public int? Booking_No { get; set; }

    [Required(ErrorMessage = "Дата купівлі обов'язкова")]
    [Display(Name = "Дата купівлі")]
    public DateOnly PurchaseDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "Поле обов'язкове")]
    [Range(1, 100, ErrorMessage = "Кількість від 1 до 100")]
    [Display(Name = "Кількість")]
    public int Quantity { get; set; } = 1;

    [ForeignKey("Screening_ID")]
    public Screening? Screening { get; set; }

    [ForeignKey("Customer_ID")]
    public Customer? Customer { get; set; }
}
