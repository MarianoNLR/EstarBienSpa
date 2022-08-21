using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentirseBien_NET.Modelos
{
    public class Paciente
    {
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

        public string Rol { get; set; }
        public Paciente(int id_paciente, string nombre, string apellido, string dni, string direccion, string email, DateTime nacimiento, string contacto, DateTime creacion, string password, string rol)
        {
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
            this.Rol = rol;
        }

        public override string ToString()
        {
            return this.Nombre + "\n" + this.Apellido + "\n" + this.DNI + "\n" + this.Direccion + "\n" + this.Email + "\n" + this.FechaNacimiento + "\n" + this.TelContacto + "\n" + this.Password + "\n" + this.Rol;
        }
    }
}
