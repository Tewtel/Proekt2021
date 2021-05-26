// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteSession
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.IO;

namespace System.Data.SQLite
{
  /// <summary>
  /// This interface contains methods to query and manipulate the state of a
  /// change tracking session for a database.
  /// </summary>
  public interface ISQLiteSession : IDisposable
  {
    /// <summary>
    /// Determines if this session is currently tracking changes to its
    /// associated database.
    /// </summary>
    /// <returns>
    /// Non-zero if changes to the associated database are being trakced;
    /// otherwise, zero.
    /// </returns>
    bool IsEnabled();

    /// <summary>
    /// Enables tracking of changes to the associated database.
    /// </summary>
    void SetToEnabled();

    /// <summary>
    /// Disables tracking of changes to the associated database.
    /// </summary>
    void SetToDisabled();

    /// <summary>
    /// Determines if this session is currently set to mark changes as
    /// indirect (i.e. as though they were made via a trigger or foreign
    /// key action).
    /// </summary>
    /// <returns>
    /// Non-zero if changes to the associated database are being marked as
    /// indirect; otherwise, zero.
    /// </returns>
    bool IsIndirect();

    /// <summary>
    /// Sets the indirect flag for this session.  Subsequent changes will
    /// be marked as indirect until this flag is changed again.
    /// </summary>
    void SetToIndirect();

    /// <summary>
    /// Clears the indirect flag for this session.  Subsequent changes will
    /// be marked as direct until this flag is changed again.
    /// </summary>
    void SetToDirect();

    /// <summary>
    /// Determines if there are any tracked changes currently within the
    /// data for this session.
    /// </summary>
    /// <returns>
    /// Non-zero if there are no changes within the data for this session;
    /// otherwise, zero.
    /// </returns>
    bool IsEmpty();

    /// <summary>
    /// Upon success, causes changes to the specified table(s) to start
    /// being tracked.  Any tables impacted by calls to this method will
    /// not cause the <see cref="T:System.Data.SQLite.SessionTableFilterCallback" /> callback
    /// to be invoked.
    /// </summary>
    /// <param name="name">
    /// The name of the table to be tracked -OR- null to track all
    /// applicable tables within this database.
    /// </param>
    void AttachTable(string name);

    /// <summary>
    /// This method is used to set the table filter for this instance.
    /// </summary>
    /// <param name="callback">
    /// The table filter callback -OR- null to clear any existing table
    /// filter callback.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    void SetTableFilter(SessionTableFilterCallback callback, object clientData);

    /// <summary>
    /// Attempts to create and return, via <paramref name="rawData" />, the
    /// combined set of changes represented by this session instance.
    /// </summary>
    /// <param name="rawData">
    /// Upon success, this will contain the raw byte data for all the
    /// changes in this session instance.
    /// </param>
    void CreateChangeSet(ref byte[] rawData);

    /// <summary>
    /// Attempts to create and write, via <paramref name="stream" />, the
    /// combined set of changes represented by this session instance.
    /// </summary>
    /// <param name="stream">
    /// Upon success, the raw byte data for all the changes in this session
    /// instance will be written to this <see cref="T:System.IO.Stream" />.
    /// </param>
    void CreateChangeSet(Stream stream);

    /// <summary>
    /// Attempts to create and return, via <paramref name="rawData" />, the
    /// combined set of changes represented by this session instance as a
    /// patch set.
    /// </summary>
    /// <param name="rawData">
    /// Upon success, this will contain the raw byte data for all the
    /// changes in this session instance.
    /// </param>
    void CreatePatchSet(ref byte[] rawData);

    /// <summary>
    /// Attempts to create and write, via <paramref name="stream" />, the
    /// combined set of changes represented by this session instance as a
    /// patch set.
    /// </summary>
    /// <param name="stream">
    /// Upon success, the raw byte data for all the changes in this session
    /// instance will be written to this <see cref="T:System.IO.Stream" />.
    /// </param>
    void CreatePatchSet(Stream stream);

    /// <summary>
    /// This method loads the differences between two tables [with the same
    /// name, set of columns, and primary key definition] into this session
    /// instance.
    /// </summary>
    /// <param name="fromDatabaseName">
    /// The name of the database containing the table with the original
    /// data (i.e. it will need updating in order to be identical to the
    /// one within the database associated with this session instance).
    /// </param>
    /// <param name="tableName">The name of the table.</param>
    void LoadDifferencesFromTable(string fromDatabaseName, string tableName);
  }
}
