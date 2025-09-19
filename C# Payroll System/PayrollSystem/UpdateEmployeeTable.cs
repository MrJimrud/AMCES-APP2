using System;
using System.Windows.Forms;

namespace PayrollSystem
{
    public static class UpdateEmployeeTable
    {
        public static bool AddEmployeeCodeColumn()
        {
            try
            {
                string alterTableSql = "ALTER TABLE employees ADD COLUMN employee_code VARCHAR(50) NOT NULL AFTER employee_id";
                
                // Initialize database connection
                DatabaseManager.InitializeConnection();
                
                // Execute the ALTER TABLE command
                int result = DatabaseManager.ExecuteNonQuery(alterTableSql);
                
                MessageBox.Show("Employee table updated successfully. Added 'employee_code' column.", 
                    "Database Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee table: {ex.Message}", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}