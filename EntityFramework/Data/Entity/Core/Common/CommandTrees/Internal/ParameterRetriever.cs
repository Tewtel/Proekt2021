// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.Internal.ParameterRetriever
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
  internal sealed class ParameterRetriever : BasicCommandTreeVisitor
  {
    private readonly Dictionary<string, DbParameterReferenceExpression> paramMappings = new Dictionary<string, DbParameterReferenceExpression>();

    private ParameterRetriever()
    {
    }

    internal static ReadOnlyCollection<DbParameterReferenceExpression> GetParameters(
      DbCommandTree tree)
    {
      ParameterRetriever parameterRetriever = new ParameterRetriever();
      parameterRetriever.VisitCommandTree(tree);
      return new ReadOnlyCollection<DbParameterReferenceExpression>((IList<DbParameterReferenceExpression>) parameterRetriever.paramMappings.Values.ToList<DbParameterReferenceExpression>());
    }

    public override void Visit(DbParameterReferenceExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbParameterReferenceExpression>(expression, nameof (expression));
      this.paramMappings[expression.ParameterName] = expression;
    }
  }
}
