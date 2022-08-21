using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentirseBien_NET.Modelos
{
    class LineaFactura
    {
        public int idLineaFactura { get; set; }

        public int Cantidad { get; set; }

        public int Importe { get; set; }

        public string Nombre { get; set; }

        public int Precio { get; set; }

        public LineaFactura(int id, int cantidad, int importe, string nombre, int precio)
        {
            this.idLineaFactura = id;
            this.Cantidad = cantidad;
            this.Importe = importe;
            this.Nombre = nombre;
            this.Precio = precio;
        }


    }
}
