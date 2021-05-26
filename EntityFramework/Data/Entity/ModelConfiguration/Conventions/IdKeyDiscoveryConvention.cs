// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.IdKeyDiscoveryConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to detect primary key properties.
  /// Recognized naming patterns in order of precedence are:
  /// 1. 'Id'
  /// 2. [type name]Id
  /// Primary key detection is case insensitive.
  /// </summary>
  public class IdKeyDiscoveryConvention : KeyDiscoveryConvention
  {
    private const string Id = "Id";

    /// <inheritdoc />
    protected override IEnumerable<EdmProperty> MatchKeyProperty(
      EntityType entityType,
      IEnumerable<EdmProperty> primitiveProperties)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(entityType, nameof (entityType));
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<EdmProperty>>(primitiveProperties, nameof (primitiveProperties));
      IEnumerable<EdmProperty> source = primitiveProperties.Where<EdmProperty>((Func<EdmProperty, bool>) (p => "Id".Equals(p.Name, StringComparison.OrdinalIgnoreCase)));
      if (!source.Any<EdmProperty>())
        source = primitiveProperties.Where<EdmProperty>((Func<EdmProperty, bool>) (p => (entityType.Name + "Id").Equals(p.Name, StringComparison.OrdinalIgnoreCase)));
      return source.Count<EdmProperty>() <= 1 ? source : throw System.Data.Entity.Resources.Error.MultiplePropertiesMatchedAsKeys((object) source.First<EdmProperty>().Name, (object) entityType.Name);
    }
  }
}
