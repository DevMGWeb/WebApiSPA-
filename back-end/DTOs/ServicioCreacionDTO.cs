using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class ServicioCreacionDTO
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        public double Valor { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int TipoServicioId { get; set; }
        public IFormFile Foto { get; set; }
    }
}
