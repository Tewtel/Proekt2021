// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.ComplexTypeExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class ComplexTypeExtensions
  {
    public static EdmProperty AddComplexProperty(
      this ComplexType complexType,
      string name,
      ComplexType targetComplexType)
    {
      EdmProperty complex = EdmProperty.CreateComplex(name, targetComplexType);
      complexType.AddMember((EdmMember) complex);
      return complex;
    }

    public static object GetConfiguration(this ComplexType complexType) => complexType.Annotations.GetConfiguration();

    public static Type GetClrType(this ComplexType complexType) => complexType.Annotations.GetClrType();

    internal static IEnumerable<ComplexType> ToHierarchy(
      this ComplexType edmType)
    {
      return EdmType.SafeTraverseHierarchy<ComplexType>(edmType);
    }
  }
}
