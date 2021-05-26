// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.ForeignKeyAssociationMultiplicityConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to distinguish between optional and required relationships based on CLR nullability of the foreign key property.
  /// </summary>
  public class ForeignKeyAssociationMultiplicityConvention : 
    IConceptualModelConvention<AssociationType>,
    IConvention
  {
    /// <inheritdoc />
    public virtual void Apply(AssociationType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      ReferentialConstraint constraint = item.Constraint;
      if (constraint == null)
        return;
      NavigationPropertyConfiguration configuration = item.Annotations.GetConfiguration() as NavigationPropertyConfiguration;
      if (!constraint.ToProperties.All<EdmProperty>((Func<EdmProperty, bool>) (p => !p.Nullable)))
        return;
      AssociationEndMember principalEnd = item.GetOtherEnd(constraint.DependentEnd);
      NavigationProperty navigationProperty = model.ConceptualModel.EntityTypes.SelectMany<EntityType, NavigationProperty>((Func<EntityType, IEnumerable<NavigationProperty>>) (et => (IEnumerable<NavigationProperty>) et.DeclaredNavigationProperties)).SingleOrDefault<NavigationProperty>((Func<NavigationProperty, bool>) (np => np.ResultEnd == principalEnd));
      PropertyInfo clrPropertyInfo;
      if (configuration != null && navigationProperty != null && (clrPropertyInfo = navigationProperty.Annotations.GetClrPropertyInfo()) != (PropertyInfo) null)
      {
        RelationshipMultiplicity? nullable;
        if (clrPropertyInfo == configuration.NavigationProperty)
        {
          nullable = configuration.RelationshipMultiplicity;
          if (nullable.HasValue)
            return;
        }
        if (clrPropertyInfo == configuration.InverseNavigationProperty)
        {
          nullable = configuration.InverseEndKind;
          if (nullable.HasValue)
            return;
        }
      }
      principalEnd.RelationshipMultiplicity = RelationshipMultiplicity.One;
    }
  }
}
