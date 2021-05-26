﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Internal.Materialization.CollectionTranslatorResult
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Linq.Expressions;

namespace System.Data.Entity.Core.Common.Internal.Materialization
{
  internal class CollectionTranslatorResult : TranslatorResult
  {
    internal readonly Expression ExpressionToGetCoordinator;

    internal CollectionTranslatorResult(
      Expression returnedExpression,
      Type requestedType,
      Expression expressionToGetCoordinator)
      : base(returnedExpression, requestedType)
    {
      this.ExpressionToGetCoordinator = expressionToGetCoordinator;
    }
  }
}
