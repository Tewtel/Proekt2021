// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.LengthPropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive
{
  internal abstract class LengthPropertyConfiguration : PrimitivePropertyConfiguration
  {
    public bool? IsFixedLength { get; set; }

    public int? MaxLength { get; set; }

    public bool? IsMaxLength { get; set; }

    protected LengthPropertyConfiguration()
    {
    }

    protected LengthPropertyConfiguration(LengthPropertyConfiguration source)
      : base((PrimitivePropertyConfiguration) source)
    {
      System.Data.Entity.Utilities.Check.NotNull<LengthPropertyConfiguration>(source, nameof (source));
      this.IsFixedLength = source.IsFixedLength;
      this.MaxLength = source.MaxLength;
      this.IsMaxLength = source.IsMaxLength;
    }

    protected override void ConfigureProperty(EdmProperty property)
    {
      base.ConfigureProperty(property);
      if (this.IsFixedLength.HasValue)
        property.IsFixedLength = this.IsFixedLength;
      if (this.MaxLength.HasValue)
        property.MaxLength = this.MaxLength;
      if (!this.IsMaxLength.HasValue)
        return;
      property.IsMaxLength = this.IsMaxLength.Value;
    }

    internal override void Configure(EdmProperty column, FacetDescription facetDescription)
    {
      base.Configure(column, facetDescription);
      switch (facetDescription.FacetName)
      {
        case "FixedLength":
          column.IsFixedLength = facetDescription.IsConstant ? new bool?() : this.IsFixedLength ?? column.IsFixedLength;
          break;
        case "MaxLength":
          column.MaxLength = facetDescription.IsConstant ? new int?() : this.MaxLength ?? column.MaxLength;
          column.IsMaxLength = !facetDescription.IsConstant && ((int) this.IsMaxLength ?? (column.IsMaxLength ? 1 : 0)) != 0;
          break;
      }
    }

    internal override void CopyFrom(PrimitivePropertyConfiguration other)
    {
      base.CopyFrom(other);
      if (!(other is LengthPropertyConfiguration propertyConfiguration))
        return;
      this.IsFixedLength = propertyConfiguration.IsFixedLength;
      this.MaxLength = propertyConfiguration.MaxLength;
      this.IsMaxLength = propertyConfiguration.IsMaxLength;
    }

    internal override void FillFrom(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.FillFrom(other, inCSpace);
      if (!(other is LengthPropertyConfiguration propertyConfiguration))
        return;
      bool? nullable = this.IsFixedLength;
      if (!nullable.HasValue)
        this.IsFixedLength = propertyConfiguration.IsFixedLength;
      if (!this.MaxLength.HasValue)
        this.MaxLength = propertyConfiguration.MaxLength;
      nullable = this.IsMaxLength;
      if (nullable.HasValue)
        return;
      this.IsMaxLength = propertyConfiguration.IsMaxLength;
    }

    internal override void MakeCompatibleWith(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.MakeCompatibleWith(other, inCSpace);
      if (!(other is LengthPropertyConfiguration propertyConfiguration))
        return;
      bool? nullable1 = propertyConfiguration.IsFixedLength;
      if (nullable1.HasValue)
      {
        nullable1 = new bool?();
        this.IsFixedLength = nullable1;
      }
      int? nullable2 = propertyConfiguration.MaxLength;
      if (nullable2.HasValue)
      {
        nullable2 = new int?();
        this.MaxLength = nullable2;
      }
      nullable1 = propertyConfiguration.IsMaxLength;
      if (!nullable1.HasValue)
        return;
      nullable1 = new bool?();
      this.IsMaxLength = nullable1;
    }

    internal override bool IsCompatible(
      PrimitivePropertyConfiguration other,
      bool inCSpace,
      out string errorMessage)
    {
      LengthPropertyConfiguration other1 = other as LengthPropertyConfiguration;
      bool flag1 = base.IsCompatible(other, inCSpace, out errorMessage);
      int num1;
      if (other1 != null)
        num1 = this.IsCompatible<bool, LengthPropertyConfiguration>((Expression<Func<LengthPropertyConfiguration, bool?>>) (c => c.IsFixedLength), other1, ref errorMessage) ? 1 : 0;
      else
        num1 = 1;
      bool flag2 = num1 != 0;
      int num2;
      if (other1 != null)
        num2 = this.IsCompatible<bool, LengthPropertyConfiguration>((Expression<Func<LengthPropertyConfiguration, bool?>>) (c => c.IsMaxLength), other1, ref errorMessage) ? 1 : 0;
      else
        num2 = 1;
      bool flag3 = num2 != 0;
      int num3;
      if (other1 != null)
        num3 = this.IsCompatible<int, LengthPropertyConfiguration>((Expression<Func<LengthPropertyConfiguration, int?>>) (c => c.MaxLength), other1, ref errorMessage) ? 1 : 0;
      else
        num3 = 1;
      bool flag4 = num3 != 0;
      return flag1 & flag2 & flag3 & flag4;
    }
  }
}
