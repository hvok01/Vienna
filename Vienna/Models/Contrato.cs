using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }
        public byte EstadoContrato { get; set; }
        public decimal PrecioFinal { get; set; }
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }
        public byte Pagado { get; set; }
        [ForeignKey("IdEvento")]
        public Evento Evento { get; set; }
    }
}
