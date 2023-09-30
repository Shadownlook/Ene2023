using System;
using System.Windows.Forms;
using Ene2023_1;

namespace ene2023
{
    public partial class Form1 : Form
    {
        private UserValidation userValidation;
        public Form1()
        {
            InitializeComponent();

            textBox2.PasswordChar = '*';
            userValidation = new UserValidation();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Lógica de autenticación utilizando UserValidation
            bool isAuthenticated = userValidation.ValidateUser(username, password);

            if (isAuthenticated)
            {
                MessageBox.Show("Inicio de sesión exitoso.");
                // Puedes abrir la ventana principal de la aplicación o realizar otras acciones aquí.
            }
            else
            {
                MessageBox.Show("Nombre de usuario o contraseña incorrectos.");
            }
        }
    }
}
