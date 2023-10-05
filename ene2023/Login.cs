using System;
using System.Windows.Forms;
using Ene2023_1;
using MenuServicios;

namespace ene2023
{
    public partial class Login : Form
    {
        private UserValidation userValidation;
        public Login()
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
                // Oculta el formulario de inicio de sesión
                this.Hide();

                // Muestra el formulario del menú principal
                MenuPrincipal menuForm = new MenuPrincipal();
                menuForm.ShowDialog();

                // Cuando el menú principal se cierre, puedes realizar acciones adicionales si es necesario
                // Por ejemplo, puedes mostrar de nuevo el formulario de inicio de sesión si el usuario cierra el menú principal
                this.Show();

            }
            else
            {
                MessageBox.Show("Nombre de usuario o contraseña incorrectos.");
            }
        }
    }
}
