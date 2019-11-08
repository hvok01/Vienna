using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }
        public string Nombre { get; set; }
        public DateTime InicioEvento { get; set; }
        public DateTime FinEvento { get; set; }
        public int CantidadAsistentes { get; set; }
        public decimal PrecioEntrada { get; set; }
        public byte EstadoEvento { get; set; }
        [Display(Name = "Sala")]
        public int IdSala { get; set; }
        [Display(Name = "DuenioEvento")]
        public int IdDuenioEvento { get; set; }
        [ForeignKey("IdSala")]
        public Sala Sala { get; set; }
        [ForeignKey("IdDuenioEvento")]
        public DuenioEvento DuenioEvento { get; set; }
        public List<EventoInvitado> EventoInvitado { get; set; }

    }
}
