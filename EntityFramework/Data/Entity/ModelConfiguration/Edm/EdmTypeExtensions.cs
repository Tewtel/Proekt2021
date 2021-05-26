// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.EdmTypeExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class EdmTypeExtensions
  {
    public static Type GetClrType(this EdmType item)
    {
      switch (item)
      {
        case EntityType entityType:
          return EntityTypeExtensions.GetClrType(entityType);
        case EnumType enumType:
          return EnumTypeExtensions.GetClrType(enumType);
        case ComplexType complexType:
          return ComplexTypeExtensions.GetClrType(complexType);
        default:
          return (Type) null;
      }
    }
  }
}
