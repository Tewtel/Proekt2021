// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.ByteFacetDescriptionElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class ByteFacetDescriptionElement : FacetDescriptionElement
  {
    public ByteFacetDescriptionElement(TypeElement type, string name)
      : base(type, name)
    {
    }

    public override EdmType FacetType => (EdmType) MetadataItem.EdmProviderManifest.GetPrimitiveType(PrimitiveTypeKind.Byte);

    protected override void HandleDefaultAttribute(XmlReader reader)
    {
      byte field = 0;
      if (!this.HandleByteAttribute(reader, ref field))
        return;
      this.DefaultValue = (object) field;
    }
  }
}
