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

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            //Cuando pasemos valores deben tener la "," y no el "." si es un valor decimal
            decimal precioCompra = 0;
            decimal precioVenta = 0;
            bool productoExiste = false;

            //Verificamos si pusimos un producto o no
            if (int.Parse(textIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Verificamos si el precio de compra tiene el formato correcto
            if (!decimal.TryParse(textPrecioCompra.Text, out precioCompra))
            {
                MessageBox.Show("Precio de compra - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textPrecioCompra.Select();
                return;
            }

            //Verificamos si el precio de venta tiene el formato correcto
            if (!decimal.TryParse(textPrecioVenta.Text, out precioVenta))
            {
                MessageBox.Show("Precio de venta - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textPrecioCompra.Select();
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
                    precioCompra.ToString("0.00"),
                    precioVenta.ToString("0.00"),
                    textCantidad.Value.ToString(),
                    (textCantidad.Value * precioCompra).ToString("0.00")
                });

                calcularTotal();
                limpiarProducto();
                textCodProducto.Select();
            }
        }

        private void limpiarProducto()
        {
            textIdProducto.Text = "0";
            textCodProducto.Text = "";
            textCodProducto.BackColor = Color.White;
            textProducto.Text = "";
            textPrecioCompra.Text = "";
            textPrecioVenta.Text = "";
            textCantidad.Value = 1;
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

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 6)
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

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Verificamos si la celda que clickeamos es el boton "btnSeleccionar"
            if (dgvData.Columns[e.ColumnIndex].Name == "btneliminar")
            {
                //Guardamos el indice de la fila
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    dgvData.Rows.RemoveAt(indice);
                    calcularTotal();
                }
            }
        }

        private void textPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Verificamos si lo que ingresamos es un numero o no
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                //Verificamos si el primer caracter es un punto
                if (textPrecioCompra.Text.Trim().Length ==  0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    //Verificamos si presionamos un control como la flecha de borrar
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void textPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Verificamos si lo que ingresamos es un numero o no
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                //Verificamos si el primer caracter es un punto
                if (textPrecioVenta.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    //Verificamos si presionamos un control como la flecha de borrar
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            //Verificamos si hay un proveedor seleccionado
            if (Convert.ToInt32(textIdProveedor.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un proveedor", "Mensaje", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            //Verificamos si se agrego algun producto a la compra
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos en la compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalle_compra = new DataTable();

            detalle_compra.Columns.Add("IdProducto", typeof(int));
            detalle_compra.Columns.Add("PrecioCompra", typeof(decimal));
            detalle_compra.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_compra.Columns.Add("Cantidad", typeof(int));
            detalle_compra.Columns.Add("MontoTotal", typeof(decimal));

            //
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                detalle_compra.Rows.Add(
                    new object[] {
                        Convert.ToInt32(row.Cells["IdProducto"].Value.ToString()),
                        row.Cells["PrecioCompra"].Value.ToString(),
                        row.Cells["PrecioVenta"].Value.ToString(),
                        row.Cells["Cantidad"].Value.ToString(),
                        row.Cells["SubTotal"].Value.ToString()
                    }); 
            }

            int idCorrelativo = new CapaNegocio_Compra().ObtenerCorrelativo();
            string numeroDocumento = string.Format("{0:00000}",idCorrelativo);

            Compra oCompra = new Compra()
            {
                oUsuario = new Usuario() { idUsuario = usuario.idUsuario },
                oProveedor = new Proveedor() { idProveedor = Convert.ToInt32(textIdProveedor.Text) },
                tipoDocumento = ((OpcionCombo)cboTipoDocumento.SelectedItem).Texto,
                numeroDocumento = numeroDocumento,
                montoTotal = Convert.ToDecimal(textTotalPagar.Text)
            };

            string mensaje = string.Empty;
            bool respuesta = new CapaNegocio_Compra().Registrar(oCompra,detalle_compra, out mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("Numero de compra generada:\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?","Mensaje", MessageBoxButtons.YesNo,MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Clipboard.SetText(numeroDocumento);
                }

                textIdProveedor.Text = "0";
                textDocProveedor.Text = "";
                textNombreProveedor.Text = "";
                dgvData.Rows.Clear();
                calcularTotal();
            }
            else
            {
                MessageBox.Show(mensaje,"Mensaje",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
