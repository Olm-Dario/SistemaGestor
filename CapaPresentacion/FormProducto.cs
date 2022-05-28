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

namespace CapaPresentacion
{
    public partial class FormProducto : Form
    {
        public FormProducto()
        {
            InitializeComponent();
        }

        private void FormProducto_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            //Rellena comboBox de Categoria
            List<Categoria> listaCategoria = new CapaNegocio_Categoria().listar();

            foreach (Categoria item in listaCategoria)
            {
                cboCategoria.Items.Add(new OpcionCombo() { Valor = item.idCategoria, Texto = item.descripcion });
            }
            cboCategoria.DisplayMember = "Texto";
            cboCategoria.ValueMember = "Valor";
            cboCategoria.SelectedIndex = 0;


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


            //Muestra todos los Productos
            List<Producto> lista = new CapaNegocio_Producto().listar();

            foreach (Producto item in lista)
            {
                dgvData.Rows.Add(new object[]
                {   "",
                    item.idProducto,
                    item.codigo,
                    item.nombre,
                    item.descripcion,
                    item.oCategoria.idCategoria,
                    item.oCategoria.descripcion,
                    item.stock,
                    item.precioCompra,
                    item.precioVenta,
                    item.estado == true ? 1 : 0,
                    item.estado == true ? "Activo" : "No Activo"

                });
            }



            //inicializamos en 0
            textId.Text = "0";
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
                    textCodigo.Text = dgvData.Rows[indice].Cells["Codigo"].Value.ToString();
                    textNombre.Text = dgvData.Rows[indice].Cells["Nombre"].Value.ToString();
                    textDescripcion.Text = dgvData.Rows[indice].Cells["Descripcion"].Value.ToString();


                    //Recorro las opciones del ComboBox Categoria
                    foreach (OpcionCombo oc in cboCategoria.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["IdCategoria"].Value))
                        {
                            //Obtengo el indice del comboBox
                            int indiceCombo = cboCategoria.Items.IndexOf(oc);

                            //Dejamos seleccionado el indice buscado
                            cboCategoria.SelectedIndex = indiceCombo;
                            break;
                        }
                    }

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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            //Tomamos todos los valores de los textBox y comboBox
            //y lo pasamos al objeto de la clase Producto
            Producto obj = new Producto()
            {
                idProducto = Convert.ToInt32(textId.Text),
                codigo = textCodigo.Text,
                nombre = textNombre.Text,
                descripcion = textDescripcion.Text,
                oCategoria = new Categoria() { idCategoria = Convert.ToInt32(((OpcionCombo)cboCategoria.SelectedItem).Valor) },
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1 ? true : false
            };


            //Verificamos si vamos a editar o registrar con el id
            if (obj.idProducto == 0)
            {
                //Obtengo el valor de respuesta de si se genero o no el registro del Producto
                int idGenerado = new CapaNegocio_Producto().Registrar(obj, out mensaje);

                //Verifico si se registro o no el Producto
                if (idGenerado != 0)
                {
                    //Cargo el dataGridView con los datos del registro del Producto
                    dgvData.Rows.Add(new object[]
                    {
                        "",
                        idGenerado,
                        textCodigo.Text,
                        textNombre.Text,
                        textDescripcion.Text,
                        ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString(),
                        "0",
                        "0.00",
                        "0.00",
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
                bool resultado = new CapaNegocio_Producto().Editar(obj, out mensaje);

                if (resultado)
                {
                    //Selecciono la fila que voy a editar
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(textIndice.Text)];

                    //Cargo los datos editados al datagridview
                    row.Cells["Id"].Value = textId.Text;
                    row.Cells["Codigo"].Value = textCodigo.Text;
                    row.Cells["Nombre"].Value = textNombre.Text;
                    row.Cells["Descripcion"].Value = textDescripcion.Text;
                    row.Cells["IdCategoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString();
                    row.Cells["Categoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString();
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
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //Verifica que haya un producto seleccionado
            if (Convert.ToInt32(textId.Text) != 0)
            {
                //Verifica si el usuario dice que si a la eliminacion
                if (MessageBox.Show("¿Desea Eliminar el Producto?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;

                    Producto obj = new Producto()
                    {
                        idProducto = Convert.ToInt32(textId.Text)
                    };

                    bool respuesta = new CapaNegocio_Producto().Eliminar(obj, out mensaje);

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
        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            textBuscar.Text = "";

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        //Metodo que limpia los textBox
        private void limpiar()
        {
            textIndice.Text = "-1";
            textId.Text = "0";
            textCodigo.Text = "";
            textNombre.Text = "";
            textDescripcion.Text = "";
            cboCategoria.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;

            textCodigo.Select();
        }

    }
}
