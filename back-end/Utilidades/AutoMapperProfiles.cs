using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<SucursalCreacionDTO, Sucursal>()
              .ForMember(x => x.Ubicacion, x => x.MapFrom(dto =>
                  geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));

            CreateMap<Sucursal, SucursalDTO>()
                .ForMember(x => x.Latitud, dto => dto.MapFrom(campo => campo.Ubicacion.Y))
                .ForMember(x => x.Longitud, dto => dto.MapFrom(campo => campo.Ubicacion.X));

            CreateMap<TipoServicioCreacionDTO, TipoServicio>()
                .ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<TipoServicio, TipoServicioDTO>();

            CreateMap<ServicioCreacionDTO, Servicio>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<Servicio, ServicioDTO>();
            CreateMap<Servicio, ServicioPaqueteDTO>();

            CreateMap<TipoServicio, TipoServicioLandingPageDTO>();
            CreateMap<Servicio, ServicioLandingPageDTO>()
                .ForMember(x => x.Descripcion, options => options.MapFrom(campo => campo.Descripcion.Substring(0,120) + "..." ));

            CreateMap<PaqueteCreacionDTO, Paquete>()
                .ForMember(x => x.PaquetesServicios, options => options.MapFrom(MapearPaquetesServicios))
                .ForMember(x => x.PaquetesSucursales, options => options.MapFrom(MapearPaquetesSucursales));
 
            CreateMap<Paquete, PaqueteDTO>()
                .ForMember(x => x.Servicios, options => options.MapFrom(MapearPaquetesServiciosDTO))
                .ForMember(x => x.Sucursales, options => options.MapFrom(MapearPaquetesSucursalesDTO));

            CreateMap<IdentityUser, UsuarioDTO>()
                .ForMember(x => x.Estado, options => options.Ignore());
        }

        private object MapearPaquetesSucursalesDTO(Paquete paquete, PaqueteDTO paqueteDTO)
        {
            var resultado = new List<SucursalPaqueteDTO>();

            if(paquete.PaquetesSucursales != null)
            {
                foreach (var sucursal in paquete.PaquetesSucursales)
                {
                    resultado.Add(new SucursalPaqueteDTO()
                    {
                        Id = sucursal.SucursalId,
                        Nombre = sucursal.Sucursal.Nombre,
                        Valor = sucursal.Valor,
                    });
                }
            }

            return resultado;
        }

        private object MapearPaquetesServiciosDTO(Paquete paquete, PaqueteDTO paqueteDTO)
        {
            var resultado = new List<ServicioDTO>();

            if(paquete.PaquetesServicios != null)
            {
                foreach (var servicio in paquete.PaquetesServicios)
                {
                    resultado.Add(new ServicioDTO()
                    {
                        Id = servicio.ServicioId,
                        Nombre = servicio.Servicio.Nombre,
                        Descripcion = servicio.Servicio.Descripcion,
                        Valor = servicio.Servicio.Valor,
                        Estado = servicio.Servicio.Estado,
                        TipoServicioId = servicio.Servicio.TipoServicioId,
                    });
                }
            }

            return resultado;
        }

        private object MapearPaquetesSucursales(PaqueteCreacionDTO paqueteCreacionDTO,
            Paquete paquete)
        {
            var resultado = new List<PaquetesSucursales>();

            if(paqueteCreacionDTO.Sucursales == null)
            {
                return resultado;
            }

            foreach (var sucursal in paqueteCreacionDTO.Sucursales)
            {
                resultado.Add(new PaquetesSucursales() { SucursalId = sucursal.Id, Valor = sucursal.Valor });
            }

            return resultado;
        }

        private object MapearPaquetesServicios(PaqueteCreacionDTO paqueteCreacionDTO,
           Paquete paquete)
        {
            var resultado = new List<PaquetesServicios>();

            if(paqueteCreacionDTO.ServiciosIds == null)
            {
                return resultado;
            }

            foreach (var id in paqueteCreacionDTO.ServiciosIds)
            {
                resultado.Add(new PaquetesServicios() { ServicioId = id });
            }

            return resultado;
        }
    }
}
