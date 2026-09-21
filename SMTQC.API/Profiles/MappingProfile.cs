using AutoMapper;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;
using SMTQC.API.Models;
using SMTQC.API.DTOs;

namespace SMTQC.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Productor
            CreateMap<Productore, ProductorDto>().ReverseMap();

            // Autorizacion
            CreateMap<Autorizacione, AutorizacionDto>()
                .ForMember(dest => dest.GeometriaWkt, 
                           opt => opt.MapFrom(src => src.Geometria != null ? src.Geometria.ToText() : null));

            CreateMap<AutorizacionCreateDto, Autorizacione>()
                .ForMember(dest => dest.Geometria, 
                           opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.GeometriaWkt) 
                               ? new WKTReader { DefaultSRID = 4326 }.Read(src.GeometriaWkt) : null))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => "Activo"));
        }
    }
}
