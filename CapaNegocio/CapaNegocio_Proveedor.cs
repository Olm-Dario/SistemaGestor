using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaNegocio_Proveedor
    {

        private CapaDato_Proveedor objCD_Proveedor = new CapaDato_Proveedor();

        public List<Proveedor> listar()
        {
            return objCD_Proveedor.listar();
        }

        public int Registrar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            /*REGLAS DE NEGOCIO*/

            if (obj.documento == "")
            {
                Mensaje += "Es necesario el documento del Proveedor\n";
            }

            if (obj.razonSocial == "")
            {
                Mensaje += "Es necesario la razon social del Proveedor\n";
            }

            if (obj.correo == "")
            {
                Mensaje += "Es necesario el correo del Proveedor\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objCD_Proveedor.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            /*REGLAS DE NEGOCIO*/

            if (obj.documento == "")
            {
                Mensaje += "Es necesario el documento del Proveedor\n";
            }

            if (obj.razonSocial == "")
            {
                Mensaje += "Es necesario la razon social del Proveedor\n";
            }

            if (obj.correo == "")
            {
                Mensaje += "Es necesario el correo del Proveedor\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objCD_Proveedor.Editar(obj, out Mensaje);
            }

        }

        public bool Eliminar(Proveedor obj, out string Mensaje)
        {
            return objCD_Proveedor.Eliminar(obj, out Mensaje);
        }

    }
}
