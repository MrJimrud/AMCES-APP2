using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;
using MySqlConnector;
using PayrollSystem.Extensions;

namespace PayrollSystem
{
    public partial class FrmDTR : Form
    {
        private DataGridView dgvDTR;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private ComboBox cmbEmployee;
        private ComboBox cmbDepartment;
        private Button btnSearch;
        private Button btnTimeIn;
        private Button btnTimeOut;
        private Button btnBreakOut;
        private Button btnBreakIn;
        private Button btnGenerateQR;
        private Button btnScanQR;
        private Button btnExport;
        private Button btnClose;
        private Label lblFromDate;
        private Label lblToDate;
        private Label lblEmployee;
        private Label lblDepartment;
        private Label lblCurrentTime;
        private Label lblStatus;
        private System.Windows.Forms.Timer timerClock;
        private Panel pnlControls;
        private Panel pnlGrid;
        private PictureBox picQRCode;
        private TextBox txtQRData;
        private GroupBox grpQRCode;
        private GroupBox grpTimeTracking;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatusText;

        public FrmDTR()
        {
            dtpToDate = new DateTimePicker();
            cmbEmployee = new ComboBox();
            InitializeComponent();
            LoadEmployees();
            LoadDepartments();
            InitializeTimer();
            LoadDTRRecords();
        }

