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
    public partial class ModalProveedor : Form
    {
        public Proveedor _Proveedor { get; set; }

        public ModalProveedor()
        {
            InitializeComponent();
        }

        private void ModalProveedor_Load(object sender, EventArgs e)
        {
            //CARGA DEL ComboBusqueda

            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                if (column.Visible == true)
                {
                    cboBuscar.Items.Add(new OpcionCombo() { Valor = column.Name, Texto = column.HeaderText });

                }
            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;

            //Muestra todos los usuarios
            List<Proveedor> listaCliente = new CapaNegocio_Proveedor().listar();

            foreach (Proveedor item in listaCliente)
            {
                dgvData.Rows.Add(new object[]
                {
                    item.idProveedor, item.documento, item.razonSocial
                });
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Tomamos indice de fila y columna
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            //Verificamos si existe 
            if (iRow >= 0 && iColum > 0)
            {
                _Proveedor = new Proveedor()
                {
                    idProveedor = Convert.ToInt32(dgvData.Rows[iRow].Cells["Id"].Value.ToString()),
                    documento = dgvData.Rows[iRow].Cells["Documento"].Value.ToString(),
                    razonSocial = dgvData.Rows[iRow].Cells["RazonSocial"].Value.ToString()
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
