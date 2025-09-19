using System;
using System.Data;
using MySqlConnector;
using System.Windows.Forms;
using System.IO;

namespace PayrollSystem
{
    /// <summary>
    /// Manages cash advance table validation and automatic fixing
    /// </summary>
    public static class CashAdvanceTableManager
    {
        /// <summary>
        /// Validates and fixes cash advance table issues
        /// </summary>
        /// <returns>True if tables are valid or successfully fixed</returns>
        public static bool ValidateAndFixTables()
        {
            try
            {
                // Check if main cash_advances table exists
                if (!TableExists("cash_advances"))
                {
                    CreateCashAdvancesTable();
                }
                
                // Check if legacy tbl_ca table exists and migrate data
                if (TableExists("tbl_ca"))
                {
                    MigrateLegacyData();
                }
                
                // Ensure required columns exist
                EnsureRequiredColumns();
                
                // Create or update views for backward compatibility
                CreateCompatibilityViews();
                
                // Create indexes for performance
                CreateIndexes();
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating cash advance tables: {ex.Message}", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        /// <summary>
        /// Checks if a table exists in the database
        /// </summary>
        private static bool TableExists(string tableName)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM information_schema.tables 
                               WHERE table_schema = @database AND table_name = @tableName";
                
                var parameters = new MySqlParameter[]
                {
                    DatabaseManager.CreateParameter("@database", DatabaseManager.DBName),
                    DatabaseManager.CreateParameter("@tableName", tableName)
                };
                
                object result = DatabaseManager.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Creates the main cash_advances table
        /// </summary>
        private static void CreateCashAdvancesTable()
        {
            string createTableQuery = @"
                CREATE TABLE cash_advances (
                    advance_id INT AUTO_INCREMENT PRIMARY KEY,
                    employee_id VARCHAR(20) NOT NULL,
                    request_date DATE NOT NULL,
                    amount DECIMAL(10,2) NOT NULL,
                    reason TEXT,
                    status ENUM('Pending', 'Approved', 'Rejected', 'Paid', 'Deducted') DEFAULT 'Pending',
                    approved_by INT,
                    approved_date DATE,
                    payment_date DATE,
                    deduction_start_period INT,
                    deduction_amount DECIMAL(10,2),
                    monthly_deduction DECIMAL(10,2),
                    deduction_months INT DEFAULT 1,
                    remaining_balance DECIMAL(10,2),
                    remarks TEXT,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                    INDEX idx_employee (employee_id),
                    INDEX idx_status (status),
                    INDEX idx_request_date (request_date)
                )";
            
            DatabaseManager.ExecuteNonQuery(createTableQuery);
            
            // Add foreign key constraints if tables exist
            try
            {
                if (TableExists("employees"))
                {
                    DatabaseManager.ExecuteNonQuery(
                        "ALTER TABLE cash_advances ADD FOREIGN KEY (employee_id) REFERENCES employees(employee_id)");
                }
                
                if (TableExists("users"))
                {
                    DatabaseManager.ExecuteNonQuery(
                        "ALTER TABLE cash_advances ADD FOREIGN KEY (approved_by) REFERENCES users(user_id)");
                }
                
                if (TableExists("payroll_periods"))
                {
                    DatabaseManager.ExecuteNonQuery(
                        "ALTER TABLE cash_advances ADD FOREIGN KEY (deduction_start_period) REFERENCES payroll_periods(period_id)");
                }
            }
            catch (Exception ex)
            {
                // Foreign key constraints may fail if referenced tables don't exist yet
                // This is acceptable as they can be added later
                System.Diagnostics.Debug.WriteLine($"Foreign key constraint warning: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Migrates data from legacy tbl_ca table
        /// </summary>
        private static void MigrateLegacyData()
        {
            try
            {
                // Check if tbl_ca has data
                string checkDataQuery = "SELECT COUNT(*) FROM tbl_ca";
                int recordCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkDataQuery));
                
                if (recordCount > 0)
                {
                    // Migrate data from tbl_ca to cash_advances
                    string migrateQuery = @"
                        INSERT IGNORE INTO cash_advances 
                        (employee_id, amount, request_date, status, remaining_balance, created_at, updated_at)
                        SELECT 
                            employee_id,
                            COALESCE(cash_advance, amount, 0) as amount,
                            COALESCE(payrollperiod, CURDATE()) as request_date,
                            CASE 
                                WHEN status IS NULL OR status = '' THEN 'Approved'
                                ELSE status
                            END as status,
                            COALESCE(cash_advance, amount, 0) as remaining_balance,
                            COALESCE(created_at, NOW()) as created_at,
                            COALESCE(updated_at, NOW()) as updated_at
                        FROM tbl_ca 
                        WHERE COALESCE(cash_advance, amount, 0) > 0";
                    
                    int migratedRecords = DatabaseManager.ExecuteNonQuery(migrateQuery);
                    
                    if (migratedRecords > 0)
                    {
                        MessageBox.Show($"Successfully migrated {migratedRecords} records from tbl_ca to cash_advances.", 
                            "Migration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Warning: Could not migrate data from tbl_ca: {ex.Message}", 
                    "Migration Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        /// <summary>
        /// Ensures all required columns exist in the cash_advances table
        /// </summary>
        private static void EnsureRequiredColumns()
        {
            try
            {
                // Check and add missing columns
                string[] requiredColumns = {
                    "monthly_deduction DECIMAL(10,2)",
                    "deduction_months INT DEFAULT 1"
                };
                
                foreach (string column in requiredColumns)
                {
                    string columnName = column.Split(' ')[0];
                    if (!ColumnExists("cash_advances", columnName))
                    {
                        string alterQuery = $"ALTER TABLE cash_advances ADD COLUMN {column}";
                        DatabaseManager.ExecuteNonQuery(alterQuery);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Column addition warning: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Checks if a column exists in a table
        /// </summary>
        private static bool ColumnExists(string tableName, string columnName)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM information_schema.columns 
                               WHERE table_schema = @database AND table_name = @tableName AND column_name = @columnName";
                
                var parameters = new MySqlParameter[]
                {
                    DatabaseManager.CreateParameter("@database", DatabaseManager.DBName),
                    DatabaseManager.CreateParameter("@tableName", tableName),
                    DatabaseManager.CreateParameter("@columnName", columnName)
                };
                
                object result = DatabaseManager.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Creates views for backward compatibility
        /// </summary>
        private static void CreateCompatibilityViews()
        {
            try
            {
                // Create vw_cash_advances view
                string vwCashAdvancesQuery = @"
                    CREATE OR REPLACE VIEW vw_cash_advances AS
                    SELECT 
                        advance_id as id,
                        advance_id,
                        advance_id as cash_advance_id,
                        employee_id,
                        request_date,
                        amount,
                        amount as cash_advance,
                        reason,
                        status,
                        approved_by,
                        approved_date,
                        payment_date,
                        deduction_start_period,
                        deduction_amount,
                        monthly_deduction,
                        deduction_months,
                        remaining_balance,
                        remarks,
                        created_at,
                        updated_at
                    FROM cash_advances";
                
                DatabaseManager.ExecuteNonQuery(vwCashAdvancesQuery);
                
                // Create tbl_ca view for legacy compatibility
                string tblCaViewQuery = @"
                    CREATE OR REPLACE VIEW tbl_ca AS
                    SELECT 
                        advance_id as id,
                        employee_id,
                        amount as cash_advance,
                        request_date as payrollperiod,
                        status,
                        remaining_balance,
                        created_at,
                        updated_at
                    FROM cash_advances";
                
                DatabaseManager.ExecuteNonQuery(tblCaViewQuery);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"View creation warning: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Creates indexes for better performance
        /// </summary>
        private static void CreateIndexes()
        {
            try
            {
                string[] indexes = {
                    "CREATE INDEX IF NOT EXISTS idx_cash_advance_employee ON cash_advances(employee_id)",
                    "CREATE INDEX IF NOT EXISTS idx_cash_advance_status ON cash_advances(status)",
                    "CREATE INDEX IF NOT EXISTS idx_cash_advance_date ON cash_advances(request_date)",
                    "CREATE INDEX IF NOT EXISTS idx_cash_advance_period ON cash_advances(deduction_start_period)"
                };
                
                foreach (string indexQuery in indexes)
                {
                    DatabaseManager.ExecuteNonQuery(indexQuery);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Index creation warning: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Runs the SQL script to fix cash advance tables
        /// </summary>
        public static bool RunFixScript()
        {
            try
            {
                string scriptPath = Path.Combine(Application.StartupPath, "fix_cash_advance_tables.sql");
                
                if (File.Exists(scriptPath))
                {
                    string script = File.ReadAllText(scriptPath);
                    string[] commands = script.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (string command in commands)
                    {
                        string trimmedCommand = command.Trim();
                        if (!string.IsNullOrEmpty(trimmedCommand) && !trimmedCommand.StartsWith("--"))
                        {
                            DatabaseManager.ExecuteNonQuery(trimmedCommand);
                        }
                    }
                    
                    MessageBox.Show("Cash advance tables fixed successfully!", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    // Fallback to programmatic fix
                    return ValidateAndFixTables();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running fix script: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}