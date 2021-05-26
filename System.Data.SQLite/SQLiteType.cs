// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteType
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Class used internally to determine the datatype of a column in a resultset
  /// </summary>
  internal sealed class SQLiteType
  {
    /// <summary>
    /// The DbType of the column, or DbType.Object if it cannot be determined
    /// </summary>
    internal DbType Type;
    /// <summary>
    /// The affinity of a column, used for expressions or when Type is DbType.Object
    /// </summary>
    internal TypeAffinity Affinity;

    /// <summary>Constructs a default instance of this type.</summary>
    public SQLiteType()
    {
    }

    /// <summary>
    /// Constructs an instance of this type with the specified field values.
    /// </summary>
    /// <param name="affinity">
    /// The type affinity to use for the new instance.
    /// </param>
    /// <param name="type">
    /// The database type to use for the new instance.
    /// </param>
    public SQLiteType(TypeAffinity affinity, DbType type)
      : this()
    {
      this.Affinity = affinity;
      this.Type = type;
    }
  }
}
