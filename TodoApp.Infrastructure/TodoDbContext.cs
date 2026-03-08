using Microsoft.EntityFrameworkCore;
using TodoApp.Domain;

namespace TodoApp.Infrastructure; // <--- Vérifie bien ce nom

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
    public DbSet<TodoItem> Todos { get; set; }
} 