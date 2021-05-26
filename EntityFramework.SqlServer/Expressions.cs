// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Expressions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.SqlServer
{
  internal static class Expressions
  {
    internal static Expression Null<TNullType>() => (Expression) Expression.Constant((object) null, typeof (TNullType));

    internal static Expression Null(Type nullType) => (Expression) Expression.Constant((object) null, nullType);

    internal static Expression<Func<TArg, TResult>> Lambda<TArg, TResult>(
      string argumentName,
      Func<ParameterExpression, Expression> createLambdaBodyGivenParameter)
    {
      ParameterExpression parameterExpression;
      return Expression.Lambda<Func<TArg, TResult>>(createLambdaBodyGivenParameter(parameterExpression), parameterExpression);
    }

    internal static Expression Call(this Expression exp, string methodName) => (Expression) Expression.Call(exp, methodName, Type.EmptyTypes);

    internal static Expression ConvertTo(this Expression exp, Type convertToType) => (Expression) Expression.Convert(exp, convertToType);

    internal static Expression ConvertTo<TConvertToType>(this Expression exp) => (Expression) Expression.Convert(exp, typeof (TConvertToType));

    internal static System.Data.Entity.SqlServer.Expressions.ConditionalExpressionBuilder IfTrueThen(
      this Expression conditionExp,
      Expression resultIfTrue)
    {
      return new System.Data.Entity.SqlServer.Expressions.ConditionalExpressionBuilder(conditionExp, resultIfTrue);
    }

    internal static Expression Property<TPropertyType>(
      this Expression exp,
      string propertyName)
    {
      PropertyInfo runtimeProperty = exp.Type.GetRuntimeProperty(propertyName);
      return (Expression) Expression.Property(exp, runtimeProperty);
    }

    internal sealed class ConditionalExpressionBuilder
    {
      private readonly Expression condition;
      private readonly Expression ifTrueThen;

      internal ConditionalExpressionBuilder(
        Expression conditionExpression,
        Expression ifTrueExpression)
      {
        this.condition = conditionExpression;
        this.ifTrueThen = ifTrueExpression;
      }

      internal Expression Else(Expression resultIfFalse) => (Expression) Expression.Condition(this.condition, this.ifTrueThen, resultIfFalse);
    }
  }
}
