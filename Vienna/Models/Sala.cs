using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class Sala
    {
        [Key]
        public int IdSala { get; set; }
        public int CapacidadMaxima { get; set; }
        public string Nombre { get; set; }
        public byte Disponibilidad { get; set; }
        public byte EstadoSala { get; set; }

    }
}
