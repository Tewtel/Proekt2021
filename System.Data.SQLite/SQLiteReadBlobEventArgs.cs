// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteReadBlobEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the parameters that are provided to
  /// the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" /> method, with
  /// the exception of the column index (provided separately).
  /// </summary>
  public class SQLiteReadBlobEventArgs : SQLiteReadEventArgs
  {
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadBlobEventArgs.ReadOnly" /> property.
    /// </summary>
    private bool readOnly;

    /// <summary>
    /// Constructs an instance of this class to pass into a user-defined
    /// callback associated with the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" />
    /// method.
    /// </summary>
    /// <param name="readOnly">
    /// The value that was originally specified for the "readOnly"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" /> method.
    /// </param>
    internal SQLiteReadBlobEventArgs(bool readOnly) => this.readOnly = readOnly;

    /// <summary>
    /// The value that was originally specified for the "readOnly"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" /> method.
    /// </summary>
    public bool ReadOnly
    {
      get => this.readOnly;
      set => this.readOnly = value;
    }
  }
}
