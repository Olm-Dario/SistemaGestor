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
    public partial class FormCompras : Form
    {
        private Usuario usuario;
        public FormCompras(Usuario oUsuario = null)
        {
            InitializeComponent();
            this.usuario = oUsuario;
        }

        private void FormCompras_Load(object sender, EventArgs e)
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

        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new ModalProveedor())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textIdProveedor.Text = modal._Proveedor.idProveedor.ToString();
                    textNombreProveedor.Text = modal._Proveedor.razonSocial;
                    textDocProveedor.Text = modal._Proveedor.documento;
                }
                else
                    textDocProveedor.Select();
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
                    textPrecioCompra.Select();
                }
                else
                    textCodProducto.Select();
            }
        }

        //Evento que detectara la tecla enter
        //Evento pensado en caso de un lector de barra
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
                    textPrecioCompra.Select();
                }
                else
                {
                    textCodProducto.BackColor = Color.MistyRose;
                    textIdProducto.Text = "0";
                    textProducto.Text = "";
                }
            }
        }
    }
}
