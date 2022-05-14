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
    }
}
