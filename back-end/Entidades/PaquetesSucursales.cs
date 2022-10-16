using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class PaquetesSucursales
    {
        public int PaqueteId { get; set; }
        public int SucursalId { get; set; }
        public Paquete Paquete { get; set; }
        public Sucursal Sucursal { get; set; }
        public double Valor { get; set; }
    }
}
