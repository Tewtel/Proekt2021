// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.ValidationAttributeValidator
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
  internal class ValidationAttributeValidator : IValidator
  {
    private readonly DisplayAttribute _displayAttribute;
    private readonly ValidationAttribute _validationAttribute;

    public ValidationAttributeValidator(
      ValidationAttribute validationAttribute,
      DisplayAttribute displayAttribute)
    {
      this._validationAttribute = validationAttribute;
      this._displayAttribute = displayAttribute;
    }

    public virtual IEnumerable<DbValidationError> Validate(
      EntityValidationContext entityValidationContext,
      InternalMemberEntry property)
    {
      if (!this.AttributeApplicable(entityValidationContext, property))
        return Enumerable.Empty<DbValidationError>();
      ValidationContext validationContext = entityValidationContext.ExternalValidationContext;
      validationContext.SetDisplayName(property, this._displayAttribute);
      object obj = property == null ? entityValidationContext.InternalEntity.Entity : property.CurrentValue;
      ValidationResult validationResult;
      try
      {
        validationResult = this._validationAttribute.GetValidationResult(obj, validationContext);
      }
      catch (Exception ex)
      {
        throw new DbUnexpectedValidationException(System.Data.Entity.Resources.Strings.DbUnexpectedValidationException_ValidationAttribute((object) validationContext.DisplayName, (object) this._validationAttribute.GetType()), ex);
      }
      if (validationResult == ValidationResult.Success)
        return Enumerable.Empty<DbValidationError>();
      return DbHelpers.SplitValidationResults(validationContext.MemberName, (IEnumerable<ValidationResult>) new ValidationResult[1]
      {
        validationResult
      });
    }

    protected virtual bool AttributeApplicable(
      EntityValidationContext entityValidationContext,
      InternalMemberEntry property)
    {
      InternalNavigationEntry internalNavigationEntry = property as InternalNavigationEntry;
      return !(this._validationAttribute is RequiredAttribute) || property == null || (property.InternalEntityEntry == null || property.InternalEntityEntry.State == EntityState.Added) || (property.InternalEntityEntry.State == EntityState.Detached || internalNavigationEntry == null || internalNavigationEntry.IsLoaded);
    }
  }
}
