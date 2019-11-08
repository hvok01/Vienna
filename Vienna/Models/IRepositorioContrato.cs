using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    interface IRepositorioContrato : IRepositorio<Contrato>
    {
        Contrato ObtenerPorEvento(string nombre);
    }
}
