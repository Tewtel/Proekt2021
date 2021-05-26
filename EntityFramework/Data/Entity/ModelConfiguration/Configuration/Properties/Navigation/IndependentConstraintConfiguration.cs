// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation.IndependentConstraintConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.ModelConfiguration.Edm;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation
{
  internal class IndependentConstraintConfiguration : ConstraintConfiguration
  {
    private static readonly ConstraintConfiguration _instance = (ConstraintConfiguration) new IndependentConstraintConfiguration();

    private IndependentConstraintConfiguration()
    {
    }

    public static ConstraintConfiguration Instance => IndependentConstraintConfiguration._instance;

    internal override ConstraintConfiguration Clone() => IndependentConstraintConfiguration._instance;

    internal override void Configure(
      AssociationType associationType,
      AssociationEndMember dependentEnd,
      EntityTypeConfiguration entityTypeConfiguration)
    {
      associationType.MarkIndependent();
    }
  }
}
