// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportComplexTypeMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Represents a complex type mapping for a function import result.
  /// </summary>
  public sealed class FunctionImportComplexTypeMapping : FunctionImportStructuralTypeMapping
  {
    private readonly ComplexType _returnType;

    /// <summary>
    /// Initializes a new FunctionImportComplexTypeMapping instance.
    /// </summary>
    /// <param name="returnType">The return type.</param>
    /// <param name="properties">The property mappings for the result type of a function import.</param>
    public FunctionImportComplexTypeMapping(
      ComplexType returnType,
      Collection<FunctionImportReturnTypePropertyMapping> properties)
      : this(System.Data.Entity.Utilities.Check.NotNull<ComplexType>(returnType, nameof (returnType)), System.Data.Entity.Utilities.Check.NotNull<Collection<FunctionImportReturnTypePropertyMapping>>(properties, nameof (properties)), LineInfo.Empty)
    {
    }

    internal FunctionImportComplexTypeMapping(
      ComplexType returnType,
      Collection<FunctionImportReturnTypePropertyMapping> properties,
      LineInfo lineInfo)
      : base(properties, lineInfo)
    {
      this._returnType = returnType;
    }

    /// <summary>Ges the return type.</summary>
    public ComplexType ReturnType => this._returnType;
  }
}
