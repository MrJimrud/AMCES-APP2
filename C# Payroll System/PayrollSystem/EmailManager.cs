using System;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace PayrollSystem
{
    public static class EmailManager
    {
        private static string smtpServer;
        private static int smtpPort;
        private static string smtpUsername;
        private static string smtpPassword;
        private static bool enableSsl;
        private static string emailFrom;
        private static string emailFromName;
        private static string subjectTemplate;
        private static string bodyTemplate;

        public static void LoadEmailSettings()
        {
            try
            {
                smtpServer = IniFileManager.ReadIni("Email", "SMTP_Server", "config.ini", "smtp.gmail.com");
                smtpPort = Convert.ToInt32(IniFileManager.ReadIni("Email", "SMTP_Port", "config.ini", "587"));
                smtpUsername = IniFileManager.ReadIni("Email", "SMTP_Username", "config.ini", "your-email@gmail.com");
                smtpPassword = IniFileManager.ReadIni("Email", "SMTP_Password", "config.ini", "your-app-password");
                enableSsl = Convert.ToBoolean(IniFileManager.ReadIni("Email", "SMTP_EnableSSL", "config.ini", "true"));
                emailFrom = IniFileManager.ReadIni("Email", "Email_From", "config.ini", smtpUsername);
                emailFromName = IniFileManager.ReadIni("Email", "Email_FromName", "config.ini", "Payroll System");
                subjectTemplate = IniFileManager.ReadIni("Email", "Email_Subject_Template", "config.ini", "Payslip for {PeriodName}");
                bodyTemplate = IniFileManager.ReadIni("Email", "Email_Body_Template", "config.ini", "Dear {EmployeeName},\n\nPlease find attached your payslip for {PeriodName}.\n\nBest regards,\nPayroll Team");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading email settings: {ex.Message}");
            }
        }

        public static async Task SendPayslipEmailAsync(string employeeEmail, string employeeName, string periodName, string payslipPath)
        {
            try
            {
                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    throw new Exception("Email settings not configured. Please configure SMTP settings in the configuration file.");
                }

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = enableSsl;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(emailFrom, emailFromName);
                        message.To.Add(employeeEmail);
                        message.Subject = subjectTemplate.Replace("{PeriodName}", periodName);
                        message.Body = bodyTemplate
                            .Replace("{EmployeeName}", employeeName)
                            .Replace("{PeriodName}", periodName);

                        if (File.Exists(payslipPath))
                        {
                            message.Attachments.Add(new Attachment(payslipPath));
                        }
                        else
                        {
                            throw new Exception("Payslip file not found.");
                        }

                        await client.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email: {ex.Message}");
            }
        }

        public static async Task SendBulkPayslipEmailsAsync(string periodName, string[] employeeEmails, string[] employeeNames, string[] payslipPaths)
        {
            if (employeeEmails.Length != employeeNames.Length || employeeEmails.Length != payslipPaths.Length)
            {
                throw new ArgumentException("The number of email addresses, employee names, and payslip paths must match.");
            }

            for (int i = 0; i < employeeEmails.Length; i++)
            {
                try
                {
                    await SendPayslipEmailAsync(employeeEmails[i], employeeNames[i], periodName, payslipPaths[i]);
                }
                catch (Exception ex)
                {
                    // Log the error but continue with the next email
                    Console.WriteLine($"Error sending email to {employeeEmails[i]}: {ex.Message}");
                }
            }
        }
    }
}