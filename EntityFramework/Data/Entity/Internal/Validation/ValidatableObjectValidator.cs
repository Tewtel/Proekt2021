// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.ValidatableObjectValidator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Utilities;
using System.Data.Entity.Validation;
using System.Linq;

namespace System.Data.Entity.Internal.Validation
{
  internal class ValidatableObjectValidator : IValidator
  {
    private readonly DisplayAttribute _displayAttribute;

    public ValidatableObjectValidator(DisplayAttribute displayAttribute) => this._displayAttribute = displayAttribute;

    public virtual IEnumerable<DbValidationError> Validate(
      EntityValidationContext entityValidationContext,
      InternalMemberEntry property)
    {
      if (property != null && property.CurrentValue == null)
        return Enumerable.Empty<DbValidationError>();
      ValidationContext validationContext = entityValidationContext.ExternalValidationContext;
      validationContext.SetDisplayName(property, this._displayAttribute);
      IValidatableObject validatableObject = property == null ? (IValidatableObject) entityValidationContext.InternalEntity.Entity : (IValidatableObject) property.CurrentValue;
      IEnumerable<ValidationResult> validationResults;
      try
      {
        validationResults = validatableObject.Validate(validationContext);
      }
      catch (Exception ex)
      {
        throw new DbUnexpectedValidationException(System.Data.Entity.Resources.Strings.DbUnexpectedValidationException_IValidatableObject((object) validationContext.DisplayName, (object) ObjectContextTypeCache.GetObjectType(validatableObject.GetType())), ex);
      }
      return DbHelpers.SplitValidationResults(validationContext.MemberName, validationResults ?? Enumerable.Empty<ValidationResult>());
    }
  }
}
