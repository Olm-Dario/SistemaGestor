using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaNegocio_Rol
    {

        private CapaDato_Rol objCD_rol = new CapaDato_Rol();

        public List<Rol> listar()
        {
            return objCD_rol.listar();
        }
    }
}
