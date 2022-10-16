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
    [Route("api/paquetes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class PaquetesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PaquetesController(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaqueteDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Paquetes.AsQueryable();
            await HttpContext.InsertarParametrosEnCabecera(queryable);
            var paquetes = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<PaqueteDTO>>(paquetes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaqueteDTO>> GetById(int id)
        {
            var paquete = await context.Paquetes
                .Include(x => x.PaquetesServicios).ThenInclude(x => x.Servicio)
                .Include(x => x.PaquetesSucursales).ThenInclude(x => x.Sucursal)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(paquete == null)
            {
                return NotFound();
            }

            return mapper.Map<PaqueteDTO>(paquete);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PaqueteCreacionDTO paqueteCreacionDTO)
        {
            var paquete = mapper.Map<Paquete>(paqueteCreacionDTO);

            context.Paquetes.Add(paquete);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] PaqueteCreacionDTO paqueteCreacionDTO)
        {
            var paquete = await context.Paquetes
                .Include(x => x.PaquetesServicios)
                .Include(x => x.PaquetesSucursales)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (paquete == null)
            {
                return NotFound();
            }

            paquete = mapper.Map(paqueteCreacionDTO, paquete);
            await context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("setactive/{id:int}")]
        public async Task<ActionResult> Put(int id)
        {
            var paquete = await context.Paquetes.FirstOrDefaultAsync(x => x.Id == id);

            if (paquete == null)
            {
                return NotFound();
            }

            paquete.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("softdelete/{id:int}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var paquete = await context.Paquetes.FirstOrDefaultAsync(x => x.Id == id);

            if (paquete == null)
            {
                return NotFound();
            }

            paquete.Estado = false;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var paquete = await context.Paquetes.AnyAsync(x => x.Id == id);

            if (!paquete)
            {
                return NotFound();
            }

            context.Remove(new Paquete { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
