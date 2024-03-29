﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.Property
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal abstract class Property : SchemaElement
  {
    internal Property(StructuredType parentElement)
      : base((SchemaElement) parentElement)
    {
    }

    public abstract SchemaType Type { get; }

    protected override bool HandleElement(XmlReader reader)
    {
      if (base.HandleElement(reader))
        return true;
      if (this.Schema.DataModel == SchemaDataModelOption.EntityDataModel)
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
  }
}
