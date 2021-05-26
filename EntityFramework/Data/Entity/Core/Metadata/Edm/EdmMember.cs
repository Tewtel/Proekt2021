// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EdmMember
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Represents the edm member class</summary>
  public abstract class EdmMember : MetadataItem, INamedDataModelItem
  {
    private StructuralType _declaringType;
    private TypeUsage _typeUsage;
    private string _name;
    private string _identity;

    internal EdmMember()
    {
    }

    internal EdmMember(string name, TypeUsage memberTypeUsage)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotNull<TypeUsage>(memberTypeUsage, nameof (memberTypeUsage));
      this._name = name;
      this._typeUsage = memberTypeUsage;
    }

    string INamedDataModelItem.Identity => this.Identity;

    internal override string Identity => this._identity ?? this.Name;

    /// <summary>
    /// Gets or sets the name of the property. Setting this from a store-space model-convention will change the name of the database
    /// column for this property. In the conceptual model, this should align with the corresponding property from the entity class
    /// and should not be changed.
    /// </summary>
    /// <returns>The name of this member.</returns>
    [MetadataProperty(PrimitiveTypeKind.String, false)]
    public virtual string Name
    {
      get => this._name;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        Util.ThrowIfReadOnly((MetadataItem) this);
        if (string.Equals(this._name, value, StringComparison.Ordinal))
          return;
        string identity = this.Identity;
        this._name = value;
        if (this._declaringType == null)
          return;
        if (this._declaringType.Members.Except<EdmMember>((IEnumerable<EdmMember>) new EdmMember[1]
        {
          this
        }).Any<EdmMember>((Func<EdmMember, bool>) (c => string.Equals(this.Identity, c.Identity, StringComparison.Ordinal))))
          this._identity = this._declaringType.Members.Select<EdmMember, string>((Func<EdmMember, string>) (i => i.Identity)).Uniquify(this.Identity);
        this._declaringType.NotifyItemIdentityChanged(this, identity);
      }
    }

    /// <summary>Gets the type on which this member is declared.</summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.StructuralType" /> object that represents the type on which this member is declared.
    /// </returns>
    public virtual StructuralType DeclaringType => this._declaringType;

    /// <summary>
    /// Gets the instance of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.TypeUsage" /> class that contains both the type of the member and facets for the type.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.TypeUsage" /> object that contains both the type of the member and facets for the type.
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.TypeUsage, false)]
    public virtual TypeUsage TypeUsage
    {
      get => this._typeUsage;
      protected set
      {
        System.Data.Entity.Utilities.Check.NotNull<TypeUsage>(value, nameof (value));
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._typeUsage = value;
      }
    }

    /// <summary>Returns the name of this member.</summary>
    /// <returns>The name of this member.</returns>
    public override string ToString() => this.Name;

    internal override void SetReadOnly()
    {
      if (this.IsReadOnly)
        return;
      base.SetReadOnly();
      string identity = this._identity;
      this._identity = this.Name;
      if (this._declaringType == null || identity == null || string.Equals(identity, this._identity, StringComparison.Ordinal))
        return;
      this._declaringType.NotifyItemIdentityChanged(this, identity);
    }

    internal void ChangeDeclaringTypeWithoutCollectionFixup(StructuralType newDeclaringType) => this._declaringType = newDeclaringType;

    /// <summary>
    /// Tells whether this member is marked as a Computed member in the EDM definition
    /// </summary>
    public bool IsStoreGeneratedComputed
    {
      get
      {
        Facet facet;
        return this.TypeUsage.Facets.TryGetValue("StoreGeneratedPattern", false, out facet) && (StoreGeneratedPattern) facet.Value == StoreGeneratedPattern.Computed;
      }
    }

    /// <summary>
    /// Tells whether this member's Store generated pattern is marked as Identity in the EDM definition
    /// </summary>
    public bool IsStoreGeneratedIdentity
    {
      get
      {
        Facet facet;
        return this.TypeUsage.Facets.TryGetValue("StoreGeneratedPattern", false, out facet) && (StoreGeneratedPattern) facet.Value == StoreGeneratedPattern.Identity;
      }
    }

    internal virtual bool IsPrimaryKeyColumn => this._declaringType is EntityTypeBase declaringType && declaringType.KeyMembers.Contains(this);
  }
}
