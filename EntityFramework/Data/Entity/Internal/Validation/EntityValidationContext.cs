// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.EntityValidationContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations;

namespace System.Data.Entity.Internal.Validation
{
  internal class EntityValidationContext
  {
    private readonly InternalEntityEntry _entityEntry;

    public EntityValidationContext(
      InternalEntityEntry entityEntry,
      ValidationContext externalValidationContext)
    {
      this._entityEntry = entityEntry;
      this.ExternalValidationContext = externalValidationContext;
    }

    public ValidationContext ExternalValidationContext { get; private set; }

    public InternalEntityEntry InternalEntity => this._entityEntry;
  }
}
