﻿using System;
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

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            //Cuando pasemos valores deben tener la "," y no el "." si es un valor decimal
            decimal precio = 0;
            bool productoExiste = false;

            //Verificamos si pusimos un producto o no
            if (int.Parse(textIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Verificamos si el precio de compra tiene el formato correcto
            if (!decimal.TryParse(textPrecio.Text, out precio))
            {
                MessageBox.Show("Precio - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textPrecio.Select();
                return;
            }

            //Verificamos si la cantidad supera al stock
            if (Convert.ToInt32(textStock.Text) < Convert.ToInt32(textCantidad.Value.ToString()))
            {
                MessageBox.Show("La cantidad no puede ser mayor al stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Verificamos si hay un producto en el datagrid
            foreach (DataGridViewRow fila in dgvData.Rows)
            {
                if (fila.Cells["IdProducto"].Value.ToString() == textIdProducto.Text)
                {
                    productoExiste = true;
                    break;
                }
            }

            //Agregamos el producto
            if (!productoExiste)
            {
                dgvData.Rows.Add(new object[] {
                    textIdProducto.Text,
                    textProducto.Text,
                    precio.ToString("0.00"),
                    textCantidad.Value.ToString(),
                    (textCantidad.Value * precio).ToString("0.00")
                });

                calcularTotal();
                limpiarProducto();
                textCodProducto.Select();
            }
        }
        private void calcularTotal()
        {
            decimal total = 0;

            //Varificamos si hay o no un registro
            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
                }
            }

            //Mostramos el valo del total
            textTotalPagar.Text = total.ToString("0.00");
        }

        private void limpiarProducto()
        {
            textIdProducto.Text = "0";
            textCodProducto.Text = "";
            textCodProducto.BackColor = Color.White;
            textProducto.Text = "";
            textPrecio.Text = "";
            textStock.Text = "";
            textCantidad.Value = 1;
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 5)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                //Almaceno el ancho y alto de la imagen
                var w = Properties.Resources.delete17.Width;
                var h = Properties.Resources.delete17.Height;

                //Ajustamos la imagen al boton dondo lo colocaremos
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                //Colocamos la imagen en el boton
                e.Graphics.DrawImage(Properties.Resources.delete17, new Rectangle(x, y, w, h));

                //Le decimos que si esta permitido el evento click
                //Esto lo hacemos mas que nada por las dudas que no funcione el evento luego de pintar la imagen en el boton
                e.Handled = true;

            }
        }
    }
}
