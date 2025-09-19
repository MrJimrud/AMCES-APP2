using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            this.Load += FrmLogin_Load;
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.btnCancel = new Button();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.lblTitle = new Label();
            this.panelMain = new Panel();
            this.SuspendLayout();

            // 
            // panelMain
            // 
            this.panelMain.BackColor = Color.FromArgb(64, 64, 64);
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Controls.Add(this.lblUsername);
            this.panelMain.Controls.Add(this.txtUsername);
            this.panelMain.Controls.Add(this.lblPassword);
            this.panelMain.Controls.Add(this.txtPassword);
            this.panelMain.Controls.Add(this.btnLogin);
            this.panelMain.Controls.Add(this.btnCancel);
            this.panelMain.Dock = DockStyle.Fill;
            this.panelMain.Location = new Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new Size(400, 300);
            this.panelMain.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(100, 50);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(200, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Payroll System";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new Font("Segoe UI", 10F);
            this.lblUsername.ForeColor = Color.White;
            this.lblUsername.Location = new Point(50, 120);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(71, 19);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username:";

            // 
            // txtUsername
            // 
            this.txtUsername.Font = new Font("Segoe UI", 10F);
            this.txtUsername.Location = new Point(150, 117);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(200, 25);
            this.txtUsername.Text = "admin";        // Default username for testing
            this.txtUsername.TabIndex = 2;

            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new Font("Segoe UI", 10F);
            this.lblPassword.ForeColor = Color.White;
            this.lblPassword.Location = new Point(50, 160);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(67, 19);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";

            // 
            // txtPassword
            // 
            this.txtPassword.Font = new Font("Segoe UI", 10F);
            this.txtPassword.Location = new Point(150, 157);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(200, 25);
            this.txtPassword.Text = "admin";        // Default password for testing 
            this.txtPassword.TabIndex = 4;
            this.txtPassword.KeyPress += TxtPassword_KeyPress;

            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.Location = new Point(150, 210);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(90, 35);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += BtnLogin_Click;

            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = Color.FromArgb(192, 57, 43);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(260, 210);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(90, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += BtnCancel_Click;

            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 300);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Payroll System - Login";
            this.ResumeLayout(false);
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                // Using admin user credentials from config.ini
                
                // Load configuration first
                DatabaseManager.LoadConfiguration();

                
                // Initialize database connection
                DatabaseManager.InitializeConnection();
                
                // Set focus to username textbox
                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PerformLogin();
            }
        }

        private void PerformLogin()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please enter username.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter password.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Authenticate user
                if (AuthenticateUser(txtUsername.Text.Trim(), txtPassword.Text))
                {
                    // Store current user information
                    DatabaseManager.CurrentUser = txtUsername.Text.Trim();
                    
                    // Hide login form and show main form
                    this.Hide();
                    
                    frmMain mainForm = new frmMain();
                    mainForm.ShowDialog();
                    
                    // Close application when main form is closed
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Authentication Failed", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            try
            {
                // Ensure connection is open
                if (!DatabaseManager.OpenConnection())
                {
                    throw new Exception("Connection must be valid and open.");
                }
                
                string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password AND is_active = 1";
                
                using (MySqlCommand cmd = new MySqlCommand(query, DatabaseManager.Connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password); // In production, use hashed passwords
                    
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Authentication error: {ex.Message}");
            }
            finally
            {
                DatabaseManager.CloseConnection();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Designer Variables
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancel;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblTitle;
        private Panel panelMain;
        #endregion
    }
}