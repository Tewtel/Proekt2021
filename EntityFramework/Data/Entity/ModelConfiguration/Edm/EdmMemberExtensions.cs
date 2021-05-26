// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.EdmMemberExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class EdmMemberExtensions
  {
    public static PropertyInfo GetClrPropertyInfo(this EdmMember property) => property.Annotations.GetClrPropertyInfo();

    public static void SetClrPropertyInfo(this EdmMember property, PropertyInfo propertyInfo) => property.GetMetadataProperties().SetClrPropertyInfo(propertyInfo);

    public static IEnumerable<T> GetClrAttributes<T>(this EdmMember property) where T : Attribute
    {
      IList<Attribute> clrAttributes = property.Annotations.GetClrAttributes();
      return clrAttributes == null ? Enumerable.Empty<T>() : clrAttributes.OfType<T>();
    }
  }
}
