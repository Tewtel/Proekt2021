// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.PrimitiveSchema
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class PrimitiveSchema : Schema
  {
    public PrimitiveSchema(SchemaManager schemaManager)
      : base(schemaManager)
    {
      this.Schema = (Schema) this;
      DbProviderManifest providerManifest = this.ProviderManifest;
      if (providerManifest == null)
      {
        this.AddError(new EdmSchemaError(System.Data.Entity.Resources.Strings.FailedToRetrieveProviderManifest, 168, EdmSchemaErrorSeverity.Error));
      }
      else
      {
        IList<PrimitiveType> source = (IList<PrimitiveType>) providerManifest.GetStoreTypes();
        if (schemaManager.DataModel == SchemaDataModelOption.EntityDataModel && schemaManager.SchemaVersion < 3.0)
          source = (IList<PrimitiveType>) source.Where<PrimitiveType>((Func<PrimitiveType, bool>) (t => !Helper.IsSpatialType(t))).ToList<PrimitiveType>();
        foreach (PrimitiveType primitiveType in (IEnumerable<PrimitiveType>) source)
          this.TryAddType((SchemaType) new ScalarType((Schema) this, primitiveType.Name, primitiveType), false);
      }
    }

    internal override string Alias => this.ProviderManifest.NamespaceName;

    internal override string Namespace => this.ProviderManifest != null ? this.ProviderManifest.NamespaceName : string.Empty;

    protected override bool HandleAttribute(XmlReader reader) => false;
  }
}
