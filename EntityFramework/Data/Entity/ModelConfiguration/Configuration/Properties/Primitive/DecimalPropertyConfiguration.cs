// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DecimalPropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive
{
  internal class DecimalPropertyConfiguration : PrimitivePropertyConfiguration
  {
    public byte? Precision { get; set; }

    public byte? Scale { get; set; }

    public DecimalPropertyConfiguration()
    {
    }

    private DecimalPropertyConfiguration(DecimalPropertyConfiguration source)
      : base((PrimitivePropertyConfiguration) source)
    {
      this.Precision = source.Precision;
      this.Scale = source.Scale;
    }

    internal override PrimitivePropertyConfiguration Clone() => (PrimitivePropertyConfiguration) new DecimalPropertyConfiguration(this);

    protected override void ConfigureProperty(EdmProperty property)
    {
      base.ConfigureProperty(property);
      if (this.Precision.HasValue)
        property.Precision = this.Precision;
      if (!this.Scale.HasValue)
        return;
      property.Scale = this.Scale;
    }

    internal override void Configure(EdmProperty column, FacetDescription facetDescription)
    {
      base.Configure(column, facetDescription);
      switch (facetDescription.FacetName)
      {
        case "Precision":
          column.Precision = facetDescription.IsConstant ? new byte?() : this.Precision ?? column.Precision;
          break;
        case "Scale":
          column.Scale = facetDescription.IsConstant ? new byte?() : this.Scale ?? column.Scale;
          break;
      }
    }

    internal override void CopyFrom(PrimitivePropertyConfiguration other)
    {
      base.CopyFrom(other);
      if (!(other is DecimalPropertyConfiguration propertyConfiguration))
        return;
      this.Precision = propertyConfiguration.Precision;
      this.Scale = propertyConfiguration.Scale;
    }

    internal override void FillFrom(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.FillFrom(other, inCSpace);
      if (!(other is DecimalPropertyConfiguration propertyConfiguration))
        return;
      byte? nullable = this.Precision;
      if (!nullable.HasValue)
        this.Precision = propertyConfiguration.Precision;
      nullable = this.Scale;
      if (nullable.HasValue)
        return;
      this.Scale = propertyConfiguration.Scale;
    }

    internal override void MakeCompatibleWith(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.MakeCompatibleWith(other, inCSpace);
      if (!(other is DecimalPropertyConfiguration propertyConfiguration))
        return;
      byte? nullable = propertyConfiguration.Precision;
      if (nullable.HasValue)
      {
        nullable = new byte?();
        this.Precision = nullable;
      }
      nullable = propertyConfiguration.Scale;
      if (!nullable.HasValue)
        return;
      nullable = new byte?();
      this.Scale = nullable;
    }

    internal override bool IsCompatible(
      PrimitivePropertyConfiguration other,
      bool inCSpace,
      out string errorMessage)
    {
      DecimalPropertyConfiguration other1 = other as DecimalPropertyConfiguration;
      bool flag1 = base.IsCompatible(other, inCSpace, out errorMessage);
      int num1;
      if (other1 != null)
        num1 = this.IsCompatible<byte, DecimalPropertyConfiguration>((Expression<Func<DecimalPropertyConfiguration, byte?>>) (c => c.Precision), other1, ref errorMessage) ? 1 : 0;
      else
        num1 = 1;
      bool flag2 = num1 != 0;
      int num2;
      if (other1 != null)
        num2 = this.IsCompatible<byte, DecimalPropertyConfiguration>((Expression<Func<DecimalPropertyConfiguration, byte?>>) (c => c.Scale), other1, ref errorMessage) ? 1 : 0;
      else
        num2 = 1;
      bool flag3 = num2 != 0;
      return flag1 & flag2 & flag3;
    }
  }
}
