using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models;

public class Studio
{
    public int Studio_ID { get; set; }

    [Required(ErrorMessage = "Назва студії обов'язкова")]
    [StringLength(50, ErrorMessage = "Максимум 50 символів")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле обов'язкове")]
    [StringLength(30, ErrorMessage = "Максимум 30 символів")]
    [Display(Name = "Країна")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "Поле обов'язкове")]
    [Range(1800, 2100, ErrorMessage = "Рік заснування має бути між 1800 і 2100")]
    [Display(Name = "Рік заснування")]
    public int? Founded { get; set; }

    [StringLength(100, ErrorMessage = "Максимум 100 символів")]
    [Display(Name = "Інформація")]
    public string? Info { get; set; }

    public ICollection<Film> Films { get; set; } = new List<Film>();
}
