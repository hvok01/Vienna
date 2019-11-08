using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    interface IRepositorioSala : IRepositorio<Sala>
    {
        Sala ObtenerPorNombre(string nombre);
    }
}
