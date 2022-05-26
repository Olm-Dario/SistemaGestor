using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaNegocio_Producto
    {
        private CapaDato_Producto objCD_Producto = new CapaDato_Producto();

        public List<Producto> listar()
        {
            return objCD_Producto.listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            /*REGLAS DE NEGOCIO*/

            if (obj.codigo == "")
            {
                Mensaje += "Es necesario el codifo del Producto\n";
            }

            if (obj.nombre == "")
            {
                Mensaje += "Es necesario el nombre del Producto\n";
            }

            if (obj.descripcion == "")
            {
                Mensaje += "Es necesario la descripcion del Producto\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objCD_Producto.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            /*REGLAS DE NEGOCIO*/

            if (obj.codigo == "")
            {
                Mensaje += "Es necesario el codifo del Producto\n";
            }

            if (obj.nombre == "")
            {
                Mensaje += "Es necesario el nombre del Producto\n";
            }

            if (obj.descripcion == "")
            {
                Mensaje += "Es necesario la descripcion del Producto\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objCD_Producto.Editar(obj, out Mensaje);
            }

        }

        public bool Eliminar(Producto obj, out string Mensaje)
        {
            return objCD_Producto.Eliminar(obj, out Mensaje);
        }
    }
}
