// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.MappingBase
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Represents the base item class for all the mapping metadata
  /// </summary>
  public abstract class MappingBase : GlobalItem
  {
    internal MappingBase()
      : base(MetadataItem.MetadataFlags.Readonly)
    {
    }

    internal MappingBase(MetadataItem.MetadataFlags flags)
      : base(flags)
    {
    }

    internal abstract MetadataItem EdmItem { get; }
  }
}
