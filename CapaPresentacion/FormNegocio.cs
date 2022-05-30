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
using System.IO;

namespace CapaPresentacion
{
    public partial class FormNegocio : Form
    {
        public FormNegocio()
        {
            InitializeComponent();
        }

        //Metedo que pasa un array de byte a una imagen
        public Image ByteToImage(byte[] imageByte)
        {
            MemoryStream ms = new MemoryStream();
            
            //Array de byte - indice con comienza el array - cantidad de byte de la imagen
            ms.Write(imageByte, 0, imageByte.Length);
            
            //Conversion a imagen
            Image image = new Bitmap(ms);

            return image;
        }

        private void FormNegocio_Load(object sender, EventArgs e)
        {
            bool obtenido = true;
            byte[] image = new CapaNegocio_Negocio().ObtenerLogo(out obtenido);

            //Verifica si realmente obtuvo la imagen
            if (obtenido)
            {
                //Pinta la imagen en el pictureBox
                picLogo.Image = ByteToImage(image);
            }

            //Obtiene los datos y los pasa a los textBox para mostrarlos
            Negocio dato = new CapaNegocio_Negocio().ObtenerDatos();
            textNombre.Text = dato.Nombre;
            textDireccion.Text = dato.Direccion;

        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            //Filtro que tipo de archivos me dejara ver el OpenFileDialog
            openFileDialog.FileName = "Files|*.jpg;*.jpeg;*.png";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] byteimage = File.ReadAllBytes(openFileDialog.FileName);
                bool respuesta = new CapaNegocio_Negocio().ActualizarLogo(byteimage, out mensaje);

                if (respuesta)
                    picLogo.Image = ByteToImage(byteimage);
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Negocio obj = new Negocio()
            {
                Nombre = textNombre.Text,
                Direccion = textDireccion.Text
            };

            bool respuesta = new CapaNegocio_Negocio().GuardarDatos(obj, out mensaje);

            if (respuesta)
                MessageBox.Show("Los cambios fueron guardados", "Mensaje",MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se pudo guardar los cambios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
