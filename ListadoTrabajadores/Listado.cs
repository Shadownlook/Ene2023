using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace ListadoTrabajadores
{
    public partial class Form1 : Form
    {
        private MySqlConnection mysqlConnection;
        private MySqlDataAdapter mysqlDataAdapter;
        private DataTable dataTable;
        public Form1()
        {
            InitializeComponent();

            // Obtener la cadena de conexión desde el archivo de configuración
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

            // Crear la conexión MySQL
            mysqlConnection = new MySqlConnection(connectionString);

            // Configurar la consulta SQL para seleccionar los datos de la tabla "trabajadores"
            string sqlQuery = "SELECT RutEmpleado, Nombre, Dirección, Teléfono, ValorHora, ValorExtra FROM trabajadores";

            // Crear el adaptador de datos MySQL
            mysqlDataAdapter = new MySqlDataAdapter(sqlQuery, mysqlConnection);

            // Crear el DataTable
            dataTable = new DataTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                // Abrir la conexión a la base de datos
                mysqlConnection.Open();

                // Llenar el DataTable con los datos de la base de datos
                mysqlDataAdapter.Fill(dataTable);

                // Vincular el DataGridView al DataTable
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Manejar errores, por ejemplo, mostrar un mensaje de error
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
            finally
            {
                // Asegurarse de cerrar la conexión cuando se termine
                if (mysqlConnection.State == ConnectionState.Open)
                {
                    mysqlConnection.Close();
                }
            }
        }
    }
}