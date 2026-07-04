using AutoMapper;
using Inventory.Application.Features.Medicine.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Features.Medicine.Mappings;

public class MedicineMappingProfile : Profile
{
    public MedicineMappingProfile()
    {
        CreateMap<Domain.Entities.Medicine, MedicineDto>();
        CreateMap<Domain.Entities.Medicine, MedicineResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
            .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit != null ? src.Unit.UnitName : null))
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer != null ? src.Manufacturer.ManufacturerName : null));

        CreateMap<CreateMedicineRequest, Domain.Entities.Medicine>();
        CreateMap<UpdateMedicineRequest, Domain.Entities.Medicine>();
    }
}
