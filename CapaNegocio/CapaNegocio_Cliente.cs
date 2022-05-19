using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CapaNegocio_Cliente
    {

        private CapaDato_Cliente objCD_cliente = new CapaDato_Cliente();

        public List<Cliente> Listar()
        {
            return objCD_cliente.Listar();
        }
    }
}

