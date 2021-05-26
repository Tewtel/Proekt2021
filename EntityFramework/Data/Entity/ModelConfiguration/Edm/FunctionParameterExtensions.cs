// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.FunctionParameterExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class FunctionParameterExtensions
  {
    public static object GetConfiguration(this FunctionParameter functionParameter) => functionParameter.Annotations.GetConfiguration();

    public static void SetConfiguration(
      this FunctionParameter functionParameter,
      object configuration)
    {
      functionParameter.GetMetadataProperties().SetConfiguration(configuration);
    }
  }
}
