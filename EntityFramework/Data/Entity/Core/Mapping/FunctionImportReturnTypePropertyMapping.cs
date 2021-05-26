// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportReturnTypePropertyMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Base class for mapping a property of a function import return type.
  /// </summary>
  public abstract class FunctionImportReturnTypePropertyMapping : MappingItem
  {
    internal readonly LineInfo LineInfo;

    internal FunctionImportReturnTypePropertyMapping(LineInfo lineInfo) => this.LineInfo = lineInfo;

    internal abstract string CMember { get; }

    internal abstract string SColumn { get; }
  }
}
