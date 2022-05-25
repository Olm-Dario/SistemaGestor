using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaNegocio_Categoria
    {
        private CapaDato_Categoria objCD_Categoria = new CapaDato_Categoria();

        public List<Categoria> listar()
        {
            return objCD_Categoria.listar();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            /*REGLAS DE NEGOCIO*/
            if (obj.descripcion == "")
            {
                Mensaje += "Es necesario la descripcion de la Categoria\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objCD_Categoria.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            /*REGLAS DE NEGOCIO*/
            if (obj.descripcion == "")
            {
                Mensaje += "Es necesario la descripcion de la Categoria\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objCD_Categoria.Editar(obj, out Mensaje);
            }

        }

        public bool Eliminar(Categoria obj, out string Mensaje)
        {
            return objCD_Categoria.Eliminar(obj, out Mensaje);
        }

    }
}
