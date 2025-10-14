using System.ComponentModel.DataAnnotations;

namespace WebForum.Models;

public class Post
{
    public int Id { get; set; }
    
    [MaxLength(500)]
    public string? PhotoPath { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;
    
    public int AuthorId { get; set; }
    public int ThreadId { get; set; }

    public virtual User? Author { get; set; }
    public virtual Thread? Thread { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;
    
    //Инициализация свзяей
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}