using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    interface IRepositorioInvitado : IRepositorio<Invitado>
    {
        Invitado ObtenerPorDni(decimal dni);
    }
}
