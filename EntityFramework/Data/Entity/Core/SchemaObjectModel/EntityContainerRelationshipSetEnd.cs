// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.EntityContainerRelationshipSetEnd
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class EntityContainerRelationshipSetEnd : SchemaElement
  {
    private IRelationshipEnd _relationshipEnd;
    private string _unresolvedEntitySetName;
    private EntityContainerEntitySet _entitySet;

    public EntityContainerRelationshipSetEnd(EntityContainerRelationshipSet parentElement)
      : base((SchemaElement) parentElement)
    {
    }

    public IRelationshipEnd RelationshipEnd
    {
      get => this._relationshipEnd;
      internal set => this._relationshipEnd = value;
    }

    public EntityContainerEntitySet EntitySet
    {
      get => this._entitySet;
      internal set => this._entitySet = value;
    }

    protected override bool ProhibitAttribute(string namespaceUri, string localName)
    {
      if (base.ProhibitAttribute(namespaceUri, localName))
        return true;
      if (namespaceUri != null)
        return false;
      int num = localName == "Name" ? 1 : 0;
      return false;
    }

    protected override bool HandleAttribute(XmlReader reader)
    {
      if (base.HandleAttribute(reader))
        return true;
      if (!SchemaElement.CanHandleAttribute(reader, "EntitySet"))
        return false;
      this.HandleEntitySetAttribute(reader);
      return true;
    }

    private void HandleEntitySetAttribute(XmlReader reader)
    {
      if (this.Schema.DataModel == SchemaDataModelOption.ProviderDataModel)
        this._unresolvedEntitySetName = reader.Value;
      else
        this._unresolvedEntitySetName = this.HandleUndottedNameAttribute(reader, this._unresolvedEntitySetName);
    }

    internal override void ResolveTopLevelNames()
    {
      base.ResolveTopLevelNames();
      if (this._entitySet != null)
        return;
      this._entitySet = this.ParentElement.ParentElement.FindEntitySet(this._unresolvedEntitySetName);
      if (this._entitySet != null)
        return;
      this.AddError(ErrorCode.InvalidEndEntitySet, EdmSchemaErrorSeverity.Error, (object) Strings.InvalidEntitySetNameReference((object) this._unresolvedEntitySetName, (object) this.Name));
    }

    internal override void Validate()
    {
      base.Validate();
      if (this._relationshipEnd == null || this._entitySet == null || (this._relationshipEnd.Type.IsOfType((StructuredType) this._entitySet.EntityType) || this._entitySet.EntityType.IsOfType((StructuredType) this._relationshipEnd.Type)))
        return;
      this.AddError(ErrorCode.InvalidEndEntitySet, EdmSchemaErrorSeverity.Error, (object) Strings.InvalidEndEntitySetTypeMismatch((object) this._relationshipEnd.Name));
    }

    internal EntityContainerRelationshipSet ParentElement => (EntityContainerRelationshipSet) base.ParentElement;
  }
}
