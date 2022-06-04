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
using CapaPresentacion.Utilidades;
using CapaPresentacion.Modales;

namespace CapaPresentacion
{
    public partial class FormCompras : Form
    {
        private Usuario usuario;
        public FormCompras(Usuario oUsuario = null)
        {
            InitializeComponent();
            this.usuario = oUsuario;
        }

        private void FormCompras_Load(object sender, EventArgs e)
        {
            //Cargamos el comboBox Tipo de Documento
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

            //Cargamos El campo de fecha con la fecha actual
            textFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            textIdProducto.Text = "0";
            textIdProveedor.Text = "0";
        }

        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new ModalProveedor())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textIdProveedor.Text = modal._Proveedor.idProveedor.ToString();
                    textNombreProveedor.Text = modal._Proveedor.razonSocial;
                    textDocProveedor.Text = modal._Proveedor.documento;
                }
                else
                    textDocProveedor.Select();
            }
        }
    }
}
