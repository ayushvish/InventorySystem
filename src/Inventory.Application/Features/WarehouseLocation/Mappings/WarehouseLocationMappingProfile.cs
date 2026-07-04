using AutoMapper;
using Inventory.Application.Features.WarehouseLocation.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Features.WarehouseLocation.Mappings;

public class WarehouseLocationMappingProfile : Profile
{
    public WarehouseLocationMappingProfile()
    {
        CreateMap<Domain.Entities.WarehouseLocation, WarehouseLocationDto>();
        CreateMap<Domain.Entities.WarehouseLocation, WarehouseLocationResponse>()
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.WarehouseName : null));

        CreateMap<CreateWarehouseLocationRequest, Domain.Entities.WarehouseLocation>();
        CreateMap<UpdateWarehouseLocationRequest, Domain.Entities.WarehouseLocation>();
    }
}
