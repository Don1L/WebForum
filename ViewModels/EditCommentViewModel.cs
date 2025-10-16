using System.ComponentModel.DataAnnotations;

namespace WebForum.ViewModels;

public class EditCommentViewModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(500, ErrorMessage = "The {0} must be at max {1} characters long."), MaxLength(500)]
    public string Text { get; set; } =  string.Empty;
    
    [Required]
    [StringLength(500, ErrorMessage = "The {0} must be at max {1} characters long."), MaxLength(500)]
    public string? PhotoPath { get; set; }
    
    //Id поста к которому будет написан комментарий
    public int PostId { get; set; }
    
    //Id треда в котором будет наш пост
    public int ThreadId { get; set; }
}