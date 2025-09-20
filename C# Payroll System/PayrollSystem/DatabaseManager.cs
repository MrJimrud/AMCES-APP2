using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySqlConnector;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace PayrollSystem
{
    public static class DatabaseManager
    {
        private static MySqlConnection connection { get; set; }
        private static string connectionString;

        // Public property to access the connection
        public static MySqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new MySqlConnection();
                }
                return connection;
            }
        }
        
        // Database configuration properties
        public static string DBServer { get; set; }
        public static string DBName { get; set; }
        public static string DBUserID { get; set; }
        public static string DBPassword { get; set; }
        
        // Global variables similar to VB.NET Module1
        public static MySqlCommand Command { get; set; }
        public static MySqlDataReader DataReader { get; set; }
        
        // Application variables
        public static string CurrentUser { get; set; }
        public static string CompanyName { get; set; }
        public static string CompanyAddress { get; set; }
        public static string CompanyContact { get; set; }
        public static string CompanyEmail { get; set; }
        public static string PayrollPeriod { get; set; }
        public static string CutOffPeriod { get; set; }
        public static string NumberOfDays { get; set; }
        public static string CutOffFrom { get; set; }
        public static string CutOffTo { get; set; }
        public static string QRCode { get; set; }
        
        // Message constants
        public static string DeleteMessage = "Are you sure you want to delete this record?";
        public static string ApplicationTitle = "Payroll Management System";
        
        static DatabaseManager()
        {
            connection = new MySqlConnection();
        }
        
        public static void LoadConfiguration()
        {
            try
            {
                string configPath = Path.Combine(Application.StartupPath, "config.ini");
                if (File.Exists(configPath))
                {
                    DBServer = IniFileManager.ReadIni(configPath, "Settings", "DB_Server", "localhost");
                    DBName = IniFileManager.ReadIni(configPath, "Settings", "DB_Name", "payroll_system");
                    DBUserID = IniFileManager.ReadIni(configPath, "Settings", "DB_UserID", "root");
                    DBPassword = IniFileManager.ReadIni(configPath, "Settings", "DB_Password", "");
                }
                else
                {
                    // Default values - use localhost
                    DBServer = "localhost";
                    DBName = "payroll_system";
                    DBUserID = "root";
                    DBPassword = "";
                }
                
                // Build MariaDB-compatible connection string with explicit password parameter
                connectionString = $"server={DBServer}; port=3306; database={DBName}; uid={DBUserID}; pwd={DBPassword ?? ""}; sslmode=none;";
                // Log connection string for debugging (without password)
                var debugConnectionString = $"server={DBServer};port=3306;database={DBName};uid={DBUserID};pwd=***;sslmode=none;";
                System.Diagnostics.Debug.WriteLine($"Connection string: {debugConnectionString}");
                connection.ConnectionString = connectionString;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading configuration: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}
        

        public static bool OpenConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    //connection.ConnectionString = "server=localhost;port=3306;database=payroll_system;uid=root;pwd=;sslmode=none;";
                    connection.Open();

                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        public static void CloseConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing connection: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static MySqlConnection GetConnection()
        {
            return connection;
        }
        

        
        public static string GetScalar(string query)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                var result = Command.ExecuteScalar();
                connection.Close();
                return result?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing scalar query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }
        
        public static string GetScalar(string query, Dictionary<string, object> parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        Command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }
                
                var result = Command.ExecuteScalar();
                connection.Close();
                return result?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing scalar query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }
        
        public static DataTable GetDataTable(string query)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
        
        public static DataTable GetDataTable(string query, Dictionary<string, object> parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        Command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing parameterized query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
        
        // ExecuteNonQuery method moved to compatibility section below
        
        public static double GenerateNewIDNumber(string tableName, string fieldName)
        {
            try
            {
                DataTable dt = GetDataTable($"SELECT MAX({fieldName}) FROM {tableName}");
                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    return Convert.ToDouble(dt.Rows[0][0]) + 1;
                }
                return 1;
            }
            catch
            {
                return 1;
            }
        }
        
        public static double ToNumber(string value, bool returnZeroIfNegative = false)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
                
            try
            {
                string cleanValue = value.Replace(",", "");
                double result = Convert.ToDouble(cleanValue);
                
                if (returnZeroIfNegative && result < 0)
                    return 0;
                    
                return result;
            }
            catch
            {
                return 0;
            }
        }
        
        public static void LoadCompanySettings()
        {
            try
            {
                OpenConnection();
                Command = new MySqlCommand("SELECT * FROM company_settings", connection);
                DataReader = Command.ExecuteReader();
                
                if (DataReader.Read())
                {
                    CompanyName = DataReader["company_name"].ToString();
                    CompanyAddress = DataReader["address"].ToString();
                    CompanyContact = DataReader["contact"].ToString();
                    CompanyEmail = DataReader["email"].ToString();
                }
                
                DataReader.Close();
                CloseConnection();
            }
            catch (Exception ex)
            {
                CloseConnection();
                MessageBox.Show($"Error loading company settings: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public static void LoadPayrollSettings()
        {
            try
            {
                OpenConnection();
                Command = new MySqlCommand("SELECT * FROM payroll_periods WHERE status = 'Active'", connection);
                DataReader = Command.ExecuteReader();
                
                if (DataReader.Read())
                {
                    PayrollPeriod = DataReader["period_name"].ToString();
                    CutOffPeriod = $"{Convert.ToDateTime(DataReader["start_date"]):MMM-dd-yyyy}-{Convert.ToDateTime(DataReader["end_date"]):MMM-dd-yyyy}";
                    NumberOfDays = DataReader["days_count"].ToString();
                    CutOffFrom = Convert.ToDateTime(DataReader["start_date"]).ToString("MMM-dd-yyyy");
                    CutOffTo = Convert.ToDateTime(DataReader["end_date"]).ToString("MMM-dd-yyyy");
                }
                
                DataReader.Close();
                CloseConnection();
            }
            catch (Exception ex)
            {
                CloseConnection();
                MessageBox.Show($"Error loading payroll settings: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public static void GetDashboardData()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                
                // Get employee count
                string employeeCount = GetScalar("SELECT COUNT(*) FROM employees");
                
                // Get active payroll period
                LoadPayrollSettings();
                
                // Get cash advance total
                string caTotal = GetScalar($"SELECT IFNULL(SUM(amount),0) FROM cash_advances WHERE period_id = '{PayrollPeriod}'");
                
                // Get today's log count
                string logCount = GetScalar($"SELECT COUNT(*) FROM activity_logs WHERE log_date BETWEEN '{today}' AND '{today}'");
                
                // Update dashboard form if it exists
                // This will be implemented when dashboard form is created
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting dashboard data: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public static void GetQRLength()
        {
            try
            {
                OpenConnection();
                Command = new MySqlCommand("SELECT * FROM qr_settings", connection);
                DataReader = Command.ExecuteReader();
                
                if (DataReader.Read())
                {
                    QRCode = DataReader["qr"].ToString();
                }
                
                DataReader.Close();
                CloseConnection();
            }
            catch (Exception ex)
            {
                CloseConnection();
                MessageBox.Show($"Error loading QR settings: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Additional methods for compatibility
        public static MySqlParameter CreateParameter(string parameterName, object value)
        {
            return new MySqlParameter(parameterName, value ?? DBNull.Value);
        }
        
        public static int ExecuteNonQuery(string query)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                int result = Command.ExecuteNonQuery();
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing non-query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        
        public static int ExecuteNonQuery(string query, MySqlParameter[] parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                
                if (parameters != null)
                {
                    Command.Parameters.AddRange(parameters);
                }
                
                int result = Command.ExecuteNonQuery();
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing parameterized non-query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        
        public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        Command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }
                
                int result = Command.ExecuteNonQuery();
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing parameterized non-query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        
        public static void InitializeConnection()
        {
            try
            {
                connectionString = GlobalVariables.GetConnectionString();
                connection = new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing connection: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public static DataTable ExecuteQuery(string query)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
        
        public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                
                if (parameters != null)
                {
                    Command.Parameters.AddRange(parameters);
                }
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing parameterized query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
        
        public static object ExecuteScalar(string query)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                var result = Command.ExecuteScalar();
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing scalar query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        
        public static string GenerateNewId(string tableName, string fieldName, string prefix = "")
        {
            try
            {
                string query = $"SELECT MAX(CAST(SUBSTRING({fieldName}, {prefix.Length + 1}) AS UNSIGNED)) FROM {tableName}";
                object result = ExecuteScalar(query);
                
                int nextNumber = 1;
                if (result != null && result != DBNull.Value)
                {
                    nextNumber = Convert.ToInt32(result) + 1;
                }
                
                return prefix + nextNumber.ToString("D4"); // Format as 4-digit number with leading zeros
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating new ID: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return prefix + "0001";
            }
        }
        
        public static object ExecuteScalar(string query, MySqlParameter[] parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                
                if (parameters != null)
                {
                    Command.Parameters.AddRange(parameters);
                }
                
                var result = Command.ExecuteScalar();
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing scalar query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        
        public static DataTable GetDataTable(string query, MySqlParameter[] parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                Command = new MySqlCommand(query, connection);
                
                if (parameters != null)
                {
                    Command.Parameters.AddRange(parameters);
                }
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(Command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing parameterized query: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
        
        public static DataTable GetPayrollPeriods()
        {
            return GetDataTable("SELECT * FROM payroll_periods ORDER BY start_date DESC");
        }
        
        public static DataTable GetDepartments()
        {
            return GetDataTable("SELECT * FROM departments ORDER BY department_name");
        }
        
        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("C2");
        }
        
        public static decimal ConvertCurrencyToNumber(string currencyString)
        {
            if (string.IsNullOrEmpty(currencyString))
                return 0;
                
            try
            {
                // Remove currency symbols and formatting
                string cleanValue = currencyString.Replace("$", "").Replace(",", "").Trim();
                return Convert.ToDecimal(cleanValue);
            }
            catch
            {
                return 0;
            }
        }
        
        // Method to get connection information for backup/restore operations
        public static (string Server, string Database, string UserId, string Password) GetConnectionInfo()
        {
            return (DBServer, DBName, DBUserID, DBPassword);
        }
        
        // Method to execute SQL script from file
        public static bool ExecuteSqlScriptFromFile(string filePath, Action<string> progressCallback = null)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"SQL script file not found: {filePath}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                string script = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(script))
                {
                    MessageBox.Show("SQL script file is empty", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                // Split the script on GO statements (if any)
                string[] batches = Regex.Split(script, @"\r\nGO\r\n|\nGO\n|\rGO\r|;\s*\r\n|;\s*\n", RegexOptions.IgnoreCase);
                
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                
                int successCount = 0;
                int totalBatches = batches.Length;
                
                foreach (string batch in batches)
                {
                    if (string.IsNullOrWhiteSpace(batch))
                        continue;
                        
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(batch, connection))
                        {
                            cmd.ExecuteNonQuery();
                            successCount++;
                            
                            // Report progress if callback provided
                            progressCallback?.Invoke($"Executed {successCount} of {totalBatches} commands");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error executing SQL batch: {ex.Message}\n\nBatch:\n{batch.Substring(0, Math.Min(batch.Length, 100))}...", 
                            ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                connection.Close();
                
                if (successCount > 0)
                {
                    progressCallback?.Invoke($"Successfully executed {successCount} of {totalBatches} commands");
                    return true;
                }
                else
                {
                    progressCallback?.Invoke("Failed to execute any SQL commands");
                    return false;
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing SQL script: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        // Method to check if database exists
        public static bool DatabaseExists()
        {
            try
            {
                // Create a connection without specifying the database
                string connectionWithoutDb = $"Server={DBServer};User ID={DBUserID};Password={DBPassword}";
                using (MySqlConnection conn = new MySqlConnection(connectionWithoutDb))
                {
                    conn.Open();
                    string query = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{DBName}'";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking if database exists: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        // Method to create database
        public static bool CreateDatabase()
        {
            try
            {
                // Create a connection without specifying the database
                string connectionWithoutDb = $"Server={DBServer};User ID={DBUserID};Password={DBPassword}";
                using (MySqlConnection conn = new MySqlConnection(connectionWithoutDb))
                {
                    conn.Open();
                    string query = $"CREATE DATABASE IF NOT EXISTS `{DBName}`";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating database: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        // Method to execute SQL script directly from string content
        public static bool ExecuteSqlScript(string scriptContent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(scriptContent))
                {
                    MessageBox.Show("SQL script is empty", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                // Split the script on GO statements (if any) or semicolons
                string[] batches = Regex.Split(scriptContent, @"\r\nGO\r\n|\nGO\n|\rGO\r|;\s*\r\n|;\s*\n", RegexOptions.IgnoreCase);
                
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    
                connection.Open();
                
                int successCount = 0;
                int totalBatches = batches.Length;
                
                foreach (string batch in batches)
                {
                    if (string.IsNullOrWhiteSpace(batch))
                        continue;
                        
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(batch, connection))
                        {
                            cmd.ExecuteNonQuery();
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error executing SQL batch: {ex.Message}\n\nBatch:\n{batch.Substring(0, Math.Min(batch.Length, 100))}...", 
                            ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                connection.Close();
                
                if (successCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show($"Error executing SQL script: {ex.Message}", ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}