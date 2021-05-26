// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.StopWords
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  public class StopWords : Form
  {
    private IniFile INI = new IniFile("Config.ini");
    private IContainer components = (IContainer) null;
    private TabControl TabControlStopWords;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private TextBox Url1StopWordsTextBox;
    private TabPage tabPage3;
    private TabPage tabPage4;
    private TabPage tabPage5;
    private TabPage tabPage6;
    private TabPage tabPage7;
    private TabPage tabPage8;
    private TabPage tabPage9;
    private TabPage tabPage10;
    private TabPage tabPage11;
    private TabPage tabPage12;
    private TabPage tabPage13;
    private TabPage tabPage14;
    private TabPage tabPage15;
    private TabPage tabPage16;
    private TextBox Url2StopWordsTextBox;
    private TextBox Url3StopWordsTextBox;
    private TextBox Url4StopWordsTextBox;
    private TextBox Url5StopWordsTextBox;
    private TextBox Url6StopWordsTextBox;
    private TextBox Url7StopWordsTextBox;
    private TextBox Url8StopWordsTextBox;
    private TextBox Url9StopWordsTextBox;
    private TextBox Url10StopWordsTextBox;
    private TextBox Url11StopWordsTextBox;
    private TextBox Url12StopWordsTextBox;
    private TextBox Url13StopWordsTextBox;
    private TextBox Url14StopWordsTextBox;
    private TextBox Url15StopWordsTextBox;
    private TextBox Url16StopWordsTextBox;
    private Button button2;
    private Button button3;
    private Button button4;

    public StopWords() => this.InitializeComponent();

    private void Url1StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[0] = this.Url1StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL1", this.Url1StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private static void AllWordsMaker()
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
      if (!(StatSetClass.stopWordsList[15] != ""))
        return;
      StatSetClass.allstopWords = StatSetClass.allstopWords + StatSetClass.stopWordsList[15] + "|";
    }

    private void StopWords_Load(object sender, EventArgs e)
    {
      /*this.Url1StopWordsTextBox.Text = StatSetClass.stopWordsList[0];
      this.Url2StopWordsTextBox.Text = StatSetClass.stopWordsList[1];
      this.Url3StopWordsTextBox.Text = StatSetClass.stopWordsList[2];
      this.Url4StopWordsTextBox.Text = StatSetClass.stopWordsList[3];
      this.Url5StopWordsTextBox.Text = StatSetClass.stopWordsList[4];
      this.Url6StopWordsTextBox.Text = StatSetClass.stopWordsList[5];
      this.Url7StopWordsTextBox.Text = StatSetClass.stopWordsList[6];
      this.Url8StopWordsTextBox.Text = StatSetClass.stopWordsList[7];
      this.Url9StopWordsTextBox.Text = StatSetClass.stopWordsList[8];
      this.Url10StopWordsTextBox.Text = StatSetClass.stopWordsList[9];
      this.Url11StopWordsTextBox.Text = StatSetClass.stopWordsList[10];
      this.Url12StopWordsTextBox.Text = StatSetClass.stopWordsList[11];
      this.Url13StopWordsTextBox.Text = StatSetClass.stopWordsList[12];
      this.Url14StopWordsTextBox.Text = StatSetClass.stopWordsList[13];
      this.Url15StopWordsTextBox.Text = StatSetClass.stopWordsList[14];
      this.Url16StopWordsTextBox.Text = StatSetClass.stopWordsList[15];
      */
      this.Url1StopWordsTextBox.Font = this.Url2StopWordsTextBox.Font = this.Url3StopWordsTextBox.Font = this.Url4StopWordsTextBox.Font = this.Url5StopWordsTextBox.Font = this.Url6StopWordsTextBox.Font = this.Url7StopWordsTextBox.Font = this.Url8StopWordsTextBox.Font = this.Url9StopWordsTextBox.Font = this.Url10StopWordsTextBox.Font = this.Url11StopWordsTextBox.Font = this.Url12StopWordsTextBox.Font = this.Url13StopWordsTextBox.Font = this.Url14StopWordsTextBox.Font = this.Url15StopWordsTextBox.Font = this.Url16StopWordsTextBox.Font = new Font(FontFamily.GenericSansSerif, 12f);
      this.Url1StopWordsTextBox.ScrollBars = this.Url2StopWordsTextBox.ScrollBars = this.Url3StopWordsTextBox.ScrollBars = this.Url4StopWordsTextBox.ScrollBars = this.Url5StopWordsTextBox.ScrollBars = this.Url6StopWordsTextBox.ScrollBars = this.Url7StopWordsTextBox.ScrollBars = this.Url8StopWordsTextBox.ScrollBars = this.Url9StopWordsTextBox.ScrollBars = this.Url10StopWordsTextBox.ScrollBars = this.Url11StopWordsTextBox.ScrollBars = this.Url12StopWordsTextBox.ScrollBars = this.Url13StopWordsTextBox.ScrollBars = this.Url14StopWordsTextBox.ScrollBars = this.Url15StopWordsTextBox.ScrollBars = this.Url16StopWordsTextBox.ScrollBars = ScrollBars.Vertical;
    }

    private void Url2StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[1] = this.Url2StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL2", this.Url2StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url3StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[2] = this.Url3StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL3", this.Url3StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url4StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[3] = this.Url4StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL4", this.Url4StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url5StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[4] = this.Url5StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL5", this.Url5StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url6StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[5] = this.Url6StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL6", this.Url6StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url7StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[6] = this.Url7StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL7", this.Url7StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url8StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[7] = this.Url8StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL8", this.Url8StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url9StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[8] = this.Url9StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL9", this.Url9StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url10StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[9] = this.Url10StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL10", this.Url10StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url11StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[10] = this.Url11StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL11", this.Url11StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url12StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[11] = this.Url12StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL12", this.Url12StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url13StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[12] = this.Url13StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL13", this.Url13StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url14StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[13] = this.Url14StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL14", this.Url14StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url15StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[14] = this.Url15StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL15", this.Url15StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void Url16StopWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.stopWordsList[15] = this.Url16StopWordsTextBox.Text;
      this.INI.WriteINI("STOPWORDS", "URL16", this.Url16StopWordsTextBox.Text.Trim());
      StopWords.AllWordsMaker();
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void StopWords_MouseDown(object sender, MouseEventArgs e)
    {
      this.Capture = false;
      Message m = Message.Create(this.Handle, 161, new IntPtr(2), IntPtr.Zero);
      this.WndProc(ref m);
    }

    private void button2_Click(object sender, EventArgs e) => this.Close();

    private void button3_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

    private string CloneWords(string words)
    {
      string[] strArray = words.ToLower().Split('|');
      List<string> stringList = new List<string>();
      foreach (string str1 in strArray)
      {
        bool flag = false;
        foreach (string str2 in stringList)
        {
          if (str1 == str2)
            flag = true;
        }
        if (!flag)
          stringList.Add(str1);
      }
      return string.Join("|", (IEnumerable<string>) stringList);
    }

    private void button4_Click(object sender, EventArgs e)
    {
      switch (this.TabControlStopWords.SelectedIndex)
      {
        case 0:
          this.Url1StopWordsTextBox.Text = this.Url1StopWordsTextBox.Text.Insert(this.Url1StopWordsTextBox.SelectionStart, "|");
          this.Url1StopWordsTextBox.Text = this.CloneWords(this.Url1StopWordsTextBox.Text);
          break;
        case 1:
          this.Url2StopWordsTextBox.Text = this.Url2StopWordsTextBox.Text.Insert(this.Url2StopWordsTextBox.SelectionStart, "|");
          this.Url2StopWordsTextBox.Text = this.CloneWords(this.Url2StopWordsTextBox.Text);
          break;
        case 2:
          this.Url3StopWordsTextBox.Text = this.Url3StopWordsTextBox.Text.Insert(this.Url3StopWordsTextBox.SelectionStart, "|");
          this.Url3StopWordsTextBox.Text = this.CloneWords(this.Url3StopWordsTextBox.Text);
          break;
        case 3:
          this.Url4StopWordsTextBox.Text = this.Url4StopWordsTextBox.Text.Insert(this.Url4StopWordsTextBox.SelectionStart, "|");
          this.Url4StopWordsTextBox.Text = this.CloneWords(this.Url4StopWordsTextBox.Text);
          break;
        case 4:
          this.Url5StopWordsTextBox.Text = this.Url5StopWordsTextBox.Text.Insert(this.Url5StopWordsTextBox.SelectionStart, "|");
          this.Url5StopWordsTextBox.Text = this.CloneWords(this.Url5StopWordsTextBox.Text);
          break;
        case 5:
          this.Url6StopWordsTextBox.Text = this.Url6StopWordsTextBox.Text.Insert(this.Url6StopWordsTextBox.SelectionStart, "|");
          this.Url6StopWordsTextBox.Text = this.CloneWords(this.Url6StopWordsTextBox.Text);
          break;
        case 6:
          this.Url7StopWordsTextBox.Text = this.Url7StopWordsTextBox.Text.Insert(this.Url7StopWordsTextBox.SelectionStart, "|");
          this.Url7StopWordsTextBox.Text = this.CloneWords(this.Url7StopWordsTextBox.Text);
          break;
        case 7:
          this.Url8StopWordsTextBox.Text = this.Url8StopWordsTextBox.Text.Insert(this.Url8StopWordsTextBox.SelectionStart, "|");
          this.Url8StopWordsTextBox.Text = this.CloneWords(this.Url8StopWordsTextBox.Text);
          break;
        case 8:
          this.Url9StopWordsTextBox.Text = this.Url9StopWordsTextBox.Text.Insert(this.Url9StopWordsTextBox.SelectionStart, "|");
          this.Url9StopWordsTextBox.Text = this.CloneWords(this.Url9StopWordsTextBox.Text);
          break;
        case 9:
          this.Url10StopWordsTextBox.Text = this.Url10StopWordsTextBox.Text.Insert(this.Url10StopWordsTextBox.SelectionStart, "|");
          this.Url10StopWordsTextBox.Text = this.CloneWords(this.Url10StopWordsTextBox.Text);
          break;
        case 10:
          this.Url11StopWordsTextBox.Text = this.Url11StopWordsTextBox.Text.Insert(this.Url11StopWordsTextBox.SelectionStart, "|");
          this.Url11StopWordsTextBox.Text = this.CloneWords(this.Url11StopWordsTextBox.Text);
          break;
        case 11:
          this.Url12StopWordsTextBox.Text = this.Url12StopWordsTextBox.Text.Insert(this.Url12StopWordsTextBox.SelectionStart, "|");
          this.Url12StopWordsTextBox.Text = this.CloneWords(this.Url12StopWordsTextBox.Text);
          break;
        case 12:
          this.Url13StopWordsTextBox.Text = this.Url13StopWordsTextBox.Text.Insert(this.Url13StopWordsTextBox.SelectionStart, "|");
          this.Url13StopWordsTextBox.Text = this.CloneWords(this.Url13StopWordsTextBox.Text);
          break;
        case 13:
          this.Url14StopWordsTextBox.Text = this.Url14StopWordsTextBox.Text.Insert(this.Url14StopWordsTextBox.SelectionStart, "|");
          this.Url14StopWordsTextBox.Text = this.CloneWords(this.Url14StopWordsTextBox.Text);
          break;
        case 14:
          this.Url15StopWordsTextBox.Text = this.Url15StopWordsTextBox.Text.Insert(this.Url15StopWordsTextBox.SelectionStart, "|");
          this.Url15StopWordsTextBox.Text = this.CloneWords(this.Url15StopWordsTextBox.Text);
          break;
        case 15:
          this.Url16StopWordsTextBox.Text = this.Url16StopWordsTextBox.Text.Insert(this.Url16StopWordsTextBox.SelectionStart, "|");
          this.Url16StopWordsTextBox.Text = this.CloneWords(this.Url16StopWordsTextBox.Text);
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.TabControlStopWords = new TabControl();
      this.tabPage1 = new TabPage();
      this.Url1StopWordsTextBox = new TextBox();
      this.tabPage2 = new TabPage();
      this.Url2StopWordsTextBox = new TextBox();
      this.tabPage3 = new TabPage();
      this.Url3StopWordsTextBox = new TextBox();
      this.tabPage4 = new TabPage();
      this.Url4StopWordsTextBox = new TextBox();
      this.tabPage5 = new TabPage();
      this.Url5StopWordsTextBox = new TextBox();
      this.tabPage6 = new TabPage();
      this.Url6StopWordsTextBox = new TextBox();
      this.tabPage7 = new TabPage();
      this.Url7StopWordsTextBox = new TextBox();
      this.tabPage8 = new TabPage();
      this.Url8StopWordsTextBox = new TextBox();
      this.tabPage9 = new TabPage();
      this.Url9StopWordsTextBox = new TextBox();
      this.tabPage10 = new TabPage();
      this.Url10StopWordsTextBox = new TextBox();
      this.tabPage11 = new TabPage();
      this.Url11StopWordsTextBox = new TextBox();
      this.tabPage12 = new TabPage();
      this.Url12StopWordsTextBox = new TextBox();
      this.tabPage13 = new TabPage();
      this.Url13StopWordsTextBox = new TextBox();
      this.tabPage14 = new TabPage();
      this.Url14StopWordsTextBox = new TextBox();
      this.tabPage15 = new TabPage();
      this.Url15StopWordsTextBox = new TextBox();
      this.tabPage16 = new TabPage();
      this.Url16StopWordsTextBox = new TextBox();
      this.button2 = new Button();
      this.button3 = new Button();
      this.button4 = new Button();
      this.TabControlStopWords.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.tabPage5.SuspendLayout();
      this.tabPage6.SuspendLayout();
      this.tabPage7.SuspendLayout();
      this.tabPage8.SuspendLayout();
      this.tabPage9.SuspendLayout();
      this.tabPage10.SuspendLayout();
      this.tabPage11.SuspendLayout();
      this.tabPage12.SuspendLayout();
      this.tabPage13.SuspendLayout();
      this.tabPage14.SuspendLayout();
      this.tabPage15.SuspendLayout();
      this.tabPage16.SuspendLayout();
      this.SuspendLayout();
      this.TabControlStopWords.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.TabControlStopWords.Controls.Add((Control) this.tabPage1);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage2);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage3);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage4);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage5);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage6);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage7);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage8);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage9);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage10);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage11);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage12);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage13);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage14);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage15);
      this.TabControlStopWords.Controls.Add((Control) this.tabPage16);
      this.TabControlStopWords.Location = new Point(2, 24);
      this.TabControlStopWords.Name = "TabControlStopWords";
      this.TabControlStopWords.SelectedIndex = 0;
      this.TabControlStopWords.Size = new Size(900, 546);
      this.TabControlStopWords.TabIndex = 0;
      this.tabPage1.Controls.Add((Control) this.Url1StopWordsTextBox);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(892, 520);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Поиск 1";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.Url1StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url1StopWordsTextBox.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Url1StopWordsTextBox.Location = new Point(6, 6);
      this.Url1StopWordsTextBox.Multiline = true;
      this.Url1StopWordsTextBox.Name = "Url1StopWordsTextBox";
      this.Url1StopWordsTextBox.ScrollBars = ScrollBars.Vertical;
      this.Url1StopWordsTextBox.Size = new Size(880, 502);
      this.Url1StopWordsTextBox.TabIndex = 0;
      this.Url1StopWordsTextBox.TextChanged += new EventHandler(this.Url1StopWordsTextBox_TextChanged);
      this.tabPage2.Controls.Add((Control) this.Url2StopWordsTextBox);
      this.tabPage2.Location = new Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(892, 520);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Поиск 2";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.Url2StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url2StopWordsTextBox.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Url2StopWordsTextBox.Location = new Point(6, 6);
      this.Url2StopWordsTextBox.Multiline = true;
      this.Url2StopWordsTextBox.Name = "Url2StopWordsTextBox";
      this.Url2StopWordsTextBox.Size = new Size(880, 502);
      this.Url2StopWordsTextBox.TabIndex = 1;
      this.Url2StopWordsTextBox.TextChanged += new EventHandler(this.Url2StopWordsTextBox_TextChanged);
      this.tabPage3.Controls.Add((Control) this.Url3StopWordsTextBox);
      this.tabPage3.Location = new Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Size = new Size(892, 520);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Поиск 3";
      this.tabPage3.UseVisualStyleBackColor = true;
      this.Url3StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url3StopWordsTextBox.Location = new Point(6, 6);
      this.Url3StopWordsTextBox.Multiline = true;
      this.Url3StopWordsTextBox.Name = "Url3StopWordsTextBox";
      this.Url3StopWordsTextBox.Size = new Size(880, 502);
      this.Url3StopWordsTextBox.TabIndex = 1;
      this.Url3StopWordsTextBox.TextChanged += new EventHandler(this.Url3StopWordsTextBox_TextChanged);
      this.tabPage4.Controls.Add((Control) this.Url4StopWordsTextBox);
      this.tabPage4.Location = new Point(4, 22);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Size = new Size(892, 520);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "Поиск 4";
      this.tabPage4.UseVisualStyleBackColor = true;
      this.Url4StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url4StopWordsTextBox.Location = new Point(6, 6);
      this.Url4StopWordsTextBox.Multiline = true;
      this.Url4StopWordsTextBox.Name = "Url4StopWordsTextBox";
      this.Url4StopWordsTextBox.Size = new Size(880, 501);
      this.Url4StopWordsTextBox.TabIndex = 1;
      this.Url4StopWordsTextBox.TextChanged += new EventHandler(this.Url4StopWordsTextBox_TextChanged);
      this.tabPage5.Controls.Add((Control) this.Url5StopWordsTextBox);
      this.tabPage5.Location = new Point(4, 22);
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.Size = new Size(892, 520);
      this.tabPage5.TabIndex = 4;
      this.tabPage5.Text = "Поиск 5";
      this.tabPage5.UseVisualStyleBackColor = true;
      this.Url5StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url5StopWordsTextBox.Location = new Point(6, 6);
      this.Url5StopWordsTextBox.Multiline = true;
      this.Url5StopWordsTextBox.Name = "Url5StopWordsTextBox";
      this.Url5StopWordsTextBox.Size = new Size(880, 501);
      this.Url5StopWordsTextBox.TabIndex = 1;
      this.Url5StopWordsTextBox.TextChanged += new EventHandler(this.Url5StopWordsTextBox_TextChanged);
      this.tabPage6.Controls.Add((Control) this.Url6StopWordsTextBox);
      this.tabPage6.Location = new Point(4, 22);
      this.tabPage6.Name = "tabPage6";
      this.tabPage6.Size = new Size(892, 520);
      this.tabPage6.TabIndex = 5;
      this.tabPage6.Text = "Поиск 6";
      this.tabPage6.UseVisualStyleBackColor = true;
      this.Url6StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url6StopWordsTextBox.Location = new Point(6, 6);
      this.Url6StopWordsTextBox.Multiline = true;
      this.Url6StopWordsTextBox.Name = "Url6StopWordsTextBox";
      this.Url6StopWordsTextBox.Size = new Size(880, 501);
      this.Url6StopWordsTextBox.TabIndex = 1;
      this.Url6StopWordsTextBox.TextChanged += new EventHandler(this.Url6StopWordsTextBox_TextChanged);
      this.tabPage7.Controls.Add((Control) this.Url7StopWordsTextBox);
      this.tabPage7.Location = new Point(4, 22);
      this.tabPage7.Name = "tabPage7";
      this.tabPage7.Size = new Size(892, 520);
      this.tabPage7.TabIndex = 6;
      this.tabPage7.Text = "Поиск 7";
      this.tabPage7.UseVisualStyleBackColor = true;
      this.Url7StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url7StopWordsTextBox.Location = new Point(6, 6);
      this.Url7StopWordsTextBox.Multiline = true;
      this.Url7StopWordsTextBox.Name = "Url7StopWordsTextBox";
      this.Url7StopWordsTextBox.Size = new Size(880, 501);
      this.Url7StopWordsTextBox.TabIndex = 1;
      this.Url7StopWordsTextBox.TextChanged += new EventHandler(this.Url7StopWordsTextBox_TextChanged);
      this.tabPage8.Controls.Add((Control) this.Url8StopWordsTextBox);
      this.tabPage8.Location = new Point(4, 22);
      this.tabPage8.Name = "tabPage8";
      this.tabPage8.Size = new Size(892, 520);
      this.tabPage8.TabIndex = 7;
      this.tabPage8.Text = "Поиск 8";
      this.tabPage8.UseVisualStyleBackColor = true;
      this.Url8StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url8StopWordsTextBox.Location = new Point(6, 6);
      this.Url8StopWordsTextBox.Multiline = true;
      this.Url8StopWordsTextBox.Name = "Url8StopWordsTextBox";
      this.Url8StopWordsTextBox.Size = new Size(880, 501);
      this.Url8StopWordsTextBox.TabIndex = 1;
      this.Url8StopWordsTextBox.TextChanged += new EventHandler(this.Url8StopWordsTextBox_TextChanged);
      this.tabPage9.Controls.Add((Control) this.Url9StopWordsTextBox);
      this.tabPage9.Location = new Point(4, 22);
      this.tabPage9.Name = "tabPage9";
      this.tabPage9.Size = new Size(892, 520);
      this.tabPage9.TabIndex = 8;
      this.tabPage9.Text = "Поиск 9";
      this.tabPage9.UseVisualStyleBackColor = true;
      this.Url9StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url9StopWordsTextBox.Location = new Point(6, 6);
      this.Url9StopWordsTextBox.Multiline = true;
      this.Url9StopWordsTextBox.Name = "Url9StopWordsTextBox";
      this.Url9StopWordsTextBox.Size = new Size(880, 501);
      this.Url9StopWordsTextBox.TabIndex = 1;
      this.Url9StopWordsTextBox.TextChanged += new EventHandler(this.Url9StopWordsTextBox_TextChanged);
      this.tabPage10.Controls.Add((Control) this.Url10StopWordsTextBox);
      this.tabPage10.Location = new Point(4, 22);
      this.tabPage10.Name = "tabPage10";
      this.tabPage10.Size = new Size(892, 520);
      this.tabPage10.TabIndex = 9;
      this.tabPage10.Text = "Поиск 10";
      this.tabPage10.UseVisualStyleBackColor = true;
      this.Url10StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url10StopWordsTextBox.Location = new Point(6, 6);
      this.Url10StopWordsTextBox.Multiline = true;
      this.Url10StopWordsTextBox.Name = "Url10StopWordsTextBox";
      this.Url10StopWordsTextBox.Size = new Size(880, 501);
      this.Url10StopWordsTextBox.TabIndex = 1;
      this.Url10StopWordsTextBox.TextChanged += new EventHandler(this.Url10StopWordsTextBox_TextChanged);
      this.tabPage11.Controls.Add((Control) this.Url11StopWordsTextBox);
      this.tabPage11.Location = new Point(4, 22);
      this.tabPage11.Name = "tabPage11";
      this.tabPage11.Size = new Size(892, 520);
      this.tabPage11.TabIndex = 10;
      this.tabPage11.Text = "Поиск 11";
      this.tabPage11.UseVisualStyleBackColor = true;
      this.Url11StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url11StopWordsTextBox.Location = new Point(6, 6);
      this.Url11StopWordsTextBox.Multiline = true;
      this.Url11StopWordsTextBox.Name = "Url11StopWordsTextBox";
      this.Url11StopWordsTextBox.Size = new Size(880, 501);
      this.Url11StopWordsTextBox.TabIndex = 1;
      this.Url11StopWordsTextBox.TextChanged += new EventHandler(this.Url11StopWordsTextBox_TextChanged);
      this.tabPage12.Controls.Add((Control) this.Url12StopWordsTextBox);
      this.tabPage12.Location = new Point(4, 22);
      this.tabPage12.Name = "tabPage12";
      this.tabPage12.Size = new Size(892, 520);
      this.tabPage12.TabIndex = 11;
      this.tabPage12.Text = "Поиск 12";
      this.tabPage12.UseVisualStyleBackColor = true;
      this.Url12StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url12StopWordsTextBox.Location = new Point(6, 6);
      this.Url12StopWordsTextBox.Multiline = true;
      this.Url12StopWordsTextBox.Name = "Url12StopWordsTextBox";
      this.Url12StopWordsTextBox.Size = new Size(880, 501);
      this.Url12StopWordsTextBox.TabIndex = 1;
      this.Url12StopWordsTextBox.TextChanged += new EventHandler(this.Url12StopWordsTextBox_TextChanged);
      this.tabPage13.Controls.Add((Control) this.Url13StopWordsTextBox);
      this.tabPage13.Location = new Point(4, 22);
      this.tabPage13.Name = "tabPage13";
      this.tabPage13.Size = new Size(892, 520);
      this.tabPage13.TabIndex = 12;
      this.tabPage13.Text = "Поиск 13";
      this.tabPage13.UseVisualStyleBackColor = true;
      this.Url13StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url13StopWordsTextBox.Location = new Point(6, 6);
      this.Url13StopWordsTextBox.Multiline = true;
      this.Url13StopWordsTextBox.Name = "Url13StopWordsTextBox";
      this.Url13StopWordsTextBox.Size = new Size(880, 501);
      this.Url13StopWordsTextBox.TabIndex = 1;
      this.Url13StopWordsTextBox.TextChanged += new EventHandler(this.Url13StopWordsTextBox_TextChanged);
      this.tabPage14.Controls.Add((Control) this.Url14StopWordsTextBox);
      this.tabPage14.Location = new Point(4, 22);
      this.tabPage14.Name = "tabPage14";
      this.tabPage14.Size = new Size(892, 520);
      this.tabPage14.TabIndex = 13;
      this.tabPage14.Text = "Поиск 14";
      this.tabPage14.UseVisualStyleBackColor = true;
      this.Url14StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url14StopWordsTextBox.Location = new Point(6, 6);
      this.Url14StopWordsTextBox.Multiline = true;
      this.Url14StopWordsTextBox.Name = "Url14StopWordsTextBox";
      this.Url14StopWordsTextBox.Size = new Size(880, 501);
      this.Url14StopWordsTextBox.TabIndex = 1;
      this.Url14StopWordsTextBox.TextChanged += new EventHandler(this.Url14StopWordsTextBox_TextChanged);
      this.tabPage15.Controls.Add((Control) this.Url15StopWordsTextBox);
      this.tabPage15.Location = new Point(4, 22);
      this.tabPage15.Name = "tabPage15";
      this.tabPage15.Size = new Size(892, 520);
      this.tabPage15.TabIndex = 14;
      this.tabPage15.Text = "Поиск 15";
      this.tabPage15.UseVisualStyleBackColor = true;
      this.Url15StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url15StopWordsTextBox.Location = new Point(6, 6);
      this.Url15StopWordsTextBox.Multiline = true;
      this.Url15StopWordsTextBox.Name = "Url15StopWordsTextBox";
      this.Url15StopWordsTextBox.Size = new Size(880, 501);
      this.Url15StopWordsTextBox.TabIndex = 1;
      this.Url15StopWordsTextBox.TextChanged += new EventHandler(this.Url15StopWordsTextBox_TextChanged);
      this.tabPage16.Controls.Add((Control) this.Url16StopWordsTextBox);
      this.tabPage16.Location = new Point(4, 22);
      this.tabPage16.Name = "tabPage16";
      this.tabPage16.Size = new Size(892, 520);
      this.tabPage16.TabIndex = 15;
      this.tabPage16.Text = "Поиск 16";
      this.tabPage16.UseVisualStyleBackColor = true;
      this.Url16StopWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url16StopWordsTextBox.Location = new Point(6, 6);
      this.Url16StopWordsTextBox.Multiline = true;
      this.Url16StopWordsTextBox.Name = "Url16StopWordsTextBox";
      this.Url16StopWordsTextBox.Size = new Size(880, 501);
      this.Url16StopWordsTextBox.TabIndex = 1;
      this.Url16StopWordsTextBox.TextChanged += new EventHandler(this.Url16StopWordsTextBox_TextChanged);
      this.button2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.button2.Location = new Point(879, 1);
      this.button2.Name = "button2";
      this.button2.Size = new Size(20, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "X";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.button3.Location = new Point(858, 1);
      this.button3.Name = "button3";
      this.button3.Size = new Size(20, 23);
      this.button3.TabIndex = 2;
      this.button3.Text = "_";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.button4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.button4.Location = new Point(631, 1);
      this.button4.Name = "button4";
      this.button4.Size = new Size(225, 23);
      this.button4.TabIndex = 3;
      this.button4.Text = "Вставить знак разделения слов '|'";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(902, 570);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.TabControlStopWords);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (StopWords);
      this.Text = nameof (StopWords);
      this.Load += new EventHandler(this.StopWords_Load);
      this.MouseDown += new MouseEventHandler(this.StopWords_MouseDown);
      this.TabControlStopWords.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.tabPage3.PerformLayout();
      this.tabPage4.ResumeLayout(false);
      this.tabPage4.PerformLayout();
      this.tabPage5.ResumeLayout(false);
      this.tabPage5.PerformLayout();
      this.tabPage6.ResumeLayout(false);
      this.tabPage6.PerformLayout();
      this.tabPage7.ResumeLayout(false);
      this.tabPage7.PerformLayout();
      this.tabPage8.ResumeLayout(false);
      this.tabPage8.PerformLayout();
      this.tabPage9.ResumeLayout(false);
      this.tabPage9.PerformLayout();
      this.tabPage10.ResumeLayout(false);
      this.tabPage10.PerformLayout();
      this.tabPage11.ResumeLayout(false);
      this.tabPage11.PerformLayout();
      this.tabPage12.ResumeLayout(false);
      this.tabPage12.PerformLayout();
      this.tabPage13.ResumeLayout(false);
      this.tabPage13.PerformLayout();
      this.tabPage14.ResumeLayout(false);
      this.tabPage14.PerformLayout();
      this.tabPage15.ResumeLayout(false);
      this.tabPage15.PerformLayout();
      this.tabPage16.ResumeLayout(false);
      this.tabPage16.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
