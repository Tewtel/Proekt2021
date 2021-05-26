// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.OriginalValueRecord
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects
{
  /// <summary>
  /// The original values of the properties of an entity when it was retrieved from the database.
  /// </summary>
  public abstract class OriginalValueRecord : DbUpdatableDataRecord
  {
    internal OriginalValueRecord(
      ObjectStateEntry cacheEntry,
      StateManagerTypeMetadata metadata,
      object userObject)
      : base(cacheEntry, metadata, userObject)
    {
    }
  }
}
