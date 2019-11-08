using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    interface IRepositorioEventoInvitado : IRepositorio<EventoInvitado>
    {
        List<EventoInvitado> ObtenerActivos();

        int Baja(int id, int id2);
    }
}
