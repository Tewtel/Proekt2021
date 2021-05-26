// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.TextElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class TextElement : SchemaElement
  {
    public TextElement(SchemaElement parentElement)
      : base(parentElement)
    {
    }

    public string Value { get; private set; }

    protected override bool HandleText(XmlReader reader)
    {
      this.TextElementTextHandler(reader);
      return true;
    }

    private void TextElementTextHandler(XmlReader reader)
    {
      string str = reader.Value;
      if (string.IsNullOrEmpty(str))
        return;
      if (string.IsNullOrEmpty(this.Value))
        this.Value = str;
      else
        this.Value += str;
    }
  }
}
