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

                    // Verificar si la cuenta está bloqueada
                    if (IsAccountLocked(username))
                    {
                        Console.WriteLine("La cuenta está bloqueada. Comuníquese con el administrador.");
                        return false;
                    }

                    string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @Username AND Contraseña = @Password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        // Restablecer el contador de intentos incorrectos si el inicio de sesión es exitoso
                        ResetLoginAttempts(username);
                        return true;
                    }
                    else
                    {
                        // Incrementar el contador de intentos incorrectos
                        IncrementLoginAttempts(username);

                        // Bloquear la cuenta si se exceden los 3 intentos fallidos
                        if (GetLoginAttempts(username) >= 3)
                        {
                            LockAccount(username);
                            Console.WriteLine("Ha excedido el límite de intentos incorrectos. La cuenta está bloqueada.");
                        }
                        else
                        {
                            Console.WriteLine("Nombre de usuario o contraseña incorrectos.");
                        }

                        return false;
                    }
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

        private void IncrementLoginAttempts(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Usuarios SET intentos = intentos + 1 WHERE NombreUsuario = @Username";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Manejo de excepciones de MySQL
                    Console.WriteLine("Error de MySQL: " + ex.Message);
                    // Puedes agregar un manejo de errores específico si es necesario
                }
            }
        }

        private void ResetLoginAttempts(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Usuarios SET intentos = 0 WHERE NombreUsuario = @Username";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Manejo de excepciones de MySQL
                    Console.WriteLine("Error de MySQL: " + ex.Message);
                    // Puedes agregar un manejo de errores específico si es necesario
                }
            }
        }

        private int GetLoginAttempts(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT intentos FROM Usuarios WHERE NombreUsuario = @Username";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
                catch (MySqlException ex)
                {
                    // Manejo de excepciones de MySQL
                    Console.WriteLine("Error de MySQL: " + ex.Message);
                    // Puedes agregar un manejo de errores específico si es necesario
                }
            }

            return 0;
        }
        private void LockAccount(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Usuarios SET estado_de_cuenta = 'bloqueado' WHERE NombreUsuario = @Username";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Manejo de excepciones de MySQL
                    Console.WriteLine("Error de MySQL: " + ex.Message);
                    // Puedes agregar un manejo de errores específico si es necesario
                }
            }
        }
        public bool IsAccountLocked(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT estado_de_cuenta FROM Usuarios WHERE NombreUsuario = @Username";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    object result = command.ExecuteScalar();

                    if (result != null && result.ToString() == "bloqueado")
                    {
                        return true;
                    }
                }
                catch (MySqlException ex)
                {
                    // Manejo de excepciones de MySQL
                    Console.WriteLine("Error de MySQL: " + ex.Message);
                    // Puedes agregar un manejo de errores específico si es necesario
                }
            }

            return false;
        }
    }
}

