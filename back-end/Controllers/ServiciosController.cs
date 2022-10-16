using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/servicios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class ServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "servicios";

        public ServiciosController(ApplicationDbContext context,
            IAlmacenadorArchivos almacenadorArchivos,
            IMapper mapper)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }

        [HttpGet("ladingPage")]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageDTO>> GetLandingPage()
        {
            var tipoServicios = await this.context.TipoServicios
                 .Include(x => x.Servicios.Where(x => x.Estado == true))
                 .Where(x => x.Estado == true 
                    && x.MostrarEnElMenuPrincipal == true
                    && x.Servicios.Count > 0)
                 .ToListAsync();

            var tipoServiciosDTO = this.mapper.Map<List<TipoServicioLandingPageDTO>>(tipoServicios);

            var landingPageList = new LandingPageDTO();
            landingPageList.TiposServicios = tipoServiciosDTO;

            return landingPageList;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServicioDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Servicios.Include(x => x.TipoServicio).AsQueryable();
            await HttpContext.InsertarParametrosEnCabecera(queryable);
            var servicios = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<ServicioDTO>>(servicios);
        }


        [HttpGet("GetCombo")]
        public async Task<ActionResult<List<ServicioPaqueteDTO>>> GetCombo()
        {
            var servicio = await context.Servicios
                .Where(x => x.Estado == true)
                .OrderBy(x => x.Nombre)
                .ToListAsync();

            if (servicio == null)
            {
                return NotFound();
            }

            return mapper.Map<List<ServicioPaqueteDTO>>(servicio);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServicioDTO>> GetById(int id)
        {
            var servicio = await context.Servicios.FirstOrDefaultAsync(x => x.Id == id);

            if (servicio == null)
            {
                return NotFound();
            }

            return mapper.Map<ServicioDTO>(servicio);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ServicioCreacionDTO servicioCreacionDTO)
        {
            var servicio = mapper.Map<Servicio>(servicioCreacionDTO);

            if (servicioCreacionDTO.Foto != null)
            {
                servicio.Foto = await almacenadorArchivos.GuardarArchivo(contenedor, servicioCreacionDTO.Foto);
            }

            this.context.Add(servicio);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ServicioCreacionDTO servicioCreacionDTO)
        {
            var servicio = await context.Servicios.FirstOrDefaultAsync(x => x.Id == id);

            if (servicio == null)
            {
                return NotFound();
            }

            servicio = mapper.Map(servicioCreacionDTO, servicio);

            if (servicioCreacionDTO.Foto != null)
            {
                servicio.Foto = await almacenadorArchivos.EditarArchivo(contenedor, servicioCreacionDTO.Foto, servicio.Foto);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("setactive/{id:int}")]
        public async Task<ActionResult> Put(int id)
        {
            var servicio = await context.Servicios.FirstOrDefaultAsync(x => x.Id == id);

            if (servicio == null)
            {
                return NotFound();
            }

            servicio.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("softdelete/{id:int}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var servicio = await context.Servicios.FirstOrDefaultAsync(x => x.Id == id);

            if (servicio == null)
            {
                return NotFound();
            }

            servicio.Estado = false;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Servicios.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Servicio() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
