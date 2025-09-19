using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Mail;
using ClosedXML.Excel;

namespace PayrollSystem
{
    public partial class frmPayrollPreview : Form
    {
        private DataTable payrollData;
        private int currentPage = 1;
        private const int itemsPerPage = 20;

        public frmPayrollPreview(DataTable data)
    {
        InitializeComponent();
        payrollData = data;
    }

    public frmPayrollPreview(DataSet ds)
    {
        InitializeComponent();
        if (ds == null || ds.Tables.Count == 0)
            throw new ArgumentException("DataSet cannot be null or empty", nameof(ds));
        payrollData = ds.Tables[0];
    }

        private void FrmPayrollPreview_Load(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            if (payrollData == null || payrollData.Rows.Count == 0)
            {
                reportViewer.Text = "No payroll data available.";
                return;
            }

            int totalPages = (int)Math.Ceiling((double)payrollData.Rows.Count / itemsPerPage);
            int startIndex = (currentPage - 1) * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, payrollData.Rows.Count);

            // Update navigation controls
            btnPrevPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
            lblPageInfo.Text = $"Page {currentPage} of {totalPages}";

            // Generate report content
            System.Text.StringBuilder report = new System.Text.StringBuilder();

            // Company header
            report.AppendLine("AMCES PAYROLL SYSTEM".PadLeft(65));
            report.AppendLine("Payroll Report".PadLeft(60));
            report.AppendLine();

            // Period information
            if (payrollData.Rows.Count > 0)
            {
                DateTime startDate = payrollData.Rows[0]["start_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(payrollData.Rows[0]["start_date"]);
                DateTime endDate = payrollData.Rows[0]["end_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(payrollData.Rows[0]["end_date"]);
                DateTime payDate = payrollData.Rows[0]["pay_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(payrollData.Rows[0]["pay_date"]);

                report.AppendLine($"Period: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}");
                report.AppendLine($"Pay Date: {payDate:MM/dd/yyyy}");
                report.AppendLine();
            }

            // Column headers
            report.AppendLine(new string('=', 110));
            report.AppendLine("Employee Code   Employee Name                    Department         Basic Pay    OT Pay    Allowances    Deductions    Net Pay");
            report.AppendLine(new string('=', 110));

            // Data rows
            for (int i = startIndex; i < endIndex; i++)
            {
                DataRow row = payrollData.Rows[i];
                report.AppendLine(string.Format("{0,-14} {1,-32} {2,-16} {3,10:N2} {4,9:N2} {5,12:N2} {6,12:N2} {7,10:N2}",
                    row["employee_code"],
                    row["employee_name"],
                    row["department_name"],
                    row["basic_pay"] == DBNull.Value ? 0m : Convert.ToDecimal(row["basic_pay"]),
                    row["overtime_pay"] == DBNull.Value ? 0m : Convert.ToDecimal(row["overtime_pay"]),
                    row["allowances"] == DBNull.Value ? 0m : Convert.ToDecimal(row["allowances"]),
                    (row["sss_deduction"] == DBNull.Value ? 0m : Convert.ToDecimal(row["sss_deduction"])) +
                    (row["philhealth_deduction"] == DBNull.Value ? 0m : Convert.ToDecimal(row["philhealth_deduction"])) +
                    (row["pagibig_deduction"] == DBNull.Value ? 0m : Convert.ToDecimal(row["pagibig_deduction"])) +
                    (row["tax_deduction"] == DBNull.Value ? 0m : Convert.ToDecimal(row["tax_deduction"])) +
                    (row["other_deductions"] == DBNull.Value ? 0m : Convert.ToDecimal(row["other_deductions"])),
                    row["net_pay"] == DBNull.Value ? 0m : Convert.ToDecimal(row["net_pay"])));
            }

            report.AppendLine(new string('-', 110));

            // Page totals
            decimal pageBasicPay = 0, pageOTPay = 0, pageAllowances = 0, pageDeductions = 0, pageNetPay = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                DataRow row = payrollData.Rows[i];
                pageBasicPay += Convert.ToDecimal(row["basic_pay"]);
                pageOTPay += Convert.ToDecimal(row["overtime_pay"]);
                pageAllowances += Convert.ToDecimal(row["allowances"]);
                pageDeductions += Convert.ToDecimal(row["sss_deduction"]) +
                                Convert.ToDecimal(row["philhealth_deduction"]) +
                                Convert.ToDecimal(row["pagibig_deduction"]) +
                                Convert.ToDecimal(row["tax_deduction"]) +
                                Convert.ToDecimal(row["other_deductions"]);
                pageNetPay += Convert.ToDecimal(row["net_pay"]);
            }

            report.AppendLine(string.Format("Page Totals:{0,44}{1,10:N2} {2,9:N2} {3,12:N2} {4,12:N2} {5,10:N2}",
                "", pageBasicPay, pageOTPay, pageAllowances, pageDeductions, pageNetPay));

            // Display the report
            reportViewer.Text = report.ToString();
        }

        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                GenerateReport();
            }
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)payrollData.Rows.Count / itemsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                GenerateReport();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                // Print the report content
                Font printFont = new Font("Courier New", 10);
                float yPos = e.MarginBounds.Top;
                float leftMargin = e.MarginBounds.Left;
                float linesPerPage = e.MarginBounds.Height / printFont.GetHeight();

