using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public interface IRepositorioDuenio : IRepositorio<DuenioEvento>
    {
        DuenioEvento ObtenerPorCorreo(string correo);

        IList<DuenioEvento> ObtenerPorNombreApellido(string nombre, string apellido);
    }
}
