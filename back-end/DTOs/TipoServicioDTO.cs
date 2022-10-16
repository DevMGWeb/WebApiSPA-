using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class TipoServicioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Poster { get; set; }
        public bool MostrarEnElMenuPrincipal { get; set; }
        public bool Estado { get; set; }
    }
}
