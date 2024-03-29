﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ELinq.Binding
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq.Expressions;

namespace System.Data.Entity.Core.Objects.ELinq
{
  internal sealed class Binding
  {
    internal readonly Expression LinqExpression;
    internal readonly DbExpression CqtExpression;

    internal Binding(Expression linqExpression, DbExpression cqtExpression)
    {
      this.LinqExpression = linqExpression;
      this.CqtExpression = cqtExpression;
    }
  }
}
