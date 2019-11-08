using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class DuenioEvento
    {
        [Key]
        public int IdDuenioEvento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public byte EstadoDuenio { get; set; }

    }
}
