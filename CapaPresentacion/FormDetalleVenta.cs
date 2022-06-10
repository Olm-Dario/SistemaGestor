using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace CapaPresentacion
{
    public partial class FormDetalleVenta : Form
    {
        public FormDetalleVenta()
        {
            InitializeComponent();
        }
        private void FormDetalleVenta_Load(object sender, EventArgs e)
        {
            textBusqueda.Select();
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

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            textFecha.Text = "";
            textTipoDocumento.Text = "";
            textUsuario.Text = "";
            textDocCliente.Text = "";
            textNombreCliente.Text = "";

            dgvData.Rows.Clear();
            textMontoTotal.Text = "0.00";
            textMontoPago.Text = "0.00";
            textMontoCambio.Text = "0.00";
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            if (textTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string Texto_html = Properties.Resources.PlantillaVenta.ToString();
            Negocio oDatos = new CapaNegocio_Negocio().ObtenerDatos();

            Texto_html = Texto_html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Texto_html = Texto_html.Replace("@direcnegocio", oDatos.Direccion);

            Texto_html = Texto_html.Replace("@tipodocumento", textTipoDocumento.Text.ToUpper());
            Texto_html = Texto_html.Replace("@numerodocumento", textNumeroDocumento.Text);

            Texto_html = Texto_html.Replace("@doccliente", textDocCliente.Text);
            Texto_html = Texto_html.Replace("@nombrecliente", textNombreCliente.Text);
            Texto_html = Texto_html.Replace("@fecharegistro", textFecha.Text);
            Texto_html = Texto_html.Replace("@usuarioregistro", textUsuario.Text);

            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Precio"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                filas += "</tr>";
            }
            Texto_html = Texto_html.Replace("@filas", filas);
            Texto_html = Texto_html.Replace("@montototal", textMontoTotal.Text);
            Texto_html = Texto_html.Replace("@pagocon", textMontoPago.Text);
            Texto_html = Texto_html.Replace("@cambio", textMontoCambio.Text);

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("Venta_{0}.pdf", textNumeroDocumento.Text);
            saveFile.Filter = "Pdf Files|*.pdf";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    bool obtenido = true;
                    byte[] byteImage = new CapaNegocio_Negocio().ObtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }

                    using (StringReader sr = new StringReader(Texto_html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Documento Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }
    }
}
