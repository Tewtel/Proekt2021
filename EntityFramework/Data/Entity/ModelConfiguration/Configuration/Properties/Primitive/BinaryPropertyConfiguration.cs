// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.BinaryPropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive
{
  internal class BinaryPropertyConfiguration : LengthPropertyConfiguration
  {
    public bool? IsRowVersion { get; set; }

    public BinaryPropertyConfiguration()
    {
    }

    private BinaryPropertyConfiguration(BinaryPropertyConfiguration source)
      : base((LengthPropertyConfiguration) source)
    {
      this.IsRowVersion = source.IsRowVersion;
    }

    internal override PrimitivePropertyConfiguration Clone() => (PrimitivePropertyConfiguration) new BinaryPropertyConfiguration(this);

    protected override void ConfigureProperty(EdmProperty property)
    {
      if (this.IsRowVersion.HasValue && this.IsRowVersion.Value)
      {
        this.ConcurrencyMode = new ConcurrencyMode?((ConcurrencyMode) ((int) this.ConcurrencyMode ?? 1));
        this.DatabaseGeneratedOption = new DatabaseGeneratedOption?((DatabaseGeneratedOption) ((int) this.DatabaseGeneratedOption ?? 2));
        this.IsNullable = new bool?(this.IsNullable.GetValueOrDefault());
        this.MaxLength = new int?(this.MaxLength ?? 8);
      }
      base.ConfigureProperty(property);
    }

    protected override void ConfigureColumn(
      EdmProperty column,
      EntityType table,
      DbProviderManifest providerManifest)
    {
      if (this.IsRowVersion.HasValue && this.IsRowVersion.Value)
        this.ColumnType = this.ColumnType ?? "rowversion";
      base.ConfigureColumn(column, table, providerManifest);
      bool? isRowVersion = this.IsRowVersion;
      if (!isRowVersion.HasValue)
        return;
      isRowVersion = this.IsRowVersion;
      if (!isRowVersion.Value)
        return;
      column.MaxLength = new int?();
    }

    internal override void CopyFrom(PrimitivePropertyConfiguration other)
    {
      base.CopyFrom(other);
      if (!(other is BinaryPropertyConfiguration propertyConfiguration))
        return;
      this.IsRowVersion = propertyConfiguration.IsRowVersion;
    }

    internal override void FillFrom(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.FillFrom(other, inCSpace);
      if (!(other is BinaryPropertyConfiguration propertyConfiguration) || this.IsRowVersion.HasValue)
        return;
      this.IsRowVersion = propertyConfiguration.IsRowVersion;
    }

    internal override void MakeCompatibleWith(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      base.MakeCompatibleWith(other, inCSpace);
      if (!(other is BinaryPropertyConfiguration propertyConfiguration))
        return;
      bool? nullable = propertyConfiguration.IsRowVersion;
      if (!nullable.HasValue)
        return;
      nullable = new bool?();
      this.IsRowVersion = nullable;
    }

    internal override bool IsCompatible(
      PrimitivePropertyConfiguration other,
      bool inCSpace,
      out string errorMessage)
    {
      BinaryPropertyConfiguration other1 = other as BinaryPropertyConfiguration;
      bool flag1 = base.IsCompatible(other, inCSpace, out errorMessage);
      int num;
      if (other1 != null)
        num = this.IsCompatible<bool, BinaryPropertyConfiguration>((Expression<Func<BinaryPropertyConfiguration, bool?>>) (c => c.IsRowVersion), other1, ref errorMessage) ? 1 : 0;
      else
        num = 1;
      bool flag2 = num != 0;
      return flag1 & flag2;
    }
  }
}
