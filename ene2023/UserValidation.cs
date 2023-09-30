using System;
using System.Configuration;
using System.Data.SqlClient;

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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @Username AND Contraseña = @Password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
                catch (SqlException ex)
                {
                    // Manejo de excepciones de SQL
                    Console.WriteLine("Error de SQL: " + ex.Message);
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
