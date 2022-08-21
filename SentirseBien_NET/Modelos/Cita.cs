using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentirseBien_NET.Modelos
{
    public class Cita
    {
        public int Id_Cita { get; set; }

        public DateTime Fecha { get; set; }

        public string Hora { get; set; }

        public int Servicio_Id { get; set; }

        public int Paciente_Id { get; set; }

        public int Id_Servicio { get; set; }

        public string NombreServicio { get; set; }

        public string Precio { get; set; }

        public string Descripcion { get; set; }

        public DateTime Modificacion { get; set; }

        public int Id_Paciente { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string DNI { get; set; }

        public string Direccion { get; set; }

        public string Email { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string TelContacto { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string Password { get; set; }

        public Cita(int id, DateTime fecha, string hora, int servicio_id, int paciente_id, int id_servicio, string nombre_servicio, string precio, string descripcion, DateTime modif, int id_paciente, string nombre, string apellido, string dni, string direccion, string email, DateTime nacimiento, string contacto, DateTime creacion, string password)
        {
            this.Id_Cita = id;
            this.Fecha = fecha;
            this.Hora = hora;
            this.Servicio_Id = servicio_id;
            this.Paciente_Id = paciente_id;
            this.Id_Servicio = id_servicio;
            this.NombreServicio = nombre_servicio;
            this.Precio = precio;
            this.Descripcion = descripcion;
            this.Modificacion = modif;
            this.Id_Paciente = id_paciente;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.DNI = dni;
            this.Direccion = direccion;
            this.Email = email;
            this.FechaNacimiento = nacimiento;
            this.TelContacto = contacto;
            this.FechaCreacion = creacion;
            this.Password = password;
        }
    }
}
