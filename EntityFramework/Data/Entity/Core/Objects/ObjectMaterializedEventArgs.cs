// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectMaterializedEventArgs
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects
{
  /// <summary>EventArgs for the ObjectMaterialized event.</summary>
  public class ObjectMaterializedEventArgs : EventArgs
  {
    private readonly object _entity;

    /// <summary>
    /// Constructs new arguments for the ObjectMaterialized event.
    /// </summary>
    /// <param name="entity"> The object that has been materialized. </param>
    public ObjectMaterializedEventArgs(object entity) => this._entity = entity;

    /// <summary>Gets the entity object that was created.</summary>
    /// <returns>The entity object that was created.</returns>
    public object Entity => this._entity;
  }
}
