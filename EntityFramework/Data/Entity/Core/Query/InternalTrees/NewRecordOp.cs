// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.NewRecordOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class NewRecordOp : ScalarOp
  {
    private readonly List<EdmProperty> m_fields;
    internal static readonly NewRecordOp Pattern = new NewRecordOp();

    internal NewRecordOp(TypeUsage type)
      : base(OpType.NewRecord, type)
    {
      this.m_fields = new List<EdmProperty>((IEnumerable<EdmProperty>) TypeHelpers.GetEdmType<RowType>(type).Properties);
    }

    internal NewRecordOp(TypeUsage type, List<EdmProperty> fields)
      : base(OpType.NewRecord, type)
    {
      this.m_fields = fields;
    }

    private NewRecordOp()
      : base(OpType.NewRecord)
    {
    }

    internal bool GetFieldPosition(EdmProperty field, out int fieldPosition)
    {
      fieldPosition = 0;
      for (int index = 0; index < this.m_fields.Count; ++index)
      {
        if (this.m_fields[index] == field)
        {
          fieldPosition = index;
          return true;
        }
      }
      return false;
    }

    internal List<EdmProperty> Properties => this.m_fields;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
