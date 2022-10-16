using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class PaqueteCreacionDTO
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> ServiciosIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<SucursalPaqueteCreacionDTO>>))]
        public List<SucursalPaqueteCreacionDTO> Sucursales { get; set; }
    }
}
