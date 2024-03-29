﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.DataModelErrorEventArgsExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Text;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class DataModelErrorEventArgsExtensions
  {
    public static string ToErrorMessage(
      this IEnumerable<DataModelErrorEventArgs> validationErrors)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(Strings.ValidationHeader);
      stringBuilder.AppendLine();
      foreach (DataModelErrorEventArgs validationError in validationErrors)
        stringBuilder.AppendLine(Strings.ValidationItemFormat((object) validationError.Item, (object) validationError.PropertyName, (object) validationError.ErrorMessage));
      return stringBuilder.ToString();
    }
  }
}
