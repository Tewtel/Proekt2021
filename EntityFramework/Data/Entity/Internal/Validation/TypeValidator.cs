// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.TypeValidator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace System.Data.Entity.Internal.Validation
{
  internal abstract class TypeValidator
  {
    private readonly IEnumerable<IValidator> _typeLevelValidators;
    private readonly IEnumerable<PropertyValidator> _propertyValidators;

    public TypeValidator(
      IEnumerable<PropertyValidator> propertyValidators,
      IEnumerable<IValidator> typeLevelValidators)
    {
      this._typeLevelValidators = typeLevelValidators;
      this._propertyValidators = propertyValidators;
    }

    public IEnumerable<IValidator> TypeLevelValidators => this._typeLevelValidators;

    public IEnumerable<PropertyValidator> PropertyValidators => this._propertyValidators;

    protected IEnumerable<DbValidationError> Validate(
      EntityValidationContext entityValidationContext,
      InternalPropertyEntry property)
    {
      List<DbValidationError> dbValidationErrorList = new List<DbValidationError>();
      this.ValidateProperties(entityValidationContext, property, dbValidationErrorList);
      if (!dbValidationErrorList.Any<DbValidationError>())
      {
        foreach (IValidator typeLevelValidator in this._typeLevelValidators)
          dbValidationErrorList.AddRange(typeLevelValidator.Validate(entityValidationContext, (InternalMemberEntry) property));
      }
      return (IEnumerable<DbValidationError>) dbValidationErrorList;
    }

    protected abstract void ValidateProperties(
      EntityValidationContext entityValidationContext,
      InternalPropertyEntry parentProperty,
      List<DbValidationError> validationErrors);

    public PropertyValidator GetPropertyValidator(string name) => this._propertyValidators.SingleOrDefault<PropertyValidator>((Func<PropertyValidator, bool>) (v => v.PropertyName == name));
  }
}
