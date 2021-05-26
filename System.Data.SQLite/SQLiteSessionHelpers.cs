// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteSessionHelpers
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class contains some static helper methods for use within this
  /// subsystem.
  /// </summary>
  internal static class SQLiteSessionHelpers
  {
    /// <summary>
    /// This method checks the byte array specified by the caller to make
    /// sure it will be usable.
    /// </summary>
    /// <param name="rawData">
    /// A byte array provided by the caller into one of the public methods
    /// for the classes that belong to this subsystem.  This value cannot
    /// be null or represent an empty array; otherwise, an appropriate
    /// exception will be thrown.
    /// </param>
    public static void CheckRawData(byte[] rawData)
    {
      if (rawData == null)
        throw new ArgumentNullException(nameof (rawData));
      if (rawData.Length == 0)
        throw new ArgumentException("empty change set data", nameof (rawData));
    }
  }
}
