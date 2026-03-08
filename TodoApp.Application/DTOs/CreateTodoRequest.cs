namespace TodoApp.Application.DTOs;

// Un "record" est parfait pour un DTO : c'est léger et immuable
public record CreateTodoRequest(
    string Title,
    string? Description,
    DateTime? DueDate
);