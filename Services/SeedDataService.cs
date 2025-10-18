using Microsoft.EntityFrameworkCore;
using WebForum.Data;
using WebForum.Models;
using Thread = WebForum.Models.Thread;

namespace WebForum.Services;

public class SeedDataService
{
    private readonly ApplicationDbContext _context;
    public SeedDataService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Users.AnyAsync()) return;

        var user = new List<User>();
        for (int i = 1; i <= 50; i++)
        {
            user.Add(new User
            {
                Username = $"user{i}",
                Email = $"user{i}@email.com",
                PasswordHash = "hashed_password_placeholder"
            });
        }
        _context.Users.AddRange(user);
        await _context.SaveChangesAsync();
        
        var topics = new List<Topic>();
        string[] topicNames = { "Techology", "Programming", "Gaming", "Anime", "Music", "Art", "Politics", "Sports" };
        foreach (var name in topicNames)
        {
            topics.Add(new Topic
            {
              Name = name,
              Description = $"Post about {name.ToLower()}."
            });
        }
        _context.Topics.AddRange(topics);
        await _context.SaveChangesAsync();
        
        
        var threads = new List<Thread>();
        Random rnd = new Random();
        for (int i = 1; i < 100; i++)
        {
            threads.Add(new Thread
            {
                Title = $"Thread {i}",
                AuthorId = rnd.Next(1, user.Count + 1),
                CreatedAt = DateTime.UtcNow.AddDays(-rnd.Next(1, 30)),
                IsHidden = false,
                IsDeleted = false,
            });
        }
        _context.Threads.AddRange(threads);
        await _context.SaveChangesAsync();
        
        var threadTopics = new List<ThreadTopic>();
        foreach (var thread in threads)
        {
            int topicCount = rnd.Next();
            var randomTopic = topics.OrderBy(_ => rnd.Next()).Take(topicCount);
            foreach (var topic in randomTopic)
            {
                threadTopics.Add(new ThreadTopic
                {
                    TopicId = topic.Id,
                    ThreadId = thread.Id,
                });
            }
        }
        _context.ThreadTopics.AddRange(threadTopics);
        await _context.SaveChangesAsync();
            
        var post = new List<Post>();
        for (int i = 1; i <= 1500; i++)
        {
            post.Add(new Post
            {
                Text = $"This is post {i}. Test text text",
                AuthorId = rnd.Next(1, user.Count + 1),
                ThreadId = rnd.Next(1, threads.Count + 1),
                CreatedAt = DateTime.UtcNow.AddMinutes(-rnd.Next(1, 10000)),
                IsDeleted = false
            });
        }
        _context.Posts.AddRange(post);
        await _context.SaveChangesAsync();
            
        var comments = new List<Comment>();
        for (int i = 1; i <= 800; i++)
        {
            comments.Add(new Comment
            {
                Text = $"This is comment #{i} on a post.",
                AuthorId = rnd.Next(1, user.Count + 1),
                PostId = rnd.Next(1, post.Count + 1),
                CreatedAt = DateTime.UtcNow.AddMinutes(-rnd.Next(1, 5000)),
                IsDeleted = false
            });
        }
        _context.Comments.AddRange(comments);
        await _context.SaveChangesAsync();
    }

}