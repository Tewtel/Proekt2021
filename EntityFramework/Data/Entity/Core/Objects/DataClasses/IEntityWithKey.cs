﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.IEntityWithKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>Interface that defines an entity containing a key.</summary>
  public interface IEntityWithKey
  {
    /// <summary>
    /// Gets or sets the <see cref="T:System.Data.Entity.Core.EntityKey" /> for instances of entity types that implement this interface.
    /// </summary>
    /// <remarks>
    /// If an object is being managed by a change tracker, it is expected that
    /// IEntityChangeTracker methods EntityMemberChanging and EntityMemberChanged will be
    /// used to report changes on EntityKey. This allows the change tracker to validate the
    /// EntityKey's new value and to verify if the change tracker is in a state where it can
    /// allow updates to the EntityKey.
    /// </remarks>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.EntityKey" /> for instances of entity types that implement this interface.
    /// </returns>
    EntityKey EntityKey { get; set; }
  }
}
