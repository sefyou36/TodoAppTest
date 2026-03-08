using Microsoft.EntityFrameworkCore;
using TodoApp.Domain;
using TodoApp.Infrastructure;
using Xunit;

namespace TodoApp.Tests;

public class TodoServiceTests
{
    private TodoDbContext GetDbContext()
    {
        // On crée un nom de base de données unique pour chaque test
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new TodoDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddItemToDatabase()
    {
        // ARRANGE
        using var context = GetDbContext();
        var service = new TodoService(context);
        var newItem = new TodoItem { Title = "Acheter du pain", Description = "Boulangerie" };

        // ACT
        await service.AddAsync(newItem);

        // ASSERT
        var items = await context.Todos.ToListAsync();
        Assert.Single(items);
        Assert.Equal("Acheter du pain", items[0].Title);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItems()
    {
        // ARRANGE
        using var context = GetDbContext();
        context.Todos.Add(new TodoItem { Title = "Task 1" });
        context.Todos.Add(new TodoItem { Title = "Task 2" });
        await context.SaveChangesAsync();

        var service = new TodoService(context);

        // ACT
        var result = await service.GetAllAsync();

        // ASSERT
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectItem()
    {
        // ARRANGE
        using var context = GetDbContext();
        var item = new TodoItem { Id = 10, Title = "Specific Task" };
        context.Todos.Add(item);
        await context.SaveChangesAsync();

        var service = new TodoService(context);

        // ACT
        var result = await service.GetByIdAsync(10);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("Specific Task", result.Title);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveItem()
    {
        // ARRANGE
        using var context = GetDbContext();
        var item = new TodoItem { Id = 1, Title = "To Delete" };
        context.Todos.Add(item);
        await context.SaveChangesAsync();

        var service = new TodoService(context);

        // ACT
        await service.DeleteAsync(1);

        // ASSERT
        var exists = await context.Todos.AnyAsync(t => t.Id == 1);
        Assert.False(exists);
    }
}