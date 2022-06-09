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
using CapaPresentacion.Utilidades;
using CapaPresentacion.Modales;

namespace CapaPresentacion
{
    public partial class FormVentas : Form
    {
        private Usuario _Usuario;

        public FormVentas(Usuario oUsuario = null)
        {
            InitializeComponent();
            this._Usuario = oUsuario;
        }

        private void FormVentas_Load(object sender, EventArgs e)
        {
            //Cargamos el comboBox Tipo de Documento
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

            //Cargamos El campo de fecha con la fecha actual
            textFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            textIdProducto.Text = "0";
            textIdProveedor.Text = "0";
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new ModalCliente())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textDocumentoCliente.Text = modal._Cliente.documento;
                    textNombreCliente.Text = modal._Cliente.apellido + " " + modal._Cliente.nombre;
                    textCodProducto.Select();
                }
                else
                    textDocumentoCliente.Select();
            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new ModalProducto())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textIdProducto.Text = modal._Producto.idProducto.ToString();
                    textCodProducto.Text = modal._Producto.codigo;
                    textProducto.Text = modal._Producto.nombre;
                    textPrecio.Text = modal._Producto.precioVenta.ToString("0.00");
                    textStock.Text = modal._Producto.stock.ToString();
                    textCantidad.Select();
                }
                else
                    textCodProducto.Select();
            }
        }

        private void textCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            //Verificamos si se presiona la tecla enter
            if (e.KeyData == Keys.Enter)
            {
                //Buscamos dentro de la lista de productos si hay un producto que coincida
                //con el codigo que estoy pasando y verificamos tambien si su estado esta en activo
                Producto oProducto = new CapaNegocio_Producto().listar().Where(p => p.codigo == textCodProducto.Text && p.estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    textCodProducto.BackColor = Color.Honeydew;
                    textIdProducto.Text = oProducto.idProducto.ToString();
                    textProducto.Text = oProducto.nombre;
                    textPrecio.Text = oProducto.precioVenta.ToString("0.00");
                    textStock.Text = oProducto.stock.ToString();
                    textCantidad.Select();
                }
                else
                {
                    textCodProducto.BackColor = Color.MistyRose;
                    textIdProducto.Text = "0";
                    textProducto.Text = "";
                    textPrecio.Text = "";
                    textStock.Text = "";
                    textCantidad.Value = 1;
                }
            }
        }



    }
}
