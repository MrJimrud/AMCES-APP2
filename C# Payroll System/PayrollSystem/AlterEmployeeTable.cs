using System;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    class AlterEmployeeTable
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                Console.WriteLine("Starting database update...");
                
                // Load database configuration
                DatabaseManager.LoadConfiguration();
                
                // Initialize connection
                string connectionString = $"server={DatabaseManager.DBServer};user id={DatabaseManager.DBUserID};password={DatabaseManager.DBPassword};database={DatabaseManager.DBName};charset=utf8;";
                
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    Console.WriteLine("Opening database connection...");
                    connection.Open();
                    
                    string alterTableSql = "ALTER TABLE employees ADD COLUMN employee_code VARCHAR(50) NOT NULL AFTER employee_id";
                    
                    Console.WriteLine("Executing SQL command: " + alterTableSql);
                    
                    using (MySqlCommand command = new MySqlCommand(alterTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    
                    Console.WriteLine("Database updated successfully!");
                    MessageBox.Show("Employee table updated successfully. Added 'employee_code' column.", 
                        "Database Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
                if (ex.Message.Contains("Duplicate column"))
                {
                    MessageBox.Show("The employee_code column already exists in the employees table.", 
                        "Column Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Database error: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}