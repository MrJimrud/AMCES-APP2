using System;
using MySqlConnector;
using System.Windows.Forms;

namespace PayrollSystem
{
    public static class ConnectionStringDebugger
    {
        public static void TestConnectionString()
        {
            try
            {
                // Load configuration
                DatabaseManager.LoadConfiguration();
                
                // Show current configuration values
                MessageBox.Show($"Configuration Values:\n" +
                    $"DBServer: '{DatabaseManager.DBServer}'\n" +
                    $"DBName: '{DatabaseManager.DBName}'\n" +
                    $"DBUserID: '{DatabaseManager.DBUserID}'\n" +
                    $"DBPassword: '{DatabaseManager.DBPassword}'\n",
                    "Configuration Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Test multiple connection string formats
                string[] testConnections = {
                    $"server={DatabaseManager.DBServer};port=3306;database={DatabaseManager.DBName};uid={DatabaseManager.DBUserID};pwd={DatabaseManager.DBPassword ?? ""};sslmode=none;",
                    $"Server={DatabaseManager.DBServer};Port=3306;Database={DatabaseManager.DBName};Uid={DatabaseManager.DBUserID};Pwd={DatabaseManager.DBPassword ?? ""};SslMode=none;",
                    $"server={DatabaseManager.DBServer};port=3306;database={DatabaseManager.DBName};user={DatabaseManager.DBUserID};password={DatabaseManager.DBPassword ?? ""};sslmode=none;",
                    $"server={DatabaseManager.DBServer};database={DatabaseManager.DBName};uid={DatabaseManager.DBUserID};pwd={DatabaseManager.DBPassword ?? ""};",
                    $"server={DatabaseManager.DBServer};port=3306;database={DatabaseManager.DBName};uid={DatabaseManager.DBUserID};sslmode=none;"
                };
                
                for (int i = 0; i < testConnections.Length; i++)
                {
                    try
                    {
                        string connStr = testConnections[i];
                        MessageBox.Show($"Testing connection string {i + 1}:\n{connStr}", 
                            "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        using (var connection = new MySqlConnection(connStr))
                        {
                            connection.Open();
                            MessageBox.Show($"✅ SUCCESS with connection string {i + 1}!\n" +
                                $"Server Version: {connection.ServerVersion}\n" +
                                $"Database: {connection.Database}\n" +
                                $"Connection State: {connection.State}", 
                                "Connection Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Test a simple query
                            using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = 'admin'", connection))
                            {
                                var count = cmd.ExecuteScalar();
                                MessageBox.Show($"✅ Query test successful!\nAdmin user count: {count}", 
                                    "Query Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            
                            connection.Close();
                            return; // Exit on first successful connection
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Connection string {i + 1} FAILED:\n{ex.Message}\n\nTrying next...", 
                            "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                MessageBox.Show("❌ ALL connection strings failed!", "All Tests Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Debug test error:\n{ex.Message}\n\nStack:\n{ex.StackTrace}", 
                    "Debug Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}