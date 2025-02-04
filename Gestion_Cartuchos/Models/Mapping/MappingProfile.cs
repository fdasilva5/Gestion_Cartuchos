using AutoMapper;
using Models;
using Models.DTOs;

namespace Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cartucho, CartuchoDTO>();
            CreateMap<CartuchoDTO, Cartucho>();
            CreateMap<Impresora, ImpresoraDTO>().ReverseMap(); 
            CreateMap<Modelo, ModeloDTO>();
            CreateMap<ModeloDTO, Modelo>();
            CreateMap<ImpresoraModelo, ImpresoraModeloDTO>().ReverseMap();
            CreateMap<Oficina, OficinaDTO>().ReverseMap();
            CreateMap<Asignar_Impresora, Asignar_Impresora_DTO>().ReverseMap();
            CreateMap<Recargas, RecargasDTO>().ReverseMap();
            CreateMap<Estado, EstadoDTO>().ReverseMap();
        }
    }
}