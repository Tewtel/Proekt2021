// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.SetForm
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  public class SetForm : Form
  {
    private bool numUpdownWarning = false;
    private IniFile INI = new IniFile("Config.ini");
    private RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
    private Form1 mainForm;
    private IContainer components = (IContainer) null;
    public Label label12;
    public TextBox TgrammTokenTextBox;
    public CheckBox BrowserOpenChekBox;
    public CheckBox AutoStartWindowsChekBox;
    public CheckBox BeepCheckBox;
    public Button SabeButton;
    public Label label1;
    public Label label9;
    public CheckBox SearchNowCheckBox;
    public ToolTip toolTip1;
    public CheckBox TelegrammCheckBox;
    public TextBox TgrammKeyTextBox;
    public TextBox TgrammChatTextBox;
    public Label label10;
    public Label label11;
    public Button TgrammKeyChatWhantToNowButton;
    public Panel panel1;
    private TextBox TIntervalTextBox;
    public Label label13;
    private NumericUpDown numericUpDown1;
    public Label label2;
    private OpenFileDialog openFileDialog1;
    private TextBox ActKeyTextBox;
    private Label label3;
    private Label label14;
    private TextBox UcodeTextBox;
    public CheckBox logCheckBox1;
    public CheckBox notifyCheckBox;
    private Label label5;
    private TextBox ProxyLogin;
    private TextBox ProxyPass;
    private Panel panel2;
    private Label label8;
    private Label label6;
    private TextBox ProxyPort;
    private Label label7;
    private TextBox ProxyIP;
    private Label label4;
    private Panel panel3;
    private Label label16;
    private TextBox PortDynamic;
    private Label label17;
    private TextBox IpDynamic;
    private Label label18;
    private TextBox PassDynamic;
    private Label label19;
    private TextBox LoginDynamic;
    private CheckBox UseDynamicProxyCheckBox;
    private CheckBox RaiseAdvertsCheck;
    private CheckBox MagazCheck;
    public CheckBox AvitoDostavkaCheckBox;
    public Button button1;
    public Button button2;

    public SetForm(Form1 fm)
    {
      this.InitializeComponent();
      this.mainForm = fm;
      this.auto_read_settings();
      if (this.rkApp.GetValue("AvitoAvtoRinger") == null)
        this.AutoStartWindowsChekBox.Checked = false;
      else
        this.AutoStartWindowsChekBox.Checked = true;
    }

    private void TokenButton_Click(object sender, EventArgs e) => VKWorker.TokenGet();

    private void SabeButton_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Сохранить настройки?", "Вы уверены?", MessageBoxButtons.OKCancel) == DialogResult.OK)
      {
        IniFile ini1 = this.INI;
        CheckState checkState = this.BrowserOpenChekBox.CheckState;
        string str1 = checkState.ToString();
        ini1.WriteINI("CHECKBOX", "BrowserOpen", str1);
        IniFile ini2 = this.INI;
        checkState = this.BeepCheckBox.CheckState;
        string str2 = checkState.ToString();
        ini2.WriteINI("CHECKBOX", "Beep", str2);
        IniFile ini3 = this.INI;
        checkState = this.MagazCheck.CheckState;
        string str3 = checkState.ToString();
        ini3.WriteINI("CHECKBOX", "MagazCheck", str3);
        IniFile ini4 = this.INI;
        checkState = this.SearchNowCheckBox.CheckState;
        string str4 = checkState.ToString();
        ini4.WriteINI("CHECKBOX", "SearchNow", str4);
        IniFile ini5 = this.INI;
        checkState = this.TelegrammCheckBox.CheckState;
        string str5 = checkState.ToString();
        ini5.WriteINI("CHECKBOX", "Telegramm", str5);
        IniFile ini6 = this.INI;
        checkState = this.notifyCheckBox.CheckState;
        string str6 = checkState.ToString();
        ini6.WriteINI("CHECKBOX", "Notify", str6);
        IniFile ini7 = this.INI;
        checkState = this.UseDynamicProxyCheckBox.CheckState;
        string str7 = checkState.ToString();
        ini7.WriteINI("CHECKBOX", "dynamic", str7);
        IniFile ini8 = this.INI;
        checkState = this.RaiseAdvertsCheck.CheckState;
        string str8 = checkState.ToString();
        ini8.WriteINI("CHECKBOX", "RaiseAdverts", str8);
        IniFile ini9 = this.INI;
        checkState = this.AvitoDostavkaCheckBox.CheckState;
        string str9 = checkState.ToString();
        ini9.WriteINI("CHECKBOX", "AvitoDostavka", str9);
        this.INI.WriteINI("TELEGRAMM", "chat", this.TgrammChatTextBox.Text);
        this.INI.WriteINI("TELEGRAMM", "key", this.TgrammKeyTextBox.Text);
        this.INI.WriteINI("TELEGRAMM", "Token", this.TgrammTokenTextBox.Text);
        this.INI.WriteINI("SETCLASS", "TIntervalTime", this.numericUpDown1.Value.ToString());
        this.INI.WriteINI("USERPROXY", "login", this.ProxyLogin.Text.Trim());
        this.INI.WriteINI("USERPROXY", "pass", this.ProxyPass.Text.Trim());
        this.INI.WriteINI("USERPROXY", "ip", this.ProxyIP.Text.Trim());
        this.INI.WriteINI("USERPROXY", "port", this.ProxyPort.Text.Trim());
        this.INI.WriteINI("USERPROXY", "loginDynamic", this.LoginDynamic.Text.Trim());
        this.INI.WriteINI("USERPROXY", "passDynamic", this.PassDynamic.Text.Trim());
        this.INI.WriteINI("USERPROXY", "ipDynamic", this.IpDynamic.Text.Trim());
        this.INI.WriteINI("USERPROXY", "portDynamic", this.PortDynamic.Text.Trim());
        if (this.ProxyIP.Text != "" && this.ProxyPort.Text != "")
        {
          StatSetClass.proxyLogin = this.ProxyLogin.Text.Trim();
          StatSetClass.proxyPassword = this.ProxyPass.Text.Trim();
          StatSetClass.proxyIp = this.ProxyIP.Text.Trim();
          StatSetClass.proxyPort = this.ProxyPort.Text.Trim();
        }
        StatSetClass.proxyIpDynamic = this.IpDynamic.Text.Trim();
        StatSetClass.proxyPortDynamic = this.PortDynamic.Text.Trim();
        StatSetClass.proxyLoginDynamic = this.LoginDynamic.Text.Trim();
        StatSetClass.proxyPasswordDynamic = this.PassDynamic.Text.Trim();
      }
      else if (StatSetClass.r)
        new Thread(new ThreadStart(this.mainForm.CheckUpdates)).Start();
      StatSetClass.TimerInterval = (int) this.numericUpDown1.Value;
      this.INI.WriteINI("SETCLASS", "TInterval", this.TIntervalTextBox.Text);
      StatSetClass.timeOut = int.Parse(this.TIntervalTextBox.Text.Trim());
      if (!this.INI.KeyExists("ACTIVATION", StatSetClass.ACTkey))
        return;
      if (this.INI.ReadINI("ACTIVATION", StatSetClass.ACTkey) != this.ActKeyTextBox.Text && MessageBox.Show("Вы сменили ключ активации, теперь нужно перезапустить программу, чтобы изменения вступили в силу", "Предупреждение о смене ключа", MessageBoxButtons.OKCancel) == DialogResult.OK)
        this.INI.WriteINI("ACTIVATION", StatSetClass.ACTkey, this.ActKeyTextBox.Text);
      if (this.INI.KeyExists("TELEGRAMM", "key"))
        this.mainForm.tgKey = long.Parse(this.INI.ReadINI("TELEGRAMM", "key"));
      if (this.INI.KeyExists("TELEGRAMM", "chat"))
        this.mainForm.tgChat = int.Parse(this.INI.ReadINI("TELEGRAMM", "chat"));
      if (this.INI.KeyExists("TELEGRAMM", "Token"))
        this.mainForm.tgToken = this.INI.ReadINI("TELEGRAMM", "Token");
      this.mainForm.auto_read_settings();
      this.Close();
    }

    private void auto_read_settings()
    {
      if (this.INI.KeyExists("CHECKBOX", "BrowserOpen"))
        this.BrowserOpenChekBox.Checked = this.INI.ReadINI("CHECKBOX", "BrowserOpen") == "Checked";
      if (this.INI.KeyExists("CHECKBOX", "dynamic"))
        this.UseDynamicProxyCheckBox.Checked = this.INI.ReadINI("CHECKBOX", "dynamic") == "Checked";
      if (this.INI.KeyExists("CHECKBOX", "Beep"))
        this.BeepCheckBox.Checked = this.INI.ReadINI("CHECKBOX", "Beep") == "Checked";
      if (this.INI.KeyExists("CHECKBOX", "SearchNow"))
        this.SearchNowCheckBox.Checked = this.INI.ReadINI("CHECKBOX", "SearchNow") == "Checked";
      if (this.INI.KeyExists("CHECKBOX", "Telegramm"))
        this.TelegrammCheckBox.Checked = this.INI.ReadINI("CHECKBOX", "Telegramm") == "Checked";
      if (this.INI.KeyExists("CHECKBOX", "Notify"))
        this.notifyCheckBox.Checked = this.INI.ReadINI("CHECKBOX", "Notify") == "Checked";
      this.RaiseAdvertsCheck.Checked = StatSetClass.RaiseAdverts;
      this.MagazCheck.Checked = StatSetClass.MagazCheck;
      if (this.INI.KeyExists("TELEGRAMM", "chat"))
        this.TgrammChatTextBox.Text = this.INI.ReadINI("TELEGRAMM", "chat");
      else
        this.TgrammChatTextBox.Text = "";
      if (this.INI.KeyExists("TELEGRAMM", "key"))
        this.TgrammKeyTextBox.Text = this.INI.ReadINI("TELEGRAMM", "key");
      else
        this.TgrammKeyTextBox.Text = "";
      if (this.INI.KeyExists("TELEGRAMM", "Token"))
        this.TgrammTokenTextBox.Text = this.INI.ReadINI("TELEGRAMM", "Token");
      else
        this.TgrammTokenTextBox.Text = "";
      if (this.INI.KeyExists("SETCLASS", "TInterval"))
        this.TIntervalTextBox.Text = this.INI.ReadINI("SETCLASS", "TInterval");
      this.numericUpDown1.Value = (Decimal) (StatSetClass.TimerInterval / 1000);
      if (StatSetClass.r)
        return;
      this.TgrammChatTextBox.Enabled = false;
      this.TgrammKeyTextBox.Enabled = false;
      this.TgrammTokenTextBox.Enabled = false;
      this.TelegrammCheckBox.Enabled = false;
      this.TelegrammCheckBox.Checked = false;
    }

    private void Interval_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void ChatIdTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void label9_Click(object sender, EventArgs e) => Process.Start("https://www.youtube.com/channel/UCqBQctYsXrT2sUYzfwCZ2YQ");

    private void AutoStartWindowsChekBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rkApp.GetValue("AvitoAvtoRinger") != null)
      {
        if (this.AutoStartWindowsChekBox.Checked)
          return;
        this.rkApp.DeleteValue("AvitoAvtoRinger", false);
      }
      else
        this.rkApp.SetValue("AvitoAvtoRinger", (object) Application.ExecutablePath.ToString());
    }

    private void SetForm_Load(object sender, EventArgs e)
    {
      if (StatSetClass.browser != "")
        this.button1.Text = "Открывать в " + StatSetClass.browser;
      if (this.INI.KeyExists("ACTIVATION", StatSetClass.ACTkey))
        this.ActKeyTextBox.Text = this.INI.ReadINI("ACTIVATION", StatSetClass.ACTkey);
      this.UcodeTextBox.Text = StatSetClass.ACTkey;
      if (this.INI.KeyExists("CHECKBOX", "Notify"))
        StatSetClass.notify = this.INI.ReadINI("CHECKBOX", "Notify") == "Checked";
      this.ProxyLogin.Text = StatSetClass.proxyLogin;
      this.ProxyPass.Text = StatSetClass.proxyPassword;
      this.ProxyIP.Text = StatSetClass.proxyIp;
      this.ProxyPort.Text = StatSetClass.proxyPort;
      this.LoginDynamic.Text = StatSetClass.proxyLoginDynamic;
      this.PassDynamic.Text = StatSetClass.proxyPasswordDynamic;
      this.IpDynamic.Text = StatSetClass.proxyIpDynamic;
      this.PortDynamic.Text = StatSetClass.proxyPortDynamic;
      this.AvitoDostavkaCheckBox.Checked = StatSetClass.AvitoDostavka;
      new Thread(new ThreadStart(this.CheckIp)).Start();
    }

    public void CheckIp()
    {
      try
      {
        StreamReader reader = new StreamReader(new WebClient().OpenRead("https://avtoringer.ru/getIP.php"));
        this.Invoke((Action) (() => this.Text = this.Text + " | Ваш IP: " + reader.ReadLine()));
      }
      catch
      {
        this.Invoke((Action) (() => this.Text += " | Ну удалось узнать IP"));
      }
    }

    private void TgrammKeyChatWhantToNowButton_Click(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.WindowState = FormWindowState.Minimized;
        new TgrammKeyFinder(this).Show((IWin32Window) this);
      }
      else
      {
        int num = (int) MessageBox.Show("Отправка в телеграмм, только в полной версии, для покупки обращайтесь vk.com/scrypto");
      }
    }

    private void button1_Click(object sender, EventArgs e) => new Browsers(this).Show((IWin32Window) this);

    private void PayedAdvertsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      if (!StatSetClass.r)
      {
        this.mainForm.timer1.Interval = (int) this.numericUpDown1.Value * 1000;
        if (!(this.numericUpDown1.Value < 120M))
          return;
        this.numericUpDown1.Value = 120M;
        int num = (int) MessageBox.Show("В демо версии интервал нельзя устанавливать меньше 2 минут");
      }
      else
      {
        this.mainForm.timer1.Interval = (int) this.numericUpDown1.Value * 1000;
        if (!this.numUpdownWarning && this.numericUpDown1.Value < 30M)
        {
          int num = (int) MessageBox.Show("При уменьшении интервала ниже 30 секунд, авито может забанить за большое количество запросов", "Обязательно к прочтению", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.numUpdownWarning = true;
        }
      }
    }

    private void logCheckBox1_CheckedChanged(object sender, EventArgs e)
    {
      if (this.logCheckBox1.Checked)
        StatSetClass.l = true;
      else
        StatSetClass.l = false;
    }

    private void notifyCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.notifyCheckBox.Checked)
        return;
      this.BeepCheckBox.Checked = false;
    }

    private void BeepCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.BeepCheckBox.Checked)
      {
        this.notifyCheckBox.Checked = false;
        StatSetClass.notify = true;
      }
      else
        StatSetClass.notify = false;
    }

    private void label8_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Если поля Proxy пустые, то использует прокси программ, если заполнены то данные из полей");
    }

    private void UseDynamicProxyCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.UseDynamicProxyCheckBox.Checked)
        StatSetClass.dynamicCheckBox = true;
      else
        StatSetClass.dynamicCheckBox = false;
    }

    private void BrowserOpenChekBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.BrowserOpenChekBox.Checked)
        StatSetClass.bwsrOpen = true;
      else
        StatSetClass.bwsrOpen = false;
    }

    private void RaiseAdvertsCheck_CheckedChanged(object sender, EventArgs e)
    {
      if (this.RaiseAdvertsCheck.Checked)
        StatSetClass.RaiseAdverts = true;
      else
        StatSetClass.RaiseAdverts = false;
    }

    private void MagazCheck_CheckedChanged(object sender, EventArgs e)
    {
      if (this.MagazCheck.Checked)
        StatSetClass.MagazCheck = true;
      else
        StatSetClass.MagazCheck = false;
    }

    private void AvitoDostavkaCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.AvitoDostavkaCheckBox.Checked)
        StatSetClass.AvitoDostavka = true;
      else
        StatSetClass.AvitoDostavka = false;
    }

    private void TelegrammCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.TelegrammCheckBox.Checked)
        StatSetClass.telgSend = true;
      else
        StatSetClass.telgSend = false;
    }

    private void button2_Click(object sender, EventArgs e) => Process.Start("explorer", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Avtoringer\\");

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.BrowserOpenChekBox = new CheckBox();
      this.AutoStartWindowsChekBox = new CheckBox();
      this.BeepCheckBox = new CheckBox();
      this.SabeButton = new Button();
      this.label1 = new Label();
      this.label9 = new Label();
      this.SearchNowCheckBox = new CheckBox();
      this.toolTip1 = new ToolTip(this.components);
      this.TelegrammCheckBox = new CheckBox();
      this.TgrammKeyTextBox = new TextBox();
      this.TgrammChatTextBox = new TextBox();
      this.label10 = new Label();
      this.label11 = new Label();
      this.TgrammKeyChatWhantToNowButton = new Button();
      this.panel1 = new Panel();
      this.label12 = new Label();
      this.TgrammTokenTextBox = new TextBox();
      this.label13 = new Label();
      this.TIntervalTextBox = new TextBox();
      this.numericUpDown1 = new NumericUpDown();
      this.label2 = new Label();
      this.openFileDialog1 = new OpenFileDialog();
      this.button1 = new Button();
      this.ActKeyTextBox = new TextBox();
      this.label3 = new Label();
      this.label14 = new Label();
      this.UcodeTextBox = new TextBox();
      this.logCheckBox1 = new CheckBox();
      this.notifyCheckBox = new CheckBox();
      this.label5 = new Label();
      this.ProxyLogin = new TextBox();
      this.ProxyPass = new TextBox();
      this.panel2 = new Panel();
      this.label8 = new Label();
      this.label6 = new Label();
      this.ProxyPort = new TextBox();
      this.label7 = new Label();
      this.ProxyIP = new TextBox();
      this.label4 = new Label();
      this.panel3 = new Panel();
      this.UseDynamicProxyCheckBox = new CheckBox();
      this.label16 = new Label();
      this.PortDynamic = new TextBox();
      this.label17 = new Label();
      this.IpDynamic = new TextBox();
      this.label18 = new Label();
      this.PassDynamic = new TextBox();
      this.label19 = new Label();
      this.LoginDynamic = new TextBox();
      this.RaiseAdvertsCheck = new CheckBox();
      this.MagazCheck = new CheckBox();
      this.AvitoDostavkaCheckBox = new CheckBox();
      this.button2 = new Button();
      this.panel1.SuspendLayout();
      this.numericUpDown1.BeginInit();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.BrowserOpenChekBox.AutoSize = true;
      this.BrowserOpenChekBox.Location = new Point(14, 44);
      this.BrowserOpenChekBox.Name = "BrowserOpenChekBox";
      this.BrowserOpenChekBox.Size = new Size(141, 17);
      this.BrowserOpenChekBox.TabIndex = 3;
      this.BrowserOpenChekBox.Text = "Открывать в браузере";
      this.BrowserOpenChekBox.UseVisualStyleBackColor = true;
      this.BrowserOpenChekBox.CheckedChanged += new EventHandler(this.BrowserOpenChekBox_CheckedChanged);
      this.AutoStartWindowsChekBox.AutoSize = true;
      this.AutoStartWindowsChekBox.Location = new Point(14, 12);
      this.AutoStartWindowsChekBox.Name = "AutoStartWindowsChekBox";
      this.AutoStartWindowsChekBox.Size = new Size(141, 17);
      this.AutoStartWindowsChekBox.TabIndex = 4;
      this.AutoStartWindowsChekBox.Text = "Автозапуск с Windows";
      this.AutoStartWindowsChekBox.UseVisualStyleBackColor = true;
      this.AutoStartWindowsChekBox.CheckedChanged += new EventHandler(this.AutoStartWindowsChekBox_CheckedChanged);
      this.BeepCheckBox.AutoSize = true;
      this.BeepCheckBox.Location = new Point(14, 60);
      this.BeepCheckBox.Name = "BeepCheckBox";
      this.BeepCheckBox.Size = new Size(170, 17);
      this.BeepCheckBox.TabIndex = 5;
      this.BeepCheckBox.Text = "Звук при новом объявлении";
      this.BeepCheckBox.UseVisualStyleBackColor = true;
      this.BeepCheckBox.CheckedChanged += new EventHandler(this.BeepCheckBox_CheckedChanged);
      this.SabeButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.SabeButton.ForeColor = System.Drawing.Color.Red;
      this.SabeButton.Location = new Point(226, 312);
      this.SabeButton.Name = "SabeButton";
      this.SabeButton.Size = new Size(415, 23);
      this.SabeButton.TabIndex = 6;
      this.SabeButton.Text = "Сохранить";
      this.SabeButton.UseVisualStyleBackColor = true;
      this.SabeButton.Click += new EventHandler(this.SabeButton_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(513, 49);
      this.label1.Name = "label1";
      this.label1.Size = new Size(0, 13);
      this.label1.TabIndex = 8;
      this.label9.Cursor = Cursors.Hand;
      this.label9.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label9.ForeColor = SystemColors.MenuHighlight;
      this.label9.Location = new Point(20, 409);
      this.label9.Name = "label9";
      this.label9.Size = new Size(625, 39);
      this.label9.TabIndex = 23;
      this.label9.Text = "У автора этой программы, есть свою Youtube канал, на котором можно найти инструкцию по использованию данной программы, достаточно щелкнуть по этой надписи";
      this.label9.Click += new EventHandler(this.label9_Click);
      this.SearchNowCheckBox.AutoSize = true;
      this.SearchNowCheckBox.Location = new Point(14, 28);
      this.SearchNowCheckBox.Name = "SearchNowCheckBox";
      this.SearchNowCheckBox.Size = new Size(278, 17);
      this.SearchNowCheckBox.TabIndex = 24;
      this.SearchNowCheckBox.Text = "Начинать поиск сразу после запуска программы";
      this.SearchNowCheckBox.UseVisualStyleBackColor = true;
      this.TelegrammCheckBox.AutoSize = true;
      this.TelegrammCheckBox.Location = new Point(14, 76);
      this.TelegrammCheckBox.Name = "TelegrammCheckBox";
      this.TelegrammCheckBox.Size = new Size(156, 17);
      this.TelegrammCheckBox.TabIndex = 27;
      this.TelegrammCheckBox.Text = "Отправлять в Телеграмм";
      this.TelegrammCheckBox.UseVisualStyleBackColor = true;
      this.TelegrammCheckBox.CheckedChanged += new EventHandler(this.TelegrammCheckBox_CheckedChanged);
      this.TgrammKeyTextBox.Location = new Point(107, 8);
      this.TgrammKeyTextBox.Name = "TgrammKeyTextBox";
      this.TgrammKeyTextBox.Size = new Size(376, 20);
      this.TgrammKeyTextBox.TabIndex = 28;
      this.TgrammChatTextBox.Location = new Point(554, 7);
      this.TgrammChatTextBox.Name = "TgrammChatTextBox";
      this.TgrammChatTextBox.Size = new Size(74, 20);
      this.TgrammChatTextBox.TabIndex = 29;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(489, 11);
      this.label10.Name = "label10";
      this.label10.Size = new Size(66, 13);
      this.label10.TabIndex = 30;
      this.label10.Text = "Чат телеги:";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(5, 11);
      this.label11.Name = "label11";
      this.label11.Size = new Size(99, 13);
      this.label11.TabIndex = 31;
      this.label11.Text = "Ключ бота телеги:";
      this.TgrammKeyChatWhantToNowButton.Location = new Point(3, 85);
      this.TgrammKeyChatWhantToNowButton.Name = "TgrammKeyChatWhantToNowButton";
      this.TgrammKeyChatWhantToNowButton.Size = new Size(624, 23);
      this.TgrammKeyChatWhantToNowButton.TabIndex = 32;
      this.TgrammKeyChatWhantToNowButton.Text = "Узнать ключ и чат в телеге";
      this.TgrammKeyChatWhantToNowButton.UseVisualStyleBackColor = true;
      this.TgrammKeyChatWhantToNowButton.Click += new EventHandler(this.TgrammKeyChatWhantToNowButton_Click);
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.label12);
      this.panel1.Controls.Add((Control) this.TgrammTokenTextBox);
      this.panel1.Controls.Add((Control) this.label13);
      this.panel1.Controls.Add((Control) this.TgrammKeyTextBox);
      this.panel1.Controls.Add((Control) this.TIntervalTextBox);
      this.panel1.Controls.Add((Control) this.TgrammKeyChatWhantToNowButton);
      this.panel1.Controls.Add((Control) this.TgrammChatTextBox);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Location = new Point(12, 143);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(633, 115);
      this.panel1.TabIndex = 33;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(63, 36);
      this.label12.Name = "label12";
      this.label12.Size = new Size(41, 13);
      this.label12.TabIndex = 34;
      this.label12.Text = "Токен:";
      this.TgrammTokenTextBox.Location = new Point(107, 33);
      this.TgrammTokenTextBox.Name = "TgrammTokenTextBox";
      this.TgrammTokenTextBox.Size = new Size(521, 20);
      this.TgrammTokenTextBox.TabIndex = 33;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(65, 62);
      this.label13.Name = "label13";
      this.label13.Size = new Size(457, 13);
      this.label13.TabIndex = 35;
      this.label13.Text = "Таймаут между отправкой  сообщений в телеграмм в миллисек.(1 сек = 1000миллисек)";
      this.TIntervalTextBox.Location = new Point(528, 59);
      this.TIntervalTextBox.Name = "TIntervalTextBox";
      this.TIntervalTextBox.Size = new Size(100, 20);
      this.TIntervalTextBox.TabIndex = 34;
      this.TIntervalTextBox.Text = "1000";
      this.numericUpDown1.Location = new Point(543, 12);
      this.numericUpDown1.Maximum = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.numericUpDown1.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.numericUpDown1.Name = "numericUpDown1";
      this.numericUpDown1.Size = new Size(100, 20);
      this.numericUpDown1.TabIndex = 36;
      this.numericUpDown1.Value = new Decimal(new int[4]
      {
        120,
        0,
        0,
        0
      });
      this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(313, 14);
      this.label2.Name = "label2";
      this.label2.Size = new Size(228, 13);
      this.label2.TabIndex = 37;
      this.label2.Text = "Проверять новые объявления каждые сек.";
      this.openFileDialog1.FileName = "openFileDialog1";
      this.button1.Location = new Point(191, 53);
      this.button1.Name = "button1";
      this.button1.Size = new Size(450, 23);
      this.button1.TabIndex = 38;
      this.button1.Text = "Укажите путь к браузеру, которым открывать объявления";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.ActKeyTextBox.Location = new Point(119, 374);
      this.ActKeyTextBox.Name = "ActKeyTextBox";
      this.ActKeyTextBox.Size = new Size(526, 20);
      this.ActKeyTextBox.TabIndex = 39;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(18, 377);
      this.label3.Name = "label3";
      this.label3.Size = new Size(95, 13);
      this.label3.TabIndex = 40;
      this.label3.Text = "Ключ активации: ";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(18, 351);
      this.label14.Name = "label14";
      this.label14.Size = new Size(95, 13);
      this.label14.TabIndex = 42;
      this.label14.Text = "Уникальный код:";
      this.UcodeTextBox.Location = new Point(119, 348);
      this.UcodeTextBox.Name = "UcodeTextBox";
      this.UcodeTextBox.Size = new Size(526, 20);
      this.UcodeTextBox.TabIndex = 41;
      this.logCheckBox1.AutoSize = true;
      this.logCheckBox1.Location = new Point(259, 125);
      this.logCheckBox1.Name = "logCheckBox1";
      this.logCheckBox1.Size = new Size(386, 17);
      this.logCheckBox1.TabIndex = 43;
      this.logCheckBox1.Text = "Писать лог! (Будет писать в папку logging, до выключения программы)";
      this.logCheckBox1.UseVisualStyleBackColor = true;
      this.logCheckBox1.CheckedChanged += new EventHandler(this.logCheckBox1_CheckedChanged);
      this.notifyCheckBox.AutoSize = true;
      this.notifyCheckBox.Enabled = false;
      this.notifyCheckBox.Location = new Point(14, 110);
      this.notifyCheckBox.Name = "notifyCheckBox";
      this.notifyCheckBox.Size = new Size(170, 17);
      this.notifyCheckBox.TabIndex = 44;
      this.notifyCheckBox.Text = "Всплывающие уведомления";
      this.notifyCheckBox.UseVisualStyleBackColor = true;
      this.notifyCheckBox.CheckedChanged += new EventHandler(this.notifyCheckBox_CheckedChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(123, 6);
      this.label5.Name = "label5";
      this.label5.Size = new Size(33, 13);
      this.label5.TabIndex = 46;
      this.label5.Text = "Login";
      this.ProxyLogin.Location = new Point(164, 0);
      this.ProxyLogin.Name = "ProxyLogin";
      this.ProxyLogin.Size = new Size(100, 20);
      this.ProxyLogin.TabIndex = 47;
      this.ProxyPass.Location = new Point(164, 20);
      this.ProxyPass.Name = "ProxyPass";
      this.ProxyPass.Size = new Size(100, 20);
      this.ProxyPass.TabIndex = 48;
      this.panel2.BorderStyle = BorderStyle.FixedSingle;
      this.panel2.Controls.Add((Control) this.label8);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.ProxyPort);
      this.panel2.Controls.Add((Control) this.label7);
      this.panel2.Controls.Add((Control) this.ProxyIP);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.ProxyPass);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.ProxyLogin);
      this.panel2.Location = new Point(192, 82);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(449, 42);
      this.panel2.TabIndex = 49;
      this.label8.Cursor = Cursors.Hand;
      this.label8.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label8.ForeColor = SystemColors.ActiveCaption;
      this.label8.Location = new Point(4, -1);
      this.label8.Name = "label8";
      this.label8.Size = new Size(118, 43);
      this.label8.TabIndex = 54;
      this.label8.Text = "PROXY ПО УМОЛЧАНИЮ:";
      this.label8.TextAlign = ContentAlignment.MiddleCenter;
      this.label8.Click += new EventHandler(this.label8_Click);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(279, 23);
      this.label6.Name = "label6";
      this.label6.Size = new Size(26, 13);
      this.label6.TabIndex = 53;
      this.label6.Text = "Port";
      this.ProxyPort.Location = new Point(315, 20);
      this.ProxyPort.Name = "ProxyPort";
      this.ProxyPort.Size = new Size(100, 20);
      this.ProxyPort.TabIndex = 52;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(279, 6);
      this.label7.Name = "label7";
      this.label7.Size = new Size(17, 13);
      this.label7.TabIndex = 50;
      this.label7.Text = "IP";
      this.ProxyIP.Location = new Point(315, 0);
      this.ProxyIP.Name = "ProxyIP";
      this.ProxyIP.Size = new Size(100, 20);
      this.ProxyIP.TabIndex = 51;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(123, 23);
      this.label4.Name = "label4";
      this.label4.Size = new Size(30, 13);
      this.label4.TabIndex = 49;
      this.label4.Text = "Pass";
      this.panel3.BorderStyle = BorderStyle.FixedSingle;
      this.panel3.Controls.Add((Control) this.UseDynamicProxyCheckBox);
      this.panel3.Controls.Add((Control) this.label16);
      this.panel3.Controls.Add((Control) this.PortDynamic);
      this.panel3.Controls.Add((Control) this.label17);
      this.panel3.Controls.Add((Control) this.IpDynamic);
      this.panel3.Controls.Add((Control) this.label18);
      this.panel3.Controls.Add((Control) this.PassDynamic);
      this.panel3.Controls.Add((Control) this.label19);
      this.panel3.Controls.Add((Control) this.LoginDynamic);
      this.panel3.Location = new Point(12, 264);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(633, 42);
      this.panel3.TabIndex = 50;
      this.UseDynamicProxyCheckBox.AutoSize = true;
      this.UseDynamicProxyCheckBox.Enabled = false;
      this.UseDynamicProxyCheckBox.Location = new Point(6, 11);
      this.UseDynamicProxyCheckBox.Name = "UseDynamicProxyCheckBox";
      this.UseDynamicProxyCheckBox.Size = new Size(214, 17);
      this.UseDynamicProxyCheckBox.TabIndex = 51;
      this.UseDynamicProxyCheckBox.Text = "Использовать динамические прокси";
      this.UseDynamicProxyCheckBox.UseVisualStyleBackColor = true;
      this.UseDynamicProxyCheckBox.CheckedChanged += new EventHandler(this.UseDynamicProxyCheckBox_CheckedChanged);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(457, 23);
      this.label16.Name = "label16";
      this.label16.Size = new Size(26, 13);
      this.label16.TabIndex = 53;
      this.label16.Text = "Port";
      this.PortDynamic.Location = new Point(493, 20);
      this.PortDynamic.Name = "PortDynamic";
      this.PortDynamic.Size = new Size(100, 20);
      this.PortDynamic.TabIndex = 52;
      this.label17.AutoSize = true;
      this.label17.Location = new Point(457, 6);
      this.label17.Name = "label17";
      this.label17.Size = new Size(17, 13);
      this.label17.TabIndex = 50;
      this.label17.Text = "IP";
      this.IpDynamic.Location = new Point(493, 0);
      this.IpDynamic.Name = "IpDynamic";
      this.IpDynamic.Size = new Size(100, 20);
      this.IpDynamic.TabIndex = 51;
      this.label18.AutoSize = true;
      this.label18.Location = new Point(301, 23);
      this.label18.Name = "label18";
      this.label18.Size = new Size(30, 13);
      this.label18.TabIndex = 49;
      this.label18.Text = "Pass";
      this.PassDynamic.Location = new Point(342, 20);
      this.PassDynamic.Name = "PassDynamic";
      this.PassDynamic.Size = new Size(100, 20);
      this.PassDynamic.TabIndex = 48;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(301, 6);
      this.label19.Name = "label19";
      this.label19.Size = new Size(33, 13);
      this.label19.TabIndex = 46;
      this.label19.Text = "Login";
      this.LoginDynamic.Location = new Point(342, 0);
      this.LoginDynamic.Name = "LoginDynamic";
      this.LoginDynamic.Size = new Size(100, 20);
      this.LoginDynamic.TabIndex = 47;
      this.RaiseAdvertsCheck.AutoSize = true;
      this.RaiseAdvertsCheck.Location = new Point(14, 93);
      this.RaiseAdvertsCheck.Name = "RaiseAdvertsCheck";
      this.RaiseAdvertsCheck.Size = new Size(178, 17);
      this.RaiseAdvertsCheck.TabIndex = 48;
      this.RaiseAdvertsCheck.Text = "Показывать поднятые объяв.";
      this.RaiseAdvertsCheck.UseVisualStyleBackColor = true;
      this.RaiseAdvertsCheck.CheckedChanged += new EventHandler(this.RaiseAdvertsCheck_CheckedChanged);
      this.MagazCheck.AutoSize = true;
      this.MagazCheck.Location = new Point(14, 126);
      this.MagazCheck.Name = "MagazCheck";
      this.MagazCheck.Size = new Size(220, 17);
      this.MagazCheck.TabIndex = 51;
      this.MagazCheck.Text = "Игнорировать объявления магазинов";
      this.MagazCheck.UseVisualStyleBackColor = true;
      this.MagazCheck.CheckedChanged += new EventHandler(this.MagazCheck_CheckedChanged);
      this.AvitoDostavkaCheckBox.AutoSize = true;
      this.AvitoDostavkaCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.AvitoDostavkaCheckBox.Checked = true;
      this.AvitoDostavkaCheckBox.CheckState = CheckState.Checked;
      this.AvitoDostavkaCheckBox.Location = new Point(375, 33);
      this.AvitoDostavkaCheckBox.Name = "AvitoDostavkaCheckBox";
      this.AvitoDostavkaCheckBox.Size = new Size(267, 17);
      this.AvitoDostavkaCheckBox.TabIndex = 52;
      this.AvitoDostavkaCheckBox.Text = "Показывать объявления только своего города";
      this.AvitoDostavkaCheckBox.UseVisualStyleBackColor = false;
      this.AvitoDostavkaCheckBox.CheckedChanged += new EventHandler(this.AvitoDostavkaCheckBox_CheckedChanged);
      this.button2.Location = new Point(12, 312);
      this.button2.Name = "button2";
      this.button2.Size = new Size(208, 23);
      this.button2.TabIndex = 36;
      this.button2.Text = "Открыть Config файлы";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(656, 404);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.AvitoDostavkaCheckBox);
      this.Controls.Add((Control) this.MagazCheck);
      this.Controls.Add((Control) this.RaiseAdvertsCheck);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.notifyCheckBox);
      this.Controls.Add((Control) this.logCheckBox1);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.UcodeTextBox);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.ActKeyTextBox);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.numericUpDown1);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.TelegrammCheckBox);
      this.Controls.Add((Control) this.SearchNowCheckBox);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.SabeButton);
      this.Controls.Add((Control) this.BeepCheckBox);
      this.Controls.Add((Control) this.AutoStartWindowsChekBox);
      this.Controls.Add((Control) this.BrowserOpenChekBox);
      this.Cursor = Cursors.Arrow;
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (SetForm);
      this.Text = "Настройки AvtoRinger";
      this.Load += new EventHandler(this.SetForm_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.numericUpDown1.EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
