// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.CellLabel
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class CellLabel
  {
    private readonly int m_startLineNumber;
    private readonly int m_startLinePosition;
    private readonly string m_sourceLocation;

    internal CellLabel(CellLabel source)
    {
      this.m_startLineNumber = source.m_startLineNumber;
      this.m_startLinePosition = source.m_startLinePosition;
      this.m_sourceLocation = source.m_sourceLocation;
    }

    internal CellLabel(MappingFragment fragmentInfo)
      : this(fragmentInfo.StartLineNumber, fragmentInfo.StartLinePosition, fragmentInfo.SourceLocation)
    {
    }

    internal CellLabel(int startLineNumber, int startLinePosition, string sourceLocation)
    {
      this.m_startLineNumber = startLineNumber;
      this.m_startLinePosition = startLinePosition;
      this.m_sourceLocation = sourceLocation;
    }

    internal int StartLineNumber => this.m_startLineNumber;

    internal int StartLinePosition => this.m_startLinePosition;

    internal string SourceLocation => this.m_sourceLocation;
  }
}
