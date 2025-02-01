using AutoMapper;
using MongoExample.Application.DTOs;
using MongoExample.Domain.Entities;

namespace MongoExample.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
