// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.FunctionImportElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class FunctionImportElement : Function
  {
    private string _unresolvedEntitySet;
    private bool _entitySetPathDefined;
    private EntityContainer _container;
    private EntityContainerEntitySet _entitySet;
    private bool? _isSideEffecting;

    internal FunctionImportElement(EntityContainer container)
      : base(container.Schema)
    {
      if (this.Schema.DataModel == SchemaDataModelOption.EntityDataModel)
        this.OtherContent.Add(this.Schema.SchemaSource);
      this._container = container;
      this._isComposable = false;
    }

    public override bool IsFunctionImport => true;

    public override string FQName => this._container.Name + "." + this.Name;

    public override string Identity => this.Name;

    public EntityContainer Container => this._container;

    public EntityContainerEntitySet EntitySet => this._entitySet;

    protected override bool HandleAttribute(XmlReader reader)
    {
      if (base.HandleAttribute(reader))
        return true;
      if (SchemaElement.CanHandleAttribute(reader, "EntitySet"))
      {
        string str;
        if (Utils.GetString(this.Schema, reader, out str))
          this._unresolvedEntitySet = str;
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "EntitySetPath"))
      {
        if (Utils.GetString(this.Schema, reader, out string _))
          this._entitySetPathDefined = true;
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "IsBindable"))
        return true;
      if (!SchemaElement.CanHandleAttribute(reader, "IsSideEffecting"))
        return false;
      bool field = true;
      if (this.HandleBoolAttribute(reader, ref field))
        this._isSideEffecting = new bool?(field);
      return true;
    }

    internal override void ResolveTopLevelNames()
    {
      base.ResolveTopLevelNames();
      this.ResolveEntitySet((SchemaElement) this, this._unresolvedEntitySet, ref this._entitySet);
    }

    internal void ResolveEntitySet(
      SchemaElement owner,
      string unresolvedEntitySet,
      ref EntityContainerEntitySet entitySet)
    {
      if (entitySet != null || unresolvedEntitySet == null)
        return;
      entitySet = this._container.FindEntitySet(unresolvedEntitySet);
      if (entitySet != null)
        return;
      owner.AddError(ErrorCode.FunctionImportUnknownEntitySet, EdmSchemaErrorSeverity.Error, (object) Strings.FunctionImportUnknownEntitySet((object) unresolvedEntitySet, (object) this.FQName));
    }

    internal override void Validate()
    {
      base.Validate();
      this.ValidateFunctionImportReturnType((SchemaElement) this, this._type, this.CollectionKind, this._entitySet, this._entitySetPathDefined);
      if (this._returnTypeList != null)
      {
        foreach (ReturnType returnType in this._returnTypeList)
          this.ValidateFunctionImportReturnType((SchemaElement) returnType, returnType.Type, returnType.CollectionKind, returnType.EntitySet, returnType.EntitySetPathDefined);
      }
      if (this._isComposable && this._isSideEffecting.HasValue && this._isSideEffecting.Value)
        this.AddError(ErrorCode.FunctionImportComposableAndSideEffectingNotAllowed, EdmSchemaErrorSeverity.Error, (object) Strings.FunctionImportComposableAndSideEffectingNotAllowed((object) this.FQName));
      if (this._parameters == null)
        return;
      foreach (Parameter parameter in this._parameters)
      {
        if (parameter.IsRefType || parameter.CollectionKind != CollectionKind.None)
          this.AddError(ErrorCode.FunctionImportCollectionAndRefParametersNotAllowed, EdmSchemaErrorSeverity.Error, (object) Strings.FunctionImportCollectionAndRefParametersNotAllowed((object) this.FQName));
        if (!parameter.TypeUsageBuilder.Nullable)
          this.AddError(ErrorCode.FunctionImportNonNullableParametersNotAllowed, EdmSchemaErrorSeverity.Error, (object) Strings.FunctionImportNonNullableParametersNotAllowed((object) this.FQName));
      }
    }

    private void ValidateFunctionImportReturnType(
      SchemaElement owner,
      SchemaType returnType,
      CollectionKind returnTypeCollectionKind,
      EntityContainerEntitySet entitySet,
      bool entitySetPathDefined)
    {
      if (returnType != null && !this.ReturnTypeMeetsFunctionImportBasicRequirements(returnType, returnTypeCollectionKind))
        owner.AddError(ErrorCode.FunctionImportUnsupportedReturnType, EdmSchemaErrorSeverity.Error, owner, (object) this.GetReturnTypeErrorMessage(this.Name));
      this.ValidateFunctionImportReturnType(owner, returnType, entitySet, entitySetPathDefined);
    }

    private bool ReturnTypeMeetsFunctionImportBasicRequirements(
      SchemaType type,
      CollectionKind returnTypeCollectionKind)
    {
      return type is ScalarType && returnTypeCollectionKind == CollectionKind.Bag || type is SchemaEntityType && returnTypeCollectionKind == CollectionKind.Bag || this.Schema.SchemaVersion == 1.1 && (type is ScalarType && returnTypeCollectionKind == CollectionKind.None || type is SchemaEntityType && returnTypeCollectionKind == CollectionKind.None || (type is SchemaComplexType && returnTypeCollectionKind == CollectionKind.None || type is SchemaComplexType && returnTypeCollectionKind == CollectionKind.Bag)) || (this.Schema.SchemaVersion >= 2.0 && type is SchemaComplexType && returnTypeCollectionKind == CollectionKind.Bag || this.Schema.SchemaVersion >= 3.0 && type is SchemaEnumType && returnTypeCollectionKind == CollectionKind.Bag);
    }

    private void ValidateFunctionImportReturnType(
      SchemaElement owner,
      SchemaType returnType,
      EntityContainerEntitySet entitySet,
      bool entitySetPathDefined)
    {
      SchemaEntityType schemaEntityType = returnType as SchemaEntityType;
      if (entitySet != null & entitySetPathDefined)
        owner.AddError(ErrorCode.FunctionImportEntitySetAndEntitySetPathDeclared, EdmSchemaErrorSeverity.Error, (object) Strings.FunctionImportEntitySetAndEntitySetPathDeclared((object) this.FQName));
      if (schemaEntityType != null)
      {
        if (entitySet == null)
        {
          owner.AddError(ErrorCode.FunctionImportReturnsEntitiesButDoesNotSpecifyEntitySet, EdmSchemaErrorSeverity.Error, (object) Strings.FunctionImportReturnEntitiesButDoesNotSpecifyEntitySet((object) this.FQName));
        }
        else
        {
          if (entitySet.EntityType == null || schemaEntityType.IsOfType((StructuredType) entitySet.EntityType))
            return;
          owner.AddError(ErrorCode.FunctionImportEntityTypeDoesNotMatchEntitySet, EdmSchemaErrorSeverity.Error, (object) Strings.FunctionImportEntityTypeDoesNotMatchEntitySet((object) this.FQName, (object) entitySet.EntityType.FQName, (object) entitySet.Name));
        }
      }
      else if (returnType is SchemaComplexType schemaComplexType2)
      {
        if (!(entitySet != null | entitySetPathDefined))
          return;
        owner.AddError(ErrorCode.ComplexTypeAsReturnTypeAndDefinedEntitySet, EdmSchemaErrorSeverity.Error, owner.LineNumber, owner.LinePosition, (object) Strings.ComplexTypeAsReturnTypeAndDefinedEntitySet((object) this.FQName, (object) schemaComplexType2.Name));
      }
      else
      {
        if (!(entitySet != null | entitySetPathDefined))
          return;
        owner.AddError(ErrorCode.FunctionImportSpecifiesEntitySetButDoesNotReturnEntityType, EdmSchemaErrorSeverity.Error, (object) Strings.FunctionImportSpecifiesEntitySetButNotEntityType((object) this.FQName));
      }
    }

    private string GetReturnTypeErrorMessage(string functionName) => this.Schema.SchemaVersion != 1.0 ? (this.Schema.SchemaVersion != 1.1 ? Strings.FunctionImportWithUnsupportedReturnTypeV2((object) functionName) : Strings.FunctionImportWithUnsupportedReturnTypeV1_1((object) functionName)) : Strings.FunctionImportWithUnsupportedReturnTypeV1((object) functionName);

    internal override SchemaElement Clone(SchemaElement parentElement)
    {
      FunctionImportElement functionImportElement = new FunctionImportElement((EntityContainer) parentElement);
      this.CloneSetFunctionFields((Function) functionImportElement);
      functionImportElement._container = this._container;
      functionImportElement._entitySet = this._entitySet;
      functionImportElement._unresolvedEntitySet = this._unresolvedEntitySet;
      functionImportElement._entitySetPathDefined = this._entitySetPathDefined;
      return (SchemaElement) functionImportElement;
    }
  }
}
