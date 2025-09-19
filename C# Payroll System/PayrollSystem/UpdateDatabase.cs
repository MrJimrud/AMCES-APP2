using System;
using System.Windows.Forms;

namespace PayrollSystem
{
    class UpdateDatabase
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                // Initialize global variables
                GlobalVariables.ResetVariables();
                
                // Load configuration
                GlobalVariables.InitializeFromConfig();
                
                // Check if connection is configured
                if (!GlobalVariables.IsConnectionConfigured())
                {
                    MessageBox.Show("Failed to load configuration. Please check your config.ini file.",
                        "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Initialize database connection
                DatabaseManager.InitializeConnection();
                
                // Run the database update
                bool success = UpdateEmployeeTable.AddEmployeeCodeColumn();
                
                if (success)
                {
                    Console.WriteLine("Database updated successfully.");
                    MessageBox.Show("Database updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Console.WriteLine("Database update failed.");
                    MessageBox.Show("Database update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}