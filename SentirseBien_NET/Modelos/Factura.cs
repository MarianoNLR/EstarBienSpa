using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentirseBien_NET.Modelos
{
    class Factura
    {

        public int IdFactura { get; set; }

        public string Fecha { get; set; }

        public int Importe { get; set; }

        public string Tipo_Pago { get; set; }

        public int IdPaciente { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string DNI { get; set; }

        public Factura(int idfactura, string fecha, int importe, string tipopago, int idpaciente, string nombrepaciente, string apellidopaciente, string dni)
        {
            this.IdFactura = idfactura;
            this.Fecha = fecha;
            this.Importe = importe;
            this.Tipo_Pago = tipopago;
            this.IdPaciente = idpaciente;
            this.Nombre = nombrepaciente;
            this.Apellido = apellidopaciente;
            this.DNI = dni;
        }
    }
}
