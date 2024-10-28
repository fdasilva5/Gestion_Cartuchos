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
            CreateMap<Impresora, ImpresoraDTO>();
            CreateMap<ImpresoraDTO, Impresora>();
            CreateMap<Modelo, ModeloDTO>();
            CreateMap<ModeloDTO, Modelo>();
            CreateMap<Oficina, OficinaDTO>();
            CreateMap<OficinaDTO, Oficina>();
            CreateMap<Asignar_Impresora, Asignar_Impresora_DTO>();
            CreateMap<Asignar_Impresora_DTO, Asignar_Impresora>();
            CreateMap<Recargas, RecargasDTO>();
            CreateMap<RecargasDTO, Recargas>();
        }
    }
}