// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.ModelFunction
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class ModelFunction : Function
  {
    private readonly TypeUsageBuilder _typeUsageBuilder;

    public ModelFunction(Schema parentElement)
      : base(parentElement)
    {
      this._isComposable = true;
      this._typeUsageBuilder = new TypeUsageBuilder((SchemaElement) this);
    }

    public override SchemaType Type => this._type;

    internal TypeUsage TypeUsage
    {
      get
      {
        if (this._typeUsageBuilder.TypeUsage == null)
          return (TypeUsage) null;
        return this.CollectionKind != CollectionKind.None ? TypeUsage.Create((EdmType) new CollectionType(this._typeUsageBuilder.TypeUsage)) : this._typeUsageBuilder.TypeUsage;
      }
    }

    internal void ValidateAndSetTypeUsage(ScalarType scalar) => this._typeUsageBuilder.ValidateAndSetTypeUsage(scalar, false);

    internal void ValidateAndSetTypeUsage(EdmType edmType) => this._typeUsageBuilder.ValidateAndSetTypeUsage(edmType, false);

    protected override bool HandleElement(XmlReader reader)
    {
      if (base.HandleElement(reader))
        return true;
      if (this.CanHandleElement(reader, "DefiningExpression"))
      {
        this.HandleDefiningExpressionElement(reader);
        return true;
      }
      if (!this.CanHandleElement(reader, "Parameter"))
        return false;
      this.HandleParameterElement(reader);
      return true;
    }

    protected override void HandleReturnTypeAttribute(XmlReader reader)
    {
      base.HandleReturnTypeAttribute(reader);
      this._isComposable = true;
    }

    protected override bool HandleAttribute(XmlReader reader) => base.HandleAttribute(reader) || this._typeUsageBuilder.HandleAttribute(reader);

    internal override void ResolveTopLevelNames()
    {
      if (this.UnresolvedReturnType != null && this.Schema.ResolveTypeName((SchemaElement) this, this.UnresolvedReturnType, out this._type) && this._type is ScalarType)
        this._typeUsageBuilder.ValidateAndSetTypeUsage(this._type as ScalarType, false);
      foreach (SchemaElement parameter in this.Parameters)
        parameter.ResolveTopLevelNames();
      if (this.ReturnTypeList == null)
        return;
      this.ReturnTypeList[0].ResolveTopLevelNames();
    }

    private void HandleDefiningExpressionElement(XmlReader reader)
    {
      FunctionCommandText functionCommandText = new FunctionCommandText((Function) this);
      functionCommandText.Parse(reader);
      this._commandText = functionCommandText;
    }

    internal override void Validate()
    {
      base.Validate();
      ValidationHelper.ValidateFacets((SchemaElement) this, this._type, this._typeUsageBuilder);
      if (!this._isRefType)
        return;
      ValidationHelper.ValidateRefType((SchemaElement) this, this._type);
    }
  }
}
