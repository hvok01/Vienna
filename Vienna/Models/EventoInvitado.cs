using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class EventoInvitado
    {
        [Key]
        public int IdEvento { get; set; }
        [Key]
        public int IdInvitado { get; set; }
        public byte EstadoRelacion { get; set; }
        public Evento Evento { get; set; }
        public Invitado Invitado { get; set; }
    }
}
