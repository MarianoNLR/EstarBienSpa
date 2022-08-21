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
    public partial class FormPagos : Form
    {
        DataSet Factura;
        DataTable dtfilter = new DataTable();
        Paciente userActual;
        BusinessLogicLayer _businessLogicLayer = new BusinessLogicLayer();
        public FormPagos(Paciente userActual)
        {
            
            InitializeComponent();
            labelAvisoNoResultados.Visible = false;
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Factura = _businessLogicLayer.obtenerPagos();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = Factura.Tables[0];
            this.dataGridViewFacturas.DataSource = dt;
            if (dataGridViewFacturas.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                return;
            }
            btnFiltrar.Enabled = true;
            btnLimpiar.Enabled = true;
            calcularIngresos();
        }

        

        private void FormPagos_Load(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Todos los Pagos";
            printer.SubTitle = labelTotalDebito.Text + "\n" + labelTotalCredito.Text + "\n" + labelIngresos.Text;
            if(comboBoxTipoPago.Text == "Debito")
            {
                printer.Title = "Pagos de tipo " + comboBoxTipoPago.Text;
                printer.SubTitle = labelTotalDebito.Text;
            }
            else if (comboBoxTipoPago.Text == "Credito")
            {
                printer.Title = "Pagos de tipo " + comboBoxTipoPago.Text;
                printer.SubTitle = labelTotalCredito.Text;
            }

            if (dateTimePickerDesde.Checked && dateTimePickerHasta.Checked)
            {
                printer.SubTitle += "\n" +  dateTimePickerDesde.Value.ToShortDateString() + " - " + dateTimePickerHasta.Value.ToShortDateString();
                printer.SubTitleAlignment = StringAlignment.Center;
            }else if (!dateTimePickerDesde.Checked && dateTimePickerHasta.Checked)
            {
                printer.SubTitle += "\n - " + dateTimePickerHasta.Value.ToShortDateString();
                printer.SubTitleAlignment = StringAlignment.Center;
            }else if (dateTimePickerDesde.Checked && !dateTimePickerHasta.Checked)
            {
                printer.SubTitle += "\n A partir del " + dateTimePickerDesde.Value.ToShortDateString();
                printer.SubTitleAlignment = StringAlignment.Center;
            }

            
            

            /*else
            {
                printer.Title = comboBox1.Text;
            }*/

            printer.PageNumbers = true;
            printer.PorportionalColumns = false;
            printer.TableAlignment = DGVPrinter.Alignment.Center;
            printer.RowHeight = DGVPrinter.RowHeightSetting.DataHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
            printer.PrintPreviewDataGridView(this.dataGridViewFacturas);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            var desde = dateTimePickerDesde.Value;
            var hasta = dateTimePickerHasta.Value;
            desde = desde.AddDays(-1);
            //hasta = hasta.AddDays(1);
            string sdesde = "";
            string shasta = "";

            this.dtfilter = this.Factura.Tables[0];
            
            this.dataGridViewFacturas.DataSource = dtfilter;
            //Filtra por rango de fecha y servicio en caso de que las fechas esten seleccionadas.
            if (dateTimePickerDesde.Checked && dateTimePickerHasta.Checked)
            {
                sdesde = desde.ToString("yyyy-MM-dd");
                shasta = hasta.ToString("yyyy-MM-dd");
                Debug.WriteLine(sdesde);
                Debug.WriteLine(shasta);
                this.dtfilter.DefaultView.RowFilter = $"DNI LIKE '{textBoxDNI.Text}%' AND Fecha >= '{sdesde}%' AND Fecha <= '{shasta}%' AND Tipo_Pago LIKE '{comboBoxTipoPago.Text}%'";
            }
            else if (dateTimePickerDesde.Checked && !dateTimePickerHasta.Checked) //Filtra por servicio y desde una fecha cuando solo se selecciona la fecha "desde"
            {

                sdesde = desde.ToString("yyyy-MM-dd");
                this.dtfilter.DefaultView.RowFilter = $"DNI LIKE '{textBoxDNI.Text}%' AND Fecha >= '{sdesde}%' AND Tipo_Pago LIKE '{comboBoxTipoPago.Text}%'";
            }
            else if (!dateTimePickerDesde.Checked && dateTimePickerHasta.Checked) //Filtra por servicio y hasta una fecha cuando solo se selecciona la fecha "hasta"
            {
                shasta = hasta.ToString("yyyy-MM-dd");
                this.dtfilter.DefaultView.RowFilter = $"DNI LIKE '{textBoxDNI.Text}%' AND Fecha <= '{shasta}%' AND Tipo_Pago LIKE '{comboBoxTipoPago.Text}%'";
            }
            else if (!dateTimePickerDesde.Checked && !dateTimePickerHasta.Checked)                                                                                                               //Filtra solo por servicio cuando no se seleccionan fechas
            {
                this.dtfilter.DefaultView.RowFilter = $"DNI LIKE '{textBoxDNI.Text}%' AND Tipo_Pago LIKE '{comboBoxTipoPago.Text}%'";
            }
            else
            {
                this.dtfilter.DefaultView.RowFilter = $"DNI LIKE '{comboBoxTipoPago.Text}%'";
            }
            if (dtfilter.DefaultView.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                btnLimpiar.PerformClick();
                return;
            }
            calcularIngresos();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            textBoxDNI.ResetText();
            comboBoxTipoPago.ResetText();
            dateTimePickerDesde.ResetText();
            dateTimePickerHasta.ResetText();
            DataTable dt = new DataTable();
            dt = Factura.Tables[0];
            this.dataGridViewFacturas.DataSource = dt;
            dt.DefaultView.RowFilter = "";
            if (dataGridViewFacturas.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                return;
            }
            calcularIngresos();
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


        private void calcularIngresos()
        {
            int totalCredito = 0;
            int totalDebito = 0;

            foreach (DataGridViewRow row in dataGridViewFacturas.Rows)
            {
                if(row.Cells[3].Value.ToString() == "Debito")
                {
                    totalDebito += int.Parse(row.Cells[2].Value.ToString());
                }else if (row.Cells[3].Value.ToString() == "Credito")
                {
                    totalCredito += int.Parse(row.Cells[2].Value.ToString());
                }
                //total += int.Parse(row.Cells[2].Value.ToString());

            }
            labelTotalDebito.Text = "Total Debito: $" + totalDebito;
            labelTotalCredito.Text = "Total Credito: $" + totalCredito;
            labelIngresos.Text = "Total de Ingresos: $" + (totalDebito + totalCredito);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void iconButtonDetalle_Click(object sender, EventArgs e)
        {

            int idfactura = int.Parse(dataGridViewFacturas.CurrentRow.Cells[0].Value.ToString());
            string tipoPago = dataGridViewFacturas.CurrentRow.Cells[3].Value.ToString();
            string nombre = dataGridViewFacturas.CurrentRow.Cells[4].Value.ToString();
            string apellido = dataGridViewFacturas.CurrentRow.Cells[5].Value.ToString();
            Form frm = new Formularios.FormDetalleFactura(idfactura, nombre, apellido, tipoPago);
            frm.ShowDialog();
            
        }
    }
}
