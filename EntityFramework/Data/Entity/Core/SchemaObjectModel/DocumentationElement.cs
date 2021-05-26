// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.DocumentationElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class DocumentationElement : SchemaElement
  {
    private readonly Documentation _metdataDocumentation = new Documentation();

    public DocumentationElement(SchemaElement parentElement)
      : base(parentElement)
    {
    }

    public Documentation MetadataDocumentation
    {
      get
      {
        this._metdataDocumentation.SetReadOnly();
        return this._metdataDocumentation;
      }
    }

    protected override bool HandleElement(XmlReader reader)
    {
      if (base.HandleElement(reader))
        return true;
      if (this.CanHandleElement(reader, "Summary"))
      {
        this.HandleSummaryElement(reader);
        return true;
      }
      if (!this.CanHandleElement(reader, "LongDescription"))
        return false;
      this.HandleLongDescriptionElement(reader);
      return true;
    }

    protected override bool HandleText(XmlReader reader)
    {
      if (!string.IsNullOrWhiteSpace(reader.Value))
        this.AddError(ErrorCode.UnexpectedXmlElement, EdmSchemaErrorSeverity.Error, (object) Strings.InvalidDocumentationBothTextAndStructure);
      return true;
    }

    private void HandleSummaryElement(XmlReader reader)
    {
      TextElement textElement = new TextElement((SchemaElement) this);
      textElement.Parse(reader);
      this._metdataDocumentation.Summary = textElement.Value;
    }

    private void HandleLongDescriptionElement(XmlReader reader)
    {
      TextElement textElement = new TextElement((SchemaElement) this);
      textElement.Parse(reader);
      this._metdataDocumentation.LongDescription = textElement.Value;
    }
  }
}
