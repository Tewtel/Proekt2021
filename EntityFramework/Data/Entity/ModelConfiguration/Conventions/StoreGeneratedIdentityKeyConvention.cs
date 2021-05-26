// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.StoreGeneratedIdentityKeyConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to configure integer primary keys to be identity.
  /// </summary>
  public class StoreGeneratedIdentityKeyConvention : 
    IConceptualModelConvention<EntityType>,
    IConvention
  {
    private static readonly IEnumerable<PrimitiveTypeKind> _applicableTypes = (IEnumerable<PrimitiveTypeKind>) new PrimitiveTypeKind[3]
    {
      PrimitiveTypeKind.Int16,
      PrimitiveTypeKind.Int32,
      PrimitiveTypeKind.Int64
    };

    /// <inheritdoc />
    public virtual void Apply(EntityType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      if (item.BaseType != null || item.KeyProperties.Count != 1 || item.DeclaredProperties.Select(p => new
      {
        p = p,
        sgp = p.GetStoreGeneratedPattern()
      }).Where(_param1 =>
      {
        if (!_param1.sgp.HasValue)
          return false;
        StoreGeneratedPattern? sgp = _param1.sgp;
        StoreGeneratedPattern generatedPattern = StoreGeneratedPattern.Identity;
        return sgp.GetValueOrDefault() == generatedPattern & sgp.HasValue;
      }).Select(_param1 => _param1.sgp).Any<StoreGeneratedPattern?>())
        return;
      EdmProperty property = item.KeyProperties.Single<EdmProperty>();
      if (property.GetStoreGeneratedPattern().HasValue || property.PrimitiveType == null || (!StoreGeneratedIdentityKeyConvention._applicableTypes.Contains<PrimitiveTypeKind>(property.PrimitiveType.PrimitiveTypeKind) || model.ConceptualModel.AssociationTypes.Any<AssociationType>((Func<AssociationType, bool>) (a => StoreGeneratedIdentityKeyConvention.IsNonTableSplittingForeignKey(a, property)))) || StoreGeneratedIdentityKeyConvention.ParentOfTpc(item, model.ConceptualModel))
        return;
      property.SetStoreGeneratedPattern(StoreGeneratedPattern.Identity);
    }

    private static bool IsNonTableSplittingForeignKey(
      AssociationType association,
      EdmProperty property)
    {
      if (association.Constraint == null || !association.Constraint.ToProperties.Contains(property))
        return false;
      EntityTypeConfiguration configuration1 = (EntityTypeConfiguration) association.SourceEnd.GetEntityType().GetConfiguration();
      EntityTypeConfiguration configuration2 = (EntityTypeConfiguration) association.TargetEnd.GetEntityType().GetConfiguration();
      return configuration1 == null || configuration2 == null || (configuration1.GetTableName() == null || configuration2.GetTableName() == null) || !configuration1.GetTableName().Equals(configuration2.GetTableName());
    }

    private static bool ParentOfTpc(EntityType entityType, EdmModel model) => model.EntityTypes.Where<EntityType>((Func<EntityType, bool>) (et => et.GetRootType() == entityType)).Select(e => new
    {
      e = e,
      configuration = e.GetConfiguration() as EntityTypeConfiguration
    }).Where(_param1 => _param1.configuration != null && _param1.configuration.IsMappingAnyInheritedProperty(_param1.e)).Select(_param1 => _param1.e).Any<EntityType>();
  }
}
