using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models;

public class TicketsLog
{
    public int Log_ID { get; set; }

    [Required]
    public int Ticket_ID { get; set; }

    public DateTime ModifyDate { get; set; } = DateTime.Now;

    [Required]
    [StringLength(10)]
    public string Action { get; set; } = string.Empty;
}
