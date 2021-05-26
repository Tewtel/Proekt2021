// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.PropertyEntryMetadata
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Internal
{
  internal class PropertyEntryMetadata : MemberEntryMetadata
  {
    private readonly bool _isMapped;
    private readonly bool _isComplex;

    public PropertyEntryMetadata(
      Type declaringType,
      Type propertyType,
      string propertyName,
      bool isMapped,
      bool isComplex)
      : base(declaringType, propertyType, propertyName)
    {
      this._isMapped = isMapped;
      this._isComplex = isComplex;
    }

    public static PropertyEntryMetadata ValidateNameAndGetMetadata(
      InternalContext internalContext,
      Type declaringType,
      Type requestedType,
      string propertyName)
    {
      Type type;
      DbHelpers.GetPropertyTypes(declaringType).TryGetValue(propertyName, out type);
      MetadataWorkspace metadataWorkspace = internalContext.ObjectContext.MetadataWorkspace;
      StructuralType structuralType = metadataWorkspace.GetItem<StructuralType>(declaringType.FullNameWithNesting(), DataSpace.OSpace);
      bool isMapped = false;
      bool isComplex = false;
      EdmMember edmMember;
      structuralType.Members.TryGetValue(propertyName, false, out edmMember);
      if (edmMember != null)
      {
        if (!(edmMember is EdmProperty edmProperty2))
          return (PropertyEntryMetadata) null;
        if (type == (Type) null)
          type = !(edmProperty2.TypeUsage.EdmType is PrimitiveType edmType4) ? ((ObjectItemCollection) metadataWorkspace.GetItemCollection(DataSpace.OSpace)).GetClrType((StructuralType) edmProperty2.TypeUsage.EdmType) : edmType4.ClrEquivalentType;
        isMapped = true;
        isComplex = edmProperty2.TypeUsage.EdmType.BuiltInTypeKind == BuiltInTypeKind.ComplexType;
      }
      else
      {
        IDictionary<string, Func<object, object>> propertyGetters = DbHelpers.GetPropertyGetters(declaringType);
        IDictionary<string, Action<object, object>> propertySetters = DbHelpers.GetPropertySetters(declaringType);
        string key = propertyName;
        if (!propertyGetters.ContainsKey(key) && !propertySetters.ContainsKey(propertyName))
          return (PropertyEntryMetadata) null;
      }
      if (!requestedType.IsAssignableFrom(type))
        throw Error.DbEntityEntry_WrongGenericForProp((object) propertyName, (object) declaringType.Name, (object) requestedType.Name, (object) type.Name);
      return new PropertyEntryMetadata(declaringType, type, propertyName, isMapped, isComplex);
    }

    public override InternalMemberEntry CreateMemberEntry(
      InternalEntityEntry internalEntityEntry,
      InternalPropertyEntry parentPropertyEntry)
    {
      return parentPropertyEntry != null ? (InternalMemberEntry) new InternalNestedPropertyEntry(parentPropertyEntry, this) : (InternalMemberEntry) new InternalEntityPropertyEntry(internalEntityEntry, this);
    }

    public bool IsComplex => this._isComplex;

    public override MemberEntryType MemberEntryType => !this._isComplex ? MemberEntryType.ScalarProperty : MemberEntryType.ComplexProperty;

    public bool IsMapped => this._isMapped;

    public override Type MemberType => this.ElementType;
  }
}
