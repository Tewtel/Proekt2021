// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ConfigurationTypeFilter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Utilities;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  internal class ConfigurationTypeFilter
  {
    public virtual bool IsEntityTypeConfiguration(Type type) => ConfigurationTypeFilter.IsStructuralTypeConfiguration(type, typeof (EntityTypeConfiguration<>));

    public virtual bool IsComplexTypeConfiguration(Type type) => ConfigurationTypeFilter.IsStructuralTypeConfiguration(type, typeof (ComplexTypeConfiguration<>));

    private static bool IsStructuralTypeConfiguration(Type type, Type structuralTypeConfiguration) => !type.IsAbstract() && type.TryGetElementType(structuralTypeConfiguration) != (Type) null;
  }
}
