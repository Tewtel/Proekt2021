// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteChangeSet
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite
{
  /// <summary>
  /// This interface contains methods used to manipulate a set of changes for
  /// a database.
  /// </summary>
  public interface ISQLiteChangeSet : 
    IEnumerable<ISQLiteChangeSetMetadataItem>,
    IEnumerable,
    IDisposable
  {
    /// <summary>
    /// This method "inverts" the set of changes within this instance.
    /// Applying an inverted set of changes to a database reverses the
    /// effects of applying the uninverted changes.  Specifically:
    /// <![CDATA[<ul>]]><![CDATA[<li>]]>
    /// Each DELETE change is changed to an INSERT, and
    /// <![CDATA[</li>]]><![CDATA[<li>]]>
    /// Each INSERT change is changed to a DELETE, and
    /// <![CDATA[</li>]]><![CDATA[<li>]]>
    /// For each UPDATE change, the old.* and new.* values are exchanged.
    /// <![CDATA[</li>]]><![CDATA[</ul>]]>
    /// This method does not change the order in which changes appear
    /// within the set of changes. It merely reverses the sense of each
    /// individual change.
    /// </summary>
    /// <returns>
    /// The new <see cref="T:System.Data.SQLite.ISQLiteChangeSet" /> instance that represents
    /// the resulting set of changes -OR- null if it is not available.
    /// </returns>
    ISQLiteChangeSet Invert();

    /// <summary>
    /// This method combines the specified set of changes with the ones
    /// contained in this instance.
    /// </summary>
    /// <param name="changeSet">
    /// The changes to be combined with those in this instance.
    /// </param>
    /// <returns>
    /// The new <see cref="T:System.Data.SQLite.ISQLiteChangeSet" /> instance that represents
    /// the resulting set of changes -OR- null if it is not available.
    /// </returns>
    ISQLiteChangeSet CombineWith(ISQLiteChangeSet changeSet);

    /// <summary>
    /// Attempts to apply the set of changes in this instance to the
    /// associated database.
    /// </summary>
    /// <param name="conflictCallback">
    /// The <see cref="T:System.Data.SQLite.SessionConflictCallback" /> delegate that will need
    /// to handle any conflicting changes that may arise.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    void Apply(SessionConflictCallback conflictCallback, object clientData);

    /// <summary>
    /// Attempts to apply the set of changes in this instance to the
    /// associated database.
    /// </summary>
    /// <param name="conflictCallback">
    /// The <see cref="T:System.Data.SQLite.SessionConflictCallback" /> delegate that will need
    /// to handle any conflicting changes that may arise.
    /// </param>
    /// <param name="tableFilterCallback">
    /// The optional <see cref="T:System.Data.SQLite.SessionTableFilterCallback" /> delegate
    /// that can be used to filter the list of tables impacted by the set
    /// of changes.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    void Apply(
      SessionConflictCallback conflictCallback,
      SessionTableFilterCallback tableFilterCallback,
      object clientData);
  }
}
