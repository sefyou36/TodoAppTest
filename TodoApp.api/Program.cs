using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore; // Ajoute cet import pour UseSqlServer
using TodoApp.api.Middleware;
using TodoApp.Application;
using TodoApp.Application.DTOs;
using TodoApp.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICES ---
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoRequestValidator>();
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

// CONFIGURATION SQL SERVER
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CHANGEMENT : On passe de Singleton à Scoped car on utilise une base de données
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication(); // Qui es-tu ?
app.UseAuthorization();  // As-tu le droit ?

// --- 2. PIPELINE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
// --- Ajout pour les migrations automatiques ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TodoDbContext>(); // Remplace par le nom de ton DbContext
        context.Database.Migrate();
        Console.WriteLine("Base de données migrée avec succès !");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la migration : {ex.Message}");
    }
}
// ----------------------------------------------

app.Run();