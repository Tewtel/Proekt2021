// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportEntityTypeMappingConditionIsNull
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Represents a mapping condition for the result of a function import
  /// evaluated by checking null or not null.
  /// </summary>
  public sealed class FunctionImportEntityTypeMappingConditionIsNull : 
    FunctionImportEntityTypeMappingCondition
  {
    private readonly bool _isNull;

    /// <summary>
    /// Initializes a new FunctionImportEntityTypeMappingConditionIsNull instance.
    /// </summary>
    /// <param name="columnName">The name of the column used to evaluate the condition.</param>
    /// <param name="isNull">Flag that indicates whether a null or not null check is performed.</param>
    public FunctionImportEntityTypeMappingConditionIsNull(string columnName, bool isNull)
      : this(System.Data.Entity.Utilities.Check.NotNull<string>(columnName, nameof (columnName)), isNull, LineInfo.Empty)
    {
    }

    internal FunctionImportEntityTypeMappingConditionIsNull(
      string columnName,
      bool isNull,
      LineInfo lineInfo)
      : base(columnName, lineInfo)
    {
      this._isNull = isNull;
    }

    /// <summary>
    /// Gets a flag that indicates whether a null or not null check is performed.
    /// </summary>
    public bool IsNull => this._isNull;

    internal override ValueCondition ConditionValue => !this.IsNull ? ValueCondition.IsNotNull : ValueCondition.IsNull;

    internal override bool ColumnValueMatchesCondition(object columnValue) => (columnValue == null ? 1 : (Convert.IsDBNull(columnValue) ? 1 : 0)) == (this.IsNull ? 1 : 0);
  }
}
