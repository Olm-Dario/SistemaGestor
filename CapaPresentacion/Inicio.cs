using CapaPresentacion.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {

        //Campos
        private static Usuario usuarioActual;
        private static Form formularioActivo = null;

        //Constructor
        public Inicio(Usuario usuario)
        {
            InitializeComponent();

            //Seteo el valor del usuario actual
            usuarioActual = usuario;

            //Sacar barra de titulo default
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

        }

        //Codigo que se ejecuta cuando se abre la aplicacion
        private void Inicio_Load(object sender, EventArgs e)
        {
            //Lista todos los permisos del usuario actual segun su rol
            List<Permiso> listaPermisos = new CapaNegocio_Permiso().listar(usuarioActual.idUsuario);

            //Con este foreach estamos viendo que menus se van a mostrar segun 
            //el permiso del usuario y su rol
            foreach (IconMenuItem iconMenu in barraMenu.Items)
            {
                //"m" representa cada elemento de la lista "listaPermisos"
                //"iconMenu" representa todos los menues en la "barraMenu"
                bool encontrado = listaPermisos.Any(m => m.nombreMenu == iconMenu.Name);


            }

            //Esto es para que no se superpongan los botones al iniciar el programa
            btnRestaurar.Visible = false;
            
            //Rederiza el toolstrig 
            barraMenu.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());

            //Muestro el nombre del usuario actual
            apellidoUser.Text = usuarioActual.nombre;
        }

        //Codigo para desplazar la ventana con el mouse
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panelTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        //Codigo de los botenes da la barra de titulos
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnMaximizar.Visible = true;
            btnRestaurar.Visible = false;
        }
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;

        }


        //Metodo que llama a los formularios
        private void abrirFormulario(Form formulario)
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }

            //Configura el formulario
            formularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.SteelBlue;

            //Lo pone el panel contenedor y lo muestra
            contenedor.Controls.Add(formulario);
            formulario.Show();

        }

        //Evento para llamar al formulario de Usuario
        private void menuUsuario_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormUsuarios());
        }

        private void subMenuCategoria_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormCategoria());
        }

        private void subMenuProducto_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormProducto());
        }

        private void subMenuRegistrarVenta_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormVentas());
        }

        private void subMenuVerDetalleVenta_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormDetalleVenta());
        }

        private void subMenuRegistrarCompra_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormCompras(usuarioActual));
        }

        private void subMenuVerDetalleCompra_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormDetalleCompra());
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormCliente());
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormProveedor());
        }

        private void menuReportes_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormReporte());
        }

        private void negocioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FormNegocio());
        }
    }
}
