// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.GoalWords
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  public class GoalWords : Form
  {
    private IniFile INI = new IniFile("Config.ini");
    private IContainer components = (IContainer) null;
    private TextBox Url16GoalWordsTextBox;
    private TextBox Url15GoalWordsTextBox;
    private TabPage tabPage1;
    private TextBox Url1GoalWordsTextBox;
    private TextBox Url14GoalWordsTextBox;
    private TabPage tabPage2;
    private TextBox Url2GoalWordsTextBox;
    private TextBox Url13GoalWordsTextBox;
    private TabPage tabPage3;
    private TextBox Url3GoalWordsTextBox;
    private TextBox Url12GoalWordsTextBox;
    private TabPage tabPage4;
    private TextBox Url4GoalWordsTextBox;
    private TextBox Url11GoalWordsTextBox;
    private TabPage tabPage5;
    private TextBox Url5GoalWordsTextBox;
    private TextBox Url10GoalWordsTextBox;
    private TabPage tabPage6;
    private TextBox Url6GoalWordsTextBox;
    private TextBox Url9GoalWordsTextBox;
    private TabPage tabPage7;
    private TextBox Url7GoalWordsTextBox;
    private TextBox Url8GoalWordsTextBox;
    private TabPage tabPage8;
    private TabControl TabControlGoalWords;
    private TabPage tabPage9;
    private TabPage tabPage10;
    private TabPage tabPage11;
    private TabPage tabPage12;
    private TabPage tabPage13;
    private TabPage tabPage14;
    private TabPage tabPage15;
    private TabPage tabPage16;
    private Button button4;
    private Button button3;
    private Button button2;

    public GoalWords() => this.InitializeComponent();

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

    private void Url1GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[0] = this.Url1GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL1", this.Url1GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url2GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[1] = this.Url2GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL2", this.Url2GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url3GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[2] = this.Url3GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL3", this.Url3GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url4GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[3] = this.Url4GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL4", this.Url4GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url5GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[4] = this.Url5GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL5", this.Url5GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url6GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[5] = this.Url6GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL6", this.Url6GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url7GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[6] = this.Url7GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL7", this.Url7GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url8GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[7] = this.Url8GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL8", this.Url8GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url9GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[8] = this.Url9GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL9", this.Url9GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url10GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[9] = this.Url10GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL10", this.Url10GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url11GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[10] = this.Url11GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL11", this.Url11GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url12GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[11] = this.Url12GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL12", this.Url12GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url13GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[12] = this.Url13GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL13", this.Url13GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url14GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[13] = this.Url14GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL14", this.Url14GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url15GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[14] = this.Url15GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL15", this.Url15GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void Url16GoalWordsTextBox_TextChanged(object sender, EventArgs e)
    {
      StatSetClass.goalWordsList[15] = this.Url16GoalWordsTextBox.Text;
      this.INI.WriteINI("GOALWORDS", "URL16", this.Url16GoalWordsTextBox.Text.Trim());
      GoalWords.AllWordsMaker();
    }

    private void GoalWords_Load(object sender, EventArgs e)
    {
      /*this.Url1GoalWordsTextBox.Text = StatSetClass.goalWordsList[0];
      this.Url2GoalWordsTextBox.Text = StatSetClass.goalWordsList[1];
      this.Url3GoalWordsTextBox.Text = StatSetClass.goalWordsList[2];
      this.Url4GoalWordsTextBox.Text = StatSetClass.goalWordsList[3];
      this.Url5GoalWordsTextBox.Text = StatSetClass.goalWordsList[4];
      this.Url6GoalWordsTextBox.Text = StatSetClass.goalWordsList[5];
      this.Url7GoalWordsTextBox.Text = StatSetClass.goalWordsList[6];
      this.Url8GoalWordsTextBox.Text = StatSetClass.goalWordsList[7];
      this.Url9GoalWordsTextBox.Text = StatSetClass.goalWordsList[8];
      this.Url10GoalWordsTextBox.Text = StatSetClass.goalWordsList[9];
      this.Url11GoalWordsTextBox.Text = StatSetClass.goalWordsList[10];
      this.Url12GoalWordsTextBox.Text = StatSetClass.goalWordsList[11];
      this.Url13GoalWordsTextBox.Text = StatSetClass.goalWordsList[12];
      this.Url14GoalWordsTextBox.Text = StatSetClass.goalWordsList[13];
      this.Url15GoalWordsTextBox.Text = StatSetClass.goalWordsList[14];
      this.Url16GoalWordsTextBox.Text = StatSetClass.goalWordsList[15];
      */
      this.Url1GoalWordsTextBox.Font = this.Url2GoalWordsTextBox.Font = this.Url3GoalWordsTextBox.Font = this.Url4GoalWordsTextBox.Font = this.Url5GoalWordsTextBox.Font = this.Url6GoalWordsTextBox.Font = this.Url7GoalWordsTextBox.Font = this.Url8GoalWordsTextBox.Font = this.Url9GoalWordsTextBox.Font = this.Url10GoalWordsTextBox.Font = this.Url11GoalWordsTextBox.Font = this.Url12GoalWordsTextBox.Font = this.Url13GoalWordsTextBox.Font = this.Url14GoalWordsTextBox.Font = this.Url15GoalWordsTextBox.Font = this.Url16GoalWordsTextBox.Font = new Font(FontFamily.GenericSansSerif, 12f);
      this.Url1GoalWordsTextBox.ScrollBars = this.Url2GoalWordsTextBox.ScrollBars = this.Url3GoalWordsTextBox.ScrollBars = this.Url4GoalWordsTextBox.ScrollBars = this.Url5GoalWordsTextBox.ScrollBars = this.Url6GoalWordsTextBox.ScrollBars = this.Url7GoalWordsTextBox.ScrollBars = this.Url8GoalWordsTextBox.ScrollBars = this.Url9GoalWordsTextBox.ScrollBars = this.Url10GoalWordsTextBox.ScrollBars = this.Url11GoalWordsTextBox.ScrollBars = this.Url12GoalWordsTextBox.ScrollBars = this.Url13GoalWordsTextBox.ScrollBars = this.Url14GoalWordsTextBox.ScrollBars = this.Url15GoalWordsTextBox.ScrollBars = this.Url16GoalWordsTextBox.ScrollBars = ScrollBars.Vertical;
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void button4_Click(object sender, EventArgs e)
    {
      switch (this.TabControlGoalWords.SelectedIndex)
      {
        case 0:
          this.Url1GoalWordsTextBox.AppendText("|");
          break;
        case 1:
          this.Url2GoalWordsTextBox.AppendText("|");
          break;
        case 2:
          this.Url3GoalWordsTextBox.AppendText("|");
          break;
        case 3:
          this.Url4GoalWordsTextBox.AppendText("|");
          break;
        case 4:
          this.Url5GoalWordsTextBox.AppendText("|");
          break;
        case 5:
          this.Url6GoalWordsTextBox.AppendText("|");
          break;
        case 6:
          this.Url7GoalWordsTextBox.AppendText("|");
          break;
        case 7:
          this.Url8GoalWordsTextBox.AppendText("|");
          break;
        case 8:
          this.Url9GoalWordsTextBox.AppendText("|");
          break;
        case 9:
          this.Url10GoalWordsTextBox.AppendText("|");
          break;
        case 10:
          this.Url11GoalWordsTextBox.AppendText("|");
          break;
        case 11:
          this.Url12GoalWordsTextBox.AppendText("|");
          break;
        case 12:
          this.Url13GoalWordsTextBox.AppendText("|");
          break;
        case 13:
          this.Url14GoalWordsTextBox.AppendText("|");
          break;
        case 14:
          this.Url15GoalWordsTextBox.AppendText("|");
          break;
        case 15:
          this.Url16GoalWordsTextBox.AppendText("|");
          break;
      }
    }

    private void GoalWords_MouseDown(object sender, MouseEventArgs e)
    {
      this.Capture = false;
      Message m = Message.Create(this.Handle, 161, new IntPtr(2), IntPtr.Zero);
      this.WndProc(ref m);
    }

    private void button2_Click(object sender, EventArgs e) => this.Close();

    private void button3_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Url16GoalWordsTextBox = new TextBox();
      this.Url15GoalWordsTextBox = new TextBox();
      this.tabPage1 = new TabPage();
      this.Url1GoalWordsTextBox = new TextBox();
      this.Url14GoalWordsTextBox = new TextBox();
      this.tabPage2 = new TabPage();
      this.Url2GoalWordsTextBox = new TextBox();
      this.Url13GoalWordsTextBox = new TextBox();
      this.tabPage3 = new TabPage();
      this.Url3GoalWordsTextBox = new TextBox();
      this.Url12GoalWordsTextBox = new TextBox();
      this.tabPage4 = new TabPage();
      this.Url4GoalWordsTextBox = new TextBox();
      this.Url11GoalWordsTextBox = new TextBox();
      this.tabPage5 = new TabPage();
      this.Url5GoalWordsTextBox = new TextBox();
      this.Url10GoalWordsTextBox = new TextBox();
      this.tabPage6 = new TabPage();
      this.Url6GoalWordsTextBox = new TextBox();
      this.Url9GoalWordsTextBox = new TextBox();
      this.tabPage7 = new TabPage();
      this.Url7GoalWordsTextBox = new TextBox();
      this.Url8GoalWordsTextBox = new TextBox();
      this.tabPage8 = new TabPage();
      this.TabControlGoalWords = new TabControl();
      this.tabPage9 = new TabPage();
      this.tabPage10 = new TabPage();
      this.tabPage11 = new TabPage();
      this.tabPage12 = new TabPage();
      this.tabPage13 = new TabPage();
      this.tabPage14 = new TabPage();
      this.tabPage15 = new TabPage();
      this.tabPage16 = new TabPage();
      this.button4 = new Button();
      this.button3 = new Button();
      this.button2 = new Button();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.tabPage5.SuspendLayout();
      this.tabPage6.SuspendLayout();
      this.tabPage7.SuspendLayout();
      this.tabPage8.SuspendLayout();
      this.TabControlGoalWords.SuspendLayout();
      this.tabPage9.SuspendLayout();
      this.tabPage10.SuspendLayout();
      this.tabPage11.SuspendLayout();
      this.tabPage12.SuspendLayout();
      this.tabPage13.SuspendLayout();
      this.tabPage14.SuspendLayout();
      this.tabPage15.SuspendLayout();
      this.tabPage16.SuspendLayout();
      this.SuspendLayout();
      this.Url16GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url16GoalWordsTextBox.Location = new Point(6, 6);
      this.Url16GoalWordsTextBox.Multiline = true;
      this.Url16GoalWordsTextBox.Name = "Url16GoalWordsTextBox";
      this.Url16GoalWordsTextBox.Size = new Size(880, 498);
      this.Url16GoalWordsTextBox.TabIndex = 1;
      this.Url16GoalWordsTextBox.TextChanged += new EventHandler(this.Url16GoalWordsTextBox_TextChanged);
      this.Url15GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url15GoalWordsTextBox.Location = new Point(6, 6);
      this.Url15GoalWordsTextBox.Multiline = true;
      this.Url15GoalWordsTextBox.Name = "Url15GoalWordsTextBox";
      this.Url15GoalWordsTextBox.Size = new Size(880, 498);
      this.Url15GoalWordsTextBox.TabIndex = 1;
      this.Url15GoalWordsTextBox.TextChanged += new EventHandler(this.Url15GoalWordsTextBox_TextChanged);
      this.tabPage1.Controls.Add((Control) this.Url1GoalWordsTextBox);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(892, 513);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Поиск 1";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.Url1GoalWordsTextBox.Location = new Point(6, 6);
      this.Url1GoalWordsTextBox.Multiline = true;
      this.Url1GoalWordsTextBox.Name = "Url1GoalWordsTextBox";
      this.Url1GoalWordsTextBox.Size = new Size(880, 501);
      this.Url1GoalWordsTextBox.TabIndex = 0;
      this.Url1GoalWordsTextBox.TextChanged += new EventHandler(this.Url1GoalWordsTextBox_TextChanged);
      this.Url14GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url14GoalWordsTextBox.Location = new Point(6, 6);
      this.Url14GoalWordsTextBox.Multiline = true;
      this.Url14GoalWordsTextBox.Name = "Url14GoalWordsTextBox";
      this.Url14GoalWordsTextBox.Size = new Size(880, 498);
      this.Url14GoalWordsTextBox.TabIndex = 1;
      this.Url14GoalWordsTextBox.TextChanged += new EventHandler(this.Url14GoalWordsTextBox_TextChanged);
      this.tabPage2.Controls.Add((Control) this.Url2GoalWordsTextBox);
      this.tabPage2.Location = new Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(892, 513);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Поиск 2";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.Url2GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url2GoalWordsTextBox.Location = new Point(6, 6);
      this.Url2GoalWordsTextBox.Multiline = true;
      this.Url2GoalWordsTextBox.Name = "Url2GoalWordsTextBox";
      this.Url2GoalWordsTextBox.Size = new Size(880, 498);
      this.Url2GoalWordsTextBox.TabIndex = 1;
      this.Url2GoalWordsTextBox.TextChanged += new EventHandler(this.Url2GoalWordsTextBox_TextChanged);
      this.Url13GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url13GoalWordsTextBox.Location = new Point(6, 6);
      this.Url13GoalWordsTextBox.Multiline = true;
      this.Url13GoalWordsTextBox.Name = "Url13GoalWordsTextBox";
      this.Url13GoalWordsTextBox.Size = new Size(880, 498);
      this.Url13GoalWordsTextBox.TabIndex = 1;
      this.Url13GoalWordsTextBox.TextChanged += new EventHandler(this.Url13GoalWordsTextBox_TextChanged);
      this.tabPage3.Controls.Add((Control) this.Url3GoalWordsTextBox);
      this.tabPage3.Location = new Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Size = new Size(892, 513);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Поиск 3";
      this.tabPage3.UseVisualStyleBackColor = true;
      this.Url3GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url3GoalWordsTextBox.Location = new Point(6, 6);
      this.Url3GoalWordsTextBox.Multiline = true;
      this.Url3GoalWordsTextBox.Name = "Url3GoalWordsTextBox";
      this.Url3GoalWordsTextBox.Size = new Size(880, 498);
      this.Url3GoalWordsTextBox.TabIndex = 1;
      this.Url3GoalWordsTextBox.TextChanged += new EventHandler(this.Url3GoalWordsTextBox_TextChanged);
      this.Url12GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url12GoalWordsTextBox.Location = new Point(6, 6);
      this.Url12GoalWordsTextBox.Multiline = true;
      this.Url12GoalWordsTextBox.Name = "Url12GoalWordsTextBox";
      this.Url12GoalWordsTextBox.Size = new Size(880, 498);
      this.Url12GoalWordsTextBox.TabIndex = 1;
      this.Url12GoalWordsTextBox.TextChanged += new EventHandler(this.Url12GoalWordsTextBox_TextChanged);
      this.tabPage4.Controls.Add((Control) this.Url4GoalWordsTextBox);
      this.tabPage4.Location = new Point(4, 22);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Size = new Size(892, 513);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "Поиск 4";
      this.tabPage4.UseVisualStyleBackColor = true;
      this.Url4GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url4GoalWordsTextBox.Location = new Point(6, 6);
      this.Url4GoalWordsTextBox.Multiline = true;
      this.Url4GoalWordsTextBox.Name = "Url4GoalWordsTextBox";
      this.Url4GoalWordsTextBox.Size = new Size(880, 498);
      this.Url4GoalWordsTextBox.TabIndex = 1;
      this.Url4GoalWordsTextBox.TextChanged += new EventHandler(this.Url4GoalWordsTextBox_TextChanged);
      this.Url11GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url11GoalWordsTextBox.Location = new Point(6, 6);
      this.Url11GoalWordsTextBox.Multiline = true;
      this.Url11GoalWordsTextBox.Name = "Url11GoalWordsTextBox";
      this.Url11GoalWordsTextBox.Size = new Size(880, 498);
      this.Url11GoalWordsTextBox.TabIndex = 1;
      this.Url11GoalWordsTextBox.TextChanged += new EventHandler(this.Url11GoalWordsTextBox_TextChanged);
      this.tabPage5.Controls.Add((Control) this.Url5GoalWordsTextBox);
      this.tabPage5.Location = new Point(4, 22);
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.Size = new Size(892, 513);
      this.tabPage5.TabIndex = 4;
      this.tabPage5.Text = "Поиск 5";
      this.tabPage5.UseVisualStyleBackColor = true;
      this.Url5GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url5GoalWordsTextBox.Location = new Point(6, 6);
      this.Url5GoalWordsTextBox.Multiline = true;
      this.Url5GoalWordsTextBox.Name = "Url5GoalWordsTextBox";
      this.Url5GoalWordsTextBox.Size = new Size(880, 498);
      this.Url5GoalWordsTextBox.TabIndex = 1;
      this.Url5GoalWordsTextBox.TextChanged += new EventHandler(this.Url5GoalWordsTextBox_TextChanged);
      this.Url10GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url10GoalWordsTextBox.Location = new Point(6, 6);
      this.Url10GoalWordsTextBox.Multiline = true;
      this.Url10GoalWordsTextBox.Name = "Url10GoalWordsTextBox";
      this.Url10GoalWordsTextBox.Size = new Size(880, 498);
      this.Url10GoalWordsTextBox.TabIndex = 1;
      this.Url10GoalWordsTextBox.TextChanged += new EventHandler(this.Url10GoalWordsTextBox_TextChanged);
      this.tabPage6.Controls.Add((Control) this.Url6GoalWordsTextBox);
      this.tabPage6.Location = new Point(4, 22);
      this.tabPage6.Name = "tabPage6";
      this.tabPage6.Size = new Size(892, 513);
      this.tabPage6.TabIndex = 5;
      this.tabPage6.Text = "Поиск 6";
      this.tabPage6.UseVisualStyleBackColor = true;
      this.Url6GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url6GoalWordsTextBox.Location = new Point(6, 6);
      this.Url6GoalWordsTextBox.Multiline = true;
      this.Url6GoalWordsTextBox.Name = "Url6GoalWordsTextBox";
      this.Url6GoalWordsTextBox.Size = new Size(880, 498);
      this.Url6GoalWordsTextBox.TabIndex = 1;
      this.Url6GoalWordsTextBox.TextChanged += new EventHandler(this.Url6GoalWordsTextBox_TextChanged);
      this.Url9GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url9GoalWordsTextBox.Location = new Point(6, 6);
      this.Url9GoalWordsTextBox.Multiline = true;
      this.Url9GoalWordsTextBox.Name = "Url9GoalWordsTextBox";
      this.Url9GoalWordsTextBox.Size = new Size(880, 498);
      this.Url9GoalWordsTextBox.TabIndex = 1;
      this.Url9GoalWordsTextBox.TextChanged += new EventHandler(this.Url9GoalWordsTextBox_TextChanged);
      this.tabPage7.Controls.Add((Control) this.Url7GoalWordsTextBox);
      this.tabPage7.Location = new Point(4, 22);
      this.tabPage7.Name = "tabPage7";
      this.tabPage7.Size = new Size(892, 513);
      this.tabPage7.TabIndex = 6;
      this.tabPage7.Text = "Поиск 7";
      this.tabPage7.UseVisualStyleBackColor = true;
      this.Url7GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url7GoalWordsTextBox.Location = new Point(6, 6);
      this.Url7GoalWordsTextBox.Multiline = true;
      this.Url7GoalWordsTextBox.Name = "Url7GoalWordsTextBox";
      this.Url7GoalWordsTextBox.Size = new Size(880, 498);
      this.Url7GoalWordsTextBox.TabIndex = 1;
      this.Url7GoalWordsTextBox.TextChanged += new EventHandler(this.Url7GoalWordsTextBox_TextChanged);
      this.Url8GoalWordsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Url8GoalWordsTextBox.Location = new Point(6, 6);
      this.Url8GoalWordsTextBox.Multiline = true;
      this.Url8GoalWordsTextBox.Name = "Url8GoalWordsTextBox";
      this.Url8GoalWordsTextBox.Size = new Size(880, 498);
      this.Url8GoalWordsTextBox.TabIndex = 1;
      this.Url8GoalWordsTextBox.TextChanged += new EventHandler(this.Url8GoalWordsTextBox_TextChanged);
      this.tabPage8.Controls.Add((Control) this.Url8GoalWordsTextBox);
      this.tabPage8.Location = new Point(4, 22);
      this.tabPage8.Name = "tabPage8";
      this.tabPage8.Size = new Size(892, 513);
      this.tabPage8.TabIndex = 7;
      this.tabPage8.Text = "Поиск 8";
      this.tabPage8.UseVisualStyleBackColor = true;
      this.TabControlGoalWords.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage1);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage2);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage3);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage4);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage5);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage6);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage7);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage8);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage9);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage10);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage11);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage12);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage13);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage14);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage15);
      this.TabControlGoalWords.Controls.Add((Control) this.tabPage16);
      this.TabControlGoalWords.Location = new Point(1, 23);
      this.TabControlGoalWords.Name = "TabControlGoalWords";
      this.TabControlGoalWords.SelectedIndex = 0;
      this.TabControlGoalWords.Size = new Size(900, 539);
      this.TabControlGoalWords.TabIndex = 1;
      this.tabPage9.Controls.Add((Control) this.Url9GoalWordsTextBox);
      this.tabPage9.Location = new Point(4, 22);
      this.tabPage9.Name = "tabPage9";
      this.tabPage9.Size = new Size(892, 513);
      this.tabPage9.TabIndex = 8;
      this.tabPage9.Text = "Поиск 9";
      this.tabPage9.UseVisualStyleBackColor = true;
      this.tabPage10.Controls.Add((Control) this.Url10GoalWordsTextBox);
      this.tabPage10.Location = new Point(4, 22);
      this.tabPage10.Name = "tabPage10";
      this.tabPage10.Size = new Size(892, 513);
      this.tabPage10.TabIndex = 9;
      this.tabPage10.Text = "Поиск 10";
      this.tabPage10.UseVisualStyleBackColor = true;
      this.tabPage11.Controls.Add((Control) this.Url11GoalWordsTextBox);
      this.tabPage11.Location = new Point(4, 22);
      this.tabPage11.Name = "tabPage11";
      this.tabPage11.Size = new Size(892, 513);
      this.tabPage11.TabIndex = 10;
      this.tabPage11.Text = "Поиск 11";
      this.tabPage11.UseVisualStyleBackColor = true;
      this.tabPage12.Controls.Add((Control) this.Url12GoalWordsTextBox);
      this.tabPage12.Location = new Point(4, 22);
      this.tabPage12.Name = "tabPage12";
      this.tabPage12.Size = new Size(892, 513);
      this.tabPage12.TabIndex = 11;
      this.tabPage12.Text = "Поиск 12";
      this.tabPage12.UseVisualStyleBackColor = true;
      this.tabPage13.Controls.Add((Control) this.Url13GoalWordsTextBox);
      this.tabPage13.Location = new Point(4, 22);
      this.tabPage13.Name = "tabPage13";
      this.tabPage13.Size = new Size(892, 513);
      this.tabPage13.TabIndex = 12;
      this.tabPage13.Text = "Поиск 13";
      this.tabPage13.UseVisualStyleBackColor = true;
      this.tabPage14.Controls.Add((Control) this.Url14GoalWordsTextBox);
      this.tabPage14.Location = new Point(4, 22);
      this.tabPage14.Name = "tabPage14";
      this.tabPage14.Size = new Size(892, 513);
      this.tabPage14.TabIndex = 13;
      this.tabPage14.Text = "Поиск 14";
      this.tabPage14.UseVisualStyleBackColor = true;
      this.tabPage15.Controls.Add((Control) this.Url15GoalWordsTextBox);
      this.tabPage15.Location = new Point(4, 22);
      this.tabPage15.Name = "tabPage15";
      this.tabPage15.Size = new Size(892, 513);
      this.tabPage15.TabIndex = 14;
      this.tabPage15.Text = "Поиск 15";
      this.tabPage15.UseVisualStyleBackColor = true;
      this.tabPage16.Controls.Add((Control) this.Url16GoalWordsTextBox);
      this.tabPage16.Location = new Point(4, 22);
      this.tabPage16.Name = "tabPage16";
      this.tabPage16.Size = new Size(892, 513);
      this.tabPage16.TabIndex = 15;
      this.tabPage16.Text = "Поиск 16";
      this.tabPage16.UseVisualStyleBackColor = true;
      this.button4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.button4.Location = new Point(629, 1);
      this.button4.Name = "button4";
      this.button4.Size = new Size(225, 23);
      this.button4.TabIndex = 6;
      this.button4.Text = "Вставить знак разделения слов '|'";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.button3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.button3.Location = new Point(856, 1);
      this.button3.Name = "button3";
      this.button3.Size = new Size(20, 23);
      this.button3.TabIndex = 5;
      this.button3.Text = "_";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.button2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.button2.Location = new Point(877, 1);
      this.button2.Name = "button2";
      this.button2.Size = new Size(20, 23);
      this.button2.TabIndex = 4;
      this.button2.Text = "X";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(897, 561);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.TabControlGoalWords);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (GoalWords);
      this.Text = "Целевые слова";
      this.Load += new EventHandler(this.GoalWords_Load);
      this.MouseDown += new MouseEventHandler(this.GoalWords_MouseDown);
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
      this.TabControlGoalWords.ResumeLayout(false);
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
