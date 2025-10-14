using System.ComponentModel.DataAnnotations;

namespace WebForum.Models;

public class Thread
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } =  string.Empty;
    
    
    public int AuthorId { get; set; }

    public virtual User? Author { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsHidden {get; set; } = false;
    
    public bool IsDeleted { get; set; } = false;
    
    
    // Инициализация связей
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<ThreadTopic> ThreadTopics { get; set; } = new List<ThreadTopic>();

}