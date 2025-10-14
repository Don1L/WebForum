using System.ComponentModel.DataAnnotations;


namespace WebForum.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public bool IsDeleted { get; set; } = false;
    
    //Инициализация связей
    
    public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

}