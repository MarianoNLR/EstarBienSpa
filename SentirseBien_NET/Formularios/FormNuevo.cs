using SentirseBien_NET;
using SentirseBien_NET.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms_sentirseBien.Formularios
{
    public partial class FormNuevo : Form
    {
        Paciente userActual;
        BusinessLogicLayer _businessLogicLayer = new BusinessLogicLayer();
        public FormNuevo(Paciente userActual)
        {
            DoubleBuffered = true;
            InitializeComponent();
            this.userActual = userActual;
            
        }

        private void textBoxNombre_TextChanged(object sender, EventArgs e)
        {

        }

        public void OnGetFocus(object sender, EventArgs e)
        {
            //Comprobamos si el texto es el default, y si lo es lo borramos
            if (textBoxNombre.Text.Contains(textBoxNombre.Tag.ToString()))
                textBoxNombre.Text = "";
            //Ponemos el color en negro
            textBoxNombre.ForeColor = Color.Black;

        }

        public void OnLostFocus(object sender, EventArgs e)
        {
            //En caso de que no haya texto, añadimos el texto por defecto y ponemos el color en gris
            if (String.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                textBoxNombre.Text = textBoxNombre.Tag.ToString();
                textBoxNombre.ForeColor = Color.Gray;

            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


        private Boolean email_isCorrect()
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(textBoxEmail.Text, expresion))
            {
                if (Regex.Replace(textBoxEmail.Text, expresion, String.Empty).Length == 0)
                    return true;
            }
            return false;
        }

        private bool verificarDatos()
        {
            if (textBoxNombre.Text == "" || textBoxApellido.Text == "" || textBoxDNI.Text == "" || textBoxDireccion.Text == "" || textBoxEmail.Text == "" || textBoxTelefono.Text == "" || comboBoxRol.Text == "" || textBoxPassword.Text == "")
            {
                return false;
            }
            if (!email_isCorrect())
            {
                return false;
            }
            return true;
        }

        private void FormNuevo_Load(object sender, EventArgs e)
        {

        }

        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {
            if (verificarDatos())
            {
                btnRegistrar.Enabled = false;
                string nombre = textBoxNombre.Text;
                string apellido = textBoxApellido.Text;
                string email = textBoxEmail.Text;
                string dni = textBoxDNI.Text;
                string telefono = textBoxTelefono.Text;
                string direccion = textBoxDireccion.Text;
                string password = textBoxPassword.Text;
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                string rol = comboBoxRol.Text;
                DateTime nacimiento = dateTimePickerNacimiento.Value;
                Paciente nuevo = new Paciente(0, nombre, apellido, dni, direccion, email, nacimiento, telefono, new DateTime(), passwordHash, rol);
                bool result = _businessLogicLayer.agregarPersonal(nuevo);

                if (result)
                {
                    MessageBox.Show("Cuenta creada para: " + nombre + "\nCon rol: " + rol);
                    textBoxNombre.ResetText();
                    textBoxApellido.ResetText();
                    textBoxDNI.ResetText();
                    textBoxDireccion.ResetText();
                    textBoxEmail.ResetText();
                    textBoxPassword.ResetText();
                    textBoxTelefono.ResetText();
                    comboBoxRol.ResetText();
                    dateTimePickerNacimiento.ResetText();
                    btnRegistrar.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No se pudo crear la cuenta. Intente nuevamente con otros datos");
                    btnRegistrar.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Ingreso de datos invalido!");
                btnRegistrar.Enabled = true;
            }
        }

        private void textBoxNombre_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Space))
            {
                e.Handled = true;
                return;
            }
        }

        private void textBoxApellido_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Space))
            {
                e.Handled = true;
                return;
            }
        }

        private void textBoxDNI_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxTelefono_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
