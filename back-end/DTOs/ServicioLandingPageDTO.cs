using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class ServicioLandingPageDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Valor { get; set; }
        public string Descripcion { get; set; }
        public int TipoServicioId { get; set; }
        public string Foto { get; set; }
    }
}
