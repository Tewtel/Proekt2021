// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.PluralizingEntitySetNameConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Pluralization;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to set the entity set name to be a pluralized version of the entity type name.
  /// </summary>
  public class PluralizingEntitySetNameConvention : 
    IConceptualModelConvention<EntitySet>,
    IConvention
  {
    private static readonly IPluralizationService _pluralizationService = DbConfiguration.DependencyResolver.GetService<IPluralizationService>();

    /// <inheritdoc />
    public virtual void Apply(EntitySet item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntitySet>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      if (item.GetConfiguration() != null)
        return;
      item.Name = ((IEnumerable<INamedDataModelItem>) model.ConceptualModel.GetEntitySets().Except<EntitySet>((IEnumerable<EntitySet>) new EntitySet[1]
      {
        item
      })).UniquifyName(PluralizingEntitySetNameConvention._pluralizationService.Pluralize(item.Name));
    }
  }
}
