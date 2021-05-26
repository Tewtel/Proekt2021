// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.ModelContainerConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// This <see cref="T:System.Data.Entity.DbModelBuilder" /> convention uses the name of the derived
  /// <see cref="T:System.Data.Entity.DbContext" /> class as the container for the conceptual model built by
  /// Code First.
  /// </summary>
  public class ModelContainerConvention : IConceptualModelConvention<EntityContainer>, IConvention
  {
    private readonly string _containerName;

    internal ModelContainerConvention(string containerName) => this._containerName = containerName;

    /// <summary>Applies the convention to the given model.</summary>
    /// <param name="item"> The container to apply the convention to. </param>
    /// <param name="model"> The model. </param>
    public virtual void Apply(EntityContainer item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      item.Name = this._containerName;
    }
  }
}
