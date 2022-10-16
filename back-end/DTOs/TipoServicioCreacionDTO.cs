using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class TipoServicioCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Poster { get; set; }
        public bool Estado { get; set; }
        public bool MostrarEnElMenuPrincipal { get; set; }
    }
}
