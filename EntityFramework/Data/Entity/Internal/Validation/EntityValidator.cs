﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.EntityValidator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace System.Data.Entity.Internal.Validation
{
  internal class EntityValidator : TypeValidator
  {
    public EntityValidator(
      IEnumerable<PropertyValidator> propertyValidators,
      IEnumerable<IValidator> typeLevelValidators)
      : base(propertyValidators, typeLevelValidators)
    {
    }

    public DbEntityValidationResult Validate(
      EntityValidationContext entityValidationContext)
    {
      IEnumerable<DbValidationError> validationErrors = this.Validate(entityValidationContext, (InternalPropertyEntry) null);
      return new DbEntityValidationResult(entityValidationContext.InternalEntity, validationErrors);
    }

    protected override void ValidateProperties(
      EntityValidationContext entityValidationContext,
      InternalPropertyEntry parentProperty,
      List<DbValidationError> validationErrors)
    {
      InternalEntityEntry internalEntity = entityValidationContext.InternalEntity;
      foreach (PropertyValidator propertyValidator in this.PropertyValidators)
        validationErrors.AddRange(propertyValidator.Validate(entityValidationContext, internalEntity.Member(propertyValidator.PropertyName)));
    }
  }
}
