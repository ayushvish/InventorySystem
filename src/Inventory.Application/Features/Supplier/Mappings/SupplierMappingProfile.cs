using AutoMapper;
using Inventory.Application.Features.Supplier.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Features.Supplier.Mappings;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Domain.Entities.Supplier, SupplierDto>();
        CreateMap<Domain.Entities.Supplier, SupplierResponse>();
        CreateMap<CreateSupplierRequest, Domain.Entities.Supplier>();
        CreateMap<UpdateSupplierRequest, Domain.Entities.Supplier>();
    }
}
