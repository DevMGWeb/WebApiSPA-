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
    [Route("api/sucursales")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class SucursalesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public SucursalesController(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<SucursalDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Sucursales.AsQueryable();
            await HttpContext.InsertarParametrosEnCabecera(queryable);
            var sucursales = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<SucursalDTO>>(sucursales);
        }

        [HttpGet("GetCombo")]
        public async Task<ActionResult<List<SucursalDTO>>> GetCombo()
        {
            var sucursales = await context.Sucursales
                .OrderBy(x => x.Nombre).ToListAsync();

            return mapper.Map<List<SucursalDTO>>(sucursales);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<SucursalDTO>> GetById(int Id)
        {
            var sucursal = await this.context.Sucursales.FirstOrDefaultAsync(x => x.Id == Id);

            if(sucursal == null)
            {
                return NotFound();
            }

            return mapper.Map<SucursalDTO>(sucursal);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SucursalCreacionDTO sucursalCreacionDTO)
        {
            var sucursal = mapper.Map<Sucursal>(sucursalCreacionDTO);

            context.Add(sucursal);
            await this.context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] SucursalCreacionDTO sucursalCreacionDTO)
        {
            var sucursal = await context.Sucursales.FirstOrDefaultAsync(x => x.Id == Id);

            if(sucursal == null)
            {
                return NotFound();
            }

            sucursal = mapper.Map(sucursalCreacionDTO, sucursal);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var existe = await context.Sucursales.AnyAsync(x => x.Id == Id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Sucursal() { Id = Id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
