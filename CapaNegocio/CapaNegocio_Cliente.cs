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

        private readonly CapaDato_Cliente objCD_cliente = new CapaDato_Cliente();

        public List<Cliente> Listar()
        {
            return objCD_cliente.Listar();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(obj.documento == "")
            {
                Mensaje += "Es necesario el documento del cliente\n";
            }

            if(obj.nombre == "")
            {
                Mensaje += "Es necesario el nombre del cliente\n";
            }

            if (obj.apellido == "")
            {
                Mensaje += "Es necesario el apellido del cliente\n";
            }

            if (obj.correo == "")
            {
                Mensaje += "Es necesario el correo del cliente\n";
            }

            if (obj.telefono == "")
            {
                Mensaje += "Es necesario el telefono del cliente\n";
            }


            if (Mensaje!= string.Empty)
            {
                return 0;
            }
            else
            {
                return objCD_cliente.Registrar(obj, out Mensaje);
            }
        }
    }
}

