﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.ReferenceTypeElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class ReferenceTypeElement : ModelFunctionTypeElement
  {
    internal ReferenceTypeElement(SchemaElement parentElement)
      : base(parentElement)
    {
    }

    protected override bool HandleAttribute(XmlReader reader)
    {
      if (base.HandleAttribute(reader))
        return true;
      if (!SchemaElement.CanHandleAttribute(reader, "Type"))
        return false;
      this.HandleTypeElementAttribute(reader);
      return true;
    }

    protected void HandleTypeElementAttribute(XmlReader reader)
    {
      string name;
      if (!Utils.GetString(this.Schema, reader, out name) || !Utils.ValidateDottedName(this.Schema, reader, name))
        return;
      this._unresolvedType = name;
    }

    internal override void WriteIdentity(StringBuilder builder) => builder.Append("Ref(" + this.UnresolvedType + ")");

    internal override TypeUsage GetTypeUsage() => this._typeUsage;

    internal override bool ResolveNameAndSetTypeUsage(
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      if (this._typeUsage == null)
      {
        RefType refType = new RefType((EdmType) Converter.LoadSchemaElement(this._type, this._type.Schema.ProviderManifest, convertedItemCache, newGlobalItems) as System.Data.Entity.Core.Metadata.Edm.EntityType);
        refType.AddMetadataProperties((IEnumerable<MetadataProperty>) this.OtherContent);
        this._typeUsage = TypeUsage.Create((EdmType) refType);
      }
      return true;
    }

    internal override void Validate()
    {
      base.Validate();
      ValidationHelper.ValidateRefType((SchemaElement) this, this._type);
    }
  }
}
