// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.ComplexTypeValidator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace System.Data.Entity.Internal.Validation
{
  internal class ComplexTypeValidator : TypeValidator
  {
    public ComplexTypeValidator(
      IEnumerable<PropertyValidator> propertyValidators,
      IEnumerable<IValidator> typeLevelValidators)
      : base(propertyValidators, typeLevelValidators)
    {
    }

    public new IEnumerable<DbValidationError> Validate(
      EntityValidationContext entityValidationContext,
      InternalPropertyEntry property)
    {
      return base.Validate(entityValidationContext, property);
    }

    protected override void ValidateProperties(
      EntityValidationContext entityValidationContext,
      InternalPropertyEntry parentProperty,
      List<DbValidationError> validationErrors)
    {
      foreach (PropertyValidator propertyValidator in this.PropertyValidators)
      {
        InternalPropertyEntry internalPropertyEntry = parentProperty.Property(propertyValidator.PropertyName);
        validationErrors.AddRange(propertyValidator.Validate(entityValidationContext, (InternalMemberEntry) internalPropertyEntry));
      }
    }
  }
}
