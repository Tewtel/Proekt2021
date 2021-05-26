// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.ComplexPropertyValidator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace System.Data.Entity.Internal.Validation
{
  internal class ComplexPropertyValidator : PropertyValidator
  {
    private readonly ComplexTypeValidator _complexTypeValidator;

    public ComplexTypeValidator ComplexTypeValidator => this._complexTypeValidator;

    public ComplexPropertyValidator(
      string propertyName,
      IEnumerable<IValidator> propertyValidators,
      ComplexTypeValidator complexTypeValidator)
      : base(propertyName, propertyValidators)
    {
      this._complexTypeValidator = complexTypeValidator;
    }

    public override IEnumerable<DbValidationError> Validate(
      EntityValidationContext entityValidationContext,
      InternalMemberEntry property)
    {
      List<DbValidationError> source = new List<DbValidationError>();
      source.AddRange(base.Validate(entityValidationContext, property));
      if (!source.Any<DbValidationError>() && property.CurrentValue != null && this._complexTypeValidator != null)
        source.AddRange(this._complexTypeValidator.Validate(entityValidationContext, (InternalPropertyEntry) property));
      return (IEnumerable<DbValidationError>) source;
    }
  }
}
