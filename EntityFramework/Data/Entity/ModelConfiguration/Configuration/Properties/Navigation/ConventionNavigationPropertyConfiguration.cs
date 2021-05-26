// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation.ConventionNavigationPropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation
{
  internal class ConventionNavigationPropertyConfiguration
  {
    private readonly NavigationPropertyConfiguration _configuration;
    private readonly System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration _modelConfiguration;

    internal ConventionNavigationPropertyConfiguration(
      NavigationPropertyConfiguration configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      this._configuration = configuration;
      this._modelConfiguration = modelConfiguration;
    }

    public virtual PropertyInfo ClrPropertyInfo => this._configuration == null ? (PropertyInfo) null : this._configuration.NavigationProperty;

    internal NavigationPropertyConfiguration Configuration => this._configuration;

    public virtual void HasConstraint<T>() where T : ConstraintConfiguration => this.HasConstraintInternal<T>((Action<T>) null);

    public virtual void HasConstraint<T>(Action<T> constraintConfigurationAction) where T : ConstraintConfiguration
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<T>>(constraintConfigurationAction, nameof (constraintConfigurationAction));
      this.HasConstraintInternal<T>(constraintConfigurationAction);
    }

    private void HasConstraintInternal<T>(Action<T> constraintConfigurationAction) where T : ConstraintConfiguration
    {
      if (this._configuration == null || this.HasConfiguredConstraint())
        return;
      Type type = typeof (T);
      if (this._configuration.Constraint == null)
        this._configuration.Constraint = !(type == typeof (IndependentConstraintConfiguration)) ? (ConstraintConfiguration) Activator.CreateInstance(type) : IndependentConstraintConfiguration.Instance;
      else if (this._configuration.Constraint.GetType() != type)
        return;
      if (constraintConfigurationAction == null)
        return;
      constraintConfigurationAction((T) this._configuration.Constraint);
    }

    private bool HasConfiguredConstraint()
    {
      if (this._configuration != null && this._configuration.Constraint != null && this._configuration.Constraint.IsFullySpecified)
        return true;
      if (this._configuration != null && this._configuration.InverseNavigationProperty != (PropertyInfo) null)
      {
        Type targetType = this._configuration.NavigationProperty.PropertyType.GetTargetType();
        if (this._modelConfiguration.Entities.Contains<Type>(targetType))
        {
          EntityTypeConfiguration typeConfiguration = this._modelConfiguration.Entity(targetType);
          if (typeConfiguration.IsNavigationPropertyConfigured(this._configuration.InverseNavigationProperty))
            return typeConfiguration.Navigation(this._configuration.InverseNavigationProperty).Constraint != null;
        }
      }
      return false;
    }

    public virtual ConventionNavigationPropertyConfiguration HasInverseNavigationProperty(
      Func<PropertyInfo, PropertyInfo> inverseNavigationPropertyGetter)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<PropertyInfo, PropertyInfo>>(inverseNavigationPropertyGetter, nameof (inverseNavigationPropertyGetter));
      if (this._configuration != null && this._configuration.InverseNavigationProperty == (PropertyInfo) null)
      {
        PropertyInfo propertyInfo = inverseNavigationPropertyGetter(this.ClrPropertyInfo);
        System.Data.Entity.Utilities.Check.NotNull<PropertyInfo>(propertyInfo, "inverseNavigationProperty");
        if (!propertyInfo.IsValidEdmNavigationProperty())
          throw new InvalidOperationException(System.Data.Entity.Resources.Strings.LightweightEntityConfiguration_InvalidNavigationProperty((object) propertyInfo.Name));
        if (!propertyInfo.DeclaringType.IsAssignableFrom(this._configuration.NavigationProperty.PropertyType.GetTargetType()))
          throw new InvalidOperationException(System.Data.Entity.Resources.Strings.LightweightEntityConfiguration_MismatchedInverseNavigationProperty((object) this._configuration.NavigationProperty.PropertyType.GetTargetType().FullName, (object) this._configuration.NavigationProperty.Name, (object) propertyInfo.DeclaringType.FullName, (object) propertyInfo.Name));
        if (!this._configuration.NavigationProperty.DeclaringType.IsAssignableFrom(propertyInfo.PropertyType.GetTargetType()))
          throw new InvalidOperationException(System.Data.Entity.Resources.Strings.LightweightEntityConfiguration_InvalidInverseNavigationProperty((object) this._configuration.NavigationProperty.DeclaringType.FullName, (object) this._configuration.NavigationProperty.Name, (object) propertyInfo.PropertyType.GetTargetType().FullName, (object) propertyInfo.Name));
        if (this._configuration.InverseEndKind.HasValue)
          ConventionNavigationPropertyConfiguration.VerifyMultiplicityCompatibility(this._configuration.InverseEndKind.Value, propertyInfo);
        this._modelConfiguration.Entity(this._configuration.NavigationProperty.PropertyType.GetTargetType()).Navigation(propertyInfo);
        this._configuration.InverseNavigationProperty = propertyInfo;
      }
      return this;
    }

    public virtual ConventionNavigationPropertyConfiguration HasInverseEndMultiplicity(
      RelationshipMultiplicity multiplicity)
    {
      if (this._configuration != null && !this._configuration.InverseEndKind.HasValue)
      {
        if (this._configuration.InverseNavigationProperty != (PropertyInfo) null)
          ConventionNavigationPropertyConfiguration.VerifyMultiplicityCompatibility(multiplicity, this._configuration.InverseNavigationProperty);
        this._configuration.InverseEndKind = new RelationshipMultiplicity?(multiplicity);
      }
      return this;
    }

    public virtual ConventionNavigationPropertyConfiguration IsDeclaringTypePrincipal(
      bool isPrincipal)
    {
      if (this._configuration != null && !this._configuration.IsNavigationPropertyDeclaringTypePrincipal.HasValue)
        this._configuration.IsNavigationPropertyDeclaringTypePrincipal = new bool?(isPrincipal);
      return this;
    }

    public virtual ConventionNavigationPropertyConfiguration HasDeleteAction(
      OperationAction deleteAction)
    {
      if (this._configuration != null && !this._configuration.DeleteAction.HasValue)
        this._configuration.DeleteAction = new OperationAction?(deleteAction);
      return this;
    }

    public virtual ConventionNavigationPropertyConfiguration HasRelationshipMultiplicity(
      RelationshipMultiplicity multiplicity)
    {
      if (this._configuration != null && !this._configuration.RelationshipMultiplicity.HasValue)
      {
        ConventionNavigationPropertyConfiguration.VerifyMultiplicityCompatibility(multiplicity, this._configuration.NavigationProperty);
        this._configuration.RelationshipMultiplicity = new RelationshipMultiplicity?(multiplicity);
      }
      return this;
    }

    private static void VerifyMultiplicityCompatibility(
      RelationshipMultiplicity multiplicity,
      PropertyInfo propertyInfo)
    {
      bool flag;
      switch (multiplicity)
      {
        case RelationshipMultiplicity.ZeroOrOne:
        case RelationshipMultiplicity.One:
          flag = !propertyInfo.PropertyType.IsCollection();
          break;
        case RelationshipMultiplicity.Many:
          flag = propertyInfo.PropertyType.IsCollection();
          break;
        default:
          throw new InvalidOperationException(System.Data.Entity.Resources.Strings.LightweightNavigationPropertyConfiguration_InvalidMultiplicity((object) multiplicity));
      }
      if (!flag)
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.LightweightNavigationPropertyConfiguration_IncompatibleMultiplicity((object) RelationshipMultiplicityConverter.MultiplicityToString(multiplicity), (object) (propertyInfo.DeclaringType.Name + "." + propertyInfo.Name), (object) propertyInfo.PropertyType));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
