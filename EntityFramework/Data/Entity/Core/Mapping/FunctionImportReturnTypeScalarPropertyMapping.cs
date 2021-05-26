// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportReturnTypeScalarPropertyMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Maps a function import return type property to a table column.
  /// </summary>
  public sealed class FunctionImportReturnTypeScalarPropertyMapping : 
    FunctionImportReturnTypePropertyMapping
  {
    private readonly string _propertyName;
    private readonly string _columnName;

    /// <summary>
    /// Initializes a new FunctionImportReturnTypeScalarPropertyMapping instance.
    /// </summary>
    /// <param name="propertyName">The mapped property name.</param>
    /// <param name="columnName">The mapped column name.</param>
    public FunctionImportReturnTypeScalarPropertyMapping(string propertyName, string columnName)
      : this(System.Data.Entity.Utilities.Check.NotNull<string>(propertyName, nameof (propertyName)), System.Data.Entity.Utilities.Check.NotNull<string>(columnName, nameof (columnName)), LineInfo.Empty)
    {
    }

    internal FunctionImportReturnTypeScalarPropertyMapping(
      string propertyName,
      string columnName,
      LineInfo lineInfo)
      : base(lineInfo)
    {
      this._propertyName = propertyName;
      this._columnName = columnName;
    }

    /// <summary>Gets the mapped property name.</summary>
    public string PropertyName => this._propertyName;

    internal override string CMember => this.PropertyName;

    /// <summary>Gets the mapped column name.</summary>
    public string ColumnName => this._columnName;

    internal override string SColumn => this.ColumnName;
  }
}
