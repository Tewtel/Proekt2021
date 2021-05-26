// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.TgrammKeyFinder
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using AvtoRinger.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace AvitoAvtoRinger
{
  public class TgrammKeyFinder : Form
  {
    private BackgroundWorker bw;
    public Form1 mainForm;
    private SetForm setMain;
    private IContainer components = (IContainer) null;
    private Label label1;
    private Label label2;
    private PictureBox pictureBox1;
    private TextBox TokenTextBox;
    private Label label3;
    private Button BotStartButton;
    private PictureBox pictureBox2;
    private Label label4;
    private Button button1;

    public TgrammKeyFinder(SetForm st1)
    {
      try
      {
        this.InitializeComponent();
        this.setMain = st1;
        this.bw = new BackgroundWorker();
        this.bw.DoWork += new DoWorkEventHandler(this.bw_DoWork);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private async void bw_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      string key = e.Argument as string;
      try
      {
        TelegramBotClient Bot = new TelegramBotClient(key);
        await Bot.SetWebhookAsync("", (InputFileStream) null, 0, (IEnumerable<UpdateType>) null, new CancellationToken());
        this.BotStartButton.Invoke((Action) (() => this.BotStartButton.Text = "Бот ождиает введения /avito"));
        Bot.OnUpdate += (EventHandler<UpdateEventArgs>) (async (su, evu) =>
        {
          Update update;
          if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null)
          {
            update = (Update) null;
          }
          else
          {
            update = evu.Update;
            Telegram.Bot.Types.Message message = update.Message;
            if (message == null)
              update = (Update) null;
            else if (message.Type != MessageType.Text)
            {
              update = (Update) null;
            }
            else
            {
              if (message.Text == "/avito")
              {
                this.setMain.TgrammTokenTextBox.Invoke((Action) (() => this.setMain.TgrammTokenTextBox.Text = this.TokenTextBox.Text.Replace(" ", "")));
                this.setMain.TgrammKeyTextBox.Invoke((Action) (() => this.setMain.TgrammKeyTextBox.Text = message.Chat.Id.ToString()));
                this.setMain.TgrammChatTextBox.Invoke((Action) (() => this.setMain.TgrammChatTextBox.Text = message.MessageId.ToString()));
                Telegram.Bot.Types.Message message1 = await Bot.SendTextMessageAsync((ChatId) message.Chat.Id, "НЕ УДАЛЯЙТЕ ЭТО СООБЩЕНИЕ! ИНАЧЕ НЕ БУДЕТ ОТПРАВЛЯТЬ! Всё отлично, не забудьте сохранить настройки, и обязательно подписывайтесь на телеграмм канал программы AVTORINGER - https://t.me/avtoringer, все новости программы, обновления и т.п. ТАМ!!!", ParseMode.Default, false, false, message.MessageId, (IReplyMarkup) null, new CancellationToken());
                this.setMain.Invoke((Action) (() => this.setMain.WindowState = FormWindowState.Normal));
                this.Invoke((Action) (() => this.WindowState = FormWindowState.Minimized));
                this.Invoke((Action) (() => this.Visible = false));
              }
              update = (Update) null;
            }
          }
        });
        Bot.StartReceiving((UpdateType[]) null, new CancellationToken());
        worker = (BackgroundWorker) null;
        key = (string) null;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не могу соединится с телеграм, вомзожно вы ввели неверных токен" + Environment.NewLine + "Либо замените DNS сервера на гугловские 8.8.8.8 и 8.8.4.4" + Environment.NewLine + ex.ToString());
        this.BotStartButton.Invoke((Action) (() => this.BotStartButton.Text = "Старт - Почитай, что нужно сделать на картинке ниже"));
        this.BotStartButton.Invoke((Action) (() => this.BotStartButton.Enabled = true));
        worker = (BackgroundWorker) null;
        key = (string) null;
      }
    }

    private void BotStartButton_Click(object sender, EventArgs e)
    {
      try
      {
        string str = this.TokenTextBox.Text.Replace(" ", "");
        if (!(str != "") || this.bw.IsBusy)
          return;
        this.bw.RunWorkerAsync((object) str);
        this.BotStartButton.Enabled = false;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void TgrammKeyFinder_Load(object sender, EventArgs e)
    {
    }

    private void TgrammKeyFinder_FormClosed(object sender, FormClosedEventArgs e) => this.setMain.WindowState = FormWindowState.Normal;

    private void button1_Click(object sender, EventArgs e) => new tlgzavis().Show((IWin32Window) this);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TgrammKeyFinder));
      this.label1 = new Label();
      this.label2 = new Label();
      this.pictureBox1 = new PictureBox();
      this.TokenTextBox = new TextBox();
      this.label3 = new Label();
      this.BotStartButton = new Button();
      this.pictureBox2 = new PictureBox();
      this.label4 = new Label();
      this.button1 = new Button();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      this.SuspendLayout();
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label1.Location = new Point(8, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(550, 100);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label2.Location = new Point(15, 171);
      this.label2.Name = "label2";
      this.label2.Size = new Size(414, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Скопируй токен, который тебе дал батя бот (BotFather) в поле ниже";
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(82, 112);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(380, 50);
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      this.TokenTextBox.Location = new Point(8, 187);
      this.TokenTextBox.Name = "TokenTextBox";
      this.TokenTextBox.Size = new Size(547, 20);
      this.TokenTextBox.TabIndex = 3;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label3.Location = new Point(15, 210);
      this.label3.Name = "label3";
      this.label3.Size = new Size(530, 56);
      this.label3.TabIndex = 4;
      this.label3.Text = componentResourceManager.GetString("label3.Text");
      this.BotStartButton.Font = new Font("Roboto Medium", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.BotStartButton.ForeColor = Color.Purple;
      this.BotStartButton.Location = new Point(8, 269);
      this.BotStartButton.Name = "BotStartButton";
      this.BotStartButton.Size = new Size(547, 23);
      this.BotStartButton.TabIndex = 5;
      this.BotStartButton.Text = "Старт - Почитай, что нужно сделать на картинке ниже";
      this.BotStartButton.UseVisualStyleBackColor = true;
      this.BotStartButton.Click += new EventHandler(this.BotStartButton_Click);
      this.pictureBox2.Image = (Image) Resources.Аннотация_2020_02_16_115721;
      this.pictureBox2.Location = new Point(8, 324);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(547, 280);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pictureBox2.TabIndex = 6;
      this.pictureBox2.TabStop = false;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label4.Location = new Point(5, 296);
      this.label4.Name = "label4";
      this.label4.Size = new Size(553, 32);
      this.label4.TabIndex = 7;
      this.label4.Text = "Убедись, что установлены обе галочки, и нажми кнопку Разрешить доступ, если появится это окно";
      this.label4.TextAlign = ContentAlignment.TopCenter;
      this.button1.Cursor = Cursors.Hand;
      this.button1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.button1.ForeColor = Color.OrangeRed;
      this.button1.Location = new Point(8, 602);
      this.button1.Name = "button1";
      this.button1.Size = new Size(547, 31);
      this.button1.TabIndex = 8;
      this.button1.Text = "Если после нажатия кнопки старт программа зависает, жми сюда";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(564, 636);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.pictureBox2);
      this.Controls.Add((Control) this.BotStartButton);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.TokenTextBox);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (TgrammKeyFinder);
      this.Text = nameof (TgrammKeyFinder);
      this.FormClosed += new FormClosedEventHandler(this.TgrammKeyFinder_FormClosed);
      this.Load += new EventHandler(this.TgrammKeyFinder_Load);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
