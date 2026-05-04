using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models;

public class Film
{
    public int Film_ID { get; set; }

    [Required(ErrorMessage = "Назва фільму обов'язкова")]
    [StringLength(100, ErrorMessage = "Максимум 100 символів")]
    [Display(Name = "Назва")]
    public string Title { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "Максимум 30 символів")]
    [Display(Name = "Жанр")]
    public string? Genre { get; set; }

    [Range(1888, 2100, ErrorMessage = "Рік виходу не може бути раніше за 1888")]
    [PastYear]
    [Display(Name = "Рік виходу")]
    public int? ReleaseYear { get; set; }

    [Range(1, 600, ErrorMessage = "Тривалість має бути від 1 до 600 хвилин")]
    [Display(Name = "Тривалість (хв)")]
    public int? Duration { get; set; }

    [Range(0.0, 10.0, ErrorMessage = "Рейтинг має бути від 0 до 10")]
    [Display(Name = "Рейтинг")]
    public double? Rating { get; set; }

    [Required(ErrorMessage = "Студія обов'язкова")]
    [Display(Name = "Студія")]
    public int Studio_ID { get; set; }

    [StringLength(200, ErrorMessage = "Максимум 200 символів")]
    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [ForeignKey("Studio_ID")]
    public Studio? Studio { get; set; }

    public ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}


public class PastYearAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is int year)
        {
            int currentYear = DateTime.Now.Year;
            if (year > currentYear)
                return new ValidationResult($"Рік виходу не може бути в майбутньому (максимум {currentYear})");
            if (year < 1888)
                return new ValidationResult("Рік виходу не може бути раніше 1888");
        }
        return ValidationResult.Success;
    }
}