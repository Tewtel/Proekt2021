// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.AssociationInverseDiscoveryConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to detect navigation properties to be inverses of each other when only one pair
  /// of navigation properties exists between the related types.
  /// </summary>
  public class AssociationInverseDiscoveryConvention : 
    IConceptualModelConvention<EdmModel>,
    IConvention
  {
    /// <inheritdoc />
    public virtual void Apply(EdmModel item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmModel>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      foreach (var data in item.AssociationTypes.SelectMany((Func<AssociationType, IEnumerable<AssociationType>>) (a1 => item.AssociationTypes), (a1, a2) => new
      {
        a1 = a1,
        a2 = a2
      }).Where(_param1 => _param1.a1 != _param1.a2).Where(_param1 => _param1.a1.SourceEnd.GetEntityType() == _param1.a2.TargetEnd.GetEntityType() && _param1.a1.TargetEnd.GetEntityType() == _param1.a2.SourceEnd.GetEntityType()).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier0 = _param1,
        a1Configuration = _param1.a1.GetConfiguration() as NavigationPropertyConfiguration
      }).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier1 = _param1,
        a2Configuration = _param1.\u003C\u003Eh__TransparentIdentifier0.a2.GetConfiguration() as NavigationPropertyConfiguration
      }).Where(_param1 =>
      {
        RelationshipMultiplicity? inverseEndKind;
        if (_param1.\u003C\u003Eh__TransparentIdentifier1.a1Configuration != null)
        {
          inverseEndKind = _param1.\u003C\u003Eh__TransparentIdentifier1.a1Configuration.InverseEndKind;
          if (inverseEndKind.HasValue || !(_param1.\u003C\u003Eh__TransparentIdentifier1.a1Configuration.InverseNavigationProperty == (PropertyInfo) null))
            return false;
        }
        if (_param1.a2Configuration == null)
          return true;
        inverseEndKind = _param1.a2Configuration.InverseEndKind;
        return !inverseEndKind.HasValue && _param1.a2Configuration.InverseNavigationProperty == (PropertyInfo) null;
      }).Select(_param1 => new
      {
        a1 = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.a1,
        a2 = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.a2
      }).Distinct((a, b) => a.a1 == b.a2 && a.a2 == b.a1).GroupBy((a, b) => a.a1.SourceEnd.GetEntityType() == b.a2.TargetEnd.GetEntityType() && a.a1.TargetEnd.GetEntityType() == b.a2.SourceEnd.GetEntityType()).Where<IGrouping<\u003C\u003Ef__AnonymousType62<AssociationType, AssociationType>, \u003C\u003Ef__AnonymousType62<AssociationType, AssociationType>>>(g => g.Count() == 1).Select(g => g.Single()))
      {
        AssociationType unifiedAssociation = data.a2.GetConfiguration() != null ? data.a2 : data.a1;
        AssociationType associationType = unifiedAssociation == data.a1 ? data.a2 : data.a1;
        unifiedAssociation.SourceEnd.RelationshipMultiplicity = associationType.TargetEnd.RelationshipMultiplicity;
        if (associationType.Constraint != null)
        {
          unifiedAssociation.Constraint = associationType.Constraint;
          unifiedAssociation.Constraint.FromRole = (RelationshipEndMember) unifiedAssociation.SourceEnd;
          unifiedAssociation.Constraint.ToRole = (RelationshipEndMember) unifiedAssociation.TargetEnd;
        }
        PropertyInfo clrPropertyInfo = associationType.SourceEnd.GetClrPropertyInfo();
        if (clrPropertyInfo != (PropertyInfo) null)
          unifiedAssociation.TargetEnd.SetClrPropertyInfo(clrPropertyInfo);
        AssociationInverseDiscoveryConvention.FixNavigationProperties(item, unifiedAssociation, associationType);
        item.RemoveAssociationType(associationType);
      }
    }

    private static void FixNavigationProperties(
      EdmModel model,
      AssociationType unifiedAssociation,
      AssociationType redundantAssociation)
    {
      foreach (NavigationProperty navigationProperty in model.EntityTypes.SelectMany<EntityType, NavigationProperty>((Func<EntityType, IEnumerable<NavigationProperty>>) (e => (IEnumerable<NavigationProperty>) e.NavigationProperties)).Where<NavigationProperty>((Func<NavigationProperty, bool>) (np => np.Association == redundantAssociation)))
      {
        navigationProperty.RelationshipType = (RelationshipType) unifiedAssociation;
        navigationProperty.FromEndMember = (RelationshipEndMember) unifiedAssociation.TargetEnd;
        navigationProperty.ToEndMember = (RelationshipEndMember) unifiedAssociation.SourceEnd;
      }
    }
  }
}
