#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace Crudelicious;
public class Dish
{

    [Key]
    public int DishID { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Chef { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "The number must be between 1 and 5.")] 
    public int Tastiness { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The number must be greater than 0.")]
    public int Calories { get; set; }

    [Required]
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}