using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using PayrollSystem.Properties;

namespace PayrollSystem
{
    public partial class frmLoginEmployee : Form
    {
        private Panel pnlMain;
        private Panel pnlLeft;
        private Panel pnlRight;
        private PictureBox picLogo;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkRememberMe;
        private Button btnLogin;
        private Button btnCancel;
        private LinkLabel lnkForgotPassword;
        private Label lblVersion;
        private ProgressBar progressBar;
        private Label lblStatus;
        private System.Windows.Forms.Timer loginTimer;
        
        public string LoggedInEmployeeId { get; private set; }
        public string LoggedInEmployeeName { get; private set; }
        public string LoggedInUserRole { get; private set; }
        public bool LoginSuccessful { get; private set; }

        public frmLoginEmployee()
        {
            InitializeComponent();
            LoadRememberedCredentials();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee Login - Payroll System";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Icon = SystemIcons.Application;

            // Main panel
            pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Left panel with branding
            pnlLeft = new Panel
            {
                Width = 350,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(52, 73, 94)
            };

            // Logo
            picLogo = new PictureBox
            {
                Size = new Size(120, 120),
                Location = new Point(115, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            // Add a simple logo placeholder
            picLogo.Paint += (s, e) => {
                e.Graphics.DrawString("PAYROLL\nSYSTEM", new Font("Arial", 12, FontStyle.Bold), 
                    Brushes.DarkBlue, new RectangleF(10, 30, 100, 60), 
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            };

            // Title and subtitle
            lblTitle = new Label
            {
                Text = "Employee Portal",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 220),
                Size = new Size(250, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblSubtitle = new Label
            {
                Text = "Secure access to your payroll information",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(30, 260),
                Size = new Size(290, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Version label
            lblVersion = new Label
            {
                Text = "Version 1.0.0",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(149, 165, 166),
                Location = new Point(10, 450),
                Size = new Size(100, 20)
            };

            pnlLeft.Controls.AddRange(new Control[] { picLogo, lblTitle, lblSubtitle, lblVersion });

            // Right panel with login form
            pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(50, 80, 50, 50)
            };

            // Login form title
            Label lblLoginTitle = new Label
            {
                Text = "Employee Login",
                Font = new Font("Segoe UI", 24, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(50, 50),
                Size = new Size(300, 40)
            };

            Label lblLoginSubtitle = new Label
            {
                Text = "Please enter your credentials to continue",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(50, 95),
                Size = new Size(300, 20)
            };

            // Username
            lblUsername = new Label
            {
                Text = "Employee ID / Username:",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(50, 140),
                Size = new Size(200, 20)
            };

            txtUsername = new TextBox
            {
                Location = new Point(50, 165),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtUsername.SetPlaceholderText("Enter your employee ID or username");
            txtUsername.KeyPress += TxtUsername_KeyPress;

            // Password
            lblPassword = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(50, 210),
                Size = new Size(100, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(50, 235),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true
            };
            txtPassword.SetPlaceholderText("Enter your password");
            txtPassword.KeyPress += TxtPassword_KeyPress;

            // Remember me checkbox
            chkRememberMe = new CheckBox
            {
                Text = "Remember my credentials",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(50, 280),
                Size = new Size(200, 20)
            };

            // Forgot password link
            lnkForgotPassword = new LinkLabel
            {
                Text = "Forgot Password?",
                Font = new Font("Segoe UI", 9),
                LinkColor = Color.FromArgb(52, 152, 219),
                Location = new Point(250, 280),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            lnkForgotPassword.Click += LnkForgotPassword_Click;

            // Login button
            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(50, 320),
                Size = new Size(140, 40),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(210, 320),
                Size = new Size(140, 40),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(50, 380),
                Size = new Size(300, 5),
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Visible = false
            };

            // Status label
            lblStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(231, 76, 60),
                Location = new Point(50, 395),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlRight.Controls.AddRange(new Control[] { 
                lblLoginTitle, lblLoginSubtitle, lblUsername, txtUsername, 
                lblPassword, txtPassword, chkRememberMe, lnkForgotPassword,
                btnLogin, btnCancel, progressBar, lblStatus
            });

            pnlMain.Controls.AddRange(new Control[] { pnlLeft, pnlRight });
            this.Controls.Add(pnlMain);

            // Initialize timer
            loginTimer = new System.Windows.Forms.Timer
            {
                Interval = 2000 // 2 seconds
            };
            loginTimer.Tick += LoginTimer_Tick;

            // Set default button
            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancel;
        }

        private void LoadRememberedCredentials()
        {
            try
            {
                // Load remembered credentials from settings or registry
                string rememberedUsername = Properties.Settings.Default.RememberedUsername;
                bool rememberCredentials = Properties.Settings.Default.RememberCredentials;

                if (rememberCredentials && !string.IsNullOrEmpty(rememberedUsername))
                {
                    txtUsername.Text = rememberedUsername;
                    chkRememberMe.Checked = true;
                    txtPassword.Focus();
                }
                else
                {
                    txtUsername.Focus();
                }
            }
            catch
            {
                // Ignore errors when loading settings
                txtUsername.Focus();
            }
        }

        private void SaveCredentials()
        {
            try
            {
                if (chkRememberMe.Checked)
                {
                    Properties.Settings.Default.RememberedUsername = txtUsername.Text.Trim();
                    Properties.Settings.Default.RememberCredentials = true;
                }
                else
                {
                    Properties.Settings.Default.RememberedUsername = "";
                    Properties.Settings.Default.RememberCredentials = false;
                }
                Properties.Settings.Default.Save();
            }
            catch
            {
                // Ignore errors when saving settings
            }
        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnLogin_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                PerformLogin();
            }
        }

        private bool ValidateInput()
        {
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.FromArgb(231, 76, 60);

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                lblStatus.Text = "Please enter your employee ID or username.";
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblStatus.Text = "Please enter your password.";
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private void PerformLogin()
        {
            try
            {
                // Show progress
                SetLoginInProgress(true);
                lblStatus.Text = "Authenticating...";
                lblStatus.ForeColor = Color.FromArgb(52, 152, 219);

                // Start timer for simulated delay
                loginTimer.Start();
            }
            catch (Exception ex)
            {
                SetLoginInProgress(false);
                lblStatus.Text = $"Login error: {ex.Message}";
                lblStatus.ForeColor = Color.FromArgb(231, 76, 60);
            }
        }

        private void LoginTimer_Tick(object sender, EventArgs e)
        {
            loginTimer.Stop();
            ProcessLogin();
        }

        private void ProcessLogin()
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;
                string hashedPassword = HashPassword(password);

                // Query to authenticate employee
                string query = $@"
                    SELECT 
                        e.id,
                        CONCAT(e.first_name, ' ', e.last_name) as full_name,
                        e.employee_id,
                        e.email,
                        e.user_role,
                        e.is_active,
                        e.password_hash,
                        d.department_name,
                        jt.job_title
                    FROM employees e
                    LEFT JOIN departments d ON e.department_id = d.department_id
                    LEFT JOIN job_titles jt ON e.job_title_id = jt.job_title_id
                    WHERE (e.employee_id = '{username}' OR e.email = '{username}') 
                    AND e.is_active = 1";

                DataTable dt = UtilityHelper.GetDataSet(query);

                if (dt.Rows.Count == 0)
                {
                    SetLoginInProgress(false);
                    lblStatus.Text = "Invalid credentials or account is inactive.";
                    lblStatus.ForeColor = Color.FromArgb(231, 76, 60);
                    return;
                }

                DataRow row = dt.Rows[0];
                string storedPasswordHash = row["password_hash"].ToString();

                // Verify password
                if (VerifyPassword(password, storedPasswordHash))
                {
                    // Login successful
                    LoggedInEmployeeId = row["id"].ToString();
                    LoggedInEmployeeName = row["full_name"].ToString();
                    LoggedInUserRole = row["user_role"].ToString();
                    LoginSuccessful = true;

                    // Save credentials if remember me is checked
                    SaveCredentials();

                    // Log the login activity
                    LogLoginActivity(LoggedInEmployeeId, true);

                    // Update global variables
                    GlobalVariables.CurrentUserId = int.Parse(LoggedInEmployeeId);
                    GlobalVariables.CurrentUserName = LoggedInEmployeeName;
                    GlobalVariables.CurrentUserRole = LoggedInUserRole;

                    SetLoginInProgress(false);
                    lblStatus.Text = "Login successful! Welcome back.";
                    lblStatus.ForeColor = Color.FromArgb(39, 174, 96);

                    // Close form after short delay
                    System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer { Interval = 1000 };
                    closeTimer.Tick += (s, args) => {
                        closeTimer.Stop();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    };
                    closeTimer.Start();
                }
                else
                {
                    // Invalid password
                    LogLoginActivity(row["id"].ToString(), false);
                    SetLoginInProgress(false);
                    lblStatus.Text = "Invalid credentials. Please try again.";
                    lblStatus.ForeColor = Color.FromArgb(231, 76, 60);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                SetLoginInProgress(false);
                lblStatus.Text = $"Login error: {ex.Message}";
                lblStatus.ForeColor = Color.FromArgb(231, 76, 60);
            }
        }

        private void SetLoginInProgress(bool inProgress)
        {
            btnLogin.Enabled = !inProgress;
            btnCancel.Enabled = !inProgress;
            txtUsername.Enabled = !inProgress;
            txtPassword.Enabled = !inProgress;
            chkRememberMe.Enabled = !inProgress;
            progressBar.Visible = inProgress;

            if (inProgress)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
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

        private bool VerifyPassword(string password, string storedHash)
        {
            string hashedPassword = HashPassword(password);
            return string.Equals(hashedPassword, storedHash, StringComparison.OrdinalIgnoreCase);
        }

        private void LogLoginActivity(string employeeId, bool successful)
        {
            try
            {
                string query = $@"
                    INSERT INTO tbl_login_log (employee_id, login_time, ip_address, user_agent, successful, created_date)
                    VALUES ({employeeId}, NOW(), 'Local', 'Payroll System Desktop', {(successful ? 1 : 0)}, NOW())";
                
                DatabaseManager.ExecuteNonQuery(query);
            }
            catch
            {
                // Ignore logging errors
            }
        }

        private void LnkForgotPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact your system administrator to reset your password.", 
                "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (loginTimer != null)
            {
                loginTimer.Stop();
                loginTimer.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}
