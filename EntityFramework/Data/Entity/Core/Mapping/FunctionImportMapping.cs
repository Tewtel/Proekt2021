// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Represents a mapping from a model function import to a store composable or non-composable function.
  /// </summary>
  public abstract class FunctionImportMapping : MappingItem
  {
    private readonly EdmFunction _functionImport;
    private readonly EdmFunction _targetFunction;

    internal FunctionImportMapping(EdmFunction functionImport, EdmFunction targetFunction)
    {
      this._functionImport = functionImport;
      this._targetFunction = targetFunction;
    }

    /// <summary>Gets model function (or source of the mapping)</summary>
    public EdmFunction FunctionImport => this._functionImport;

    /// <summary>Gets store function (or target of the mapping)</summary>
    public EdmFunction TargetFunction => this._targetFunction;
  }
}
