using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PayrollSystem
{
    public class frmQr : Form
    {
        private TabControl tabControl;
        private TabPage tabGenerate;
        private TabPage tabSettings;
        private TabPage tabHistory;
        
        // Generate Tab Controls
        private ComboBox cmbQrType;
        private ComboBox cmbEmployee;
        private TextBox txtCustomData;
        private PictureBox picQrCode;
        private Button btnGenerate;
        private Button btnSaveQr;
        private Button btnPrintQr;
        private GroupBox grpPreview;
        private Label lblQrInfo;
        
        // Settings Tab Controls
        private NumericUpDown nudQrSize;
        private ComboBox cmbErrorCorrection;
        private ComboBox cmbQrFormat;
        private TextBox txtQrPrefix;
        private CheckBox chkIncludeEmployeeId;
        private CheckBox chkIncludeName;
        private CheckBox chkIncludeDepartment;
        private CheckBox chkIncludeTimestamp;
        private Button btnSaveSettings;
        private Button btnResetSettings;
        
        // History Tab Controls
        private DataGridView dgvHistory;
        private TextBox txtSearchHistory;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Button btnSearchHistory;
        private Button btnDeleteHistory;
        private Button btnExportHistory;
        private Label lblTotalHistory;

        public frmQr()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void InitializeComponent()
        {
            this.Text = "QR Code Management";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(800, 600);

            // Create tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Create tabs
            tabGenerate = new TabPage("Generate QR Code");
            tabSettings = new TabPage("QR Settings");
            tabHistory = new TabPage("QR History");

            tabControl.TabPages.AddRange(new TabPage[] { tabGenerate, tabSettings, tabHistory });

            InitializeGenerateTab();
            InitializeSettingsTab();
            InitializeHistoryTab();

            this.Controls.Add(tabControl);
        }

        private void InitializeGenerateTab()
        {
            // Left panel for controls
            Panel pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "QR Code Generator",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(300, 30)
            };

            Label lblQrType = new Label
            {
                Text = "QR Code Type:",
                Location = new Point(20, 70),
                Size = new Size(100, 23)
            };

            cmbQrType = new ComboBox
            {
                Location = new Point(20, 95),
                Size = new Size(280, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbQrType.Items.AddRange(new[] { "Employee ID", "Employee Profile", "Attendance", "Payroll", "Custom Data" });
            cmbQrType.SelectedIndex = 0;
            cmbQrType.SelectedIndexChanged += CmbQrType_SelectedIndexChanged;

            Label lblEmployee = new Label
            {
                Text = "Select Employee:",
                Location = new Point(20, 130),
                Size = new Size(120, 23)
            };

            cmbEmployee = new ComboBox
            {
                Location = new Point(20, 155),
                Size = new Size(280, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbEmployee.SelectedIndexChanged += CmbEmployee_SelectedIndexChanged;

            Label lblCustomData = new Label
            {
                Text = "Custom Data:",
                Location = new Point(20, 190),
                Size = new Size(100, 23)
            };

            txtCustomData = new TextBox
            {
                Location = new Point(20, 215),
                Size = new Size(280, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Enabled = false
            };
            txtCustomData.SetPlaceholderText("Enter custom data for QR code...");

            btnGenerate = new Button
            {
                Text = "Generate QR Code",
                Location = new Point(20, 310),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnGenerate.Click += BtnGenerate_Click;

            btnSaveQr = new Button
            {
                Text = "Save QR",
                Location = new Point(160, 310),
                Size = new Size(70, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnSaveQr.Click += BtnSaveQr_Click;

            btnPrintQr = new Button
            {
                Text = "Print",
                Location = new Point(240, 310),
                Size = new Size(60, 35),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnPrintQr.Click += BtnPrintQr_Click;

            lblQrInfo = new Label
            {
                Text = "Generate a QR code to see information here.",
                Location = new Point(20, 360),
                Size = new Size(280, 60),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };

            pnlLeft.Controls.AddRange(new Control[] {
                lblTitle, lblQrType, cmbQrType, lblEmployee, cmbEmployee,
                lblCustomData, txtCustomData, btnGenerate, btnSaveQr, btnPrintQr, lblQrInfo
            });

            // Right panel for QR preview
            Panel pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            grpPreview = new GroupBox
            {
                Text = "QR Code Preview",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            picQrCode = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            grpPreview.Controls.Add(picQrCode);
            pnlRight.Controls.Add(grpPreview);

            tabGenerate.Controls.AddRange(new Control[] { pnlLeft, pnlRight });
        }

        private void InitializeSettingsTab()
        {
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            GroupBox grpQrProperties = new GroupBox
            {
                Text = "QR Code Properties",
                Location = new Point(30, 30),
                Size = new Size(400, 180),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblQrSize = new Label
            {
                Text = "QR Code Size (pixels):",
                Location = new Point(20, 35),
                Size = new Size(150, 23)
            };

            nudQrSize = new NumericUpDown
            {
                Location = new Point(180, 32),
                Size = new Size(100, 23),
                Minimum = 100,
                Maximum = 1000,
                Value = 300,
                Increment = 50
            };

            Label lblErrorCorrection = new Label
            {
                Text = "Error Correction Level:",
                Location = new Point(20, 70),
                Size = new Size(150, 23)
            };

            cmbErrorCorrection = new ComboBox
            {
                Location = new Point(180, 67),
                Size = new Size(150, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbErrorCorrection.Items.AddRange(new[] { "Low (7%)", "Medium (15%)", "Quartile (25%)", "High (30%)" });
            cmbErrorCorrection.SelectedIndex = 1;

            Label lblQrFormat = new Label
            {
                Text = "Output Format:",
                Location = new Point(20, 105),
                Size = new Size(150, 23)
            };

            cmbQrFormat = new ComboBox
            {
                Location = new Point(180, 102),
                Size = new Size(150, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbQrFormat.Items.AddRange(new[] { "PNG", "JPEG", "BMP", "GIF" });
            cmbQrFormat.SelectedIndex = 0;

            Label lblQrPrefix = new Label
            {
                Text = "QR Data Prefix:",
                Location = new Point(20, 140),
                Size = new Size(150, 23)
            };

            txtQrPrefix = new TextBox
            {
                Location = new Point(180, 137),
                Size = new Size(150, 23),
                Text = "PAYROLL_"
            };
            txtQrPrefix.SetPlaceholderText("e.g., COMPANY_");

            grpQrProperties.Controls.AddRange(new Control[] {
                lblQrSize, nudQrSize, lblErrorCorrection, cmbErrorCorrection,
                lblQrFormat, cmbQrFormat, lblQrPrefix, txtQrPrefix
            });

            GroupBox grpDataInclusion = new GroupBox
            {
                Text = "Data Inclusion Options",
                Location = new Point(450, 30),
                Size = new Size(350, 180),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            chkIncludeEmployeeId = new CheckBox
            {
                Text = "Include Employee ID",
                Location = new Point(20, 35),
                Size = new Size(200, 23),
                Checked = true
            };

            chkIncludeName = new CheckBox
            {
                Text = "Include Employee Name",
                Location = new Point(20, 65),
                Size = new Size(200, 23),
                Checked = true
            };

            chkIncludeDepartment = new CheckBox
            {
                Text = "Include Department",
                Location = new Point(20, 95),
                Size = new Size(200, 23),
                Checked = false
            };

            chkIncludeTimestamp = new CheckBox
            {
                Text = "Include Generation Timestamp",
                Location = new Point(20, 125),
                Size = new Size(250, 23),
                Checked = true
            };

            grpDataInclusion.Controls.AddRange(new Control[] {
                chkIncludeEmployeeId, chkIncludeName, chkIncludeDepartment, chkIncludeTimestamp
            });

            // Buttons
            btnSaveSettings = new Button
            {
                Text = "Save Settings",
                Location = new Point(450, 240),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSaveSettings.Click += BtnSaveSettings_Click;

            btnResetSettings = new Button
            {
                Text = "Reset to Default",
                Location = new Point(580, 240),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnResetSettings.Click += BtnResetSettings_Click;

            pnlMain.Controls.AddRange(new Control[] { grpQrProperties, grpDataInclusion, btnSaveSettings, btnResetSettings });
            tabSettings.Controls.Add(pnlMain);
        }

        private void InitializeHistoryTab()
        {
            // Top panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 25),
                Size = new Size(60, 23)
            };

            txtSearchHistory = new TextBox
            {
                Location = new Point(85, 22),
                Size = new Size(150, 23)
            };
            txtSearchHistory.SetPlaceholderText("Employee name...");

            Label lblFromDate = new Label
            {
                Text = "From:",
                Location = new Point(250, 25),
                Size = new Size(40, 23)
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(295, 22),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(-1)
            };

            Label lblToDate = new Label
            {
                Text = "To:",
                Location = new Point(430, 25),
                Size = new Size(30, 23)
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(465, 22),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };

            btnSearchHistory = new Button
            {
                Text = "Search",
                Location = new Point(600, 20),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearchHistory.Click += BtnSearchHistory_Click;

            btnDeleteHistory = new Button
            {
                Text = "Delete",
                Location = new Point(680, 20),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteHistory.Click += BtnDeleteHistory_Click;

            btnExportHistory = new Button
            {
                Text = "Export",
                Location = new Point(760, 20),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportHistory.Click += BtnExportHistory_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblSearch, txtSearchHistory, lblFromDate, dtpFromDate, lblToDate, dtpToDate,
                btnSearchHistory, btnDeleteHistory, btnExportHistory
            });

            // DataGridView for history
            dgvHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvHistory.CellDoubleClick += DgvHistory_CellDoubleClick;

            // Bottom panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalHistory = new Label
            {
                Text = "Total Records: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            pnlBottom.Controls.Add(lblTotalHistory);

            tabHistory.Controls.AddRange(new Control[] { pnlTop, dgvHistory, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvHistory);
        }

        private void LoadInitialData()
        {
            LoadEmployees();
            LoadQrSettings();
            LoadQrHistory();
        }

        private void LoadEmployees()
        {
            try
            {
                string query = @"
                    SELECT 
                        id,
                        CONCAT(employee_id, ' - ', first_name, ' ', last_name) as display_name
                    FROM tbl_employee 
                    WHERE status = 'Active'
                    ORDER BY first_name, last_name";

                DataTable dt = UtilityHelper.GetDataSet(query);
                
                cmbEmployee.Items.Clear();
                cmbEmployee.Items.Add(new { id = 0, display_name = "-- Select Employee --" });
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbEmployee.Items.Add(new { 
                        id = Convert.ToInt32(row["id"]), 
                        display_name = row["display_name"].ToString() 
                    });
                }
                
                cmbEmployee.DisplayMember = "display_name";
                cmbEmployee.ValueMember = "id";
                cmbEmployee.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadQrSettings()
        {
            try
            {
                string query = "SELECT * FROM tbl_qr_settings WHERE id = 1";
                DataTable dt = UtilityHelper.GetDataSet(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    nudQrSize.Value = Convert.ToDecimal(row["qr_size"]);
                    cmbErrorCorrection.SelectedIndex = Convert.ToInt32(row["error_correction_level"]);
                    cmbQrFormat.SelectedIndex = Convert.ToInt32(row["output_format"]);
                    txtQrPrefix.Text = row["data_prefix"].ToString();
                    chkIncludeEmployeeId.Checked = Convert.ToBoolean(row["include_employee_id"]);
                    chkIncludeName.Checked = Convert.ToBoolean(row["include_name"]);
                    chkIncludeDepartment.Checked = Convert.ToBoolean(row["include_department"]);
                    chkIncludeTimestamp.Checked = Convert.ToBoolean(row["include_timestamp"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading QR settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadQrHistory()
        {
            try
            {
                string query = @"
                    SELECT 
                        qh.id,
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        e.employee_id as 'Employee ID',
                        qh.qr_type as 'QR Type',
                        qh.qr_data as 'QR Data',
                        qh.file_path as 'File Path',
                        qh.created_date as 'Generated Date',
                        qh.created_by as 'Generated By'
                    FROM tbl_qr_history qh
                    LEFT JOIN tbl_employee e ON qh.employee_id = e.id
                    WHERE qh.created_date >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                    ORDER BY qh.created_date DESC";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvHistory.DataSource = dt;

                if (dgvHistory.Columns["id"] != null)
                    dgvHistory.Columns["id"].Visible = false;
                if (dgvHistory.Columns["File Path"] != null)
                    dgvHistory.Columns["File Path"].Visible = false;

                // Format date column
                if (dgvHistory.Columns["Generated Date"] != null)
                {
                    dgvHistory.Columns["Generated Date"].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm";
                }

                UpdateHistoryCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading QR history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateHistoryCount()
        {
            lblTotalHistory.Text = $"Total Records: {dgvHistory.Rows.Count:N0}";
        }

        private string GenerateQrData()
        {
            try
            {
                var selectedEmployee = cmbEmployee.SelectedItem;
                if (selectedEmployee == null || (int)selectedEmployee.GetType().GetProperty("id").GetValue(selectedEmployee) == 0)
                {
                    if (cmbQrType.SelectedItem.ToString() != "Custom Data")
                    {
                        MessageBox.Show("Please select an employee.", "No Employee Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                }

                StringBuilder qrData = new StringBuilder();
                qrData.Append(txtQrPrefix.Text);

                switch (cmbQrType.SelectedItem.ToString())
                {
                    case "Employee ID":
                        qrData.Append(GetEmployeeData("employee_id"));
                        break;
                    case "Employee Profile":
                        qrData.Append(GetEmployeeProfileData());
                        break;
                    case "Attendance":
                        qrData.Append($"ATT_{GetEmployeeData("employee_id")}_{DateTime.Now:yyyyMMddHHmmss}");
                        break;
                    case "Payroll":
                        qrData.Append($"PAY_{GetEmployeeData("employee_id")}_{DateTime.Now:yyyyMM}");
                        break;
                    case "Custom Data":
                        qrData.Append(txtCustomData.Text);
                        break;
                }

                if (chkIncludeTimestamp.Checked && cmbQrType.SelectedItem.ToString() != "Custom Data")
                {
                    qrData.Append($"_TS_{DateTime.Now:yyyyMMddHHmmss}");
                }

                return qrData.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating QR data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private string GetEmployeeData(string field)
        {
            try
            {
                var selectedEmployee = cmbEmployee.SelectedItem;
                int employeeId = (int)selectedEmployee.GetType().GetProperty("id").GetValue(selectedEmployee);
                
                string query = $"SELECT {field} FROM tbl_employee WHERE id = {employeeId}";
                return UtilityHelper.GetScalar(query)?.ToString() ?? "";
            }
            catch
            {
                return "";
            }
        }

        private string GetEmployeeProfileData()
        {
            try
            {
                var selectedEmployee = cmbEmployee.SelectedItem;
                int employeeId = (int)selectedEmployee.GetType().GetProperty("id").GetValue(selectedEmployee);
                
                string query = @"
                    SELECT 
                        CONCAT(employee_id, '|', first_name, '|', last_name, '|', 
                               IFNULL(department, ''), '|', IFNULL(position, '')) as profile_data
                    FROM tbl_employee 
                    WHERE id = " + employeeId;
                
                return UtilityHelper.GetScalar(query)?.ToString() ?? "";
            }
            catch
            {
                return "";
            }
        }

        // Event handlers
        private void CmbQrType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isCustomData = cmbQrType.SelectedItem.ToString() == "Custom Data";
            txtCustomData.Enabled = isCustomData;
            cmbEmployee.Enabled = !isCustomData;
            
            if (isCustomData)
            {
                txtCustomData.Focus();
            }
        }

        private void CmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update QR info when employee changes
            if (cmbEmployee.SelectedIndex > 0)
            {
                var selectedEmployee = cmbEmployee.SelectedItem;
                string displayName = selectedEmployee.GetType().GetProperty("display_name").GetValue(selectedEmployee).ToString();
                lblQrInfo.Text = $"Selected: {displayName}\nClick Generate to create QR code.";
            }
            else
            {
                lblQrInfo.Text = "Select an employee to generate QR code.";
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string qrData = GenerateQrData();
                if (string.IsNullOrEmpty(qrData))
                    return;

                // Simulate QR code generation (in real implementation, use a QR library like QRCoder)
                Bitmap qrImage = GenerateQrCodeImage(qrData);
                picQrCode.Image = qrImage;
                
                lblQrInfo.Text = $"QR Code Generated\nData: {qrData}\nSize: {nudQrSize.Value}x{nudQrSize.Value} pixels";
                
                btnSaveQr.Enabled = true;
                btnPrintQr.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating QR code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Bitmap GenerateQrCodeImage(string data)
        {
            // This is a placeholder implementation
            // In a real application, you would use a QR code library like QRCoder
            int size = (int)nudQrSize.Value;
            Bitmap bitmap = new Bitmap(size, size);
            
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                g.DrawRectangle(Pens.Black, 0, 0, size - 1, size - 1);
                
                // Draw a simple pattern to represent QR code
                Random rand = new Random(data.GetHashCode());
                int blockSize = size / 25;
                
                for (int x = 0; x < 25; x++)
                {
                    for (int y = 0; y < 25; y++)
                    {
                        if (rand.Next(2) == 1)
                        {
                            g.FillRectangle(Brushes.Black, x * blockSize, y * blockSize, blockSize, blockSize);
                        }
                    }
                }
                
                // Draw corner markers
                g.FillRectangle(Brushes.Black, 0, 0, blockSize * 7, blockSize * 7);
                g.FillRectangle(Brushes.White, blockSize, blockSize, blockSize * 5, blockSize * 5);
                g.FillRectangle(Brushes.Black, blockSize * 2, blockSize * 2, blockSize * 3, blockSize * 3);
            }
            
            return bitmap;
        }

        private void BtnSaveQr_Click(object sender, EventArgs e)
        {
            if (picQrCode.Image == null)
            {
                MessageBox.Show("Please generate a QR code first.", "No QR Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = $"{cmbQrFormat.SelectedItem} files (*.{cmbQrFormat.SelectedItem.ToString().ToLower()})|*.{cmbQrFormat.SelectedItem.ToString().ToLower()}";
                    sfd.FileName = $"QRCode_{DateTime.Now:yyyyMMdd_HHmmss}.{cmbQrFormat.SelectedItem.ToString().ToLower()}";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        ImageFormat format = ImageFormat.Png;
                        switch (cmbQrFormat.SelectedItem.ToString())
                        {
                            case "JPEG": format = ImageFormat.Jpeg; break;
                            case "BMP": format = ImageFormat.Bmp; break;
                            case "GIF": format = ImageFormat.Gif; break;
                        }
                        
                        picQrCode.Image.Save(sfd.FileName, format);
                        MessageBox.Show($"QR code saved successfully to {sfd.FileName}", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Save to history
                        SaveQrHistory(sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving QR code: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveQrHistory(string filePath)
        {
            try
            {
                var selectedEmployee = cmbEmployee.SelectedItem;
                int employeeId = 0;
                
                if (selectedEmployee != null)
                {
                    employeeId = (int)selectedEmployee.GetType().GetProperty("id").GetValue(selectedEmployee);
                }
                
                string qrData = GenerateQrData();
                string qrType = cmbQrType.SelectedItem.ToString();
                
                string query = $@"
                    INSERT INTO tbl_qr_history (employee_id, qr_type, qr_data, file_path, created_date, created_by)
                    VALUES ({(employeeId > 0 ? employeeId.ToString() : "NULL")}, '{qrType}', '{qrData}', '{filePath}', NOW(), '{GlobalVariables.Username}')";
                
                DatabaseManager.ExecuteNonQuery(query);
                
                LoadQrHistory(); // Refresh history
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving QR history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrintQr_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Print QR Code functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                string query = $@"
                    INSERT INTO tbl_qr_settings (id, qr_size, error_correction_level, output_format, data_prefix, 
                                                 include_employee_id, include_name, include_department, include_timestamp, modified_date)
                    VALUES (1, {nudQrSize.Value}, {cmbErrorCorrection.SelectedIndex}, {cmbQrFormat.SelectedIndex}, '{txtQrPrefix.Text}',
                            {(chkIncludeEmployeeId.Checked ? 1 : 0)}, {(chkIncludeName.Checked ? 1 : 0)}, 
                            {(chkIncludeDepartment.Checked ? 1 : 0)}, {(chkIncludeTimestamp.Checked ? 1 : 0)}, NOW())
                    ON DUPLICATE KEY UPDATE
                    qr_size = {nudQrSize.Value},
                    error_correction_level = {cmbErrorCorrection.SelectedIndex},
                    output_format = {cmbQrFormat.SelectedIndex},
                    data_prefix = '{txtQrPrefix.Text}',
                    include_employee_id = {(chkIncludeEmployeeId.Checked ? 1 : 0)},
                    include_name = {(chkIncludeName.Checked ? 1 : 0)},
                    include_department = {(chkIncludeDepartment.Checked ? 1 : 0)},
                    include_timestamp = {(chkIncludeTimestamp.Checked ? 1 : 0)},
                    modified_date = NOW()";

                DatabaseManager.ExecuteNonQuery(query);

                MessageBox.Show("QR settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset to default settings?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                nudQrSize.Value = 300;
                cmbErrorCorrection.SelectedIndex = 1;
                cmbQrFormat.SelectedIndex = 0;
                txtQrPrefix.Text = "PAYROLL_";
                chkIncludeEmployeeId.Checked = true;
                chkIncludeName.Checked = true;
                chkIncludeDepartment.Checked = false;
                chkIncludeTimestamp.Checked = true;
            }
        }

        private void BtnSearchHistory_Click(object sender, EventArgs e)
        {
            LoadQrHistory(); // Simplified - would include search filters in real implementation
        }

        private void BtnDeleteHistory_Click(object sender, EventArgs e)
        {
            if (dgvHistory.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this QR history record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Delete QR History functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnExportHistory_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv";
                    sfd.FileName = $"QRHistory_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder csv = new StringBuilder();

                        // Add headers
                        var headers = new List<string>();
                        for (int i = 1; i < dgvHistory.Columns.Count; i++) // Skip ID column
                        {
                            if (dgvHistory.Columns[i].Visible)
                                headers.Add(dgvHistory.Columns[i].HeaderText);
                        }
                        csv.AppendLine(string.Join(",", headers));

                        // Add data
                        foreach (DataGridViewRow row in dgvHistory.Rows)
                        {
                            var values = new List<string>();
                            for (int i = 1; i < dgvHistory.Columns.Count; i++) // Skip ID column
                            {
                                if (dgvHistory.Columns[i].Visible)
                                {
                                    string value = row.Cells[i].Value?.ToString() ?? "";
                                    values.Add($"\"{value}\"");
                                }
                            }
                            csv.AppendLine(string.Join(",", values));
                        }

                        File.WriteAllText(sfd.FileName, csv.ToString());
                        MessageBox.Show($"Data exported successfully to {sfd.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MessageBox.Show("View QR History Details functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
