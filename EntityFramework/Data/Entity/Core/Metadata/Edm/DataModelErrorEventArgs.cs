// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.DataModelErrorEventArgs
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Information about an error that occurred processing an Entity Framework model.
  /// </summary>
  [Serializable]
  public class DataModelErrorEventArgs : EventArgs
  {
    [NonSerialized]
    private MetadataItem _item;

    /// <summary>
    /// Gets an optional value indicating which property of the source item caused the event to be raised.
    /// </summary>
    public string PropertyName { get; internal set; }

    /// <summary>
    /// Gets an optional descriptive message the describes the error that is being raised.
    /// </summary>
    public string ErrorMessage { get; internal set; }

    /// <summary>
    /// Gets a value indicating the <see cref="T:System.Data.Entity.Core.Metadata.Edm.MetadataItem" /> that caused the event to be raised.
    /// </summary>
    public MetadataItem Item
    {
      get => this._item;
      set => this._item = value;
    }
  }
}
