// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.RefreshMode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects
{
  /// <summary>
  /// Defines the different ways to handle modified properties when refreshing in-memory data from the database.
  /// </summary>
  public enum RefreshMode
  {
    /// <summary>
    /// Discard all changes on the client and refresh values with store values.
    /// Client original values is updated to match the store.
    /// </summary>
    StoreWins = 1,
    /// <summary>
    /// For unmodified client objects, same behavior as StoreWins.  For modified client
    /// objects, Refresh original values with store value, keeping all values on client
    /// object. The next time an update happens, all the client change units will be
    /// considered modified and require updating.
    /// </summary>
    ClientWins = 2,
  }
}
