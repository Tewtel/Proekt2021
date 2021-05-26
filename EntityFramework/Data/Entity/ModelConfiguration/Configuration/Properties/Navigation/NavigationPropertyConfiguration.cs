// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation.NavigationPropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.ModelConfiguration.Edm.Services;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation
{
  internal class NavigationPropertyConfiguration : PropertyConfiguration
  {
    private readonly PropertyInfo _navigationProperty;
    private System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity? _endKind;
    private PropertyInfo _inverseNavigationProperty;
    private System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity? _inverseEndKind;
    private ConstraintConfiguration _constraint;
    private AssociationMappingConfiguration _associationMappingConfiguration;
    private ModificationStoredProceduresConfiguration _modificationStoredProceduresConfiguration;

    internal NavigationPropertyConfiguration(PropertyInfo navigationProperty) => this._navigationProperty = navigationProperty;

    private NavigationPropertyConfiguration(NavigationPropertyConfiguration source)
    {
      this._navigationProperty = source._navigationProperty;
      this._endKind = source._endKind;
      this._inverseNavigationProperty = source._inverseNavigationProperty;
      this._inverseEndKind = source._inverseEndKind;
      this._constraint = source._constraint == null ? (ConstraintConfiguration) null : source._constraint.Clone();
      this._associationMappingConfiguration = source._associationMappingConfiguration == null ? (AssociationMappingConfiguration) null : source._associationMappingConfiguration.Clone();
      this.DeleteAction = source.DeleteAction;
      this.IsNavigationPropertyDeclaringTypePrincipal = source.IsNavigationPropertyDeclaringTypePrincipal;
      this._modificationStoredProceduresConfiguration = source._modificationStoredProceduresConfiguration == null ? (ModificationStoredProceduresConfiguration) null : source._modificationStoredProceduresConfiguration.Clone();
    }

    internal virtual NavigationPropertyConfiguration Clone() => new NavigationPropertyConfiguration(this);

    public OperationAction? DeleteAction { get; set; }

    internal PropertyInfo NavigationProperty => this._navigationProperty;

    public System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity? RelationshipMultiplicity
    {
      get => this._endKind;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity>(value, nameof (value));
        this._endKind = value;
      }
    }

    internal PropertyInfo InverseNavigationProperty
    {
      get => this._inverseNavigationProperty;
      set => this._inverseNavigationProperty = !(value == this._navigationProperty) ? value : throw System.Data.Entity.Resources.Error.NavigationInverseItself((object) value.Name, (object) value.ReflectedType);
    }

    internal System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity? InverseEndKind
    {
      get => this._inverseEndKind;
      set => this._inverseEndKind = value;
    }

    public ConstraintConfiguration Constraint
    {
      get => this._constraint;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<ConstraintConfiguration>(value, nameof (value));
        this._constraint = value;
      }
    }

    internal bool? IsNavigationPropertyDeclaringTypePrincipal { get; set; }

    internal AssociationMappingConfiguration AssociationMappingConfiguration
    {
      get => this._associationMappingConfiguration;
      set => this._associationMappingConfiguration = value;
    }

    internal ModificationStoredProceduresConfiguration ModificationStoredProceduresConfiguration
    {
      get => this._modificationStoredProceduresConfiguration;
      set => this._modificationStoredProceduresConfiguration = value;
    }

    internal void Configure(
      System.Data.Entity.Core.Metadata.Edm.NavigationProperty navigationProperty,
      EdmModel model,
      EntityTypeConfiguration entityTypeConfiguration)
    {
      navigationProperty.SetConfiguration((object) this);
      AssociationType association = navigationProperty.Association;
      if (!(association.GetConfiguration() is NavigationPropertyConfiguration configuration))
        association.SetConfiguration((object) this);
      else
        this.EnsureConsistency(configuration);
      this.ConfigureInverse(association, model);
      this.ConfigureEndKinds(association, configuration);
      this.ConfigureDependentBehavior(association, model, entityTypeConfiguration);
    }

    internal void Configure(
      AssociationSetMapping associationSetMapping,
      DbDatabaseMapping databaseMapping,
      DbProviderManifest providerManifest)
    {
      if (this.AssociationMappingConfiguration != null)
      {
        associationSetMapping.SetConfiguration((object) this);
        this.AssociationMappingConfiguration.Configure(associationSetMapping, databaseMapping.Database, this._navigationProperty);
      }
      if (this._modificationStoredProceduresConfiguration == null)
        return;
      if (associationSetMapping.ModificationFunctionMapping == null)
        new ModificationFunctionMappingGenerator(providerManifest).Generate(associationSetMapping, databaseMapping);
      this._modificationStoredProceduresConfiguration.Configure(associationSetMapping.ModificationFunctionMapping, providerManifest);
    }

    private void ConfigureInverse(AssociationType associationType, EdmModel model)
    {
      if (this._inverseNavigationProperty == (PropertyInfo) null)
        return;
      System.Data.Entity.Core.Metadata.Edm.NavigationProperty navigationProperty = model.GetNavigationProperty(this._inverseNavigationProperty);
      if (navigationProperty == null || navigationProperty.Association == associationType)
        return;
      associationType.SourceEnd.RelationshipMultiplicity = navigationProperty.Association.TargetEnd.RelationshipMultiplicity;
      if (associationType.Constraint == null && this._constraint == null && navigationProperty.Association.Constraint != null)
      {
        associationType.Constraint = navigationProperty.Association.Constraint;
        associationType.Constraint.FromRole = (RelationshipEndMember) associationType.SourceEnd;
        associationType.Constraint.ToRole = (RelationshipEndMember) associationType.TargetEnd;
      }
      model.RemoveAssociationType(navigationProperty.Association);
      navigationProperty.RelationshipType = (RelationshipType) associationType;
      navigationProperty.FromEndMember = (RelationshipEndMember) associationType.TargetEnd;
      navigationProperty.ToEndMember = (RelationshipEndMember) associationType.SourceEnd;
    }

    private void ConfigureEndKinds(
      AssociationType associationType,
      NavigationPropertyConfiguration configuration)
    {
      AssociationEndMember associationEndMember1 = associationType.SourceEnd;
      AssociationEndMember associationEndMember2 = associationType.TargetEnd;
      if (configuration != null && configuration.InverseNavigationProperty != (PropertyInfo) null)
      {
        associationEndMember1 = associationType.TargetEnd;
        associationEndMember2 = associationType.SourceEnd;
      }
      if (this._inverseEndKind.HasValue)
        associationEndMember1.RelationshipMultiplicity = this._inverseEndKind.Value;
      if (!this._endKind.HasValue)
        return;
      associationEndMember2.RelationshipMultiplicity = this._endKind.Value;
    }

    private void EnsureConsistency(
      NavigationPropertyConfiguration navigationPropertyConfiguration)
    {
      System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity? inverseEndKind;
      System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity? nullable1;
      if (this.RelationshipMultiplicity.HasValue)
      {
        if (!navigationPropertyConfiguration.InverseEndKind.HasValue)
        {
          navigationPropertyConfiguration.InverseEndKind = this.RelationshipMultiplicity;
        }
        else
        {
          inverseEndKind = navigationPropertyConfiguration.InverseEndKind;
          nullable1 = this.RelationshipMultiplicity;
          if (!(inverseEndKind.GetValueOrDefault() == nullable1.GetValueOrDefault() & inverseEndKind.HasValue == nullable1.HasValue))
            throw System.Data.Entity.Resources.Error.ConflictingMultiplicities((object) this.NavigationProperty.Name, (object) this.NavigationProperty.ReflectedType);
        }
      }
      nullable1 = this.InverseEndKind;
      if (nullable1.HasValue)
      {
        nullable1 = navigationPropertyConfiguration.RelationshipMultiplicity;
        if (!nullable1.HasValue)
        {
          navigationPropertyConfiguration.RelationshipMultiplicity = this.InverseEndKind;
        }
        else
        {
          nullable1 = navigationPropertyConfiguration.RelationshipMultiplicity;
          inverseEndKind = this.InverseEndKind;
          if (!(nullable1.GetValueOrDefault() == inverseEndKind.GetValueOrDefault() & nullable1.HasValue == inverseEndKind.HasValue))
          {
            if (this.InverseNavigationProperty == (PropertyInfo) null)
              throw System.Data.Entity.Resources.Error.ConflictingMultiplicities((object) this.NavigationProperty.Name, (object) this.NavigationProperty.ReflectedType);
            throw System.Data.Entity.Resources.Error.ConflictingMultiplicities((object) this.InverseNavigationProperty.Name, (object) this.InverseNavigationProperty.ReflectedType);
          }
        }
      }
      if (this.DeleteAction.HasValue)
      {
        if (!navigationPropertyConfiguration.DeleteAction.HasValue)
        {
          navigationPropertyConfiguration.DeleteAction = this.DeleteAction;
        }
        else
        {
          OperationAction? deleteAction1 = navigationPropertyConfiguration.DeleteAction;
          OperationAction? deleteAction2 = this.DeleteAction;
          if (!(deleteAction1.GetValueOrDefault() == deleteAction2.GetValueOrDefault() & deleteAction1.HasValue == deleteAction2.HasValue))
            throw System.Data.Entity.Resources.Error.ConflictingCascadeDeleteOperation((object) this.NavigationProperty.Name, (object) this.NavigationProperty.ReflectedType);
        }
      }
      if (this.Constraint != null)
      {
        if (navigationPropertyConfiguration.Constraint == null)
          navigationPropertyConfiguration.Constraint = this.Constraint;
        else if (!object.Equals((object) navigationPropertyConfiguration.Constraint, (object) this.Constraint))
          throw System.Data.Entity.Resources.Error.ConflictingConstraint((object) this.NavigationProperty.Name, (object) this.NavigationProperty.ReflectedType);
      }
      if (this.IsNavigationPropertyDeclaringTypePrincipal.HasValue)
      {
        if (!navigationPropertyConfiguration.IsNavigationPropertyDeclaringTypePrincipal.HasValue)
        {
          NavigationPropertyConfiguration propertyConfiguration = navigationPropertyConfiguration;
          bool? declaringTypePrincipal = this.IsNavigationPropertyDeclaringTypePrincipal;
          bool? nullable2 = declaringTypePrincipal.HasValue ? new bool?(!declaringTypePrincipal.GetValueOrDefault()) : new bool?();
          propertyConfiguration.IsNavigationPropertyDeclaringTypePrincipal = nullable2;
        }
        else
        {
          bool? declaringTypePrincipal1 = navigationPropertyConfiguration.IsNavigationPropertyDeclaringTypePrincipal;
          bool? declaringTypePrincipal2 = this.IsNavigationPropertyDeclaringTypePrincipal;
          if (declaringTypePrincipal1.GetValueOrDefault() == declaringTypePrincipal2.GetValueOrDefault() & declaringTypePrincipal1.HasValue == declaringTypePrincipal2.HasValue)
            throw System.Data.Entity.Resources.Error.ConflictingConstraint((object) this.NavigationProperty.Name, (object) this.NavigationProperty.ReflectedType);
        }
      }
      if (this.AssociationMappingConfiguration != null)
      {
        if (navigationPropertyConfiguration.AssociationMappingConfiguration == null)
          navigationPropertyConfiguration.AssociationMappingConfiguration = this.AssociationMappingConfiguration;
        else if (!object.Equals((object) navigationPropertyConfiguration.AssociationMappingConfiguration, (object) this.AssociationMappingConfiguration))
          throw System.Data.Entity.Resources.Error.ConflictingMapping((object) this.NavigationProperty.Name, (object) this.NavigationProperty.ReflectedType);
      }
      if (this.ModificationStoredProceduresConfiguration == null)
        return;
      if (navigationPropertyConfiguration.ModificationStoredProceduresConfiguration == null)
        navigationPropertyConfiguration.ModificationStoredProceduresConfiguration = this.ModificationStoredProceduresConfiguration;
      else if (!navigationPropertyConfiguration.ModificationStoredProceduresConfiguration.IsCompatibleWith(this.ModificationStoredProceduresConfiguration))
        throw System.Data.Entity.Resources.Error.ConflictingFunctionsMapping((object) this.NavigationProperty.Name, (object) this.NavigationProperty.ReflectedType);
    }

    private void ConfigureDependentBehavior(
      AssociationType associationType,
      EdmModel model,
      EntityTypeConfiguration entityTypeConfiguration)
    {
      AssociationEndMember principalEnd;
      AssociationEndMember dependentEnd;
      if (!associationType.TryGuessPrincipalAndDependentEnds(out principalEnd, out dependentEnd))
      {
        if (this.IsNavigationPropertyDeclaringTypePrincipal.HasValue)
        {
          associationType.MarkPrincipalConfigured();
          System.Data.Entity.Core.Metadata.Edm.NavigationProperty navigationProperty = model.EntityTypes.SelectMany<EntityType, System.Data.Entity.Core.Metadata.Edm.NavigationProperty>((Func<EntityType, IEnumerable<System.Data.Entity.Core.Metadata.Edm.NavigationProperty>>) (et => (IEnumerable<System.Data.Entity.Core.Metadata.Edm.NavigationProperty>) et.DeclaredNavigationProperties)).Single<System.Data.Entity.Core.Metadata.Edm.NavigationProperty>((Func<System.Data.Entity.Core.Metadata.Edm.NavigationProperty, bool>) (np => np.RelationshipType.Equals((object) associationType) && np.GetClrPropertyInfo().IsSameAs(this.NavigationProperty)));
          principalEnd = this.IsNavigationPropertyDeclaringTypePrincipal.Value ? associationType.GetOtherEnd(navigationProperty.ResultEnd) : navigationProperty.ResultEnd;
          dependentEnd = associationType.GetOtherEnd(principalEnd);
          if (associationType.SourceEnd != principalEnd)
          {
            associationType.SourceEnd = principalEnd;
            associationType.TargetEnd = dependentEnd;
            AssociationSet associationSet = model.Containers.SelectMany<EntityContainer, AssociationSet>((Func<EntityContainer, IEnumerable<AssociationSet>>) (ct => (IEnumerable<AssociationSet>) ct.AssociationSets)).Single<AssociationSet>((Func<AssociationSet, bool>) (aset => aset.ElementType == associationType));
            EntitySet sourceSet = associationSet.SourceSet;
            associationSet.SourceSet = associationSet.TargetSet;
            associationSet.TargetSet = sourceSet;
          }
        }
        if (principalEnd == null)
          dependentEnd = associationType.TargetEnd;
      }
      this.ConfigureConstraint(associationType, dependentEnd, entityTypeConfiguration);
      this.ConfigureDeleteAction(associationType.GetOtherEnd(dependentEnd));
    }

    private void ConfigureConstraint(
      AssociationType associationType,
      AssociationEndMember dependentEnd,
      EntityTypeConfiguration entityTypeConfiguration)
    {
      if (this._constraint == null)
        return;
      this._constraint.Configure(associationType, dependentEnd, entityTypeConfiguration);
      ReferentialConstraint constraint = associationType.Constraint;
      if (constraint == null || !constraint.ToProperties.SequenceEqual<EdmProperty>((IEnumerable<EdmProperty>) constraint.ToRole.GetEntityType().KeyProperties) || (this._inverseEndKind.HasValue || !associationType.SourceEnd.IsMany()))
        return;
      associationType.SourceEnd.RelationshipMultiplicity = System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne;
      associationType.TargetEnd.RelationshipMultiplicity = System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity.One;
    }

    private void ConfigureDeleteAction(AssociationEndMember principalEnd)
    {
      if (!this.DeleteAction.HasValue)
        return;
      principalEnd.DeleteBehavior = this.DeleteAction.Value;
    }

    internal void Reset()
    {
      this._endKind = new System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity?();
      this._inverseNavigationProperty = (PropertyInfo) null;
      this._inverseEndKind = new System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity?();
      this._constraint = (ConstraintConfiguration) null;
      this._associationMappingConfiguration = (AssociationMappingConfiguration) null;
    }
  }
}
