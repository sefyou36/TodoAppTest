using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using TodoApp.api.Controllers;
using TodoApp.Application;
using TodoApp.Application.DTOs;
using TodoApp.Domain;
using Xunit;

namespace TodoApp.Tests;

public class TodoControllerTests
{
    private readonly Mock<ITodoService> _serviceMock = new();
    private readonly Mock<ILogger<TodoController>> _loggerMock = new();
    private readonly Mock<IMapper> _mapperMock = new(); // On utilise un Mock au lieu d'une vraie instance
    private readonly TodoController _controller;

    public TodoControllerTests()
    {
        // On injecte le .Object du Mock
        _controller = new TodoController(_serviceMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // ARRANGE
        var request = new CreateTodoRequest("Titre", "Desc", DateTime.Now.AddDays(1));
        var todoItem = new TodoItem { Id = 1, Title = "Titre" };

        // On simule le comportement du Mapper
        _mapperMock.Setup(m => m.Map<TodoItem>(It.IsAny<CreateTodoRequest>())).Returns(todoItem);
        _serviceMock.Setup(s => s.AddAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

        // ACT
        var result = await _controller.Create(request);

        // ASSERT
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // ARRANGE
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<TodoItem>());

        // ACT
        var result = await _controller.GetAll();

        // ASSERT
        Assert.IsType<OkObjectResult>(result.Result);
    }
}