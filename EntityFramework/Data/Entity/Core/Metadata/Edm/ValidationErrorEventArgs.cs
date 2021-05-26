﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ValidationErrorEventArgs
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class ValidationErrorEventArgs : EventArgs
  {
    private readonly EdmItemError _validationError;

    public ValidationErrorEventArgs(EdmItemError validationError) => this._validationError = validationError;

    public EdmItemError ValidationError => this._validationError;
  }
}
