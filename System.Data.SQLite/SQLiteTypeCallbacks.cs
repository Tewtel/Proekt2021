// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTypeCallbacks
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the custom data type handling callbacks
  /// for a single type name.
  /// </summary>
  public sealed class SQLiteTypeCallbacks
  {
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteTypeCallbacks.TypeName" /> property.
    /// </summary>
    private string typeName;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteTypeCallbacks.BindValueCallback" /> property.
    /// </summary>
    private SQLiteBindValueCallback bindValueCallback;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteTypeCallbacks.ReadValueCallback" /> property.
    /// </summary>
    private SQLiteReadValueCallback readValueCallback;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteTypeCallbacks.BindValueUserData" /> property.
    /// </summary>
    private object bindValueUserData;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteTypeCallbacks.ReadValueUserData" /> property.
    /// </summary>
    private object readValueUserData;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="bindValueCallback">
    /// The custom paramater binding callback.  This parameter may be null.
    /// </param>
    /// <param name="readValueCallback">
    /// The custom data reader value callback.  This parameter may be null.
    /// </param>
    /// <param name="bindValueUserData">
    /// The extra data to pass into the parameter binding callback.  This
    /// parameter may be null.
    /// </param>
    /// <param name="readValueUserData">
    /// The extra data to pass into the data reader value callback.  This
    /// parameter may be null.
    /// </param>
    private SQLiteTypeCallbacks(
      SQLiteBindValueCallback bindValueCallback,
      SQLiteReadValueCallback readValueCallback,
      object bindValueUserData,
      object readValueUserData)
    {
      this.bindValueCallback = bindValueCallback;
      this.readValueCallback = readValueCallback;
      this.bindValueUserData = bindValueUserData;
      this.readValueUserData = readValueUserData;
    }

    /// <summary>
    /// Creates an instance of the <see cref="T:System.Data.SQLite.SQLiteTypeCallbacks" /> class.
    /// </summary>
    /// <param name="bindValueCallback">
    /// The custom paramater binding callback.  This parameter may be null.
    /// </param>
    /// <param name="readValueCallback">
    /// The custom data reader value callback.  This parameter may be null.
    /// </param>
    /// <param name="bindValueUserData">
    /// The extra data to pass into the parameter binding callback.  This
    /// parameter may be null.
    /// </param>
    /// <param name="readValueUserData">
    /// The extra data to pass into the data reader value callback.  This
    /// parameter may be null.
    /// </param>
    public static SQLiteTypeCallbacks Create(
      SQLiteBindValueCallback bindValueCallback,
      SQLiteReadValueCallback readValueCallback,
      object bindValueUserData,
      object readValueUserData)
    {
      return new SQLiteTypeCallbacks(bindValueCallback, readValueCallback, bindValueUserData, readValueUserData);
    }

    /// <summary>
    /// The database type name that the callbacks contained in this class
    /// will apply to.  This value may not be null.
    /// </summary>
    public string TypeName
    {
      get => this.typeName;
      internal set => this.typeName = value;
    }

    /// <summary>
    /// The custom paramater binding callback.  This value may be null.
    /// </summary>
    public SQLiteBindValueCallback BindValueCallback => this.bindValueCallback;

    /// <summary>
    /// The custom data reader value callback.  This value may be null.
    /// </summary>
    public SQLiteReadValueCallback ReadValueCallback => this.readValueCallback;

    /// <summary>
    /// The extra data to pass into the parameter binding callback.  This
    /// value may be null.
    /// </summary>
    public object BindValueUserData => this.bindValueUserData;

    /// <summary>
    /// The extra data to pass into the data reader value callback.  This
    /// value may be null.
    /// </summary>
    public object ReadValueUserData => this.readValueUserData;
  }
}
