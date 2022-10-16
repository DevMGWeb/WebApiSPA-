using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class PaqueteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public List<ServicioDTO> Servicios { get; set; }
        public List<SucursalPaqueteDTO> Sucursales { get; set; }
    }
}
