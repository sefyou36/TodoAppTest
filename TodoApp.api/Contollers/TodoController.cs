using AutoMapper; // Ajout de AutoMapper
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application;
using TodoApp.Application.DTOs;
using TodoApp.Domain;
using Microsoft.AspNetCore.Authorization;


namespace TodoApp.api.Controllers;
//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;
    private readonly ILogger<TodoController> _logger;
    private readonly IMapper _mapper; // Ajout du Mapper

    public TodoController(ITodoService todoService, ILogger<TodoController> logger, IMapper mapper)
    {
        _todoService = todoService;
        _logger = logger;
        _mapper = mapper; // Initialisation du Mapper
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {

        _logger.LogInformation("Récupération des probléme , on teste le stash.");

        var items = await _todoService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        var item = await _todoService.GetByIdAsync(id);
        if (item == null)
        {
            _logger.LogWarning("TodoItem avec l'ID {Id} non trouvé.", id);
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateTodoRequest request)
    {
        _logger.LogInformation("Tentative de création d'une tâche : {Title}", request.Title);

        // Utilisation de AutoMapper au lieu de l'instanciation manuelle
        var newItem = _mapper.Map<TodoItem>(request);

        await _todoService.AddAsync(newItem);

        _logger.LogInformation("Tâche créée avec succès ! ID: {Id}", newItem.Id);

        return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Suppression de la tâche ID: {Id}", id);
        await _todoService.DeleteAsync(id);
        return NoContent();
    }
}