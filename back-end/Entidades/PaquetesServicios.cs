using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class PaquetesServicios
    {
        public int PaqueteId { get; set; }
        public int ServicioId { get; set; }
        public Paquete Paquete { get; set; }
        public Servicio Servicio { get; set; }
    }
}
