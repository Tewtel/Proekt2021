// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.GlobalItem
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Represents the base item class for all the metadata</summary>
  public abstract class GlobalItem : MetadataItem
  {
    internal GlobalItem()
    {
    }

    internal GlobalItem(MetadataItem.MetadataFlags flags)
      : base(flags)
    {
    }

    [MetadataProperty(typeof (DataSpace), false)]
    internal virtual DataSpace DataSpace
    {
      get => this.GetDataSpace();
      set => this.SetDataSpace(value);
    }
  }
}
