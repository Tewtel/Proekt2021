// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.ViewGenResults
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration
{
  internal class ViewGenResults : InternalBase
  {
    private readonly KeyToListMap<EntitySetBase, GeneratedView> m_views;
    private readonly ErrorLog m_errorLog;

    internal ViewGenResults()
    {
      this.m_views = new KeyToListMap<EntitySetBase, GeneratedView>((IEqualityComparer<EntitySetBase>) EqualityComparer<EntitySetBase>.Default);
      this.m_errorLog = new ErrorLog();
    }

    internal KeyToListMap<EntitySetBase, GeneratedView> Views => this.m_views;

    internal IEnumerable<EdmSchemaError> Errors => this.m_errorLog.Errors;

    internal bool HasErrors => this.m_errorLog.Count > 0;

    internal void AddErrors(ErrorLog errorLog) => this.m_errorLog.Merge(errorLog);

    internal string ErrorsToString() => this.m_errorLog.ToString();

    internal override void ToCompactString(StringBuilder builder)
    {
      builder.Append(this.m_errorLog.Count);
      builder.Append(" ");
      this.m_errorLog.ToCompactString(builder);
    }
  }
}
