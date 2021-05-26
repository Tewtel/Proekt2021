// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.tlgzavis
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  public class tlgzavis : Form
  {
    private IContainer components = (IContainer) null;
    private Label label1;
    private Button button1;
    private Button button2;
    private Label label2;
    private Label label3;
    private Label label4;
    private TextBox textBox1;
    private TextBox textBox2;

    public tlgzavis() => this.InitializeComponent();

    private void tlgzavis_Load(object sender, EventArgs e)
    {
    }

    private void button1_Click(object sender, EventArgs e) => Process.Start("https://www.comss.ru/page.php?id=754#w7");

    private void button2_Click(object sender, EventArgs e) => Process.Start("https://www.comss.ru/page.php?id=754#w8");

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (tlgzavis));
      this.label1 = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.textBox1 = new TextBox();
      this.textBox2 = new TextBox();
      this.SuspendLayout();
      this.label1.BorderStyle = BorderStyle.FixedSingle;
      this.label1.FlatStyle = FlatStyle.Flat;
      this.label1.Font = new Font("Microsoft Sans Serif", 13f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label1.ForeColor = Color.ForestGreen;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(776, 209);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.button1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 204);
      this.button1.ForeColor = Color.FromArgb(0, 0, 192);
      this.button1.Location = new Point(11, 221);
      this.button1.Name = "button1";
      this.button1.Size = new Size(352, 34);
      this.button1.TabIndex = 1;
      this.button1.Text = "Инструкция для Windows 7";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 204);
      this.button2.ForeColor = Color.FromArgb(0, 0, 192);
      this.button2.Location = new Point(448, 221);
      this.button2.Name = "button2";
      this.button2.Size = new Size(341, 34);
      this.button2.TabIndex = 2;
      this.button2.Text = "Инструкция для Windows 10,8,8.1";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label2.Location = new Point(12, 273);
      this.label2.Name = "label2";
      this.label2.Size = new Size(276, 20);
      this.label2.TabIndex = 3;
      this.label2.Text = "Либо просто перейдите по ссылке:";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label3.Location = new Point(8, 318);
      this.label3.Name = "label3";
      this.label3.Size = new Size(85, 17);
      this.label3.TabIndex = 4;
      this.label3.Text = "Windows 7";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label4.Location = new Point(9, 345);
      this.label4.Name = "label4";
      this.label4.Size = new Size(141, 17);
      this.label4.TabIndex = 5;
      this.label4.Text = "Windows 10,8, 8.1";
      this.textBox1.BorderStyle = BorderStyle.FixedSingle;
      this.textBox1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Underline, GraphicsUnit.Point, (byte) 204);
      this.textBox1.ForeColor = SystemColors.MenuHighlight;
      this.textBox1.Location = new Point(150, 318);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(638, 23);
      this.textBox1.TabIndex = 6;
      this.textBox1.Text = "https://www.comss.ru/page.php?id=754#w7";
      this.textBox2.BorderStyle = BorderStyle.FixedSingle;
      this.textBox2.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Underline, GraphicsUnit.Point, (byte) 204);
      this.textBox2.ForeColor = SystemColors.MenuHighlight;
      this.textBox2.Location = new Point(150, 345);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(638, 23);
      this.textBox2.TabIndex = 7;
      this.textBox2.Text = "https://www.comss.ru/page.php?id=754#w8";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 373);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (tlgzavis);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (tlgzavis);
      this.Load += new EventHandler(this.tlgzavis_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
