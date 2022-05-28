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
