﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.IntegerFacetDescriptionElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class IntegerFacetDescriptionElement : FacetDescriptionElement
  {
    public IntegerFacetDescriptionElement(TypeElement type, string name)
      : base(type, name)
    {
    }

    public override EdmType FacetType => (EdmType) MetadataItem.EdmProviderManifest.GetPrimitiveType(PrimitiveTypeKind.Int32);

    protected override void HandleDefaultAttribute(XmlReader reader)
    {
      int field = -1;
      if (!this.HandleIntAttribute(reader, ref field))
        return;
      this.DefaultValue = (object) field;
    }
  }
}
