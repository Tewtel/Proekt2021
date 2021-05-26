// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Utils.ExceptionHelpers
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Utils
{
  internal static class ExceptionHelpers
  {
    internal static void ThrowMappingException(
      ErrorLog.Record errorRecord,
      ConfigViewGenerator config)
    {
      InternalMappingException mappingException = new InternalMappingException(errorRecord.ToUserString(), errorRecord);
      if (config.IsNormalTracing)
        mappingException.ErrorLog.PrintTrace();
      throw mappingException;
    }

    internal static void ThrowMappingException(ErrorLog errorLog, ConfigViewGenerator config)
    {
      InternalMappingException mappingException = new InternalMappingException(errorLog.ToUserString(), errorLog);
      if (config.IsNormalTracing)
        mappingException.ErrorLog.PrintTrace();
      throw mappingException;
    }
  }
}
