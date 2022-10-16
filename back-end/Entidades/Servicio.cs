using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Servicio
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        public double Valor { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int TipoServicioId { get; set; }
        public TipoServicio TipoServicio { get; set; }
        public List<PaquetesServicios> PaquetesServicios { get; set; }
        public string Foto { get; set; }
    }
}
