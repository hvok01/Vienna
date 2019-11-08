using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    interface IRepositorioEvento : IRepositorio<Evento>
    {
        Evento ObtenerPorNombre(string nombre);
    }
}
