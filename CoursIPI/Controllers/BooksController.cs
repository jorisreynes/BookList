using CoursIPI.Models.Context;
using CoursIPI.NewFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoursIPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly DatabaseContext _context;

    public BooksController(DatabaseContext context)
    {
        _context = context;
    }

    // GET: api/BookItems
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Book>>> GetBookItems()
    {
        if (_context.Books == null)
        {
            return NotFound();
        }
        return await _context.Books.ToListAsync();
    }

    // GET: api/BookItems/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Book>> GetBookItems(long id)
    {
        if (_context.Books == null)
        {
            return NotFound();
        }
        var bookItems = await _context.Books.FindAsync(id);

        if (bookItems == null)
        {
            return NotFound();
        }

        return bookItems;
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutBookItems(long id, Book bookItems)
    {
        if (id != bookItems.Id)
        {
            return BadRequest();
        }

        _context.Entry(bookItems).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookItemsExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/BookItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Book>> PostBookItems(Book todoItems)
    {
        if (_context.Books == null)
        {
            return Problem("Entity set 'BookContext.BookItems'  is null.");
        }
        _context.Books.Add(todoItems);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBookItems", new { id = todoItems.Id }, todoItems);
    }

    // DELETE: api/BookItems/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteBookItems(long id)
    {
        if (_context.Books == null)
        {
            return NotFound();
        }
        var todoItems = await _context.Books.FindAsync(id);
        if (todoItems == null)
        {
            return NotFound();
        }

        _context.Books.Remove(todoItems);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BookItemsExists(long id)
    {
        return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
