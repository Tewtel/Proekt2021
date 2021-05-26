// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SR
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System.Data.SQLite
{
  /// <summary>
  ///   A strongly-typed resource class, for looking up localized strings, etc.
  /// </summary>
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal sealed class SR
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal SR()
    {
    }

    /// <summary>
    ///   Returns the cached ResourceManager instance used by this class.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) SR.resourceMan, (object) null))
          SR.resourceMan = new ResourceManager("System.Data.SQLite.SR", typeof (SR).Assembly);
        return SR.resourceMan;
      }
    }

    /// <summary>
    ///   Overrides the current thread's CurrentUICulture property for all
    ///   resource lookups using this strongly typed resource class.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => SR.resourceCulture;
      set => SR.resourceCulture = value;
    }

    /// <summary>
    ///    Looks up a localized string similar to &lt;?xml version="1.0" standalone="yes"?&gt;
    /// &lt;DocumentElement&gt;
    ///   &lt;DataTypes&gt;
    ///     &lt;TypeName&gt;smallint&lt;/TypeName&gt;
    ///     &lt;ProviderDbType&gt;10&lt;/ProviderDbType&gt;
    ///     &lt;ColumnSize&gt;5&lt;/ColumnSize&gt;
    ///     &lt;DataType&gt;System.Int16&lt;/DataType&gt;
    ///     &lt;CreateFormat&gt;smallint&lt;/CreateFormat&gt;
    ///     &lt;IsAutoIncrementable&gt;false&lt;/IsAutoIncrementable&gt;
    ///     &lt;IsCaseSensitive&gt;false&lt;/IsCaseSensitive&gt;
    ///     &lt;IsFixedLength&gt;true&lt;/IsFixedLength&gt;
    ///     &lt;IsFixedPrecisionScale&gt;true&lt;/IsFixedPrecisionScale&gt;
    ///     &lt;IsLong&gt;false&lt;/IsLong&gt;
    ///     &lt;IsNullable&gt;true&lt;/ [rest of string was truncated]";.
    ///  </summary>
    internal static string DataTypes => SR.ResourceManager.GetString(nameof (DataTypes), SR.resourceCulture);

    /// <summary>
    ///   Looks up a localized string similar to ALL,ALTER,AND,AS,AUTOINCREMENT,BETWEEN,BY,CASE,CHECK,COLLATE,COMMIT,CONSTRAINT,CREATE,CROSS,DEFAULT,DEFERRABLE,DELETE,DISTINCT,DROP,ELSE,ESCAPE,EXCEPT,FOREIGN,FROM,FULL,GROUP,HAVING,IN,INDEX,INNER,INSERT,INTERSECT,INTO,IS,ISNULL,JOIN,LEFT,LIMIT,NATURAL,NOT,NOTNULL,NULL,ON,OR,ORDER,OUTER,PRIMARY,REFERENCES,RIGHT,ROLLBACK,SELECT,SET,TABLE,THEN,TO,TRANSACTION,UNION,UNIQUE,UPDATE,USING,VALUES,WHEN,WHERE.
    /// </summary>
    internal static string Keywords => SR.ResourceManager.GetString(nameof (Keywords), SR.resourceCulture);

    /// <summary>
    ///    Looks up a localized string similar to &lt;?xml version="1.0" encoding="utf-8" ?&gt;
    /// &lt;DocumentElement&gt;
    ///   &lt;MetaDataCollections&gt;
    ///     &lt;CollectionName&gt;MetaDataCollections&lt;/CollectionName&gt;
    ///     &lt;NumberOfRestrictions&gt;0&lt;/NumberOfRestrictions&gt;
    ///     &lt;NumberOfIdentifierParts&gt;0&lt;/NumberOfIdentifierParts&gt;
    ///   &lt;/MetaDataCollections&gt;
    ///   &lt;MetaDataCollections&gt;
    ///     &lt;CollectionName&gt;DataSourceInformation&lt;/CollectionName&gt;
    ///     &lt;NumberOfRestrictions&gt;0&lt;/NumberOfRestrictions&gt;
    ///     &lt;NumberOfIdentifierParts&gt;0&lt;/NumberOfIdentifierParts&gt;
    ///   &lt;/MetaDataCollections&gt;
    ///   &lt;MetaDataC [rest of string was truncated]";.
    ///  </summary>
    internal static string MetaDataCollections => SR.ResourceManager.GetString(nameof (MetaDataCollections), SR.resourceCulture);
  }
}
