using AutoMapper;
using Inventory.Application.Features.Unit.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Features.Unit.Mappings;

public class UnitMappingProfile : Profile
{
    public UnitMappingProfile()
    {
        CreateMap<Domain.Entities.Unit, UnitDto>();
        CreateMap<Domain.Entities.Unit, UnitResponse>();
        CreateMap<CreateUnitRequest, Domain.Entities.Unit>();
        CreateMap<UpdateUnitRequest, Domain.Entities.Unit>();
    }
}
