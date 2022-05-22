using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaNegocio_Usuario
    {
        private CapaDato_Usuario objCD_usuario = new CapaDato_Usuario();

        public List<Usuario> listar()
        {
            return objCD_usuario.listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            /*REGLAS DE NEGOCIO*/

            if (obj.documento == "")
            {
                Mensaje += "Es necesario el documento del usuario\n";
            }

            if (obj.nombre == "")
            {
                Mensaje += "Es necesario el nombre del usuario\n";
            }

            if (obj.apellido == "")
            {
                Mensaje += "Es necesario el apellido del usuario\n";
            }

            if (obj.clave == "")
            {
                Mensaje += "Es necesario la clave del usuario\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objCD_usuario.Registrar(obj, out Mensaje);
            }

            
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            /*REGLAS DE NEGOCIO*/

            if (obj.documento == "")
            {
                Mensaje += "Es necesario el documento del usuario\n";
            }

            if (obj.nombre == "")
            {
                Mensaje += "Es necesario el nombre del usuario\n";
            }

            if (obj.apellido == "")
            {
                Mensaje += "Es necesario el apellido del usuario\n";
            }

            if (obj.clave == "")
            {
                Mensaje += "Es necesario la clave del usuario\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objCD_usuario.Editar(obj, out Mensaje);
            }
            
        }

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            return objCD_usuario.Eliminar(obj, out Mensaje);
        }

    }
}
