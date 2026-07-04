using AutoMapper;
using Inventory.Application.Features.Category.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Features.Category.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Domain.Entities.Category, CategoryDto>();
        CreateMap<Domain.Entities.Category, CategoryResponse>();
        CreateMap<CreateCategoryRequest, Domain.Entities.Category>();
        CreateMap<UpdateCategoryRequest, Domain.Entities.Category>();
    }
}
