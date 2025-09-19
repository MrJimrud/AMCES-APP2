using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PayrollSystem
{
    public partial class frmWait : Form
    {
        private ProgressBar progressBar;
        private Label lblMessage;
        private Label lblProgress;
        private Button btnCancel;
        private Panel pnlMain;
        private CancellationTokenSource cancellationTokenSource;
        private bool allowCancel;
        private string operationMessage;
        private int progressValue;
        private int maxValue;

        public frmWait()
        {
            InitializeComponent();
            InitializeForm();
        }

        public frmWait(string message, bool allowCancel = false) : this()
        {
            this.operationMessage = message;
            this.allowCancel = allowCancel;
            lblMessage.Text = message;
            btnCancel.Visible = allowCancel;
        }

        private void InitializeComponent()
        {
            this.pnlMain = new Panel();
            this.lblMessage = new Label();
            this.progressBar = new ProgressBar();
            this.lblProgress = new Label();
            this.btnCancel = new Button();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();

            // pnlMain
            this.pnlMain.BackColor = Color.White;
            this.pnlMain.BorderStyle = BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.btnCancel);
            this.pnlMain.Controls.Add(this.lblProgress);
            this.pnlMain.Controls.Add(this.progressBar);
            this.pnlMain.Controls.Add(this.lblMessage);
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.Location = new Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new Size(400, 150);
            this.pnlMain.TabIndex = 0;

            // lblMessage
            this.lblMessage.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblMessage.Location = new Point(20, 20);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(360, 23);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Please wait...";
            this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;

            // progressBar
            this.progressBar.Location = new Point(20, 55);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(360, 23);
            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 1;
            this.progressBar.MarqueeAnimationSpeed = 30;

            // lblProgress
            this.lblProgress.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblProgress.Location = new Point(20, 85);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new Size(360, 20);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "";
            this.lblProgress.TextAlign = ContentAlignment.MiddleCenter;

            // btnCancel
            this.btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(160, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // frmWait
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 150);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "frmWait";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Please Wait";
            this.TopMost = true;
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void InitializeForm()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.progressValue = 0;
            this.maxValue = 100;
            
            // Set form properties
            this.ShowInTaskbar = false;
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        public CancellationToken CancellationToken
        {
            get { return cancellationTokenSource.Token; }
        }

        public bool IsCancelled
        {
            get { return cancellationTokenSource.IsCancellationRequested; }
        }

        public void SetMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(SetMessage), message);
                return;
            }

            lblMessage.Text = message;
            this.operationMessage = message;
        }

        public void SetProgress(int value, int maximum = 100)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int, int>(SetProgress), value, maximum);
                return;
            }

            this.progressValue = value;
            this.maxValue = maximum;

            if (progressBar.Style != ProgressBarStyle.Continuous)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
            }

            progressBar.Maximum = maximum;
            progressBar.Value = Math.Min(value, maximum);
            
            lblProgress.Text = $"{value} / {maximum} ({(value * 100 / maximum):F0}%)";
        }

        public void SetProgressText(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(SetProgressText), text);
                return;
            }

            lblProgress.Text = text;
        }

        public void SetMarqueeMode(bool marquee = true)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(SetMarqueeMode), marquee);
                return;
            }

            progressBar.Style = marquee ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            
            if (marquee)
            {
                lblProgress.Text = "";
            }
        }

        public void SetCancelVisible(bool visible)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(SetCancelVisible), visible);
                return;
            }

            btnCancel.Visible = visible;
            this.allowCancel = visible;
        }

        public void CompleteOperation()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(CompleteOperation));
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void CancelOperation()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(CancelOperation));
                return;
            }

            cancellationTokenSource.Cancel();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (allowCancel)
            {
                CancelOperation();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!allowCancel && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                return;
            }

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }

            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cancellationTokenSource?.Dispose();
            }
            base.Dispose(disposing);
        }

        // Static helper methods for common usage patterns
        public static frmWait ShowWait(string message, bool allowCancel = false)
        {
            var waitForm = new frmWait(message, allowCancel);
            waitForm.Show();
            Application.DoEvents();
            return waitForm;
        }

        public static frmWait ShowWaitDialog(IWin32Window owner, string message, bool allowCancel = false)
        {
            var waitForm = new frmWait(message, allowCancel);
            if (owner != null)
            {
                waitForm.StartPosition = FormStartPosition.CenterParent;
            }
            waitForm.Show(owner);
            Application.DoEvents();
            return waitForm;
        }

        public static async Task<T> ExecuteWithWait<T>(Func<CancellationToken, Task<T>> operation, string message, bool allowCancel = false, IWin32Window owner = null)
        {
            var waitForm = owner != null ? ShowWaitDialog(owner, message, allowCancel) : ShowWait(message, allowCancel);
            
            try
            {
                var result = await operation(waitForm.CancellationToken);
                waitForm.CompleteOperation();
                return result;
            }
            catch (OperationCanceledException)
            {
                waitForm.CancelOperation();
                throw;
            }
            catch (Exception)
            {
                waitForm.Close();
                throw;
            }
        }

        public static async Task ExecuteWithWait(Func<CancellationToken, Task> operation, string message, bool allowCancel = false, IWin32Window owner = null)
        {
            var waitForm = owner != null ? ShowWaitDialog(owner, message, allowCancel) : ShowWait(message, allowCancel);
            
            try
            {
                await operation(waitForm.CancellationToken);
                waitForm.CompleteOperation();
            }
            catch (OperationCanceledException)
            {
                waitForm.CancelOperation();
                throw;
            }
            catch (Exception)
            {
                waitForm.Close();
                throw;
            }
        }
    }
}
