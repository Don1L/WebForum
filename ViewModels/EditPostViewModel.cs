using System.ComponentModel.DataAnnotations;

namespace WebForum.ViewModels;

public class EditPostViewModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(1000, ErrorMessage = "The {0} must be at max {1} characters long."), MaxLength(1000)]
    public string Text { get; set; } =  string.Empty;
    
    [StringLength(500, ErrorMessage = "The {0} must be at max {1} characters long."), MaxLength(1000)]
    public string? PhotoPath { get; set; }
    
    //Id треда которому принадлежит текст
    public int ThreadId { get; set; }
}