using TodoApp.Application.DTOs;
using Xunit;

namespace TodoApp.Tests;

public class ValidatorTests
{
    private readonly CreateTodoRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        // ARRANGE
        var model = new CreateTodoRequest("", "Description", DateTime.Now.AddDays(1));

        // ACT
        var result = _validator.Validate(model);

        // ASSERT
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Title");
    }
    [Fact]
    public void Should_Have_Error_When_Date_Is_In_Past()
    {
        // ARRANGE : Une date hier
        var model = new CreateTodoRequest("Titre valide", "Description", DateTime.Now.AddDays(-1));

        // ACT
        var result = _validator.Validate(model);

        // ASSERT
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage.Contains("passé"));
    }
    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Long()
    {
        // ARRANGE : Un titre de 101 caractères
        string longTitle = new string('A', 101);
        var model = new CreateTodoRequest(longTitle, "Description", DateTime.Now.AddDays(1));

        // ACT
        var result = _validator.Validate(model);

        // ASSERT
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Title");
    }
    [Fact]
    public void Should_Be_Valid_When_Description_Is_Null()
    {
        // ARRANGE : Un titre valide, mais description à NULL
        var model = new CreateTodoRequest("Titre valide", null, DateTime.Now.AddDays(1));

        // ACT
        var result = _validator.Validate(model);

        // ASSERT
        Assert.True(result.IsValid);
    }
    [Fact]
    public void Should_Be_Valid_When_DueDate_Is_Null()
    {
        // ARRANGE : Pas de date limite
        var model = new CreateTodoRequest("Titre valide", "Description", null);

        // ACT
        var result = _validator.Validate(model);

        // ASSERT
        Assert.True(result.IsValid);
    }
}