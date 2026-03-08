namespace TodoApp.Domain;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } // Le ? signifie qu'elle peut être vide (null)
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}