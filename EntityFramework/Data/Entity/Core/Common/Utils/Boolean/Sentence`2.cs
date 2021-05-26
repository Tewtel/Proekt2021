// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.Sentence`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal abstract class Sentence<T_Identifier, T_Clause> : NormalFormNode<T_Identifier>
    where T_Clause : Clause<T_Identifier>, IEquatable<T_Clause>
  {
    private readonly Set<T_Clause> _clauses;

    protected Sentence(Set<T_Clause> clauses, ExprType treeType)
      : base(Sentence<T_Identifier, T_Clause>.ConvertClausesToExpr(clauses, treeType))
    {
      this._clauses = clauses.AsReadOnly();
    }

    private static BoolExpr<T_Identifier> ConvertClausesToExpr(
      Set<T_Clause> clauses,
      ExprType treeType)
    {
      int num = treeType == ExprType.And ? 1 : 0;
      IEnumerable<BoolExpr<T_Identifier>> children = clauses.Select<T_Clause, BoolExpr<T_Identifier>>(new Func<T_Clause, BoolExpr<T_Identifier>>(NormalFormNode<T_Identifier>.ExprSelector<T_Clause>));
      return num != 0 ? (BoolExpr<T_Identifier>) new AndExpr<T_Identifier>(children) : (BoolExpr<T_Identifier>) new OrExpr<T_Identifier>(children);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Sentence{");
      stringBuilder.Append((object) this._clauses);
      return stringBuilder.Append("}").ToString();
    }
  }
}
