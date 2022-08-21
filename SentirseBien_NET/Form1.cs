using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using SentirseBien_NET.Modelos;

namespace WinForms_sentirseBien
{
    public partial class Form1 : Form
    {
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;
        private Paciente userActual;
        public Form1(Paciente result)
        {
            InitializeComponent();
            userActual = result;
            if(result.Rol != "Administrador")
            {
                if (result.Rol != "Secretario")
                {
                    iconButton1.Hide();
                    iconButton3.Hide();
                    iconButton4.Hide();
                    iconButtonAgregarPersonal.Hide();
                }
                else
                {
                    iconButton1.Hide();
                    iconButton4.Hide();
                    iconButtonAgregarPersonal.Hide();
                }
            }
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(8, 70);
            panelMenu.Controls.Add(leftBorderBtn);
            //quitar barra del form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            
        }

        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                
                DisableButton();
                
                //boton
                currentBtn = (IconButton)senderBtn;
                //44; 80; 54
                currentBtn.BackColor = Color.FromArgb(201, 230, 225);
                //currentBtn.BackColor = Color.FromArgb(144, 79, 116);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //borde de boton izquierdo
                leftBorderBtn.BackColor = RGBColors.color0;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //cambia el icono hijo del formulario
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = RGBColors.color0;
                lblTitleChildForm.Text = currentBtn.Text;
                
            }
        }

        private void DisableButton()
        {
            if(currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(146, 79, 117);                
                currentBtn.ForeColor = RGBColors.color6;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.FromArgb(202, 230, 225);
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
                
            }
        }

        //para los colores de los iconos cuando se seleccionan
        private struct RGBColors
        {
            //44; 80; 54
            public static Color color0 = Color.FromArgb(44, 80, 54);
            //public static Color color1 = Color.FromArgb(218, 185, 204);            
            public static Color color1 = Color.FromArgb(146, 79, 117);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            //verde clarito
            public static Color color6 = Color.FromArgb(202, 230, 225);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            
            OpenChildForm(new Formularios.FormClientes(this.userActual));
            
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            
            OpenChildForm(new Formularios.FormTurnos(this.userActual));
            
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            
            OpenChildForm(new Formularios.FormPagos(this.userActual));
            
        }

        public void iconButton4_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            
            OpenChildForm(new Formularios.FormStaff(this.userActual));
            
        }

        private void iconButtonAgregarPersonal_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);

            OpenChildForm(new Formularios.FormNuevo(this.userActual));

        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            //falta poner la condicion si no hay hijos
            currentChildForm.Close();
            Reset();
        }

        private void OpenChildForm(Form childForm)
        {
            //open only form
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            //End


            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = childForm.Text;
        }
        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            //cambia el icono hijo a los valores defecto
            iconCurrentChildForm.IconChar = IconChar.Home;
            iconCurrentChildForm.IconColor = RGBColors.color0;
            lblTitleChildForm.Text = "Home";
        }

        //arrastrar Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]

        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]

        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconPictureBox3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            
            else
                WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //habilita timer
            timer1.Enabled = true;
            OpenChildForm(new Formularios.FormTurnos(this.userActual));
            iconButton2.PerformClick();
            this.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void iconCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Close();
            Form frm = new Formularios.FormLogin();
            frm.Show();
        }

        
    }
}
