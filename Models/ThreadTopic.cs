using WebForum.Models;

namespace WebForum.Models;

public class ThreadTopic
{
    public int TheadId { get; set; }
    public int TopicId { get; set; }

    public virtual Thread? Thread { get; set; }
    public virtual Topic? Topic { get; set; }
}