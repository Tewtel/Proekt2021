// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.InternalMappingException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Runtime.Serialization;

namespace System.Data.Entity.Core
{
  [Serializable]
  internal class InternalMappingException : EntityException
  {
    private readonly ErrorLog m_errorLog;

    internal InternalMappingException()
    {
    }

    internal InternalMappingException(string message)
      : base(message)
    {
    }

    internal InternalMappingException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected InternalMappingException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal InternalMappingException(string message, ErrorLog errorLog)
      : base(message)
    {
      this.m_errorLog = errorLog;
    }

    internal InternalMappingException(string message, ErrorLog.Record record)
      : base(message)
    {
      this.m_errorLog = new ErrorLog();
      this.m_errorLog.AddEntry(record);
    }

    internal ErrorLog ErrorLog => this.m_errorLog;
  }
}
