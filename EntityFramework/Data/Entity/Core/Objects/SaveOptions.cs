﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.SaveOptions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects
{
  /// <summary>
  /// Flags used to modify behavior of ObjectContext.SaveChanges()
  /// </summary>
  [Flags]
  public enum SaveOptions
  {
    /// <summary>
    /// Changes are saved without the DetectChanges or the AcceptAllChangesAfterSave methods being called.
    /// </summary>
    None = 0,
    /// <summary>
    /// After changes are saved, the AcceptAllChangesAfterSave method is called, which resets change tracking in the ObjectStateManager.
    /// </summary>
    AcceptAllChangesAfterSave = 1,
    /// <summary>
    /// Before changes are saved, the DetectChanges method is called to synchronize the property values of objects that are attached to the object context with data in the ObjectStateManager.
    /// </summary>
    DetectChangesBeforeSave = 2,
  }
}
