using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using MySqlConnector;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PayrollSystem
{
    public partial class frmChangePass : Form
    {
        // DatabaseManager is static - no instance needed
        private string currentUsername;
        
        // Form Controls
        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelMain;
        private Label lblCurrentPassword;
        private TextBox txtCurrentPassword;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private CheckBox chkShowPassword;
        private Button btnChangePassword;
        private Button btnCancel;
        private Label lblPasswordStrength;
        private ProgressBar pbPasswordStrength;
        private Label lblRequirements;
        
        public frmChangePass(string username)
        {
            currentUsername = username;
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Change Password";
            this.Size = new Size(450, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // Header Panel
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(41, 128, 185)
            };
            
            lblTitle = new Label
            {
                Text = "Change Password",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            
            panelHeader.Controls.Add(lblTitle);
            
            // Main Panel
            panelMain = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(390, 360),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // Current Password
            lblCurrentPassword = new Label
            {
                Text = "Current Password:",
                Location = new Point(20, 30),
                Size = new Size(120, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            txtCurrentPassword = new TextBox
            {
                Location = new Point(150, 30),
                Size = new Size(200, 23),
                UseSystemPasswordChar = true
            };
            
            // New Password
            lblNewPassword = new Label
            {
                Text = "New Password:",
                Location = new Point(20, 70),
                Size = new Size(120, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            txtNewPassword = new TextBox
            {
                Location = new Point(150, 70),
                Size = new Size(200, 23),
                UseSystemPasswordChar = true
            };
            txtNewPassword.TextChanged += TxtNewPassword_TextChanged;
            
            // Confirm Password
            lblConfirmPassword = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(20, 110),
                Size = new Size(120, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            txtConfirmPassword = new TextBox
            {
                Location = new Point(150, 110),
                Size = new Size(200, 23),
                UseSystemPasswordChar = true
            };
            txtConfirmPassword.TextChanged += TxtConfirmPassword_TextChanged;
            
            // Show Password Checkbox
            chkShowPassword = new CheckBox
            {
                Text = "Show Passwords",
                Location = new Point(150, 150),
                Size = new Size(120, 23),
                CheckAlign = ContentAlignment.MiddleLeft
            };
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;
            
            // Password Strength
            lblPasswordStrength = new Label
            {
                Text = "Password Strength:",
                Location = new Point(20, 190),
                Size = new Size(120, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            pbPasswordStrength = new ProgressBar
            {
                Location = new Point(150, 190),
                Size = new Size(200, 20),
                Minimum = 0,
                Maximum = 100
            };
            
            // Password Requirements
            lblRequirements = new Label
            {
                Text = "Password Requirements:\n" +
                       "• At least 8 characters\n" +
                       "• At least one uppercase letter\n" +
                       "• At least one lowercase letter\n" +
                       "• At least one number\n" +
                       "• At least one special character",
                Location = new Point(20, 220),
                Size = new Size(330, 100),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };
            
            // Change Password Button
            btnChangePassword = new Button
            {
                Text = "Change Password",
                Location = new Point(150, 330),
                Size = new Size(120, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnChangePassword.Click += BtnChangePassword_Click;
            
            // Cancel Button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(280, 330),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += BtnCancel_Click;
            
            panelMain.Controls.AddRange(new Control[] {
                lblCurrentPassword, txtCurrentPassword,
                lblNewPassword, txtNewPassword,
                lblConfirmPassword, txtConfirmPassword,
                chkShowPassword, lblPasswordStrength, pbPasswordStrength,
                lblRequirements, btnChangePassword, btnCancel
            });
            
            this.Controls.AddRange(new Control[] {
                panelHeader, panelMain
            });
        }
        
        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool showPassword = chkShowPassword.Checked;
            txtCurrentPassword.UseSystemPasswordChar = !showPassword;
            txtNewPassword.UseSystemPasswordChar = !showPassword;
            txtConfirmPassword.UseSystemPasswordChar = !showPassword;
        }
        
        private void TxtNewPassword_TextChanged(object sender, EventArgs e)
        {
            UpdatePasswordStrength();
            ValidateForm();
        }
        
        private void TxtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }
        
        private void UpdatePasswordStrength()
        {
            string password = txtNewPassword.Text;
            int strength = CalculatePasswordStrength(password);
            
            pbPasswordStrength.Value = strength;
            
            if (strength < 30)
            {
                pbPasswordStrength.ForeColor = Color.Red;
            }
            else if (strength < 60)
            {
                pbPasswordStrength.ForeColor = Color.Orange;
            }
            else if (strength < 80)
            {
                pbPasswordStrength.ForeColor = Color.Yellow;
            }
            else
            {
                pbPasswordStrength.ForeColor = Color.Green;
            }
        }
        
        private int CalculatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
                return 0;
            
            int score = 0;
            
            // Length
            if (password.Length >= 8) score += 25;
            if (password.Length >= 12) score += 10;
            
            // Character types
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) score += 15;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) score += 15;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]")) score += 15;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[^a-zA-Z0-9]")) score += 20;
            
            return Math.Min(score, 100);
        }
        
        private void ValidateForm()
        {
            bool isValid = true;
            
            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNewPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                isValid = false;
            }
            
            // Check password strength
            if (CalculatePasswordStrength(txtNewPassword.Text) < 60)
            {
                isValid = false;
            }
            
            // Check if passwords match
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                isValid = false;
            }
            
            btnChangePassword.Enabled = isValid;
        }
        
        private async void BtnChangePassword_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;
            
            try
            {
                btnChangePassword.Enabled = false;
                
                // Verify current password
                if (!await VerifyCurrentPassword())
                {
                    MessageBox.Show("Current password is incorrect.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCurrentPassword.Focus();
                    return;
                }
                
                // Update password
                await UpdatePassword();
                
                MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnChangePassword.Enabled = true;
            }
        }
        
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                MessageBox.Show("Please enter your current password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCurrentPassword.Focus();
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Please enter a new password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return false;
            }
            
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("New password and confirm password do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }
            
            if (CalculatePasswordStrength(txtNewPassword.Text) < 60)
            {
                MessageBox.Show("Password does not meet minimum strength requirements.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return false;
            }
            
            if (txtCurrentPassword.Text == txtNewPassword.Text)
            {
                MessageBox.Show("New password must be different from current password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return false;
            }
            
            return true;
        }
        
        private async Task<bool> VerifyCurrentPassword()
        {
            try
            {
                string hashedCurrentPassword = HashPassword(txtCurrentPassword.Text);
                string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                
                var parameters = new Dictionary<string, object>
                {
                    { "@username", currentUsername },
                    { "@password", hashedCurrentPassword }
                };
                
                return await Task.Run(() =>
                {
                    DataTable result = DatabaseManager.GetDataTable(query, parameters);
                    return Convert.ToInt32(result.Rows[0][0]) > 0;
                });
            }
            catch
            {
                return false;
            }
        }
        
        private async System.Threading.Tasks.Task UpdatePassword()
        {
            string hashedNewPassword = HashPassword(txtNewPassword.Text);
            string query = "UPDATE users SET password = @password, last_password_change = NOW() WHERE username = @username";
            
            var parameters = new System.Collections.Generic.Dictionary<string, object>
            {
                { "@password", hashedNewPassword },
                { "@username", currentUsername }
            };
            
            MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
            await Task.Run(async () => 
            {
                using (var connection = new MySqlConnection(DatabaseManager.Connection.ConnectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(mysqlParams);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            });
        }
        
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
