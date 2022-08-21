using MySql.Data.MySqlClient;
using SentirseBien_NET.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentirseBien_NET
{
    public class DataAccessLayer
    {
        public Paciente userActual { get; set; }
        private MySqlConnection conn = new MySqlConnection("Server=undefined; Database=undefined; User id=root;Pwd=root;");

        public Paciente IniciarSesion(string email, string password)
        {
            using (MySqlConnection con = this.conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from estarbienspa.Paciente where email=@email", con))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    MySqlDataReader reader = cmd.ExecuteReader();




                    while (reader.Read())
                    {
                        string pass_user = reader["password"].ToString();
                        if (BCrypt.Net.BCrypt.Verify(password, pass_user))
                        {
                            int id = int.Parse(reader["id"].ToString());
                            string nombre = reader["nombre"].ToString();
                            string apellido = reader["apellido"].ToString();
                            string dni = reader["dni"].ToString();
                            string direccion = reader["direccion"].ToString();
                            string email_user = reader["email"].ToString();
                            DateTime nacimiento = DateTime.Parse(reader["fechaNacimiento"].ToString());
                            string telefono = reader["telContacto"].ToString();
                            DateTime creacion = DateTime.Parse(reader["fechaCreacion"].ToString());
                            //string pass_user = reader["password"].ToString();
                            string rol = reader["rol"].ToString();
                            if(rol == "")
                            {
                                return null;
                            }
                            userActual = new Paciente(id, nombre, apellido, dni, direccion, email_user, nacimiento, telefono, creacion, pass_user, rol);
                            Debug.WriteLine(userActual.ToString());
                            
                            return userActual;
                        }
                        else
                        {
                            return null;
                        }
                        
                    }

                }
            }
            return null;
        }

        public DataSet obtenerClientes()
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection con = this.conn)
                {
                    string query;

                    query = @"SELECT estarbienspa.Paciente.nombre, estarbienspa.Paciente.apellido, estarbienspa.Paciente.dni, estarbienspa.Paciente.direccion, estarbienspa.Paciente.email, estarbienspa.Paciente.telContacto FROM estarbienspa.Paciente where estarbienspa.Paciente.rol is null order by nombre, apellido;";
                    

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(ds);
                        }
                    }

                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataSet obtenerProfesionales()
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection con = this.conn)
                {
                    string query;

                    query = @"SELECT * FROM estarbienspa.Paciente where estarbienspa.Paciente.rol IS NOT NULL;";


                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(ds);
                        }
                    }

                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet obtenerPagos()
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection con = this.conn)
                {
                    string query;

                    query = @"SELECT idfactura, DATE_FORMAT(fecha,'%Y-%m-%d') as Fecha, importe, tipo_pago, estarbienspa.Paciente.nombre, estarbienspa.Paciente.apellido, estarbienspa.Paciente.email, estarbienspa.Paciente.dni  FROM estarbienspa.Factura INNER JOIN estarbienspa.Paciente ON (estarbienspa.Factura.idusuario_fk = estarbienspa.Paciente.id) order by Fecha;"; 


                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(ds);
                        }
                    }

                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataSet obtenerCitas(string rolActual)
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection con = this.conn)
                {
                    string query;

                    if(rolActual == "Administrador" || rolActual == "Secretario")
                    {
                        query = @"SELECT estarbienspa.Cita.fecha, estarbienspa.Cita.hora , estarbienspa.Servicio.nombre_servicio, estarbienspa.Servicio.precio, 
                                        estarbienspa.Paciente.nombre, estarbienspa.Paciente.apellido, estarbienspa.Paciente.dni, estarbienspa.Paciente.direccion, estarbienspa.Paciente.email, 
                                        estarbienspa.Paciente.telContacto 
                                        FROM estarbienspa.Cita 
                                        INNER JOIN estarbienspa.Servicio on (estarbienspa.Cita.servicio_id = estarbienspa.Servicio.id)
                                        INNER JOIN estarbienspa.Paciente ON (estarbienspa.Cita.paciente_id = estarbienspa.Paciente.id) order by estarbienspa.Cita.fecha, estarbienspa.Cita.hora;";
                    }
                    else
                    {
                        query = @"SELECT estarbienspa.Cita.fecha, estarbienspa.Cita.hora , estarbienspa.Servicio.nombre_servicio, estarbienspa.Servicio.precio, 
                                        estarbienspa.Paciente.nombre, estarbienspa.Paciente.apellido, estarbienspa.Paciente.dni, estarbienspa.Paciente.direccion, estarbienspa.Paciente.email, 
                                        estarbienspa.Paciente.telContacto 
                                        FROM estarbienspa.Cita 
                                        INNER JOIN estarbienspa.Servicio on (estarbienspa.Cita.servicio_id = estarbienspa.Servicio.id)
                                        INNER JOIN estarbienspa.Paciente ON (estarbienspa.Cita.paciente_id = estarbienspa.Paciente.id) where estarbienspa.Servicio.nombre_servicio = @rol order by estarbienspa.Cita.fecha, estarbienspa.Cita.hora asc;";
                    }
                    
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@rol", rolActual);
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(ds);
                        }
                    }

                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AgregarPersonal(Paciente nuevo)
        {
            try
            {
                using (MySqlConnection con = this.conn)
                {
                    con.Open();
                    string query;

                    query = @"INSERT INTO estarbienspa.Paciente (nombre, apellido, dni, direccion, email, fechaNacimiento, telContacto, password, rol) 
                                VALUES (@nombre, @apellido, @dni, @direccion, @email, @nacimiento, @telefono, @password, @rol);";


                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nuevo.Nombre);
                        cmd.Parameters.AddWithValue("@apellido", nuevo.Apellido);
                        cmd.Parameters.AddWithValue("@dni", nuevo.DNI);
                        cmd.Parameters.AddWithValue("@direccion", nuevo.Direccion);
                        cmd.Parameters.AddWithValue("@email", nuevo.Email);
                        cmd.Parameters.AddWithValue("@nacimiento", nuevo.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@telefono", nuevo.TelContacto);
                        cmd.Parameters.AddWithValue("@password", nuevo.Password);
                        cmd.Parameters.AddWithValue("@rol", nuevo.Rol);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    return true;

                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void DarDeBaja(string email)
        {
            try
            {
                using (MySqlConnection con = this.conn)
                {
                    con.Open();
                    string query;

                    query = @"DELETE FROM estarbienspa.Paciente where email=@email";


                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();

                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet obtenerLineasFactura(int idfactura)
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection con = this.conn)
                {
                    string query;

                    query = @"SELECT estarbienspa.Linea_Factura.cantidad, estarbienspa.Linea_Factura.importe, estarbienspa.Servicio.nombre_servicio as Nombre, estarbienspa.Servicio.precio
                                FROM estarbienspa.Linea_Factura  
                                INNER JOIN estarbienspa.Servicio ON (estarbienspa.Linea_Factura.servicio_fk = estarbienspa.Servicio.id) where estarbienspa.Linea_Factura.idfactura_fk = @idfactura;";


                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@idfactura", idfactura);
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(ds);
                        }
                    }

                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
