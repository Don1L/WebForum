using System.ComponentModel.DataAnnotations;
namespace WebForum.ViewModels;

public class CreateThreadViewModel
{
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;
    
    // Если же хотим добавить текст первого поста при создании треда
    [Required]
    [StringLength(1000, ErrorMessage = "The {0} must be at max {1} characters long.", MinimumLength = 1000)]
    public string Text { get; set; } = string.Empty;
}