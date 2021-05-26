// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportReturnTypeStructuralTypeColumn
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  internal sealed class FunctionImportReturnTypeStructuralTypeColumn
  {
    internal readonly StructuralType Type;
    internal readonly bool IsTypeOf;
    internal readonly string ColumnName;
    internal readonly LineInfo LineInfo;

    internal FunctionImportReturnTypeStructuralTypeColumn(
      string columnName,
      StructuralType type,
      bool isTypeOf,
      LineInfo lineInfo)
    {
      this.ColumnName = columnName;
      this.IsTypeOf = isTypeOf;
      this.Type = type;
      this.LineInfo = lineInfo;
    }
  }
}
