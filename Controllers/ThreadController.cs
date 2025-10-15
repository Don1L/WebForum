using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebForum.Models;
using WebForum.Data;
using WebForum.ViewModels;
using Thread = WebForum.Models.Thread;

namespace WebForum.Controllers;

public class ThreadController : Controller
{
    private readonly ApplicationDbContext _context;

    public ThreadController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET: Thread
    public async Task<IActionResult> Index()
    {
        var threads = await _context.Threads
            .Include(t => t.Author)
            .Where(t => !t.IsDeleted)
            .ToListAsync();
        
        return View(threads);
    }

    // GET: Thread/Details
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var thread = await _context.Threads
            .Include(t => t.Author)
            .Include(t=> t.ThreadTopics)
                .ThenInclude(tt=> tt.Topic)
            .Include(t=> t.Posts.Where(p => !p.IsDeleted))
                .ThenInclude(p => p.Author)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

        if (thread == null)
        {
            return NotFound();
        }
        return View(thread);
    }
    
    // GET: Thread/Create
    public IActionResult Create()
    {
        return View();
    }
    
    // POST: Thread/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Text")] CreateThreadViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            //заглушка на время
            var thread = new Thread()
            {
                Title = viewModel.Title,
                AuthorId = 1, //заглушка
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsHidden = false
            };
            
            _context.Add(thread);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }
    
    //GET: Thread/Edit
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var thread = await _context.Threads.FindAsync(id);
        if (thread == null || thread.IsDeleted)
        {
            return NotFound();
        }

        var ViewModel = new EditThreadViewModel()
        {
            Id = thread.Id,
            Title = thread.Title,
            IsHidden = thread.IsHidden,
        };
        return View(ViewModel);
    }
    
    //POST: Thread/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Title, IsHidden")] EditThreadViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var thread = await _context.Threads.FindAsync(id);
                if (thread == null || thread.IsDeleted)
                {
                    return NotFound();
                }

                thread.Title = viewModel.Title;
                thread.IsHidden = viewModel.IsHidden;

                _context.Update(thread);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadExists(viewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }
    
    //GET: Thread/Delete
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var thread = await _context.Threads
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        if (thread == null)
        {
            return NotFound();
        }
        return View(thread);
    }
    
    //POST: Thread/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var thread = await _context.Threads.FindAsync(id);
        if (thread != null)
        {
            _context.Threads.Remove(thread);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ThreadExists(int id)
    {
        return _context.Threads.Any(e => e.Id == id);
    }
}