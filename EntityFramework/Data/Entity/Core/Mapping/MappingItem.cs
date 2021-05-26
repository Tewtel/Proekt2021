// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.MappingItem
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Base class for items in the mapping space (DataSpace.CSSpace)
  /// </summary>
  public abstract class MappingItem
  {
    private bool _readOnly;
    private readonly List<MetadataProperty> _annotations = new List<MetadataProperty>();

    internal bool IsReadOnly => this._readOnly;

    internal IList<MetadataProperty> Annotations => (IList<MetadataProperty>) this._annotations;

    internal virtual void SetReadOnly()
    {
      this._annotations.TrimExcess();
      this._readOnly = true;
    }

    internal void ThrowIfReadOnly()
    {
      if (this.IsReadOnly)
        throw new InvalidOperationException(Strings.OperationOnReadOnlyItem);
    }

    internal static void SetReadOnly(MappingItem item) => item?.SetReadOnly();

    internal static void SetReadOnly(IEnumerable<MappingItem> items)
    {
      if (items == null)
        return;
      foreach (MappingItem mappingItem in items)
        MappingItem.SetReadOnly(mappingItem);
    }
  }
}
