// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.ValueExpression
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class ValueExpression : ExpressionResolution
  {
    internal readonly DbExpression Value;

    internal ValueExpression(DbExpression value)
      : base(ExpressionResolutionClass.Value)
    {
      this.Value = value;
    }

    internal override string ExpressionClassName => ValueExpression.ValueClassName;

    internal static string ValueClassName => Strings.LocalizedValueExpression;
  }
}
