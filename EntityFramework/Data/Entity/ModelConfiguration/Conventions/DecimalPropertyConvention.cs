// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.DecimalPropertyConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to set precision to 18 and scale to 2 for decimal properties.
  /// </summary>
  public class DecimalPropertyConvention : IConceptualModelConvention<EdmProperty>, IConvention
  {
    private readonly byte _precision;
    private readonly byte _scale;

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.ModelConfiguration.Conventions.DecimalPropertyConvention" /> with the default precision and scale.
    /// </summary>
    public DecimalPropertyConvention()
      : this((byte) 18, (byte) 2)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.ModelConfiguration.Conventions.DecimalPropertyConvention" /> with the specified precision and scale.
    /// </summary>
    /// <param name="precision"> Precision </param>
    /// <param name="scale"> Scale </param>
    public DecimalPropertyConvention(byte precision, byte scale)
    {
      this._precision = precision;
      this._scale = scale;
    }

    /// <inheritdoc />
    public virtual void Apply(EdmProperty item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmProperty>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      if (item.PrimitiveType != PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Decimal))
        return;
      byte? nullable = item.Precision;
      if (!nullable.HasValue)
        item.Precision = new byte?(this._precision);
      nullable = item.Scale;
      if (nullable.HasValue)
        return;
      item.Scale = new byte?(this._scale);
    }
  }
}
