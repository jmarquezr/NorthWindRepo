using NorthWind.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthWind.Win
{
    
    public partial class frmProducto : Form
    {
        public event EventHandler<TbProductoBE> onProductoSeleccionado;
        List<TbProductoBE> Lista = new List<TbProductoBE>();
        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            Lista = TbProductoBE.SelectAll();
            this.TbProductobindingSource.DataSource = Lista;
            this.dataGridView1.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

        }

        private void AgregarProductoFactura()
        {
            int i = dataGridView1.CurrentRow.Index;
            string codigoProducto = dataGridView1.Rows[i].Cells[0].Value.ToString();
            TbProductoBE oProducto = (from item in Lista.ToArray()
                                    where item.CodProducto == codigoProducto
                                    select item).Single();
            onProductoSeleccionado(new object(), oProducto);
            this.Close();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            AgregarProductoFactura();
        }

        private void dataGridView1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                AgregarProductoFactura();
            }
        }
    }
}
