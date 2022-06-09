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
    public partial class ModalCliente : Form
    {
        public Cliente _Cliente { get; set; }

        public ModalCliente()
        {
            InitializeComponent();
        }

        private void ModalCliente_Load(object sender, EventArgs e)
        {
            //CARGA DEL ComboBusqueda
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                cboBuscar.Items.Add(new OpcionCombo() { Valor = column.Name, Texto = column.HeaderText });
            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;


            List<Cliente> listaCliente = new CapaNegocio_Cliente().Listar();

            foreach (Cliente item in listaCliente)
            {
                if(item.estado)
                    dgvData.Rows.Add(new object[]{ item.documento, item.apellido, item.nombre }); 
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Tomamos indice de fila y columna
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            //Verificamos si existe 
            if (iRow >= 0 && iColum >= 0)
            {
                _Cliente = new Cliente()
                {
                    documento = dgvData.Rows[iRow].Cells["Documento"].Value.ToString(),
                    apellido = dgvData.Rows[iRow].Cells["Apellido"].Value.ToString(),
                    nombre = dgvData.Rows[iRow].Cells["Nombre"].Value.ToString()
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
