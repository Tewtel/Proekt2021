// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.ReturnType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class ReturnType : ModelFunctionTypeElement
  {
    private CollectionKind _collectionKind;
    private bool _isRefType;
    private string _unresolvedEntitySet;
    private bool _entitySetPathDefined;
    private ModelFunctionTypeElement _typeSubElement;
    private EntityContainerEntitySet _entitySet;

    internal ReturnType(Function parentElement)
      : base((SchemaElement) parentElement)
    {
      this._typeUsageBuilder = new TypeUsageBuilder((SchemaElement) this);
    }

    internal bool IsRefType => this._isRefType;

    internal CollectionKind CollectionKind => this._collectionKind;

    internal EntityContainerEntitySet EntitySet => this._entitySet;

    internal bool EntitySetPathDefined => this._entitySetPathDefined;

    internal ModelFunctionTypeElement SubElement => this._typeSubElement;

    internal override TypeUsage TypeUsage
    {
      get
      {
        if (this._typeSubElement != null)
          return this._typeSubElement.GetTypeUsage();
        if (this._typeUsage != null)
          return this._typeUsage;
        if (base.TypeUsage == null)
          return (TypeUsage) null;
        return this._collectionKind != CollectionKind.None ? TypeUsage.Create((EdmType) new CollectionType(base.TypeUsage)) : base.TypeUsage;
      }
    }

    internal override SchemaElement Clone(SchemaElement parentElement)
    {
      ReturnType returnType = new ReturnType((Function) parentElement);
      returnType._type = this._type;
      returnType.Name = this.Name;
      returnType._typeUsageBuilder = this._typeUsageBuilder;
      returnType._unresolvedType = this._unresolvedType;
      returnType._unresolvedEntitySet = this._unresolvedEntitySet;
      returnType._entitySetPathDefined = this._entitySetPathDefined;
      returnType._entitySet = this._entitySet;
      return (SchemaElement) returnType;
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
      if (SchemaElement.CanHandleAttribute(reader, "EntitySet"))
      {
        this.HandleEntitySetAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "EntitySetPath"))
      {
        this.HandleEntitySetPathAttribute(reader);
        return true;
      }
      return this._typeUsageBuilder.HandleAttribute(reader);
    }

    internal bool ResolveNestedTypeNames(
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      return this._typeSubElement.ResolveNameAndSetTypeUsage(convertedItemCache, newGlobalItems);
    }

    private void HandleTypeAttribute(XmlReader reader)
    {
      string type;
      if (!Utils.GetString(this.Schema, reader, out type))
        return;
      TypeModifier typeModifier;
      Function.RemoveTypeModifier(ref type, out typeModifier, out this._isRefType);
      if (typeModifier == TypeModifier.Array)
        this._collectionKind = CollectionKind.Bag;
      if (!Utils.ValidateDottedName(this.Schema, reader, type))
        return;
      this.UnresolvedType = type;
    }

    private void HandleEntitySetAttribute(XmlReader reader)
    {
      string str;
      if (!Utils.GetString(this.Schema, reader, out str))
        return;
      this._unresolvedEntitySet = str;
    }

    private void HandleEntitySetPathAttribute(XmlReader reader)
    {
      if (!Utils.GetString(this.Schema, reader, out string _))
        return;
      this._entitySetPathDefined = true;
    }

    protected override bool HandleElement(XmlReader reader)
    {
      if (base.HandleElement(reader))
        return true;
      if (this.CanHandleElement(reader, "CollectionType"))
      {
        this.HandleCollectionTypeElement(reader);
        return true;
      }
      if (this.CanHandleElement(reader, "ReferenceType"))
      {
        this.HandleReferenceTypeElement(reader);
        return true;
      }
      if (this.CanHandleElement(reader, "TypeRef"))
      {
        this.HandleTypeRefElement(reader);
        return true;
      }
      if (!this.CanHandleElement(reader, "RowType"))
        return false;
      this.HandleRowTypeElement(reader);
      return true;
    }

    protected void HandleCollectionTypeElement(XmlReader reader)
    {
      CollectionTypeElement collectionTypeElement = new CollectionTypeElement((SchemaElement) this);
      collectionTypeElement.Parse(reader);
      this._typeSubElement = (ModelFunctionTypeElement) collectionTypeElement;
    }

    protected void HandleReferenceTypeElement(XmlReader reader)
    {
      ReferenceTypeElement referenceTypeElement = new ReferenceTypeElement((SchemaElement) this);
      referenceTypeElement.Parse(reader);
      this._typeSubElement = (ModelFunctionTypeElement) referenceTypeElement;
    }

    protected void HandleTypeRefElement(XmlReader reader)
    {
      TypeRefElement typeRefElement = new TypeRefElement((SchemaElement) this);
      typeRefElement.Parse(reader);
      this._typeSubElement = (ModelFunctionTypeElement) typeRefElement;
    }

    protected void HandleRowTypeElement(XmlReader reader)
    {
      RowTypeElement rowTypeElement = new RowTypeElement((SchemaElement) this);
      rowTypeElement.Parse(reader);
      this._typeSubElement = (ModelFunctionTypeElement) rowTypeElement;
    }

    internal override void ResolveTopLevelNames()
    {
      if (this._unresolvedType != null)
        base.ResolveTopLevelNames();
      if (this._typeSubElement != null)
        this._typeSubElement.ResolveTopLevelNames();
      if (!this.ParentElement.IsFunctionImport || this._unresolvedEntitySet == null)
        return;
      ((FunctionImportElement) this.ParentElement).ResolveEntitySet((SchemaElement) this, this._unresolvedEntitySet, ref this._entitySet);
    }

    internal override void Validate()
    {
      base.Validate();
      ValidationHelper.ValidateTypeDeclaration((SchemaElement) this, this._type, (SchemaElement) this._typeSubElement);
      ValidationHelper.ValidateFacets((SchemaElement) this, this._type, this._typeUsageBuilder);
      if (this._isRefType)
        ValidationHelper.ValidateRefType((SchemaElement) this, this._type);
      if (this.Schema.DataModel != SchemaDataModelOption.EntityDataModel)
      {
        if (this.Schema.DataModel == SchemaDataModelOption.ProviderManifestModel)
        {
          if (this._type != null && (!(this._type is ScalarType) || this._collectionKind != CollectionKind.None) || this._typeSubElement != null && !(this._typeSubElement.Type is ScalarType))
          {
            string str = "";
            if (this._type != null)
              str = Function.GetTypeNameForErrorMessage(this._type, this._collectionKind, this._isRefType);
            else if (this._typeSubElement != null)
              str = this._typeSubElement.FQName;
            this.AddError(ErrorCode.FunctionWithNonEdmTypeNotSupported, EdmSchemaErrorSeverity.Error, (SchemaElement) this, (object) System.Data.Entity.Resources.Strings.FunctionWithNonEdmPrimitiveTypeNotSupported((object) str, (object) this.ParentElement.FQName));
          }
        }
        else if (this._type != null)
        {
          if (!(this._type is ScalarType) || this._collectionKind != CollectionKind.None)
            this.AddError(ErrorCode.FunctionWithNonPrimitiveTypeNotSupported, EdmSchemaErrorSeverity.Error, (SchemaElement) this, (object) System.Data.Entity.Resources.Strings.FunctionWithNonPrimitiveTypeNotSupported(this._isRefType ? (object) this._unresolvedType : (object) this._type.FQName, (object) this.ParentElement.FQName));
        }
        else if (this._typeSubElement != null && !(this._typeSubElement.Type is ScalarType))
        {
          if (this.Schema.SchemaVersion < 3.0)
            this.AddError(ErrorCode.FunctionWithNonPrimitiveTypeNotSupported, EdmSchemaErrorSeverity.Error, (SchemaElement) this, (object) System.Data.Entity.Resources.Strings.FunctionWithNonPrimitiveTypeNotSupported((object) this._typeSubElement.FQName, (object) this.ParentElement.FQName));
          else if (this._typeSubElement is CollectionTypeElement typeSubElement6 && typeSubElement6.SubElement is RowTypeElement subElement6 && subElement6.Properties.Any<RowTypePropertyElement>((Func<RowTypePropertyElement, bool>) (p => !p.ValidateIsScalar())))
            this.AddError(ErrorCode.TVFReturnTypeRowHasNonScalarProperty, EdmSchemaErrorSeverity.Error, (SchemaElement) this, (object) System.Data.Entity.Resources.Strings.TVFReturnTypeRowHasNonScalarProperty);
        }
      }
      if (this._typeSubElement == null)
        return;
      this._typeSubElement.Validate();
    }

    internal override void WriteIdentity(StringBuilder builder)
    {
    }

    internal override TypeUsage GetTypeUsage() => this.TypeUsage;

    internal override bool ResolveNameAndSetTypeUsage(
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      return false;
    }
  }
}
