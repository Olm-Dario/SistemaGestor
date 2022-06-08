using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaNegocio_Compra
    {
        private CapaDato_Compra objCD_compra = new CapaDato_Compra();

        public int ObtenerCorrelativo()
        {
            return objCD_compra.ObtenerCorrelativo();
        }

        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            return objCD_compra.Registrar(obj,DetalleCompra, out Mensaje);
        }
        
        public Compra ObtenerCompra(string numero)
        {
            Compra oCompra = objCD_compra.ObtenerCompra(numero);

            if (oCompra.idCompra != 0)
            {
                List<Detalle_Compra> oDetalleCompra = objCD_compra.ObtenerDetalleCompra(oCompra.idCompra);

                oCompra.oDetalle_Compra = oDetalleCompra;
            }
            return oCompra;
        }
    
    }
}
