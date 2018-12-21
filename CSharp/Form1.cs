using MetroFramework.Forms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CSharp
{
	public class Form1 : MetroForm
    {
		public delegate int NotifyCallBack(int wParam, int lParam);

		private const string LICENSEID = "{00000000-0000-0000-0000-000000000000}";

		private const int MOD_ALT = 1;

		private const int MOD_CONTROL = 2;

		private const int MOD_SHIFT = 4;

		private const int MOD_WIN = 8;

		private const int VK_LBUTTON = 1;

		private const int VK_RBUTTON = 2;

		private const int VK_MBUTTON = 4;

		private const int WM_LBUTTONDBLCLK = 515;

		private const int SW_HIDE = 0;

		private const int SW_SHOW = 5;

		private bool bGetWordUnloaded;

		private NotifyCallBack callbackHighlightReady;

		private NotifyCallBack callbackMouseMonitor;

		private IContainer components;

		private GroupBox groupBox3;

		private TextBox textHighlight;
        private Button button1;
        private CheckBox checkHighlight;

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out Point lpPoint);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr WindowFromPoint(Point point);

		[DllImport("kernel32.dll")]
		public static extern void Sleep(int uMilliSec);

		[DllImport("kernel32.dll")]
		public static extern int FreeLibrary(int hLibModule);

		[DllImport("kernel32.dll")]
		public static extern int GetModuleHandle(string lpModuleName);

		[DllImport("GetWord.dll")]
		public static extern void SetLicenseID([MarshalAs(UnmanagedType.BStr)] string szLicense);

		[DllImport("GetWord.dll")]
		public static extern void SetNotifyWnd(int hWndNotify);

		[DllImport("GetWord.dll")]
		public static extern void UnSetNotifyWnd(int hWndNotify);

		[DllImport("GetWord.dll")]
		public static extern void SetDelay(int uMilliSec);

		[DllImport("GetWord.dll")]
		public static extern bool EnableCursorCapture(bool bEnable);

		[DllImport("GetWord.dll")]
		public static extern bool EnableHotkeyCapture(bool bEnable, int fsModifiers, int vKey);

		[DllImport("GetWord.dll")]
		public static extern bool EnableHighlightCapture(bool bEnable);

		[DllImport("GetWord.dll")]
		public static extern bool GetString(int x, int y, [MarshalAs(UnmanagedType.BStr)] out string str, out int nCursorPos);

		[DllImport("GetWord.dll")]
		public static extern bool GetString2(int x, int y, [MarshalAs(UnmanagedType.BStr)] out string str, out int nCursorPos, out int left, out int top, out int right, out int bottom);

		[DllImport("GetWord.dll")]
		public static extern bool FreeString([MarshalAs(UnmanagedType.BStr)] out string str);

		[DllImport("GetWord.dll")]
		public static extern bool GetRectString(int hWnd, int left, int top, int right, int bottom, [MarshalAs(UnmanagedType.BStr)] out string str);

		[DllImport("GetWord.dll")]
		public static extern int GetRectStringPairs(int hWnd, int left, int top, int right, int bottom, [MarshalAs(UnmanagedType.BStr)] out string str, [MarshalAs(UnmanagedType.BStr)] out string rectList);

		[DllImport("GetWord.dll")]
		public static extern int GetPointStringPairs(int x, int y, [MarshalAs(UnmanagedType.BStr)] out string str, [MarshalAs(UnmanagedType.BStr)] out string rectList);

		[DllImport("GetWord.dll")]
		public static extern bool FreePairs([MarshalAs(UnmanagedType.BStr)] out string str, [MarshalAs(UnmanagedType.BStr)] out string rectList);

		[DllImport("GetWord.dll")]
		public static extern bool GetPairItem(int totalCount, [MarshalAs(UnmanagedType.BStr)] out string str, [MarshalAs(UnmanagedType.BStr)] out string rectList, int index, [MarshalAs(UnmanagedType.BStr)] out string substr, out int substrLen, out int substrLeft, out int substrTop, out int substrRight, out int substrBottom);

		[DllImport("GetWord.dll")]
		public static extern bool GetHighlightText(int hWnd, [MarshalAs(UnmanagedType.BStr)] out string str);

		[DllImport("GetWord.dll")]
		public static extern bool GetHighlightText2(int x, int y, [MarshalAs(UnmanagedType.BStr)] out string str);

		[DllImport("GetWord.dll")]
		public static extern bool SetCaptureReadyCallback(NotifyCallBack callback);

		[DllImport("GetWord.dll")]
		public static extern bool RemoveCaptureReadyCallback(NotifyCallBack callback);

		[DllImport("GetWord.dll")]
		public static extern bool SetHighlightReadyCallback(NotifyCallBack callback);

		[DllImport("GetWord.dll")]
		public static extern bool RemoveHighlightReadyCallback(NotifyCallBack callback);

		[DllImport("GetWord.dll")]
		public static extern bool SetMouseMonitorCallback(NotifyCallBack callback);

		[DllImport("GetWord.dll")]
		public static extern bool RemoveMouseMonitorCallback(NotifyCallBack callback);

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			SetLicenseID("{00000000-0000-0000-0000-000000000000}");
			SetDelay(300);
			callbackHighlightReady = OnHighlightReady;
			SetHighlightReadyCallback(callbackHighlightReady);
			callbackMouseMonitor = OnMouseMonitor;
			SetMouseMonitorCallback(callbackMouseMonitor);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
            Debug.WriteLine("Form1_FormClosing");
            bGetWordUnloaded = true;
            RemoveHighlightReadyCallback(callbackHighlightReady);
            RemoveMouseMonitorCallback(callbackMouseMonitor);
        }

		private void checkHighlight_CheckedChanged(object sender, EventArgs e)
		{
			bool flag = false;
			flag = (checkHighlight.Checked ? true : false);
			EnableHighlightCapture(flag);
		}

		private int OnHighlightReady(int wParam, int lParam)
		{
			if (bGetWordUnloaded)
			{
				return 1;
			}
			int result = 0;
			if (bGetWordUnloaded)
			{
				return result;
			}
			int x = wParam & 0xFFFF;
			int y = (int)((wParam & 4294901760u) >> 16);
			string str = "";
			if (GetHighlightText2(x, y, out str))
			{
				textHighlight.Text = str;
				FreeString(out str);
				return 0;
			}
			textHighlight.Text = "";
			return 1;
		}

		private int OnMouseMonitor(int wParam, int lParam)
		{
			if (bGetWordUnloaded)
			{
				return 1;
			}
			int result = 0;
			if (bGetWordUnloaded)
			{
				return result;
			}
			if (wParam == 515)
			{
				result = OnHighlightReady(wParam, lParam);
			}
			return result;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			this.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textHighlight = new System.Windows.Forms.TextBox();
            this.checkHighlight = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textHighlight);
            this.groupBox3.Controls.Add(this.checkHighlight);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(20, 60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(439, 423);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "捕获鼠标选择的文本";
            // 
            // textHighlight
            // 
            this.textHighlight.AcceptsReturn = true;
            this.textHighlight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textHighlight.Location = new System.Drawing.Point(3, 17);
            this.textHighlight.Multiline = true;
            this.textHighlight.Name = "textHighlight";
            this.textHighlight.ReadOnly = true;
            this.textHighlight.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textHighlight.Size = new System.Drawing.Size(433, 403);
            this.textHighlight.TabIndex = 1;
            this.textHighlight.WordWrap = false;
            // 
            // checkHighlight
            // 
            this.checkHighlight.AutoSize = true;
            this.checkHighlight.Location = new System.Drawing.Point(6, 0);
            this.checkHighlight.Name = "checkHighlight";
            this.checkHighlight.Size = new System.Drawing.Size(186, 16);
            this.checkHighlight.TabIndex = 0;
            this.checkHighlight.Text = "启用 鼠标选择的文本捕获功能";
            this.checkHighlight.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(381, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(479, 503);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "屏幕取词";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
