// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.FacetEnabledSchemaElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal abstract class FacetEnabledSchemaElement : SchemaElement
  {
    protected SchemaType _type;
    protected string _unresolvedType;
    protected TypeUsageBuilder _typeUsageBuilder;

    internal Function ParentElement => base.ParentElement as Function;

    internal SchemaType Type => this._type;

    internal virtual TypeUsage TypeUsage => this._typeUsageBuilder.TypeUsage;

    internal TypeUsageBuilder TypeUsageBuilder => this._typeUsageBuilder;

    internal bool HasUserDefinedFacets => this._typeUsageBuilder.HasUserDefinedFacets;

    internal string UnresolvedType
    {
      get => this._unresolvedType;
      set => this._unresolvedType = value;
    }

    internal FacetEnabledSchemaElement(Function parentElement)
      : base((SchemaElement) parentElement)
    {
    }

    internal FacetEnabledSchemaElement(SchemaElement parentElement)
      : base(parentElement)
    {
    }

    internal override void ResolveTopLevelNames()
    {
      base.ResolveTopLevelNames();
      if (!this.Schema.ResolveTypeName((SchemaElement) this, this.UnresolvedType, out this._type) || this.Schema.DataModel != SchemaDataModelOption.ProviderManifestModel || !this._typeUsageBuilder.HasUserDefinedFacets)
        return;
      this._typeUsageBuilder.ValidateAndSetTypeUsage((ScalarType) this._type, this.Schema.DataModel != SchemaDataModelOption.ProviderManifestModel);
    }

    internal void ValidateAndSetTypeUsage(ScalarType scalar) => this._typeUsageBuilder.ValidateAndSetTypeUsage(scalar, false);

    internal void ValidateAndSetTypeUsage(EdmType edmType) => this._typeUsageBuilder.ValidateAndSetTypeUsage(edmType, false);

    protected override bool HandleAttribute(XmlReader reader) => base.HandleAttribute(reader) || this._typeUsageBuilder.HandleAttribute(reader);
  }
}
