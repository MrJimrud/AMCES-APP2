using System;
using MySqlConnector;
using System.Windows.Forms;

namespace PayrollSystem
{
    public static class TestConnection
    {
        public static void TestDatabaseConnection()
        {
            try
            {
                // Test different connection string formats
                string[] connectionStrings = {
                    "server=localhost;user id=root;database=payroll_system;",
                    "server=localhost;uid=root;database=payroll_system;",
                    "server=127.0.0.1;user id=root;database=payroll_system;",
                    "server=localhost;port=3306;user id=root;database=payroll_system;",
                    "server=localhost;user id=root;password=;database=payroll_system;"
                };

                foreach (string connStr in connectionStrings)
                {
                    try
                    {
                        using (var connection = new MySqlConnection(connStr))
                        {
                            connection.Open();
                            using (var command = new MySqlCommand("SELECT 'Connection Success' as result", connection))
                            {
                                var result = command.ExecuteScalar();
                                MessageBox.Show($"SUCCESS with connection string: {connStr}\nResult: {result}", 
                                    "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return; // Exit on first success
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"FAILED with connection string: {connStr}\nError: {ex.Message}", 
                            "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                
                MessageBox.Show("All connection attempts failed!", "Connection Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Test error: {ex.Message}", "Connection Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
