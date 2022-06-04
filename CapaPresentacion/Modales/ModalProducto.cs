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
using CapaPresentacion.Utilidades;

namespace CapaPresentacion.Modales
{
    public partial class ModalProducto : Form
    {
        public Producto _Producto { get; set; }

        public ModalProducto()
        {
            InitializeComponent();
        }

        private void ModalProducto_Load(object sender, EventArgs e)
        {
            //Rellena el comboBox de la busqueda
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true)
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
                {
                    item.idProducto,
                    item.codigo,
                    item.nombre,
                    item.oCategoria.descripcion,
                    item.stock,
                    item.precioCompra,
                    item.precioVenta
                });
            }
        }
    }
}
