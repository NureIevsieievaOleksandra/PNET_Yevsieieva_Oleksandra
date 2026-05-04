using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models;

public class Screening
{
    public int Screening_ID { get; set; }

    [Required(ErrorMessage = "Фільм обов'язковий")]
    [Display(Name = "Фільм")]
    public int Film_ID { get; set; }

    [Required(ErrorMessage = "Поле обов'язкове")]
    [StringLength(20, ErrorMessage = "Максимум 20 символів")]
    [Display(Name = "Зал")]
    public string? Hall { get; set; }

    [Required(ErrorMessage = "Час початку обов'язковий")]
    [Display(Name = "Час початку")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Ціна квитка обов'язкова")]
    [Range(0.01, 10000, ErrorMessage = "Ціна має бути від 0.01 до 10000")]
    [Display(Name = "Ціна квитка (грн)")]
    public double TicketPrice { get; set; }

    [Range(1, 1000, ErrorMessage = "Кількість місць від 1 до 1000")]
    [Display(Name = "Всього місць")]
    public int TotalSeats { get; set; } = 100;

    [ForeignKey("Film_ID")]
    public Film? Film { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
