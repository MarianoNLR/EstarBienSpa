using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentirseBien_NET.Modelos
{
    public class Servicio
    {
        public int Id_Servicio { get; set; }

        public string Nombre_Servicio { get; set; }

        public string Precio { get; set; }

        public string Descripcion { get; set; }

        public DateTime Modificacion { get; set; }


        public Servicio(int id_servicio, string nombre_servicio, string precio, string descripcion, DateTime modif)
        {
            this.Id_Servicio = id_servicio;
            this.Nombre_Servicio = nombre_servicio;
            this.Precio = precio;
            this.Descripcion = descripcion;
            this.Modificacion = modif;

        }
    }
}
