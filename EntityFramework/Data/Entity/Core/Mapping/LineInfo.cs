// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.LineInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Xml;
using System.Xml.XPath;

namespace System.Data.Entity.Core.Mapping
{
  internal sealed class LineInfo : IXmlLineInfo
  {
    private readonly bool m_hasLineInfo;
    private readonly int m_lineNumber;
    private readonly int m_linePosition;
    internal static readonly LineInfo Empty = new LineInfo();

    internal LineInfo(XPathNavigator nav)
      : this((IXmlLineInfo) nav)
    {
    }

    internal LineInfo(IXmlLineInfo lineInfo)
    {
      this.m_hasLineInfo = lineInfo.HasLineInfo();
      this.m_lineNumber = lineInfo.LineNumber;
      this.m_linePosition = lineInfo.LinePosition;
    }

    private LineInfo()
    {
      this.m_hasLineInfo = false;
      this.m_lineNumber = 0;
      this.m_linePosition = 0;
    }

    public int LineNumber => this.m_lineNumber;

    public int LinePosition => this.m_linePosition;

    public bool HasLineInfo() => this.m_hasLineInfo;
  }
}
