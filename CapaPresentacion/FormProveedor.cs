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

namespace CapaPresentacion
{
    public partial class FormProveedor : Form
    {
        public FormProveedor()
        {
            InitializeComponent();
        }

        private void FormProveedor_Load(object sender, EventArgs e)
        {
            textId.Text = "0";
            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            //Muestra todos los usuarios
            List<Proveedor> listaCliente = new CapaNegocio_Proveedor().listar();

            foreach (Proveedor item in listaCliente)
            {
                dgvData.Rows.Add(new object[]

                {
                    "",
                    item.idProveedor,
                    item.documento,
                    item.razonSocial,
                    item.correo,
                    item.telefono,
                    item.estado == true ? 1 : 0,
                    item.estado == true ? "Activo" : "No Activo"

                });
            }

            //CARGA DEL ComboBusqueda

            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                if (column.Visible == true)
                {
                    cboBuscar.Items.Add(new OpcionCombo() { Valor = column.Name, Texto = column.HeaderText });

                }
            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;


            Proveedor obj = new Proveedor()
            {
                idProveedor = Convert.ToInt32(textId.Text),
                documento = textDocumento.Text,
                razonSocial = textRazonSocial.Text,
                correo = textCorreo.Text,
                telefono = textTelefono.Text,
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1
            };

            if (obj.idProveedor == 0)
            {
                int idProveedorGenerado = new CapaNegocio_Proveedor().Registrar(obj, out mensaje);

                if (idProveedorGenerado != 0)
                {
                    dgvData.Rows.Add(new object[] {
                        "",
                        idProveedorGenerado,
                        textDocumento.Text,
                        textRazonSocial.Text,
                        textCorreo.Text,
                        textTelefono.Text,
                        ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString()
                    });

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
            else
            {
                bool resultado = new CapaNegocio_Proveedor().Editar(obj, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(textIndice.Text)];

                    row.Cells["Id"].Value = textId.Text;
                    row.Cells["Documento"].Value = textDocumento.Text;
                    row.Cells["RazonSocial"].Value = textRazonSocial.Text;
                    row.Cells["Correo"].Value = textCorreo.Text;
                    row.Cells["Telefono"].Value = textTelefono.Text;

                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }

            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textId.Text) != 0)
            {
                if (MessageBox.Show("¿Desea Eliminar el proveedor?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;

                    Proveedor obj = new Proveedor()
                    {
                        idProveedor = Convert.ToInt32(textId.Text)
                    };

                    bool respuesta = new CapaNegocio_Proveedor().Eliminar(obj, out mensaje);

                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(textIndice.Text));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                //Almaceno el ancho y alto de la imagen
                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;

                //Ajustamos la imagen al boton dondo lo colocaremos
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                //Colocamos la imagen en el boton
                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));

                //Le decimos que si esta permitido el evento click
                //Esto lo hacemos mas que nada por las dudas que no funcione el evento luego de pintar la imagen en el boton
                e.Handled = true;

            }
        }
        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Verificamos si la celda que clickeamos es el boton "btnSeleccionar"
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                //Guardamos el indice de la fila
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    //Tomo los valores de la fila segun su indice
                    //Paso los valores de la fila a los textBox
                    textIndice.Text = indice.ToString();
                    textId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString();
                    textDocumento.Text = dgvData.Rows[indice].Cells["Documento"].Value.ToString();
                    textRazonSocial.Text = dgvData.Rows[indice].Cells["RazonSocial"].Value.ToString();
                    textCorreo.Text = dgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    textTelefono.Text = dgvData.Rows[indice].Cells["Telefono"].Value.ToString();

                    //Recorro las opciones del ComboBox Estado
                    foreach (OpcionCombo oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            //Obtengo el indice del comboBox
                            int indiceCombo = cboEstado.Items.IndexOf(oc);

                            //Dejamos seleccionado el indice buscado
                            cboEstado.SelectedIndex = indiceCombo;
                            break;
                        }
                    }

                }
            }
        }

        private void Limpiar()
        {
            textIndice.Text = "-1";
            textId.Text = "0";
            textDocumento.Text = "";
            textRazonSocial.Text = "";
            textCorreo.Text = "";
            textTelefono.Text = "";
            cboEstado.SelectedIndex = 0;

            textDocumento.Select();
        }

    }
}
