// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.ComplexTypeDiscoveryConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to configure a type as a complex type if it has no primary key, no mapped base type and no navigation properties.
  /// </summary>
  public class ComplexTypeDiscoveryConvention : IConceptualModelConvention<EdmModel>, IConvention
  {
    /// <inheritdoc />
    public virtual void Apply(EdmModel item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmModel>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      foreach (var data in item.EntityTypes.Where<EntityType>((Func<EntityType, bool>) (entityType => entityType.KeyProperties.Count == 0 && entityType.BaseType == null)).Select(entityType => new
      {
        entityType = entityType,
        entityTypeConfiguration = entityType.GetConfiguration() as EntityTypeConfiguration
      }).Where(_param1 => (_param1.entityTypeConfiguration == null || !_param1.entityTypeConfiguration.IsExplicitEntity && _param1.entityTypeConfiguration.IsStructuralConfigurationOnly) && !_param1.entityType.Members.Where<EdmMember>(new Func<EdmMember, bool>(Helper.IsNavigationProperty)).Any<EdmMember>()).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier0 = _param1,
        matchingAssociations = item.AssociationTypes.Where<AssociationType>((Func<AssociationType, bool>) (associationType => associationType.SourceEnd.GetEntityType() == _param1.entityType || associationType.TargetEnd.GetEntityType() == _param1.entityType)).Select(associationType => new
        {
          associationType = associationType,
          declaringEnd = associationType.SourceEnd.GetEntityType() == _param1.entityType ? associationType.SourceEnd : associationType.TargetEnd
        }).Select(_param1 => new
        {
          \u003C\u003Eh__TransparentIdentifier0 = _param1,
          declaringEntity = _param1.associationType.GetOtherEnd(_param1.declaringEnd).GetEntityType()
        }).Select(_param1 => new
        {
          \u003C\u003Eh__TransparentIdentifier1 = _param1,
          navigationProperties = _param1.declaringEntity.Members.Where<EdmMember>(new Func<EdmMember, bool>(Helper.IsNavigationProperty)).Cast<NavigationProperty>().Where<NavigationProperty>((Func<NavigationProperty, bool>) (n => n.ResultEnd.GetEntityType() == _param1.entityType))
        }).Select(_param1 => new
        {
          DeclaringEnd = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.declaringEnd,
          AssociationType = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.associationType,
          DeclaringEntityType = _param1.\u003C\u003Eh__TransparentIdentifier1.declaringEntity,
          NavigationProperties = _param1.navigationProperties.ToList<NavigationProperty>()
        })
      }).Where(_param1 => _param1.matchingAssociations.All(a => a.AssociationType.Constraint == null && a.AssociationType.GetConfiguration() == null && (!a.AssociationType.IsSelfReferencing() && a.DeclaringEnd.IsOptional()) && a.NavigationProperties.All<NavigationProperty>((Func<NavigationProperty, bool>) (n => n.GetConfiguration() == null)))).Select(_param1 => new
      {
        EntityType = _param1.\u003C\u003Eh__TransparentIdentifier0.entityType,
        MatchingAssociations = _param1.matchingAssociations.ToList()
      }).ToList())
      {
        ComplexType complexType = item.AddComplexType(data.EntityType.Name, data.EntityType.NamespaceName);
        foreach (EdmProperty declaredProperty in data.EntityType.DeclaredProperties)
          complexType.AddMember((EdmMember) declaredProperty);
        foreach (MetadataProperty annotation in data.EntityType.Annotations)
          complexType.GetMetadataProperties().Add(annotation);
        foreach (var matchingAssociation in data.MatchingAssociations)
        {
          foreach (NavigationProperty navigationProperty in matchingAssociation.NavigationProperties)
          {
            if (matchingAssociation.DeclaringEntityType.Members.Where<EdmMember>(new Func<EdmMember, bool>(Helper.IsNavigationProperty)).Contains<EdmMember>((EdmMember) navigationProperty))
            {
              matchingAssociation.DeclaringEntityType.RemoveMember((EdmMember) navigationProperty);
              EdmProperty edmProperty = matchingAssociation.DeclaringEntityType.AddComplexProperty(navigationProperty.Name, complexType);
              foreach (MetadataProperty annotation in navigationProperty.Annotations)
                edmProperty.GetMetadataProperties().Add(annotation);
            }
          }
          item.RemoveAssociationType(matchingAssociation.AssociationType);
        }
        item.RemoveEntityType(data.EntityType);
      }
    }
  }
}
