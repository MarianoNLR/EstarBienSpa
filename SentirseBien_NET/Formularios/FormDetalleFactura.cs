using DGVPrinterHelper;
using SentirseBien_NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms_sentirseBien.Formularios
{
    public partial class FormDetalleFactura : Form
    {
        DataSet lineasFactura;
        BusinessLogicLayer _businessLogicLayer = new BusinessLogicLayer();
        public int IdFactura { get; set; }
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string TipoPago { get; set; }
        public FormDetalleFactura(int idfactura, string nombre, string apellido, string tipoPago)
        {
            DoubleBuffered = true;
            InitializeComponent();
            this.IdFactura = idfactura;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.TipoPago = tipoPago;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.lineasFactura = _businessLogicLayer.obtenerLineasFactura(this.IdFactura);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = lineasFactura.Tables[0];
            this.dataGridViewLineasFactura.DataSource = dt;
        }

        private void FormDetalleFactura_Load(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();

            printer.Title = "Detalles de Factura";
            printer.SubTitle = this.Nombre + " " + this.Apellido + "\n" + this.TipoPago;
            printer.TableAlignment = DGVPrinter.Alignment.Center;
            printer.PageNumbers = false;
            printer.PorportionalColumns = false;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;

            printer.HeaderCellAlignment = StringAlignment.Near;

            printer.PrintPreviewDataGridView(this.dataGridViewLineasFactura);
        }
    }
}
