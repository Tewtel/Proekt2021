// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.Browsers
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
  public class Browsers : Form
  {
    private IniFile INI = new IniFile("Config.ini");
    private ImageList imageList = new ImageList();
    private List<Browser> listBrowsers;
    private SetForm sf;
    private IContainer components = (IContainer) null;
    private ListView listView1;
    private ColumnHeader Изображение;
    private ColumnHeader Название;
    private ColumnHeader Расположение;
    private Button button1;
    private Button button2;
    private OpenFileDialog openFileDialog1;
    private Label label1;

    public Browsers(SetForm sf)
    {
      this.InitializeComponent();
      this.sf = sf;
    }

    private void Browsers_Load(object sender, EventArgs e)
    {
      this.listBrowsers = GetVersionBrowser.GetBrowsers();
      this.listView1.FullRowSelect = true;
      foreach (Browser listBrowser in this.listBrowsers)
      {
        this.imageList.ImageSize = new Size(30, 30);
        this.imageList.Images.Add((Image) new Bitmap((Image) Icon.ExtractAssociatedIcon(listBrowser.PathBrowser).ToBitmap()));
      }
      this.listView1.SmallImageList = this.imageList;
      for (int index = 0; index < this.listBrowsers.Count; ++index)
        this.listView1.Items.Add(new ListViewItem(new string[3]
        {
          "",
          this.listBrowsers[index].NameBrowser,
          this.listBrowsers[index].PathBrowser
        })
        {
          ImageIndex = index
        });
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.listView1.SelectedItems.Count <= 0)
        return;
      this.INI.WriteINI("SETCLASS", "Browser", this.listView1.Items[this.listView1.SelectedIndices[0]].SubItems[2].Text);
      StatSetClass.browser = this.listView1.Items[this.listView1.SelectedIndices[0]].SubItems[2].Text;
      this.sf.button1.Text = "Открывать в " + this.listView1.Items[this.listView1.SelectedIndices[0]].SubItems[2].Text;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.imageList.Images.Add((Image) new Bitmap((Image) Icon.ExtractAssociatedIcon(this.openFileDialog1.FileName).ToBitmap()));
      this.listView1.Items.Add(new ListViewItem(new string[3]
      {
        "",
        this.openFileDialog1.SafeFileName,
        this.openFileDialog1.FileName
      })
      {
        ImageIndex = this.listBrowsers.Count
      });
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listView1 = new ListView();
      this.Изображение = new ColumnHeader();
      this.Название = new ColumnHeader();
      this.Расположение = new ColumnHeader();
      this.button1 = new Button();
      this.button2 = new Button();
      this.openFileDialog1 = new OpenFileDialog();
      this.label1 = new Label();
      this.SuspendLayout();
      this.listView1.Columns.AddRange(new ColumnHeader[3]
      {
        this.Изображение,
        this.Название,
        this.Расположение
      });
      this.listView1.GridLines = true;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(12, 12);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(479, 208);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.Изображение.Text = "Изображение";
      this.Название.Text = "Название";
      this.Расположение.Text = "Расположение";
      this.button1.Location = new Point(12, 239);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "Применить";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.Location = new Point(93, 239);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "Указать свой браузер";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.openFileDialog1.FileName = "openFileDialog1";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label1.Location = new Point(12, 223);
      this.label1.Name = "label1";
      this.label1.Size = new Size(375, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Если вашего браузера нет в спике, нажмите кнопку Указать";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(503, 271);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.listView1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (Browsers);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (Browsers);
      this.Load += new EventHandler(this.Browsers_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
