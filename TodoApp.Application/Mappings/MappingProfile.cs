using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // On autorise la transformation du DTO vers l'Entité
        CreateMap<CreateTodoRequest, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // L'ID est géré par la BDD
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false)); // Par défaut à false
    }
}