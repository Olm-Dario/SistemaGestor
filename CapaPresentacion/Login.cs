using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using CapaNegocio;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        //Eventos para simular el placeholder en USUARIO
        private void textUser_Enter(object sender, EventArgs e)
        {
            if (textUser.Text == "USUARIO")
            {
                textUser.Text = "";
                textUser.ForeColor = Color.LightGray;
            }
        }
        private void textUser_Leave(object sender, EventArgs e)
        {
            if (textUser.Text == "")
            {
                textUser.Text = "USUARIO";
                textUser.ForeColor = Color.DimGray;
            }
        }

        //Eventos para simular el placeholder en CONTRASEÑA
        private void textPass_Enter(object sender, EventArgs e)
        {
            if (textPass.Text == "CONTRASEÑA")
            {
                textPass.Text = "";
                textPass.ForeColor = Color.LightGray;
                textPass.UseSystemPasswordChar = true;
            }
        }
        private void textPass_Leave(object sender, EventArgs e)
        {
            if (textPass.Text == "")
            {
                textPass.Text = "CONTRASEÑA";
                textPass.ForeColor = Color.DimGray;
                textPass.UseSystemPasswordChar = false;
            }
        }

        //Evento para cerrar la ventana
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //Evento para ingresar al sistema
        private void btnLogin_Click(object sender, EventArgs e)
        {
            List<Usuario> test = new CapaNegocio_Usuario().listar();

            Usuario oUsuario = new CapaNegocio_Usuario().listar().Where(u => u.documento == textUser.Text && u.clave == textPass.Text).FirstOrDefault();

            if(oUsuario != null)
            {
                Inicio form = new Inicio(oUsuario);

                //Abrimos el formulario del sistema
                form.Show();

                //Ocultamos el login
                this.Hide();

                form.FormClosing += closingLogin;
            }
            else
            {
                MessageBox.Show("No se encontro el Usuario","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }

            

        }

        //Evento para que aparesca el login una ves cerrado el sistema
        private void closingLogin(object sender, FormClosingEventArgs e)
        {
            textUser.Text = "USUARIO";
            textPass.Text = "CONTRASEÑA";
            textPass.UseSystemPasswordChar = false;

            this.Show();
        }



        //Codigo para desplazar la ventana con el mouse
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void move_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        
    }
}
