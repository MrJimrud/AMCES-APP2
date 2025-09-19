using System;
using System.Drawing;

namespace PayrollSystem
{
    /// <summary>
    /// Manages theme colors and appearance settings for the application
    /// </summary>
    public static class ThemeManager
    {
        // Dark mode colors
        public static Color DarkBackColor { get; } = Color.FromArgb(45, 45, 48);
        public static Color DarkForeColor { get; } = Color.White;
        public static Color DarkControlBackColor { get; } = Color.FromArgb(60, 60, 60);
        public static Color DarkControlForeColor { get; } = Color.White;
        public static Color DarkButtonBackColor { get; } = Color.FromArgb(52, 73, 94);
        public static Color DarkButtonForeColor { get; } = Color.White;
        
        // Light mode colors
        public static Color LightBackColor { get; } = Color.FromArgb(240, 240, 240);
        public static Color LightForeColor { get; } = Color.Black;
        public static Color LightControlBackColor { get; } = Color.White;
        public static Color LightControlForeColor { get; } = Color.Black;
        public static Color LightButtonBackColor { get; } = Color.FromArgb(52, 152, 219);
        public static Color LightButtonForeColor { get; } = Color.White;
        
        /// <summary>
        /// Gets the appropriate background color based on the current theme mode
        /// </summary>
        /// <param name="isDarkMode">Whether dark mode is enabled</param>
        /// <returns>The appropriate background color</returns>
        public static Color GetBackColor(bool isDarkMode)
        {
            return isDarkMode ? DarkBackColor : LightBackColor;
        }
        
        /// <summary>
        /// Gets the appropriate foreground color based on the current theme mode
        /// </summary>
        /// <param name="isDarkMode">Whether dark mode is enabled</param>
        /// <returns>The appropriate foreground color</returns>
        public static Color GetForeColor(bool isDarkMode)
        {
            return isDarkMode ? DarkForeColor : LightForeColor;
        }
        
        /// <summary>
        /// Gets the appropriate control background color based on the current theme mode
        /// </summary>
        /// <param name="isDarkMode">Whether dark mode is enabled</param>
        /// <returns>The appropriate control background color</returns>
        public static Color GetControlBackColor(bool isDarkMode)
        {
            return isDarkMode ? DarkControlBackColor : LightControlBackColor;
        }
        
        /// <summary>
        /// Gets the appropriate control foreground color based on the current theme mode
        /// </summary>
        /// <param name="isDarkMode">Whether dark mode is enabled</param>
        /// <returns>The appropriate control foreground color</returns>
        public static Color GetControlForeColor(bool isDarkMode)
        {
            return isDarkMode ? DarkControlForeColor : LightControlForeColor;
        }
        
        /// <summary>
        /// Gets the appropriate button background color based on the current theme mode
        /// </summary>
        /// <param name="isDarkMode">Whether dark mode is enabled</param>
        /// <returns>The appropriate button background color</returns>
        public static Color GetButtonBackColor(bool isDarkMode)
        {
            return isDarkMode ? DarkButtonBackColor : LightButtonBackColor;
        }
        
        /// <summary>
        /// Gets the appropriate button foreground color based on the current theme mode
        /// </summary>
        /// <param name="isDarkMode">Whether dark mode is enabled</param>
        /// <returns>The appropriate button foreground color</returns>
        public static Color GetButtonForeColor(bool isDarkMode)
        {
            return isDarkMode ? DarkButtonForeColor : LightButtonForeColor;
        }
        
        /// <summary>
        /// Applies the appropriate theme to a form based on the current theme mode
        /// </summary>
        /// <param name="form">The form to apply the theme to</param>
        /// <param name="isDarkMode">Whether dark mode is enabled</param>
        public static void ApplyTheme(System.Windows.Forms.Form form, bool isDarkMode)
        {
            form.BackColor = GetBackColor(isDarkMode);
            form.ForeColor = GetForeColor(isDarkMode);
            
            // Apply to all controls
            foreach (System.Windows.Forms.Control control in form.Controls)
            {
                ApplyThemeToControl(control, isDarkMode);
            }
        }
        
        /// <summary>
        /// Applies the appropriate theme to a control based on the current theme mode
        /// </summary>
        /// <param name="control">The control to apply the theme to</param>
        /// <param name="isDarkMode">Whether dark mode is enabled</param>
        public static void ApplyThemeToControl(System.Windows.Forms.Control control, bool isDarkMode)
        {
            if (control is System.Windows.Forms.Label)
            {
                control.BackColor = GetBackColor(isDarkMode);
                control.ForeColor = GetForeColor(isDarkMode);
            }
            else if (control is System.Windows.Forms.TextBox || 
                     control is System.Windows.Forms.ComboBox || 
                     control is System.Windows.Forms.DateTimePicker)
            {
                control.BackColor = GetControlBackColor(isDarkMode);
                control.ForeColor = GetControlForeColor(isDarkMode);
            }
            else if (control is System.Windows.Forms.Button)
            {
                control.BackColor = GetButtonBackColor(isDarkMode);
                control.ForeColor = GetButtonForeColor(isDarkMode);
            }
            else if (control is System.Windows.Forms.GroupBox)
            {
                control.BackColor = GetBackColor(isDarkMode);
                control.ForeColor = GetForeColor(isDarkMode);
                
                // Apply to controls inside the GroupBox
                foreach (System.Windows.Forms.Control groupControl in ((System.Windows.Forms.GroupBox)control).Controls)
                {
                    ApplyThemeToControl(groupControl, isDarkMode);
                }
            }
            else if (control is System.Windows.Forms.Panel)
            {
                control.BackColor = GetBackColor(isDarkMode);
                
                // Apply to controls inside the Panel
                foreach (System.Windows.Forms.Control panelControl in control.Controls)
                {
                    ApplyThemeToControl(panelControl, isDarkMode);
                }
            }
        }
    }
}