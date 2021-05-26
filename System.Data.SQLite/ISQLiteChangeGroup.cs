// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteChangeGroup
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.IO;

namespace System.Data.SQLite
{
  /// <summary>
  /// This interface contains methods used to manipulate multiple sets of
  /// changes for a database.
  /// </summary>
  public interface ISQLiteChangeGroup : IDisposable
  {
    /// <summary>
    /// Attempts to add a change set (or patch set) to this change group
    /// instance.  The underlying data must be contained entirely within
    /// the <paramref name="rawData" /> byte array.
    /// </summary>
    /// <param name="rawData">
    /// The raw byte data for the specified change set (or patch set).
    /// </param>
    void AddChangeSet(byte[] rawData);

    /// <summary>
    /// Attempts to add a change set (or patch set) to this change group
    /// instance.  The underlying data will be read from the specified
    /// <see cref="T:System.IO.Stream" />.
    /// </summary>
    /// <param name="stream">
    /// The <see cref="T:System.IO.Stream" /> instance containing the raw change set
    /// (or patch set) data to read.
    /// </param>
    void AddChangeSet(Stream stream);

    /// <summary>
    /// Attempts to create and return, via <paramref name="rawData" />, the
    /// combined set of changes represented by this change group instance.
    /// </summary>
    /// <param name="rawData">
    /// Upon success, this will contain the raw byte data for all the
    /// changes in this change group instance.
    /// </param>
    void CreateChangeSet(ref byte[] rawData);

    /// <summary>
    /// Attempts to create and write, via <paramref name="stream" />, the
    /// combined set of changes represented by this change group instance.
    /// </summary>
    /// <param name="stream">
    /// Upon success, the raw byte data for all the changes in this change
    /// group instance will be written to this <see cref="T:System.IO.Stream" />.
    /// </param>
    void CreateChangeSet(Stream stream);
  }
}
