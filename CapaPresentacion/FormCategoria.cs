using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocio;
using CapaDatos;

namespace CapaPresentacion
{
    public partial class FormCategoria : Form
    {
        public FormCategoria()
        {
            InitializeComponent();
        }

        private void FormCategoria_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            //Rellena el comboBox de la busqueda
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cboBuscar.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;



            //Muestra todos los usuarios
            List<Categoria> lista = new CapaNegocio_Categoria().listar();

            foreach (Categoria item in lista)
            {
                dgvData.Rows.Add(new object[]
                {   "",
                    item.idCategoria,
                    item.descripcion,
                    item.estado == true ? 1 : 0,
                    item.estado == true ? "Activo" : "No Activo"

                });
            }

            //inicializamos en 0
            textId.Text = "0";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            //Tomamos todos los valores de los textBox y comboBox
            //y lo pasamos al objeto de la clase Categoria
            Categoria obj = new Categoria()
            {
                idCategoria = Convert.ToInt32(textId.Text),
                descripcion = textDescripcion.Text,
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1 ? true : false
            };


            //Verificamos si vamos a editar o registrar con el id
            if (obj.idCategoria == 0)
            {
                //Obtengo el valor de respuesta de si se genero o no el registro de la categoria
                int idGenerado = new CapaNegocio_Categoria().Registrar(obj, out mensaje);

                //Verifico si se registro o no el usuario
                if (idGenerado != 0)
                {
                    //Cargo el dataGridView con los datos del registro del usuario
                    dgvData.Rows.Add(new object[]
                    {
                        "",
                        idGenerado,
                        textDescripcion.Text,
                        ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString()
                    });

                    limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
            else
            {
                //Obtengo el valor de respuesta de si se genero o no la edicion del usuario
                bool resultado = new CapaNegocio_Categoria().Editar(obj, out mensaje);

                if (resultado)
                {
                    //Selecciono la fila que voy a editar
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(textIndice.Text)];

                    //Cargo los datos editados al datagridview
                    row.Cells["Id"].Value = textId.Text;
                    row.Cells["Descripcion"].Value = textDescripcion.Text;
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();

                    limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }


        }

        //Metodo que limpia los textBox
        private void limpiar()
        {
            textIndice.Text = "-1";
            textId.Text = "0";
            textDescripcion.Text = "";
            cboEstado.SelectedIndex = 0;

            textDescripcion.Select();
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
                    textDescripcion.Text = dgvData.Rows[indice].Cells["Descripcion"].Value.ToString();

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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //Verifica que haya una categoria seleccionado
            if (Convert.ToInt32(textId.Text) != 0)
            {
                //Verifica si el usuario dice que si a la eliminacion
                if (MessageBox.Show("¿Desea Eliminar la categoria?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;

                    Categoria obj = new Categoria()
                    {
                        idCategoria = Convert.ToInt32(textId.Text)
                    };

                    bool respuesta = new CapaNegocio_Categoria().Eliminar(obj, out mensaje);

                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(textIndice.Text));
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBuscar.SelectedItem).Valor.ToString();
            string palabraClave = textBuscar.Text.Trim().ToUpper();

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(palabraClave))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }
    }
}
