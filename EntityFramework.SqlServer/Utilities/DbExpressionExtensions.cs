// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.DbExpressionExtensions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal static class DbExpressionExtensions
  {
    public static IEnumerable<DbExpression> GetLeafNodes(
      this DbExpression root,
      DbExpressionKind kind,
      Func<DbExpression, IEnumerable<DbExpression>> getChildNodes)
    {
      Stack<DbExpression> nodes = new Stack<DbExpression>();
      nodes.Push(root);
      while (nodes.Count > 0)
      {
        DbExpression dbExpression1 = nodes.Pop();
        if (dbExpression1.ExpressionKind != kind)
        {
          yield return dbExpression1;
        }
        else
        {
          foreach (DbExpression dbExpression2 in getChildNodes(dbExpression1).Reverse<DbExpression>())
            nodes.Push(dbExpression2);
        }
      }
    }
  }
}
