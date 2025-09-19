using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PayrollSystem
{
    /// <summary>
    /// INI file management class - equivalent to modIni.vb
    /// Provides functionality to read and write INI configuration files
    /// </summary>
    public static class IniManager
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int WritePrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpString,
            string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetPrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName);

        /// <summary>
        /// Writes a value to an INI file
        /// </summary>
        /// <param name="iniFileName">Path to the INI file</param>
        /// <param name="section">Section name</param>
        /// <param name="paramName">Parameter name</param>
        /// <param name="paramValue">Parameter value</param>
        public static void WriteIni(string iniFileName, string section, string paramName, string paramValue)
        {
            try
            {
                WritePrivateProfileString(section, paramName, paramValue, iniFileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing to INI file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Reads a value from an INI file
        /// </summary>
        /// <param name="iniFileName">Path to the INI file</param>
        /// <param name="section">Section name</param>
        /// <param name="paramName">Parameter name</param>
        /// <param name="defaultValue">Default value if parameter not found</param>
        /// <returns>The parameter value or default value</returns>
        public static string ReadIni(string iniFileName, string section, string paramName, string defaultValue = "")
        {
            try
            {
                StringBuilder returnValue = new StringBuilder(1024);
                int length = GetPrivateProfileString(section, paramName, defaultValue, returnValue, returnValue.Capacity, iniFileName);
                return returnValue.ToString().Substring(0, length);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading from INI file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if an INI file exists
        /// </summary>
        /// <param name="iniFileName">Path to the INI file</param>
        /// <returns>True if file exists, false otherwise</returns>
        public static bool IniFileExists(string iniFileName)
        {
            return System.IO.File.Exists(iniFileName);
        }

        /// <summary>
        /// Creates a default INI file with basic structure
        /// </summary>
        /// <param name="iniFileName">Path to the INI file</param>
        public static void CreateDefaultIni(string iniFileName)
        {
            try
            {
                WriteIni(iniFileName, "Settings", "DB_Server", "localhost");
                WriteIni(iniFileName, "Settings", "DB_Name", "payroll_system");
                WriteIni(iniFileName, "Settings", "DB_UserID", "root");
                WriteIni(iniFileName, "Settings", "DB_Password", "");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating default INI file: {ex.Message}", ex);
            }
        }
    }
}
