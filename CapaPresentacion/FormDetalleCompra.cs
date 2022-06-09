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

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            textFecha.Text = "";
            textTipoDocumento.Text = "";
            textUsuario.Text = "";
            textDocProveedor.Text = "";
            textNombreProveedor.Text = "";

            dgvData.Rows.Clear();
            textMontoTotal.Text = "0.00";
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            if (textTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string Texto_html = Properties.Resources.PlantillaCompra.ToString();
            Negocio oDatos = new CapaNegocio_Negocio().ObtenerDatos();

            Texto_html = Texto_html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Texto_html = Texto_html.Replace("@direcnegocio", oDatos.Direccion);

            Texto_html = Texto_html.Replace("@tipodocumento", textTipoDocumento.Text.ToUpper());
            Texto_html = Texto_html.Replace("@numerodocumento", textNumeroDocumento.Text);

            Texto_html = Texto_html.Replace("@docproveedor", textDocProveedor.Text);
            Texto_html = Texto_html.Replace("@nombreproveedor", textNombreProveedor.Text);
            Texto_html = Texto_html.Replace("@fecharegistro", textFecha.Text);
            Texto_html = Texto_html.Replace("@usuarioregistro", textUsuario.Text);

            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["PrecioCompra"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                filas += "</tr>";
            }
            Texto_html = Texto_html.Replace("@filas", filas);
            Texto_html = Texto_html.Replace("@montototal", textMontoTotal.Text);

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("Compra_{0}.pdf", textNumeroDocumento.Text);
            saveFile.Filter = "Pdf Files|*.pdf";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4,25,25,25,25);

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
                    MessageBox.Show("Documento Generado","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);

                }
            }

        }
    }
}
