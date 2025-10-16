using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebForum.Models;
using WebForum.ViewModels;
using WebForum.Data;

namespace WebForum.Controllers;

public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public CommentController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    //GET: Comment/Create/
    // Создание комментрария к посту с ID= postId
    public async Task<IActionResult> Create(int? postId)
    {
        if (postId == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Thread)
            .FirstOrDefaultAsync(m => m.Id == postId && !m.IsDeleted);

        if (post == null)
        {
            return NotFound();
        }
        
        //Передача Id поста в ViewModel
        var viewModel = new CreateCommentViewModel
        {
            PostId = postId.Value,
            ThreadId = post.Thread.Id, //передаёс id треда чтобы вернуться в него после создания 
        };
        return View(viewModel);
    }
    
    //POST: Comment/Create/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int postId, [Bind("Text, PhotoPath")]CreateCommentViewModel viewModel)
    {
        if (postId != viewModel.PostId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var post = await _context.Posts
                .Include(p => p.Thread)
                .FirstOrDefaultAsync(m => m.Id == postId && !m.IsDeleted);

            if (post == null)
            {
                ModelState.AddModelError("", "Post not found or deleted.");
                return View(viewModel);
            }

            var comment = new Comment
            {
                Text = viewModel.Text,
                PhotoPath = viewModel.PhotoPath,
                AuthorId = 1, //так же пока заглушка
                PostId = postId,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            
            _context.Add(comment);
            await _context.SaveChangesAsync();

            //После создания комментария возвращаемся на страницу треда
            return RedirectToAction("Details", "Thread", new { id = post.ThreadId });
            
        }
        return View(viewModel);
    }
    
    //GET: Comment/Edit/
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments
            .Include(c => c.Post) //подгружаем пост к которому принадлежит комментрай
            .ThenInclude(p => p.Thread) //подгружаем тред, чтобы знать куда возвращаться
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

        if (comment == null)
        {
            return NotFound();
        }

        var viewModel = new EditCommentViewModel
        {
            Id = comment.Id,
            Text = comment.Text,
            PhotoPath = comment.PhotoPath,
            PostId = comment.PostId,
            ThreadId = comment.Post.Thread.Id
        };
        return View(viewModel);
    }
    
    //POST: Comment/Edit/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Text, PhotoPath")] EditCommentViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null && comment.IsDeleted)
            {
                return NotFound();
            }
            
            comment.Text =  viewModel.Text;
            comment.PhotoPath= viewModel.PhotoPath;
            
            _context.Update(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Thread", new{id = comment.Post.ThreadId});
        }
        return View(viewModel);
    }
    
    //GET: Comment/Delete/
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments
            .Include(c => c.Post)
            .ThenInclude(p => p.Thread)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

        if (comment == null)
        {
            return NotFound();
        }
        
        return View(comment);
    }
    
    //POST: Comment/Delete/
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            comment.IsDeleted = true;
            _context.Update(comment);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Thread", new { id = comment.Post.ThreadId });
    }
    
}