using DGVPrinterHelper;
using SentirseBien_NET;
using SentirseBien_NET.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinForms_sentirseBien.Formularios
{
    public partial class FormClientes : Form
    {
        DataSet clientes = new DataSet();
        DataTable dtfilter = new DataTable();
        BusinessLogicLayer _businessLogicLayer = new BusinessLogicLayer();
        Paciente userActual;
        public FormClientes(Paciente userActual)
        {
            
            InitializeComponent();
            this.userActual = userActual;
            btnFiltrar.Enabled = false;
            btnLimpiar.Enabled = false;
            /*dateTimePickerDesde.Text = "";
            dateTimePickerHasta.Text = "";
            dateTimePickerDesde.ShowCheckBox = true;
            dateTimePickerDesde.Checked = false;
            dateTimePickerHasta.ShowCheckBox = true;
            dateTimePickerHasta.Checked = false;
            dateTimePickerDesde.Format = DateTimePickerFormat.Custom;
            dateTimePickerDesde.CustomFormat = "yyyy-MM-dd";
            dateTimePickerHasta.Format = DateTimePickerFormat.Custom;
            dateTimePickerHasta.CustomFormat = "yyyy-MM-dd";*/
        }

        private void FormClientes_Load(object sender, EventArgs e)
        {
            //this.dataGridViewClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            //panel1.Visible = false;
            
            if (backgroundWorker1.IsBusy != true)
                backgroundWorker1.RunWorkerAsync();

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            //comboBox1.ResetText();
            DataTable dt = new DataTable();
            dt = clientes.Tables[0];
            this.dataGridViewClientes.DataSource = dt;
            dt.DefaultView.RowFilter = "";
            textBoxDNI.Text = "";
            //dateTimePickerDesde.Checked = false;
            //dateTimePickerHasta.Checked = false;
            if (dataGridViewClientes.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");

                return;
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {

            //var desde = dateTimePickerDesde.Value;
            //var hasta = dateTimePickerHasta.Value;
            //desde = desde.AddDays(-1);
            //hasta = hasta.AddDays(1);
            //string sdesde = "";
            //string shasta = "";

            this.dtfilter = this.clientes.Tables[0];
            this.dataGridViewClientes.DataSource = dtfilter;
            //Filtra por rango de fecha y servicio en caso de que las fechas esten seleccionadas.
            /*if (dateTimePickerDesde.Checked && dateTimePickerHasta.Checked)
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
            }*/
            this.dtfilter.DefaultView.RowFilter = $"DNI LIKE '{textBoxDNI.Text}%'";
            if (dtfilter.DefaultView.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");

                return;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.clientes = _businessLogicLayer.obtenerClientes();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = clientes.Tables[0];
            this.dataGridViewClientes.DataSource = dt;
            if (dataGridViewClientes.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                
                return;
            }
            btnFiltrar.Enabled = true;
            btnLimpiar.Enabled = true;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            
            printer.Title = "Listado de Clientes";
            printer.TableAlignment = DGVPrinter.Alignment.Center;
            printer.PageNumbers = false;
            printer.PorportionalColumns = false;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;

            printer.HeaderCellAlignment = StringAlignment.Near;

            printer.PrintPreviewDataGridView(this.dataGridViewClientes);
        }


        private void dateTimePickerHasta_ValueChanged(object sender, EventArgs e)
        {

        }


        private void textBox1_KeyUp_1(object sender, KeyEventArgs e)
        {
            /*DataTable dtfilter = new DataTable();
            dtfilter = this.clientes.Tables[0];
            this.dataGridViewClientes.DataSource = dtfilter;
            dtfilter.DefaultView.RowFilter = $"DNI LIKE '{textBoxDNI.Text}%'";*/
        }

        private void dataGridViewClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void textBoxDNI_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
