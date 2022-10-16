using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Paquete
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public List<PaquetesServicios> PaquetesServicios { get; set; }
        public List<PaquetesSucursales> PaquetesSucursales { get; set; }
    }
}
