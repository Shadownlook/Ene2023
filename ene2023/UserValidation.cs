using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Ene2023_1
{
    public class UserValidation
    {
        private string connectionString;

        public UserValidation()
        {
            // Obtén la cadena de conexión del archivo de configuración
            connectionString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;
        }

        public bool ValidateUser(string username, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @Username AND Contraseña = @Password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
                catch (MySqlException ex)
                {
                    // Manejo de excepciones de MySQL
                    Console.WriteLine("Error de MySQL: " + ex.Message);
                    return false; // Cambia el manejo de errores según tus necesidades
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones generales
                    Console.WriteLine("Error: " + ex.Message);
                    return false; // Cambia el manejo de errores según tus necesidades
                }
            }
        }
    }
}
