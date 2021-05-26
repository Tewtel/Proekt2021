// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.ModelNamespaceConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.ModelConfiguration.Conventions;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// This <see cref="T:System.Data.Entity.DbModelBuilder" /> convention uses the namespace of the derived
  /// <see cref="T:System.Data.Entity.DbContext" /> class as the namespace of the conceptual model built by
  /// Code First.
  /// </summary>
  public class ModelNamespaceConvention : Convention
  {
    private readonly string _modelNamespace;

    internal ModelNamespaceConvention(string modelNamespace) => this._modelNamespace = modelNamespace;

    internal override void ApplyModelConfiguration(System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      base.ApplyModelConfiguration(modelConfiguration);
      modelConfiguration.ModelNamespace = this._modelNamespace;
    }
  }
}
