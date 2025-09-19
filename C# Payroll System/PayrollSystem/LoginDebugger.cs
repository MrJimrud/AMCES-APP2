using System;
using MySqlConnector;
using System.Windows.Forms;

namespace PayrollSystem
{
    public static class LoginDebugger
    {
        public static void TestLoginProcess()
        {
            try
            {
                MessageBox.Show("Starting login debug test...", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Test 1: Database connection
                string connectionString = "Server=localhost;Port=3306;Database=payroll_system;Uid=root;SslMode=none;";
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("✅ Database connection successful!", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Test 2: Check admin user with correct password
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password AND is_active = 1";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", "admin");
                        cmd.Parameters.AddWithValue("@password", "admin"); // Correct password from database
                        
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        if (count > 0)
                        {
                            MessageBox.Show($"✅ Login test PASSED! Found {count} matching user(s).\nUsername: admin\nPassword: admin", 
                                "Debug - SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("❌ Login test FAILED! No matching user found.", 
                                "Debug - FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    
                    // Test 3: Show all users for verification
                    string allUsersQuery = "SELECT username, password, is_active FROM users";
                    using (var cmd = new MySqlCommand(allUsersQuery, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            string userList = "All users in database:\n";
                            while (reader.Read())
                            {
                                userList += $"Username: {reader["username"]}, Password: {reader["password"]}, Active: {reader["is_active"]}\n";
                            }
                            MessageBox.Show(userList, "Debug - All Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Debug test failed!\nError: {ex.Message}\nStack: {ex.StackTrace}", 
                    "Debug - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}