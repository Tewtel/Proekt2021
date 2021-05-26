// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.UsingElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class UsingElement : SchemaElement
  {
    internal UsingElement(Schema parentElement)
      : base((SchemaElement) parentElement)
    {
    }

    public virtual string Alias { get; private set; }

    public virtual string NamespaceName { get; private set; }

    public override string FQName => (string) null;

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
      if (SchemaElement.CanHandleAttribute(reader, "Namespace"))
      {
        this.HandleNamespaceAttribute(reader);
        return true;
      }
      if (!SchemaElement.CanHandleAttribute(reader, "Alias"))
        return false;
      this.HandleAliasAttribute(reader);
      return true;
    }

    private void HandleNamespaceAttribute(XmlReader reader)
    {
      ReturnValue<string> returnValue = this.HandleDottedNameAttribute(reader, this.NamespaceName);
      if (!returnValue.Succeeded)
        return;
      this.NamespaceName = returnValue.Value;
    }

    private void HandleAliasAttribute(XmlReader reader) => this.Alias = this.HandleUndottedNameAttribute(reader, this.Alias);
  }
}
