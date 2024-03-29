﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.DbProviderManifest
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;
using System.Xml;

namespace System.Data.Entity.Core.Common
{
  /// <summary>Metadata Interface for all CLR types</summary>
  public abstract class DbProviderManifest
  {
    /// <summary>
    /// Value to pass to GetInformation to get the StoreSchemaDefinition
    /// </summary>
    public const string StoreSchemaDefinition = "StoreSchemaDefinition";
    /// <summary>
    /// Value to pass to GetInformation to get the StoreSchemaMapping
    /// </summary>
    public const string StoreSchemaMapping = "StoreSchemaMapping";
    /// <summary>
    /// Value to pass to GetInformation to get the ConceptualSchemaDefinition
    /// </summary>
    public const string ConceptualSchemaDefinition = "ConceptualSchemaDefinition";
    /// <summary>
    /// Value to pass to GetInformation to get the StoreSchemaDefinitionVersion3
    /// </summary>
    public const string StoreSchemaDefinitionVersion3 = "StoreSchemaDefinitionVersion3";
    /// <summary>
    /// Value to pass to GetInformation to get the StoreSchemaMappingVersion3
    /// </summary>
    public const string StoreSchemaMappingVersion3 = "StoreSchemaMappingVersion3";
    /// <summary>
    /// Value to pass to GetInformation to get the ConceptualSchemaDefinitionVersion3
    /// </summary>
    public const string ConceptualSchemaDefinitionVersion3 = "ConceptualSchemaDefinitionVersion3";
    /// <summary>Name of the MaxLength Facet</summary>
    public const string MaxLengthFacetName = "MaxLength";
    /// <summary>Name of the Unicode Facet</summary>
    public const string UnicodeFacetName = "Unicode";
    /// <summary>Name of the FixedLength Facet</summary>
    public const string FixedLengthFacetName = "FixedLength";
    /// <summary>Name of the Precision Facet</summary>
    public const string PrecisionFacetName = "Precision";
    /// <summary>Name of the Scale Facet</summary>
    public const string ScaleFacetName = "Scale";
    /// <summary>Name of the Nullable Facet</summary>
    public const string NullableFacetName = "Nullable";
    /// <summary>Name of the DefaultValue Facet</summary>
    public const string DefaultValueFacetName = "DefaultValue";
    /// <summary>Name of the Collation Facet</summary>
    public const string CollationFacetName = "Collation";
    /// <summary>Name of the SRID Facet</summary>
    public const string SridFacetName = "SRID";
    /// <summary>Name of the IsStrict Facet</summary>
    public const string IsStrictFacetName = "IsStrict";

    /// <summary>Gets the namespace used by this provider manifest.</summary>
    /// <returns>The namespace used by this provider manifest.</returns>
    public abstract string NamespaceName { get; }

    /// <summary>When overridden in a derived class, returns the set of primitive types supported by the data source.</summary>
    /// <returns>The set of types supported by the data source.</returns>
    public abstract ReadOnlyCollection<PrimitiveType> GetStoreTypes();

    /// <summary>When overridden in a derived class, returns a collection of EDM functions supported by the provider manifest.</summary>
    /// <returns>A collection of EDM functions.</returns>
    public abstract ReadOnlyCollection<EdmFunction> GetStoreFunctions();

    /// <summary>Returns the FacetDescription objects for a particular type.</summary>
    /// <returns>The FacetDescription objects for the specified EDM type.</returns>
    /// <param name="edmType">The EDM type to return the facet description for.</param>
    public abstract ReadOnlyCollection<FacetDescription> GetFacetDescriptions(
      EdmType edmType);

    /// <summary>When overridden in a derived class, this method maps the specified storage type and a set of facets for that type to an EDM type.</summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Metadata.Edm.TypeUsage" /> instance that describes an EDM type and a set of facets for that type.
    /// </returns>
    /// <param name="storeType">The TypeUsage instance that describes a storage type and a set of facets for that type to be mapped to the EDM type.</param>
    public abstract TypeUsage GetEdmType(TypeUsage storeType);

