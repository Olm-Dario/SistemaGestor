using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaNegocio_Permiso
    {

        private CapaDato_Permisos objCD_permiso = new CapaDato_Permisos();

        public List<Permiso> listar(int idUsuario)
        {
            return objCD_permiso.listar(idUsuario);
        }

    }
}
