using System;
using MySqlConnector;
using System.Windows.Forms;

namespace PayrollSystem
{
    public static class SimpleConnectionTest
    {
        public static void TestBasicConnection()
        {
            try
            {
                // Test the exact connection string format
                string connectionString = "Server=localhost;Database=payroll_system;Uid=root;Pwd=;Port=3306;";
                
                using (var connection = new MySqlConnection(connectionString))
                {
                    MessageBox.Show($"Attempting connection with: {connectionString}", "Connection Test");
                    
                    connection.Open();
                    MessageBox.Show("Connection opened successfully!", "Success");
                    
                    using (var command = new MySqlCommand("SELECT 'Hello from .NET' as message", connection))
                    {
                        var result = command.ExecuteScalar();
                        MessageBox.Show($"Query result: {result}", "Query Success");
                    }
                    
                    connection.Close();
                    MessageBox.Show("Connection closed successfully!", "Complete");
                }
            }
            catch (MySqlException mysqlEx)
            {
                MessageBox.Show($"MySQL Error: {mysqlEx.Message}\nError Code: {mysqlEx.Number}\nSQL State: {mysqlEx.SqlState}", 
                    "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General Error: {ex.Message}\nType: {ex.GetType().Name}\nStack: {ex.StackTrace}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}