        private void InitializeComponent()
        {
            this.dgvDTR = new DataGridView();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.cmbEmployee = new ComboBox();
            this.cmbDepartment = new ComboBox();
            this.btnSearch = new Button();
            this.btnTimeIn = new Button();
            this.btnTimeOut = new Button();
            this.btnBreakOut = new Button();
            this.btnBreakIn = new Button();
            this.btnGenerateQR = new Button();
            this.btnScanQR = new Button();
            this.btnExport = new Button();
            this.btnClose = new Button();
            this.lblFromDate = new Label();
            this.lblToDate = new Label();
            this.lblEmployee = new Label();
            this.lblDepartment = new Label();
            this.lblCurrentTime = new Label();
            this.lblStatus = new Label();
            this.timerClock = new System.Windows.Forms.Timer();
            this.pnlControls = new Panel();
            this.pnlGrid = new Panel();
            this.picQRCode = new PictureBox();
            this.txtQRData = new TextBox();
            this.grpQRCode = new GroupBox();
            this.grpTimeTracking = new GroupBox();
            this.statusStrip = new StatusStrip();
            this.lblStatusText = new ToolStripStatusLabel();
            
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Daily Time Record (DTR) Management";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            
            // Controls Panel
            this.pnlControls.Location = new Point(12, 12);
            this.pnlControls.Size = new Size(1176, 100);
            this.pnlControls.BorderStyle = BorderStyle.FixedSingle;
            
            // Date Range Controls
            this.lblFromDate.Text = "From Date:";
            this.lblFromDate.Location = new Point(10, 15);
            this.lblFromDate.Size = new Size(70, 23);
            
            this.dtpFromDate.Location = new Point(85, 12);
            this.dtpFromDate.Size = new Size(120, 23);
            this.dtpFromDate.Format = DateTimePickerFormat.Short;
            this.dtpFromDate.Value = DateTime.Today.AddDays(-7);
            
            this.lblToDate.Text = "To Date:";
            this.lblToDate.Location = new Point(220, 15);
            this.lblToDate.Size = new Size(60, 23);
            
            this.dtpToDate.Location = new Point(285, 12);
            this.dtpToDate.Size = new Size(120, 23);
            this.dtpToDate.Format = DateTimePickerFormat.Short;
            this.dtpToDate.Value = DateTime.Today;
            
            // Employee and Department filters
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.Location = new Point(420, 15);
            this.lblEmployee.Size = new Size(70, 23);
            
            this.cmbEmployee.Location = new Point(495, 12);
            this.cmbEmployee.Size = new Size(200, 23);
            this.cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            
            this.lblDepartment.Text = "Department:";
            this.lblDepartment.Location = new Point(710, 15);
            this.lblDepartment.Size = new Size(80, 23);
            
            this.cmbDepartment.Location = new Point(795, 12);
            this.cmbDepartment.Size = new Size(150, 23);
            this.cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDepartment.SelectedIndexChanged += new EventHandler(this.CmbDepartment_SelectedIndexChanged);
            
            this.btnSearch.Text = "Search";
            this.btnSearch.Location = new Point(960, 12);
            this.btnSearch.Size = new Size(80, 25);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            
            // Current Time Display
            this.lblCurrentTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
            this.lblCurrentTime.Location = new Point(10, 45);
            this.lblCurrentTime.Size = new Size(400, 23);
            this.lblCurrentTime.Font = new Font("Arial", 10, FontStyle.Bold);
            this.lblCurrentTime.ForeColor = System.Drawing.Color.Blue;
            
            this.lblStatus.Text = "Ready";
            this.lblStatus.Location = new Point(10, 70);
            this.lblStatus.Size = new Size(400, 23);
            this.lblStatus.Font = new Font("Arial", 9, FontStyle.Regular);
            this.lblStatus.ForeColor = System.Drawing.Color.Green;
            
            // Export and Close buttons
            this.btnExport.Text = "Export";
            this.btnExport.Location = new Point(960, 45);
            this.btnExport.Size = new Size(80, 25);
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            
            this.btnClose.Text = "Close";
            this.btnClose.Location = new Point(960, 70);
            this.btnClose.Size = new Size(80, 25);
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            
            // Add controls to panel
            this.pnlControls.Controls.Add(this.lblFromDate);
            this.pnlControls.Controls.Add(this.dtpFromDate);
            this.pnlControls.Controls.Add(this.lblToDate);
            this.pnlControls.Controls.Add(this.dtpToDate);
            this.pnlControls.Controls.Add(this.lblEmployee);
            this.pnlControls.Controls.Add(this.cmbEmployee);
            this.pnlControls.Controls.Add(this.lblDepartment);
            this.pnlControls.Controls.Add(this.cmbDepartment);
            this.pnlControls.Controls.Add(this.btnSearch);
            this.pnlControls.Controls.Add(this.lblCurrentTime);
            this.pnlControls.Controls.Add(this.lblStatus);
            this.pnlControls.Controls.Add(this.btnExport);
            this.pnlControls.Controls.Add(this.btnClose);
            
            // Time Tracking Group
            this.grpTimeTracking.Text = "Time Tracking";
            this.grpTimeTracking.Location = new Point(12, 120);
            this.grpTimeTracking.Size = new Size(580, 120);
            
            this.btnTimeIn.Text = "Time In";
            this.btnTimeIn.Location = new Point(20, 30);
            this.btnTimeIn.Size = new Size(100, 35);
            this.btnTimeIn.BackColor = System.Drawing.Color.LightGreen;
            this.btnTimeIn.Click += new EventHandler(this.btnTimeIn_Click);
            
            this.btnBreakOut.Text = "Break Out";
            this.btnBreakOut.Location = new Point(140, 30);
            this.btnBreakOut.Size = new Size(100, 35);
            this.btnBreakOut.BackColor = System.Drawing.Color.LightYellow;
            this.btnBreakOut.Click += new EventHandler(this.btnBreakOut_Click);
            
            this.btnBreakIn.Text = "Break In";
            this.btnBreakIn.Location = new Point(260, 30);
            this.btnBreakIn.Size = new Size(100, 35);
            this.btnBreakIn.BackColor = System.Drawing.Color.LightBlue;
            this.btnBreakIn.Click += new EventHandler(this.btnBreakIn_Click);
            
            this.btnTimeOut.Text = "Time Out";
            this.btnTimeOut.Location = new Point(380, 30);
            this.btnTimeOut.Size = new Size(100, 35);
            this.btnTimeOut.BackColor = System.Drawing.Color.LightCoral;
            this.btnTimeOut.Click += new EventHandler(this.btnTimeOut_Click);
            
            this.grpTimeTracking.Controls.Add(this.btnTimeIn);
            this.grpTimeTracking.Controls.Add(this.btnBreakOut);
            this.grpTimeTracking.Controls.Add(this.btnBreakIn);
            this.grpTimeTracking.Controls.Add(this.btnTimeOut);
            
            // QR Code Group
            this.grpQRCode.Text = "QR Code Management";
            this.grpQRCode.Location = new Point(608, 120);
            this.grpQRCode.Size = new Size(580, 120);
            
            this.picQRCode.Location = new Point(20, 25);
            this.picQRCode.Size = new Size(80, 80);
            this.picQRCode.BorderStyle = BorderStyle.FixedSingle;
            this.picQRCode.SizeMode = PictureBoxSizeMode.StretchImage;
            
            this.txtQRData.Location = new Point(120, 25);
            this.txtQRData.Size = new Size(200, 23);
            this.txtQRData.SetPlaceholderText("Enter employee ID or scan QR");
            
            this.btnGenerateQR.Text = "Generate QR";
            this.btnGenerateQR.Location = new Point(340, 25);
            this.btnGenerateQR.Size = new Size(100, 30);
            this.btnGenerateQR.Click += new EventHandler(this.btnGenerateQR_Click);
            
            this.btnScanQR.Text = "Scan QR";
            this.btnScanQR.Location = new Point(460, 25);
            this.btnScanQR.Size = new Size(100, 30);
            this.btnScanQR.Click += new EventHandler(this.btnScanQR_Click);
            
            this.grpQRCode.Controls.Add(this.picQRCode);
            this.grpQRCode.Controls.Add(this.txtQRData);
            this.grpQRCode.Controls.Add(this.btnGenerateQR);
            this.grpQRCode.Controls.Add(this.btnScanQR);
            
            // Grid Panel
            this.pnlGrid.Location = new Point(12, 250);
            this.pnlGrid.Size = new Size(1176, 400);
            this.pnlGrid.BorderStyle = BorderStyle.FixedSingle;
            
            // DataGridView
            this.dgvDTR.Location = new Point(5, 5);
            this.dgvDTR.Size = new Size(1166, 390);
            this.dgvDTR.AllowUserToAddRows = false;
            this.dgvDTR.AllowUserToDeleteRows = false;
            this.dgvDTR.ReadOnly = true;
            this.dgvDTR.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvDTR.MultiSelect = false;
            this.dgvDTR.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            this.pnlGrid.Controls.Add(this.dgvDTR);
            
            // Status Strip
            this.lblStatusText.Text = "Ready";
            this.statusStrip.Items.Add(this.lblStatusText);
            
            // Add controls to form
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.grpTimeTracking);
            this.Controls.Add(this.grpQRCode);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.statusStrip);
            
