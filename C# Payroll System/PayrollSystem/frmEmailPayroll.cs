using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace PayrollSystem
{
    public partial class frmEmailPayroll : Form
    {
        private string attachmentPath;

        public frmEmailPayroll(string attachmentPath)
        {
            InitializeComponent();
            this.attachmentPath = attachmentPath;
        }
        
        // Test SMTP connection before attempting to send email
        private bool TestSmtpConnection(string server, int port, bool useSsl, string username, string password)
        {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient())
                {
                    // Set a short timeout for the connection test
                    var result = client.BeginConnect(server, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(5000); // 5 second timeout
                    
                    if (!success)
                    {
                        return false;
                    }
                    
                    client.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void FrmEmailPayroll_Load(object sender, EventArgs e)
        {
            // Set default values
            txtSubject.Text = $"Payroll Report - {DateTime.Now:MMMM yyyy}";
            txtBody.Text = $"Please find attached the payroll report for {DateTime.Now:MMMM yyyy}.\n\nRegards,\nAMCES Payroll System";
            
            // Enable SSL/TLS by default as most modern SMTP servers require it
            chkEnableSSL.Checked = true;
            
            // Set default port to 587 (common TLS port)
            if (string.IsNullOrEmpty(txtPort.Text))
            {
                txtPort.Text = "587";
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFrom.Text) || string.IsNullOrWhiteSpace(txtTo.Text) ||
                string.IsNullOrWhiteSpace(txtSmtpServer.Text) || string.IsNullOrWhiteSpace(txtPort.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPort.Text, out int port))
            {
                MessageBox.Show("Port must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                lblStatus.Text = "Testing connection...";
                Application.DoEvents();
                
                // Test connection before attempting to send
                if (!TestSmtpConnection(txtSmtpServer.Text, port, chkEnableSSL.Checked, txtUsername.Text, txtPassword.Text))
                {
                    MessageBox.Show("Cannot connect to the SMTP server. Please check your server address, port, and internet connection.\n\n" +
                        "If using Gmail, make sure you're using an App Password if you have 2-Step Verification enabled.", 
                        "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Connection failed.";
                    Cursor = Cursors.Default;
                    return;
                }
                
                lblStatus.Text = "Sending email...";
                Application.DoEvents();

                // Configure SMTP client based on server type
                string smtpServer = txtSmtpServer.Text.ToLower();
                bool isGmail = smtpServer.Contains("gmail") || smtpServer.Contains("smtp.gmail.com");
                
                using (SmtpClient client = new SmtpClient())
                {
                    // Set server and port
                    client.Host = txtSmtpServer.Text;
                    client.Port = port;
                    
                    // Configure SMTP client with robust settings
                    client.EnableSsl = chkEnableSSL.Checked;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Timeout = 120000; // 120 seconds timeout for better reliability
                    
                    // Always provide credentials when available
                    if (!string.IsNullOrWhiteSpace(txtUsername.Text))
                    {
                        client.Credentials = new NetworkCredential(txtUsername.Text, txtPassword.Text);
                    }
                    else
                    {
                        // Show error if authentication is likely required but credentials not provided
                        MessageBox.Show("Most SMTP servers require authentication. Please provide username and password.", 
                            "Authentication Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Cursor = Cursors.Default;
                        lblStatus.Text = "Email not sent. Authentication required.";
                        return;
                    }
                    
                    // Special handling for Gmail
                    if (isGmail)
                    {
                        // Gmail requires specific settings
                        client.Host = "smtp.gmail.com"; // Force correct server for Gmail
                        client.Port = 587; // Force correct port for Gmail
                        client.EnableSsl = true; // Force SSL for Gmail
                        lblStatus.Text = "Using Gmail-specific settings...";
                        Application.DoEvents();
                    }

                    // Create the mail message
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(txtFrom.Text);
                        
                        // Add recipients (support multiple recipients separated by semicolon or comma)
                        foreach (string recipient in txtTo.Text.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            message.To.Add(recipient.Trim());
                        }
                        
                        // Add CC recipients if provided
                        if (!string.IsNullOrWhiteSpace(txtCC.Text))
                        {
                            foreach (string recipient in txtCC.Text.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                message.CC.Add(recipient.Trim());
                            }
                        }

                        message.Subject = txtSubject.Text;
                        message.Body = txtBody.Text;
                        message.IsBodyHtml = false;

                        // Add attachment
                        if (File.Exists(attachmentPath))
                        {
                            message.Attachments.Add(new Attachment(attachmentPath));
                        }
                        else
                        {
                            MessageBox.Show("Attachment file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Cursor = Cursors.Default;
                            return;
                        }

                        // Send the message
                        client.Send(message);
                    }
                }

                MessageBox.Show("Email sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SmtpException smtpEx)
            {
                string errorMessage = smtpEx.Message;
                string suggestion = "";
                string detailedError = "";
                
                // Get inner exception details if available
                if (smtpEx.InnerException != null)
                {
                    detailedError = $"\n\nDetailed error: {smtpEx.InnerException.Message}";
                }
                
                if (errorMessage.Contains("5.7.0") && errorMessage.Contains("STARTTLS"))
                {
                    suggestion = "Please ensure 'Enable SSL/TLS' is checked and port 587 is used.";
                }
                else if (errorMessage.Contains("authentication"))
                {
                    suggestion = "Please verify your username and password are correct.";
                    if (txtSmtpServer.Text.ToLower().Contains("gmail"))
                    {
                        suggestion += "\nFor Gmail, you may need to use an App Password if 2-Step Verification is enabled.";
                    }
                }
                else if (errorMessage.Contains("Failure sending mail"))
                {
                    suggestion = "Please check your internet connection and verify the SMTP server address is correct.";
                    if (txtSmtpServer.Text.ToLower().Contains("gmail"))
                    {
                        suggestion += "\nIf using Gmail, make sure 'Less secure app access' is enabled or use an App Password.";
                        if (smtpEx.InnerException != null && smtpEx.InnerException.Message.Contains("net_io_connectionclosed"))
                        {
                            suggestion += "\nThe connection was closed by the server. This often happens with Gmail when you're not using an App Password.";
                        }
                    }
                }
                
                // Log the error to help with debugging
                System.IO.File.AppendAllText(
                    Path.Combine(Application.StartupPath, "email_error.log"),
                    $"[{DateTime.Now}] SMTP Error: {errorMessage}{detailedError}\r\n"
                );
                
                MessageBox.Show($"SMTP Error: {errorMessage}{detailedError}\n\n{suggestion}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Failed to send email.";
            }
            catch (Exception ex)
            {
                // Log the error to help with debugging
                System.IO.File.AppendAllText(
                    Path.Combine(Application.StartupPath, "email_error.log"),
                    $"[{DateTime.Now}] General Error: {ex.Message}\r\n{ex.StackTrace}\r\n"
                );
                
                MessageBox.Show($"Error sending email: {ex.Message}\n\nCheck email_error.log for more details.", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Failed to send email.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        // Add a help button click handler
        private void BtnHelp_Click(object sender, EventArgs e)
        {
            string helpText = 
                "Email Sending Tips:\n\n" +
                "For Gmail Users:\n" +
                "1. Use smtp.gmail.com as the SMTP server\n" +
                "2. Use port 587\n" +
                "3. Enable SSL/TLS\n" +
                "4. Use your full Gmail address as the username\n" +
                "5. For password, you need to use an App Password:\n" +
                "   - Go to your Google Account > Security\n" +
                "   - Enable 2-Step Verification if not already enabled\n" +
                "   - Go to App passwords\n" +
                "   - Select 'Mail' and 'Windows Computer'\n" +
                "   - Generate and use the 16-character password\n\n" +
                "For Other Email Providers:\n" +
                "- Check with your email provider for the correct SMTP settings\n" +
                "- Most providers require SSL/TLS enabled\n" +
                "- Make sure your account allows SMTP access";
                
            MessageBox.Show(helpText, "Email Setup Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}