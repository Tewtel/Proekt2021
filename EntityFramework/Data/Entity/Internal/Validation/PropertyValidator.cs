// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.PropertyValidator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace System.Data.Entity.Internal.Validation
{
  internal class PropertyValidator
  {
    private readonly IEnumerable<IValidator> _propertyValidators;
    private readonly string _propertyName;

    public PropertyValidator(string propertyName, IEnumerable<IValidator> propertyValidators)
    {
      this._propertyValidators = propertyValidators;
      this._propertyName = propertyName;
    }

    public IEnumerable<IValidator> PropertyAttributeValidators => this._propertyValidators;

    public string PropertyName => this._propertyName;

    public virtual IEnumerable<DbValidationError> Validate(
      EntityValidationContext entityValidationContext,
      InternalMemberEntry property)
    {
      List<DbValidationError> dbValidationErrorList = new List<DbValidationError>();
      foreach (IValidator propertyValidator in this._propertyValidators)
        dbValidationErrorList.AddRange(propertyValidator.Validate(entityValidationContext, property));
      return (IEnumerable<DbValidationError>) dbValidationErrorList;
    }
  }
}
