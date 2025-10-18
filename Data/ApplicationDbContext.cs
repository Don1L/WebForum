using Microsoft.EntityFrameworkCore;
using WebForum.Models;
using Thread = WebForum.Models.Thread;

namespace WebForum.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { 
    }
    
    // Добавление DbSet'ов
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Thread> Threads { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Topic> Topics { get; set; } = null!;
    public DbSet<ThreadTopic> ThreadTopics { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Составной первыичный ключ для ThreadTopic
        modelBuilder.Entity<ThreadTopic>()
            .HasKey(tt => new{tt.ThreadId, tt.TopicId});
        
        modelBuilder.Entity<ThreadTopic>()
            .HasOne(tt => tt.Thread)
            .WithMany(t => t.ThreadTopics)
            .HasForeignKey(tt => tt.ThreadId);
        
        modelBuilder.Entity<ThreadTopic>()
            .HasOne(tt => tt.Topic)
            .WithMany(t => t.ThreadTopics)
            .HasForeignKey(tt => tt.TopicId);
    }
}