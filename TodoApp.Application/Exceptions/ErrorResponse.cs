namespace TodoApp.Application.Exceptions;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; } // Utile pour le débug
}