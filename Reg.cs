// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.Reg
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  public class Reg : Form
  {
    private IniFile INIF = new IniFile("Config.ini");
    private Form1 mform;
    private IContainer components = (IContainer) null;
    private GroupBox groupBox1;
    private Button button2;
    private Button button1;
    private TextBox textBox2;
    private Label label2;
    private Label label1;
    private TextBox textBox1;
    private Button button3;
    private Button button4;

    public Reg(Form1 mf)
    {
      this.InitializeComponent();
      this.TopMost = true;
      this.mform = mf;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      int num1 = Reg.deHash(this.textBox2.Text.Trim(), Reg.getHDD());
      StatSetClass.perm = (StatSetClass.Permissions) num1;
      if (num1 > 1)
      {
        this.mform.Enabled = true;
        this.INIF.WriteINI("ACTIVATION", this.textBox1.Text, this.textBox2.Text.Trim());
        this.mform.UrlColorerAvito();
        this.mform.ButtonMakerAdverts();
        this.Close();
      }
      else
      {
        int num2 = (int) MessageBox.Show("Неверный ключ авторизации");
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Сейчас вы будете перенаправлены на сайт программы, в раздел покупка", "", MessageBoxButtons.OKCancel) != DialogResult.OK)
        return;
      Process.Start("http://avtoringer.ru/forum/viewtopic.php?f=2&t=2");
    }

    private void Form2_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (Reg.deHash(this.textBox2.Text, this.textBox1.Text) != 1)
        return;
      StatSetClass.r = false;
      StatSetClass.perm = StatSetClass.Permissions.Demka;
      this.mform.Enabled = true;
      this.mform.CheckBoxDEMO();
    }

    public static string getHDD()
    {
      ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
      string text = "";
      foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
        text = managementBaseObject["ProcessorId"].ToString();
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard").Get())
        text += managementObject["Product"].ToString();
      return Reg.Crypt(text);
    }

    public static string Crypt(string text)
    {
      string empty = string.Empty;
      foreach (char ch in text)
        empty += ((char) ((uint) ch ^ 1U)).ToString();
      return empty;
    }

    public static string GetMd5Hash(MD5 md5Hash, string input)
    {
      byte[] hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("x2"));
      return stringBuilder.ToString();
    }

    public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash) => StringComparer.OrdinalIgnoreCase.Compare(Reg.GetMd5Hash(md5Hash, input), hash) == 0;

    private static string sha1(string input)
    {
      byte[] hash;
      using (SHA1CryptoServiceProvider cryptoServiceProvider = new SHA1CryptoServiceProvider())
        hash = cryptoServiceProvider.ComputeHash(Encoding.Unicode.GetBytes(input));
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in hash)
        stringBuilder.AppendFormat("{0:x2}", (object) num);
      return stringBuilder.ToString();
    }

    public static int deHash(string pass, string val)
    {
      MD5 md5Hash = MD5.Create();
      string[] strArray = StatSetClass.Base64Decode(pass).Split(':');
      if (strArray.Length != 2)
        return 1;
      string str1 = "antivzlom89";
      string str2 = Reg.sha1(Reg.sha1(Reg.GetMd5Hash(md5Hash, val) + str1));
      return strArray[0] == str2 ? int.Parse(strArray[1]) : 1;
    }

    private void Form2_Load(object sender, EventArgs e)
    {
      try
      {
        this.textBox1.Text = Reg.getHDD();
      }
      catch (Exception ex)
      {
        this.textBox1.Text = "Error to generate SYS code!";
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      StatSetClass.r = false;
      StatSetClass.perm = StatSetClass.Permissions.Demka;
      this.mform.Enabled = true;
      this.mform.CheckBoxDEMO();
      this.Close();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void button4_Click(object sender, EventArgs e) => Clipboard.SetText(this.textBox1.Text);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.button4 = new Button();
      this.button3 = new Button();
      this.button2 = new Button();
      this.button1 = new Button();
      this.textBox2 = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.textBox1 = new TextBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.button4);
      this.groupBox1.Controls.Add((Control) this.button3);
      this.groupBox1.Controls.Add((Control) this.button2);
      this.groupBox1.Controls.Add((Control) this.button1);
      this.groupBox1.Controls.Add((Control) this.textBox2);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.textBox1);
      this.groupBox1.Location = new Point(8, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(399, 160);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Авторизация";
      this.button4.BackColor = Color.Aquamarine;
      this.button4.Cursor = Cursors.Hand;
      this.button4.FlatStyle = FlatStyle.Flat;
      this.button4.Location = new Point(268, 60);
      this.button4.Name = "button4";
      this.button4.Size = new Size(109, 22);
      this.button4.TabIndex = 7;
      this.button4.Text = "Скопировать код";
      this.button4.UseVisualStyleBackColor = false;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.button3.Location = new Point(136, 126);
      this.button3.Name = "button3";
      this.button3.Size = new Size(111, 23);
      this.button3.TabIndex = 6;
      this.button3.Text = "Демо - режим";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.button2.Location = new Point(6, 126);
      this.button2.Name = "button2";
      this.button2.Size = new Size(111, 23);
      this.button2.TabIndex = 5;
      this.button2.Text = "Купить ключ";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button1.Location = new Point(268, 126);
      this.button1.Name = "button1";
      this.button1.Size = new Size(109, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "Активировать";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.textBox2.Location = new Point(6, 100);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(371, 20);
      this.textBox2.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 84);
      this.label2.Name = "label2";
      this.label2.Size = new Size(92, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Ключ активации:";
      this.label1.Location = new Point(6, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(371, 41);
      this.label1.TabIndex = 1;
      this.label1.Text = "Ваш уникальный код: (Скидывать только скопировав, вручную введенный может неправильно активироваться, и в отдельном сообщении) ";
      this.textBox1.BackColor = Color.Aquamarine;
      this.textBox1.Location = new Point(6, 61);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(241, 20);
      this.textBox1.TabIndex = 0;
      this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(408, 174);
      this.Controls.Add((Control) this.groupBox1);
      this.Name = nameof (Reg);
      this.Text = "Авторизация";
      this.FormClosing += new FormClosingEventHandler(this.Form2_FormClosing);
      this.Load += new EventHandler(this.Form2_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
