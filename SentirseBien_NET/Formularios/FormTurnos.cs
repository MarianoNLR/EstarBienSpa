using DGVPrinterHelper;
using SentirseBien_NET;
using SentirseBien_NET.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;


namespace WinForms_sentirseBien.Formularios
{
    public partial class FormTurnos : Form
    {

        DataSet pacientes;
        DataTable dtfilter = new DataTable();
        BusinessLogicLayer _businessLogicLayer = new BusinessLogicLayer();
        Paciente userActual;
        public FormTurnos(Paciente userActual)
        {
            
            InitializeComponent();
            this.userActual = userActual;
            btnFiltrar.Enabled = false;
            btnLimpiar.Enabled = false;
            dateTimePickerDesde.Text = "";
            dateTimePickerHasta.Text = "";
            dateTimePickerDesde.ShowCheckBox = true;
            dateTimePickerDesde.Checked = false;
            dateTimePickerHasta.ShowCheckBox = true;
            dateTimePickerHasta.Checked = false;
            dateTimePickerDesde.Format = DateTimePickerFormat.Custom;
            dateTimePickerDesde.CustomFormat = "yyyy-MM-dd";
            dateTimePickerHasta.Format = DateTimePickerFormat.Custom;
            dateTimePickerHasta.CustomFormat = "yyyy-MM-dd";
            

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            var desde = dateTimePickerDesde.Value;
            var hasta = dateTimePickerHasta.Value;
            desde = desde.AddDays(-1);
            //hasta = hasta.AddDays(1);
            string sdesde = "";
            string shasta = "";

            this.dtfilter = this.pacientes.Tables[0];
            this.dataGridViewClientes.DataSource = dtfilter;
            //Filtra por rango de fecha y servicio en caso de que las fechas esten seleccionadas.
            if (dateTimePickerDesde.Checked && dateTimePickerHasta.Checked)
            {
                sdesde = desde.ToString("yyyy-MM-dd");
                shasta = hasta.ToString("yyyy-MM-dd");
                Debug.WriteLine(sdesde);
                Debug.WriteLine(shasta);
                this.dtfilter.DefaultView.RowFilter = $"Nombre_Servicio LIKE '{comboBox1.Text}%' AND Fecha >= '{sdesde}%' AND Fecha <= '{shasta}%' AND DNI LIKE '{textBoxDNI.Text}%'";
            }
            else if (dateTimePickerDesde.Checked && !dateTimePickerHasta.Checked) //Filtra por servicio y desde una fecha cuando solo se selecciona la fecha "desde"
            {

                sdesde = desde.ToString("yyyy-MM-dd");
                this.dtfilter.DefaultView.RowFilter = $"Nombre_Servicio LIKE '{comboBox1.Text}%' AND Fecha >= '{sdesde}%' AND DNI LIKE '{textBoxDNI.Text}%'";
            }
            else if (!dateTimePickerDesde.Checked && dateTimePickerHasta.Checked) //Filtra por servicio y hasta una fecha cuando solo se selecciona la fecha "hasta"
            {
                shasta = hasta.ToString("yyyy-MM-dd");
                this.dtfilter.DefaultView.RowFilter = $"Nombre_Servicio LIKE '{comboBox1.Text}%' AND Fecha <= '{shasta}%' AND DNI LIKE '{textBoxDNI.Text}%'";
            }
            else if (!dateTimePickerDesde.Checked && !dateTimePickerHasta.Checked)                                                                                                               //Filtra solo por servicio cuando no se seleccionan fechas
            {
                this.dtfilter.DefaultView.RowFilter = $"Nombre_Servicio LIKE '{comboBox1.Text}%' AND DNI LIKE '{textBoxDNI.Text}%'";
            }
            else
            {
                this.dtfilter.DefaultView.RowFilter = $"DNI LIKE '{textBoxDNI.Text}%'";
            }
            if (dtfilter.DefaultView.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                btnLimpiar.PerformClick();
                return;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            comboBox1.ResetText();
            DataTable dt = new DataTable();
            dt = pacientes.Tables[0];
            this.dataGridViewClientes.DataSource = dt;
            dt.DefaultView.RowFilter = "";
            textBoxDNI.Text = "";
            dateTimePickerDesde.Checked = false;
            dateTimePickerHasta.Checked = false;
            if (dataGridViewClientes.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                return;
            }
        }

        private void FormTurnos_Load(object sender, EventArgs e)
        {

            if (userActual.Rol != "Administrador" && userActual.Rol != "Secretario")
            {
                //label1.Hide();
                //comboBox1.Hide();
                comboBox1.Enabled = false;
            }

            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync();
            }



        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.pacientes = _businessLogicLayer.obtenerCitas(userActual.Rol);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            DataTable dt = new DataTable();
            dt = pacientes.Tables[0];
            this.dataGridViewClientes.DataSource = dt;
            if (userActual.Rol != "Administrador" && userActual.Rol != "Secretario")
            {
                comboBox1.Text = userActual.Rol;
            }
            btnFiltrar.Enabled = true;
            btnLimpiar.Enabled = true;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            if(comboBox1.Text == "")
            {
                printer.Title = "Todos los turnos";
            }
            else
            {
                printer.Title = comboBox1.Text;
            }
            if (textBoxDNI.Text != "")
            {
                printer.SubTitle = "Cliente con DNI: " + textBoxDNI.Text;
            }
            else {
                printer.SubTitle = "Todos los clientes" + textBoxDNI.Text;
            }

            if (dateTimePickerDesde.Checked && dateTimePickerHasta.Checked)
            {
                printer.SubTitle += "\n" + dateTimePickerDesde.Value.ToShortDateString() + " - " + dateTimePickerHasta.Value.ToShortDateString();
                printer.SubTitleAlignment = StringAlignment.Center;
            }
            
            
            printer.PageNumbers = false;
            printer.PorportionalColumns = false;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
            printer.TableAlignment = DGVPrinter.Alignment.Left;
            printer.RowHeight = DGVPrinter.RowHeightSetting.DataHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.PrintPreviewDataGridView(this.dataGridViewClientes);

            /*PrintDocument doc = new PrintDocument();
            doc.DefaultPageSettings.Landscape = true;
            doc.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            PrintPreviewDialog ppd = new PrintPreviewDialog { Document = doc };
            ((Form)ppd).WindowState = FormWindowState.Maximized;

            doc.PrintPage += delegate (object ev, PrintPageEventArgs ep)
             {
                 const int DGV_ALTO = 499;
                 int left = ep.MarginBounds.Left, top = ep.MarginBounds.Top;

                 foreach (DataGridViewColumn col in dataGridViewClientes.Columns)
                 {
                     ep.Graphics.DrawString(col.HeaderText, new Font("SEGOE UI", 16, FontStyle.Bold), Brushes.DeepSkyBlue, left, top);
                     left += col.Width;
                 }
                 left = ep.MarginBounds.Left;
                 ep.Graphics.FillRectangle(Brushes.Black, left, top + 20, ep.MarginBounds.Right - left, 3);
                 top += 43;

                 foreach(DataGridViewRow row in dataGridViewClientes.Rows)
                 {
                     foreach(DataGridViewCell cell in row.Cells)
                     {
                         ep.Graphics.DrawString(Convert.ToString(cell.Value), new Font("Segoe UI", 13), Brushes.Black, left, top + 4);
                         left += cell.OwningColumn.Width;
                     }
                     top += DGV_ALTO;
                 }
             };
            ppd.ShowDialog();
        }*/
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*DataTable dtfilter = new DataTable();
            dtfilter = this.pacientes.Tables[0];
            this.dataGridViewClientes.DataSource = dtfilter;
            dtfilter.DefaultView.RowFilter = $"Nombre_Servicio LIKE '{comboBox1.Text}%'";*/
        }

        private void FormTurnos_SizeChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
    }
}
