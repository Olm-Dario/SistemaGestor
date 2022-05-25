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
    public partial class FormUsuarios : Form
    {
        public FormUsuarios()
        {
            InitializeComponent();
        }

        private void FormUsuarios_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo"});
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            //Obtiene la lista de los roles
            List<Rol> listaRol = new CapaNegocio_Rol().listar();

            foreach (Rol item in listaRol)
            {
                cboRol.Items.Add(new OpcionCombo() { Valor = item.idRol, Texto = item.descripcion });
            }

            cboRol.DisplayMember = "Texto";
            cboRol.ValueMember = "Valor";
            cboRol.SelectedIndex = 0;


            //Muestra todos los usuarios
            List<Usuario> listaUsuario = new CapaNegocio_Usuario().listar();

            foreach (Usuario item in listaUsuario)
            {
                dgvDataUsuario.Rows.Add(new object[] 
                {   "", 
                    item.idUsuario, 
                    item.documento, 
                    item.apellido, 
                    item.nombre, 
                    item.correo, 
                    item.clave,
                    item.oRol.idRol,
                    item.oRol.descripcion,
                    item.estado == true ? 1 : 0,
                    item.estado == true ? "Activo" : "No Activo"

                });
            }

            cboRol.DisplayMember = "Texto";
            cboRol.ValueMember = "Valor";
            cboRol.SelectedIndex = 0;

            //inicializamos en 0
            textId.Text = "0";
        }

        //Evento que pinta una imagen en un boton del datagridview
        private void dgvDataUsuario_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if(e.RowIndex < 0)
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

        private void dgvDataUsuario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Verificamos si la celda que clickeamos es el boton "btnSeleccionar"
            if (dgvDataUsuario.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                //Guardamos el indice de la fila
                int indice = e.RowIndex;


                if (indice >= 0)
                {
                    //Tomo los valores de la fila segun su indice
                    //Paso los valores de la fila a los textBox
                    textIndice.Text = indice.ToString();
                    textId.Text = dgvDataUsuario.Rows[indice].Cells["Id"].Value.ToString();
                    textDocumento.Text = dgvDataUsuario.Rows[indice].Cells["Documento"].Value.ToString();
                    textApellido.Text = dgvDataUsuario.Rows[indice].Cells["Apellido"].Value.ToString();
                    textNombre.Text = dgvDataUsuario.Rows[indice].Cells["Nombre"].Value.ToString();
                    textCorreo.Text = dgvDataUsuario.Rows[indice].Cells["Correo"].Value.ToString();
                    textClave.Text = dgvDataUsuario.Rows[indice].Cells["Clave"].Value.ToString();
                    textConfirmarClave.Text = dgvDataUsuario.Rows[indice].Cells["Clave"].Value.ToString();

                    //Recorro las opciones del ComboBox Rol
                    foreach (OpcionCombo oc in cboRol.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvDataUsuario.Rows[indice].Cells["IdRol"].Value))
                        {
                            //Obtengo el indice del comboBox
                            int indiceCombo = cboRol.Items.IndexOf(oc);

                            //Dejamos seleccionado el indice buscado
                            cboRol.SelectedIndex = indiceCombo;
                            break;
                        }
                    }

                    //Recorro las opciones del ComboBox Estado
                    foreach (OpcionCombo oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvDataUsuario.Rows[indice].Cells["EstadoValor"].Value))
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
            //y lo pasamos al objeto de la clase Usuario
            Usuario objUsuario = new Usuario() {
                idUsuario = Convert.ToInt32(textId.Text),
                documento = textDocumento.Text,
                nombre = textNombre.Text,
                apellido = textApellido.Text,
                correo = textCorreo.Text,
                clave = textClave.Text,
                oRol = new Rol() { idRol= Convert.ToInt32(((OpcionCombo)cboRol.SelectedItem).Valor) },
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1 ? true : false
            };


            //Verificamos si vamos a editar o registrar con el id
            if (objUsuario.idUsuario == 0)
            {
                //Obtengo el valor de respuesta de si se genero o no el registro del usuario
                int idUsuarioGenerado = new CapaNegocio_Usuario().Registrar(objUsuario, out mensaje);

                //Verifico si se registro o no el usuario
                if (idUsuarioGenerado != 0)
                {
                    //Cargo el dataGridView con los datos del registro del usuario
                    dgvDataUsuario.Rows.Add(new object[]
                    {
                    "",
                    idUsuarioGenerado,
                    textDocumento.Text,
                    textApellido.Text,
                    textNombre.Text,
                    textCorreo.Text,
                    textClave,
                    ((OpcionCombo)cboRol.SelectedItem).Valor.ToString(),
                    ((OpcionCombo)cboRol.SelectedItem).Texto.ToString(),
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
                bool resultado = new CapaNegocio_Usuario().Editar(objUsuario, out mensaje);

                if (resultado)
                {
                    //Selecciono la fila que voy a editar
                    DataGridViewRow row = dgvDataUsuario.Rows[Convert.ToInt32(textIndice.Text)];

                    //Cargo los datos editados al datagridview
                    row.Cells["Id"].Value = textId.Text;
                    row.Cells["Documento"].Value = textDocumento.Text;
                    row.Cells["Apellido"].Value = textApellido.Text;
                    row.Cells["Nombre"].Value = textNombre.Text;
                    row.Cells["Correo"].Value = textCorreo.Text;
                    row.Cells["Clave"].Value = textClave.Text;
                    row.Cells["IdRol"].Value = ((OpcionCombo)cboRol.SelectedItem).Valor.ToString();
                    row.Cells["Rol"].Value = ((OpcionCombo)cboRol.SelectedItem).Texto.ToString();
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

        }

        //Metodo que limpia los textBox
        private void limpiar()
        {
            textIndice.Text = "-1";
            textId.Text = "0";
            textDocumento.Text = "";
            textApellido.Text = "";
            textNombre.Text = "";
            textCorreo.Text = "";
            textClave.Text = "";
            textConfirmarClave.Text = "";
            cboRol.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;

            textDocumento.Select();
        }

    }
}
