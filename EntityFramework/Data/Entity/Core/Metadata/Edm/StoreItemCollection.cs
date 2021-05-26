// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.StoreItemCollection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.QueryCache;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Data.Entity.Core.SchemaObjectModel;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using System.Text;
using System.Xml;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Class for representing a collection of items in Store space.
  /// </summary>
  public class StoreItemCollection : ItemCollection
  {
    private double _schemaVersion;
    private readonly CacheForPrimitiveTypes _primitiveTypeMaps = new CacheForPrimitiveTypes();
    private readonly Memoizer<EdmFunction, EdmFunction> _cachedCTypeFunction;
    private readonly DbProviderManifest _providerManifest;
    private readonly string _providerInvariantName;
    private readonly string _providerManifestToken;
    private readonly DbProviderFactory _providerFactory;
    private readonly QueryCacheManager _queryCacheManager = QueryCacheManager.Create();

    internal StoreItemCollection()
      : base(DataSpace.SSpace)
    {
    }

    internal StoreItemCollection(
      DbProviderFactory factory,
      DbProviderManifest manifest,
      string providerInvariantName,
      string providerManifestToken)
      : base(DataSpace.SSpace)
    {
      this._providerFactory = factory;
      this._providerManifest = manifest;
      this._providerInvariantName = providerInvariantName;
      this._providerManifestToken = providerManifestToken;
      this._cachedCTypeFunction = new Memoizer<EdmFunction, EdmFunction>(new Func<EdmFunction, EdmFunction>(StoreItemCollection.ConvertFunctionSignatureToCType), (IEqualityComparer<EdmFunction>) null);
      this.LoadProviderManifest(this._providerManifest);
    }

    private StoreItemCollection(
      IEnumerable<XmlReader> xmlReaders,
      ReadOnlyCollection<string> filePaths,
      IDbDependencyResolver resolver,
      out IList<EdmSchemaError> errors)
      : base(DataSpace.SSpace)
    {
      errors = this.Init(xmlReaders, (IEnumerable<string>) filePaths, false, resolver, out this._providerManifest, out this._providerFactory, out this._providerInvariantName, out this._providerManifestToken, out this._cachedCTypeFunction);
    }

    internal StoreItemCollection(IEnumerable<XmlReader> xmlReaders, IEnumerable<string> filePaths)
      : base(DataSpace.SSpace)
    {
      EntityUtil.CheckArgumentEmpty<XmlReader>(ref xmlReaders, new Func<string, string>(System.Data.Entity.Resources.Strings.StoreItemCollectionMustHaveOneArtifact), "xmlReader");
      this.Init(xmlReaders, filePaths, true, (IDbDependencyResolver) null, out this._providerManifest, out this._providerFactory, out this._providerInvariantName, out this._providerManifestToken, out this._cachedCTypeFunction);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.StoreItemCollection" /> class using the specified XMLReader.
    /// </summary>
    /// <param name="xmlReaders">The XMLReader used to create metadata.</param>
    public StoreItemCollection(IEnumerable<XmlReader> xmlReaders)
      : base(DataSpace.SSpace)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<XmlReader>>(xmlReaders, nameof (xmlReaders));
      EntityUtil.CheckArgumentEmpty<XmlReader>(ref xmlReaders, new Func<string, string>(System.Data.Entity.Resources.Strings.StoreItemCollectionMustHaveOneArtifact), "xmlReader");
      MetadataArtifactLoader compositeFromXmlReaders = MetadataArtifactLoader.CreateCompositeFromXmlReaders(xmlReaders);
      this.Init((IEnumerable<XmlReader>) compositeFromXmlReaders.GetReaders(), (IEnumerable<string>) compositeFromXmlReaders.GetPaths(), true, (IDbDependencyResolver) null, out this._providerManifest, out this._providerFactory, out this._providerInvariantName, out this._providerManifestToken, out this._cachedCTypeFunction);
    }

    /// <summary>Initializes a new instances of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.StoreItemCollection" /> class.</summary>
    /// <param name="model">The model of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.StoreItemCollection" />.</param>
    public StoreItemCollection(EdmModel model)
      : base(DataSpace.SSpace)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmModel>(model, nameof (model));
      this._providerManifest = model.ProviderManifest;
      this._providerInvariantName = model.ProviderInfo.ProviderInvariantName;
      this._providerFactory = DbConfiguration.DependencyResolver.GetService<DbProviderFactory>((object) this._providerInvariantName);
      this._providerManifestToken = model.ProviderInfo.ProviderManifestToken;
      this._cachedCTypeFunction = new Memoizer<EdmFunction, EdmFunction>(new Func<EdmFunction, EdmFunction>(StoreItemCollection.ConvertFunctionSignatureToCType), (IEqualityComparer<EdmFunction>) null);
      this.LoadProviderManifest(this._providerManifest);
      this._schemaVersion = model.SchemaVersion;
      model.Validate();
      foreach (GlobalItem globalItem in model.GlobalItems)
      {
        globalItem.SetReadOnly();
        this.AddInternal(globalItem);
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.StoreItemCollection" /> class using the specified file paths.
    /// </summary>
    /// <param name="filePaths">The file paths used to create metadata.</param>
    public StoreItemCollection(params string[] filePaths)
      : base(DataSpace.SSpace)
    {
      System.Data.Entity.Utilities.Check.NotNull<string[]>(filePaths, nameof (filePaths));
      IEnumerable<string> enumerableArgument1 = (IEnumerable<string>) filePaths;
      EntityUtil.CheckArgumentEmpty<string>(ref enumerableArgument1, new Func<string, string>(System.Data.Entity.Resources.Strings.StoreItemCollectionMustHaveOneArtifact), nameof (filePaths));
      List<XmlReader> source = (List<XmlReader>) null;
      try
      {
        MetadataArtifactLoader compositeFromFilePaths = MetadataArtifactLoader.CreateCompositeFromFilePaths(enumerableArgument1, ".ssdl");
        source = compositeFromFilePaths.CreateReaders(DataSpace.SSpace);
        IEnumerable<XmlReader> enumerableArgument2 = source.AsEnumerable<XmlReader>();
        EntityUtil.CheckArgumentEmpty<XmlReader>(ref enumerableArgument2, new Func<string, string>(System.Data.Entity.Resources.Strings.StoreItemCollectionMustHaveOneArtifact), nameof (filePaths));
        this.Init((IEnumerable<XmlReader>) source, (IEnumerable<string>) compositeFromFilePaths.GetPaths(DataSpace.SSpace), true, (IDbDependencyResolver) null, out this._providerManifest, out this._providerFactory, out this._providerInvariantName, out this._providerManifestToken, out this._cachedCTypeFunction);
      }
      finally
      {
        if (source != null)
          Helper.DisposeXmlReaders((IEnumerable<XmlReader>) source);
      }
    }

    private IList<EdmSchemaError> Init(
      IEnumerable<XmlReader> xmlReaders,
      IEnumerable<string> filePaths,
      bool throwOnError,
      IDbDependencyResolver resolver,
      out DbProviderManifest providerManifest,
      out DbProviderFactory providerFactory,
      out string providerInvariantName,
      out string providerManifestToken,
      out Memoizer<EdmFunction, EdmFunction> cachedCTypeFunction)
    {
      cachedCTypeFunction = new Memoizer<EdmFunction, EdmFunction>(new Func<EdmFunction, EdmFunction>(StoreItemCollection.ConvertFunctionSignatureToCType), (IEqualityComparer<EdmFunction>) null);
      StoreItemCollection.Loader loader = new StoreItemCollection.Loader(xmlReaders, filePaths, throwOnError, resolver);
      providerFactory = loader.ProviderFactory;
      providerManifest = loader.ProviderManifest;
      providerManifestToken = loader.ProviderManifestToken;
      providerInvariantName = loader.ProviderInvariantName;
      if (!loader.HasNonWarningErrors)
      {
        this.LoadProviderManifest(loader.ProviderManifest);
        List<EdmSchemaError> edmSchemaErrorList = EdmItemCollection.LoadItems(this._providerManifest, loader.Schemas, (ItemCollection) this);
        foreach (EdmSchemaError edmSchemaError in edmSchemaErrorList)
          loader.Errors.Add(edmSchemaError);
        if (throwOnError && edmSchemaErrorList.Count != 0)
          loader.ThrowOnNonWarningErrors();
      }
      return loader.Errors;
    }

    internal QueryCacheManager QueryCacheManager => this._queryCacheManager;

    /// <summary>Gets the provider factory of the StoreItemCollection.</summary>
    /// <returns>The provider factory of the StoreItemCollection.</returns>
    public virtual DbProviderFactory ProviderFactory => this._providerFactory;

    /// <summary>Gets the provider manifest of the StoreItemCollection.</summary>
    /// <returns>The provider manifest of the StoreItemCollection.</returns>
    public virtual DbProviderManifest ProviderManifest => this._providerManifest;

    /// <summary>Gets the manifest token of the StoreItemCollection.</summary>
    /// <returns>The manifest token of the StoreItemCollection.</returns>
    public virtual string ProviderManifestToken => this._providerManifestToken;

    /// <summary>Gets the invariant name of the StoreItemCollection.</summary>
    /// <returns>The invariant name of the StoreItemCollection.</returns>
    public virtual string ProviderInvariantName => this._providerInvariantName;

    /// <summary>Gets the version of the store schema for this collection.</summary>
    /// <returns>The version of the store schema for this collection.</returns>
    public double StoreSchemaVersion
    {
      get => this._schemaVersion;
      internal set => this._schemaVersion = value;
    }

    /// <summary>
    /// Returns a collection of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" /> objects.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> object that represents the collection of the
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// objects.
    /// </returns>
    public virtual ReadOnlyCollection<PrimitiveType> GetPrimitiveTypes() => this._primitiveTypeMaps.GetTypes();

    internal override PrimitiveType GetMappedPrimitiveType(
      PrimitiveTypeKind primitiveTypeKind)
    {
      PrimitiveType type = (PrimitiveType) null;
      this._primitiveTypeMaps.TryGetType(primitiveTypeKind, (IEnumerable<Facet>) null, out type);
      return type;
    }

    private void LoadProviderManifest(DbProviderManifest storeManifest)
    {
      foreach (PrimitiveType storeType in storeManifest.GetStoreTypes())
      {
        this.AddInternal((GlobalItem) storeType);
        this._primitiveTypeMaps.Add(storeType);
      }
      foreach (GlobalItem storeFunction in storeManifest.GetStoreFunctions())
        this.AddInternal(storeFunction);
    }

    internal ReadOnlyCollection<EdmFunction> GetCTypeFunctions(
      string functionName,
      bool ignoreCase)
    {
      ReadOnlyCollection<EdmFunction> functionOverloads;
      if (!this.FunctionLookUpTable.TryGetValue(functionName, out functionOverloads))
        return Helper.EmptyEdmFunctionReadOnlyCollection;
      ReadOnlyCollection<EdmFunction> ctypeFunctions = this.ConvertToCTypeFunctions(functionOverloads);
      return ignoreCase ? ctypeFunctions : ItemCollection.GetCaseSensitiveFunctions(ctypeFunctions, functionName);
    }

    private ReadOnlyCollection<EdmFunction> ConvertToCTypeFunctions(
      ReadOnlyCollection<EdmFunction> functionOverloads)
    {
      List<EdmFunction> edmFunctionList = new List<EdmFunction>();
      foreach (EdmFunction functionOverload in functionOverloads)
        edmFunctionList.Add(this.ConvertToCTypeFunction(functionOverload));
      return new ReadOnlyCollection<EdmFunction>((IList<EdmFunction>) edmFunctionList);
    }

    internal EdmFunction ConvertToCTypeFunction(EdmFunction sTypeFunction) => this._cachedCTypeFunction.Evaluate(sTypeFunction);

    internal static EdmFunction ConvertFunctionSignatureToCType(EdmFunction sTypeFunction)
    {
      if (sTypeFunction.IsFromProviderManifest)
        return sTypeFunction;
      FunctionParameter functionParameter1 = (FunctionParameter) null;
      if (sTypeFunction.ReturnParameter != null)
      {
        TypeUsage edmTypeUsage = MetadataHelper.ConvertStoreTypeUsageToEdmTypeUsage(sTypeFunction.ReturnParameter.TypeUsage);
        functionParameter1 = new FunctionParameter(sTypeFunction.ReturnParameter.Name, edmTypeUsage, sTypeFunction.ReturnParameter.GetParameterMode());
      }
      List<FunctionParameter> functionParameterList = new List<FunctionParameter>();
      if (sTypeFunction.Parameters.Count > 0)
      {
        foreach (FunctionParameter parameter in sTypeFunction.Parameters)
        {
          TypeUsage edmTypeUsage = MetadataHelper.ConvertStoreTypeUsageToEdmTypeUsage(parameter.TypeUsage);
          FunctionParameter functionParameter2 = new FunctionParameter(parameter.Name, edmTypeUsage, parameter.GetParameterMode());
          functionParameterList.Add(functionParameter2);
        }
      }
      FunctionParameter[] functionParameterArray1;
      if (functionParameter1 != null)
        functionParameterArray1 = new FunctionParameter[1]
        {
          functionParameter1
        };
      else
        functionParameterArray1 = new FunctionParameter[0];
      FunctionParameter[] functionParameterArray2 = functionParameterArray1;
      EdmFunction edmFunction = new EdmFunction(sTypeFunction.Name, sTypeFunction.NamespaceName, DataSpace.CSpace, new EdmFunctionPayload()
      {
        Schema = sTypeFunction.Schema,
        StoreFunctionName = sTypeFunction.StoreFunctionNameAttribute,
        CommandText = sTypeFunction.CommandTextAttribute,
        IsAggregate = new bool?(sTypeFunction.AggregateAttribute),
        IsBuiltIn = new bool?(sTypeFunction.BuiltInAttribute),
        IsNiladic = new bool?(sTypeFunction.NiladicFunctionAttribute),
        IsComposable = new bool?(sTypeFunction.IsComposableAttribute),
        IsFromProviderManifest = new bool?(sTypeFunction.IsFromProviderManifest),
        IsCachedStoreFunction = new bool?(true),
        IsFunctionImport = new bool?(sTypeFunction.IsFunctionImport),
        ReturnParameters = (IList<FunctionParameter>) functionParameterArray2,
        Parameters = (IList<FunctionParameter>) functionParameterList.ToArray(),
        ParameterTypeSemantics = new ParameterTypeSemantics?(sTypeFunction.ParameterTypeSemanticsAttribute)
      });
      edmFunction.SetReadOnly();
      return edmFunction;
    }

    /// <summary>
    /// Factory method that creates a <see cref="T:System.Data.Entity.Core.Metadata.Edm.StoreItemCollection" />.
    /// </summary>
    /// <param name="xmlReaders">
    /// SSDL artifacts to load. Must not be <c>null</c>.
    /// </param>
    /// <param name="filePaths">
    /// Paths to SSDL artifacts. Used in error messages. Can be <c>null</c> in which case
    /// the base Uri of the XmlReader will be used as a path.
    /// </param>
    /// <param name="resolver">
    /// Custom resolver. Currently used to resolve DbProviderServices implementation. If <c>null</c>
    /// the default resolver will be used.
    /// </param>
    /// <param name="errors">
    /// The collection of errors encountered while loading.
    /// </param>
    /// <returns>
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.StoreItemCollection" /> instance if no errors encountered. Otherwise <c>null</c>.
    /// </returns>
    public static StoreItemCollection Create(
      IEnumerable<XmlReader> xmlReaders,
      ReadOnlyCollection<string> filePaths,
      IDbDependencyResolver resolver,
      out IList<EdmSchemaError> errors)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<XmlReader>>(xmlReaders, nameof (xmlReaders));
      EntityUtil.CheckArgumentContainsNull<XmlReader>(ref xmlReaders, nameof (xmlReaders));
      EntityUtil.CheckArgumentEmpty<XmlReader>(ref xmlReaders, new Func<string, string>(System.Data.Entity.Resources.Strings.StoreItemCollectionMustHaveOneArtifact), nameof (xmlReaders));
      StoreItemCollection storeItemCollection = new StoreItemCollection(xmlReaders, filePaths, resolver, out errors);
      return errors == null || errors.Count <= 0 ? storeItemCollection : (StoreItemCollection) null;
    }

    private class Loader
    {
      private string _provider;
      private string _providerManifestToken;
      private DbProviderManifest _providerManifest;
      private DbProviderFactory _providerFactory;
      private IList<EdmSchemaError> _errors;
      private IList<Schema> _schemas;
      private readonly bool _throwOnError;
      private readonly IDbDependencyResolver _resolver;

      public Loader(
        IEnumerable<XmlReader> xmlReaders,
        IEnumerable<string> sourceFilePaths,
        bool throwOnError,
        IDbDependencyResolver resolver)
      {
        this._throwOnError = throwOnError;
        this._resolver = resolver == null ? DbConfiguration.DependencyResolver : (IDbDependencyResolver) new CompositeResolver<IDbDependencyResolver, IDbDependencyResolver>(resolver, DbConfiguration.DependencyResolver);
        this.LoadItems(xmlReaders, sourceFilePaths);
      }

      public IList<EdmSchemaError> Errors => this._errors;

      public IList<Schema> Schemas => this._schemas;

      public DbProviderManifest ProviderManifest => this._providerManifest;

      public DbProviderFactory ProviderFactory => this._providerFactory;

      public string ProviderManifestToken => this._providerManifestToken;

      public string ProviderInvariantName => this._provider;

      public bool HasNonWarningErrors => !MetadataHelper.CheckIfAllErrorsAreWarnings(this._errors);

      private void LoadItems(IEnumerable<XmlReader> xmlReaders, IEnumerable<string> sourceFilePaths)
      {
        this._errors = SchemaManager.ParseAndValidate(xmlReaders, sourceFilePaths, SchemaDataModelOption.ProviderDataModel, new AttributeValueNotification(this.OnProviderNotification), new AttributeValueNotification(this.OnProviderManifestTokenNotification), new ProviderManifestNeeded(this.OnProviderManifestNeeded), out this._schemas);
        if (!this._throwOnError)
          return;
        this.ThrowOnNonWarningErrors();
      }

      internal void ThrowOnNonWarningErrors()
      {
        if (!MetadataHelper.CheckIfAllErrorsAreWarnings(this._errors))
          throw EntityUtil.InvalidSchemaEncountered(Helper.CombineErrorMessage((IEnumerable<EdmSchemaError>) this._errors));
      }

      private void OnProviderNotification(
        string provider,
        System.Action<string, ErrorCode, EdmSchemaErrorSeverity> addError)
      {
        string provider1 = this._provider;
        if (this._provider == null)
        {
          this._provider = provider;
          this.InitializeProviderManifest(addError);
        }
        else
        {
          if (this._provider == provider)
            return;
          addError(System.Data.Entity.Resources.Strings.AllArtifactsMustTargetSameProvider_InvariantName((object) provider1, (object) this._provider), ErrorCode.InconsistentProvider, EdmSchemaErrorSeverity.Error);
        }
      }

      private void InitializeProviderManifest(
        System.Action<string, ErrorCode, EdmSchemaErrorSeverity> addError)
      {
        if (this._providerManifest != null || this._providerManifestToken == null || this._provider == null)
          return;
        DbProviderFactory service;
        try
        {
          service = DbConfiguration.DependencyResolver.GetService<DbProviderFactory>((object) this._provider);
        }
        catch (ArgumentException ex)
        {
          addError(ex.Message, ErrorCode.InvalidProvider, EdmSchemaErrorSeverity.Error);
          return;
        }
        try
        {
          this._providerManifest = this._resolver.GetService<DbProviderServices>((object) this._provider).GetProviderManifest(this._providerManifestToken);
          this._providerFactory = service;
          if (!(this._providerManifest is EdmProviderManifest))
            return;
          if (this._throwOnError)
            throw new NotSupportedException(System.Data.Entity.Resources.Strings.OnlyStoreConnectionsSupported);
          addError(System.Data.Entity.Resources.Strings.OnlyStoreConnectionsSupported, ErrorCode.InvalidProvider, EdmSchemaErrorSeverity.Error);
        }
        catch (ProviderIncompatibleException ex)
        {
          if (this._throwOnError)
          {
            throw;
          }
          else
          {
            System.Action<string, ErrorCode, EdmSchemaErrorSeverity> addError1 = addError;
            StoreItemCollection.Loader.AddProviderIncompatibleError(ex, addError1);
          }
        }
      }

      private void OnProviderManifestTokenNotification(
        string token,
        System.Action<string, ErrorCode, EdmSchemaErrorSeverity> addError)
      {
        if (this._providerManifestToken == null)
        {
          this._providerManifestToken = token;
          this.InitializeProviderManifest(addError);
        }
        else
        {
          if (!(this._providerManifestToken != token))
            return;
          addError(System.Data.Entity.Resources.Strings.AllArtifactsMustTargetSameProvider_ManifestToken((object) token, (object) this._providerManifestToken), ErrorCode.ProviderManifestTokenMismatch, EdmSchemaErrorSeverity.Error);
        }
      }

      private DbProviderManifest OnProviderManifestNeeded(
        System.Action<string, ErrorCode, EdmSchemaErrorSeverity> addError)
      {
        if (this._providerManifest == null)
          addError(System.Data.Entity.Resources.Strings.ProviderManifestTokenNotFound, ErrorCode.ProviderManifestTokenNotFound, EdmSchemaErrorSeverity.Error);
        return this._providerManifest;
      }

      private static void AddProviderIncompatibleError(
        ProviderIncompatibleException provEx,
        System.Action<string, ErrorCode, EdmSchemaErrorSeverity> addError)
      {
        StringBuilder stringBuilder = new StringBuilder(provEx.Message);
        if (provEx.InnerException != null && !string.IsNullOrEmpty(provEx.InnerException.Message))
          stringBuilder.AppendFormat(" {0}", (object) provEx.InnerException.Message);
        addError(stringBuilder.ToString(), ErrorCode.FailedToRetrieveProviderManifest, EdmSchemaErrorSeverity.Error);
      }
    }
  }
}
