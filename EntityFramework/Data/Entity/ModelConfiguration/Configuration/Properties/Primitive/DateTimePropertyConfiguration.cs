// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive
{
  internal class DateTimePropertyConfiguration : PrimitivePropertyConfiguration
  {
    public byte? Precision { get; set; }

    public DateTimePropertyConfiguration()
    {
    }

    private DateTimePropertyConfiguration(DateTimePropertyConfiguration source)
      : base((PrimitivePropertyConfiguration) source)
    {
      this.Precision = source.Precision;
    }

    internal override PrimitivePropertyConfiguration Clone() => (PrimitivePropertyConfiguration) new DateTimePropertyConfiguration(this);

    protected override void ConfigureProperty(EdmProperty property)
    {
      base.ConfigureProperty(property);
      if (!this.Precision.HasValue)
        return;
      property.Precision = this.Precision;
    }

    internal override void Configure(EdmProperty column, FacetDescription facetDescription)
    {
      base.Configure(column, facetDescription);
      switch (facetDescription.FacetName)
      {
        case "Precision":
          column.Precision = facetDescription.IsConstant ? new byte?() : this.Precision ?? column.Precision;
          break;
      }
    }

    internal override void CopyFrom(PrimitivePropertyConfiguration other)
    {
      base.CopyFrom(other);
      if (!(other is DateTimePropertyConfiguration propertyConfiguration))
        return;
      this.Precision = propertyConfiguration.Precision;
    }

    internal override void FillFrom(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.FillFrom(other, inCSpace);
      if (!(other is DateTimePropertyConfiguration propertyConfiguration) || this.Precision.HasValue)
        return;
      this.Precision = propertyConfiguration.Precision;
    }

    internal override void MakeCompatibleWith(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.MakeCompatibleWith(other, inCSpace);
      if (!(other is DateTimePropertyConfiguration propertyConfiguration))
        return;
      byte? nullable = propertyConfiguration.Precision;
      if (!nullable.HasValue)
        return;
      nullable = new byte?();
      this.Precision = nullable;
    }

    internal override bool IsCompatible(
      PrimitivePropertyConfiguration other,
      bool inCSpace,
      out string errorMessage)
    {
      DateTimePropertyConfiguration other1 = other as DateTimePropertyConfiguration;
      bool flag1 = base.IsCompatible(other, inCSpace, out errorMessage);
      int num;
      if (other1 != null)
        num = this.IsCompatible<byte, DateTimePropertyConfiguration>((Expression<Func<DateTimePropertyConfiguration, byte?>>) (c => c.Precision), other1, ref errorMessage) ? 1 : 0;
      else
        num = 1;
      bool flag2 = num != 0;
      return flag1 & flag2;
    }
  }
}
