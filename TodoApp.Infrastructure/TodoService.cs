using TodoApp.Domain;
using TodoApp.Infrastructure;// On ajoute l'accès à l'infra
using Microsoft.EntityFrameworkCore;
using TodoApp.Application;

namespace TodoApp.Infrastructure;

public class TodoService : ITodoService
{
    private readonly TodoDbContext _context;

    // L'injection de dépendance : .NET donne le contexte à ton service automatiquement
    public TodoService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _context.Todos.ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task AddAsync(TodoItem item)
    {
        _context.Todos.Add(item);
        await _context.SaveChangesAsync(); // Sauvegarde réelle en base !
    }

    public async Task UpdateAsync(TodoItem item)
    {
        _context.Todos.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.Todos.FindAsync(id);
        if (item != null)
        {
            _context.Todos.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}