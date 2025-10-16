using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebForum.Models;
using WebForum.ViewModels;
using WebForum.Data;

namespace WebForum.Controllers;

public class PostController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public PostController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    //GET: Post/Create/
    public async Task<IActionResult> Create(int? threadId)
    {
        if (threadId == null)
        {
            return NotFound();
        }

        var thread = await _context.Threads
            .FirstOrDefaultAsync(m => m.Id == threadId && !m.IsDeleted);
        if (thread == null)
        {
            return NotFound();
        }

        var viewModel = new CreatePostViewModel
        {
            ThreadId = threadId.Value
        };
        
        return View(viewModel);
    }
    
    //POST: Post/Create/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int threadId, [Bind("Text,PhotoPath")] CreatePostViewModel viewModel)
    {
        if (threadId != viewModel.ThreadId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var thread = await _context.Threads
                .FirstOrDefaultAsync(m => m.Id == threadId && !m.IsDeleted);

            if (thread == null)
            {
                ModelState.AddModelError("", "Thread not found or deleted.");
                return View(viewModel);
            }

            var post = new Post
            {
                Text = viewModel.Text,
                PhotoPath = viewModel.PhotoPath,
                AuthorId = 1, // заглушка
                ThreadId = threadId,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            
            _context.Add(post);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", "Thread", new { id = threadId });

        }
        return View(viewModel);
    }
    
    //GET: Post/Edit/
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Thread)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

        if (post == null)
        {
            return NotFound();
        }

        var viewModel = new EditPostViewModel
        {
            Id = post.Id,
            Text = post.Text,
            PhotoPath = post.PhotoPath,
            ThreadId = post.ThreadId,
        };
        return View(viewModel);
    }
    
    //POST: Post/Edit/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Text, PhotoPath")] EditPostViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.IsDeleted)
            {
                return NotFound();
            }
            
            post.Text= viewModel.Text;
            post.PhotoPath = viewModel.PhotoPath;
            
            _context.Update(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Thread", new{id = post.ThreadId});
        }

        return View(viewModel);
    }
    
    
    //GET: Post/Delete/
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Thread)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

        if (post == null)
        {
            return NotFound();
        }
        
        return View(post);
    }
    
    //POST: Post/Delete/
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            post.IsDeleted = true;
            _context.Update(post);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Thread", new { id = post.ThreadId });
    }
}