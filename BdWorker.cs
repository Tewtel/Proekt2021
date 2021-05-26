// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.BdWorker
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  public class BdWorker
  {
    public string dbFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Avtoringer\\baza.sqlite";
    private SQLiteConnection m_dbConn;
    private SQLiteCommand m_sqlCmd;
    private Form1 mainForm;

    public BdWorker(Form1 fm) => this.mainForm = fm;

    public void CreateBd()
    {
      this.m_dbConn = new SQLiteConnection();
      this.m_sqlCmd = new SQLiteCommand();
      if (!File.Exists(this.dbFileName))
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Avtoringer");
        if (!directoryInfo.Exists)
          directoryInfo.Create();
        SQLiteConnection.CreateFile(this.dbFileName);
      }
      try
      {
        this.m_dbConn = new SQLiteConnection("Data Source=" + this.dbFileName + ";Version=3;");
        this.m_dbConn.Open();
        this.m_sqlCmd.Connection = this.m_dbConn;
        this.m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (id INTEGER PRIMARY KEY AUTOINCREMENT, AdvertName TEXT, AdvertLink TEXT, AdvertPrice TEXT, AdvertDate TEXT)";
        this.m_sqlCmd.ExecuteNonQuery();
      }
      catch (SQLiteException ex)
      {
        this.mainForm.LogBox.Invoke((Action) (() => this.mainForm.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Не могу установить соединение с базой данных " + Environment.NewLine)));
      }
    }

    public DataTable BdReadAll()
    {
      DataTable dataTable = new DataTable();
      this.m_dbConn = new SQLiteConnection();
      this.m_sqlCmd = new SQLiteCommand();
      this.m_dbConn = new SQLiteConnection("Data Source=" + this.dbFileName + ";Version=3;");
      this.m_dbConn.Open();
      this.m_sqlCmd.Connection = this.m_dbConn;
      if (this.m_dbConn.State != ConnectionState.Open)
      {
        this.mainForm.LogBox.Invoke((Action) (() => this.mainForm.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Не могу установить соединение с базой данных " + Environment.NewLine)));
        return dataTable;
      }
      try
      {
        new SQLiteDataAdapter("SELECT * FROM Catalog", this.m_dbConn).Fill(dataTable);
        return dataTable;
      }
      catch (SQLiteException ex)
      {
        int num = (int) MessageBox.Show(DateTime.Now.ToShortTimeString() + ": Что-то пошло не так, при чтении из базы данных " + ex.ToString() + Environment.NewLine);
      }
      return dataTable;
    }

    public bool BdReadOneEditor(string lnk)
    {
      DataTable dataTable = new DataTable();
      this.m_dbConn = new SQLiteConnection();
      this.m_sqlCmd = new SQLiteCommand();
      this.m_dbConn = new SQLiteConnection("Data Source=" + this.dbFileName + ";Version=3;");
      this.m_dbConn.Open();
      this.m_sqlCmd.Connection = this.m_dbConn;
      if (this.m_dbConn.State != ConnectionState.Open)
      {
        this.mainForm.LogBox.Invoke((Action) (() => this.mainForm.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Не могу установить соединение с базой данных " + Environment.NewLine)));
        return false;
      }
      try
      {
        new SQLiteDataAdapter(string.Format("SELECT * FROM Catalog WHERE AdvertLink = '{0}'", (object) lnk), this.m_dbConn).Fill(dataTable);
        return dataTable.Rows.Count > 0;
      }
      catch (SQLiteException ex)
      {
        int num = (int) MessageBox.Show(DateTime.Now.ToShortTimeString() + ": Что-то пошло не так, при чтении из базы данных " + ex.ToString() + Environment.NewLine);
        return false;
      }
    }

    public void BdWrite(
      string AdvertName,
      string AdvertLink,
      string AdvertPrice,
      string AdvertDate)
    {
      this.m_dbConn = new SQLiteConnection();
      this.m_sqlCmd = new SQLiteCommand();
      this.m_dbConn = new SQLiteConnection("Data Source=" + this.dbFileName + ";Version=3;");
      this.m_dbConn.Open();
      this.m_sqlCmd.Connection = this.m_dbConn;
      if (this.m_dbConn.State != ConnectionState.Open)
      {
        this.mainForm.LogBox.Invoke((Action) (() => this.mainForm.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Не могу установить соединение с базой данных " + Environment.NewLine)));
      }
      else
      {
        try
        {
          AdvertName = AdvertName.Replace(",", " ");
          AdvertName = AdvertName.Replace("'", " ");
          AdvertName = AdvertName.Replace("\"", " ");
          AdvertName = AdvertName.Replace("$", " ");
          AdvertName = AdvertName.Replace("*", " ");
          this.m_sqlCmd.CommandText = "INSERT INTO Catalog ('AdvertName', 'AdvertLink','AdvertPrice','AdvertDate') values ('" + AdvertName.Trim().ToUpper() + "' , '" + AdvertLink + "', '" + AdvertPrice.Trim().ToUpper() + "', '" + AdvertDate.Trim() + "')";
          this.m_sqlCmd.ExecuteNonQuery();
        }
        catch (SQLiteException ex)
        {
          this.mainForm.LogBox.Invoke((Action) (() => this.mainForm.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Что-то пошло не так при записи в базу " + Environment.NewLine)));
        }
      }
    }
  }
}
