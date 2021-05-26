// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.StructuredProperty
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class StructuredProperty : Property
  {
    private SchemaType _type;
    private readonly TypeUsageBuilder _typeUsageBuilder;
    private CollectionKind _collectionKind;

    internal StructuredProperty(StructuredType parentElement)
      : base(parentElement)
    {
      this._typeUsageBuilder = new TypeUsageBuilder((SchemaElement) this);
    }

    public override SchemaType Type => this._type;

    public TypeUsage TypeUsage => this._typeUsageBuilder.TypeUsage;

    public bool Nullable => this._typeUsageBuilder.Nullable;

    public string Default => this._typeUsageBuilder.Default;

    public object DefaultAsObject => this._typeUsageBuilder.DefaultAsObject;

    public CollectionKind CollectionKind => this._collectionKind;

    internal override void ResolveTopLevelNames()
    {
      base.ResolveTopLevelNames();
      if (this._type != null)
        return;
      this._type = this.ResolveType(this.UnresolvedType);
      this._typeUsageBuilder.ValidateDefaultValue(this._type);
      if (!(this._type is ScalarType type))
        return;
      this._typeUsageBuilder.ValidateAndSetTypeUsage(type, true);
    }

    internal void EnsureEnumTypeFacets(
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      this._typeUsageBuilder.ValidateAndSetTypeUsage((EdmType) Converter.LoadSchemaElement(this.Type, this.Type.Schema.ProviderManifest, convertedItemCache, newGlobalItems), false);
    }

    protected virtual SchemaType ResolveType(string typeName)
    {
      SchemaType type;
      if (!this.Schema.ResolveTypeName((SchemaElement) this, typeName, out type))
        return (SchemaType) null;
      switch (type)
      {
        case SchemaComplexType _:
        case ScalarType _:
        case SchemaEnumType _:
          return type;
        default:
          this.AddError(ErrorCode.InvalidPropertyType, EdmSchemaErrorSeverity.Error, (object) Strings.InvalidPropertyType((object) this.UnresolvedType));
          return (SchemaType) null;
      }
    }

    internal string UnresolvedType { get; set; }

    internal override void Validate()
    {
      base.Validate();
      if (this._collectionKind != CollectionKind.Bag)
      {
        int collectionKind = (int) this._collectionKind;
      }
      if (this._type is SchemaEnumType type)
      {
        this._typeUsageBuilder.ValidateEnumFacets(type);
      }
      else
      {
        if (!this.Nullable || this.Schema.SchemaVersion == 1.1 || !(this._type is SchemaComplexType))
          return;
        this.AddError(ErrorCode.NullableComplexType, EdmSchemaErrorSeverity.Error, (object) Strings.ComplexObject_NullableComplexTypesNotSupported((object) this.FQName));
      }
    }

    protected override bool HandleAttribute(XmlReader reader)
    {
      if (base.HandleAttribute(reader))
        return true;
      if (SchemaElement.CanHandleAttribute(reader, "Type"))
      {
        this.HandleTypeAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "CollectionKind"))
      {
        this.HandleCollectionKindAttribute(reader);
        return true;
      }
      return this._typeUsageBuilder.HandleAttribute(reader);
    }

    private void HandleTypeAttribute(XmlReader reader)
    {
      if (this.UnresolvedType != null)
      {
        this.AddError(ErrorCode.AlreadyDefined, EdmSchemaErrorSeverity.Error, reader, (object) Strings.PropertyTypeAlreadyDefined((object) reader.Name));
      }
      else
      {
        string name;
        if (!Utils.GetDottedName(this.Schema, reader, out name))
          return;
        this.UnresolvedType = name;
      }
    }

    private void HandleCollectionKindAttribute(XmlReader reader)
    {
      string str = reader.Value;
      if (str == "None")
        this._collectionKind = CollectionKind.None;
      else if (str == "List")
      {
        this._collectionKind = CollectionKind.List;
      }
      else
      {
        if (!(str == "Bag"))
          return;
        this._collectionKind = CollectionKind.Bag;
      }
    }
  }
}
