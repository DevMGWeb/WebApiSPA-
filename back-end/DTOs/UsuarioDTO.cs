using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }
    }
}
