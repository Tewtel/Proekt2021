// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.ForeignKeyDiscoveryConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Base class for conventions that discover foreign key properties.
  /// </summary>
  public abstract class ForeignKeyDiscoveryConvention : 
    IConceptualModelConvention<AssociationType>,
    IConvention
  {
    /// <summary>
    /// Returns <c>true</c> if the convention supports pairs of entity types that have multiple associations defined between them.
    /// </summary>
    protected virtual bool SupportsMultipleAssociations => false;

    /// <summary>
    /// When overridden returns <c>true</c> if <paramref name="dependentProperty" /> should be part of the foreign key.
    /// </summary>
    /// <param name="associationType"> The association type being configured. </param>
    /// <param name="dependentAssociationEnd"> The dependent end. </param>
    /// <param name="dependentProperty"> The candidate property on the dependent end. </param>
    /// <param name="principalEntityType"> The principal end entity type. </param>
    /// <param name="principalKeyProperty"> A key property on the principal end that is a candidate target for the foreign key. </param>
    /// <returns>true if dependentProperty should be a part of the foreign key; otherwise, false.</returns>
    protected abstract bool MatchDependentKeyProperty(
      AssociationType associationType,
      AssociationEndMember dependentAssociationEnd,
      EdmProperty dependentProperty,
      EntityType principalEntityType,
      EdmProperty principalKeyProperty);

    /// <inheritdoc />
    public virtual void Apply(AssociationType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      AssociationEndMember dependentEnd;
      AssociationEndMember principalEnd;
      if (item.Constraint != null || item.IsIndependent() || item.IsOneToOne() && item.IsSelfReferencing() || !item.TryGuessPrincipalAndDependentEnds(out principalEnd, out dependentEnd))
        return;
      IEnumerable<EdmProperty> source1 = principalEnd.GetEntityType().KeyProperties();
      if (!source1.Any<EdmProperty>() || !this.SupportsMultipleAssociations && model.ConceptualModel.GetAssociationTypesBetween(principalEnd.GetEntityType(), dependentEnd.GetEntityType()).Count<AssociationType>() > 1)
        return;
      IEnumerable<EdmProperty> source2 = source1.SelectMany((Func<EdmProperty, IEnumerable<EdmProperty>>) (p => (IEnumerable<EdmProperty>) dependentEnd.GetEntityType().DeclaredProperties), (p, d) => new
      {
        p = p,
        d = d
      }).Where(_param1 => this.MatchDependentKeyProperty(item, dependentEnd, _param1.d, principalEnd.GetEntityType(), _param1.p) && _param1.p.UnderlyingPrimitiveType == _param1.d.UnderlyingPrimitiveType).Select(_param1 => _param1.d);
      if (!source2.Any<EdmProperty>() || source2.Count<EdmProperty>() != source1.Count<EdmProperty>())
        return;
      IEnumerable<EdmProperty> source3 = dependentEnd.GetEntityType().KeyProperties();
      bool flag = source3.Count<EdmProperty>() == source2.Count<EdmProperty>() && source3.All<EdmProperty>(new Func<EdmProperty, bool>(((Enumerable) source2).Contains<EdmProperty>));
      if (((dependentEnd.IsMany() ? 1 : (item.IsSelfReferencing() ? 1 : 0)) & (flag ? 1 : 0)) != 0 || !dependentEnd.IsMany() && !flag)
        return;
      ReferentialConstraint referentialConstraint = new ReferentialConstraint((RelationshipEndMember) principalEnd, (RelationshipEndMember) dependentEnd, (IEnumerable<EdmProperty>) source1.ToList<EdmProperty>(), (IEnumerable<EdmProperty>) source2.ToList<EdmProperty>());
      item.Constraint = referentialConstraint;
      if (!principalEnd.IsRequired())
        return;
      referentialConstraint.ToProperties.Each<EdmProperty, bool>((Func<EdmProperty, bool>) (p => p.Nullable = false));
    }
  }
}
