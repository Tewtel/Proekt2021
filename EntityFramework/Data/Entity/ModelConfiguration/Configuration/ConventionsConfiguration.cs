// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ConventionsConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration.Properties;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration.Conventions.Sets;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  ///     Allows the conventions used by a <see cref="T:System.Data.Entity.DbModelBuilder" /> instance to be customized.
  ///     The default conventions can be found in the System.Data.Entity.ModelConfiguration.Conventions namespace.
  /// </summary>
  public class ConventionsConfiguration
  {
    private readonly List<IConvention> _configurationConventions = new List<IConvention>();
    private readonly List<IConvention> _conceptualModelConventions = new List<IConvention>();
    private readonly List<IConvention> _conceptualToStoreMappingConventions = new List<IConvention>();
    private readonly List<IConvention> _storeModelConventions = new List<IConvention>();
    private readonly ConventionSet _initialConventionSet;

    internal ConventionsConfiguration()
      : this(V2ConventionSet.Conventions)
    {
    }

    internal ConventionsConfiguration(ConventionSet conventionSet)
    {
      this._configurationConventions.AddRange(conventionSet.ConfigurationConventions);
      this._conceptualModelConventions.AddRange(conventionSet.ConceptualModelConventions);
      this._conceptualToStoreMappingConventions.AddRange(conventionSet.ConceptualToStoreMappingConventions);
      this._storeModelConventions.AddRange(conventionSet.StoreModelConventions);
      this._initialConventionSet = conventionSet;
    }

    private ConventionsConfiguration(ConventionsConfiguration source)
    {
      this._configurationConventions.AddRange((IEnumerable<IConvention>) source._configurationConventions);
      this._conceptualModelConventions.AddRange((IEnumerable<IConvention>) source._conceptualModelConventions);
      this._conceptualToStoreMappingConventions.AddRange((IEnumerable<IConvention>) source._conceptualToStoreMappingConventions);
      this._storeModelConventions.AddRange((IEnumerable<IConvention>) source._storeModelConventions);
    }

    internal IEnumerable<IConvention> ConfigurationConventions => (IEnumerable<IConvention>) this._configurationConventions;

    internal IEnumerable<IConvention> ConceptualModelConventions => (IEnumerable<IConvention>) this._conceptualModelConventions;

    internal IEnumerable<IConvention> ConceptualToStoreMappingConventions => (IEnumerable<IConvention>) this._conceptualToStoreMappingConventions;

    internal IEnumerable<IConvention> StoreModelConventions => (IEnumerable<IConvention>) this._storeModelConventions;

    internal virtual ConventionsConfiguration Clone() => new ConventionsConfiguration(this);

    /// <summary>
    ///     Discover all conventions in the given assembly and add them to the <see cref="T:System.Data.Entity.DbModelBuilder" />.
    /// </summary>
    /// <remarks>
    ///     This method add all conventions ordered by type name. The order in which conventions are added
    ///     can have an impact on how they behave because it governs the order in which they are run.
    ///     All conventions found must have a parameterless public constructor.
    /// </remarks>
    /// <param name="assembly">The assembly containing conventions to be added.</param>
    public void AddFromAssembly(Assembly assembly)
    {
      System.Data.Entity.Utilities.Check.NotNull<Assembly>(assembly, nameof (assembly));
      new ConventionsTypeFinder().AddConventions((IEnumerable<Type>) assembly.GetAccessibleTypes().OrderBy<Type, string>((Func<Type, string>) (type => type.Name)), (Action<IConvention>) (convention => this.Add(convention)));
    }

    /// <summary>
    ///     Enables one or more conventions for the <see cref="T:System.Data.Entity.DbModelBuilder" />.
    /// </summary>
    /// <param name="conventions"> The conventions to be enabled. </param>
    public void Add(params IConvention[] conventions)
    {
      System.Data.Entity.Utilities.Check.NotNull<IConvention[]>(conventions, nameof (conventions));
      foreach (IConvention convention in conventions)
      {
        bool flag = true;
        if (ConventionsTypeFilter.IsConfigurationConvention(convention.GetType()))
        {
          flag = false;
          int index = this._configurationConventions.FindIndex((Predicate<IConvention>) (initialConvention => this._initialConventionSet.ConfigurationConventions.Contains<IConvention>(initialConvention)));
          this._configurationConventions.Insert(index == -1 ? this._configurationConventions.Count : index, convention);
        }
        if (ConventionsTypeFilter.IsConceptualModelConvention(convention.GetType()))
        {
          flag = false;
          this._conceptualModelConventions.Add(convention);
        }
        if (ConventionsTypeFilter.IsStoreModelConvention(convention.GetType()))
        {
          flag = false;
          this._storeModelConventions.Add(convention);
        }
        if (ConventionsTypeFilter.IsConceptualToStoreMappingConvention(convention.GetType()))
        {
          flag = false;
          this._conceptualToStoreMappingConventions.Add(convention);
        }
        if (flag)
          throw new InvalidOperationException(System.Data.Entity.Resources.Strings.ConventionsConfiguration_InvalidConventionType((object) convention.GetType()));
      }
    }

    /// <summary>
    ///     Enables a convention for the <see cref="T:System.Data.Entity.DbModelBuilder" />.
    /// </summary>
    /// <typeparam name="TConvention"> The type of the convention to be enabled. </typeparam>
    public void Add<TConvention>() where TConvention : IConvention, new() => this.Add((IConvention) new TConvention());

    /// <summary>
    ///     Enables a convention for the <see cref="T:System.Data.Entity.DbModelBuilder" />. This convention
    ///     will run after the one specified.
    /// </summary>
    /// <typeparam name="TExistingConvention"> The type of the convention after which the enabled one will run. </typeparam>
    /// <param name="newConvention"> The convention to enable. </param>
    public void AddAfter<TExistingConvention>(IConvention newConvention) where TExistingConvention : IConvention
    {
      System.Data.Entity.Utilities.Check.NotNull<IConvention>(newConvention, nameof (newConvention));
      bool flag = true;
      if (ConventionsTypeFilter.IsConfigurationConvention(newConvention.GetType()) && ConventionsTypeFilter.IsConfigurationConvention(typeof (TExistingConvention)))
      {
        flag = false;
        ConventionsConfiguration.Insert(typeof (TExistingConvention), 1, newConvention, (IList<IConvention>) this._configurationConventions);
      }
      if (ConventionsTypeFilter.IsConceptualModelConvention(newConvention.GetType()) && ConventionsTypeFilter.IsConceptualModelConvention(typeof (TExistingConvention)))
      {
        flag = false;
        ConventionsConfiguration.Insert(typeof (TExistingConvention), 1, newConvention, (IList<IConvention>) this._conceptualModelConventions);
      }
      if (ConventionsTypeFilter.IsStoreModelConvention(newConvention.GetType()) && ConventionsTypeFilter.IsStoreModelConvention(typeof (TExistingConvention)))
      {
        flag = false;
        ConventionsConfiguration.Insert(typeof (TExistingConvention), 1, newConvention, (IList<IConvention>) this._storeModelConventions);
      }
      if (ConventionsTypeFilter.IsConceptualToStoreMappingConvention(newConvention.GetType()) && ConventionsTypeFilter.IsConceptualToStoreMappingConvention(typeof (TExistingConvention)))
      {
        flag = false;
        ConventionsConfiguration.Insert(typeof (TExistingConvention), 1, newConvention, (IList<IConvention>) this._conceptualToStoreMappingConventions);
      }
      if (flag)
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.ConventionsConfiguration_ConventionTypeMissmatch((object) newConvention.GetType(), (object) typeof (TExistingConvention)));
    }

    /// <summary>
    ///     Enables a configuration convention for the <see cref="T:System.Data.Entity.DbModelBuilder" />. This convention
    ///     will run before the one specified.
    /// </summary>
    /// <typeparam name="TExistingConvention"> The type of the convention before which the enabled one will run. </typeparam>
    /// <param name="newConvention"> The convention to enable. </param>
    public void AddBefore<TExistingConvention>(IConvention newConvention) where TExistingConvention : IConvention
    {
      System.Data.Entity.Utilities.Check.NotNull<IConvention>(newConvention, nameof (newConvention));
      bool flag = true;
      if (ConventionsTypeFilter.IsConfigurationConvention(newConvention.GetType()) && ConventionsTypeFilter.IsConfigurationConvention(typeof (TExistingConvention)))
      {
        flag = false;
        ConventionsConfiguration.Insert(typeof (TExistingConvention), 0, newConvention, (IList<IConvention>) this._configurationConventions);
      }
      if (ConventionsTypeFilter.IsConceptualModelConvention(newConvention.GetType()) && ConventionsTypeFilter.IsConceptualModelConvention(typeof (TExistingConvention)))
      {
        flag = false;
        ConventionsConfiguration.Insert(typeof (TExistingConvention), 0, newConvention, (IList<IConvention>) this._conceptualModelConventions);
      }
      if (ConventionsTypeFilter.IsStoreModelConvention(newConvention.GetType()) && ConventionsTypeFilter.IsStoreModelConvention(typeof (TExistingConvention)))
      {
        flag = false;
        ConventionsConfiguration.Insert(typeof (TExistingConvention), 0, newConvention, (IList<IConvention>) this._storeModelConventions);
      }
      if (ConventionsTypeFilter.IsConceptualToStoreMappingConvention(newConvention.GetType()) && ConventionsTypeFilter.IsConceptualToStoreMappingConvention(typeof (TExistingConvention)))
      {
        flag = false;
        ConventionsConfiguration.Insert(typeof (TExistingConvention), 0, newConvention, (IList<IConvention>) this._conceptualToStoreMappingConventions);
      }
      if (flag)
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.ConventionsConfiguration_ConventionTypeMissmatch((object) newConvention.GetType(), (object) typeof (TExistingConvention)));
    }

    private static void Insert(
      Type existingConventionType,
      int offset,
      IConvention newConvention,
      IList<IConvention> conventions)
    {
      int num = ConventionsConfiguration.IndexOf(existingConventionType, conventions);
      if (num < 0)
        throw System.Data.Entity.Resources.Error.ConventionNotFound((object) newConvention.GetType(), (object) existingConventionType);
      conventions.Insert(num + offset, newConvention);
    }

    private static int IndexOf(Type existingConventionType, IList<IConvention> conventions)
    {
      int num = 0;
      foreach (object convention in (IEnumerable<IConvention>) conventions)
      {
        if (convention.GetType() == existingConventionType)
          return num;
        ++num;
      }
      return -1;
    }

    /// <summary>
    ///     Disables one or more conventions for the <see cref="T:System.Data.Entity.DbModelBuilder" />.
    /// </summary>
    /// <param name="conventions"> The conventions to be disabled. </param>
    public void Remove(params IConvention[] conventions)
    {
      System.Data.Entity.Utilities.Check.NotNull<IConvention[]>(conventions, nameof (conventions));
      System.Data.Entity.Utilities.Check.NotNull<IConvention[]>(conventions, nameof (conventions));
      foreach (IConvention convention in conventions)
      {
        if (ConventionsTypeFilter.IsConfigurationConvention(convention.GetType()))
          this._configurationConventions.Remove(convention);
        if (ConventionsTypeFilter.IsConceptualModelConvention(convention.GetType()))
          this._conceptualModelConventions.Remove(convention);
        if (ConventionsTypeFilter.IsStoreModelConvention(convention.GetType()))
          this._storeModelConventions.Remove(convention);
        if (ConventionsTypeFilter.IsConceptualToStoreMappingConvention(convention.GetType()))
          this._conceptualToStoreMappingConventions.Remove(convention);
      }
    }

    /// <summary>
    ///     Disables a convention for the <see cref="T:System.Data.Entity.DbModelBuilder" />.
    ///     The default conventions that are available for removal can be found in the
    ///     System.Data.Entity.ModelConfiguration.Conventions namespace.
    /// </summary>
    /// <typeparam name="TConvention"> The type of the convention to be disabled. </typeparam>
    public void Remove<TConvention>() where TConvention : IConvention
    {
      if (ConventionsTypeFilter.IsConfigurationConvention(typeof (TConvention)))
        this._configurationConventions.RemoveAll((Predicate<IConvention>) (c => c.GetType() == typeof (TConvention)));
      if (ConventionsTypeFilter.IsConceptualModelConvention(typeof (TConvention)))
        this._conceptualModelConventions.RemoveAll((Predicate<IConvention>) (c => c.GetType() == typeof (TConvention)));
      if (ConventionsTypeFilter.IsStoreModelConvention(typeof (TConvention)))
        this._storeModelConventions.RemoveAll((Predicate<IConvention>) (c => c.GetType() == typeof (TConvention)));
      if (!ConventionsTypeFilter.IsConceptualToStoreMappingConvention(typeof (TConvention)))
        return;
      this._conceptualToStoreMappingConventions.RemoveAll((Predicate<IConvention>) (c => c.GetType() == typeof (TConvention)));
    }

    internal void ApplyConceptualModel(DbModel model)
    {
      foreach (IConvention conceptualModelConvention in this._conceptualModelConventions)
        new ConventionsConfiguration.ModelConventionDispatcher(conceptualModelConvention, model, DataSpace.CSpace).Dispatch();
    }

    internal void ApplyStoreModel(DbModel model)
    {
      foreach (IConvention storeModelConvention in this._storeModelConventions)
        new ConventionsConfiguration.ModelConventionDispatcher(storeModelConvention, model, DataSpace.SSpace).Dispatch();
    }

    internal void ApplyPluralizingTableNameConvention(DbModel model)
    {
      foreach (IConvention convention in this._storeModelConventions.Where<IConvention>((Func<IConvention, bool>) (c => c is PluralizingTableNameConvention)))
        new ConventionsConfiguration.ModelConventionDispatcher(convention, model, DataSpace.SSpace).Dispatch();
    }

    internal void ApplyMapping(DbDatabaseMapping databaseMapping)
    {
      foreach (IConvention mappingConvention1 in this._conceptualToStoreMappingConventions)
      {
        if (mappingConvention1 is IDbMappingConvention mappingConvention)
          mappingConvention.Apply(databaseMapping);
      }
    }

    internal virtual void ApplyModelConfiguration(System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      for (int index = this._configurationConventions.Count - 1; index >= 0; --index)
      {
        IConvention configurationConvention1 = this._configurationConventions[index];
        if (configurationConvention1 is IConfigurationConvention configurationConvention3)
          configurationConvention3.Apply(modelConfiguration);
        if (configurationConvention1 is Convention convention2)
          convention2.ApplyModelConfiguration(modelConfiguration);
      }
    }

    internal virtual void ApplyModelConfiguration(Type type, System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      for (int index = this._configurationConventions.Count - 1; index >= 0; --index)
      {
        IConvention configurationConvention1 = this._configurationConventions[index];
        if (configurationConvention1 is IConfigurationConvention<Type> configurationConvention3)
          configurationConvention3.Apply(type, modelConfiguration);
        if (configurationConvention1 is Convention convention2)
          convention2.ApplyModelConfiguration(type, modelConfiguration);
      }
    }

    internal virtual void ApplyTypeConfiguration<TStructuralTypeConfiguration>(
      Type type,
      Func<TStructuralTypeConfiguration> structuralTypeConfiguration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
      where TStructuralTypeConfiguration : StructuralTypeConfiguration
    {
      for (int index = this._configurationConventions.Count - 1; index >= 0; --index)
      {
        IConvention configurationConvention1 = this._configurationConventions[index];
        if (configurationConvention1 is IConfigurationConvention<Type, TStructuralTypeConfiguration> configurationConvention6)
          configurationConvention6.Apply(type, structuralTypeConfiguration, modelConfiguration);
        if (configurationConvention1 is IConfigurationConvention<Type, StructuralTypeConfiguration> configurationConvention7)
          configurationConvention7.Apply(type, (Func<StructuralTypeConfiguration>) structuralTypeConfiguration, modelConfiguration);
        if (configurationConvention1 is Convention convention2)
          convention2.ApplyTypeConfiguration<TStructuralTypeConfiguration>(type, structuralTypeConfiguration, modelConfiguration);
      }
    }

    internal virtual void ApplyPropertyConfiguration(
      PropertyInfo propertyInfo,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      for (int index = this._configurationConventions.Count - 1; index >= 0; --index)
      {
        IConvention configurationConvention1 = this._configurationConventions[index];
        if (configurationConvention1 is IConfigurationConvention<PropertyInfo> configurationConvention3)
          configurationConvention3.Apply(propertyInfo, modelConfiguration);
        if (configurationConvention1 is Convention convention2)
          convention2.ApplyPropertyConfiguration(propertyInfo, modelConfiguration);
      }
    }

    internal virtual void ApplyPropertyConfiguration(
      PropertyInfo propertyInfo,
      Func<PropertyConfiguration> propertyConfiguration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      Type configurationType = StructuralTypeConfiguration.GetPropertyConfigurationType(propertyInfo.PropertyType);
      for (int index = this._configurationConventions.Count - 1; index >= 0; --index)
      {
        IConvention configurationConvention = this._configurationConventions[index];
        new ConventionsConfiguration.PropertyConfigurationConventionDispatcher(configurationConvention, configurationType, propertyInfo, propertyConfiguration, modelConfiguration).Dispatch();
        if (configurationConvention is Convention convention2)
          convention2.ApplyPropertyConfiguration(propertyInfo, propertyConfiguration, modelConfiguration);
      }
    }

    internal virtual void ApplyPropertyTypeConfiguration<TStructuralTypeConfiguration>(
      PropertyInfo propertyInfo,
      Func<TStructuralTypeConfiguration> structuralTypeConfiguration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
      where TStructuralTypeConfiguration : StructuralTypeConfiguration
    {
      for (int index = this._configurationConventions.Count - 1; index >= 0; --index)
      {
        IConvention configurationConvention1 = this._configurationConventions[index];
        if (configurationConvention1 is IConfigurationConvention<PropertyInfo, TStructuralTypeConfiguration> configurationConvention6)
          configurationConvention6.Apply(propertyInfo, structuralTypeConfiguration, modelConfiguration);
        if (configurationConvention1 is IConfigurationConvention<PropertyInfo, StructuralTypeConfiguration> configurationConvention7)
          configurationConvention7.Apply(propertyInfo, (Func<StructuralTypeConfiguration>) structuralTypeConfiguration, modelConfiguration);
        if (configurationConvention1 is Convention convention2)
          convention2.ApplyPropertyTypeConfiguration<TStructuralTypeConfiguration>(propertyInfo, structuralTypeConfiguration, modelConfiguration);
      }
    }

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>
    ///     Gets the <see cref="T:System.Type" /> of the current instance.
    /// </summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();

    private class ModelConventionDispatcher : EdmModelVisitor
    {
      private readonly IConvention _convention;
      private readonly DbModel _model;
      private readonly DataSpace _dataSpace;

      public ModelConventionDispatcher(IConvention convention, DbModel model, DataSpace dataSpace)
      {
        System.Data.Entity.Utilities.Check.NotNull<IConvention>(convention, nameof (convention));
        System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
        this._convention = convention;
        this._model = model;
        this._dataSpace = dataSpace;
      }

      public void Dispatch() => this.VisitEdmModel(this._dataSpace == DataSpace.CSpace ? this._model.ConceptualModel : this._model.StoreModel);

      private void Dispatch<T>(T item) where T : MetadataItem
      {
        if (this._dataSpace == DataSpace.CSpace)
        {
          if (!(this._convention is IConceptualModelConvention<T> convention4))
            return;
          convention4.Apply(item, this._model);
        }
        else
        {
          if (!(this._convention is IStoreModelConvention<T> convention5))
            return;
          convention5.Apply(item, this._model);
        }
      }

      protected internal override void VisitEdmModel(EdmModel item)
      {
        this.Dispatch<EdmModel>(item);
        base.VisitEdmModel(item);
      }

      protected override void VisitEdmNavigationProperty(NavigationProperty item)
      {
        this.Dispatch<NavigationProperty>(item);
        base.VisitEdmNavigationProperty(item);
      }

      protected override void VisitEdmAssociationConstraint(ReferentialConstraint item)
      {
        this.Dispatch<ReferentialConstraint>(item);
        if (item == null)
          return;
        this.VisitMetadataItem((MetadataItem) item);
      }

      protected override void VisitEdmAssociationEnd(RelationshipEndMember item)
      {
        this.Dispatch<RelationshipEndMember>(item);
        base.VisitEdmAssociationEnd(item);
      }

      protected internal override void VisitEdmProperty(EdmProperty item)
      {
        this.Dispatch<EdmProperty>(item);
        base.VisitEdmProperty(item);
      }

      protected internal override void VisitMetadataItem(MetadataItem item)
      {
        this.Dispatch<MetadataItem>(item);
        base.VisitMetadataItem(item);
      }

      protected override void VisitEdmEntityContainer(EntityContainer item)
      {
        this.Dispatch<EntityContainer>(item);
        base.VisitEdmEntityContainer(item);
      }

      protected internal override void VisitEdmEntitySet(EntitySet item)
      {
        this.Dispatch<EntitySet>(item);
        base.VisitEdmEntitySet(item);
      }

      protected override void VisitEdmAssociationSet(AssociationSet item)
      {
        this.Dispatch<AssociationSet>(item);
        base.VisitEdmAssociationSet(item);
      }

      protected override void VisitEdmAssociationSetEnd(EntitySet item)
      {
        this.Dispatch<EntitySet>(item);
        base.VisitEdmAssociationSetEnd(item);
      }

      protected override void VisitComplexType(ComplexType item)
      {
        this.Dispatch<ComplexType>(item);
        base.VisitComplexType(item);
      }

      protected internal override void VisitEdmEntityType(EntityType item)
      {
        this.Dispatch<EntityType>(item);
        this.VisitMetadataItem((MetadataItem) item);
        if (item == null)
          return;
        this.VisitDeclaredProperties(item, (IList<EdmProperty>) item.DeclaredProperties);
        this.VisitDeclaredNavigationProperties(item, (IEnumerable<NavigationProperty>) item.DeclaredNavigationProperties);
      }

      protected internal override void VisitEdmAssociationType(AssociationType item)
      {
        this.Dispatch<AssociationType>(item);
        base.VisitEdmAssociationType(item);
      }
    }

    private class PropertyConfigurationConventionDispatcher
    {
      private readonly IConvention _convention;
      private readonly Type _propertyConfigurationType;
      private readonly PropertyInfo _propertyInfo;
      private readonly Func<PropertyConfiguration> _propertyConfiguration;
      private readonly System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration _modelConfiguration;

      public PropertyConfigurationConventionDispatcher(
        IConvention convention,
        Type propertyConfigurationType,
        PropertyInfo propertyInfo,
        Func<PropertyConfiguration> propertyConfiguration,
        System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
      {
        System.Data.Entity.Utilities.Check.NotNull<IConvention>(convention, nameof (convention));
        System.Data.Entity.Utilities.Check.NotNull<Type>(propertyConfigurationType, nameof (propertyConfigurationType));
        System.Data.Entity.Utilities.Check.NotNull<PropertyInfo>(propertyInfo, nameof (propertyInfo));
        System.Data.Entity.Utilities.Check.NotNull<Func<PropertyConfiguration>>(propertyConfiguration, nameof (propertyConfiguration));
        this._convention = convention;
        this._propertyConfigurationType = propertyConfigurationType;
        this._propertyInfo = propertyInfo;
        this._propertyConfiguration = propertyConfiguration;
        this._modelConfiguration = modelConfiguration;
      }

      public void Dispatch()
      {
        this.Dispatch<PropertyConfiguration>();
        this.Dispatch<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>();
        this.Dispatch<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.LengthPropertyConfiguration>();
        this.Dispatch<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration>();
        this.Dispatch<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DecimalPropertyConfiguration>();
        this.Dispatch<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.StringPropertyConfiguration>();
        this.Dispatch<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.BinaryPropertyConfiguration>();
        this.Dispatch<NavigationPropertyConfiguration>();
      }

      private void Dispatch<TPropertyConfiguration>() where TPropertyConfiguration : PropertyConfiguration
      {
        if (!(this._convention is IConfigurationConvention<PropertyInfo, TPropertyConfiguration> convention) || !typeof (TPropertyConfiguration).IsAssignableFrom(this._propertyConfigurationType))
          return;
        convention.Apply(this._propertyInfo, (Func<TPropertyConfiguration>) (() => (TPropertyConfiguration) this._propertyConfiguration()), this._modelConfiguration);
      }
    }
  }
}
