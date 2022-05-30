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

       
}
