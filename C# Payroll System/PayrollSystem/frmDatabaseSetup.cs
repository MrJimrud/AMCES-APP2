using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem.Database
{
    public partial class frmDatabaseSetup : Form
    {
        private string schemaFilePath;
        private bool isDarkMode = false;

        public frmDatabaseSetup()
        {
            InitializeComponent();
            schemaFilePath = Path.Combine(Application.StartupPath, "database_schema.sql");
        }

        private void frmDatabaseSetup_Load(object sender, EventArgs e)
        {
            // Load current connection settings
            txtServer.Text = GlobalVariables.DB_Server;
            txtUsername.Text = GlobalVariables.DB_UserID;
            txtPassword.Text = GlobalVariables.DB_Password;
            txtDatabase.Text = GlobalVariables.DB_Name;

            // Check if schema file exists
            if (!File.Exists(schemaFilePath))
            {
                lblStatus.Text = "Error: Database schema file not found!";
                lblStatus.ForeColor = Color.Red;
                btnInitialize.Enabled = false;
            }
            else
            {
                lblStatus.Text = "Ready to initialize database.";
                lblStatus.ForeColor = Color.Green;
            }

            // Apply theme
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            isDarkMode = GlobalVariables.IsDarkMode;

            if (isDarkMode)
            {
                // Use ThemeManager to apply theme to the entire form
                ThemeManager.ApplyTheme(this, isDarkMode);
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            // Save connection settings to global variables temporarily
            string originalServer = GlobalVariables.DB_Server;
            string originalUserID = GlobalVariables.DB_UserID;
            string originalPassword = GlobalVariables.DB_Password;
            string originalDBName = GlobalVariables.DB_Name;

            // Update with form values
            GlobalVariables.DB_Server = txtServer.Text.Trim();
            GlobalVariables.DB_UserID = txtUsername.Text.Trim();
            GlobalVariables.DB_Password = txtPassword.Text.Trim();
            GlobalVariables.DB_Name = txtDatabase.Text.Trim();

            try
            {
                // Test connection without database
                string connectionString = $"Server={GlobalVariables.DB_Server};User ID={GlobalVariables.DB_UserID};Password={GlobalVariables.DB_Password}";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Connection successful!", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                }

                // Check if database exists
                bool dbExists = DatabaseManager.DatabaseExists();
                if (dbExists)
                {
                    lblDatabaseStatus.Text = "Database exists.";
                    lblDatabaseStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblDatabaseStatus.Text = "Database does not exist. It will be created during initialization.";
                    lblDatabaseStatus.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblDatabaseStatus.Text = "Connection failed.";
                lblDatabaseStatus.ForeColor = Color.Red;
            }

            // Restore original connection settings
            GlobalVariables.DB_Server = originalServer;
            GlobalVariables.DB_UserID = originalUserID;
            GlobalVariables.DB_Password = originalPassword;
            GlobalVariables.DB_Name = originalDBName;
        }

        private void btnInitialize_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtServer.Text) ||
            string.IsNullOrWhiteSpace(txtUsername.Text) ||
            string.IsNullOrWhiteSpace(txtDatabase.Text))
        {
            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Confirm with user
        DialogResult result = MessageBox.Show(
            "This will initialize the database with the default schema. Any existing data may be lost. Continue?",
            "Confirm Database Initialization",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.No)
            return;

        // Save connection settings
        GlobalVariables.DB_Server = txtServer.Text.Trim();
        GlobalVariables.DB_UserID = txtUsername.Text.Trim();
        GlobalVariables.DB_Password = txtPassword.Text.Trim();
        GlobalVariables.DB_Name = txtDatabase.Text.Trim();

        // Update connection string
        DatabaseManager.LoadConfiguration();

        // Show progress form
        using (frmWait waitForm = new frmWait("Initializing Database"))
        {
            waitForm.Show();
            waitForm.SetMessage("Checking database connection...");
            Application.DoEvents();

            try
            {
                // Create database if it doesn't exist
                waitForm.SetMessage("Creating database if it doesn't exist...");
                if (!DatabaseManager.DatabaseExists())
                {
                    if (!DatabaseManager.CreateDatabase())
                    {
                        MessageBox.Show("Failed to create database.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Initialize connection with the database
                DatabaseManager.InitializeConnection();

                // Execute schema script
                waitForm.SetMessage("Executing database schema script...");
                bool success = DatabaseManager.ExecuteSqlScriptFromFile(schemaFilePath, (status) => {
                    waitForm.SetMessage(status);
                    Application.DoEvents();
                });

                if (success)
                {
                    // Save connection settings to config file
                    waitForm.SetMessage("Saving configuration...");
                    SaveConnectionSettings();

                    MessageBox.Show("Database initialized successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to initialize database schema.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        }
        
        /// <summary>
        /// Saves the database connection settings to the config.ini file
        /// </summary>
        private void SaveConnectionSettings()
        {
            try
            {
                string configPath = Path.Combine(Application.StartupPath, "config.ini");
                
                // Create the file if it doesn't exist
                if (!File.Exists(configPath))
                {
                    IniManager.CreateDefaultIni(configPath);
                }
                
                // Write the connection settings to the config file
                IniManager.WriteIni(configPath, "Settings", "DB_Server", GlobalVariables.DB_Server);
                IniManager.WriteIni(configPath, "Settings", "DB_Name", GlobalVariables.DB_Name);
                IniManager.WriteIni(configPath, "Settings", "DB_UserID", GlobalVariables.DB_UserID);
                IniManager.WriteIni(configPath, "Settings", "DB_Password", GlobalVariables.DB_Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving connection settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
                Title = "Select SQL Script File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtScriptPath.Text = openFileDialog.FileName;
            }
        }
        
        private void btnRunScript_Click(object sender, EventArgs e)
        {
            // Validate script path
            if (string.IsNullOrEmpty(txtScriptPath.Text) || !File.Exists(txtScriptPath.Text))
            {
                MessageBox.Show("Please select a valid SQL script file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Show progress form
            using (frmWait waitForm = new frmWait("Executing SQL Script"))
            {
                waitForm.Show();
                waitForm.SetMessage("Executing SQL script...");
                Application.DoEvents();

                try
                {
                    // Execute the SQL script
                    bool success = DatabaseManager.ExecuteSqlScriptFromFile(txtScriptPath.Text, (status) => {
                        waitForm.SetMessage(status);
                        Application.DoEvents();
                    });

                    if (success)
                    {
                        MessageBox.Show("SQL script executed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("There were errors executing the SQL script. Please check the script and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error executing SQL script: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}