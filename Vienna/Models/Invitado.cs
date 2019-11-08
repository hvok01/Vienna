using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class Invitado
    {
        [Key]
        public int IdInvitado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public decimal Dni { get; set; }
        public byte EstadoInvitado { get; set; }
        public List<EventoInvitado> EventoInvitado{ get; set; }

    }
}
