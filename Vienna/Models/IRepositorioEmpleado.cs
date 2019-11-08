using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    interface IRepositorioEmpleado : IRepositorio<Empleado>
    {
        Empleado ObtenerPorCorreo(string correo);

        IList<Empleado> ObtenerPorNombreApellido(string nombre, string apellido);
    }
}
