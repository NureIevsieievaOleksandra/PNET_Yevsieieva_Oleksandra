using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models;

public class Customer
{
    public int Customer_ID { get; set; }

    [Required(ErrorMessage = "Ім'я обов'язкове")]
    [StringLength(30, ErrorMessage = "Максимум 30 символів")]
    [Display(Name = "Ім'я")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Прізвище обов'язкове")]
    [StringLength(30, ErrorMessage = "Максимум 30 символів")]
    [Display(Name = "Прізвище")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле обов'язкове")]
    [Phone(ErrorMessage = "Неправильний формат телефону")]
    [StringLength(20, ErrorMessage = "Максимум 20 символів")]
    [Display(Name = "Телефон")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Поле обов'язкове")]
    [EmailAddress(ErrorMessage = "Неправильний формат email")]
    [StringLength(50, ErrorMessage = "Максимум 50 символів")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Повне ім'я")]
    public string FullName => $"{FirstName} {LastName}";

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
