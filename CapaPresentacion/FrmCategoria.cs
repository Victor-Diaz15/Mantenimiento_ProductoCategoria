using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using SpreadsheetLight;

namespace CapaPresentacion
{
    public partial class FrmCategoria : Form
    {
        //Variables e instancias
        private string IdCategoria;
        private bool editar = false;
        N_Categoria objLogica = new N_Categoria();
        E_Categoria objEntidad = new E_Categoria();
        public FrmCategoria()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            MostrarData();
        }

        //metodo que se ejecuta cuando el txtBox cambia: Metodo buscar
        private void txt_Buscar_OnTextChange(object sender, EventArgs e)
        {
            DashboardCategoria.DataSource = objLogica.ListandoCategoria(txt_Buscar.text);
            DashboardCategoria.ClearSelection();
        }
        private void btn_Nuevo_Click(object sender, EventArgs e)
        {
            LimpiarTxt();
            DashboardCategoria.ClearSelection();
        }
        //metodo para editar una categoria
        private void btn_Editar_Click(object sender, EventArgs e)
        {
            if (DashboardCategoria.SelectedRows.Count > 0)
            {
                editar = true;
                IdCategoria = DashboardCategoria.CurrentRow.Cells[0].Value.ToString();
                txt_Nombre.Text = DashboardCategoria.CurrentRow.Cells[2].Value.ToString();
                txt_Descripcion.Text = DashboardCategoria.CurrentRow.Cells[3].Value.ToString();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una fila", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //metodo para eliminar una categoria
        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            if (DashboardCategoria.SelectedRows.Count > 0)
            {
                objEntidad.IdCategoria = Convert.ToInt32(DashboardCategoria.CurrentRow.Cells[0].Value.ToString());
                objLogica.EliminandoCategoria(objEntidad);

                MessageBox.Show("Categoria Eliminada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MostrarData();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una fila", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        //metodo para guardar una categoria
        private void btn_Guardar_Click(object sender, EventArgs e)
        {
            if (editar == false)
            {
                try
                {
                    if (txt_Nombre.Text != "" || txt_Descripcion.Text != "")
                    {
                        objEntidad.Nombre = txt_Nombre.Text.ToUpper();
                        objEntidad.Descripcion = txt_Descripcion.Text.ToUpper();

                        objLogica.AgregandoCategoria(objEntidad);
                        MessageBox.Show("Se guardo el registro", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarTxt();
                        MostrarData();
                    }
                    else
                    {
                        MessageBox.Show("Debe asegurarse de llenar los campos antes de enviar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("No se puedo guardar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    objEntidad.IdCategoria = Convert.ToInt32(IdCategoria);
                    objEntidad.Nombre = txt_Nombre.Text.ToUpper();
                    objEntidad.Descripcion = txt_Descripcion.Text.ToUpper();

                    objLogica.EditandoCategoria(objEntidad);
                    MessageBox.Show("Se edito el registro", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarTxt();
                    MostrarData();
                }
                catch (Exception)
                {
                    MessageBox.Show("No se puedo editar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        private void btn_Imprimir_Click(object sender, EventArgs e)
        {
            Print();
        }
        private void btn_Excel_Click(object sender, EventArgs e)
        {
            ExportarExcel();
        }
        //metodo para traer la data
        private void MostrarData()
        {
            DashboardCategoria.DataSource = objLogica.ListandoCategorias();
            AccionesDashboard();
        }
        //metodo acciones tablas
        private void AccionesDashboard()
        {
            DashboardCategoria.Columns[0].Visible = false;
            DashboardCategoria.ClearSelection();
        }
        //metodo para limpiar los campos de textos
        private void LimpiarTxt()
        {
            editar = false;
            txt_Nombre.Text = "";
            txt_Descripcion.Text = "";
            txt_Buscar.text = "";
            txt_Nombre.Focus();
        }
        //imprimiendo el DashboardCategoria
        private void Print()
        {
            PrintDocument doc = new PrintDocument();
            doc.DefaultPageSettings.Landscape = true;
            doc.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            PrintPreviewDialog ppd = new PrintPreviewDialog { Document = doc };
            ((Form)ppd).WindowState = FormWindowState.Maximized;

            doc.PrintPage += delegate (object ev, PrintPageEventArgs ep)
            {
                const int DGV_ALTO = 35;
                int left = ep.MarginBounds.Left, top = ep.MarginBounds.Top;

                foreach (DataGridViewColumn col in DashboardCategoria.Columns)
                {
                    ep.Graphics.DrawString(col.HeaderText, new Font("Segoe UI", 16, FontStyle.Bold), Brushes.DeepSkyBlue, left, top);
                    left += col.Width;
                }
                left = ep.MarginBounds.Left;
                ep.Graphics.FillRectangle(Brushes.Black, left, top + 40, ep.MarginBounds.Right - left, 3);
                top += 43;

                foreach (DataGridViewRow row in DashboardCategoria.Rows)
                {
                    left = ep.MarginBounds.Left;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        ep.Graphics.DrawString(Convert.ToString(cell.Value), new Font("Segoe UI", 13), Brushes.Black, left, top + 4);
                        left += cell.OwningColumn.Width;
                    }
                    top += DGV_ALTO;
                }
            };
            ppd.ShowDialog();
        }
        //metodo para exportar el DashboardCategoria a Excel
        private void ExportarExcel()
        {
            SLDocument sl = new SLDocument();
            SLStyle style = new SLStyle();
            style.Font.Bold = true;
            style.Font.FontSize = 12;
            int IC = 1;
            //recorriendo las columnas del DashboardCategoria
            foreach (DataGridViewColumn column in DashboardCategoria.Columns)
            {
                sl.SetCellValue(1, IC, column.HeaderText.ToString());
                sl.SetCellStyle(1, IC, style);
                IC++;
            }

            int IR = 2;
            //recorriendo los row del DashboardCategoria
            foreach (DataGridViewRow row in DashboardCategoria.Rows)
            {
                sl.SetCellValue(IR, 1, row.Cells[0].Value.ToString());
                sl.SetCellValue(IR, 2, row.Cells[1].Value.ToString());
                sl.SetCellValue(IR, 3, row.Cells[2].Value.ToString());
                sl.SetCellValue(IR, 4, row.Cells[3].Value.ToString());
                IR++;
            }
            string ruta = @"\\JRUBI\Users\rubi\Documents\DashboardCategoriaExcel\DashboardCategoria.xlsx";
            sl.SaveAs(ruta);
            MessageBox.Show("Excel exportado!! " + "ruta: " + ruta, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
