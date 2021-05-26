// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.StringPropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive
{
  internal class StringPropertyConfiguration : LengthPropertyConfiguration
  {
    public bool? IsUnicode { get; set; }

    public StringPropertyConfiguration()
    {
    }

    private StringPropertyConfiguration(StringPropertyConfiguration source)
      : base((LengthPropertyConfiguration) source)
    {
      this.IsUnicode = source.IsUnicode;
    }

    internal override PrimitivePropertyConfiguration Clone() => (PrimitivePropertyConfiguration) new StringPropertyConfiguration(this);

    protected override void ConfigureProperty(EdmProperty property)
    {
      base.ConfigureProperty(property);
      if (!this.IsUnicode.HasValue)
        return;
      property.IsUnicode = this.IsUnicode;
    }

    internal override void Configure(EdmProperty column, FacetDescription facetDescription)
    {
      base.Configure(column, facetDescription);
      switch (facetDescription.FacetName)
      {
        case "Unicode":
          column.IsUnicode = facetDescription.IsConstant ? new bool?() : this.IsUnicode ?? column.IsUnicode;
          break;
      }
    }

    internal override void CopyFrom(PrimitivePropertyConfiguration other)
    {
      base.CopyFrom(other);
      if (!(other is StringPropertyConfiguration propertyConfiguration))
        return;
      this.IsUnicode = propertyConfiguration.IsUnicode;
    }

    internal override void FillFrom(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.FillFrom(other, inCSpace);
      if (!(other is StringPropertyConfiguration propertyConfiguration) || this.IsUnicode.HasValue)
        return;
      this.IsUnicode = propertyConfiguration.IsUnicode;
    }

    internal override void MakeCompatibleWith(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.MakeCompatibleWith(other, inCSpace);
      if (!(other is StringPropertyConfiguration propertyConfiguration))
        return;
      bool? nullable = propertyConfiguration.IsUnicode;
      if (!nullable.HasValue)
        return;
      nullable = new bool?();
      this.IsUnicode = nullable;
    }

    internal override bool IsCompatible(
      PrimitivePropertyConfiguration other,
      bool inCSpace,
      out string errorMessage)
    {
      StringPropertyConfiguration other1 = other as StringPropertyConfiguration;
      bool flag1 = base.IsCompatible(other, inCSpace, out errorMessage);
      int num;
      if (other1 != null)
        num = this.IsCompatible<bool, StringPropertyConfiguration>((Expression<Func<StringPropertyConfiguration, bool?>>) (c => c.IsUnicode), other1, ref errorMessage) ? 1 : 0;
      else
        num = 1;
      bool flag2 = num != 0;
      return flag1 & flag2;
    }
  }
}