                // Print the current content
                string[] lines = reportViewer.Text.Split('\n');
                int count = 0;

                foreach (string line in lines)
                {
                    yPos = e.MarginBounds.Top + (count * printFont.GetHeight());
                    e.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos);
                    count++;

                    if (count >= linesPerPage)
                        break;
                }

                // Indicate if there are more pages to print
                if (count < lines.Length)
                {
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during printing: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (payrollData == null || payrollData.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to email.", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a temporary Excel file to attach
                string tempFile = Path.Combine(Path.GetTempPath(), $"Payroll_Report_{DateTime.Now:yyyyMMdd}.xlsx");
                
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Payroll Report");

                    // Add title
                    worksheet.Cell(1, 1).Value = "AMCES PAYROLL SYSTEM";
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Range(1, 1, 1, 8).Merge();
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Add period information
                    if (payrollData.Rows.Count > 0)
                    {
                        DateTime startDate = payrollData.Rows[0]["start_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(payrollData.Rows[0]["start_date"]);
                        DateTime endDate = payrollData.Rows[0]["end_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(payrollData.Rows[0]["end_date"]);
                        DateTime payDate = payrollData.Rows[0]["pay_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(payrollData.Rows[0]["pay_date"]);

                        worksheet.Cell(2, 1).Value = $"Period: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
                        worksheet.Cell(3, 1).Value = $"Pay Date: {payDate:MM/dd/yyyy}";
                        worksheet.Range(2, 1, 2, 8).Merge();
                        worksheet.Range(3, 1, 3, 8).Merge();
                    }

                    // Add headers
                    int headerRow = 5;
                    worksheet.Cell(headerRow, 1).Value = "Employee Code";
                    worksheet.Cell(headerRow, 2).Value = "Employee Name";
                    worksheet.Cell(headerRow, 3).Value = "Department";
                    worksheet.Cell(headerRow, 4).Value = "Basic Pay";
                    worksheet.Cell(headerRow, 5).Value = "OT Pay";
                    worksheet.Cell(headerRow, 6).Value = "Allowances";
                    worksheet.Cell(headerRow, 7).Value = "Deductions";
                    worksheet.Cell(headerRow, 8).Value = "Net Pay";

                    // Format header
                    var headerRange = worksheet.Range(headerRow, 1, headerRow, 8);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Add data
                    int rowIndex = headerRow + 1;
                    foreach (DataRow row in payrollData.Rows)
                    {
                        worksheet.Cell(rowIndex, 1).Value = row["employee_code"].ToString();
                        worksheet.Cell(rowIndex, 2).Value = row["employee_name"].ToString();
                        worksheet.Cell(rowIndex, 3).Value = row["department_name"].ToString();

                        decimal basicPay = Convert.ToDecimal(row["basic_pay"]);
                        decimal otPay = Convert.ToDecimal(row["overtime_pay"]);
                        decimal allowances = Convert.ToDecimal(row["allowances"]);
                        decimal deductions = Convert.ToDecimal(row["sss_deduction"]) +
                                           Convert.ToDecimal(row["philhealth_deduction"]) +
                                           Convert.ToDecimal(row["pagibig_deduction"]) +
                                           Convert.ToDecimal(row["tax_deduction"]) +
                                           Convert.ToDecimal(row["other_deductions"]);
                        decimal netPay = Convert.ToDecimal(row["net_pay"]);

                        worksheet.Cell(rowIndex, 4).Value = basicPay;
                        worksheet.Cell(rowIndex, 5).Value = otPay;
                        worksheet.Cell(rowIndex, 6).Value = allowances;
                        worksheet.Cell(rowIndex, 7).Value = deductions;
                        worksheet.Cell(rowIndex, 8).Value = netPay;

                        // Format currency cells
                        for (int col = 4; col <= 8; col++)
                        {
                            worksheet.Cell(rowIndex, col).Style.NumberFormat.Format = "#,##0.00";
                        }

                        rowIndex++;
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Save the workbook
                    workbook.SaveAs(tempFile);
                }

                // Show email dialog
                using (var emailForm = new frmEmailPayroll(tempFile))
                {
                    emailForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error preparing email: {ex.Message}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (payrollData == null || payrollData.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Save Payroll Report",
                    FileName = $"Payroll_Report_{DateTime.Now:yyyyMMdd}"
                };

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Payroll Report");

                    // Add title
                    worksheet.Cell(1, 1).Value = "AMCES PAYROLL SYSTEM";
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Range(1, 1, 1, 8).Merge();
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                    // Add period information
                    if (payrollData.Rows.Count > 0)
                    {
                        DateTime startDate = Convert.ToDateTime(payrollData.Rows[0]["start_date"]);
                        DateTime endDate = Convert.ToDateTime(payrollData.Rows[0]["end_date"]);
                        DateTime payDate = Convert.ToDateTime(payrollData.Rows[0]["pay_date"]);

                        worksheet.Cell(2, 1).Value = $"Period: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
                        worksheet.Cell(3, 1).Value = $"Pay Date: {payDate:MM/dd/yyyy}";
                        worksheet.Range(2, 1, 2, 8).Merge();
                        worksheet.Range(3, 1, 3, 8).Merge();
                    }

                    // Add headers
                    int headerRow = 5;
                    worksheet.Cell(headerRow, 1).Value = "Employee Code";
                    worksheet.Cell(headerRow, 2).Value = "Employee Name";
                    worksheet.Cell(headerRow, 3).Value = "Department";
                    worksheet.Cell(headerRow, 4).Value = "Basic Pay";
                    worksheet.Cell(headerRow, 5).Value = "OT Pay";
                    worksheet.Cell(headerRow, 6).Value = "Allowances";
                    worksheet.Cell(headerRow, 7).Value = "Deductions";
                    worksheet.Cell(headerRow, 8).Value = "Net Pay";

                    // Format header
                    var headerRange = worksheet.Range(headerRow, 1, headerRow, 8);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                    // Add data
                    int rowIndex = headerRow + 1;
                    decimal totalBasicPay = 0, totalOTPay = 0, totalAllowances = 0, totalDeductions = 0, totalNetPay = 0;

                    foreach (DataRow row in payrollData.Rows)
                    {
                        worksheet.Cell(rowIndex, 1).Value = row["employee_id"].ToString();
                        worksheet.Cell(rowIndex, 2).Value = row["employee_name"].ToString();
                        worksheet.Cell(rowIndex, 3).Value = row["department_name"].ToString();

                        decimal basicPay = Convert.ToDecimal(row["basic_pay"]);
                        decimal otPay = Convert.ToDecimal(row["overtime_pay"]);
                        decimal allowances = Convert.ToDecimal(row["allowances"]);
                        decimal deductions = Convert.ToDecimal(row["sss_deduction"]) +
                                           Convert.ToDecimal(row["philhealth_deduction"]) +
                                           Convert.ToDecimal(row["pagibig_deduction"]) +
                                           Convert.ToDecimal(row["tax_deduction"]) +
                                           Convert.ToDecimal(row["other_deductions"]);
                        decimal netPay = Convert.ToDecimal(row["net_pay"]);

                        worksheet.Cell(rowIndex, 4).Value = basicPay;
                        worksheet.Cell(rowIndex, 5).Value = otPay;
                        worksheet.Cell(rowIndex, 6).Value = allowances;
                        worksheet.Cell(rowIndex, 7).Value = deductions;
                        worksheet.Cell(rowIndex, 8).Value = netPay;

                        // Format currency cells
                        for (int col = 4; col <= 8; col++)
                        {
                            worksheet.Cell(rowIndex, col).Style.NumberFormat.Format = "#,##0.00";
                        }

                        // Add to totals
                        totalBasicPay += basicPay;
                        totalOTPay += otPay;
                        totalAllowances += allowances;
                        totalDeductions += deductions;
                        totalNetPay += netPay;

                        rowIndex++;
                    }

                    // Add totals row
                    int totalRow = rowIndex + 1;
                    worksheet.Cell(totalRow, 3).Value = "TOTALS:";
                    worksheet.Cell(totalRow, 3).Style.Font.Bold = true;
                    worksheet.Cell(totalRow, 3).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Right;

                    worksheet.Cell(totalRow, 4).Value = totalBasicPay;
                    worksheet.Cell(totalRow, 5).Value = totalOTPay;
                    worksheet.Cell(totalRow, 6).Value = totalAllowances;
                    worksheet.Cell(totalRow, 7).Value = totalDeductions;
                    worksheet.Cell(totalRow, 8).Value = totalNetPay;

                    // Format totals row
                    var totalsRange = worksheet.Range(totalRow, 4, totalRow, 8);
                    totalsRange.Style.Font.Bold = true;
                    totalsRange.Style.NumberFormat.Format = "#,##0.00";
                    totalsRange.Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    totalsRange.Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Double;

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Save the workbook
                    workbook.SaveAs(saveDialog.FileName);
                }

                MessageBox.Show("Payroll report exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open the file
                if (MessageBox.Show("Would you like to open the exported file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}