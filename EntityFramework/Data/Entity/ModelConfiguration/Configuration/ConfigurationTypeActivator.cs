// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ConfigurationTypeActivator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  internal class ConfigurationTypeActivator
  {
    public virtual TStructuralTypeConfiguration Activate<TStructuralTypeConfiguration>(Type type) where TStructuralTypeConfiguration : StructuralTypeConfiguration => !(type.GetDeclaredConstructor() == (ConstructorInfo) null) ? (TStructuralTypeConfiguration) typeof (StructuralTypeConfiguration<>).MakeGenericType(type.TryGetElementType(typeof (StructuralTypeConfiguration<>))).GetDeclaredProperty("Configuration").GetValue(Activator.CreateInstance(type, true), (object[]) null) : throw new InvalidOperationException(Strings.CreateConfigurationType_NoParameterlessConstructor((object) type.Name));
  }
}