    /// <summary>When overridden in a derived class, this method maps the specified EDM type and a set of facets for that type to a storage type.</summary>
    /// <returns>The TypeUsage instance that describes a storage type and a set of facets for that type.</returns>
    /// <param name="edmType">The TypeUsage instance that describes the EDM type and a set of facets for that type to be mapped to a storage type.</param>
    public abstract TypeUsage GetStoreType(TypeUsage edmType);

    /// <summary>When overridden in a derived class, this method returns provider-specific information.</summary>
    /// <returns>The XmlReader object that represents the mapping to the underlying data store catalog.</returns>
    /// <param name="informationType">The type of the information to return.</param>
    protected abstract XmlReader GetDbInformation(string informationType);

    /// <summary>Gets the provider-specific information.</summary>
    /// <returns>The provider-specific information.</returns>
    /// <param name="informationType">The type of the information to return.</param>
    public XmlReader GetInformation(string informationType)
    {
      XmlReader dbInformation;
      try
      {
        dbInformation = this.GetDbInformation(informationType);
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new ProviderIncompatibleException(Strings.EntityClient_FailedToGetInformation((object) informationType), ex);
        throw;
      }
      if (dbInformation != null)
        return dbInformation;
      return informationType == "ConceptualSchemaDefinitionVersion3" || informationType == "ConceptualSchemaDefinition" ? DbProviderServices.GetConceptualSchemaDefinition(informationType) : throw new ProviderIncompatibleException(Strings.ProviderReturnedNullForGetDbInformation((object) informationType));
    }

    /// <summary>Indicates if the provider supports escaping strings to be used as patterns in a Like expression.</summary>
    /// <returns>True if this provider supports escaping strings to be used as patterns in a Like expression; otherwise, false.</returns>
    /// <param name="escapeCharacter">If the provider supports escaping, the character that would be used as the escape character.</param>
    public virtual bool SupportsEscapingLikeArgument(out char escapeCharacter)
    {
      escapeCharacter = char.MinValue;
      return false;
    }

    /// <summary>
    /// Indicates if the provider supports the parameter optimization described in EntityFramework6 GitHub issue #195.
    /// The default is <c>false</c>. Providers should change this to true only after testing that schema queries (as
    /// used in the Database First flow) work correctly with this flag.
    /// </summary>
    /// <returns><c>True</c> only if the provider supports the parameter optimization.</returns>
    public virtual bool SupportsParameterOptimizationInSchemaQueries() => false;

    /// <summary>Provider writers should override this method to return the argument with the wildcards and the escape character escaped. This method is only used if SupportsEscapingLikeArgument returns true.</summary>
    /// <returns>The argument with the wildcards and the escape character escaped.</returns>
    /// <param name="argument">The argument to be escaped.</param>
    public virtual string EscapeLikeArgument(string argument)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(argument, nameof (argument));
      throw new ProviderIncompatibleException(Strings.ProviderShouldOverrideEscapeLikeArgument);
    }

    /// <summary>
    /// Returns a boolean that specifies whether the provider can handle expression trees
    /// containing instances of DbInExpression.
    /// The default implementation returns <c>false</c> for backwards compatibility. Derived classes can override this method.
    /// </summary>
    /// <returns>
    /// <c>false</c>
    /// </returns>
    public virtual bool SupportsInExpression() => false;

    /// <summary>
    /// Returns a boolean that specifies whether the provider can process expression trees not having DbProjectExpression
    /// nodes directly under both Left and Right sides of DbUnionAllExpression and DbIntersectExpression
    /// </summary>
    /// <returns>
    /// <c>false</c>
    /// </returns>
    public virtual bool SupportsIntersectAndUnionAllFlattening() => false;
  }
}
