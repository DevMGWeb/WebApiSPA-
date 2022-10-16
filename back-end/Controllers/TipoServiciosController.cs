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
    [Route("api/tiposervicios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class TipoServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "tiposervicios";

        public TipoServiciosController(ApplicationDbContext context,
            IAlmacenadorArchivos almacenadorArchivos,
            IMapper mapper
            )
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoServicioDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) 
        {
            var queryable = context.TipoServicios.AsQueryable();
            await HttpContext.InsertarParametrosEnCabecera(queryable);
            var TipoServicios = queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO);
            return mapper.Map<List<TipoServicioDTO>>(TipoServicios);
        }

        [HttpGet("GetCombo")]
        public async Task<ActionResult<List<TipoServicioDTO>>> GetCombo()
        {
            var tipoServicios = await context.TipoServicios
                .Where(x => x.Estado == true)
                .OrderBy(x => x.Descripcion).ToListAsync();

            return mapper.Map<List<TipoServicioDTO>>(tipoServicios);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TipoServicioDTO>> GetById(int id)
        {
            var tipoServicio = await context.TipoServicios.FirstOrDefaultAsync(x => x.Id == id);
            
            if(tipoServicio == null)
            {
                return NotFound();
            }

            return mapper.Map<TipoServicioDTO>(tipoServicio);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] TipoServicioCreacionDTO tipoServicioCreacionDTO)
        {
            var tipoServicio = mapper.Map<TipoServicio>(tipoServicioCreacionDTO);

            if (tipoServicioCreacionDTO.Poster != null)
            {
                tipoServicio.Poster = await almacenadorArchivos.GuardarArchivo(contenedor, tipoServicioCreacionDTO.Poster);
            }

            context.Add(tipoServicio);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] TipoServicioCreacionDTO tipoServicioCreacionDTO)
        {
            var tipoServicio = await context.TipoServicios.FirstOrDefaultAsync(x => x.Id == id);

            if(tipoServicio == null)
            {
                return NotFound();
            }

            tipoServicio = mapper.Map(tipoServicioCreacionDTO, tipoServicio);

            if (tipoServicioCreacionDTO.Poster != null)
            {
                tipoServicio.Poster = await almacenadorArchivos.EditarArchivo(contenedor, tipoServicioCreacionDTO.Poster, tipoServicio.Poster);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("setactive/{id:int}")]
        public async Task<ActionResult> PutActive(int id)
        {
            var tipoServicio = await context.TipoServicios.FirstOrDefaultAsync(x => x.Id == id);

            if (tipoServicio == null)
            {
                return NotFound();
            }

            tipoServicio.Estado = true;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("softdelete/{id:int}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var tipoServicio = await context.TipoServicios.FirstOrDefaultAsync(x => x.Id == id);

            if (tipoServicio == null)
            {
                return NotFound();
            }

            tipoServicio.Estado = false;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.TipoServicios.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new TipoServicio() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
