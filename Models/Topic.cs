using System.ComponentModel.DataAnnotations;

namespace WebForum.Models;

public class Topic
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    
    public virtual ICollection<ThreadTopic> ThreadTopics { get; set; } = new List<ThreadTopic>();
}