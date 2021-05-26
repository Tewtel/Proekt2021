// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.EntityContainerEntitySet
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class EntityContainerEntitySet : SchemaElement
  {
    private SchemaEntityType _entityType;
    private string _unresolvedEntityTypeName;
    private string _schema;
    private string _table;
    private EntityContainerEntitySetDefiningQuery _definingQueryElement;

    public EntityContainerEntitySet(EntityContainer parentElement)
      : base((SchemaElement) parentElement)
    {
    }

    public override string FQName => this.ParentElement.Name + "." + this.Name;

    public SchemaEntityType EntityType => this._entityType;

    public string DbSchema => this._schema;

    public string Table => this._table;

    public string DefiningQuery => this._definingQueryElement != null ? this._definingQueryElement.Query : (string) null;

    protected override bool HandleElement(XmlReader reader)
    {
      if (base.HandleElement(reader))
        return true;
      if (this.Schema.DataModel == SchemaDataModelOption.ProviderDataModel)
      {
        if (this.CanHandleElement(reader, "DefiningQuery"))
        {
          this.HandleDefiningQueryElement(reader);
          return true;
        }
      }
      else if (this.Schema.DataModel == SchemaDataModelOption.EntityDataModel)
      {
        if (this.CanHandleElement(reader, "ValueAnnotation"))
        {
          this.SkipElement(reader);
          return true;
        }
        if (this.CanHandleElement(reader, "TypeAnnotation"))
        {
          this.SkipElement(reader);
          return true;
        }
      }
      return false;
    }

    protected override bool HandleAttribute(XmlReader reader)
    {
      if (base.HandleAttribute(reader))
        return true;
      if (SchemaElement.CanHandleAttribute(reader, "EntityType"))
      {
        this.HandleEntityTypeAttribute(reader);
        return true;
      }
      if (this.Schema.DataModel == SchemaDataModelOption.ProviderDataModel)
      {
        if (SchemaElement.CanHandleAttribute(reader, "Schema"))
        {
          this.HandleDbSchemaAttribute(reader);
          return true;
        }
        if (SchemaElement.CanHandleAttribute(reader, "Table"))
        {
          this.HandleTableAttribute(reader);
          return true;
        }
      }
      return false;
    }

    private void HandleDefiningQueryElement(XmlReader reader)
    {
      EntityContainerEntitySetDefiningQuery setDefiningQuery = new EntityContainerEntitySetDefiningQuery(this);
      setDefiningQuery.Parse(reader);
      this._definingQueryElement = setDefiningQuery;
    }

    protected override void HandleNameAttribute(XmlReader reader)
    {
      if (this.Schema.DataModel == SchemaDataModelOption.ProviderDataModel)
        this.Name = reader.Value;
      else
        base.HandleNameAttribute(reader);
    }

    private void HandleEntityTypeAttribute(XmlReader reader)
    {
      ReturnValue<string> returnValue = this.HandleDottedNameAttribute(reader, this._unresolvedEntityTypeName);
      if (!returnValue.Succeeded)
        return;
      this._unresolvedEntityTypeName = returnValue.Value;
    }

    private void HandleDbSchemaAttribute(XmlReader reader) => this._schema = reader.Value;

    private void HandleTableAttribute(XmlReader reader) => this._table = reader.Value;

    internal override void ResolveTopLevelNames()
    {
      base.ResolveTopLevelNames();
      if (this._entityType != null)
        return;
      SchemaType type = (SchemaType) null;
      if (!this.Schema.ResolveTypeName((SchemaElement) this, this._unresolvedEntityTypeName, out type))
        return;
      this._entityType = type as SchemaEntityType;
      if (this._entityType != null)
        return;
      this.AddError(ErrorCode.InvalidPropertyType, EdmSchemaErrorSeverity.Error, (object) Strings.InvalidEntitySetType((object) this._unresolvedEntityTypeName));
    }

    internal override void Validate()
    {
      base.Validate();
      if (this._entityType.KeyProperties.Count == 0)
        this.AddError(ErrorCode.EntitySetTypeHasNoKeys, EdmSchemaErrorSeverity.Error, (object) Strings.EntitySetTypeHasNoKeys((object) this.Name, (object) this._entityType.FQName));
      if (this._definingQueryElement == null)
        return;
      this._definingQueryElement.Validate();
      if (this.DbSchema == null && this.Table == null)
        return;
      this.AddError(ErrorCode.TableAndSchemaAreMutuallyExclusiveWithDefiningQuery, EdmSchemaErrorSeverity.Error, (object) Strings.TableAndSchemaAreMutuallyExclusiveWithDefiningQuery((object) this.FQName));
    }

    internal override SchemaElement Clone(SchemaElement parentElement)
    {
      EntityContainerEntitySet containerEntitySet = new EntityContainerEntitySet((EntityContainer) parentElement);
      containerEntitySet._definingQueryElement = this._definingQueryElement;
      containerEntitySet._entityType = this._entityType;
      containerEntitySet._schema = this._schema;
      containerEntitySet._table = this._table;
      containerEntitySet.Name = this.Name;
      return (SchemaElement) containerEntitySet;
    }
  }
}
