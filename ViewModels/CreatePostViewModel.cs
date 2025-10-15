using System.ComponentModel.DataAnnotations;

namespace WebForum.ViewModels;

public class CreatePostViewModel
{
    [Required]
    [StringLength(1000, ErrorMessage = "The {0} must be at max {1} characters long."), MaxLength(1000)]
    public string Text { get; set; } =  string.Empty;
    
    [StringLength(500, ErrorMessage = "The {0} must be at max {1} characters long."), MaxLength(1000)]
    public string? PhotoPath { get; set; }
    
    //Id треда в котором будет текст
    public int ThreadId { get; set; }
}