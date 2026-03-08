using TodoApp.Domain;

namespace TodoApp.Application;

public interface ITodoService
{
    // On ajoute Task<> pour pouvoir faire un "await" dans le controller
    Task<IEnumerable<TodoItem>> GetAllAsync();

    // On doit préciser quel type d'objet la tâche va retourner : <TodoItem?>
    Task<TodoItem?> GetByIdAsync(int id);

    Task AddAsync(TodoItem item);

    // J'ajoute l'Update car tu en auras besoin pour cocher tes tâches !
    Task UpdateAsync(TodoItem item);

    Task DeleteAsync(int id);
}