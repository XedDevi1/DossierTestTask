using AutoMapper;
using MainTestTask.Dto;
using MainTestTask.Models;
using System.Data;

namespace MainTestTask.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Dossier, DossierDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.SectionCode, opt => opt.MapFrom(src => src.SectionCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children));
        }
    }
}
