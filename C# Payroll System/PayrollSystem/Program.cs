using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace PayrollSystem
{

static class Program
{
    private static string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup_errors.log");
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        try
        {
            LogMessage("Application starting...");
            
            LogMessage("Configuring application settings...");
            
            // Set visual styles for better appearance
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LogMessage("Visual styles configured");
            
            // Add global exception handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LogMessage("Exception handlers configured");
            
            // Configuration: Set to false to re-enable login
            bool bypassLogin = false; // Re-enabled login for debugging
            
            if (bypassLogin)
            {
                // Skip login and start with main form directly
                LogMessage("Bypassing login, creating main form...");
                
                // Initialize database for main form
                DatabaseManager.LoadConfiguration();
                DatabaseManager.InitializeConnection();
                DatabaseManager.CurrentUser = "admin"; // Set default user
                
                // Initialize global variables from configuration
            GlobalVariables.InitializeFromConfig();
            LogMessage("Global variables initialized from configuration");
            
            // Validate and fix cash advance tables
            try
            {
                LogMessage("Validating cash advance tables...");
                if (CashAdvanceTableManager.ValidateAndFixTables())
                {
                    LogMessage("Cash advance tables validated successfully");
                }
                else
                {
                    LogMessage("Warning: Cash advance table validation failed");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error validating cash advance tables: {ex.Message}");
            }
                 
                 var mainForm = new frmMain();
                LogMessage("Main form created, starting application...");
                Application.Run(mainForm);
            }
            else
            {
                // Start with login form (original behavior)
                LogMessage("Creating login form...");
                
                // Initialize database configuration
                DatabaseManager.LoadConfiguration();
                
                // Initialize global variables from configuration
                GlobalVariables.InitializeFromConfig();
                LogMessage("Global variables initialized from configuration");
                
                var loginForm = new frmLogin();
                LogMessage("Login form created, starting application...");
                Application.Run(loginForm);
            }
            LogMessage("Application exited normally");
        }
        catch (Exception ex)
        {
            string errorMsg = $"Application startup error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}";
            LogMessage($"CRITICAL ERROR: {errorMsg}");
            MessageBox.Show(errorMsg, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private static void LogMessage(string message)
    {
        try
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(logFile, logEntry + Environment.NewLine);
        }
        catch
        {
            // Ignore logging errors
        }
    }
    
    private static void ExecuteAlterTableCommand()
    {
        try
        {
            LogMessage("Starting database update to add employee_code column...");
            
            // Load database configuration
            DatabaseManager.LoadConfiguration();
            LogMessage("Database configuration loaded");
            
            // Initialize connection
            string connectionString = $"server={DatabaseManager.DBServer};user id={DatabaseManager.DBUserID};password={DatabaseManager.DBPassword};database={DatabaseManager.DBName};charset=utf8;";
            LogMessage($"Connection string: {connectionString.Replace(DatabaseManager.DBPassword, "*****")}");
            
            using (MySqlConnector.MySqlConnection connection = new MySqlConnector.MySqlConnection(connectionString))
            {
                LogMessage("Opening database connection...");
                connection.Open();
                
                string alterTableSql = "ALTER TABLE employees ADD COLUMN employee_code VARCHAR(50) NOT NULL AFTER employee_id";
                LogMessage($"Executing SQL command: {alterTableSql}");
                
                using (MySqlConnector.MySqlCommand command = new MySqlConnector.MySqlCommand(alterTableSql, connection))
                {
                    command.ExecuteNonQuery();
                }
                
                LogMessage("Database updated successfully!");
                MessageBox.Show("Employee table updated successfully. Added 'employee_code' column.", 
                    "Database Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (MySqlConnector.MySqlException ex)
        {
            LogMessage($"MySQL Error: {ex.Message}");
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
            LogMessage($"Error: {ex.Message}");
            MessageBox.Show($"Error: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
    {
        string errorMsg = $"Thread Exception: {e.Exception.Message}\n\nStack Trace:\n{e.Exception.StackTrace}";
        LogMessage($"THREAD ERROR: {errorMsg}");
        MessageBox.Show(errorMsg, "Thread Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Exception ex = e.ExceptionObject as Exception;
        string errorMsg = $"Unhandled Exception: {ex?.Message}\n\nStack Trace:\n{ex?.StackTrace}";
        LogMessage($"UNHANDLED ERROR: {errorMsg}");
        MessageBox.Show(errorMsg, "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }    
}
}
