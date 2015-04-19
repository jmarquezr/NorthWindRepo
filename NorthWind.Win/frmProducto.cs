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
using NorthWind.DAO;

namespace NorthWind.Win
{
    
    public partial class frmProducto : Form
    {
        public event EventHandler<TbProductoBE> onProductoSeleccionado;
        List<TbProductoBE> Lista = new List<TbProductoBE>();
        List<TbCategoriaBE> ListaCategoria = new List<TbCategoriaBE>();
        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            //Lista = TbProductoBE.SelectAll();
            //Lista = TbProductoDAO.SelectAll();
            //this.TbProductobindingSource.DataSource = Lista;
            //this.dataGridView1.SelectionMode =
            //    DataGridViewSelectionMode.FullRowSelect;
            Lista = TbProductoDAO.SelectAll();
            ListaCategoria = TbCategoriaDAO.SelectAll();

            var ProCat = (from pro in Lista
                          join
                              cat in ListaCategoria on Convert.ToInt32(pro.Categoria) equals cat.CodCategoria
                          select new { pro.CodProducto, pro.Descripcion, pro.Precio, cat.NomCategoria }).ToArray();

            //this.TbProductobindingSource.DataSource = Lista;
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = ProCat;

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //var query = from c in Lista
            //            where c.Descripcion.StartsWith(txtDescripcion.Text)
            //            select c;
            //this.TbProductobindingSource.DataSource = query;
            //this.dataGridView1.SelectionMode =
            //    DataGridViewSelectionMode.FullRowSelect;
            Lista = TbProductoDAO.SelectAll();
            ListaCategoria = TbCategoriaDAO.SelectAll();

            var ProCat = (from pro in Lista
                          join cat in ListaCategoria on Convert.ToInt32(pro.Categoria) equals cat.CodCategoria
                          select new { pro.CodProducto, pro.Descripcion, pro.Precio, cat.NomCategoria }).ToArray();

            var query = (from p in ProCat
                         where p.Descripcion.ToUpper().Trim().Contains(txtDescripcion.Text.ToUpper().Trim())
                         select new { p.CodProducto, p.Descripcion, p.Precio, p.NomCategoria }).ToArray();

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = query;
        }

    }
}
