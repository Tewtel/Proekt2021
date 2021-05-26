// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.OnOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class OnOperation : SchemaElement
  {
    public OnOperation(RelationshipEnd parentElement, Operation operation)
      : base((SchemaElement) parentElement)
    {
      this.Operation = operation;
    }

    public Operation Operation { get; private set; }

    public Action Action { get; private set; }

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
      if (!SchemaElement.CanHandleAttribute(reader, "Action"))
        return false;
      this.HandleActionAttribute(reader);
      return true;
    }

    private void HandleActionAttribute(XmlReader reader)
    {
      switch (reader.Value.Trim())
      {
        case "None":
          this.Action = Action.None;
          break;
        case "Cascade":
          this.Action = Action.Cascade;
          break;
        default:
          this.AddError(ErrorCode.InvalidAction, EdmSchemaErrorSeverity.Error, reader, (object) Strings.InvalidAction((object) reader.Value, (object) this.ParentElement.FQName));
          break;
      }
    }

    private RelationshipEnd ParentElement => (RelationshipEnd) base.ParentElement;
  }
}
