using DGVPrinterHelper;
using SentirseBien_NET;
using SentirseBien_NET.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinForms_sentirseBien.Formularios
{

    
    public partial class FormStaff : Form
    {
        DataSet profesionales = new DataSet();
        BusinessLogicLayer _businessLogicLayer = new BusinessLogicLayer();
        Paciente userActual;
        public FormStaff(Paciente userActual)
        {
            InitializeComponent();
            this.userActual = userActual;
            btnFiltrar.Enabled = false;
            btnLimpiar.Enabled = false;

        }

        private void FormStaff_Load(object sender, EventArgs e)
        {
            
            

            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            DataTable dtfilter = new DataTable();
            dtfilter = this.profesionales.Tables[0];
            this.dataGridViewProfesionales.DataSource = dtfilter;
            dtfilter.DefaultView.RowFilter = $"Rol LIKE '{comboBox1.Text}%' AND DNI LIKE '{textBox1.Text}%'";
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
            textBox1.ResetText();
            DataTable dt = new DataTable();
            dt = profesionales.Tables[0];
            this.dataGridViewProfesionales.DataSource = dt;
            dt.DefaultView.RowFilter = "";
            if (dataGridViewProfesionales.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                return;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.profesionales = _businessLogicLayer.obtenerProfesionales();
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = profesionales.Tables[0];
            dataGridViewProfesionales.DataSource = dt;
            if (dataGridViewProfesionales.Rows.Count == 0)
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

            printer.Title = "Todos los Profesionales";
            if(comboBox1.Text != "")
            {
                printer.Title = "Profesionales de " + comboBox1.Text;
            }
            printer.TableAlignment = DGVPrinter.Alignment.Center;

            printer.PageNumbers = false;
            printer.PorportionalColumns = false;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
            printer.RowHeight = DGVPrinter.RowHeightSetting.DataHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;

            printer.PrintPreviewDataGridView(this.dataGridViewProfesionales);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerDesde_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void iconButtonDarBaja_Click(object sender, EventArgs e)
        {
            //DataGridViewRow row = this.dataGridViewProfesionales.SelectedRows[0];

            DialogResult dr = MessageBox.Show("¿Seguro desea dar de baja a " + dataGridViewProfesionales.CurrentRow.Cells[0].Value.ToString() + " " + dataGridViewProfesionales.CurrentRow.Cells[1].Value.ToString() + "?",
                      "Dar de Baja", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    _businessLogicLayer.DarDeBaja(dataGridViewProfesionales.CurrentRow.Cells[4].Value.ToString());
                    if (backgroundWorker1.IsBusy != true)
                    {
                        backgroundWorker1.RunWorkerAsync();
                    }
                    if (this.userActual.Email == dataGridViewProfesionales.CurrentRow.Cells[4].Value.ToString())
                    {
                        Application.Exit();
                    }
                    
                    break;
                case DialogResult.No:
                    break;
            }
        }
    }
}
