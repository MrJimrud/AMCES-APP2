using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySqlConnector;


namespace PayrollSystem
{
    public partial class frmBackup : Form
    {
        // DatabaseManager is static - no instance needed
        
        // Form Controls
        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelMain;
        private GroupBox gbBackup;
        private Label lblBackupPath;
        private TextBox txtBackupPath;
        private Button btnBrowseBackup;
        private Button btnBackup;
        private GroupBox gbRestore;
        private Label lblRestorePath;
        private TextBox txtRestorePath;
        private Button btnBrowseRestore;
        private Button btnRestore;
        private ProgressBar progressBar;
        private Label lblStatus;
        private Button btnClose;
        private RichTextBox rtbLog;
        
        public frmBackup()
        {
            InitializeComponent();
            SetDefaultPaths();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Database Backup & Restore";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            
            // Header Panel
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(41, 128, 185)
            };
            
            lblTitle = new Label
            {
                Text = "Database Backup & Restore",
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
                Size = new Size(640, 480)
            };
            
            // Backup GroupBox
            gbBackup = new GroupBox
            {
                Text = "Database Backup",
                Location = new Point(0, 0),
                Size = new Size(640, 120),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            
            lblBackupPath = new Label
            {
                Text = "Backup Location:",
                Location = new Point(20, 30),
                Size = new Size(120, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            txtBackupPath = new TextBox
            {
                Location = new Point(150, 30),
                Size = new Size(380, 23),
                ReadOnly = true
            };
            
            btnBrowseBackup = new Button
            {
                Text = "Browse",
                Location = new Point(540, 30),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBrowseBackup.Click += BtnBrowseBackup_Click;
            
            btnBackup = new Button
            {
                Text = "Start Backup",
                Location = new Point(270, 70),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnBackup.Click += BtnBackup_Click;
            
            gbBackup.Controls.AddRange(new Control[] {
                lblBackupPath, txtBackupPath, btnBrowseBackup, btnBackup
            });
            
            // Restore GroupBox
            gbRestore = new GroupBox
            {
                Text = "Database Restore",
                Location = new Point(0, 140),
                Size = new Size(640, 120),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            
            lblRestorePath = new Label
            {
                Text = "Restore File:",
                Location = new Point(20, 30),
                Size = new Size(120, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            txtRestorePath = new TextBox
            {
                Location = new Point(150, 30),
                Size = new Size(380, 23),
                ReadOnly = true
            };
            
            btnBrowseRestore = new Button
            {
                Text = "Browse",
                Location = new Point(540, 30),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBrowseRestore.Click += BtnBrowseRestore_Click;
            
            btnRestore = new Button
            {
                Text = "Start Restore",
                Location = new Point(270, 70),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnRestore.Click += BtnRestore_Click;
            
            gbRestore.Controls.AddRange(new Control[] {
                lblRestorePath, txtRestorePath, btnBrowseRestore, btnRestore
            });
            
            // Progress Bar
            progressBar = new ProgressBar
            {
                Location = new Point(0, 280),
                Size = new Size(640, 25),
                Style = ProgressBarStyle.Continuous
            };
            
            // Status Label
            lblStatus = new Label
            {
                Text = "Ready",
                Location = new Point(0, 315),
                Size = new Size(640, 23),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };
            
            // Log RichTextBox
            rtbLog = new RichTextBox
            {
                Location = new Point(0, 350),
                Size = new Size(640, 100),
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Font = new Font("Consolas", 8)
            };
            
            // Close Button
            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(560, 460),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += BtnClose_Click;
            
            panelMain.Controls.AddRange(new Control[] {
                gbBackup, gbRestore, progressBar, lblStatus, rtbLog, btnClose
            });
            
            this.Controls.AddRange(new Control[] {
                panelHeader, panelMain
            });
        }
        
        private void SetDefaultPaths()
        {
            string backupFolder = Path.Combine(Application.StartupPath, "Backups");
            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }
            
            string defaultBackupFile = Path.Combine(backupFolder, $"payroll_backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql");
            txtBackupPath.Text = defaultBackupFile;
        }
        
        private void BtnBrowseBackup_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
                sfd.DefaultExt = "sql";
                sfd.FileName = $"payroll_backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
                
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    txtBackupPath.Text = sfd.FileName;
                }
            }
        }
        
        private void BtnBrowseRestore_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
                ofd.DefaultExt = "sql";
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtRestorePath.Text = ofd.FileName;
                }
            }
        }
        
        private bool ValidateBackupPath()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBackupPath.Text))
                {
                    MessageBox.Show("Please specify a backup location.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string directory = Path.GetDirectoryName(txtBackupPath.Text);
                if (!Directory.Exists(directory))
                {
                    MessageBox.Show("The specified backup directory does not exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                try
                {
                    using (FileStream fs = File.Create(
                        Path.Combine(directory, "test_write_permission.tmp")))
                    {
                        fs.Close();
                    }
                    File.Delete(Path.Combine(directory, "test_write_permission.tmp"));
                }
                catch (Exception)
                {
                    MessageBox.Show("Cannot write to the specified backup location. Please check folder permissions.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (File.Exists(txtBackupPath.Text))
                {
                    var result = MessageBox.Show(
                        "The backup file already exists. Do you want to overwrite it?",
                        "File Exists",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating backup path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async void BtnBackup_Click(object sender, EventArgs e)
        {
            if (!ValidateBackupPath())
            {
                return;
            }
            
            try
            {
                btnBackup.Enabled = false;
                progressBar.Style = ProgressBarStyle.Marquee;
                lblStatus.Text = "Creating backup...";
                LogMessage("Starting database backup...");
                
                await System.Threading.Tasks.Task.Run(() => CreateBackup());
                
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
                lblStatus.Text = "Backup completed successfully";
                LogMessage("Backup completed successfully!");
                
                MessageBox.Show("Database backup completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                lblStatus.Text = "Backup failed";
                LogMessage($"Backup failed: {ex.Message}");
                MessageBox.Show($"Backup failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnBackup.Enabled = true;
            }
        }
        
        private bool ValidateRestorePath()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRestorePath.Text))
                {
                    MessageBox.Show("Please select a backup file to restore.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!File.Exists(txtRestorePath.Text))
                {
                    MessageBox.Show("The selected backup file does not exist.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                FileInfo fileInfo = new FileInfo(txtRestorePath.Text);
                if (fileInfo.Length == 0)
                {
                    MessageBox.Show("The selected backup file is empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!txtRestorePath.Text.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("The selected file must be a SQL backup file (.sql extension).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                try
                {
                    using (StreamReader sr = new StreamReader(txtRestorePath.Text))
                    {
                        string firstLine = sr.ReadLine()?.ToLower() ?? "";
                        if (!firstLine.Contains("mysqldump") && !firstLine.Contains("create database"))
                        {
                            MessageBox.Show("The selected file does not appear to be a valid MySQL backup file.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Cannot read the backup file. Please check file permissions.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                DialogResult result = MessageBox.Show(
                    "WARNING: This will replace all existing data in the database. Are you sure you want to continue?",
                    "Confirm Restore",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                return result == DialogResult.Yes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating restore path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async void BtnRestore_Click(object sender, EventArgs e)
        {
            if (!ValidateRestorePath())
            {
                return;
            }
            
            try
            {
                btnRestore.Enabled = false;
                progressBar.Style = ProgressBarStyle.Marquee;
                lblStatus.Text = "Restoring database...";
                LogMessage("Starting database restore...");
                
                await System.Threading.Tasks.Task.Run(() => RestoreDatabase(txtRestorePath.Text));
                
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
                lblStatus.Text = "Restore completed successfully";
                LogMessage("Restore completed successfully!");
                
                MessageBox.Show("Database restore completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                lblStatus.Text = "Restore failed";
                LogMessage($"Restore failed: {ex.Message}");
                MessageBox.Show($"Restore failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRestore.Enabled = true;
            }
        }
        
        private void CreateBackup()
        {
            try
            {
                // Use mysqldump command to create backup
                var connectionInfo = DatabaseManager.GetConnectionInfo();
                string arguments = $"--host={connectionInfo.Server} --user={connectionInfo.UserId} --password={connectionInfo.Password} --databases {connectionInfo.Database} --result-file=\"{txtBackupPath.Text}\"";
                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "mysqldump",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                
                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception($"mysqldump failed: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Backup operation failed: {ex.Message}");
            }
        }
        
        private void RestoreDatabase(string backupFilePath)
        {
            try
            {
                // Use mysql command to restore backup
                var connectionInfo = DatabaseManager.GetConnectionInfo();
                string arguments = $"--host={connectionInfo.Server} --user={connectionInfo.UserId} --password={connectionInfo.Password} {connectionInfo.Database}";
                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "mysql",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                
                using (Process process = Process.Start(startInfo))
                {
                    string sqlContent = File.ReadAllText(txtRestorePath.Text);
                    process.StandardInput.Write(sqlContent);
                    process.StandardInput.Close();
                    process.WaitForExit();
                    
                    if (process.ExitCode != 0)
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception($"mysql restore failed: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Restore operation failed: {ex.Message}");
            }
        }
        
        private void LogMessage(string message)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => LogMessage(message)));
                return;
            }
            
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            rtbLog.AppendText($"[{timestamp}] {message}\n");
            rtbLog.ScrollToCaret();
        }
        
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
