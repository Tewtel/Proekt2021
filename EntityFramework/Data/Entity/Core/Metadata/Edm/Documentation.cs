// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.Documentation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Class representing the Documentation associated with an item
  /// </summary>
  public sealed class Documentation : MetadataItem
  {
    private string _summary = "";
    private string _longDescription = "";

    internal Documentation()
    {
    }

    /// <summary>Initializes a new Documentation instance.</summary>
    /// <param name="summary">A summary string.</param>
    /// <param name="longDescription">A long description string.</param>
    public Documentation(string summary, string longDescription)
    {
      this.Summary = summary;
      this.LongDescription = longDescription;
    }

    /// <summary>
    /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" />.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents the built-in type kind for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" />
    /// .
    /// </returns>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.Documentation;

    /// <summary>
    /// Gets the summary for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" />.
    /// </summary>
    /// <returns>
    /// The summary for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" />.
    /// </returns>
    public string Summary
    {
      get => this._summary;
      internal set
      {
        if (value != null)
          this._summary = value;
        else
          this._summary = "";
      }
    }

    /// <summary>
    /// Gets the long description for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" />.
    /// </summary>
    /// <returns>
    /// The long description for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" />.
    /// </returns>
    public string LongDescription
    {
      get => this._longDescription;
      internal set
      {
        if (value != null)
          this._longDescription = value;
        else
          this._longDescription = "";
      }
    }

    internal override string Identity => nameof (Documentation);

    /// <summary>
    /// Gets a value indicating whether this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" /> object contains only a null or an empty
    /// <see cref="P:System.Data.Entity.Core.Metadata.Edm.Documentation.Summary" />
    /// and a
    /// <see cref="P:System.Data.Entity.Core.Metadata.Edm.Documentation.Longdescription" />
    /// .
    /// </summary>
    /// <returns>
    /// true if this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" /> object contains only a null or an empty
    /// <see cref="P:System.Data.Entity.Core.Metadata.Edm.Documentation.Summary" />
    /// and a
    /// <see cref="P:System.Data.Entity.Core.Metadata.Edm.Documentation.LongDescription" />
    /// ; otherwise, false.
    /// </returns>
    public bool IsEmpty => string.IsNullOrEmpty(this._summary) && string.IsNullOrEmpty(this._longDescription);

    /// <summary>
    /// Returns the summary for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" />.
    /// </summary>
    /// <returns>
    /// The summary for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.Documentation" />.
    /// </returns>
    public override string ToString() => this._summary;
  }
}
