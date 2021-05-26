// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Types.ComplexTypeConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Configuration.Types
{
  internal class ComplexTypeConfiguration : StructuralTypeConfiguration
  {
    internal ComplexTypeConfiguration(Type structuralType)
      : base(structuralType)
    {
    }

    private ComplexTypeConfiguration(ComplexTypeConfiguration source)
      : base((StructuralTypeConfiguration) source)
    {
    }

    internal virtual ComplexTypeConfiguration Clone() => new ComplexTypeConfiguration(this);

    internal virtual void Configure(ComplexType complexType) => this.Configure(complexType.Name, (IEnumerable<EdmProperty>) complexType.Properties, (ICollection<MetadataProperty>) complexType.GetMetadataProperties());
  }
}
