// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.Form1
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using AvtoRinger.Properties;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace AvitoAvtoRinger
{
  public class Form1 : Form
  {
    private int upAdverts = 0;
    private List<Adverts1> List1 = new List<Adverts1>();
    public List<Adverts1> ListVirtual = new List<Adverts1>();
    private List<Button> ButtonList = new List<Button>();
    private List<List<Adverts1>> ListAdvertList = new List<List<Adverts1>>();
    private BdWorker bd;
    private HtmlWorker htmlWorker = new HtmlWorker();
    private WebWorker webWorker = new WebWorker();
    private int counterD = 0;
    private IniFile INIF = new IniFile("Config.ini");
    private ItemComparer itemComparer;
    public long tgKey = 0;
    public int tgChat = 0;
    public string tgToken = "";
    private Thread StWorkThread;
    private Random rndStart = new Random();
    private Thread TimerThread;
    private List<string> progProxyVersion = new List<string>();
    private bool numUpdownWarning = false;
    private IContainer components = (IContainer) null;
    public Label label1;
    public Label label2;
    public ComboBox BrandComboBox;
    public Label label3;
    public TextBox ModelTextBox;
    public Label label4;
    public Label label5;
    public Label label6;
    public TextBox YearFromTextBox;
    public TextBox YearToTextBox;
    public TextBox KmMaxTextBox;
    public TextBox KmMinTextBox;
    public Label label7;
    public Label label8;
    public Label label9;
    public Label label10;
    public Label label12;
    public Label label13;
    public Label label14;
    public ListBox TransmissionListBox;
    public ListBox BodyTypeListBox;
    public Label label15;
    public ListBox PrivListBox;
    public Label label16;
    public TextBox PriceTextBoxMax;
    public TextBox PriceTextBoxMin;
    public Label label17;
    public Label label18;
    public Button SaveAndValidateButton;
    public TextBox UrlTextBox1;
    public ComboBox TownComboBox;
    private Label LminLabel;
    private Label LmaxLabel;
    public HScrollBar LitrScrollMin;
    public HScrollBar LitrScrollMax;
    private Label label11;
    public ListBox DvTypeListBox;
    private Label label19;
    private Button StartButton;
    private ListView AdvertsLitsView;
    private ColumnHeader Data;
    private ColumnHeader Zagolovok;
    private ColumnHeader Price;
    public TextBox LogBox;
    private Label label20;
    private Button SetButton;
    private ColumnHeader Link;
    private Button button1;
    public TextBox UrlTextBox2;
    public TextBox UrlTextBox3;
    public TextBox UrlTextBox4;
    public TextBox UrlTextBox5;
    public TextBox UrlTextBox6;
    public TextBox UrlTextBox7;
    public TextBox UrlTextBox8;
    public TextBox UrlTextBox9;
    public TextBox UrlTextBox10;
    public TextBox UrlTextBox11;
    public TextBox UrlTextBox16;
    public TextBox UrlTextBox15;
    public TextBox UrlTextBox14;
    public TextBox UrlTextBox13;
    public TextBox UrlTextBox12;
    private Button ShowStopWordsButton;
    private Button ObjectiveWords;
    private Label label21;
    private Label label22;
    private Label label23;
    private Label label24;
    private Label label25;
    private Label label26;
    private Label label27;
    private Label label28;
    private Label label29;
    private Label label30;
    private Label label31;
    private Label label32;
    private Label label33;
    private Label label34;
    private Label label35;
    private Label label36;
    private ColumnHeader stopWords;
    private ColumnHeader targetWords;
    private NotifyIcon notifyIcon1;
    public System.Windows.Forms.Timer timer1;
    public CheckBox checkBox1;
    public CheckBox checkBox2;
    public CheckBox checkBox3;
    public CheckBox checkBox4;
    public CheckBox checkBox5;
    public CheckBox checkBox6;
    public CheckBox checkBox7;
    public CheckBox checkBox8;
    public CheckBox checkBox9;
    public CheckBox checkBox10;
    public CheckBox checkBox11;
    public CheckBox checkBox16;
    public CheckBox checkBox15;
    public CheckBox checkBox14;
    public CheckBox checkBox13;
    public CheckBox checkBox12;
    public CheckBox BrowserOpenChekBox;
    public CheckBox BeepCheckBox;
    public CheckBox TelegrammCheckBox;
    public Label label37;
    private CheckBox RaiseAdvertsCheck;
    private CheckBox MagazCheck;
    public CheckBox AvitoDostavkaCheckBox;
    public NumericUpDown numericUpDown1;
    private PictureBox pictureBox1;
    private PictureBox pictureBox2;
    private PictureBox pictureBox3;
    private PictureBox pictureBox4;
    private PictureBox pictureBox5;
    private PictureBox pictureBox6;
    private PictureBox pictureBox7;
    private PictureBox pictureBox8;
    private PictureBox pictureBox9;
    private PictureBox pictureBox10;
    private PictureBox pictureBox11;
    private PictureBox pictureBox12;
    private PictureBox pictureBox13;
    private PictureBox pictureBox14;
    private PictureBox pictureBox15;
    private PictureBox pictureBox16;
    private GroupBox groupBox1;
    private PictureBox AdvertPictureBox;
    private TextBox AdvertInfoTBox;
    private Label label38;
    private Button GetAdvertInfoButton;
    private System.Windows.Forms.Timer GetAdvertTimer;

    public Form1()
    {
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      this.InitializeComponent();
      this.bd = new BdWorker(this);
      if (this.INIF.KeyExists("TELEGRAMM", "key"))
        this.tgKey = long.Parse(this.INIF.ReadINI("TELEGRAMM", "key"));
      if (this.INIF.KeyExists("TELEGRAMM", "chat"))
        this.tgChat = int.Parse(this.INIF.ReadINI("TELEGRAMM", "chat"));
      if (this.INIF.KeyExists("TELEGRAMM", "Token"))
        this.tgToken = this.INIF.ReadINI("TELEGRAMM", "Token");
      this.itemComparer = new ItemComparer(this);
      if (this.INIF.KeyExists("SEARCH", "UrlBox"))
        this.UrlTextBox1.Text = this.INIF.ReadINI("SEARCH", "UrlBox");
      if (this.INIF.KeyExists("SEARCH", "UrlBox2"))
        this.UrlTextBox2.Text = this.INIF.ReadINI("SEARCH", "UrlBox2");
      if (this.INIF.KeyExists("SEARCH", "UrlBox3"))
        this.UrlTextBox3.Text = this.INIF.ReadINI("SEARCH", "UrlBox3");
      if (this.INIF.KeyExists("SEARCH", "UrlBox4"))
        this.UrlTextBox4.Text = this.INIF.ReadINI("SEARCH", "UrlBox4");
      if (this.INIF.KeyExists("SEARCH", "UrlBox5"))
        this.UrlTextBox5.Text = this.INIF.ReadINI("SEARCH", "UrlBox5");
      if (this.INIF.KeyExists("SEARCH", "UrlBox6"))
        this.UrlTextBox6.Text = this.INIF.ReadINI("SEARCH", "UrlBox6");
      if (this.INIF.KeyExists("SEARCH", "UrlBox7"))
        this.UrlTextBox7.Text = this.INIF.ReadINI("SEARCH", "UrlBox7");
      if (this.INIF.KeyExists("SEARCH", "UrlBox8"))
        this.UrlTextBox8.Text = this.INIF.ReadINI("SEARCH", "UrlBox8");
      if (this.INIF.KeyExists("SEARCH", "UrlBox9"))
        this.UrlTextBox9.Text = this.INIF.ReadINI("SEARCH", "UrlBox9");
      if (this.INIF.KeyExists("SEARCH", "UrlBox10"))
        this.UrlTextBox10.Text = this.INIF.ReadINI("SEARCH", "UrlBox10");
      if (this.INIF.KeyExists("SEARCH", "UrlBox11"))
        this.UrlTextBox11.Text = this.INIF.ReadINI("SEARCH", "UrlBox11");
      if (this.INIF.KeyExists("SEARCH", "UrlBox12"))
        this.UrlTextBox12.Text = this.INIF.ReadINI("SEARCH", "UrlBox12");
      if (this.INIF.KeyExists("SEARCH", "UrlBox13"))
        this.UrlTextBox13.Text = this.INIF.ReadINI("SEARCH", "UrlBox13");
      if (this.INIF.KeyExists("SEARCH", "UrlBox14"))
        this.UrlTextBox14.Text = this.INIF.ReadINI("SEARCH", "UrlBox14");
      if (this.INIF.KeyExists("SEARCH", "UrlBox15"))
        this.UrlTextBox15.Text = this.INIF.ReadINI("SEARCH", "UrlBox15");
      if (this.INIF.KeyExists("SEARCH", "UrlBox16"))
        this.UrlTextBox16.Text = this.INIF.ReadINI("SEARCH", "UrlBox16");
      if (this.INIF.KeyExists("USERPROXY", "loginDynamic"))
        StatSetClass.proxyLoginDynamic = this.INIF.ReadINI("USERPROXY", "loginDynamic");
      if (this.INIF.KeyExists("USERPROXY", "passDynamic"))
        StatSetClass.proxyPasswordDynamic = this.INIF.ReadINI("USERPROXY", "passDynamic");
      if (this.INIF.KeyExists("USERPROXY", "ipDynamic"))
        StatSetClass.proxyIpDynamic = this.INIF.ReadINI("USERPROXY", "ipDynamic");
      if (this.INIF.KeyExists("USERPROXY", "portDynamic"))
        StatSetClass.proxyPortDynamic = this.INIF.ReadINI("USERPROXY", "portDynamic");
      for (int index = 0; index < this.AdvertsLitsView.Columns.Count; ++index)
      {
        if (this.INIF.KeyExists("COLUMNS", index.ToString()))
        {
          try
          {
            this.AdvertsLitsView.Columns[index].Width = int.Parse(this.INIF.ReadINI("COLUMNS", index.ToString()));
          }
          catch (Exception ex)
          {
            this.LogBox.AppendText(DateTime.Now.ToString() + " Что-то с шириной колонок нет так " + ex.ToString() + Environment.NewLine);
          }
        }
      }
      this.AdvertsLitsView.ListViewItemSorter = (IComparer) this.itemComparer;
      this.AdvertsLitsView.ColumnClick += new ColumnClickEventHandler(this.OnColumnClick);
      this.label21.MouseEnter += new EventHandler(this.lbEnter);
      this.label21.MouseLeave += new EventHandler(this.lbLeave);
      this.label22.MouseEnter += new EventHandler(this.lbEnter);
      this.label22.MouseLeave += new EventHandler(this.lbLeave);
      this.label23.MouseEnter += new EventHandler(this.lbEnter);
      this.label23.MouseLeave += new EventHandler(this.lbLeave);
      this.label24.MouseEnter += new EventHandler(this.lbEnter);
      this.label24.MouseLeave += new EventHandler(this.lbLeave);
      this.label25.MouseEnter += new EventHandler(this.lbEnter);
      this.label25.MouseLeave += new EventHandler(this.lbLeave);
      this.label26.MouseEnter += new EventHandler(this.lbEnter);
      this.label26.MouseLeave += new EventHandler(this.lbLeave);
      this.label27.MouseEnter += new EventHandler(this.lbEnter);
      this.label27.MouseLeave += new EventHandler(this.lbLeave);
      this.label28.MouseEnter += new EventHandler(this.lbEnter);
      this.label28.MouseLeave += new EventHandler(this.lbLeave);
      this.label29.MouseEnter += new EventHandler(this.lbEnter);
      this.label29.MouseLeave += new EventHandler(this.lbLeave);
      this.label30.MouseEnter += new EventHandler(this.lbEnter);
      this.label30.MouseLeave += new EventHandler(this.lbLeave);
      this.label31.MouseEnter += new EventHandler(this.lbEnter);
      this.label31.MouseLeave += new EventHandler(this.lbLeave);
      this.label32.MouseEnter += new EventHandler(this.lbEnter);
      this.label32.MouseLeave += new EventHandler(this.lbLeave);
      this.label33.MouseEnter += new EventHandler(this.lbEnter);
      this.label33.MouseLeave += new EventHandler(this.lbLeave);
      this.label34.MouseEnter += new EventHandler(this.lbEnter);
      this.label34.MouseLeave += new EventHandler(this.lbLeave);
      this.label35.MouseEnter += new EventHandler(this.lbEnter);
      this.label35.MouseLeave += new EventHandler(this.lbLeave);
      this.label36.MouseEnter += new EventHandler(this.lbEnter);
      this.label36.MouseLeave += new EventHandler(this.lbLeave);
      this.pictureBox1.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox1.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox2.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox2.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox3.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox3.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox4.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox4.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox5.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox5.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox6.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox6.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox7.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox7.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox8.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox8.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox9.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox9.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox10.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox10.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox11.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox11.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox12.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox12.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox13.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox13.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox14.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox14.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox15.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox15.MouseLeave += new EventHandler(this.pbLeave);
      this.pictureBox16.MouseEnter += new EventHandler(this.pbEnter);
      this.pictureBox16.MouseLeave += new EventHandler(this.pbLeave);
    }

    private void pbEnter(object sender, EventArgs e) => (sender as PictureBox).BackColor = System.Drawing.Color.Fuchsia;

    private void pbLeave(object sender, EventArgs e) => (sender as PictureBox).BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

    public void auto_read_settings()
    {
      if (this.INIF.KeyExists("CHECKBOX", "BrowserOpen"))
        this.BrowserOpenChekBox.Checked = this.INIF.ReadINI("CHECKBOX", "BrowserOpen") == "Checked";
      if (this.INIF.KeyExists("CHECKBOX", "Beep"))
        this.BeepCheckBox.Checked = this.INIF.ReadINI("CHECKBOX", "Beep") == "Checked";
      if (this.INIF.KeyExists("CHECKBOX", "Telegramm"))
        this.TelegrammCheckBox.Checked = this.INIF.ReadINI("CHECKBOX", "Telegramm") == "Checked";
      if (this.INIF.KeyExists("CHECKBOX", "AvitoDostavka"))
        this.AvitoDostavkaCheckBox.Checked = this.INIF.ReadINI("CHECKBOX", "AvitoDostavka") == "Checked";
      this.RaiseAdvertsCheck.Checked = StatSetClass.RaiseAdverts;
      this.MagazCheck.Checked = StatSetClass.MagazCheck;
      this.numericUpDown1.Value = (Decimal) (StatSetClass.TimerInterval / 1000);
      if (!StatSetClass.r)
      {
        this.TelegrammCheckBox.Enabled = false;
        this.TelegrammCheckBox.Checked = false;
      }
      try
      {
        foreach (ColumnHeader column in this.AdvertsLitsView.Columns)
        {
          if (column.Width <= 0)
            column.Width = 100;
        }
      }
      catch
      {
      }
    }

    private static void SetWebBrowserCompatiblityLevel()
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
      int lvl = 1000 * Form1.GetBrowserVersion();
      bool flag = System.IO.File.Exists(Path.ChangeExtension(Application.ExecutablePath, ".vshost.exe"));
      Form1.WriteCompatiblityLevel("HKEY_LOCAL_MACHINE", withoutExtension + ".exe", lvl);
      if (flag)
        Form1.WriteCompatiblityLevel("HKEY_LOCAL_MACHINE", withoutExtension + ".vshost.exe", lvl);
      Form1.WriteCompatiblityLevel("HKEY_CURRENT_USER", withoutExtension + ".exe", lvl);
      if (!flag)
        return;
      Form1.WriteCompatiblityLevel("HKEY_CURRENT_USER", withoutExtension + ".vshost.exe", lvl);
    }

    private static void WriteCompatiblityLevel(string root, string appName, int lvl)
    {
      try
      {
        Registry.SetValue(root + "\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", appName, (object) lvl);
      }
      catch (Exception ex)
      {
      }
    }

    public static int GetBrowserVersion()
    {
      string keyName = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Internet Explorer";
      string[] strArray = new string[4]
      {
        "svcVersion",
        "svcUpdateVersion",
        "Version",
        "W2kVersion"
      };
      int val1 = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string s = Convert.ToString(Registry.GetValue(keyName, strArray[index], (object) "0"));
        if (s != null)
        {
          int length = s.IndexOf('.');
          if (length > 0)
            s = s.Substring(0, length);
          int result = 0;
          if (int.TryParse(s, out result))
            val1 = Math.Max(val1, result);
        }
      }
      return val1;
    }

    private async void Telegramm_SendMessage(string arg, string text)
    {
      string key = arg;
      try
      {
        TelegramBotClient Bot = new TelegramBotClient(key);
        try
        {
          Telegram.Bot.Types.Message message = await Bot.SendTextMessageAsync((ChatId) this.tgKey, text, ParseMode.Default, false, false, this.tgChat, (IReplyMarkup) null, new CancellationToken());
        }
        catch (Exception ex)
        {
          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Произошла ошибка при отправке сообщения в телеграмм " + ex.ToString() + Environment.NewLine)));
        }
        Bot = (TelegramBotClient) null;
        key = (string) null;
      }
      catch (ApiRequestException ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        key = (string) null;
      }
    }

    private void OnColumnClick(object sender, ColumnClickEventArgs e)
    {
      try
      {
        this.itemComparer.ColumnIndex = e.Column;
        this.AdvertsLitsView.VirtualListSize = this.ListVirtual.Count;
        --this.AdvertsLitsView.VirtualListSize;
        ++this.AdvertsLitsView.VirtualListSize;
      }
      catch
      {
        int num = (int) MessageBox.Show("Нечего сортировать");
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void ModelTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(this.BrandComboBox.Text == "Все") && !(this.BrandComboBox.Text == "Иномарки") && (!(this.BrandComboBox.Text == "Отечественные") && !(this.BrandComboBox.Text == "Другая")) && !(this.BrandComboBox.Text == ""))
        return;
      e.Handled = true;
      int num = (int) MessageBox.Show("Нужно выбрать какую-нибудь опеределнную марку автомобля, только после этого вводить модель", "Привет");
    }

    private void BrandComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.ModelTextBox.Text = "";

    private void YearFromTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void YearToTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void LitrScrollMin_Scroll(object sender, ScrollEventArgs e) => this.LminLabel.Text = ((double) this.LitrScrollMin.Value / 10.0).ToString();

    private void LitrScrollMax_Scroll(object sender, ScrollEventArgs e) => this.LmaxLabel.Text = ((double) this.LitrScrollMax.Value / 10.0).ToString();

    private void button2_Click(object sender, EventArgs e)
    {
      bool flag = false;
      foreach (CheckBox checkBox in this.Controls.OfType<CheckBox>())
      {
        if (checkBox.Name.Contains("checkBox") && checkBox.Checked)
          flag = true;
      }
      if (flag)
      {
        
        if (this.UrlTextBox1.Text == "" && this.UrlTextBox2.Text == "" && (this.UrlTextBox3.Text == "" && this.UrlTextBox4.Text == "") && (this.UrlTextBox5.Text == "" && this.UrlTextBox6.Text == "" && (this.UrlTextBox7.Text == "" && this.UrlTextBox8.Text == "")) && (this.UrlTextBox9.Text == "" && this.UrlTextBox10.Text == "" && (this.UrlTextBox11.Text == "" && this.UrlTextBox12.Text == "") && (this.UrlTextBox13.Text == "" && this.UrlTextBox14.Text == "" && this.UrlTextBox15.Text == "")) && this.UrlTextBox16.Text == "")
        {
          int num = (int) MessageBox.Show("Не указано ни одной строки для поиска");
        }
        else
        {
          this.FormCaptionEditor();
          this.UrlColorerAvito();
          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Начинаем загрузку данных с сервера" + Environment.NewLine)));
          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Возможно потребуется некоторое время" + Environment.NewLine)));
          this.StWorkThread = new Thread(new ThreadStart(this.startWork));
          this.StWorkThread.Start();
          this.timer1.Enabled = true;
        }
        ((Control) sender).Enabled = false;
      }
      else
      {
        int num1 = (int) MessageBox.Show("Установите галочку, напротив той поисковой строки, котрую/которые, хотите отслеживать");
      }
    }

    public void CheckBoxDEMO()
    {
      foreach (CheckBox checkBox in this.Controls.OfType<CheckBox>())
      {
        if (checkBox.Name != "checkBox1" && checkBox.Name.Contains("checkBox"))
          checkBox.Checked = false;
      }
      this.TelegrammCheckBox.Enabled = false;
      this.TelegrammCheckBox.Checked = false;
      StatSetClass.telgSend = false;
      this.numericUpDown1.Enabled = false;
      StatSetClass.TimerInterval = 120;
    }

    public void UrlColorerAvito()
    {
      foreach (TextBox textBox in this.Controls.OfType<TextBox>())
      {
        if (textBox.Name != "LogBox")
        {
          if (textBox.Text != "")
          {
            if (textBox.Text.Length > 15)
            {
              if (textBox.Text.ToLower().Contains("avito"))
              {
                if ((StatSetClass.perm & StatSetClass.Permissions.Demka) == StatSetClass.Permissions.Demka || (StatSetClass.perm & StatSetClass.Permissions.Avito) == StatSetClass.Permissions.Avito)
                {
                  textBox.Text = textBox.Text.Replace("view=gallery", "");
                  if (!textBox.Text.Contains("s=104") && !textBox.Text.Contains("?"))
                    textBox.Text += "?s=104";
                  if (!textBox.Text.Contains("s=104") && textBox.Text.Contains("?"))
                    textBox.Text += "&s=104";
                }
                else
                {
                  if (textBox.ForeColor == System.Drawing.Color.Black)
                    this.LogBox.AppendText(Environment.NewLine + "Ваша версия программы не работает с Avito, вам нужно докупить Avito, для покупки зайдите на сайт программы: http://avtoringer.ru/forum/viewtopic.php?f=2&t=2" + Environment.NewLine);
                  textBox.BackColor = System.Drawing.Color.Black;
                  textBox.ForeColor = System.Drawing.Color.White;
                }
              }
              else if (textBox.Text.ToLower().Contains("youla"))
              {
                if ((StatSetClass.perm & StatSetClass.Permissions.Demka) == StatSetClass.Permissions.Demka || (StatSetClass.perm & StatSetClass.Permissions.Youla) == StatSetClass.Permissions.Youla)
                {
                  textBox.Text = textBox.Text.Replace("youla.io", "youla.ru");
                  if (!textBox.Text.ToLower().Contains("auto.youla"))
                  {
                    if (!textBox.Text.Contains("attributes[sort_field]=date_published") && !textBox.Text.Contains("?"))
                      textBox.Text += "?attributes[sort_field]=date_published";
                    if (!textBox.Text.Contains("attributes[sort_field]=date_published") && textBox.Text.Contains("?"))
                      textBox.Text += "&attributes[sort_field]=date_published";
                  }
                }
                else
                {
                  if (textBox.ForeColor == System.Drawing.Color.Black)
                    this.LogBox.AppendText(Environment.NewLine + "Ваша версия программы не работает с youla, вам нужно докупить youla, для покупки зайдите на сайт программы: http://avtoringer.ru/forum/viewtopic.php?f=2&t=2" + Environment.NewLine);
                  textBox.BackColor = System.Drawing.Color.Black;
                  textBox.ForeColor = System.Drawing.Color.White;
                }
              }
              else if (textBox.Text.ToLower().Contains("olx"))
              {
                if ((StatSetClass.perm & StatSetClass.Permissions.Demka) == StatSetClass.Permissions.Demka || (StatSetClass.perm & StatSetClass.Permissions.Olx) == StatSetClass.Permissions.Olx)
                {
                  if (textBox.Text.ToLower().Contains("olx"))
                    ;
                }
                else
                {
                  if (textBox.ForeColor == System.Drawing.Color.Black)
                    this.LogBox.AppendText(Environment.NewLine + "Ваша версия программы не работает с olx, вам нужно докупить olx, для покупки зайдите на сайт программы: http://avtoringer.ru/forum/viewtopic.php?f=2&t=2" + Environment.NewLine);
                  textBox.BackColor = System.Drawing.Color.Black;
                  textBox.ForeColor = System.Drawing.Color.White;
                }
              }
              else if (textBox.Text.ToLower().Contains("auto.ru"))
              {
                if ((StatSetClass.perm & StatSetClass.Permissions.Demka) == StatSetClass.Permissions.Demka || (StatSetClass.perm & StatSetClass.Permissions.Autoru) == StatSetClass.Permissions.Autoru)
                {
                  if (textBox.Text.ToLower().Contains("auto.ru"))
                    ;
                }
                else
                {
                  if (textBox.ForeColor == System.Drawing.Color.Black)
                    this.LogBox.AppendText(Environment.NewLine + "Ваша версия программы не работает с auto.ru, вам нужно докупить auto.ru, для покупки зайдите на сайт программы: http://avtoringer.ru/forum/viewtopic.php?f=2&t=2" + Environment.NewLine);
                  textBox.BackColor = System.Drawing.Color.Black;
                  textBox.ForeColor = System.Drawing.Color.White;
                }
              }
              else if (!textBox.Text.ToLower().Contains("avito") || !textBox.Text.ToLower().Contains("youla") || !textBox.Text.ToLower().Contains("olx") || !textBox.Text.ToLower().Contains("auto.ru"))
              {
                if (textBox.ForeColor == System.Drawing.Color.Black)
                  this.LogBox.AppendText(Environment.NewLine + "Похоже на то, что вы вводите неверный поисковый запрос, данная программа работает только с сайтами avito или youla или olx " + Environment.NewLine);
                textBox.BackColor = System.Drawing.Color.Black;
                textBox.ForeColor = System.Drawing.Color.White;
              }
              else
              {
                textBox.BackColor = System.Drawing.Color.White;
                textBox.ForeColor = System.Drawing.Color.Black;
              }
            }
            else
            {
              textBox.BackColor = System.Drawing.Color.White;
              textBox.ForeColor = System.Drawing.Color.Black;
            }
          }
          else
          {
            textBox.BackColor = System.Drawing.Color.White;
            textBox.ForeColor = System.Drawing.Color.Black;
          }
        }
      }
    }

    private void FormCaptionEditor(string newCaption = "AvtoRinger - Мониторинг объявлений в процессе") => this.Invoke((Action) (() => this.Text = newCaption));

    private void startWork()
    {
      if (this.checkBox1.Checked)
      {
        try
        {
          if (this.UrlTextBox1.Text != "")
          {
            this.FormCaptionEditor();
            int CountOfTry;
            for (CountOfTry = 1; this.ListAdvertList[0].Count < 1 && CountOfTry <= 5; CountOfTry++)
            {
              this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + string.Format(" Попытка ({0}) соединения с сервером... ", (object) CountOfTry) + Environment.NewLine)));
              this.ListAdvertList[0] = this.webWorker.GetAdvert(this.UrlTextBox1.Text, this);
              Thread.Sleep(this.rndStart.Next(1000, 3000));
            }
            if (CountOfTry >= 5)
              throw new IndexOutOfRangeException();
            this.StartAdvertsSearch(0);
          }
          else
          {
            int num = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
          }
        }
        catch
        {
          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Не удалось установить связь с сервером " + Environment.NewLine)));
          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Проверьте правильность ввода поисковой ссылки, так же может повлиять плохое соединение с интернетом, либо авито/юла временно ограничило доступ программы с вашего IP, в таком случае либо поменяйте IP, либо выключите программу и запустите заново черзе пол часа. Но в первую очередь проверьте поисковые запросы, лучше скопруйте их заново " + Environment.NewLine)));
        }
      }
      if (this.checkBox2.Checked)
      {
        try
        {
          if (this.UrlTextBox2.Text != "")
          {
            this.FormCaptionEditor();
            int CountOfTry;
            for (CountOfTry = 1; this.ListAdvertList[1].Count < 1 && CountOfTry <= 5; CountOfTry++)
            {
              this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + string.Format(" Попытка ({0}) соединения с сервером... ", (object) CountOfTry) + Environment.NewLine)));
              this.ListAdvertList[1] = this.webWorker.GetAdvert(this.UrlTextBox2.Text, this);
              Thread.Sleep(this.rndStart.Next(1000, 3000));
            }
            if (CountOfTry >= 5)
              throw new IndexOutOfRangeException();
            this.StartAdvertsSearch(1);
          }
          else
          {
            int num = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
          }
        }
        catch
        {
          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Не удалось установить связь с сервером " + Environment.NewLine)));
          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Проверьте правильность ввода поисковой ссылки, так же может повлиять плохое соединение с интернетом, либо авито/юла временно ограничило доступ программы с вашего IP, в таком случае либо поменяйте IP, либо выключите программу и запустите заново черзе пол часа. Но в первую очередь проверьте поисковые запросы, лучше скопруйте их заново " + Environment.NewLine)));
        }
      }
      if (this.checkBox3.Checked)
      {
        if (this.UrlTextBox3.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[2].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[2] = this.webWorker.GetAdvert(this.UrlTextBox3.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(2);
        }
        else
        {
          int num1 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox4.Checked)
      {
        if (this.UrlTextBox4.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[3].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[3] = this.webWorker.GetAdvert(this.UrlTextBox4.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(3);
        }
        else
        {
          int num2 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox5.Checked)
      {
        if (this.UrlTextBox5.Text != "")
        {
          this.FormCaptionEditor();
          this.FormCaptionEditor();
          while (this.ListAdvertList[4].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[4] = this.webWorker.GetAdvert(this.UrlTextBox5.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(4);
        }
        else
        {
          int num3 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox6.Checked)
      {
        if (this.UrlTextBox6.Text != "")
        {
          this.FormCaptionEditor();
          this.FormCaptionEditor();
          while (this.ListAdvertList[5].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[5] = this.webWorker.GetAdvert(this.UrlTextBox6.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(5);
        }
        else
        {
          int num4 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox7.Checked)
      {
        if (this.UrlTextBox7.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[6].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[6] = this.webWorker.GetAdvert(this.UrlTextBox7.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(6);
        }
        else
        {
          int num5 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox8.Checked)
      {
        if (this.UrlTextBox8.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[7].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[7] = this.webWorker.GetAdvert(this.UrlTextBox8.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(7);
        }
        else
        {
          int num6 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox9.Checked)
      {
        if (this.UrlTextBox9.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[8].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[8] = this.webWorker.GetAdvert(this.UrlTextBox9.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(8);
        }
        else
        {
          int num7 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox10.Checked)
      {
        if (this.UrlTextBox10.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[9].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[9] = this.webWorker.GetAdvert(this.UrlTextBox10.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(9);
        }
        else
        {
          int num8 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox11.Checked)
      {
        if (this.UrlTextBox11.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[10].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[10] = this.webWorker.GetAdvert(this.UrlTextBox11.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(10);
        }
        else
        {
          int num9 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox12.Checked)
      {
        if (this.UrlTextBox12.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[11].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[11] = this.webWorker.GetAdvert(this.UrlTextBox12.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(11);
        }
        else
        {
          int num10 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox13.Checked)
      {
        if (this.UrlTextBox13.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[12].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[12] = this.webWorker.GetAdvert(this.UrlTextBox13.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(12);
        }
        else
        {
          int num11 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox14.Checked)
      {
        if (this.UrlTextBox14.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[13].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[13] = this.webWorker.GetAdvert(this.UrlTextBox14.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(13);
        }
        else
        {
          int num12 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox15.Checked)
      {
        if (this.UrlTextBox15.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[14].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[14] = this.webWorker.GetAdvert(this.UrlTextBox15.Text, this);
            Thread.Sleep(5000);
          }
          this.StartAdvertsSearch(14);
        }
        else
        {
          int num13 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      if (this.checkBox16.Checked)
      {
        if (this.UrlTextBox16.Text != "")
        {
          this.FormCaptionEditor();
          while (this.ListAdvertList[15].Count < 1)
          {
            this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToString("HH:mm") + " Попытка соединения с сервером... " + Environment.NewLine)));
            this.ListAdvertList[15] = this.webWorker.GetAdvert(this.UrlTextBox16.Text, this);
            Thread.Sleep(this.rndStart.Next(1000, 3000));
          }
          this.StartAdvertsSearch(15);
        }
        else
        {
          int num14 = (int) MessageBox.Show("Вам нужно заполнить строку поиска", "Обратите внимание");
        }
      }
      this.timer1.Enabled = true;
    }

    private void StartAdvertsSearch(int urlNumber)
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (Adverts1 adverts1 in this.ListAdvertList[urlNumber])
      {
        if (StatSetClass.goalWordsList[urlNumber] != "")
        {
          List<string> list = ((IEnumerable<string>) StatSetClass.goalWordsList[urlNumber].Split('|')).ToList<string>();
          string str1 = "";
          foreach (string str2 in list)
          {
            if (str2 != "" && adverts1.name.ToLower().Contains(str2.ToLower()))
              str1 = str1 + str2 + " ";
          }
          if (str1 != "")
            adverts1.targetWord = str1;
        }
        if (StatSetClass.stopWordsList[urlNumber] != "")
        {
          List<string> list = ((IEnumerable<string>) StatSetClass.stopWordsList[urlNumber].Split('|')).ToList<string>();
          string str1 = "";
          foreach (string str2 in list)
          {
            if (str2 != "" && adverts1.name.ToLower().Contains(str2.ToLower()))
              str1 = str1 + str2 + " ";
          }
          if (str1 != "")
            adverts1.stopWord = str1;
        }
        if (!this.bd.BdReadOneEditor(adverts1.link))
          this.bd.BdWrite(adverts1.name, adverts1.link, adverts1.price, adverts1.date.ToString("dd-MM-yyyy HH:mm"));
        this.ListVirtual.Add(adverts1);
      }
      this.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) => vc2.date.CompareTo(vc1.date)));
      this.AdvertsLitsView.Invoke((Action) (() => this.AdvertsLitsView.Items.Clear()));
      this.AdvertsLitsView.Invoke((Action) (() => this.AdvertsLitsView.VirtualListSize = this.ListVirtual.Count));
      if (this.AdvertsLitsView.VirtualListSize <= 0)
        return;
      this.AdvertsLitsView.Invoke((Action) (() => --this.AdvertsLitsView.VirtualListSize));
      this.AdvertsLitsView.Invoke((Action) (() => ++this.AdvertsLitsView.VirtualListSize));
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      try
      {
        this.TimerThread = new Thread(new ThreadStart(this.TimerWorkStart));
        this.TimerThread.Start();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void TimerWorkStart()
    {
      try
      {
        if (this.checkBox1.Checked)
        {
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox1.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 0);
          Thread.Sleep(this.rndStart.Next(1000, 3000));
        }
        if (this.checkBox2.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox2.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 1);
        }
        if (this.checkBox3.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox3.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 2);
        }
        if (this.checkBox4.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox4.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 3);
        }
        if (this.checkBox5.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox5.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 4);
        }
        if (this.checkBox6.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox6.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 5);
        }
        if (this.checkBox7.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox7.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 6);
        }
        if (this.checkBox8.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox8.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 7);
        }
        if (this.checkBox9.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox9.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 8);
        }
        if (this.checkBox10.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox10.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 9);
        }
        if (this.checkBox11.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox11.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 10);
        }
        if (this.checkBox12.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox12.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 11);
        }
        if (this.checkBox13.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox13.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 12);
        }
        if (this.checkBox14.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox14.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 13);
        }
        if (this.checkBox15.Checked)
        {
          Thread.Sleep(this.rndStart.Next(1000, 3000));
          List<Adverts1> adverts1List = new List<Adverts1>();
          List<Adverts1> advert = this.webWorker.GetAdvert(this.UrlTextBox15.Text, this);
          advert.Sort();
          this.AdvertsFinder(advert, 14);
        }
        if (!this.checkBox16.Checked)
          return;
        Thread.Sleep(this.rndStart.Next(1000, 3000));
        List<Adverts1> adverts1List1 = new List<Adverts1>();
        List<Adverts1> advert1 = this.webWorker.GetAdvert(this.UrlTextBox16.Text, this);
        advert1.Sort();
        this.AdvertsFinder(advert1, 15);
      }
      catch (Exception ex)
      {
        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Ошибка в коде таймера " + ex.ToString() + Environment.NewLine)));
      }
    }

    private void AdvertsFinder(List<Adverts1> AdvertList, int urlNumber)
    {
      try
      {
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        foreach (Adverts1 advert in AdvertList)
        {
          Adverts1 adv = advert;
          bool flag1 = true;
          foreach (Adverts1 adverts1 in this.ListAdvertList[urlNumber])
          {
            if (adv.link.Contains("avito"))
            {
              if (adv.id == adverts1.id)
              {
                flag1 = false;
                break;
              }
            }
            else if (adv.link == adverts1.link)
            {
              flag1 = false;
              break;
            }
          }
          if (flag1)
          {
            if (!StatSetClass.r)
            {
              if (this.counterD > 6)
              {
                Console.Beep(1000, 300);
                Console.Beep(500, 300);
                Console.Beep(1500, 400);
                this.timer1.Enabled = false;
              
                
                Process.Start("https://avtoringer.ru");
                this.StWorkThread.Abort();
                this.StWorkThread.Join();
                this.FormCaptionEditor("AvtoRinger - Мониторинг объявлений остановлен");
                this.timer1.Enabled = false;
                break;
              }
              ++this.counterD;
            }
            if (StatSetClass.stopWordsList[urlNumber] != "")
            {
              List<string> list = ((IEnumerable<string>) StatSetClass.stopWordsList[urlNumber].Split('|')).ToList<string>();
              string str1 = "";
              foreach (string str2 in list)
              {
                if (str2 != "" && adv.name.ToLower().Contains(str2.ToLower()))
                  str1 = str1 + str2 + " ";
              }
              if (str1 != "")
                adv.stopWord = str1;
            }
            if (StatSetClass.goalWordsList[urlNumber] != "")
            {
              List<string> list = ((IEnumerable<string>) StatSetClass.goalWordsList[urlNumber].Split('|')).ToList<string>();
              string str1 = "";
              foreach (string str2 in list)
              {
                if (str2 != "" && adv.name.ToLower().Contains(str2.ToLower()))
                  str1 = str1 + str2 + " ";
              }
              if (str1 != "")
                adv.targetWord = str1;
            }
            this.ListAdvertList[urlNumber].Add(adv);
            Adverts1 adverts1_1 = new Adverts1(adv.name, adv.price, adv.link, adv.date, adv.stopWord, adv.targetWord);
            TimeSpan diff = DateTime.Now - adverts1_1.date;
            if (diff.Minutes < 31)
            {
              bool flag2 = this.bd.BdReadOneEditor(adverts1_1.link);
              if (!flag2 || StatSetClass.RaiseAdverts)
              {
                if (StatSetClass.ptz)
                {
                  if (!flag2)
                    this.bd.BdWrite(adverts1_1.name, adverts1_1.link, adverts1_1.price, adverts1_1.date.ToString("dd-MM-yyyy HH:mm"));
                  if (adverts1_1.link.ToLower().Contains("petrozavodsk"))
                    this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + "Найдено объявление, но оно от компани" + Environment.NewLine);
                  else if (StatSetClass.MagazCheck && adverts1_1.name.Contains("==(") && adverts1_1.name.Contains(")=="))
                  {
                    this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + "Найдено объявление, но оно от компании" + Environment.NewLine)));
                  }
                  else
                  {
                    this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Минуты между текущим временем и датой подачи объявления (дата подачи считается не с момента публикации, а с момента подачи на модерацию): " + diff.Minutes.ToString() + Environment.NewLine)));
                    try
                    {
                      bool flag3 = false;
                      foreach (Adverts1 adverts1_2 in this.ListVirtual)
                      {
                        if (adverts1_2.link == adverts1_1.link)
                          flag3 = true;
                      }
                      if (!flag3)
                      {
                        this.ListVirtual.Add(adverts1_1);
                        this.AdvertsLitsView.Items.Clear();
                        this.AdvertsLitsView.Invoke((Action) (() => this.AdvertsLitsView.VirtualListSize = this.ListVirtual.Count));
                      }
                    }
                    catch (Exception ex)
                    {
                      if (StatSetClass.l)
                        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": " + ex.Message + Environment.NewLine)));
                    }
                    if (this.AdvertsLitsView.VirtualListSize > 0)
                    {
                      try
                      {
                        this.AdvertsLitsView.Invoke((Action) (() => --this.AdvertsLitsView.VirtualListSize));
                        this.AdvertsLitsView.Invoke((Action) (() => ++this.AdvertsLitsView.VirtualListSize));
                      }
                      catch (Exception ex)
                      {
                        if (StatSetClass.l)
                          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + "++++++++++++++++++++++++++++++++++++++++++++++++++++ Сбой пополнения списка, на работу программы не влияет" + Environment.NewLine + ex.Message + Environment.NewLine)));
                      }
                    }
                    this.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) => vc2.date.CompareTo(vc1.date)));
                    if (adverts1_1.stopWord.Replace(" ", "") == "")
                    {
                      if (StatSetClass.goalWordsList[urlNumber] == "")
                      {
                        this.BrowserOpen(adv);
                        if (this.INIF.KeyExists("CHECKBOX", "Beep") && this.INIF.ReadINI("CHECKBOX", "Beep") == "Checked")
                          this.beeep();
                        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Найдено новое объявление" + Environment.NewLine)));
                        if (!StatSetClass.notify)
                          ;
                        if (StatSetClass.telgSend)
                        {
                          if (this.tgToken != "")
                          {
                            try
                            {
                              Thread.Sleep(StatSetClass.timeOut);
                              this.Telegramm_SendMessage(this.tgToken, adv.name + "\nЦена: " + adv.price + "\n" + adv.link);
                              this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Отправляем сообщение в телеграм" + Environment.NewLine + Environment.NewLine)));
                            }
                            catch (Exception ex)
                            {
                              this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " На вашем компьютере закрыт порт 8000, либо перегружен интернет канал, откройте, чтобы была возможность отправлять сообщения в телеграм " + ex.Message + Environment.NewLine)));
                            }
                          }
                        }
                      }
                      else if (!(adverts1_1.targetWord.Replace(" ", "") == ""))
                      {
                        this.BrowserOpen(adv);
                        if (this.INIF.KeyExists("CHECKBOX", "Beep") && this.INIF.ReadINI("CHECKBOX", "Beep") == "Checked")
                          this.beeep();
                        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Найдено новое объявление" + Environment.NewLine)));
                        if (!StatSetClass.notify)
                          ;
                        if (StatSetClass.telgSend && this.tgToken != "")
                        {
                          Thread.Sleep(StatSetClass.timeOut);
                          this.Telegramm_SendMessage(this.tgToken, adv.name + "\nЦена: " + adv.price + "\n" + adv.link);
                          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Отправляем сообщение телеграммой" + Environment.NewLine)));
                        }
                      }
                      else
                        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Найдено новое объявление, но оно без ключевых слов" + Environment.NewLine + adv.name + Environment.NewLine)));
                    }
                    else
                      this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Найдено новое объявление, но оно со стоп словом" + Environment.NewLine + adv.name + Environment.NewLine)));
                  }
                }
                else
                {
                  if (!flag2)
                    this.bd.BdWrite(adverts1_1.name, adverts1_1.link, adverts1_1.price, adverts1_1.date.ToString("dd-MM-yyyy HH:mm"));
                  if (StatSetClass.MagazCheck && adverts1_1.name.Contains("==(") && adverts1_1.name.Contains(")=="))
                  {
                    this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + "Найдено объявление, но оно от компании" + Environment.NewLine)));
                  }
                  else
                  {
                    this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Минуты между текущим временем и датой подачи объявления (дата подачи считается не с момента публикации, а с момента подачи на модерацию): " + diff.Minutes.ToString() + Environment.NewLine)));
                    try
                    {
                      bool flag3 = false;
                      foreach (Adverts1 adverts1_2 in this.ListVirtual)
                      {
                        if (adverts1_2.link == adverts1_1.link)
                          flag3 = true;
                      }
                      if (!flag3)
                      {
                        this.ListVirtual.Add(adverts1_1);
                        this.AdvertsLitsView.Items.Clear();
                        this.AdvertsLitsView.Invoke((Action) (() => this.AdvertsLitsView.VirtualListSize = this.ListVirtual.Count));
                      }
                    }
                    catch (Exception ex)
                    {
                      if (StatSetClass.l)
                        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": " + ex.Message + Environment.NewLine)));
                    }
                    if (this.AdvertsLitsView.VirtualListSize > 0)
                    {
                      try
                      {
                        this.AdvertsLitsView.Invoke((Action) (() => --this.AdvertsLitsView.VirtualListSize));
                        this.AdvertsLitsView.Invoke((Action) (() => ++this.AdvertsLitsView.VirtualListSize));
                      }
                      catch (Exception ex)
                      {
                        if (StatSetClass.l)
                          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + "++++++++++++++++++++++++++++++++++++++++++++++++++++ Сбой пополнения списка, на работу программы не влияет" + Environment.NewLine + ex.Message + Environment.NewLine)));
                      }
                    }
                    this.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) => vc2.date.CompareTo(vc1.date)));
                    if (adverts1_1.stopWord.Replace(" ", "") == "")
                    {
                      if (StatSetClass.goalWordsList[urlNumber] == "")
                      {
                        this.BrowserOpen(adv);
                        if (this.INIF.KeyExists("CHECKBOX", "Beep") && this.INIF.ReadINI("CHECKBOX", "Beep") == "Checked")
                          this.beeep();
                        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Найдено новое объявление" + Environment.NewLine)));
                        if (!StatSetClass.notify)
                          ;
                        if (StatSetClass.telgSend)
                        {
                          if (this.tgToken != "")
                          {
                            try
                            {
                              Thread.Sleep(StatSetClass.timeOut);
                              this.Telegramm_SendMessage(this.tgToken, adv.name + "\nЦена: " + adv.price + "\n" + adv.link);
                              this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Отправляем сообщение в телеграм" + Environment.NewLine + Environment.NewLine)));
                            }
                            catch (Exception ex)
                            {
                              this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " На вашем компьютере закрыт порт 8000, либо перегружен интернет канал, откройте, чтобы была возможность отправлять сообщения в телеграм " + ex.Message + Environment.NewLine)));
                            }
                          }
                        }
                      }
                      else if (!(adverts1_1.targetWord.Replace(" ", "") == ""))
                      {
                        this.BrowserOpen(adv);
                        if (this.INIF.KeyExists("CHECKBOX", "Beep") && this.INIF.ReadINI("CHECKBOX", "Beep") == "Checked")
                          this.beeep();
                        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Найдено новое объявление" + Environment.NewLine)));
                        if (!StatSetClass.notify)
                          ;
                        if (StatSetClass.telgSend && this.tgToken != "")
                        {
                          Thread.Sleep(StatSetClass.timeOut);
                          this.Telegramm_SendMessage(this.tgToken, adv.name + "\nЦена: " + adv.price + "\n" + adv.link);
                          this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Отправляем сообщение телеграммой" + Environment.NewLine)));
                        }
                      }
                      else
                        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Найдено новое объявление, но оно без ключевых слов" + Environment.NewLine + adv.name + Environment.NewLine)));
                    }
                    else
                      this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Найдено новое объявление, но оно со стоп словом" + Environment.NewLine + adv.name + Environment.NewLine)));
                  }
                }
              }
              else
              {
                this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Найдено новое объявление, но оно уже есть в базе, значит просто поднято" + Environment.NewLine + adv.name + Environment.NewLine)));
                this.StartButton.Text = string.Format("Поднятых объявлений {0}", (object) ++this.upAdverts);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        if (!StatSetClass.l)
          return;
        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ex.Message + Environment.NewLine)));
      }
    }

    private void beeep()
    {
      try
      {
        new SoundPlayer("beep.wav").Play();
      }
      catch
      {
        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Файл должен быть в формате beep.wav, и лежать в папке с программой" + Environment.NewLine)));
      }
    }

    private void BrowserOpen(Adverts1 adv)
    {
      try
      {
        if (!StatSetClass.bwsrOpen)
          return;
        if (StatSetClass.browser != "")
          Process.Start(StatSetClass.browser, adv.link);
        else
          Process.Start(adv.link);
      }
      catch
      {
        Process.Start(adv.link);
      }
    }

    private void label20_Click(object sender, EventArgs e) => Process.Start("http://avtoringer.ru");

    private void SetButton_Click(object sender, EventArgs e) => new SetForm(this).Show((IWin32Window) this);

    private void button1_Click_1(object sender, EventArgs e)
    {
    }

    private void button2_Click_1(object sender, EventArgs e)
    {
    }

    private void AdvertsLitsView_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void AdvertsLitsView_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      try
      {
        Process.Start(this.AdvertsLitsView.Items[this.AdvertsLitsView.SelectedIndices[0]].SubItems[3].Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void UrlTextBox_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this.INIF.WriteINI("SEARCH", "UrlBox", this.UrlTextBox1.Text);
        this.UrlColorerAvito();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void button1_Click_2(object sender, EventArgs e)
    {
      try
      {
        if (this.StWorkThread == null)
          return;
        this.StWorkThread.Abort();
        this.StWorkThread.Join();
        this.FormCaptionEditor("AvtoRinger - Мониторинг объявлений остановлен");
        this.timer1.Enabled = false;
        this.StartButton.Enabled = true;
        ((Control) sender).Enabled = false;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void AdvertsLitsView_SelectedIndexChanged_1(object sender, EventArgs e)
    {
    }

    private void AdvertsLitsView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      try
      {
        this.AdvertsLitsView.Sorting = SortOrder.Descending;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void AdvertsLitsView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
      try
      {
        int width = this.AdvertsLitsView.Columns[e.ColumnIndex].Width;
        this.INIF.WriteINI("COLUMNS", e.ColumnIndex.ToString(), width.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void PrintItem(XmlElement item, int indent = 0)
    {
      Console.Write(new string('\t', indent) + item.LocalName + " ");
      foreach (object childNode in item.ChildNodes)
      {
        if (childNode is XmlElement xmlElement2)
        {
          Console.WriteLine();
          this.PrintItem(xmlElement2, indent + 1);
        }
        if (childNode is XmlText xmlText2)
          this.progProxyVersion.Add(xmlText2.InnerText);
      }
    }

    public void CheckUpdates()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load("http://avtoringer.ru/Avito/configAvito.xml");
        this.PrintItem(xmlDocument.DocumentElement);
        Version version = new Version(this.progProxyVersion[0]);
        StatSetClass.appVersion = version.ToString();
        
       
        try
        {
          new WebClient().DownloadStringAsync(new Uri("https://avtoringer.ru/counter/"));
        }
        catch
        {
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    public void CheckProxy()
    {
      try
      {
        string str1 = this.progProxyVersion[1];
        string str2 = this.progProxyVersion[2];
        string str3 = this.progProxyVersion[3];
        string str4 = this.progProxyVersion[4];
        StatSetClass.proxyIp = str1;
        StatSetClass.proxyPort = str2;
        StatSetClass.proxyLogin = str3;
        StatSetClass.proxyPassword = str4;
        if (!this.INIF.KeyExists("USERPROXY", "ip") || !this.INIF.KeyExists("USERPROXY", "port") || !(this.INIF.ReadINI("USERPROXY", "ip") != ""))
          return;
        if (this.INIF.KeyExists("USERPROXY", "login") && this.INIF.KeyExists("USERPROXY", "pass"))
        {
          StatSetClass.proxyLogin = this.INIF.ReadINI("USERPROXY", "login");
          StatSetClass.proxyPassword = this.INIF.ReadINI("USERPROXY", "pass");
        }
        else
        {
          StatSetClass.proxyLogin = "";
          StatSetClass.proxyPassword = "";
        }
        StatSetClass.proxyIp = this.INIF.ReadINI("USERPROXY", "ip");
        StatSetClass.proxyPort = this.INIF.ReadINI("USERPROXY", "port");
      }
      catch (Exception ex)
      {
        if (!StatSetClass.r)
          return;
        this.LogBox.Invoke((Action) (() => this.LogBox.AppendText("Не удается получить данные Proxy, попробуйте перезапустить программу, отправка в телеграмм без прокси не работает " + Environment.NewLine)));
      }
    }

    public void ButtonMakerAdverts()
    {
      try
      {
        this.ButtonList.Clear();
        foreach (Control control in (ArrangedElementCollection) this.Controls)
        {
          if (control.Name == "AvitoButton" | control.Name == "YoulaButton" | control.Name == "OlxButton" | control.Name == "DromButton" | control.Name == "AutoruButton" | control.Name == "CianButton")
            control.Dispose();
        }
        Button button1 = new Button();
        Button button2 = new Button();
        Button button3 = new Button();
        Button button4 = new Button();
        Button button5 = new Button();
        Button button6 = new Button();
        button1.Name = "AvitoButton";
        button2.Name = "YoulaButton";
        button3.Name = "OlxButton";
        button4.Name = "DromButton";
        button5.Name = "AutoruButton";
        button6.Name = "CianButton";
        button1.Text = "Avito =>";
        button2.Text = "Youla =>";
        button3.Text = "Olx =>";
        button4.Text = "Drom =>";
        button5.Text = "Autoru =>";
        button6.Text = "Cian =>";
        button1.Tag = (object) "https://avito.ru";
        button2.Tag = (object) "https://youla.ru";
        button3.Tag = (object) "https://olx.ua";
        button4.Tag = (object) "https://drom.ru";
        button5.Tag = (object) "https://auto.ru";
        button6.Tag = (object) "https://cian.ru";
        if ((StatSetClass.perm & StatSetClass.Permissions.Demka) == StatSetClass.Permissions.Demka)
        {
          this.ButtonList.Add(button1);
          this.ButtonList.Add(button2);
          this.ButtonList.Add(button3);
        }
        if ((StatSetClass.perm & StatSetClass.Permissions.Avito) == StatSetClass.Permissions.Avito)
          this.ButtonList.Add(button1);
        if ((StatSetClass.perm & StatSetClass.Permissions.Youla) == StatSetClass.Permissions.Youla)
          this.ButtonList.Add(button2);
        if ((StatSetClass.perm & StatSetClass.Permissions.Olx) == StatSetClass.Permissions.Olx)
          this.ButtonList.Add(button3);
        if ((StatSetClass.perm & StatSetClass.Permissions.Drom) == StatSetClass.Permissions.Drom)
          this.ButtonList.Add(button4);
        if ((StatSetClass.perm & StatSetClass.Permissions.Autoru) == StatSetClass.Permissions.Autoru)
          this.ButtonList.Add(button5);
        if ((StatSetClass.perm & StatSetClass.Permissions.Cian) == StatSetClass.Permissions.Cian)
          this.ButtonList.Add(button6);
        if (!this.INIF.KeyExists("CHECKBOX", "Telegramm"))
          ;
        int num = 10;
        foreach (Button button7 in this.ButtonList)
        {
          button7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
          button7.Click += new EventHandler(this.ButtonSiteOpener);
          button7.Left = num;
          button7.Top = this.AdvertsLitsView.Height + 10;
          button7.Width = 100;
          button7.Height = 28;
          new ToolTip().SetToolTip((Control) button7, string.Format("Кнопка просто открывает страницу {0}, сделана для удобства", button7.Tag));
          Bitmap www = Resources.www;
          button7.Image = (Image) www;
          button7.ImageAlign = ContentAlignment.MiddleLeft;
          this.Controls.Add((Control) button7);
          num += button7.Width;
        }
        this.SetButton.Left = this.ButtonList[this.ButtonList.Count - 1].Left + 100;
        this.ShowStopWordsButton.Left = this.SetButton.Left + 115;
        this.ObjectiveWords.Left = this.ShowStopWordsButton.Left + 115;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void ButtonSiteOpener(object sender, EventArgs e)
    {
      try
      {
        Process.Start(((Control) sender).Tag.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {


           // this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
           // System.Diagnostics.Process p = new System.Diagnostics.Process();
            //p.StartInfo.FileName = @"D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe";
           // p.Start();

      try
      {
        if (StatSetClass.r)
          this.label20.Text = "О программе";
        this.bd.CreateBd();
        for (int index = 0; index <= 15; ++index)
        {
          StatSetClass.stopWordsList.Add("");
          StatSetClass.goalWordsList.Add("");
        }
        if (this.INIF.KeyExists("SETCLASS", "Browser"))
          StatSetClass.browser = this.INIF.ReadINI("SETCLASS", "Browser");
        if (this.INIF.KeyExists("CHECKBOX", "BrowserOpen"))
          StatSetClass.bwsrOpen = this.INIF.ReadINI("CHECKBOX", "BrowserOpen") == "Checked";
        if (this.INIF.KeyExists("CHECKBOX", "Notify"))
          StatSetClass.notify = this.INIF.ReadINI("CHECKBOX", "Notify") == "Checked";
        if (this.INIF.KeyExists("CHECKBOX", "dynamic"))
          StatSetClass.dynamicCheckBox = this.INIF.ReadINI("CHECKBOX", "dynamic") == "Checked";
        if (this.INIF.KeyExists("CHECKBOX", "MagazCheck"))
          StatSetClass.MagazCheck = this.INIF.ReadINI("CHECKBOX", "MagazCheck") == "Checked";
        if (this.INIF.KeyExists("CHECKBOX", "AvitoDostavka"))
          StatSetClass.AvitoDostavka = this.INIF.ReadINI("CHECKBOX", "AvitoDostavka") == "Checked";
        if (this.INIF.KeyExists("CHECKBOX", "Telegramm"))
          StatSetClass.telgSend = this.INIF.ReadINI("CHECKBOX", "Telegramm") == "Checked";
        for (int index = 0; index <= 15; ++index)
          this.ListAdvertList.Add(new List<Adverts1>());
        if (StatSetClass.r)
        {
          StatSetClass.ACTkey = Reg.getHDD();
          int num = Reg.deHash(this.INIF.ReadINI("ACTIVATION", StatSetClass.ACTkey), StatSetClass.ACTkey);
          if (num == 1)
          {
            this.Enabled = false;
            new Reg(this).Show();
            StatSetClass.perm = (StatSetClass.Permissions) num;
          }
          else
            StatSetClass.perm = (StatSetClass.Permissions) num;
        }
        this.ButtonMakerAdverts();
        StatSetClass.allstopWords = "";
        if (this.INIF.KeyExists("STOPWORDS", "URL1"))
        {
          StatSetClass.stopWordsList[0] = this.INIF.ReadINI("STOPWORDS", "URL1");
          if (StatSetClass.stopWordsList[0] != "")
            StatSetClass.allstopWords = StatSetClass.stopWordsList[0] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL2"))
        {
          StatSetClass.stopWordsList[1] = this.INIF.ReadINI("STOPWORDS", "URL2");
          if (StatSetClass.stopWordsList[1] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[1] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL3"))
        {
          StatSetClass.stopWordsList[2] = this.INIF.ReadINI("STOPWORDS", "URL3");
          if (StatSetClass.stopWordsList[2] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[2] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL4"))
        {
          StatSetClass.stopWordsList[3] = this.INIF.ReadINI("STOPWORDS", "URL4");
          if (StatSetClass.stopWordsList[3] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[3] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL5"))
        {
          StatSetClass.stopWordsList[4] = this.INIF.ReadINI("STOPWORDS", "URL5");
          if (StatSetClass.stopWordsList[4] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[4] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL6"))
        {
          StatSetClass.stopWordsList[5] = this.INIF.ReadINI("STOPWORDS", "URL6");
          if (StatSetClass.stopWordsList[5] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[5] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL7"))
        {
          StatSetClass.stopWordsList[6] = this.INIF.ReadINI("STOPWORDS", "URL7");
          if (StatSetClass.stopWordsList[6] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[6] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL8"))
        {
          StatSetClass.stopWordsList[7] = this.INIF.ReadINI("STOPWORDS", "URL8");
          if (StatSetClass.stopWordsList[7] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[7] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL9"))
        {
          StatSetClass.stopWordsList[8] = this.INIF.ReadINI("STOPWORDS", "URL9");
          if (StatSetClass.stopWordsList[8] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[8] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL10"))
        {
          StatSetClass.stopWordsList[9] = this.INIF.ReadINI("STOPWORDS", "URL10");
          if (StatSetClass.stopWordsList[9] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[9] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL11"))
        {
          StatSetClass.stopWordsList[10] = this.INIF.ReadINI("STOPWORDS", "URL11");
          if (StatSetClass.stopWordsList[10] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[10] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL12"))
        {
          StatSetClass.stopWordsList[11] = this.INIF.ReadINI("STOPWORDS", "URL12");
          if (StatSetClass.stopWordsList[11] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[11] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL13"))
        {
          StatSetClass.stopWordsList[12] = this.INIF.ReadINI("STOPWORDS", "URL13");
          if (StatSetClass.stopWordsList[12] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[12] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL14"))
        {
          StatSetClass.stopWordsList[13] = this.INIF.ReadINI("STOPWORDS", "URL14");
          if (StatSetClass.stopWordsList[13] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[13] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL15"))
        {
          StatSetClass.stopWordsList[14] = this.INIF.ReadINI("STOPWORDS", "URL15");
          if (StatSetClass.stopWordsList[14] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[14] + "|";
        }
        if (this.INIF.KeyExists("STOPWORDS", "URL16"))
        {
          StatSetClass.stopWordsList[15] = this.INIF.ReadINI("STOPWORDS", "URL16");
          if (StatSetClass.stopWordsList[15] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[15] + "|";
        }
        StatSetClass.allgoalWords = "";
        if (this.INIF.KeyExists("GOALWORDS", "URL1"))
        {
          StatSetClass.goalWordsList[0] = this.INIF.ReadINI("GOALWORDS", "URL1");
          if (StatSetClass.goalWordsList[0] != "")
            StatSetClass.allgoalWords = StatSetClass.goalWordsList[0] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL2"))
        {
          StatSetClass.goalWordsList[1] = this.INIF.ReadINI("GOALWORDS", "URL2");
          if (StatSetClass.goalWordsList[1] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[1] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL3"))
        {
          StatSetClass.goalWordsList[2] = this.INIF.ReadINI("GOALWORDS", "URL3");
          if (StatSetClass.goalWordsList[2] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[2] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL4"))
        {
          StatSetClass.goalWordsList[3] = this.INIF.ReadINI("GOALWORDS", "URL4");
          if (StatSetClass.goalWordsList[3] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[3] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL5"))
        {
          StatSetClass.goalWordsList[4] = this.INIF.ReadINI("GOALWORDS", "URL5");
          if (StatSetClass.goalWordsList[4] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[4] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL6"))
        {
          StatSetClass.goalWordsList[5] = this.INIF.ReadINI("GOALWORDS", "URL6");
          if (StatSetClass.goalWordsList[5] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[5] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL7"))
        {
          StatSetClass.goalWordsList[6] = this.INIF.ReadINI("GOALWORDS", "URL7");
          if (StatSetClass.goalWordsList[6] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[6] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL8"))
        {
          StatSetClass.goalWordsList[7] = this.INIF.ReadINI("GOALWORDS", "URL8");
          if (StatSetClass.goalWordsList[7] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[7] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL9"))
        {
          StatSetClass.goalWordsList[8] = this.INIF.ReadINI("GOALWORDS", "URL9");
          if (StatSetClass.goalWordsList[8] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[8] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL10"))
        {
          StatSetClass.goalWordsList[9] = this.INIF.ReadINI("GOALWORDS", "URL10");
          if (StatSetClass.goalWordsList[9] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[9] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL11"))
        {
          StatSetClass.goalWordsList[10] = this.INIF.ReadINI("GOALWORDS", "URL11");
          if (StatSetClass.goalWordsList[10] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[10] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL12"))
        {
          StatSetClass.goalWordsList[11] = this.INIF.ReadINI("GOALWORDS", "URL12");
          if (StatSetClass.goalWordsList[11] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[11] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL13"))
        {
          StatSetClass.goalWordsList[12] = this.INIF.ReadINI("GOALWORDS", "URL13");
          if (StatSetClass.goalWordsList[12] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[12] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL14"))
        {
          StatSetClass.goalWordsList[13] = this.INIF.ReadINI("GOALWORDS", "URL14");
          if (StatSetClass.goalWordsList[13] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[13] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL15"))
        {
          StatSetClass.goalWordsList[14] = this.INIF.ReadINI("GOALWORDS", "URL15");
          if (StatSetClass.goalWordsList[14] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[14] + "|";
        }
        if (this.INIF.KeyExists("GOALWORDS", "URL16"))
        {
          StatSetClass.goalWordsList[15] = this.INIF.ReadINI("GOALWORDS", "URL16");
          if (StatSetClass.goalWordsList[15] != "")
            StatSetClass.allgoalWords = StatSetClass.allgoalWords + StatSetClass.goalWordsList[15] + "|";
        }
        if (StatSetClass.r)
          new Thread(new ThreadStart(this.CheckUpdates)).Start();
        if (this.INIF.KeyExists("SETCLASS", "TInterval"))
          StatSetClass.timeOut = int.Parse(this.INIF.ReadINI("SETCLASS", "TInterval"));
        if (this.INIF.KeyExists("SETCLASS", "TIntervalTime"))
        {
          StatSetClass.TimerInterval = int.Parse(this.INIF.ReadINI("SETCLASS", "TIntervalTime"));
          this.timer1.Interval = StatSetClass.TimerInterval;
        }
        if (this.INIF.KeyExists("CHECKBOX", "SearchNow") && this.INIF.ReadINI("CHECKBOX", "SearchNow") == "Checked")
        {
          StatSetClass.allstopWords = "";
          if (StatSetClass.stopWordsList[0] != "")
            StatSetClass.allstopWords = StatSetClass.stopWordsList[0] + "|";
          if (StatSetClass.stopWordsList[1] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[1] + "|";
          if (StatSetClass.stopWordsList[2] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[2] + "|";
          if (StatSetClass.stopWordsList[3] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[3] + "|";
          if (StatSetClass.stopWordsList[4] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[4] + "|";
          if (StatSetClass.stopWordsList[5] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[5] + "|";
          if (StatSetClass.stopWordsList[6] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[6] + "|";
          if (StatSetClass.stopWordsList[7] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[7] + "|";
          if (StatSetClass.stopWordsList[8] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[8] + "|";
          if (StatSetClass.stopWordsList[9] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[9] + "|";
          if (StatSetClass.stopWordsList[10] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[10] + "|";
          if (StatSetClass.stopWordsList[11] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[11] + "|";
          if (StatSetClass.stopWordsList[12] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[12] + "|";
          if (StatSetClass.stopWordsList[13] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[13] + "|";
          if (StatSetClass.stopWordsList[14] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[14] + "|";
          if (StatSetClass.stopWordsList[15] != "")
            StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[15] + "|";
          if (this.UrlTextBox1.Text == "" && this.UrlTextBox2.Text == "" && (this.UrlTextBox3.Text == "" && this.UrlTextBox4.Text == "") && (this.UrlTextBox5.Text == "" && this.UrlTextBox6.Text == "" && (this.UrlTextBox7.Text == "" && this.UrlTextBox8.Text == "")) && (this.UrlTextBox9.Text == "" && this.UrlTextBox10.Text == "" && (this.UrlTextBox11.Text == "" && this.UrlTextBox12.Text == "") && (this.UrlTextBox13.Text == "" && this.UrlTextBox14.Text == "" && this.UrlTextBox15.Text == "")) && this.UrlTextBox16.Text == "")
          {
            int num = (int) MessageBox.Show("Не указано ни одной строки для поиска");
          }
          else
          {
            this.FormCaptionEditor();
            new Thread(new ThreadStart(this.startWork)).Start();
            this.timer1.Enabled = true;
          }
        }
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "1") && this.INIF.ReadINI("MAINFORMCHECKBOX", "1") == "Checked")
          this.checkBox1.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "2") && this.INIF.ReadINI("MAINFORMCHECKBOX", "2") == "Checked")
          this.checkBox2.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "3") && this.INIF.ReadINI("MAINFORMCHECKBOX", "3") == "Checked")
          this.checkBox3.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "4") && this.INIF.ReadINI("MAINFORMCHECKBOX", "4") == "Checked")
          this.checkBox4.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "5") && this.INIF.ReadINI("MAINFORMCHECKBOX", "5") == "Checked")
          this.checkBox5.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "6") && this.INIF.ReadINI("MAINFORMCHECKBOX", "6") == "Checked")
          this.checkBox6.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "7") && this.INIF.ReadINI("MAINFORMCHECKBOX", "7") == "Checked")
          this.checkBox7.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "8") && this.INIF.ReadINI("MAINFORMCHECKBOX", "8") == "Checked")
          this.checkBox8.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "9") && this.INIF.ReadINI("MAINFORMCHECKBOX", "9") == "Checked")
          this.checkBox9.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "10") && this.INIF.ReadINI("MAINFORMCHECKBOX", "10") == "Checked")
          this.checkBox10.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "11") && this.INIF.ReadINI("MAINFORMCHECKBOX", "11") == "Checked")
          this.checkBox11.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "12") && this.INIF.ReadINI("MAINFORMCHECKBOX", "12") == "Checked")
          this.checkBox12.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "13") && this.INIF.ReadINI("MAINFORMCHECKBOX", "13") == "Checked")
          this.checkBox13.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "14") && this.INIF.ReadINI("MAINFORMCHECKBOX", "14") == "Checked")
          this.checkBox14.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "15") && this.INIF.ReadINI("MAINFORMCHECKBOX", "15") == "Checked")
          this.checkBox15.Checked = true;
        if (this.INIF.KeyExists("MAINFORMCHECKBOX", "16") && this.INIF.ReadINI("MAINFORMCHECKBOX", "16") == "Checked")
          this.checkBox16.Checked = true;
        this.WindowState = FormWindowState.Maximized;
        this.UrlColorerAvito();
        if (this.INIF.KeyExists("CHECKBOX", "RaiseAdverts"))
          StatSetClass.RaiseAdverts = this.INIF.ReadINI("CHECKBOX", "RaiseAdverts") == "Checked";
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      this.auto_read_settings();
    }

    private void AdvertsLitsView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      try
      {
        if (e.ItemIndex < 0 || e.ItemIndex >= this.ListVirtual.Count)
          return;
        e.Item = new ListViewItem(this.ListVirtual[e.ItemIndex].date.ToString("dd-MM-yyyy HH:mm"));
        e.Item.SubItems.Add(this.ListVirtual[e.ItemIndex].name);
        e.Item.SubItems.Add(this.ListVirtual[e.ItemIndex].price);
        e.Item.SubItems.Add(this.ListVirtual[e.ItemIndex].link);
        e.Item.SubItems.Add(this.ListVirtual[e.ItemIndex].stopWord);
        e.Item.SubItems.Add(this.ListVirtual[e.ItemIndex].targetWord);
        if (this.ListVirtual[e.ItemIndex].targetWord.Replace(" ", "") != "")
          e.Item.BackColor = System.Drawing.Color.Yellow;
        if (this.ListVirtual[e.ItemIndex].stopWord.Replace(" ", "") != "")
          e.Item.BackColor = System.Drawing.Color.Red;
        if (this.ListVirtual[e.ItemIndex].name.Contains("==(") && this.ListVirtual[e.ItemIndex].name.Contains(")=="))
          e.Item.Font = new Font("Arial", 10f, FontStyle.Bold);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start("https://avito.ru");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "1", this.checkBox1.CheckState.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "2", this.checkBox2.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox3_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "3", this.checkBox3.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox4_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "4", this.checkBox4.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox5_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "5", this.checkBox5.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox6_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "6", this.checkBox6.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox7_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "7", this.checkBox7.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox8_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "8", this.checkBox8.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox9_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "9", this.checkBox9.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox10_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "10", this.checkBox10.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox11_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "11", this.checkBox11.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox12_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "12", this.checkBox12.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox13_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "13", this.checkBox13.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox14_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "14", this.checkBox14.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox15_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "15", this.checkBox15.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void checkBox16_CheckedChanged(object sender, EventArgs e)
    {
      if (StatSetClass.r)
      {
        this.INIF.WriteINI("MAINFORMCHECKBOX", "16", this.checkBox16.CheckState.ToString());
      }
      else
      {
        CheckBox checkBox = (CheckBox) sender;
        if (checkBox.Checked)
        {
          checkBox.Checked = false;
        }
        else
        {
          int num = (int) MessageBox.Show("Мультипоиск доступен позднее");
        }
      }
    }

    private void UrlTextBox2_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox2", this.UrlTextBox2.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox3_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox3", this.UrlTextBox3.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox4_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox4", this.UrlTextBox4.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox5_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox5", this.UrlTextBox5.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox6_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox6", this.UrlTextBox6.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox7_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox7", this.UrlTextBox7.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox8_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox8", this.UrlTextBox8.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox9_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox9", this.UrlTextBox9.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox10_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox10", this.UrlTextBox10.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox11_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox11", this.UrlTextBox11.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox12_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox12", this.UrlTextBox12.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox13_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox13", this.UrlTextBox13.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox14_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox14", this.UrlTextBox14.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox15_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox15", this.UrlTextBox15.Text);
      this.UrlColorerAvito();
    }

    private void UrlTextBox16_TextChanged(object sender, EventArgs e)
    {
      this.INIF.WriteINI("SEARCH", "UrlBox16", this.UrlTextBox16.Text);
      this.UrlColorerAvito();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void ShowStopWordsButton_Click(object sender, EventArgs e) => new StopWords().Show((IWin32Window) this);

    private void ObjectiveWords_Click(object sender, EventArgs e) => new GoalWords().Show((IWin32Window) this);

    private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
    {
      this.Show();
      this.WindowState = FormWindowState.Normal;
      this.notifyIcon1.Visible = false;
    }

    private void Form1_ResizeEnd(object sender, EventArgs e)
    {
    }

    private void Form1_Resize(object sender, EventArgs e)
    {
    }

    private void button2_Click_3(object sender, EventArgs e)
    {
      try
      {
        Process.Start("https://youla.ru");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
    }

    private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
    {
      try
      {
        Process.Start(((NotifyIcon) sender).Tag.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void AdvertsLitsView_MouseClick(object sender, MouseEventArgs e)
    {
    }

    private void BrowserOpenChekBox_CheckedChanged(object sender, EventArgs e)
    {
      StatSetClass.bwsrOpen = this.BrowserOpenChekBox.Checked;
      this.INIF.WriteINI("CHECKBOX", "BrowserOpen", this.BrowserOpenChekBox.CheckState.ToString());
    }

    private void BeepCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      StatSetClass.notify = this.BeepCheckBox.Checked;
      this.INIF.WriteINI("CHECKBOX", "Beep", this.BeepCheckBox.CheckState.ToString());
    }

    private void MagazCheck_CheckedChanged(object sender, EventArgs e)
    {
      StatSetClass.MagazCheck = this.MagazCheck.Checked;
      this.INIF.WriteINI("CHECKBOX", "MagazCheck", this.MagazCheck.CheckState.ToString());
    }

    private void RaiseAdvertsCheck_CheckedChanged(object sender, EventArgs e)
    {
      StatSetClass.RaiseAdverts = this.RaiseAdvertsCheck.Checked;
      this.INIF.WriteINI("CHECKBOX", "RaiseAdverts", this.RaiseAdvertsCheck.CheckState.ToString());
    }

    private void TelegrammCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      StatSetClass.telgSend = this.TelegrammCheckBox.Checked;
      this.INIF.WriteINI("CHECKBOX", "Telegramm", this.TelegrammCheckBox.CheckState.ToString());
    }

    private void AvitoDostavkaCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      StatSetClass.AvitoDostavka = this.AvitoDostavkaCheckBox.Checked;
      this.INIF.WriteINI("CHECKBOX", "AvitoDostavka", this.AvitoDostavkaCheckBox.CheckState.ToString());
    }

    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      if (!StatSetClass.r)
      {
        this.timer1.Interval = (int) this.numericUpDown1.Value * 1000;
        if (this.numericUpDown1.Value < 120M)
        {
          this.numericUpDown1.Value = 120M;
       
        }
      }
      else
      {
        this.timer1.Interval = (int) this.numericUpDown1.Value * 1000;
        if (!this.numUpdownWarning && this.numericUpDown1.Value < 30M)
        {
          int num = (int) MessageBox.Show("При уменьшении интервала ниже 30 секунд, авито может забанить за большое количество запросов", "Обязательно к прочтению", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.numUpdownWarning = true;
        }
      }
      StatSetClass.TimerInterval = (int) this.numericUpDown1.Value;
      this.INIF.WriteINI("SETCLASS", "TIntervalTime", this.numericUpDown1.Value.ToString());
    }

    private void label21_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox1.Text != ""))
          return;
        Process.Start(this.UrlTextBox1.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label22_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox2.Text != ""))
          return;
        Process.Start(this.UrlTextBox2.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label23_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox3.Text != ""))
          return;
        Process.Start(this.UrlTextBox3.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label24_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox4.Text != ""))
          return;
        Process.Start(this.UrlTextBox4.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label25_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox5.Text != ""))
          return;
        Process.Start(this.UrlTextBox5.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label26_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox6.Text != ""))
          return;
        Process.Start(this.UrlTextBox6.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label27_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox7.Text != ""))
          return;
        Process.Start(this.UrlTextBox7.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label28_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox8.Text != ""))
          return;
        Process.Start(this.UrlTextBox8.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label29_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox9.Text != ""))
          return;
        Process.Start(this.UrlTextBox9.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label30_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox10.Text != ""))
          return;
        Process.Start(this.UrlTextBox10.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label31_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox11.Text != ""))
          return;
        Process.Start(this.UrlTextBox11.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label32_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox12.Text != ""))
          return;
        Process.Start(this.UrlTextBox12.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label33_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox13.Text != ""))
          return;
        Process.Start(this.UrlTextBox13.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label34_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox14.Text != ""))
          return;
        Process.Start(this.UrlTextBox14.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label35_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox15.Text != ""))
          return;
        Process.Start(this.UrlTextBox15.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label36_Click(object sender, EventArgs e)
    {
      try
      {
        if (!(this.UrlTextBox16.Text != ""))
          return;
        Process.Start(this.UrlTextBox16.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void label21_MouseEnter(object sender, EventArgs e)
    {
    }

    private void label21_MouseLeave(object sender, EventArgs e)
    {
    }

    private void lbEnter(object sender, EventArgs e)
    {
      Label label = sender as Label;
      label.BackColor = System.Drawing.Color.Blue;
      label.ForeColor = System.Drawing.Color.White;
    }

    private void lbLeave(object sender, EventArgs e)
    {
      Label label = sender as Label;
      label.BackColor = System.Drawing.Color.FromArgb(215, 228, 242);
      label.ForeColor = System.Drawing.Color.Black;
    }

    private void pictureBox1_MouseEnter(object sender, EventArgs e)
    {
    }

    private void GetAdvertInfoButton_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.AdvertsLitsView.SelectedIndices.Count > 0)
        {
          string text = this.AdvertsLitsView.Items[this.AdvertsLitsView.SelectedIndices[0]].SubItems[3].Text;
          if (text.ToLower().Contains("youla.ru"))
          {
            int num = (int) MessageBox.Show("К сожалению данная функция поддерживается только на Avito");
          }
          else
          {
            this.GetAdvertInfoButton.Enabled = false;
            this.GetAdvertTimer.Enabled = true;
            _1Advert adevertInfo = this.htmlWorker.GetAdevertInfo(text);
            this.AdvertPictureBox.Load("https:" + adevertInfo.ImageUrl);
            this.AdvertInfoTBox.Text = adevertInfo.Description;
          }
        }
        else
        {
          int num1 = (int) MessageBox.Show("Не выбрано ни одного объявления для отображения информации");
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возможно сервер Авито заблокировал доступ к запросам из программы, постарайтесь в следующий раз делать паузу между открием информации об объявлении, хотя бы 15 секунд. А пока можно выключить программу и подождать 30 минут, до разбана               " + Environment.NewLine + Environment.NewLine + ex.Message, "Дощелкались");
      }
    }

    private void GetAdvertTimer_Tick(object sender, EventArgs e)
    {
      if (this.GetAdvertInfoButton.Enabled)
        return;
      this.GetAdvertInfoButton.Enabled = true;
      this.GetAdvertTimer.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.label1 = new Label();
      this.label2 = new Label();
      this.BrandComboBox = new ComboBox();
      this.label3 = new Label();
      this.ModelTextBox = new TextBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.YearFromTextBox = new TextBox();
      this.YearToTextBox = new TextBox();
      this.KmMaxTextBox = new TextBox();
      this.KmMinTextBox = new TextBox();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label12 = new Label();
      this.label13 = new Label();
      this.label14 = new Label();
      this.TransmissionListBox = new ListBox();
      this.BodyTypeListBox = new ListBox();
      this.label15 = new Label();
      this.PrivListBox = new ListBox();
      this.label16 = new Label();
      this.PriceTextBoxMax = new TextBox();
      this.PriceTextBoxMin = new TextBox();
      this.label17 = new Label();
      this.label18 = new Label();
      this.SaveAndValidateButton = new Button();
      this.UrlTextBox1 = new TextBox();
      this.TownComboBox = new ComboBox();
      this.LogBox = new TextBox();
      this.LitrScrollMin = new HScrollBar();
      this.LitrScrollMax = new HScrollBar();
      this.LminLabel = new Label();
      this.LmaxLabel = new Label();
      this.label11 = new Label();
      this.DvTypeListBox = new ListBox();
      this.label19 = new Label();
      this.StartButton = new Button();
      this.AdvertsLitsView = new ListView();
      this.Data = new ColumnHeader();
      this.Zagolovok = new ColumnHeader();
      this.Price = new ColumnHeader();
      this.Link = new ColumnHeader();
      this.stopWords = new ColumnHeader();
      this.targetWords = new ColumnHeader();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.label20 = new Label();
      this.SetButton = new Button();
      this.button1 = new Button();
      this.UrlTextBox2 = new TextBox();
      this.UrlTextBox3 = new TextBox();
      this.UrlTextBox4 = new TextBox();
      this.UrlTextBox5 = new TextBox();
      this.UrlTextBox6 = new TextBox();
      this.UrlTextBox7 = new TextBox();
      this.UrlTextBox8 = new TextBox();
      this.UrlTextBox9 = new TextBox();
      this.UrlTextBox10 = new TextBox();
      this.UrlTextBox11 = new TextBox();
      this.checkBox1 = new CheckBox();
      this.checkBox2 = new CheckBox();
      this.checkBox3 = new CheckBox();
      this.checkBox4 = new CheckBox();
      this.checkBox5 = new CheckBox();
      this.checkBox6 = new CheckBox();
      this.checkBox7 = new CheckBox();
      this.checkBox8 = new CheckBox();
      this.checkBox9 = new CheckBox();
      this.checkBox10 = new CheckBox();
      this.checkBox11 = new CheckBox();
      this.checkBox16 = new CheckBox();
      this.checkBox15 = new CheckBox();
      this.checkBox14 = new CheckBox();
      this.checkBox13 = new CheckBox();
      this.checkBox12 = new CheckBox();
      this.UrlTextBox16 = new TextBox();
      this.UrlTextBox15 = new TextBox();
      this.UrlTextBox14 = new TextBox();
      this.UrlTextBox13 = new TextBox();
      this.UrlTextBox12 = new TextBox();
      this.ShowStopWordsButton = new Button();
      this.ObjectiveWords = new Button();
      this.label21 = new Label();
      this.label22 = new Label();
      this.label23 = new Label();
      this.label24 = new Label();
      this.label25 = new Label();
      this.label26 = new Label();
      this.label27 = new Label();
      this.label28 = new Label();
      this.label29 = new Label();
      this.label30 = new Label();
      this.label31 = new Label();
      this.label32 = new Label();
      this.label33 = new Label();
      this.label34 = new Label();
      this.label35 = new Label();
      this.label36 = new Label();
      this.notifyIcon1 = new NotifyIcon(this.components);
      this.BrowserOpenChekBox = new CheckBox();
      this.BeepCheckBox = new CheckBox();
      this.TelegrammCheckBox = new CheckBox();
      this.numericUpDown1 = new NumericUpDown();
      this.label37 = new Label();
      this.RaiseAdvertsCheck = new CheckBox();
      this.MagazCheck = new CheckBox();
      this.AvitoDostavkaCheckBox = new CheckBox();
      this.pictureBox1 = new PictureBox();
      this.pictureBox2 = new PictureBox();
      this.pictureBox3 = new PictureBox();
      this.pictureBox4 = new PictureBox();
      this.pictureBox5 = new PictureBox();
      this.pictureBox6 = new PictureBox();
      this.pictureBox7 = new PictureBox();
      this.pictureBox8 = new PictureBox();
      this.pictureBox9 = new PictureBox();
      this.pictureBox10 = new PictureBox();
      this.pictureBox11 = new PictureBox();
      this.pictureBox12 = new PictureBox();
      this.pictureBox13 = new PictureBox();
      this.pictureBox14 = new PictureBox();
      this.pictureBox15 = new PictureBox();
      this.pictureBox16 = new PictureBox();
      this.groupBox1 = new GroupBox();
      this.GetAdvertInfoButton = new Button();
      this.label38 = new Label();
      this.AdvertInfoTBox = new TextBox();
      this.AdvertPictureBox = new PictureBox();
      this.GetAdvertTimer = new System.Windows.Forms.Timer(this.components);
      this.numericUpDown1.BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      ((ISupportInitialize) this.pictureBox3).BeginInit();
      ((ISupportInitialize) this.pictureBox4).BeginInit();
      ((ISupportInitialize) this.pictureBox5).BeginInit();
      ((ISupportInitialize) this.pictureBox6).BeginInit();
      ((ISupportInitialize) this.pictureBox7).BeginInit();
      ((ISupportInitialize) this.pictureBox8).BeginInit();
      ((ISupportInitialize) this.pictureBox9).BeginInit();
      ((ISupportInitialize) this.pictureBox10).BeginInit();
      ((ISupportInitialize) this.pictureBox11).BeginInit();
      ((ISupportInitialize) this.pictureBox12).BeginInit();
      ((ISupportInitialize) this.pictureBox13).BeginInit();
      ((ISupportInitialize) this.pictureBox14).BeginInit();
      ((ISupportInitialize) this.pictureBox15).BeginInit();
      ((ISupportInitialize) this.pictureBox16).BeginInit();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.AdvertPictureBox).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3191, 54);
      this.label1.Margin = new Padding(9, 0, 9, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(37, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Город";
      this.label1.Visible = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(3184, 192);
      this.label2.Margin = new Padding(9, 0, 9, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(40, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Марка";
      this.label2.Visible = false;
      this.BrandComboBox.AutoCompleteCustomSource.AddRange(new string[152]
      {
        "Все",
        "Иномарки",
        "Отечественные",
        "ac",
        "acura",
        "alfa_romeo",
        "alpina",
        "aro",
        "asia",
        "aston_martin",
        "audi",
        "bajaj",
        "baw",
        "bentley",
        "bmw",
        "brilliance",
        "bufori",
        "bugatti",
        "buick",
        "byd",
        "cadillac",
        "caterham",
        "changan",
        "changfeng",
        "chery",
        "chevrolet",
        "chrysler",
        "citroen",
        "dacia",
        "dadi",
        "daewoo",
        "daihatsu",
        "daimler",
        "datsun",
        "derways",
        "dodge",
        "dong_feng",
        "doninvest",
        "eagle",
        "ecomotors",
        "faw",
        "ferrari",
        "fiat",
        "ford",
        "foton",
        "gac",
        "geely",
        "genesis",
        "geo",
        "gmc",
        "great_wall",
        "hafei",
        "haima",
        "haval",
        "hawtai",
        "honda",
        "huanghai",
        "hummer",
        "hyundai",
        "infiniti",
        "iran_khodro",
        "isuzu",
        "jac",
        "jaguar",
        "jeep",
        "jinbei",
        "jmc",
        "koenigsegg",
        "lamborghini",
        "lancia",
        "land_rover",
        "landwind",
        "ldv",
        "lexus",
        "lifan",
        "lincoln",
        "lotus",
        "luxgen",
        "mahindra",
        "marussia",
        "maserati",
        "maybach",
        "mazda",
        "mclaren",
        "mercedes-benz",
        "mercury",
        "metrocab",
        "mg",
        "mini",
        "mitsubishi",
        "mitsuoka",
        "morgan",
        "morris",
        "nissan",
        "noble",
        "oldsmobile",
        "opel",
        "pagani",
        "peugeot",
        "plymouth",
        "pontiac",
        "porsche",
        "proton",
        "puch",
        "ravon",
        "renault",
        "rolls_royce",
        "ronart",
        "rover",
        "saab",
        "saleen",
        "saturn",
        "scion",
        "seat",
        "shuanghuan",
        "skoda",
        "sma",
        "smart",
        "spyker",
        "ssangyong",
        "subaru",
        "suzuki",
        "talbot",
        "tata",
        "tesla",
        "tianma",
        "tianye",
        "toyota",
        "trabant",
        "volkswagen",
        "volvo",
        "vortex",
        "wartburg",
        "westfield",
        "wiesmann",
        "xin_kai",
        "zibar",
        "zotye",
        "zx",
        "Ваз(Лада)",
        "ВИС",
        "ГАЗ",
        "ЗАЗ",
        "ЗИЛ",
        "ИЖ",
        "ЛуАЗ",
        "Москвич",
        "РАФ",
        "СМЗ",
        "ТагАЗ",
        "УАЗ",
        "Другая"
      });
      this.BrandComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.BrandComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
      this.BrandComboBox.FormattingEnabled = true;
      this.BrandComboBox.Items.AddRange(new object[58]
      {
        (object) "Все",
        (object) "Иномарки",
        (object) "Отечественные",
        (object) "alfa_romeo",
        (object) "aston_martin",
        (object) "audi",
        (object) "bmw",
        (object) "cadillac",
        (object) "chery",
        (object) "chevrolet",
        (object) "citroen",
        (object) "dacia",
        (object) "daewoo",
        (object) "daihatsu",
        (object) "datsun",
        (object) "dodge",
        (object) "fiat",
        (object) "ford",
        (object) "geely",
        (object) "great_wall",
        (object) "honda",
        (object) "hyundai",
        (object) "infiniti",
        (object) "jeep",
        (object) "land_rover",
        (object) "lexus",
        (object) "lifan",
        (object) "mazda",
        (object) "mercedes-benz",
        (object) "mitsubishi",
        (object) "nissan",
        (object) "opel",
        (object) "peugeot",
        (object) "porsche",
        (object) "renault",
        (object) "rover",
        (object) "saab",
        (object) "seat",
        (object) "skoda",
        (object) "ssangyong",
        (object) "subaru",
        (object) "suzuki",
        (object) "toyota",
        (object) "volkswagen",
        (object) "volvo",
        (object) "Ваз(Лада)",
        (object) "ВИС",
        (object) "ГАЗ",
        (object) "ЗАЗ",
        (object) "ЗИЛ",
        (object) "ИЖ",
        (object) "ЛуАЗ",
        (object) "Москвич",
        (object) "РАФ",
        (object) "СМЗ",
        (object) "ТагАЗ",
        (object) "УАЗ",
        (object) "Другая"
      });
      this.BrandComboBox.Location = new Point(3295, 186);
      this.BrandComboBox.Margin = new Padding(9, 6, 9, 6);
      this.BrandComboBox.Name = "BrandComboBox";
      this.BrandComboBox.Size = new Size(473, 21);
      this.BrandComboBox.TabIndex = 3;
      this.BrandComboBox.Text = "Все";
      this.BrandComboBox.Visible = false;
      this.BrandComboBox.SelectedIndexChanged += new EventHandler(this.BrandComboBox_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3169, 244);
      this.label3.Margin = new Padding(9, 0, 9, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(46, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Модель";
      this.label3.Visible = false;
      this.ModelTextBox.Location = new Point(3295, 239);
      this.ModelTextBox.Margin = new Padding(9, 6, 9, 6);
      this.ModelTextBox.Name = "ModelTextBox";
      this.ModelTextBox.Size = new Size(473, 19);
      this.ModelTextBox.TabIndex = 5;
      this.ModelTextBox.Visible = false;
      this.ModelTextBox.KeyPress += new KeyPressEventHandler(this.ModelTextBox_KeyPress);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(3220, 471);
      this.label4.Margin = new Padding(9, 0, 9, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(25, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Год";
      this.label4.Visible = false;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(3319, 471);
      this.label5.Margin = new Padding(9, 0, 9, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(13, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "с";
      this.label5.Visible = false;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3541, 471);
      this.label6.Margin = new Padding(9, 0, 9, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(19, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "по";
      this.label6.Visible = false;
      this.YearFromTextBox.Location = new Point(3365, 466);
      this.YearFromTextBox.Margin = new Padding(9, 6, 9, 6);
      this.YearFromTextBox.Name = "YearFromTextBox";
      this.YearFromTextBox.Size = new Size(159, 19);
      this.YearFromTextBox.TabIndex = 9;
      this.YearFromTextBox.Visible = false;
      this.YearFromTextBox.KeyPress += new KeyPressEventHandler(this.YearFromTextBox_KeyPress);
      this.YearToTextBox.Location = new Point(3600, 466);
      this.YearToTextBox.Margin = new Padding(9, 6, 9, 6);
      this.YearToTextBox.Name = "YearToTextBox";
      this.YearToTextBox.Size = new Size(165, 19);
      this.YearToTextBox.TabIndex = 10;
      this.YearToTextBox.Visible = false;
      this.YearToTextBox.KeyPress += new KeyPressEventHandler(this.YearToTextBox_KeyPress);
      this.KmMaxTextBox.Location = new Point(3600, 514);
      this.KmMaxTextBox.Margin = new Padding(9, 6, 9, 6);
      this.KmMaxTextBox.Name = "KmMaxTextBox";
      this.KmMaxTextBox.Size = new Size(165, 19);
      this.KmMaxTextBox.TabIndex = 15;
      this.KmMaxTextBox.Visible = false;
      this.KmMaxTextBox.KeyPress += new KeyPressEventHandler(this.textBox5_KeyPress);
      this.KmMinTextBox.Location = new Point(3365, 514);
      this.KmMinTextBox.Margin = new Padding(9, 6, 9, 6);
      this.KmMinTextBox.Name = "KmMinTextBox";
      this.KmMinTextBox.Size = new Size(159, 19);
      this.KmMinTextBox.TabIndex = 14;
      this.KmMinTextBox.Visible = false;
      this.KmMinTextBox.KeyPress += new KeyPressEventHandler(this.textBox6_KeyPress);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(3541, 519);
      this.label7.Margin = new Padding(9, 0, 9, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(19, 13);
      this.label7.TabIndex = 13;
      this.label7.Text = "до";
      this.label7.Visible = false;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3308, 519);
      this.label8.Margin = new Padding(9, 0, 9, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(18, 13);
      this.label8.TabIndex = 12;
      this.label8.Text = "от";
      this.label8.Visible = false;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(3115, 519);
      this.label9.Margin = new Padding(9, 0, 9, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(69, 13);
      this.label9.TabIndex = 11;
      this.label9.Text = "Пробег тыс.";
      this.label9.Visible = false;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(3159, 572);
      this.label10.Margin = new Padding(9, 0, 9, 0);
      this.label10.Name = "label10";
      this.label10.Size = new Size(50, 13);
      this.label10.TabIndex = 16;
      this.label10.Text = "Коробка";
      this.label10.Visible = false;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(3124, 848);
      this.label12.Margin = new Padding(9, 0, 9, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(65, 13);
      this.label12.TabIndex = 20;
      this.label12.Text = "Объем лит.";
      this.label12.Visible = false;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(3300, 894);
      this.label13.Margin = new Padding(9, 0, 9, 0);
      this.label13.Name = "label13";
      this.label13.Size = new Size(19, 13);
      this.label13.TabIndex = 22;
      this.label13.Text = "до";
      this.label13.Visible = false;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(3300, 848);
      this.label14.Margin = new Padding(9, 0, 9, 0);
      this.label14.Name = "label14";
      this.label14.Size = new Size(18, 13);
      this.label14.TabIndex = 21;
      this.label14.Text = "от";
      this.label14.Visible = false;
      this.TransmissionListBox.FormattingEnabled = true;
      this.TransmissionListBox.Items.AddRange(new object[4]
      {
        (object) "Механика",
        (object) "Автомат",
        (object) "Робот",
        (object) "Вариатор"
      });
      this.TransmissionListBox.Location = new Point(3295, 572);
      this.TransmissionListBox.Margin = new Padding(9, 6, 9, 6);
      this.TransmissionListBox.Name = "TransmissionListBox";
      this.TransmissionListBox.SelectionMode = SelectionMode.MultiExtended;
      this.TransmissionListBox.Size = new Size(473, 56);
      this.TransmissionListBox.TabIndex = 25;
      this.TransmissionListBox.Visible = false;
      this.BodyTypeListBox.FormattingEnabled = true;
      this.BodyTypeListBox.Items.AddRange(new object[11]
      {
        (object) "Седан",
        (object) "Хетчбэк",
        (object) "Универсал",
        (object) "Внедорожник",
        (object) "Кабриолет",
        (object) "Купе",
        (object) "Лимузин",
        (object) "Минивэн",
        (object) "Пикап",
        (object) "Фургон",
        (object) "Микроавтобус"
      });
      this.BodyTypeListBox.Location = new Point(3295, 290);
      this.BodyTypeListBox.Margin = new Padding(9, 6, 9, 6);
      this.BodyTypeListBox.Name = "BodyTypeListBox";
      this.BodyTypeListBox.SelectionMode = SelectionMode.MultiExtended;
      this.BodyTypeListBox.Size = new Size(473, 121);
      this.BodyTypeListBox.TabIndex = 27;
      this.BodyTypeListBox.Visible = false;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(3172, 946);
      this.label15.Margin = new Padding(9, 0, 9, 0);
      this.label15.Name = "label15";
      this.label15.Size = new Size(45, 13);
      this.label15.TabIndex = 28;
      this.label15.Text = "Привод";
      this.label15.Visible = false;
      this.PrivListBox.FormattingEnabled = true;
      this.PrivListBox.Items.AddRange(new object[3]
      {
        (object) "Передний",
        (object) "Задний",
        (object) "Полный"
      });
      this.PrivListBox.Location = new Point(3295, 951);
      this.PrivListBox.Margin = new Padding(9, 6, 9, 6);
      this.PrivListBox.Name = "PrivListBox";
      this.PrivListBox.SelectionMode = SelectionMode.MultiExtended;
      this.PrivListBox.Size = new Size(473, 43);
      this.PrivListBox.TabIndex = 29;
      this.PrivListBox.Visible = false;
      this.label16.AutoSize = true;
      this.label16.Location = new Point(3200, 1050);
      this.label16.Margin = new Padding(9, 0, 9, 0);
      this.label16.Name = "label16";
      this.label16.Size = new Size(33, 13);
      this.label16.TabIndex = 30;
      this.label16.Text = "Цена";
      this.label16.Visible = false;
      this.PriceTextBoxMax.Location = new Point(3600, 1049);
      this.PriceTextBoxMax.Margin = new Padding(9, 6, 9, 6);
      this.PriceTextBoxMax.Name = "PriceTextBoxMax";
      this.PriceTextBoxMax.Size = new Size(165, 19);
      this.PriceTextBoxMax.TabIndex = 34;
      this.PriceTextBoxMax.Visible = false;
      this.PriceTextBoxMax.KeyPress += new KeyPressEventHandler(this.textBox9_KeyPress);
      this.PriceTextBoxMin.Location = new Point(3356, 1044);
      this.PriceTextBoxMin.Margin = new Padding(9, 6, 9, 6);
      this.PriceTextBoxMin.Name = "PriceTextBoxMin";
      this.PriceTextBoxMin.Size = new Size(159, 19);
      this.PriceTextBoxMin.TabIndex = 33;
      this.PriceTextBoxMin.Visible = false;
      this.PriceTextBoxMin.KeyPress += new KeyPressEventHandler(this.textBox10_KeyPress);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(3536, 1052);
      this.label17.Margin = new Padding(9, 0, 9, 0);
      this.label17.Name = "label17";
      this.label17.Size = new Size(19, 13);
      this.label17.TabIndex = 32;
      this.label17.Text = "до";
      this.label17.Visible = false;
      this.label18.AutoSize = true;
      this.label18.Location = new Point(3300, 1050);
      this.label18.Margin = new Padding(9, 0, 9, 0);
      this.label18.Name = "label18";
      this.label18.Size = new Size(18, 13);
      this.label18.TabIndex = 31;
      this.label18.Text = "от";
      this.label18.Visible = false;
      this.SaveAndValidateButton.Location = new Point(3131, 1102);
      this.SaveAndValidateButton.Margin = new Padding(9, 6, 9, 6);
      this.SaveAndValidateButton.Name = "SaveAndValidateButton";
      this.SaveAndValidateButton.Size = new Size(641, 42);
      this.SaveAndValidateButton.TabIndex = 35;
      this.SaveAndValidateButton.Text = "Сохранить и проверить поиск";
      this.SaveAndValidateButton.UseVisualStyleBackColor = true;
      this.SaveAndValidateButton.Visible = false;
      this.SaveAndValidateButton.Click += new EventHandler(this.button1_Click);
      this.UrlTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox1.BackColor = System.Drawing.Color.White;
      this.UrlTextBox1.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox1.Location = new Point(912, 7);
      this.UrlTextBox1.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox1.Name = "UrlTextBox1";
      this.UrlTextBox1.Size = new Size(407, 19);
      this.UrlTextBox1.TabIndex = 36;
      this.UrlTextBox1.Text = "https://www.avito.ru/moskva/bytovaya_elektronika?s=104";
      this.UrlTextBox1.TextChanged += new EventHandler(this.UrlTextBox_TextChanged);
      this.TownComboBox.FormattingEnabled = true;
      this.TownComboBox.Items.AddRange(new object[17]
      {
        (object) "rossiya",
        (object) "moskva",
        (object) "sankt-peterburg",
        (object) "novosibirsk",
        (object) "ekaterinburg",
        (object) "nizhniy_novgorod",
        (object) "kazan",
        (object) "chelyabinsk",
        (object) "omsk",
        (object) "samara",
        (object) "rostov-na-donu",
        (object) "ufa",
        (object) "krasnoyarsk",
        (object) "perm",
        (object) "petrozavodsk",
        (object) "voronezh",
        (object) "volgograd"
      });
      this.TownComboBox.Location = new Point(3295, 48);
      this.TownComboBox.Margin = new Padding(9, 6, 9, 6);
      this.TownComboBox.Name = "TownComboBox";
      this.TownComboBox.Size = new Size(473, 21);
      this.TownComboBox.TabIndex = 37;
      this.TownComboBox.Text = "rossiya";
      this.TownComboBox.Visible = false;
      this.LogBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.LogBox.BackColor = SystemColors.Control;
      this.LogBox.Location = new Point(869, 374);
      this.LogBox.Margin = new Padding(9, 6, 9, 6);
      this.LogBox.Multiline = true;
      this.LogBox.Name = "LogBox";
      this.LogBox.ScrollBars = ScrollBars.Vertical;
      this.LogBox.Size = new Size(471, 178);
      this.LogBox.TabIndex = 42;
      this.LitrScrollMin.Location = new Point(3356, 848);
      this.LitrScrollMin.Maximum = 70;
      this.LitrScrollMin.Minimum = 6;
      this.LitrScrollMin.Name = "LitrScrollMin";
      this.LitrScrollMin.Size = new Size(361, 17);
      this.LitrScrollMin.TabIndex = 43;
      this.LitrScrollMin.Value = 6;
      this.LitrScrollMin.Visible = false;
      this.LitrScrollMin.Scroll += new ScrollEventHandler(this.LitrScrollMin_Scroll);
      this.LitrScrollMax.Location = new Point(3356, 894);
      this.LitrScrollMax.Maximum = 70;
      this.LitrScrollMax.Minimum = 6;
      this.LitrScrollMax.Name = "LitrScrollMax";
      this.LitrScrollMax.Size = new Size(361, 17);
      this.LitrScrollMax.TabIndex = 44;
      this.LitrScrollMax.Value = 61;
      this.LitrScrollMax.Visible = false;
      this.LitrScrollMax.Scroll += new ScrollEventHandler(this.LitrScrollMax_Scroll);
      this.LminLabel.AutoSize = true;
      this.LminLabel.Location = new Point(3743, 852);
      this.LminLabel.Margin = new Padding(9, 0, 9, 0);
      this.LminLabel.Name = "LminLabel";
      this.LminLabel.Size = new Size(13, 13);
      this.LminLabel.TabIndex = 45;
      this.LminLabel.Text = "0";
      this.LminLabel.Visible = false;
      this.LmaxLabel.AutoSize = true;
      this.LmaxLabel.Location = new Point(3721, 897);
      this.LmaxLabel.Margin = new Padding(9, 0, 9, 0);
      this.LmaxLabel.Name = "LmaxLabel";
      this.LmaxLabel.Size = new Size(22, 13);
      this.LmaxLabel.TabIndex = 46;
      this.LmaxLabel.Text = "> 6";
      this.LmaxLabel.Visible = false;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(3113, 695);
      this.label11.Margin = new Padding(9, 0, 9, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(70, 13);
      this.label11.TabIndex = 47;
      this.label11.Text = "Тип Движка";
      this.label11.Visible = false;
      this.DvTypeListBox.FormattingEnabled = true;
      this.DvTypeListBox.Items.AddRange(new object[5]
      {
        (object) "Бензин",
        (object) "Дизель",
        (object) "Гибрид",
        (object) "Электро",
        (object) "Газ"
      });
      this.DvTypeListBox.Location = new Point(3291, 695);
      this.DvTypeListBox.Margin = new Padding(9, 6, 9, 6);
      this.DvTypeListBox.Name = "DvTypeListBox";
      this.DvTypeListBox.SelectionMode = SelectionMode.MultiExtended;
      this.DvTypeListBox.Size = new Size(475, 108);
      this.DvTypeListBox.TabIndex = 48;
      this.DvTypeListBox.Visible = false;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(3180, 290);
      this.label19.Margin = new Padding(9, 0, 9, 0);
      this.label19.Name = "label19";
      this.label19.Size = new Size(37, 13);
      this.label19.TabIndex = 49;
      this.label19.Text = "Кузов";
      this.label19.Visible = false;
      this.StartButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.StartButton.Image = (Image) componentResourceManager.GetObject("StartButton.Image");
      this.StartButton.ImageAlign = ContentAlignment.MiddleLeft;
      this.StartButton.Location = new Point(869, 343);
      this.StartButton.Margin = new Padding(9, 6, 9, 6);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new Size(239, 28);
      this.StartButton.TabIndex = 50;
      this.StartButton.Text = "Старт";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new EventHandler(this.button2_Click);
      this.AdvertsLitsView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.AdvertsLitsView.Columns.AddRange(new ColumnHeader[6]
      {
        this.Data,
        this.Zagolovok,
        this.Price,
        this.Link,
        this.stopWords,
        this.targetWords
      });
      this.AdvertsLitsView.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.AdvertsLitsView.FullRowSelect = true;
      this.AdvertsLitsView.GridLines = true;
      this.AdvertsLitsView.HideSelection = false;
      this.AdvertsLitsView.Location = new Point(8, 7);
      this.AdvertsLitsView.Margin = new Padding(9, 6, 9, 6);
      this.AdvertsLitsView.MultiSelect = false;
      this.AdvertsLitsView.Name = "AdvertsLitsView";
      this.AdvertsLitsView.Size = new Size(857, 626);
      this.AdvertsLitsView.TabIndex = 1;
      this.AdvertsLitsView.UseCompatibleStateImageBehavior = false;
      this.AdvertsLitsView.View = View.Details;
      this.AdvertsLitsView.VirtualMode = true;
      this.AdvertsLitsView.ColumnClick += new ColumnClickEventHandler(this.AdvertsLitsView_ColumnClick);
      this.AdvertsLitsView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.AdvertsLitsView_ColumnWidthChanged);
      this.AdvertsLitsView.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.AdvertsLitsView_RetrieveVirtualItem);
      this.AdvertsLitsView.SelectedIndexChanged += new EventHandler(this.AdvertsLitsView_SelectedIndexChanged_1);
      this.AdvertsLitsView.MouseClick += new MouseEventHandler(this.AdvertsLitsView_MouseClick);
      this.AdvertsLitsView.MouseDoubleClick += new MouseEventHandler(this.AdvertsLitsView_MouseDoubleClick);
      this.Data.Text = "Добавлено в программу";
      this.Data.Width = 144;
      this.Zagolovok.Text = "Заголовок объявления";
      this.Zagolovok.Width = 405;
      this.Price.Text = "Цена";
      this.Price.Width = 128;
      this.Link.Text = "Ссылка";
      this.Link.Width = 100;
      this.stopWords.Text = "Cтоп слова";
      this.targetWords.Text = "Целевые слова";
      this.timer1.Interval = 120000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.label20.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label20.Cursor = Cursors.Hand;
      this.label20.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 204);
      this.label20.ForeColor = SystemColors.Highlight;
      this.label20.Location = new Point(1129, 651);
      this.label20.Margin = new Padding(9, 0, 9, 0);
      this.label20.Name = "label20";
      this.label20.Size = new Size(212, 13);
      this.label20.TabIndex = 52;
      this.label20.Text = "AvtoRinger - О программе /Купить";
      this.label20.TextAlign = ContentAlignment.MiddleRight;
      this.label20.Click += new EventHandler(this.label20_Click);
      this.SetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.SetButton.Image = (Image) componentResourceManager.GetObject("SetButton.Image");
      this.SetButton.ImageAlign = ContentAlignment.MiddleLeft;
      this.SetButton.Location = new Point(632, 636);
      this.SetButton.Margin = new Padding(9, 6, 9, 6);
      this.SetButton.Name = "SetButton";
      this.SetButton.Size = new Size(115, 28);
      this.SetButton.TabIndex = 53;
      this.SetButton.Text = "Настройки";
      this.SetButton.UseVisualStyleBackColor = true;
      this.SetButton.Click += new EventHandler(this.SetButton_Click);
      this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.button1.Image = (Image) componentResourceManager.GetObject("button1.Image");
      this.button1.ImageAlign = ContentAlignment.MiddleLeft;
      this.button1.Location = new Point(1108, 343);
      this.button1.Margin = new Padding(9, 6, 9, 6);
      this.button1.Name = "button1";
      this.button1.Size = new Size(233, 28);
      this.button1.TabIndex = 54;
      this.button1.Text = "Стоп";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click_2);
      this.UrlTextBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox2.BackColor = System.Drawing.Color.White;
      this.UrlTextBox2.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox2.Location = new Point(912, 28);
      this.UrlTextBox2.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox2.Name = "UrlTextBox2";
      this.UrlTextBox2.Size = new Size(407, 19);
      this.UrlTextBox2.TabIndex = 57;
      this.UrlTextBox2.TextChanged += new EventHandler(this.UrlTextBox2_TextChanged);
      this.UrlTextBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox3.BackColor = System.Drawing.Color.White;
      this.UrlTextBox3.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox3.Location = new Point(912, 49);
      this.UrlTextBox3.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox3.Name = "UrlTextBox3";
      this.UrlTextBox3.Size = new Size(407, 19);
      this.UrlTextBox3.TabIndex = 58;
      this.UrlTextBox3.TextChanged += new EventHandler(this.UrlTextBox3_TextChanged);
      this.UrlTextBox4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox4.BackColor = System.Drawing.Color.White;
      this.UrlTextBox4.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox4.Location = new Point(912, 70);
      this.UrlTextBox4.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox4.Name = "UrlTextBox4";
      this.UrlTextBox4.Size = new Size(407, 19);
      this.UrlTextBox4.TabIndex = 59;
      this.UrlTextBox4.TextChanged += new EventHandler(this.UrlTextBox4_TextChanged);
      this.UrlTextBox5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox5.BackColor = System.Drawing.Color.White;
      this.UrlTextBox5.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox5.Location = new Point(912, 91);
      this.UrlTextBox5.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox5.Name = "UrlTextBox5";
      this.UrlTextBox5.Size = new Size(407, 19);
      this.UrlTextBox5.TabIndex = 60;
      this.UrlTextBox5.TextChanged += new EventHandler(this.UrlTextBox5_TextChanged);
      this.UrlTextBox6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox6.BackColor = System.Drawing.Color.White;
      this.UrlTextBox6.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox6.Location = new Point(912, 112);
      this.UrlTextBox6.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox6.Name = "UrlTextBox6";
      this.UrlTextBox6.Size = new Size(407, 19);
      this.UrlTextBox6.TabIndex = 61;
      this.UrlTextBox6.TextChanged += new EventHandler(this.UrlTextBox6_TextChanged);
      this.UrlTextBox7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox7.BackColor = System.Drawing.Color.White;
      this.UrlTextBox7.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox7.Location = new Point(912, 133);
      this.UrlTextBox7.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox7.Name = "UrlTextBox7";
      this.UrlTextBox7.Size = new Size(407, 19);
      this.UrlTextBox7.TabIndex = 62;
      this.UrlTextBox7.TextChanged += new EventHandler(this.UrlTextBox7_TextChanged);
      this.UrlTextBox8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox8.BackColor = System.Drawing.Color.White;
      this.UrlTextBox8.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox8.Location = new Point(912, 154);
      this.UrlTextBox8.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox8.Name = "UrlTextBox8";
      this.UrlTextBox8.Size = new Size(407, 19);
      this.UrlTextBox8.TabIndex = 63;
      this.UrlTextBox8.TextChanged += new EventHandler(this.UrlTextBox8_TextChanged);
      this.UrlTextBox9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox9.BackColor = System.Drawing.Color.White;
      this.UrlTextBox9.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox9.Location = new Point(912, 175);
      this.UrlTextBox9.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox9.Name = "UrlTextBox9";
      this.UrlTextBox9.Size = new Size(407, 19);
      this.UrlTextBox9.TabIndex = 64;
      this.UrlTextBox9.TextChanged += new EventHandler(this.UrlTextBox9_TextChanged);
      this.UrlTextBox10.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox10.BackColor = System.Drawing.Color.White;
      this.UrlTextBox10.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox10.Location = new Point(912, 196);
      this.UrlTextBox10.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox10.Name = "UrlTextBox10";
      this.UrlTextBox10.Size = new Size(407, 19);
      this.UrlTextBox10.TabIndex = 65;
      this.UrlTextBox10.TextChanged += new EventHandler(this.UrlTextBox10_TextChanged);
      this.UrlTextBox11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox11.BackColor = System.Drawing.Color.White;
      this.UrlTextBox11.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox11.Location = new Point(912, 217);
      this.UrlTextBox11.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox11.Name = "UrlTextBox11";
      this.UrlTextBox11.Size = new Size(407, 19);
      this.UrlTextBox11.TabIndex = 66;
      this.UrlTextBox11.TextChanged += new EventHandler(this.UrlTextBox11_TextChanged);
      this.checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new Point(869, 10);
      this.checkBox1.Margin = new Padding(9, 6, 9, 6);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new Size(15, 14);
      this.checkBox1.TabIndex = 67;
      this.checkBox1.UseVisualStyleBackColor = true;
      this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.checkBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox2.AutoSize = true;
      this.checkBox2.Location = new Point(869, 31);
      this.checkBox2.Margin = new Padding(9, 6, 9, 6);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new Size(15, 14);
      this.checkBox2.TabIndex = 68;
      this.checkBox2.UseVisualStyleBackColor = true;
      this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
      this.checkBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox3.AutoSize = true;
      this.checkBox3.Location = new Point(869, 53);
      this.checkBox3.Margin = new Padding(9, 6, 9, 6);
      this.checkBox3.Name = "checkBox3";
      this.checkBox3.Size = new Size(15, 14);
      this.checkBox3.TabIndex = 69;
      this.checkBox3.UseVisualStyleBackColor = true;
      this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
      this.checkBox4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox4.AutoSize = true;
      this.checkBox4.Location = new Point(869, 73);
      this.checkBox4.Margin = new Padding(9, 6, 9, 6);
      this.checkBox4.Name = "checkBox4";
      this.checkBox4.Size = new Size(15, 14);
      this.checkBox4.TabIndex = 70;
      this.checkBox4.UseVisualStyleBackColor = true;
      this.checkBox4.CheckedChanged += new EventHandler(this.checkBox4_CheckedChanged);
      this.checkBox5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox5.AutoSize = true;
      this.checkBox5.Location = new Point(869, 94);
      this.checkBox5.Margin = new Padding(9, 6, 9, 6);
      this.checkBox5.Name = "checkBox5";
      this.checkBox5.Size = new Size(15, 14);
      this.checkBox5.TabIndex = 71;
      this.checkBox5.UseVisualStyleBackColor = true;
      this.checkBox5.CheckedChanged += new EventHandler(this.checkBox5_CheckedChanged);
      this.checkBox6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox6.AutoSize = true;
      this.checkBox6.Location = new Point(869, 115);
      this.checkBox6.Margin = new Padding(9, 6, 9, 6);
      this.checkBox6.Name = "checkBox6";
      this.checkBox6.Size = new Size(15, 14);
      this.checkBox6.TabIndex = 72;
      this.checkBox6.UseVisualStyleBackColor = true;
      this.checkBox6.CheckedChanged += new EventHandler(this.checkBox6_CheckedChanged);
      this.checkBox7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox7.AutoSize = true;
      this.checkBox7.Location = new Point(869, 136);
      this.checkBox7.Margin = new Padding(9, 6, 9, 6);
      this.checkBox7.Name = "checkBox7";
      this.checkBox7.Size = new Size(15, 14);
      this.checkBox7.TabIndex = 73;
      this.checkBox7.UseVisualStyleBackColor = true;
      this.checkBox7.CheckedChanged += new EventHandler(this.checkBox7_CheckedChanged);
      this.checkBox8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox8.AutoSize = true;
      this.checkBox8.Location = new Point(869, 157);
      this.checkBox8.Margin = new Padding(9, 6, 9, 6);
      this.checkBox8.Name = "checkBox8";
      this.checkBox8.Size = new Size(15, 14);
      this.checkBox8.TabIndex = 74;
      this.checkBox8.UseVisualStyleBackColor = true;
      this.checkBox8.CheckedChanged += new EventHandler(this.checkBox8_CheckedChanged);
      this.checkBox9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox9.AutoSize = true;
      this.checkBox9.Location = new Point(869, 178);
      this.checkBox9.Margin = new Padding(9, 6, 9, 6);
      this.checkBox9.Name = "checkBox9";
      this.checkBox9.Size = new Size(15, 14);
      this.checkBox9.TabIndex = 75;
      this.checkBox9.UseVisualStyleBackColor = true;
      this.checkBox9.CheckedChanged += new EventHandler(this.checkBox9_CheckedChanged);
      this.checkBox10.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox10.AutoSize = true;
      this.checkBox10.Location = new Point(869, 199);
      this.checkBox10.Margin = new Padding(9, 6, 9, 6);
      this.checkBox10.Name = "checkBox10";
      this.checkBox10.Size = new Size(15, 14);
      this.checkBox10.TabIndex = 76;
      this.checkBox10.UseVisualStyleBackColor = true;
      this.checkBox10.CheckedChanged += new EventHandler(this.checkBox10_CheckedChanged);
      this.checkBox11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox11.AutoSize = true;
      this.checkBox11.Location = new Point(869, 220);
      this.checkBox11.Margin = new Padding(9, 6, 9, 6);
      this.checkBox11.Name = "checkBox11";
      this.checkBox11.Size = new Size(15, 14);
      this.checkBox11.TabIndex = 77;
      this.checkBox11.UseVisualStyleBackColor = true;
      this.checkBox11.CheckedChanged += new EventHandler(this.checkBox11_CheckedChanged);
      this.checkBox16.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox16.AutoSize = true;
      this.checkBox16.Location = new Point(869, 325);
      this.checkBox16.Margin = new Padding(9, 6, 9, 6);
      this.checkBox16.Name = "checkBox16";
      this.checkBox16.Size = new Size(15, 14);
      this.checkBox16.TabIndex = 87;
      this.checkBox16.UseVisualStyleBackColor = true;
      this.checkBox16.CheckedChanged += new EventHandler(this.checkBox16_CheckedChanged);
      this.checkBox15.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox15.AutoSize = true;
      this.checkBox15.Location = new Point(869, 304);
      this.checkBox15.Margin = new Padding(9, 6, 9, 6);
      this.checkBox15.Name = "checkBox15";
      this.checkBox15.Size = new Size(15, 14);
      this.checkBox15.TabIndex = 86;
      this.checkBox15.UseVisualStyleBackColor = true;
      this.checkBox15.CheckedChanged += new EventHandler(this.checkBox15_CheckedChanged);
      this.checkBox14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox14.AutoSize = true;
      this.checkBox14.Location = new Point(869, 283);
      this.checkBox14.Margin = new Padding(9, 6, 9, 6);
      this.checkBox14.Name = "checkBox14";
      this.checkBox14.Size = new Size(15, 14);
      this.checkBox14.TabIndex = 85;
      this.checkBox14.UseVisualStyleBackColor = true;
      this.checkBox14.CheckedChanged += new EventHandler(this.checkBox14_CheckedChanged);
      this.checkBox13.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox13.AutoSize = true;
      this.checkBox13.Location = new Point(869, 262);
      this.checkBox13.Margin = new Padding(9, 6, 9, 6);
      this.checkBox13.Name = "checkBox13";
      this.checkBox13.Size = new Size(15, 14);
      this.checkBox13.TabIndex = 84;
      this.checkBox13.UseVisualStyleBackColor = true;
      this.checkBox13.CheckedChanged += new EventHandler(this.checkBox13_CheckedChanged);
      this.checkBox12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBox12.AutoSize = true;
      this.checkBox12.Location = new Point(869, 241);
      this.checkBox12.Margin = new Padding(9, 6, 9, 6);
      this.checkBox12.Name = "checkBox12";
      this.checkBox12.Size = new Size(15, 14);
      this.checkBox12.TabIndex = 83;
      this.checkBox12.UseVisualStyleBackColor = true;
      this.checkBox12.CheckedChanged += new EventHandler(this.checkBox12_CheckedChanged);
      this.UrlTextBox16.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox16.BackColor = System.Drawing.Color.White;
      this.UrlTextBox16.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox16.Location = new Point(912, 322);
      this.UrlTextBox16.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox16.Name = "UrlTextBox16";
      this.UrlTextBox16.Size = new Size(407, 19);
      this.UrlTextBox16.TabIndex = 82;
      this.UrlTextBox16.TextChanged += new EventHandler(this.UrlTextBox16_TextChanged);
      this.UrlTextBox15.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox15.BackColor = System.Drawing.Color.White;
      this.UrlTextBox15.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox15.Location = new Point(912, 301);
      this.UrlTextBox15.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox15.Name = "UrlTextBox15";
      this.UrlTextBox15.Size = new Size(407, 19);
      this.UrlTextBox15.TabIndex = 81;
      this.UrlTextBox15.TextChanged += new EventHandler(this.UrlTextBox15_TextChanged);
      this.UrlTextBox14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox14.BackColor = System.Drawing.Color.White;
      this.UrlTextBox14.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox14.Location = new Point(912, 280);
      this.UrlTextBox14.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox14.Name = "UrlTextBox14";
      this.UrlTextBox14.Size = new Size(407, 19);
      this.UrlTextBox14.TabIndex = 80;
      this.UrlTextBox14.TextChanged += new EventHandler(this.UrlTextBox14_TextChanged);
      this.UrlTextBox13.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox13.BackColor = System.Drawing.Color.White;
      this.UrlTextBox13.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox13.Location = new Point(912, 259);
      this.UrlTextBox13.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox13.Name = "UrlTextBox13";
      this.UrlTextBox13.Size = new Size(407, 19);
      this.UrlTextBox13.TabIndex = 79;
      this.UrlTextBox13.TextChanged += new EventHandler(this.UrlTextBox13_TextChanged);
      this.UrlTextBox12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.UrlTextBox12.BackColor = System.Drawing.Color.White;
      this.UrlTextBox12.BorderStyle = BorderStyle.FixedSingle;
      this.UrlTextBox12.Location = new Point(912, 238);
      this.UrlTextBox12.Margin = new Padding(9, 6, 9, 6);
      this.UrlTextBox12.Name = "UrlTextBox12";
      this.UrlTextBox12.Size = new Size(407, 19);
      this.UrlTextBox12.TabIndex = 78;
      this.UrlTextBox12.TextChanged += new EventHandler(this.UrlTextBox12_TextChanged);
      this.ShowStopWordsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.ShowStopWordsButton.Image = (Image) componentResourceManager.GetObject("ShowStopWordsButton.Image");
      this.ShowStopWordsButton.ImageAlign = ContentAlignment.MiddleLeft;
      this.ShowStopWordsButton.Location = new Point(750, 636);
      this.ShowStopWordsButton.Margin = new Padding(9, 6, 9, 6);
      this.ShowStopWordsButton.Name = "ShowStopWordsButton";
      this.ShowStopWordsButton.Size = new Size(115, 28);
      this.ShowStopWordsButton.TabIndex = 88;
      this.ShowStopWordsButton.Text = "Стоп слова";
      this.ShowStopWordsButton.UseVisualStyleBackColor = true;
      this.ShowStopWordsButton.Click += new EventHandler(this.ShowStopWordsButton_Click);
      this.ObjectiveWords.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.ObjectiveWords.Image = (Image) componentResourceManager.GetObject("ObjectiveWords.Image");
      this.ObjectiveWords.ImageAlign = ContentAlignment.MiddleLeft;
      this.ObjectiveWords.Location = new Point(868, 636);
      this.ObjectiveWords.Margin = new Padding(9, 6, 9, 6);
      this.ObjectiveWords.Name = "ObjectiveWords";
      this.ObjectiveWords.Size = new Size(131, 28);
      this.ObjectiveWords.TabIndex = 89;
      this.ObjectiveWords.Text = "Целевые слова";
      this.ObjectiveWords.UseVisualStyleBackColor = true;
      this.ObjectiveWords.Click += new EventHandler(this.ObjectiveWords_Click);
      this.label21.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label21.AutoSize = true;
      this.label21.BackColor = SystemColors.GradientInactiveCaption;
      this.label21.BorderStyle = BorderStyle.FixedSingle;
      this.label21.Cursor = Cursors.Hand;
      this.label21.FlatStyle = FlatStyle.Flat;
      this.label21.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label21.Location = new Point(887, 8);
      this.label21.Margin = new Padding(9, 0, 9, 0);
      this.label21.Name = "label21";
      this.label21.Size = new Size(22, 17);
      this.label21.TabIndex = 90;
      this.label21.Text = "1  ";
      this.label21.Click += new EventHandler(this.label21_Click);
      this.label21.MouseEnter += new EventHandler(this.label21_MouseEnter);
      this.label21.MouseLeave += new EventHandler(this.label21_MouseLeave);
      this.label22.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label22.AutoSize = true;
      this.label22.BackColor = SystemColors.GradientInactiveCaption;
      this.label22.BorderStyle = BorderStyle.FixedSingle;
      this.label22.Cursor = Cursors.Hand;
      this.label22.FlatStyle = FlatStyle.Flat;
      this.label22.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label22.Location = new Point(887, 29);
      this.label22.Margin = new Padding(9, 0, 9, 0);
      this.label22.Name = "label22";
      this.label22.Size = new Size(22, 17);
      this.label22.TabIndex = 91;
      this.label22.Text = "2  ";
      this.label22.Click += new EventHandler(this.label22_Click);
      this.label23.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label23.AutoSize = true;
      this.label23.BackColor = SystemColors.GradientInactiveCaption;
      this.label23.BorderStyle = BorderStyle.FixedSingle;
      this.label23.Cursor = Cursors.Hand;
      this.label23.FlatStyle = FlatStyle.Flat;
      this.label23.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label23.Location = new Point(887, 50);
      this.label23.Margin = new Padding(9, 0, 9, 0);
      this.label23.Name = "label23";
      this.label23.Size = new Size(22, 17);
      this.label23.TabIndex = 92;
      this.label23.Text = "3  ";
      this.label23.Click += new EventHandler(this.label23_Click);
      this.label24.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label24.AutoSize = true;
      this.label24.BackColor = SystemColors.GradientInactiveCaption;
      this.label24.BorderStyle = BorderStyle.FixedSingle;
      this.label24.Cursor = Cursors.Hand;
      this.label24.FlatStyle = FlatStyle.Flat;
      this.label24.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label24.Location = new Point(887, 71);
      this.label24.Margin = new Padding(9, 0, 9, 0);
      this.label24.Name = "label24";
      this.label24.Size = new Size(22, 17);
      this.label24.TabIndex = 93;
      this.label24.Text = "4  ";
      this.label24.Click += new EventHandler(this.label24_Click);
      this.label25.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label25.AutoSize = true;
      this.label25.BackColor = SystemColors.GradientInactiveCaption;
      this.label25.BorderStyle = BorderStyle.FixedSingle;
      this.label25.Cursor = Cursors.Hand;
      this.label25.FlatStyle = FlatStyle.Flat;
      this.label25.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label25.Location = new Point(887, 92);
      this.label25.Margin = new Padding(9, 0, 9, 0);
      this.label25.Name = "label25";
      this.label25.Size = new Size(22, 17);
      this.label25.TabIndex = 94;
      this.label25.Text = "5  ";
      this.label25.Click += new EventHandler(this.label25_Click);
      this.label26.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label26.AutoSize = true;
      this.label26.BackColor = SystemColors.GradientInactiveCaption;
      this.label26.BorderStyle = BorderStyle.FixedSingle;
      this.label26.Cursor = Cursors.Hand;
      this.label26.FlatStyle = FlatStyle.Flat;
      this.label26.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label26.Location = new Point(887, 113);
      this.label26.Margin = new Padding(9, 0, 9, 0);
      this.label26.Name = "label26";
      this.label26.Size = new Size(22, 17);
      this.label26.TabIndex = 95;
      this.label26.Text = "6  ";
      this.label26.Click += new EventHandler(this.label26_Click);
      this.label27.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label27.AutoSize = true;
      this.label27.BackColor = SystemColors.GradientInactiveCaption;
      this.label27.BorderStyle = BorderStyle.FixedSingle;
      this.label27.Cursor = Cursors.Hand;
      this.label27.FlatStyle = FlatStyle.Flat;
      this.label27.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label27.Location = new Point(887, 134);
      this.label27.Margin = new Padding(9, 0, 9, 0);
      this.label27.Name = "label27";
      this.label27.Size = new Size(22, 17);
      this.label27.TabIndex = 96;
      this.label27.Text = "7  ";
      this.label27.Click += new EventHandler(this.label27_Click);
      this.label28.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label28.AutoSize = true;
      this.label28.BackColor = SystemColors.GradientInactiveCaption;
      this.label28.BorderStyle = BorderStyle.FixedSingle;
      this.label28.Cursor = Cursors.Hand;
      this.label28.FlatStyle = FlatStyle.Flat;
      this.label28.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label28.Location = new Point(887, 155);
      this.label28.Margin = new Padding(9, 0, 9, 0);
      this.label28.Name = "label28";
      this.label28.Size = new Size(22, 17);
      this.label28.TabIndex = 97;
      this.label28.Text = "8  ";
      this.label28.Click += new EventHandler(this.label28_Click);
      this.label29.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label29.AutoSize = true;
      this.label29.BackColor = SystemColors.GradientInactiveCaption;
      this.label29.BorderStyle = BorderStyle.FixedSingle;
      this.label29.Cursor = Cursors.Hand;
      this.label29.FlatStyle = FlatStyle.Flat;
      this.label29.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label29.Location = new Point(887, 176);
      this.label29.Margin = new Padding(9, 0, 9, 0);
      this.label29.Name = "label29";
      this.label29.Size = new Size(22, 17);
      this.label29.TabIndex = 98;
      this.label29.Text = "9  ";
      this.label29.TextAlign = ContentAlignment.MiddleCenter;
      this.label29.Click += new EventHandler(this.label29_Click);
      this.label30.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label30.AutoSize = true;
      this.label30.BackColor = SystemColors.GradientInactiveCaption;
      this.label30.BorderStyle = BorderStyle.FixedSingle;
      this.label30.Cursor = Cursors.Hand;
      this.label30.FlatStyle = FlatStyle.Flat;
      this.label30.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label30.Location = new Point(887, 197);
      this.label30.Margin = new Padding(9, 0, 9, 0);
      this.label30.Name = "label30";
      this.label30.Size = new Size(23, 17);
      this.label30.TabIndex = 99;
      this.label30.Text = "10";
      this.label30.Click += new EventHandler(this.label30_Click);
      this.label31.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label31.AutoSize = true;
      this.label31.BackColor = SystemColors.GradientInactiveCaption;
      this.label31.BorderStyle = BorderStyle.FixedSingle;
      this.label31.Cursor = Cursors.Hand;
      this.label31.FlatStyle = FlatStyle.Flat;
      this.label31.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label31.Location = new Point(887, 218);
      this.label31.Margin = new Padding(9, 0, 9, 0);
      this.label31.Name = "label31";
      this.label31.Size = new Size(23, 17);
      this.label31.TabIndex = 100;
      this.label31.Text = "11";
      this.label31.Click += new EventHandler(this.label31_Click);
      this.label32.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label32.AutoSize = true;
      this.label32.BackColor = SystemColors.GradientInactiveCaption;
      this.label32.BorderStyle = BorderStyle.FixedSingle;
      this.label32.Cursor = Cursors.Hand;
      this.label32.FlatStyle = FlatStyle.Flat;
      this.label32.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label32.Location = new Point(887, 239);
      this.label32.Margin = new Padding(9, 0, 9, 0);
      this.label32.Name = "label32";
      this.label32.Size = new Size(23, 17);
      this.label32.TabIndex = 101;
      this.label32.Text = "12";
      this.label32.Click += new EventHandler(this.label32_Click);
      this.label33.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label33.AutoSize = true;
      this.label33.BackColor = SystemColors.GradientInactiveCaption;
      this.label33.BorderStyle = BorderStyle.FixedSingle;
      this.label33.Cursor = Cursors.Hand;
      this.label33.FlatStyle = FlatStyle.Flat;
      this.label33.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label33.Location = new Point(887, 260);
      this.label33.Margin = new Padding(9, 0, 9, 0);
      this.label33.Name = "label33";
      this.label33.Size = new Size(23, 17);
      this.label33.TabIndex = 102;
      this.label33.Text = "13";
      this.label33.Click += new EventHandler(this.label33_Click);
      this.label34.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label34.AutoSize = true;
      this.label34.BackColor = SystemColors.GradientInactiveCaption;
      this.label34.BorderStyle = BorderStyle.FixedSingle;
      this.label34.Cursor = Cursors.Hand;
      this.label34.FlatStyle = FlatStyle.Flat;
      this.label34.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label34.Location = new Point(887, 281);
      this.label34.Margin = new Padding(9, 0, 9, 0);
      this.label34.Name = "label34";
      this.label34.Size = new Size(23, 17);
      this.label34.TabIndex = 103;
      this.label34.Text = "14";
      this.label34.Click += new EventHandler(this.label34_Click);
      this.label35.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label35.AutoSize = true;
      this.label35.BackColor = SystemColors.GradientInactiveCaption;
      this.label35.BorderStyle = BorderStyle.FixedSingle;
      this.label35.Cursor = Cursors.Hand;
      this.label35.FlatStyle = FlatStyle.Flat;
      this.label35.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label35.Location = new Point(887, 302);
      this.label35.Margin = new Padding(9, 0, 9, 0);
      this.label35.Name = "label35";
      this.label35.Size = new Size(23, 17);
      this.label35.TabIndex = 104;
      this.label35.Text = "15";
      this.label35.Click += new EventHandler(this.label35_Click);
      this.label36.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label36.AutoSize = true;
      this.label36.BackColor = SystemColors.GradientInactiveCaption;
      this.label36.BorderStyle = BorderStyle.FixedSingle;
      this.label36.Cursor = Cursors.Hand;
      this.label36.FlatStyle = FlatStyle.Flat;
      this.label36.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label36.Location = new Point(887, 323);
      this.label36.Margin = new Padding(9, 0, 9, 0);
      this.label36.Name = "label36";
      this.label36.Size = new Size(23, 17);
      this.label36.TabIndex = 105;
      this.label36.Text = "16";
      this.label36.Click += new EventHandler(this.label36_Click);
      this.notifyIcon1.Icon = (Icon) componentResourceManager.GetObject("notifyIcon1.Icon");
      this.notifyIcon1.Text = "AvitoAvtoRinger";
      this.notifyIcon1.Visible = true;
      this.notifyIcon1.BalloonTipClicked += new EventHandler(this.notifyIcon1_BalloonTipClicked);
      this.notifyIcon1.MouseClick += new MouseEventHandler(this.notifyIcon1_MouseClick);
      this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
      this.BrowserOpenChekBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.BrowserOpenChekBox.AutoSize = true;
      this.BrowserOpenChekBox.Checked = true;
      this.BrowserOpenChekBox.CheckState = CheckState.Checked;
      this.BrowserOpenChekBox.Location = new Point(871, 561);
      this.BrowserOpenChekBox.Name = "BrowserOpenChekBox";
      this.BrowserOpenChekBox.Size = new Size(141, 17);
      this.BrowserOpenChekBox.TabIndex = 106;
      this.BrowserOpenChekBox.Text = "Открывать в браузере";
      this.BrowserOpenChekBox.UseVisualStyleBackColor = true;
      this.BrowserOpenChekBox.CheckedChanged += new EventHandler(this.BrowserOpenChekBox_CheckedChanged);
      this.BeepCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.BeepCheckBox.AutoSize = true;
      this.BeepCheckBox.Location = new Point(871, 577);
      this.BeepCheckBox.Name = "BeepCheckBox";
      this.BeepCheckBox.Size = new Size(170, 17);
      this.BeepCheckBox.TabIndex = 107;
      this.BeepCheckBox.Text = "Звук при новом объявлении";
      this.BeepCheckBox.UseVisualStyleBackColor = true;
      this.BeepCheckBox.CheckedChanged += new EventHandler(this.BeepCheckBox_CheckedChanged);
      this.TelegrammCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.TelegrammCheckBox.AutoSize = true;
      this.TelegrammCheckBox.Location = new Point(871, 593);
      this.TelegrammCheckBox.Name = "TelegrammCheckBox";
      this.TelegrammCheckBox.Size = new Size(156, 17);
      this.TelegrammCheckBox.TabIndex = 108;
      this.TelegrammCheckBox.Text = "Отправлять в Телеграмм";
      this.TelegrammCheckBox.UseVisualStyleBackColor = true;
      this.TelegrammCheckBox.CheckedChanged += new EventHandler(this.TelegrammCheckBox_CheckedChanged);
      this.numericUpDown1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.numericUpDown1.Location = new Point(1114, 616);
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
      this.numericUpDown1.Size = new Size(100, 19);
      this.numericUpDown1.TabIndex = 109;
      this.numericUpDown1.Value = new Decimal(new int[4]
      {
        120,
        0,
        0,
        0
      });
      this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
      this.label37.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label37.AutoSize = true;
      this.label37.Location = new Point(868, 617);
      this.label37.Name = "label37";
      this.label37.Size = new Size(228, 13);
      this.label37.TabIndex = 110;
      this.label37.Text = "Проверять новые объявления каждые сек.";
      this.RaiseAdvertsCheck.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.RaiseAdvertsCheck.AutoSize = true;
      this.RaiseAdvertsCheck.Location = new Point(1036, 593);
      this.RaiseAdvertsCheck.Name = "RaiseAdvertsCheck";
      this.RaiseAdvertsCheck.Size = new Size(274, 17);
      this.RaiseAdvertsCheck.TabIndex = 111;
      this.RaiseAdvertsCheck.Text = "Показывать объявлеия, которые уже сть в базе";
      this.RaiseAdvertsCheck.UseVisualStyleBackColor = true;
      this.RaiseAdvertsCheck.CheckedChanged += new EventHandler(this.RaiseAdvertsCheck_CheckedChanged);
      this.MagazCheck.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.MagazCheck.AutoSize = true;
      this.MagazCheck.Location = new Point(1036, 577);
      this.MagazCheck.Name = "MagazCheck";
      this.MagazCheck.Size = new Size(220, 17);
      this.MagazCheck.TabIndex = 112;
      this.MagazCheck.Text = "Игнорировать объявления магазинов";
      this.MagazCheck.UseVisualStyleBackColor = true;
      this.MagazCheck.CheckedChanged += new EventHandler(this.MagazCheck_CheckedChanged);
      this.AvitoDostavkaCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.AvitoDostavkaCheckBox.AutoSize = true;
      this.AvitoDostavkaCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.AvitoDostavkaCheckBox.Checked = true;
      this.AvitoDostavkaCheckBox.CheckState = CheckState.Checked;
      this.AvitoDostavkaCheckBox.Location = new Point(1036, 561);
      this.AvitoDostavkaCheckBox.Name = "AvitoDostavkaCheckBox";
      this.AvitoDostavkaCheckBox.Size = new Size(267, 17);
      this.AvitoDostavkaCheckBox.TabIndex = 113;
      this.AvitoDostavkaCheckBox.Text = "Показывать объявления только своего города";
      this.AvitoDostavkaCheckBox.UseVisualStyleBackColor = false;
      this.AvitoDostavkaCheckBox.CheckedChanged += new EventHandler(this.AvitoDostavkaCheckBox_CheckedChanged);
      this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox1.BackColor = SystemColors.Control;
      this.pictureBox1.Cursor = Cursors.Hand;
      this.pictureBox1.Location = new Point(1321, 7);
      this.pictureBox1.Margin = new Padding(0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(20, 19);
      this.pictureBox1.TabIndex = 114;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.MouseEnter += new EventHandler(this.pictureBox1_MouseEnter);
      this.pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox2.Cursor = Cursors.Hand;
      this.pictureBox2.Location = new Point(1321, 28);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(20, 19);
      this.pictureBox2.TabIndex = 115;
      this.pictureBox2.TabStop = false;
      this.pictureBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox3.Cursor = Cursors.Hand;
      this.pictureBox3.Location = new Point(1321, 49);
      this.pictureBox3.Name = "pictureBox3";
      this.pictureBox3.Size = new Size(20, 19);
      this.pictureBox3.TabIndex = 116;
      this.pictureBox3.TabStop = false;
      this.pictureBox4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox4.Cursor = Cursors.Hand;
      this.pictureBox4.Location = new Point(1321, 70);
      this.pictureBox4.Name = "pictureBox4";
      this.pictureBox4.Size = new Size(20, 19);
      this.pictureBox4.TabIndex = 117;
      this.pictureBox4.TabStop = false;
      this.pictureBox5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox5.Cursor = Cursors.Hand;
      this.pictureBox5.Location = new Point(1321, 91);
      this.pictureBox5.Name = "pictureBox5";
      this.pictureBox5.Size = new Size(20, 19);
      this.pictureBox5.TabIndex = 118;
      this.pictureBox5.TabStop = false;
      this.pictureBox6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox6.Cursor = Cursors.Hand;
      this.pictureBox6.Location = new Point(1321, 112);
      this.pictureBox6.Name = "pictureBox6";
      this.pictureBox6.Size = new Size(20, 19);
      this.pictureBox6.TabIndex = 119;
      this.pictureBox6.TabStop = false;
      this.pictureBox7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox7.Cursor = Cursors.Hand;
      this.pictureBox7.Location = new Point(1321, 133);
      this.pictureBox7.Name = "pictureBox7";
      this.pictureBox7.Size = new Size(20, 19);
      this.pictureBox7.TabIndex = 120;
      this.pictureBox7.TabStop = false;
      this.pictureBox8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox8.Cursor = Cursors.Hand;
      this.pictureBox8.Location = new Point(1321, 154);
      this.pictureBox8.Name = "pictureBox8";
      this.pictureBox8.Size = new Size(20, 19);
      this.pictureBox8.TabIndex = 121;
      this.pictureBox8.TabStop = false;
      this.pictureBox9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox9.Cursor = Cursors.Hand;
      this.pictureBox9.Location = new Point(1321, 175);
      this.pictureBox9.Name = "pictureBox9";
      this.pictureBox9.Size = new Size(20, 19);
      this.pictureBox9.TabIndex = 122;
      this.pictureBox9.TabStop = false;
      this.pictureBox10.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox10.Cursor = Cursors.Hand;
      this.pictureBox10.Location = new Point(1321, 196);
      this.pictureBox10.Name = "pictureBox10";
      this.pictureBox10.Size = new Size(20, 19);
      this.pictureBox10.TabIndex = 123;
      this.pictureBox10.TabStop = false;
      this.pictureBox11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox11.Cursor = Cursors.Hand;
      this.pictureBox11.Location = new Point(1321, 217);
      this.pictureBox11.Name = "pictureBox11";
      this.pictureBox11.Size = new Size(20, 19);
      this.pictureBox11.TabIndex = 124;
      this.pictureBox11.TabStop = false;
      this.pictureBox12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox12.Cursor = Cursors.Hand;
      this.pictureBox12.Location = new Point(1321, 238);
      this.pictureBox12.Name = "pictureBox12";
      this.pictureBox12.Size = new Size(20, 19);
      this.pictureBox12.TabIndex = 125;
      this.pictureBox12.TabStop = false;
      this.pictureBox13.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox13.Cursor = Cursors.Hand;
      this.pictureBox13.Location = new Point(1321, 259);
      this.pictureBox13.Name = "pictureBox13";
      this.pictureBox13.Size = new Size(20, 19);
      this.pictureBox13.TabIndex = 126;
      this.pictureBox13.TabStop = false;
      this.pictureBox14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox14.Cursor = Cursors.Hand;
      this.pictureBox14.Location = new Point(1321, 280);
      this.pictureBox14.Name = "pictureBox14";
      this.pictureBox14.Size = new Size(20, 19);
      this.pictureBox14.TabIndex = (int) sbyte.MaxValue;
      this.pictureBox14.TabStop = false;
      this.pictureBox15.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox15.Cursor = Cursors.Hand;
      this.pictureBox15.Location = new Point(1321, 301);
      this.pictureBox15.Name = "pictureBox15";
      this.pictureBox15.Size = new Size(20, 19);
      this.pictureBox15.TabIndex = 128;
      this.pictureBox15.TabStop = false;
      this.pictureBox16.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBox16.Cursor = Cursors.Hand;
      this.pictureBox16.Location = new Point(1321, 322);
      this.pictureBox16.Name = "pictureBox16";
      this.pictureBox16.Size = new Size(20, 19);
      this.pictureBox16.TabIndex = 129;
      this.pictureBox16.TabStop = false;
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.GetAdvertInfoButton);
      this.groupBox1.Controls.Add((Control) this.label38);
      this.groupBox1.Controls.Add((Control) this.AdvertInfoTBox);
      this.groupBox1.Controls.Add((Control) this.AdvertPictureBox);
      this.groupBox1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.groupBox1.Location = new Point(852, -2);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(10, 626);
      this.groupBox1.TabIndex = 130;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Информация объявления";
      this.groupBox1.Visible = false;
      this.GetAdvertInfoButton.Location = new Point(6, 487);
      this.GetAdvertInfoButton.Name = "GetAdvertInfoButton";
      this.GetAdvertInfoButton.Size = new Size(295, 31);
      this.GetAdvertInfoButton.TabIndex = 3;
      this.GetAdvertInfoButton.Text = "Получить информацию объявления";
      this.GetAdvertInfoButton.UseVisualStyleBackColor = true;
      this.GetAdvertInfoButton.Click += new EventHandler(this.GetAdvertInfoButton_Click);
      this.label38.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label38.Location = new Point(6, 521);
      this.label38.Name = "label38";
      this.label38.Size = new Size(295, 105);
      this.label38.TabIndex = 2;
      this.label38.Text = componentResourceManager.GetString("label38.Text");
      this.label38.TextAlign = ContentAlignment.TopCenter;
      this.AdvertInfoTBox.BackColor = SystemColors.Control;
      this.AdvertInfoTBox.BorderStyle = BorderStyle.FixedSingle;
      this.AdvertInfoTBox.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.AdvertInfoTBox.Location = new Point(6, 260);
      this.AdvertInfoTBox.Multiline = true;
      this.AdvertInfoTBox.Name = "AdvertInfoTBox";
      this.AdvertInfoTBox.Size = new Size(295, 221);
      this.AdvertInfoTBox.TabIndex = 1;
      this.AdvertInfoTBox.Text = "Описание объявления";
    //  this.AdvertPictureBox.BackgroundImage = (Image) Resources.Без_имени_1;
      this.AdvertPictureBox.BorderStyle = BorderStyle.FixedSingle;
      this.AdvertPictureBox.Location = new Point(6, 23);
      this.AdvertPictureBox.Name = "AdvertPictureBox";
      this.AdvertPictureBox.Size = new Size(295, 232);
      this.AdvertPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
      this.AdvertPictureBox.TabIndex = 0;
      this.AdvertPictureBox.TabStop = false;
      this.GetAdvertTimer.Interval = 5000;
      this.GetAdvertTimer.Tick += new EventHandler(this.GetAdvertTimer_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1350, 666);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.pictureBox16);
      this.Controls.Add((Control) this.pictureBox15);
      this.Controls.Add((Control) this.pictureBox14);
      this.Controls.Add((Control) this.pictureBox13);
      this.Controls.Add((Control) this.pictureBox12);
      this.Controls.Add((Control) this.pictureBox11);
      this.Controls.Add((Control) this.pictureBox10);
      this.Controls.Add((Control) this.pictureBox9);
      this.Controls.Add((Control) this.pictureBox8);
      this.Controls.Add((Control) this.pictureBox7);
      this.Controls.Add((Control) this.pictureBox6);
      this.Controls.Add((Control) this.pictureBox5);
      this.Controls.Add((Control) this.pictureBox4);
      this.Controls.Add((Control) this.pictureBox3);
      this.Controls.Add((Control) this.pictureBox2);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.AvitoDostavkaCheckBox);
      this.Controls.Add((Control) this.MagazCheck);
      this.Controls.Add((Control) this.RaiseAdvertsCheck);
      this.Controls.Add((Control) this.label37);
      this.Controls.Add((Control) this.numericUpDown1);
      this.Controls.Add((Control) this.TelegrammCheckBox);
      this.Controls.Add((Control) this.BeepCheckBox);
      this.Controls.Add((Control) this.BrowserOpenChekBox);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.label35);
      this.Controls.Add((Control) this.label34);
      this.Controls.Add((Control) this.label33);
      this.Controls.Add((Control) this.label32);
      this.Controls.Add((Control) this.label31);
      this.Controls.Add((Control) this.label30);
      this.Controls.Add((Control) this.label29);
      this.Controls.Add((Control) this.label28);
      this.Controls.Add((Control) this.label27);
      this.Controls.Add((Control) this.label26);
      this.Controls.Add((Control) this.label25);
      this.Controls.Add((Control) this.label24);
      this.Controls.Add((Control) this.label23);
      this.Controls.Add((Control) this.label22);
      this.Controls.Add((Control) this.label21);
      this.Controls.Add((Control) this.ObjectiveWords);
      this.Controls.Add((Control) this.ShowStopWordsButton);
      this.Controls.Add((Control) this.checkBox16);
      this.Controls.Add((Control) this.checkBox15);
      this.Controls.Add((Control) this.checkBox14);
      this.Controls.Add((Control) this.checkBox13);
      this.Controls.Add((Control) this.checkBox12);
      this.Controls.Add((Control) this.UrlTextBox16);
      this.Controls.Add((Control) this.UrlTextBox15);
      this.Controls.Add((Control) this.UrlTextBox14);
      this.Controls.Add((Control) this.UrlTextBox13);
      this.Controls.Add((Control) this.UrlTextBox12);
      this.Controls.Add((Control) this.checkBox11);
      this.Controls.Add((Control) this.checkBox10);
      this.Controls.Add((Control) this.checkBox9);
      this.Controls.Add((Control) this.checkBox8);
      this.Controls.Add((Control) this.checkBox7);
      this.Controls.Add((Control) this.checkBox6);
      this.Controls.Add((Control) this.checkBox5);
      this.Controls.Add((Control) this.checkBox4);
      this.Controls.Add((Control) this.checkBox3);
      this.Controls.Add((Control) this.checkBox2);
      this.Controls.Add((Control) this.checkBox1);
      this.Controls.Add((Control) this.UrlTextBox11);
      this.Controls.Add((Control) this.UrlTextBox10);
      this.Controls.Add((Control) this.UrlTextBox9);
      this.Controls.Add((Control) this.UrlTextBox8);
      this.Controls.Add((Control) this.UrlTextBox7);
      this.Controls.Add((Control) this.UrlTextBox6);
      this.Controls.Add((Control) this.UrlTextBox5);
      this.Controls.Add((Control) this.UrlTextBox4);
      this.Controls.Add((Control) this.UrlTextBox3);
      this.Controls.Add((Control) this.UrlTextBox2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.SetButton);
      this.Controls.Add((Control) this.label20);
      this.Controls.Add((Control) this.AdvertsLitsView);
      this.Controls.Add((Control) this.StartButton);
      this.Controls.Add((Control) this.label19);
      this.Controls.Add((Control) this.DvTypeListBox);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.LmaxLabel);
      this.Controls.Add((Control) this.LminLabel);
      this.Controls.Add((Control) this.LitrScrollMax);
      this.Controls.Add((Control) this.LitrScrollMin);
      this.Controls.Add((Control) this.LogBox);
      this.Controls.Add((Control) this.TownComboBox);
      this.Controls.Add((Control) this.UrlTextBox1);
      this.Controls.Add((Control) this.SaveAndValidateButton);
      this.Controls.Add((Control) this.PriceTextBoxMax);
      this.Controls.Add((Control) this.PriceTextBoxMin);
      this.Controls.Add((Control) this.label17);
      this.Controls.Add((Control) this.label18);
      this.Controls.Add((Control) this.label16);
      this.Controls.Add((Control) this.PrivListBox);
      this.Controls.Add((Control) this.label15);
      this.Controls.Add((Control) this.BodyTypeListBox);
      this.Controls.Add((Control) this.TransmissionListBox);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.KmMaxTextBox);
      this.Controls.Add((Control) this.KmMinTextBox);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.YearToTextBox);
      this.Controls.Add((Control) this.YearFromTextBox);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.ModelTextBox);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.BrandComboBox);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(9, 6, 9, 6);
      this.Name = nameof (Form1);
      this.Text = "AvtoRinger - Мониторинг объявлений на Авито, Юле и Olx.ua";
      this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new EventHandler(this.Form1_Load);
      this.ResizeEnd += new EventHandler(this.Form1_ResizeEnd);
      this.Resize += new EventHandler(this.Form1_Resize);
      this.numericUpDown1.EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      ((ISupportInitialize) this.pictureBox3).EndInit();
      ((ISupportInitialize) this.pictureBox4).EndInit();
      ((ISupportInitialize) this.pictureBox5).EndInit();
      ((ISupportInitialize) this.pictureBox6).EndInit();
      ((ISupportInitialize) this.pictureBox7).EndInit();
      ((ISupportInitialize) this.pictureBox8).EndInit();
      ((ISupportInitialize) this.pictureBox9).EndInit();
      ((ISupportInitialize) this.pictureBox10).EndInit();
      ((ISupportInitialize) this.pictureBox11).EndInit();
      ((ISupportInitialize) this.pictureBox12).EndInit();
      ((ISupportInitialize) this.pictureBox13).EndInit();
      ((ISupportInitialize) this.pictureBox14).EndInit();
      ((ISupportInitialize) this.pictureBox15).EndInit();
      ((ISupportInitialize) this.pictureBox16).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((ISupportInitialize) this.AdvertPictureBox).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