            this.ResumeLayout(false);
        }

        private void InitializeTimer()
        {
            timerClock.Interval = 1000; // Update every second
            timerClock.Tick += TimerClock_Tick;
            timerClock.Start();
        }

        private void TimerClock_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        }

        private void LoadEmployees()
        {
            try
            {
                cmbEmployee.Items.Clear();
                cmbEmployee.Items.Add("All Employees");
                
                string query = "SELECT employee_id, CONCAT(first_name, ' ', last_name) as full_name FROM employees WHERE employment_status = 'Active' ORDER BY last_name, first_name";
                DataTable dt = DatabaseManager.ExecuteQuery(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbEmployee.Items.Add($"{row["employee_id"]} - {row["full_name"]}");
                }
                
                cmbEmployee.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                cmbDepartment.Items.Clear();
                cmbDepartment.Items.Add("All Departments");
                
                string query = "SELECT department_name FROM departments ORDER BY department_name";
                DataTable dt = DatabaseManager.ExecuteQuery(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbDepartment.Items.Add(row["department_name"].ToString());
                }
                
                cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDTRRecords()
        {
            try
            {
                string whereClause = "WHERE dtr.dtr_date BETWEEN @fromDate AND @toDate";
                
                if (cmbEmployee.SelectedIndex > 0)
                {
                    string selectedEmployee = cmbEmployee.SelectedItem.ToString();
                    string employeeId = selectedEmployee.Split('-')[0].Trim();
                    whereClause += " AND dtr.employee_id = @employeeId";
                }
                
                if (cmbDepartment.SelectedIndex > 0)
                {
                    whereClause += " AND d.department_name = @departmentName";
                }
                
                string query = $@"SELECT 
                    dtr.dtr_id as 'DTR ID',
                    dtr.employee_id as 'Employee ID',
                    CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                    d.department_name as 'Department',
                    dtr.dtr_date as 'Date',
                    TIME_FORMAT(dtr.time_in, '%H:%i') as 'Time In',
                    TIME_FORMAT(dtr.break_out, '%H:%i') as 'Break Out',
                    TIME_FORMAT(dtr.break_in, '%H:%i') as 'Break In',
                    TIME_FORMAT(dtr.time_out, '%H:%i') as 'Time Out',
                    ROUND(TIMESTAMPDIFF(MINUTE, 
                        dtr.time_in, 
                        dtr.time_out) / 60.0 - 
                        IFNULL(TIMESTAMPDIFF(MINUTE, 
                            dtr.break_out, 
                            dtr.break_in) / 60.0, 0), 2) as 'Total Hours',
                    ROUND(GREATEST(
                        TIMESTAMPDIFF(MINUTE, 
                            dtr.time_in, 
                            dtr.time_out) / 60.0 - 
                        IFNULL(TIMESTAMPDIFF(MINUTE, 
                            dtr.break_out, 
                            dtr.break_in) / 60.0, 0) - 8, 0), 2) as 'Overtime Hours',
                    dtr.status as 'Status'
                FROM dtr_records dtr
                LEFT JOIN employees e ON dtr.employee_id = e.employee_id
                LEFT JOIN departments d ON e.department_id = d.department_id
                {whereClause}
                ORDER BY dtr.dtr_date DESC, dtr.time_in DESC";
                
                var parameters = new System.Collections.Generic.Dictionary<string, object> { ["@fromDate"] = dtpFromDate.Value.Date, ["@toDate"] = dtpToDate.Value.Date };
                
                if (cmbEmployee.SelectedIndex > 0)
                {
                    string selectedEmployee = cmbEmployee.SelectedItem.ToString();
                    string employeeId = selectedEmployee.Split('-')[0].Trim();
                    parameters.Add("@employeeId", employeeId);
                }
                
                if (cmbDepartment.SelectedIndex > 0)
                {
                    parameters.Add("@departmentName", cmbDepartment.SelectedItem.ToString());
                }
                
                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                DataTable dt = DatabaseManager.ExecuteQuery(query, mysqlParams);

                dgvDTR.DataSource = dt;
                
                lblStatusText.Text = $"Records found: {dt.Rows.Count}";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading DTR records: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatTimeColumns()
        {
            string[] timeColumns = { "Time In", "Break Out", "Break In", "Time Out" };
            
            foreach (string column in timeColumns)
            {
                dgvDTR.SetTimeSpanFormat(column);
            }
            
            if (dgvDTR.Columns["Total Hours"] != null)
            {
                dgvDTR.Columns["Total Hours"].DefaultCellStyle.Format = "0.00";
            }
            
            if (dgvDTR.Columns["Overtime Hours"] != null)
            {
                dgvDTR.Columns["Overtime Hours"].DefaultCellStyle.Format = "0.00";
            }
        }

        private void RecordTimeEntry(string entryType)
        {
            if (string.IsNullOrWhiteSpace(txtQRData.Text))
            {
                MessageBox.Show("Please enter employee ID or scan QR code first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            try
            {
                string employeeId = txtQRData.Text.Trim();
                DateTime currentTime = DateTime.Now;
                DateTime currentDate = currentTime.Date;
                
                // Check if employee exists
                string checkEmployeeQuery = "SELECT COUNT(*) FROM employees WHERE employee_id = @employeeId AND employment_status = 'Active'";
                var checkParams = new System.Collections.Generic.Dictionary<string, object> { ["@employeeId"] = employeeId };
                
                MySqlParameter[] checkMysqlParams = checkParams.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int employeeExists = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkEmployeeQuery, checkMysqlParams));
                
                if (employeeExists == 0)
                {
                    MessageBox.Show("Employee not found or inactive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Check if DTR record exists for today
                string checkDTRQuery = "SELECT dtr_id FROM dtr_records WHERE employee_id = @employeeId AND dtr_date = @dtrDate";
                var dtrParams = new System.Collections.Generic.Dictionary<string, object> { ["@employeeId"] = employeeId, ["@dtrDate"] = currentDate };
                
                MySqlParameter[] dtrMysqlParams = dtrParams.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                object dtrId = DatabaseManager.ExecuteScalar(checkDTRQuery, dtrMysqlParams);
                
                string query;
                var parameters = new System.Collections.Generic.Dictionary<string, object>();
                
                if (dtrId == null)
                {
                    // Create new DTR record
                    query = $"INSERT INTO dtr_records (employee_id, dtr_date, {entryType.ToLower().Replace(" ", "_")}, status) VALUES (@employeeId, @dtrDate, TIME_FORMAT(@currentTime, '%H:%i'), 'Present')";
                    parameters.Add("@employeeId", employeeId);
                    parameters.Add("@dtrDate", currentDate);
                    parameters.Add("@currentTime", currentTime.ToString("HH:mm"));
                }
                else
                {
                    // Update existing DTR record with total hours calculation
                    query = $@"UPDATE dtr_records 
                        SET {entryType.ToLower().Replace(" ", "_")} = TIME_FORMAT(@currentTime, '%H:%i'),
                            total_hours = ROUND(TIMESTAMPDIFF(MINUTE, time_in, IFNULL(time_out, @currentTime)) / 60.0 - 
                                IFNULL(TIMESTAMPDIFF(MINUTE, break_out, break_in) / 60.0, 0), 2),
                            overtime_hours = ROUND(GREATEST(
                                TIMESTAMPDIFF(MINUTE, time_in, IFNULL(time_out, @currentTime)) / 60.0 - 
                                IFNULL(TIMESTAMPDIFF(MINUTE, break_out, break_in) / 60.0, 0) - 8, 0), 2)
                        WHERE dtr_id = @dtrId";
                    parameters.Add("@currentTime", currentTime.ToString("HH:mm"));
                    parameters.Add("@dtrId", dtrId);
                }
                
                MySqlParameter[] paramMysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int result = DatabaseManager.ExecuteNonQuery(query, paramMysqlParams);
                
                if (result > 0)
                {
                    lblStatus.Text = $"{entryType} recorded successfully for Employee {employeeId} at {currentTime:HH:mm:ss}";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    LoadDTRRecords();
                    txtQRData.Clear();
                }
                else
                {
                    lblStatus.Text = $"Failed to record {entryType}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error recording {entryType}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error recording {entryType}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void GenerateQRCode(string data)
        {
            try
            {
                var writer = new ZXing.Windows.Compatibility.BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = 300,
                        Height = 300,
                        Margin = 1,
                        ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M
                    }
                };
                
                var qrCodeImage = writer.Write(data);
                picQRCode.Image = qrCodeImage;
                
                lblStatus.Text = $"QR Code generated for: {data}";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating QR code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event Handlers
        private void CmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDTRRecords();
        }

        private void btnTimeIn_Click(object sender, EventArgs e)
        {
            RecordTimeEntry("Time In");
        }

        private void btnTimeOut_Click(object sender, EventArgs e)
        {
            RecordTimeEntry("Time Out");
        }

        private void btnBreakOut_Click(object sender, EventArgs e)
        {
            RecordTimeEntry("Break Out");
        }

        private void btnBreakIn_Click(object sender, EventArgs e)
        {
            RecordTimeEntry("Break In");
        }

        private void btnGenerateQR_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQRData.Text))
            {
                MessageBox.Show("Please enter employee ID to generate QR code.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            GenerateQRCode(txtQRData.Text.Trim());
        }

        private void btnScanQR_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                    Title = "Select QR Code Image"
                })
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var reader = new ZXing.Windows.Compatibility.BarcodeReader
                        {
                            Options = new DecodingOptions
                            {
                                PossibleFormats = new[] { BarcodeFormat.QR_CODE },
                                TryHarder = true
                            }
                        };

                        using (var bitmap = (Bitmap)Image.FromFile(dialog.FileName))
                        {
                            var result = reader.Decode(bitmap);
                            if (result != null)
                            {
                                txtQRData.Text = result.Text;
                                lblStatus.Text = "QR Code scanned successfully";
                                lblStatus.ForeColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                MessageBox.Show("No QR code found in the image.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error scanning QR code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.FileName = $"DTR_Report_{DateTime.Now:yyyyMMdd}.csv";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(saveDialog.FileName);
                    MessageBox.Show("DTR records exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Write headers
                string[] headers = new string[dgvDTR.Columns.Count];
                for (int i = 0; i < dgvDTR.Columns.Count; i++)
                {
                    headers[i] = dgvDTR.Columns[i].HeaderText;
                }
                writer.WriteLine(string.Join(",", headers));
                
                // Write data
                foreach (DataGridViewRow row in dgvDTR.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string[] values = new string[row.Cells.Count];
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            values[i] = row.Cells[i].Value?.ToString() ?? "";
                        }
                        writer.WriteLine(string.Join(",", values));
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            timerClock?.Stop();
            base.OnFormClosed(e);
        }
    }
}
