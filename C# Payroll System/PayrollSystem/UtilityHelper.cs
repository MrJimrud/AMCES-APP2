using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;
using System.IO;

namespace PayrollSystem
{
    /// <summary>
    /// Utility helper class - equivalent to Module1.vb
    /// Contains various utility methods for database operations, data formatting, and UI styling
    /// </summary>
    public static class UtilityHelper
    {
        private static MySqlConnection connection = new MySqlConnection();
        private static MySqlCommand command = new MySqlCommand();
        private static MySqlDataReader dataReader;

        /// <summary>
        /// DataGrid view styling modes
        /// </summary>
        public enum DataGridMode
        {
            Small,
            Normal,
            Large
        }

        /// <summary>
        /// Initializes database connection using configuration settings
        /// </summary>
        public static void InitializeConnection()
        {
            try
            {
                string configPath = Path.Combine(Application.StartupPath, "config.ini");
                
                GlobalVariables.DB_Server = IniManager.ReadIni(configPath, "Settings", "DB_Server", "localhost");
                GlobalVariables.DB_Name = IniManager.ReadIni(configPath, "Settings", "DB_Name", "payroll_db");
                GlobalVariables.DB_UserID = IniManager.ReadIni(configPath, "Settings", "DB_UserID", "root");
                GlobalVariables.DB_Password = IniManager.ReadIni(configPath, "Settings", "DB_Password", "");

                connection.ConnectionString = GlobalVariables.GetConnectionString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing connection: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets dashboard statistics and updates the dashboard form
        /// </summary>
        public static void GetDashboard()
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                
                // Get employee count
                connection.Open();
                command = new MySqlCommand("SELECT COUNT(*) FROM tbl_employee", connection);
                long employeeCount = Convert.ToInt64(command.ExecuteScalar());
                connection.Close();

                // Get active payroll period
                connection.Open();
                command = new MySqlCommand("SELECT * FROM tbl_payrollperiod WHERE status LIKE 'ACTIVE'", connection);
                dataReader = command.ExecuteReader();
                string payrollDate = "No Data";
                if (dataReader.Read())
                {
                    payrollDate = dataReader["payrolldate"].ToString();
                }
                dataReader.Close();
                connection.Close();

                // Get cash advance total
                connection.Open();
                command = new MySqlCommand($"SELECT IFNULL(SUM(amount),0) FROM cash_advances WHERE DATE_FORMAT(request_date, '%Y-%m') = '{payrollDate}'", connection);
                double cashAdvanceTotal = Convert.ToDouble(command.ExecuteScalar());
                connection.Close();

                // Get today's log count
                connection.Open();
                command = new MySqlCommand($"SELECT COUNT(*) FROM tbl_log WHERE sdate BETWEEN '{currentDate}' AND '{currentDate}'", connection);
                long logCount = Convert.ToInt64(command.ExecuteScalar());
                connection.Close();

                // Update dashboard (assuming frmDashboard exists)
                // This would need to be implemented based on the actual dashboard form
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                MessageBox.Show(ex.Message, GlobalVariables.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads QR code length setting from database
        /// </summary>
        public static void GetQrLength()
        {
            try
            {
                connection.Open();
                command = new MySqlCommand("SELECT * FROM tbl_qr", connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    GlobalVariables.QR = dataReader["qr"].ToString();
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                MessageBox.Show(ex.Message, GlobalVariables.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads company information from database
        /// </summary>
        public static void LoadCompany()
        {
            try
            {
                connection.Open();
                command = new MySqlCommand("SELECT * FROM tbl_company", connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    GlobalVariables.Company = dataReader["company"].ToString();
                    GlobalVariables.Address = dataReader["address"].ToString();
                    GlobalVariables.Contact = dataReader["contact"].ToString();
                    GlobalVariables.Email = dataReader["email"].ToString();
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                MessageBox.Show(ex.Message, GlobalVariables.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads payroll settings from database
        /// </summary>
        public static void LoadPayrollSettings()
        {
            try
            {
                connection.Open();
                command = new MySqlCommand("SELECT * FROM tbl_payrollperiod WHERE status LIKE 'ACTIVE'", connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    GlobalVariables.PayrollPeriod = dataReader["payrolldate"].ToString();
                    DateTime cutOffFrom = Convert.ToDateTime(dataReader["cut_off_from"].ToString());
                    DateTime cutOffTo = Convert.ToDateTime(dataReader["cut_off_to"].ToString());
                    GlobalVariables.CutOff = $"{cutOffFrom:MMM-dd-yyyy}-{cutOffTo:MMM-dd-yyyy}";
                    GlobalVariables.NumberOfDays = dataReader["no_of_days"].ToString();
                    GlobalVariables.CutOffFrom = cutOffFrom.ToString("MMM-dd-yyyy");
                    GlobalVariables.CutOffTo = cutOffTo.ToString("MMM-dd-yyyy");
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                MessageBox.Show(ex.Message, GlobalVariables.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Generates a new ID number for a table
        /// </summary>
        /// <param name="tableName">Source table name</param>
        /// <param name="fieldName">Source field name</param>
        /// <returns>New ID number</returns>
        public static double GenerateNewIDNumber(string tableName, string fieldName)
        {
            try
            {
                DataTable dataTable = GetDataSet($"SELECT MAX({fieldName}) FROM {tableName}");
                if (dataTable.Rows.Count > 0)
                {
                    string maxValue = dataTable.Rows[0][0].ToString();
                    return ToNumber(maxValue) + 1;
                }
                return 1;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Executes a scalar query and returns the result
        /// </summary>
        /// <param name="commandText">SQL command</param>
        /// <returns>Scalar result as string</returns>
        public static string GetScalar(string commandText)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                
                connection.Open();
                command = new MySqlCommand(commandText, connection);
                string result = command.ExecuteScalar()?.ToString() ?? "";
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw new Exception($"Error executing scalar query: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Executes a query and returns a DataTable
        /// </summary>
        /// <param name="commandText">SQL command</param>
        /// <returns>DataTable with results</returns>
        public static DataTable GetDataSet(string commandText)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(commandText, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw new Exception($"Error executing query: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// Executes a parameterized query and returns a DataTable
        /// </summary>
        /// <param name="commandText">SQL command with parameters</param>
        /// <param name="parameters">MySqlParameter array</param>
        /// <returns>DataTable with results</returns>
        public static DataTable GetDataSet(string commandText, MySqlParameter[] parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                
                connection.Open();
                MySqlCommand command = new MySqlCommand(commandText, connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw new Exception($"Error executing parameterized query: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// Executes a parameterized query and returns a DataTable
        /// </summary>
        /// <param name="commandText">SQL command with parameters</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        /// <returns>DataTable with results</returns>
        public static DataTable GetDataSet(string commandText, Dictionary<string, object> parameters)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                
                connection.Open();
                MySqlCommand command = new MySqlCommand(commandText, connection);
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw new Exception($"Error executing parameterized query: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Converts a string to a number, handling currency formatting
        /// </summary>
        /// <param name="value">String value to convert</param>
        /// <param name="returnZeroIfNegative">Return zero if result is negative</param>
        /// <returns>Numeric value</returns>
        public static double ToNumber(string value, bool returnZeroIfNegative = false)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            try
            {
                // Remove commas and convert
                string cleanValue = value.Replace(",", "");
                double result = Convert.ToDouble(cleanValue);
                
                if (returnZeroIfNegative && result < 0)
                    result = 0;
                    
                return result;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Applies dark mode styling to a DataGridView
        /// </summary>
        /// <param name="dataGrid">DataGridView to style</param>
        /// <param name="mode">Display mode</param>
        public static void ApplyDarkMode(DataGridView dataGrid, DataGridMode mode = DataGridMode.Small)
        {
            float fontSize = mode switch
            {
                DataGridMode.Small => 8f,
                DataGridMode.Normal => 9f,
                DataGridMode.Large => 10f,
                _ => 8f
            };

            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToResizeColumns = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.BackgroundColor = Color.FromArgb(45, 45, 48);
            dataGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            
            dataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(76, 76, 76);
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
            dataGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            dataGrid.ColumnHeadersHeight = 30;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.RowHeadersVisible = false;
            
            dataGrid.RowsDefaultCellStyle.BackColor = Color.White;
            dataGrid.RowsDefaultCellStyle.Font = new Font("Segoe UI", fontSize, FontStyle.Regular);
            dataGrid.RowsDefaultCellStyle.ForeColor = Color.FromArgb(45, 45, 48);
            dataGrid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(185, 223, 229);
            dataGrid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.RowTemplate.Height = 28;
            dataGrid.GridColor = Color.FromArgb(240, 240, 240);
        }

        /// <summary>
        /// Applies light mode styling to a DataGridView
        /// </summary>
        /// <param name="dataGrid">DataGridView to style</param>
        /// <param name="mode">Display mode</param>
        public static void ApplyLightMode(DataGridView dataGrid, DataGridMode mode = DataGridMode.Small)
        {
            float fontSize = mode switch
            {
                DataGridMode.Small => 8f,
                DataGridMode.Normal => 9f,
                DataGridMode.Large => 10f,
                _ => 8f
            };

            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToResizeColumns = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.BackgroundColor = Color.White;
            dataGrid.CellBorderStyle = DataGridViewCellBorderStyle.SunkenHorizontal;
            dataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            
            dataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular);
            dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
            dataGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            dataGrid.ColumnHeadersHeight = 35;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.RowHeadersVisible = false;
            
            dataGrid.RowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGrid.RowsDefaultCellStyle.Font = new Font("Segoe UI", fontSize, FontStyle.Regular);
            dataGrid.RowsDefaultCellStyle.ForeColor = Color.FromArgb(45, 45, 48);
            dataGrid.RowsDefaultCellStyle.SelectionBackColor = Color.Black;
            dataGrid.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(241, 196, 15);
            
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.RowTemplate.Height = 22;
            dataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 247, 247);
        }

        /// <summary>
        /// Alias for GetDataSet method - executes a query and returns a DataTable
        /// </summary>
        /// <param name="commandText">SQL command</param>
        /// <returns>DataTable with results</returns>
        public static DataTable GetDataTable(string commandText)
        {
            return GetDataSet(commandText);
        }
        
        /// <summary>
        /// Executes a parameterized query and returns a DataTable
        /// </summary>
        /// <param name="commandText">SQL command with parameters</param>
        /// <param name="parameters">MySqlParameter array</param>
        /// <returns>DataTable with results</returns>
        public static DataTable GetDataTable(string commandText, MySqlParameter[] parameters)
        {
            return GetDataSet(commandText, parameters);
        }
        
        /// <summary>
        /// Executes a parameterized query and returns a DataTable
        /// </summary>
        /// <param name="commandText">SQL command with parameters</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        /// <returns>DataTable with results</returns>
        public static DataTable GetDataTable(string commandText, Dictionary<string, object> parameters)
        {
            return GetDataSet(commandText, parameters);
        }

        /// <summary>
        /// Alias for GetScalar method - executes a scalar query and returns the result
        /// </summary>
        /// <param name="commandText">SQL command</param>
        /// <returns>Scalar result as string</returns>
        public static string GetScalarValue(string commandText)
        {
            return GetScalar(commandText);
        }
        
        /// <summary>
        /// Executes a parameterized scalar query and returns the result
        /// </summary>
        /// <param name="commandText">SQL command with parameter placeholder</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>Scalar result as string</returns>
        public static string GetScalarValue(string commandText, string parameterValue)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                
                connection.Open();
                command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@param", parameterValue);
                string result = command.ExecuteScalar()?.ToString() ?? "";
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw new Exception($"Error executing parameterized scalar query: {ex.Message}", ex);
            }
        }
    }
}
