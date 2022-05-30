using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    class CapaNegocio_Negocio
    {
        private CapaDato_Negocio objcd_negocio = new CapaDato_Negocio();

        public Negocio ObtenerDatos()
        {
            return objcd_negocio.ObtenerDatos();
        }
    }
}
