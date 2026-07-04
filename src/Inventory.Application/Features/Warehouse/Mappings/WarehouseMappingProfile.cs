using AutoMapper;
using Inventory.Application.Features.Warehouse.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Features.Warehouse.Mappings;

public class WarehouseMappingProfile : Profile
{
    public WarehouseMappingProfile()
    {
        CreateMap<Domain.Entities.Warehouse, WarehouseDto>();
        CreateMap<Domain.Entities.Warehouse, WarehouseResponse>();
        CreateMap<CreateWarehouseRequest, Domain.Entities.Warehouse>();
        CreateMap<UpdateWarehouseRequest, Domain.Entities.Warehouse>();
    }
}
