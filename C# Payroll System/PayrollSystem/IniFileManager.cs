using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PayrollSystem
{
    public static class IniFileManager
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Write a value to an INI file
        /// </summary>
        /// <param name="filePath">Path to the INI file</param>
        /// <param name="section">Section name</param>
        /// <param name="key">Key name</param>
        /// <param name="value">Value to write</param>
        public static void WriteIni(string filePath, string section, string key, string value)
        {
            try
            {
                WritePrivateProfileString(section, key, value, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing to INI file: {ex.Message}");
            }
        }

        /// <summary>
        /// Read a value from an INI file
        /// </summary>
        /// <param name="filePath">Path to the INI file</param>
        /// <param name="section">Section name</param>
        /// <param name="key">Key name</param>
        /// <param name="defaultValue">Default value if key is not found</param>
        /// <returns>The value from the INI file or default value</returns>
        public static string ReadIni(string filePath, string section, string key, string defaultValue)
        {
            try
            {
                StringBuilder temp = new StringBuilder(1024);
                int length = GetPrivateProfileString(section, key, defaultValue, temp, 1024, filePath);
                return temp.ToString().Substring(0, length);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading from INI file: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if an INI file exists, if not create it with default database settings
        /// </summary>
        /// <param name="filePath">Path to the INI file</param>
        public static void CreateDefaultConfigIfNotExists(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    // Create default configuration
                    WriteIni(filePath, "Settings", "DB_Server", "localhost");
                    WriteIni(filePath, "Settings", "DB_Name", "payroll_system");
                    WriteIni(filePath, "Settings", "DB_UserID", "root");
                    WriteIni(filePath, "Settings", "DB_Password", "");
                    
                    // Application settings
                    WriteIni(filePath, "Application", "ProductKey", "PAYR-OLL2-0220-0000");
                    WriteIni(filePath, "Application", "Registered", "False");
                    WriteIni(filePath, "Application", "Expired", "False");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating default configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all keys from a specific section
        /// </summary>
        /// <param name="filePath">Path to the INI file</param>
        /// <param name="section">Section name</param>
        /// <returns>Array of key names</returns>
        public static string[] GetKeys(string filePath, string section)
        {
            try
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(section, null, "", temp, 1024, filePath);
                string result = temp.ToString();
                return result.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting keys from INI file: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all sections from the INI file
        /// </summary>
        /// <param name="filePath">Path to the INI file</param>
        /// <returns>Array of section names</returns>
        public static string[] GetSections(string filePath)
        {
            try
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(null, null, "", temp, 1024, filePath);
                string result = temp.ToString();
                return result.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting sections from INI file: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a key from the INI file
        /// </summary>
        /// <param name="filePath">Path to the INI file</param>
        /// <param name="section">Section name</param>
        /// <param name="key">Key name to delete</param>
        public static void DeleteKey(string filePath, string section, string key)
        {
            try
            {
                WritePrivateProfileString(section, key, null, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting key from INI file: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete an entire section from the INI file
        /// </summary>
        /// <param name="filePath">Path to the INI file</param>
        /// <param name="section">Section name to delete</param>
        public static void DeleteSection(string filePath, string section)
        {
            try
            {
                WritePrivateProfileString(section, null, null, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting section from INI file: {ex.Message}");
            }
        }
    }
}
