using SentirseBien_NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WinForms_sentirseBien.Formularios
{
    
    public partial class FormLogin : Form
    {
        BusinessLogicLayer _businessLogicLayer = new BusinessLogicLayer();
        public FormLogin()
        {
            InitializeComponent();
            labelAviso.Visible = false;
            labelAvisoCampos.Visible = false;
            lblcorreoInvalido.Visible = false;
            
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            labelAviso.Visible = false;
            labelAvisoCampos.Visible = false;
            if (txtBox_email.Text == "" || txtBox_pass.Text == "" || !email_isCorrect())
            {
                labelAvisoCampos.Visible = true;
            }
            else
            {
                btnAceptar.Enabled = false;
                var result = _businessLogicLayer.iniciarSesion(txtBox_email.Text.Trim(), txtBox_pass.Text.Trim());
                
                if (result != null)
                {
                    //Acá abre el form principal
                    Form1 frm = new Form1(result);
                    this.Hide();
                    frm.Show();
                    //Debug.WriteLine(result.ToString());
                }
                else
                {
                    labelAviso.Text = "Datos Incorrectos";
                    btnAceptar.Enabled = true;
                    labelAviso.Visible = true;
                    this.ActiveControl = txtBox_email;
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private Boolean email_isCorrect()
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(txtBox_email.Text, expresion))
            {
                if (Regex.Replace(txtBox_email.Text, expresion, String.Empty).Length == 0)
                    return true;
            }
            return false;
        }

        private void txtBox_email_Leave(object sender, EventArgs e)
        {
            lblcorreoInvalido.Visible = !email_isCorrect() ? true : false;
        }

        private void txtBox_email_Enter(object sender, EventArgs e)
        {
            labelAviso.Visible = false;
            labelAvisoCampos.Visible = false;
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconPictureBox3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}
