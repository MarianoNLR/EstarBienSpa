using SentirseBien_NET.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SentirseBien_NET.Formularios
{
    public partial class FormNuevo : Form
    {
        Paciente userActual;
        public FormNuevo(Paciente userActual)
        {
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

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
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
            Paciente nuevo = new Paciente(0, nombre, apellido, dni, direccion, email, nacimiento, telefono, new DateTime(),  passwordHash, rol);
        }

        private void textBoxDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void textBoxApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
    }
}
