using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class TipoServicio
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Poster { get; set; }
        public bool Estado { get; set; }
        public bool MostrarEnElMenuPrincipal { get; set; }
        public List<Servicio> Servicios { get; set; }
    }
}
