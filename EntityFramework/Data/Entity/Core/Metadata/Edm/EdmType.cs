// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EdmType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;
using System.Text;
using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Base EdmType class for all the model types</summary>
  public abstract class EdmType : GlobalItem, INamedDataModelItem
  {
    private CollectionType _collectionType;
    private string _name;
    private string _namespace;
    private EdmType _baseType;

    internal static IEnumerable<T> SafeTraverseHierarchy<T>(T startFrom) where T : EdmType
    {
      HashSet<T> visitedTypes = new HashSet<T>();
      for (T thisType = startFrom; (object) thisType != null && !visitedTypes.Contains(thisType); thisType = thisType.BaseType as T)
      {
        visitedTypes.Add(thisType);
        yield return thisType;
      }
    }

    internal EdmType()
    {
    }

    internal EdmType(string name, string namespaceName, DataSpace dataSpace)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotNull<string>(namespaceName, nameof (namespaceName));
      EdmType.Initialize(this, name, namespaceName, dataSpace, false, (EdmType) null);
    }

    internal string CacheIdentity { get; private set; }

    string INamedDataModelItem.Identity => this.Identity;

    internal override string Identity
    {
      get
      {
        if (this.CacheIdentity == null)
        {
          StringBuilder builder = new StringBuilder(50);
          this.BuildIdentity(builder);
          this.CacheIdentity = builder.ToString();
        }
        return this.CacheIdentity;
      }
    }

    /// <summary>Gets the name of this type.</summary>
    /// <returns>The name of this type.</returns>
    [MetadataProperty(PrimitiveTypeKind.String, false)]
    public virtual string Name
    {
      get => this._name;
      internal set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._name = value;
      }
    }

    /// <summary>Gets the namespace of this type.</summary>
    /// <returns>The namespace of this type.</returns>
    [MetadataProperty(PrimitiveTypeKind.String, false)]
    public virtual string NamespaceName
    {
      get => this._namespace;
      internal set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._namespace = value;
      }
    }

    /// <summary>Gets a value indicating whether this type is abstract or not. </summary>
    /// <returns>true if this type is abstract; otherwise, false. </returns>
    /// <exception cref="T:System.InvalidOperationException">Thrown if the setter is called on instance that is in ReadOnly state</exception>
    [MetadataProperty(PrimitiveTypeKind.Boolean, false)]
    public bool Abstract
    {
      get => this.GetFlag(MetadataItem.MetadataFlags.IsAbstract);
      internal set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this.SetFlag(MetadataItem.MetadataFlags.IsAbstract, value);
      }
    }

    /// <summary>Gets the base type of this type.</summary>
    /// <returns>The base type of this type.</returns>
    /// <exception cref="T:System.InvalidOperationException">Thrown if the setter is called on instance that is in ReadOnly state</exception>
    /// <exception cref="T:System.ArgumentException">Thrown if the value passed in for setter will create a loop in the inheritance chain</exception>
    [MetadataProperty(BuiltInTypeKind.EdmType, false)]
    public virtual EdmType BaseType
    {
      get => this._baseType;
      internal set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this.CheckBaseType(value);
        this._baseType = value;
      }
    }

    private void CheckBaseType(EdmType baseType)
    {
      for (EdmType edmType = baseType; edmType != null; edmType = edmType.BaseType)
      {
        if (edmType == this)
          throw new ArgumentException(Strings.CannotSetBaseTypeCyclicInheritance((object) baseType.Name, (object) this.Name));
      }
      if (baseType != null && Helper.IsEntityTypeBase(this) && (((EntityTypeBase) baseType).KeyMembers.Count != 0 && ((EntityTypeBase) this).KeyMembers.Count != 0))
        throw new ArgumentException(Strings.CannotDefineKeysOnBothBaseAndDerivedTypes);
    }

    /// <summary>Gets the full name of this type.</summary>
    /// <returns>The full name of this type. </returns>
    public virtual string FullName => this.Identity;

    internal virtual Type ClrType => (Type) null;

    internal override void BuildIdentity(StringBuilder builder)
    {
      if (this.CacheIdentity != null)
        builder.Append(this.CacheIdentity);
      else
        builder.Append(EdmType.CreateEdmTypeIdentity(this.NamespaceName, this.Name));
    }

    internal static string CreateEdmTypeIdentity(string namespaceName, string name)
    {
      string str = string.Empty;
      if (!string.IsNullOrEmpty(namespaceName))
        str = namespaceName + ".";
      return str + name;
    }

    internal static void Initialize(
      EdmType type,
      string name,
      string namespaceName,
      DataSpace dataSpace,
      bool isAbstract,
      EdmType baseType)
    {
      type._baseType = baseType;
      type._name = name;
      type._namespace = namespaceName;
      type.DataSpace = dataSpace;
      type.Abstract = isAbstract;
    }

    /// <summary>Returns the full name of this type.</summary>
    /// <returns>The full name of this type. </returns>
    public override string ToString() => this.FullName;

    /// <summary>
    /// Returns an instance of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.CollectionType" /> whose element type is this type.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Metadata.Edm.CollectionType" /> object whose element type is this type.
    /// </returns>
    public CollectionType GetCollectionType()
    {
      if (this._collectionType == null)
        Interlocked.CompareExchange<CollectionType>(ref this._collectionType, new CollectionType(this), (CollectionType) null);
      return this._collectionType;
    }

    internal virtual bool IsSubtypeOf(EdmType otherType) => Helper.IsSubtypeOf(this, otherType);

    internal virtual bool IsBaseTypeOf(EdmType otherType) => otherType != null && otherType.IsSubtypeOf(this);

    internal virtual bool IsAssignableFrom(EdmType otherType) => Helper.IsAssignableFrom(this, otherType);

    internal override void SetReadOnly()
    {
      if (this.IsReadOnly)
        return;
      base.SetReadOnly();
      this.BaseType?.SetReadOnly();
    }

    internal virtual IEnumerable<FacetDescription> GetAssociatedFacetDescriptions() => (IEnumerable<FacetDescription>) MetadataItem.GetGeneralFacetDescriptions();
  }
}
