using System;
using MySqlConnector;
using System.Windows.Forms;

namespace PayrollSystem
{
    public static class ConnectionTest
    {
        public static void TestConnection()
        {
            try
            {
                string connectionString = "Server=localhost;Port=3306;Database=payroll_system;Uid=root;SslMode=none;";
                
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT username, password FROM users WHERE username='admin' AND is_active=1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader["username"].ToString();
                                string password = reader["password"].ToString();
                                MessageBox.Show($"SUCCESS!\nFound user: {username}\nPassword: {password}\nConnection works!", 
                                    "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("User 'admin' not found or not active!", 
                                    "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection FAILED!\nError: {ex.Message}\nType: {ex.GetType().Name}", 
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}