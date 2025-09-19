using System;
using MySqlConnector;
using System.Windows.Forms;

namespace PayrollSystem
{
    public static class DirectConnectionTest
    {
        public static void TestDirectConnection()
        {
            try
            {
                MessageBox.Show("Starting direct MySqlConnector test...", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Test multiple connection string formats
                string[] connectionStrings = {
                    "server=localhost;port=3306;database=payroll_system;uid=root;pwd=;sslmode=none;",
                    "Server=localhost;Port=3306;Database=payroll_system;Uid=root;Pwd=;SslMode=none;",
                    "server=127.0.0.1;port=3306;database=payroll_system;uid=root;pwd=;sslmode=none;",
                    "server=localhost;database=payroll_system;uid=root;pwd=;",
                    "server=localhost;port=3306;database=payroll_system;user=root;password=;sslmode=none;"
                };
                
                for (int i = 0; i < connectionStrings.Length; i++)
                {
                    try
                    {
                        MessageBox.Show($"Testing connection string {i + 1}:\n{connectionStrings[i]}", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        using (var connection = new MySqlConnection(connectionStrings[i]))
                        {
                            connection.Open();
                            MessageBox.Show($"✅ SUCCESS with connection string {i + 1}!\nServer Version: {connection.ServerVersion}\nDatabase: {connection.Database}", 
                                "Connection Test - SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Test a simple query
                            using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = 'admin'", connection))
                            {
                                var count = cmd.ExecuteScalar();
                                MessageBox.Show($"✅ Query test successful!\nAdmin user count: {count}", 
                                    "Query Test - SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            
                            connection.Close();
                            MessageBox.Show($"✅ Connection {i + 1} closed successfully!", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return; // Exit on first successful connection
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Connection string {i + 1} FAILED:\n{ex.Message}\n\nTrying next...", 
                            "Connection Test - FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                MessageBox.Show("❌ ALL connection strings failed!", "Connection Test - ALL FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Test framework error:\n{ex.Message}\n\nStack:\n{ex.StackTrace}", 
                    "Connection Test - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}