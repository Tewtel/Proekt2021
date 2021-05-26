// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.ValidationContextExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Internal;

namespace System.Data.Entity.Utilities
{
  internal static class ValidationContextExtensions
  {
    public static void SetDisplayName(
      this ValidationContext validationContext,
      InternalMemberEntry property,
      DisplayAttribute displayAttribute)
    {
      string str = displayAttribute == null ? (string) null : displayAttribute.GetName();
      if (property == null)
      {
        Type objectType = ObjectContextTypeCache.GetObjectType(validationContext.ObjectType);
        validationContext.DisplayName = str ?? objectType.Name;
        validationContext.MemberName = (string) null;
      }
      else
      {
        validationContext.DisplayName = str ?? DbHelpers.GetPropertyPath(property);
        validationContext.MemberName = DbHelpers.GetPropertyPath(property);
      }
    }
  }
}
