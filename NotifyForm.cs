// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.NotifyForm
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  public class NotifyForm : Form
  {
    private List<NotifyForm> nf;
    private Timer timer = new Timer();
    private IContainer components = (IContainer) null;
    public Label NameLabel;
    public Label PriceLabel;
    public Label LinkLabel;
    private Label label1;
    private Timer timer1;

    public NotifyForm(List<NotifyForm> nf)
    {
      this.nf = nf;
      this.InitializeComponent();
    }

    private void NameLabel_Click(object sender, EventArgs e)
    {
      Process.Start(this.LinkLabel.Text);
      this.Close();
    }

    private void label1_Click(object sender, EventArgs e) => this.Close();

    private void NotifyForm_Load(object sender, EventArgs e)
    {
      Size size = Screen.PrimaryScreen.Bounds.Size;
      this.Top = size.Height - this.Height - 35;
      this.Left = size.Width - this.Width - 20;
      this.timer1.Enabled = true;
    }

    private void timer1_Tick_1(object sender, EventArgs e)
    {
      this.timer.Tick += (EventHandler) ((s, e1) =>
      {
        if ((this.Opacity -= 0.01) > 0.01)
          return;
        this.timer.Stop();
        this.Close();
      });
      this.timer.Interval = 10;
      this.timer.Start();
    }

    private void NotifyForm_FormClosed(object sender, FormClosedEventArgs e)
    {
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
      this.NameLabel = new Label();
      this.PriceLabel = new Label();
      this.LinkLabel = new Label();
      this.label1 = new Label();
      this.timer1 = new Timer(this.components);
      this.SuspendLayout();
      this.NameLabel.Cursor = Cursors.Hand;
      this.NameLabel.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.NameLabel.ForeColor = SystemColors.Control;
      this.NameLabel.Location = new Point(12, 9);
      this.NameLabel.Name = "NameLabel";
      this.NameLabel.Size = new Size(393, 43);
      this.NameLabel.TabIndex = 0;
      this.NameLabel.Text = "label1";
      this.NameLabel.Click += new EventHandler(this.NameLabel_Click);
      this.PriceLabel.Cursor = Cursors.Hand;
      this.PriceLabel.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.PriceLabel.ForeColor = SystemColors.Control;
      this.PriceLabel.Location = new Point(12, 52);
      this.PriceLabel.Name = "PriceLabel";
      this.PriceLabel.Size = new Size(413, 23);
      this.PriceLabel.TabIndex = 1;
      this.PriceLabel.Text = "label2";
      this.PriceLabel.Click += new EventHandler(this.NameLabel_Click);
      this.LinkLabel.Cursor = Cursors.Hand;
      this.LinkLabel.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.LinkLabel.ForeColor = SystemColors.Control;
      this.LinkLabel.Location = new Point(12, 75);
      this.LinkLabel.Name = "LinkLabel";
      this.LinkLabel.Size = new Size(413, 41);
      this.LinkLabel.TabIndex = 2;
      this.LinkLabel.Text = "label3";
      this.LinkLabel.Click += new EventHandler(this.NameLabel_Click);
      this.label1.AutoSize = true;
      this.label1.Cursor = Cursors.Hand;
      this.label1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label1.ForeColor = Color.Red;
      this.label1.Location = new Point(406, 1);
      this.label1.Name = "label1";
      this.label1.Size = new Size(32, 31);
      this.label1.TabIndex = 3;
      this.label1.Text = "X";
      this.label1.Click += new EventHandler(this.label1_Click);
      this.timer1.Interval = 5000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick_1);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Black;
      this.ClientSize = new Size(437, 130);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.LinkLabel);
      this.Controls.Add((Control) this.PriceLabel);
      this.Controls.Add((Control) this.NameLabel);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (NotifyForm);
      this.Opacity = 0.8;
      this.Text = nameof (NotifyForm);
      this.TopMost = true;
      this.FormClosed += new FormClosedEventHandler(this.NotifyForm_FormClosed);
      this.Load += new EventHandler(this.NotifyForm_Load);
      this.Click += new EventHandler(this.NameLabel_Click);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
