using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class FormDetalleVenta : Form
    {
        public FormDetalleVenta()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CapaNegocio_Venta().ObtenerVenta(textBusqueda.Text);

            if (oVenta.idVenta != 0)
            {
                textNumeroDocumento.Text = oVenta.numeroDocumento;

                textFecha.Text = oVenta.fechaRegistro;
                textTipoDocumento.Text = oVenta.tipoDocumento;
                textUsuario.Text = oVenta.oUsuario.apellido + " " + oVenta.oUsuario.nombre;

                textDocCliente.Text = oVenta.documentoCliente;
                textNombreCliente.Text = oVenta.nombreCliente;

                dgvData.Rows.Clear();
                foreach (Detalle_Venta dv in oVenta.oDetalle_Venta)
                {
                    dgvData.Rows.Add(new object[] { dv.oProducto.nombre, dv.precioVenta, dv.cantidad, dv.subTotal });
                }

                textMontoTotal.Text = oVenta.montoTotal.ToString("0.00");
                textMontoPago.Text = oVenta.montoPago.ToString("0.00");
                textMontoCambio.Text = oVenta.montoCambio.ToString("0.00");
            }
        }
    }
}
