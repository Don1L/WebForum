using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebForum.Models;
using WebForum.ViewModels;
using WebForum.Data;

namespace WebForum.Controllers;

public class TopicController : Controller
{
    private readonly ApplicationDbContext _context;
    public TopicController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    //GET: Topic/
    public async Task<IActionResult> Index()
    {
        var topics = await _context.Topics.ToListAsync();
        
        return View(topics);
    }
    
    //GET: Topic/Details/
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var topic = await _context.Topics
            .Include(t =>  t.ThreadTopics)
                .ThenInclude(tt =>  tt.Thread)
                    .ThenInclude(t => t.Author)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (topic == null)
        {
            return NotFound();
        }

        //Фильтруем не удалённые треды
        topic.ThreadTopics = topic.ThreadTopics.Where(tt => !tt.Thread.IsDeleted).ToList();
        return View(topic);
    }
    
    //GET: Topic/Create/
    public IActionResult Create()
    {
        return View();
    }
    
    //POST: Topic/Create/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Content,AuthorId")] Topic topic)
    {
        if (ModelState.IsValid)
        {
            _context.Add(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(topic);
    }
    
    //GET: Topic/Edit/
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var topic = await _context.Topics.FindAsync(id);
        if (topic == null)
        {
            return NotFound();
        }
        return View(topic);
    }
    
    //POST: Topic/Edit/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Description")] Topic topic)
    {
        if (id != topic.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(topic);
                await _context.SaveChangesAsync();
            }
            catch  (DbUpdateConcurrencyException)
            {
                if (!TopicExists(topic.Id))
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
        return View(topic);
    }
    
    //GET: Topic/Delete/
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var topic = await _context.Topics
            .FirstOrDefaultAsync(m => m.Id == id);

        if (topic == null)
        {
            return NotFound();
        }
        return View(topic);
    }
    
    //POST: Topic/Delete/
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var topic = await _context.Topics.FindAsync(id);
        if (topic != null)
        {
            _context.Topics.Remove(topic);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    private bool TopicExists(int id)
    {
        return _context.Topics.Any(e => e.Id == id);
    }
}