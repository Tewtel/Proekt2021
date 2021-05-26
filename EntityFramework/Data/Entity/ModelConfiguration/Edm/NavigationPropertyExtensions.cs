// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.NavigationPropertyExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class NavigationPropertyExtensions
  {
    public static object GetConfiguration(this NavigationProperty navigationProperty) => navigationProperty.Annotations.GetConfiguration();

    public static void SetConfiguration(
      this NavigationProperty navigationProperty,
      object configuration)
    {
      navigationProperty.GetMetadataProperties().SetConfiguration(configuration);
    }

    public static AssociationEndMember GetFromEnd(
      this NavigationProperty navProp)
    {
      return navProp.Association.SourceEnd != navProp.ResultEnd ? navProp.Association.SourceEnd : navProp.Association.TargetEnd;
    }
  }
}
