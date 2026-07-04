using AutoMapper;
using Inventory.Application.Features.Manufacturer.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Features.Manufacturer.Mappings;

public class ManufacturerMappingProfile : Profile
{
    public ManufacturerMappingProfile()
    {
        CreateMap<Domain.Entities.Manufacturer, ManufacturerDto>();
        CreateMap<Domain.Entities.Manufacturer, ManufacturerResponse>();
        CreateMap<CreateManufacturerRequest, Domain.Entities.Manufacturer>();
        CreateMap<UpdateManufacturerRequest, Domain.Entities.Manufacturer>();
    }
}
