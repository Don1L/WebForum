using System.ComponentModel.DataAnnotations;
namespace WebForum.ViewModels;

public class EditThreadViewModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;
    
    public bool IsHidden { get; set; }
    
    
}