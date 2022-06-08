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
    public partial class FormDetalleCompra : Form
    {
        public FormDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Compra oCompra = new CapaNegocio_Compra().ObtenerCompra(textBusqueda.Text);

            if (oCompra.idCompra !=0 )
            {
                textNumeroDocumento.Text = oCompra.numeroDocumento;

                textFecha.Text = oCompra.fechaRegistro;
                textTipoDocumento.Text = oCompra.tipoDocumento;
                textUsuario.Text = oCompra.oUsuario.apellido + " " + oCompra.oUsuario.nombre;
                textDocProveedor.Text = oCompra.oProveedor.documento;
                textNombreProveedor.Text = oCompra.oProveedor.razonSocial;

                dgvData.Rows.Clear();
                foreach (Detalle_Compra dc in oCompra.oDetalle_Compra)
                {
                    dgvData.Rows.Add(new object[] { dc.oProducto.nombre, dc.precioCompra, dc.cantidad, dc.montoTotal });
                }

                textMontoTotal.Text = oCompra.montoTotal.ToString("0.00");

            }
        }
    }
}
