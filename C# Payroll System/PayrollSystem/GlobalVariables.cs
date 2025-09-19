using System;
using System.Drawing;

namespace PayrollSystem
{
    /// <summary>
    /// Global variables class - equivalent to modVariable.vb
    /// Contains database connection settings and other global variables
    /// </summary>
    public static class GlobalVariables
    {
        // Database connection variables
        public static string DB_UserID { get; set; } = "admin";
        public static string DB_Password { get; set; } = "admin";
        public static string DB_Server { get; set; } = "localhost";
        public static string DB_Name { get; set; } = "payroll_system";

        // Company information variables
        public static string Company { get; set; } = "Sample Company";
        public static string Address { get; set; } = "123 Main Street, City, State";
        public static string Contact { get; set; } = "(123) 456-7890";
        public static string Email { get; set; } = "info@company.com";

        // Payroll period variables
        public static string NumberOfDays { get; set; } = "30";
        public static string CutOff { get; set; } = "Semi-Monthly";
        public static string CutOffFrom { get; set; } = "1";
        public static string CutOffTo { get; set; } = "15";
        public static string PayrollPeriod { get; set; } = "Semi-Monthly";

        // Message variables
        public static string SaveMessage { get; set; } = "Save this record? Click yes to confirm.";
        public static string UpdateMessage { get; set; } = "Update this record? Click yes to confirm.";
        public static string DeleteMessage { get; set; } = "Delete this record? Click yes to confirm.";
        public static string SaveSuccessMessage { get; set; } = "Record has been successfully saved.";
        public static string UpdateSuccessMessage { get; set; } = "Record has been successfully updated.";
        public static string DeleteSuccessMessage { get; set; } = "Record has been successfully deleted.";

        // User variables
        public static string User { get; set; } = "admin";
        public static string Username { get; set; } = "admin";
        public static string Password { get; set; } = "admin";
        public static string Name { get; set; } = "Administrator";
        public static string QR { get; set; } = "ADMIN001";
        public static string CurrentUser { get; set; } = "admin";
        
        // Current logged-in user information
        public static int CurrentUserId { get; set; } = 1;
        public static string CurrentUserName { get; set; } = "Administrator";
        public static string CurrentUserRole { get; set; } = "Admin";

        // Additional company properties for compatibility
        public static string CompanyName { get; set; } = "Sample Company";
        public static string CompanyAddress { get; set; } = "123 Main Street, City, State";
        public static string CompanyPhone { get; set; } = "(123) 456-7890";
        public static string CompanyEmail { get; set; } = "info@company.com";

        // UI Theme variables
        public static bool IsDarkMode { get; set; } = false;
        
        // Dark mode colors - these properties use ThemeManager for backward compatibility
        public static Color DarkBackColor => ThemeManager.DarkBackColor;
        public static Color DarkForeColor => ThemeManager.DarkForeColor;
        public static Color DarkControlBackColor => ThemeManager.DarkControlBackColor;
        public static Color DarkControlForeColor => ThemeManager.DarkControlForeColor;
        public static Color DarkButtonBackColor => ThemeManager.DarkButtonBackColor;
        public static Color DarkButtonForeColor => ThemeManager.DarkButtonForeColor;

        // Application title
        public static string ApplicationTitle { get; set; } = "Payroll Management System";

        /// <summary>
        /// Resets all variables to their default values
        /// </summary>
        public static void ResetVariables()
        {
            DB_UserID = "";
            DB_Password = "";
            DB_Server = "";
            DB_Name = "";
            Company = "";
            Address = "";
            Contact = "";
            Email = "";
            NumberOfDays = "";
            CutOff = "";
            CutOffFrom = "";
            CutOffTo = "";
            PayrollPeriod = "";
            User = "admin";
            Username = "";
            Password = "";
            Name = "";
            QR = "";
        }

        /// <summary>
        /// Gets the full database connection string
        /// </summary>
        /// <returns>MySQL connection string</returns>
        public static string GetConnectionString()
        {
            return $"server={DB_Server};user id={DB_UserID};password={DB_Password};database={DB_Name};charset=utf8;";
        }

        /// <summary>
        /// Validates if all required database connection parameters are set
        /// </summary>
        /// <returns>True if all parameters are set, false otherwise</returns>
        public static bool IsConnectionConfigured()
        {
            return !string.IsNullOrEmpty(DB_Server) &&
                   !string.IsNullOrEmpty(DB_UserID) &&
                   !string.IsNullOrEmpty(DB_Name);
        }

        /// <summary>
        /// Initializes global variables from configuration and database
        /// </summary>
        public static void InitializeFromConfig()
        {
            // Load database configuration from DatabaseManager
            DB_Server = DatabaseManager.DBServer ?? "localhost";
            DB_UserID = DatabaseManager.DBUserID ?? "admin";
            DB_Password = DatabaseManager.DBPassword ?? "admin";
            DB_Name = DatabaseManager.DBName ?? "payroll_system";
            
            // Load company information from DatabaseManager
            CompanyName = DatabaseManager.CompanyName ?? "Sample Company";
            Company = CompanyName;
            CompanyAddress = DatabaseManager.CompanyAddress ?? "123 Main Street, City, State";
            Address = CompanyAddress;
            CompanyPhone = DatabaseManager.CompanyContact ?? "(123) 456-7890";
            Contact = CompanyPhone;
            CompanyEmail = DatabaseManager.CompanyEmail ?? "info@company.com";
            Email = CompanyEmail;
            
            // Load payroll settings
            PayrollPeriod = DatabaseManager.PayrollPeriod ?? "Semi-Monthly";
            CutOff = PayrollPeriod;
            NumberOfDays = DatabaseManager.NumberOfDays ?? "30";
            CutOffFrom = DatabaseManager.CutOffFrom ?? "1";
            CutOffTo = DatabaseManager.CutOffTo ?? "15";
            
            // Set current user information
            CurrentUser = DatabaseManager.CurrentUser ?? "admin";
            User = CurrentUser;
            Username = CurrentUser;
            
            // Generate QR code for current user
            QR = $"{CurrentUser.ToUpper()}001";
        }
    }
}
