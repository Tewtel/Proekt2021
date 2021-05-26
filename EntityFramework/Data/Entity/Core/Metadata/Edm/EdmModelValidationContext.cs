﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EdmModelValidationContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class EdmModelValidationContext
  {
    private readonly EdmModel _model;
    private readonly bool _validateSyntax;

    public event EventHandler<DataModelErrorEventArgs> OnError;

    public EdmModelValidationContext(EdmModel model, bool validateSyntax)
    {
      this._model = model;
      this._validateSyntax = validateSyntax;
    }

    public bool ValidateSyntax => this._validateSyntax;

    public EdmModel Model => this._model;

    public bool IsCSpace => this._model.Containers.First<EntityContainer>().DataSpace == DataSpace.CSpace;

    public void AddError(MetadataItem item, string propertyName, string errorMessage) => this.RaiseDataModelValidationEvent(new DataModelErrorEventArgs()
    {
      ErrorMessage = errorMessage,
      Item = item,
      PropertyName = propertyName
    });

    private void RaiseDataModelValidationEvent(DataModelErrorEventArgs error)
    {
      if (this.OnError == null)
        return;
      this.OnError((object) this, error);
    }
  }
}
