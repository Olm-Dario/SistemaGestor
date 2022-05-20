using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FormCliente : Form
    {
        public FormCliente()
        {
            InitializeComponent();
        }

        private void FormCliente_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;





            //Muestra todos los usuarios
            List<Cliente> listaCliente = new CapaNegocio_Cliente().Listar();

            foreach (Cliente item in listaCliente)
            {
                dgvDataCliente.Rows.Add(new object[]
                
                {
                    "",
                    item.idCliente,
                    item.documento,
                    item.apellido,
                    item.nombre,
                    item.correo,
                    item.telefono,
                    item.estado == true ? 1 : 0,
                    item.estado == true ? "Activo" : "No Activo"

                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;


            Cliente objCliente = new Cliente()
            {
                idCliente = Convert.ToInt32(textId.Text),
                documento = textDocumento.Text,
                nombre = textNombre.Text,
                apellido = textApellido.Text,
                correo = textCorreo.Text,
                telefono = textTelefono.Text,
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1
            };

            if (objCliente.idCliente == 0)
            {
                int idClienteGenerado = new CapaNegocio_Cliente().Registrar(objCliente, out mensaje);

                if (idClienteGenerado != 0)
                {
                    dgvDataCliente.Rows.Add(new object[] {
                        "",
                        idClienteGenerado,
                        textDocumento.Text,
                        textNombre.Text,
                        textApellido.Text,
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
                bool resultado = new CapaNegocio_Cliente().Editar(objCliente, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvDataCliente.Rows[Convert.ToInt32(textIndice.Text)];

                    row.Cells["Id"].Value = textId.Text;
                    row.Cells["Documento"].Value = textDocumento.Text;
                    row.Cells["Nombre"].Value = textNombre.Text;
                    row.Cells["Apellido"].Value = textApellido.Text;
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


        private void Limpiar()
        {
            textIndice.Text = "-1";
            textId.Text = "";
            textDocumento.Text = "";
            textApellido.Text = "";
            textNombre.Text = "";
            textCorreo.Text = "";
            textTelefono.Text = "";
            cboEstado.SelectedIndex = 0;

            textDocumento.Select();
        }

        private void dgvDataCliente_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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

        private void dgvDataCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Verificamos si la celda que clickeamos es el boton "btnSeleccionar"
            if (dgvDataCliente.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                //Guardamos el indice de la fila
                int indice = e.RowIndex;


                if (indice >= 0)
                {
                    //Tomo los valores de la fila segun su indice
                    //Paso los valores de la fila a los textBox
                    textIndice.Text = indice.ToString();
                    textId.Text = dgvDataCliente.Rows[indice].Cells["Id"].Value.ToString();
                    textDocumento.Text = dgvDataCliente.Rows[indice].Cells["Documento"].Value.ToString();
                    textApellido.Text = dgvDataCliente.Rows[indice].Cells["Apellido"].Value.ToString();
                    textNombre.Text = dgvDataCliente.Rows[indice].Cells["Nombre"].Value.ToString();
                    textCorreo.Text = dgvDataCliente.Rows[indice].Cells["Correo"].Value.ToString();
                    textTelefono.Text = dgvDataCliente.Rows[indice].Cells["Telefono"].Value.ToString();
                    

                    

                    //Recorro las opciones del ComboBox Estado
                    foreach (OpcionCombo oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvDataCliente.Rows[indice].Cells["EstadoValor"].Value))
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textId.Text) != 0)
            {
                if(MessageBox.Show("¿Desea Eliminar el cliente?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;

                    Cliente objCliente = new Cliente()
                    {
                        idCliente = Convert.ToInt32(textId.Text)
                    };

                    bool respuesta = new CapaDato_Cliente().Eliminar(objCliente, out mensaje);

                    if (respuesta)
                    {
                        dgvDataCliente.Rows.RemoveAt(Convert.ToInt32(textIndice));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
    }
}