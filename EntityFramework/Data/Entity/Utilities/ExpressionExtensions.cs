// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.ExpressionExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Utilities
{
  internal static class ExpressionExtensions
  {
    public static PropertyPath GetSimplePropertyAccess(
      this LambdaExpression propertyAccessExpression)
    {
      PropertyPath propertyPath = propertyAccessExpression.Parameters.Single<ParameterExpression>().MatchSimplePropertyAccess(propertyAccessExpression.Body);
      return !(propertyPath == (PropertyPath) null) ? propertyPath : throw System.Data.Entity.Resources.Error.InvalidPropertyExpression((object) propertyAccessExpression);
    }

    public static PropertyPath GetComplexPropertyAccess(
      this LambdaExpression propertyAccessExpression)
    {
      PropertyPath propertyPath = propertyAccessExpression.Parameters.Single<ParameterExpression>().MatchComplexPropertyAccess(propertyAccessExpression.Body);
      return !(propertyPath == (PropertyPath) null) ? propertyPath : throw System.Data.Entity.Resources.Error.InvalidComplexPropertyExpression((object) propertyAccessExpression);
    }

    public static IEnumerable<PropertyPath> GetSimplePropertyAccessList(
      this LambdaExpression propertyAccessExpression)
    {
      return propertyAccessExpression.MatchPropertyAccessList((Func<Expression, Expression, PropertyPath>) ((p, e) => e.MatchSimplePropertyAccess(p))) ?? throw System.Data.Entity.Resources.Error.InvalidPropertiesExpression((object) propertyAccessExpression);
    }

    public static IEnumerable<PropertyPath> GetComplexPropertyAccessList(
      this LambdaExpression propertyAccessExpression)
    {
      return propertyAccessExpression.MatchPropertyAccessList((Func<Expression, Expression, PropertyPath>) ((p, e) => e.MatchComplexPropertyAccess(p))) ?? throw System.Data.Entity.Resources.Error.InvalidComplexPropertiesExpression((object) propertyAccessExpression);
    }

    private static IEnumerable<PropertyPath> MatchPropertyAccessList(
      this LambdaExpression lambdaExpression,
      Func<Expression, Expression, PropertyPath> propertyMatcher)
    {
      if (lambdaExpression.Body.RemoveConvert() is NewExpression newExpression)
      {
        ParameterExpression parameterExpression = lambdaExpression.Parameters.Single<ParameterExpression>();
        IEnumerable<PropertyPath> propertyPaths = newExpression.Arguments.Select<Expression, PropertyPath>((Func<Expression, PropertyPath>) (a => propertyMatcher(a, (Expression) parameterExpression))).Where<PropertyPath>((Func<PropertyPath, bool>) (p => p != (PropertyPath) null));
        if (propertyPaths.Count<PropertyPath>() == newExpression.Arguments.Count<Expression>())
          return !newExpression.HasDefaultMembersOnly(propertyPaths) ? (IEnumerable<PropertyPath>) null : propertyPaths;
      }
      PropertyPath propertyPath = propertyMatcher(lambdaExpression.Body, (Expression) lambdaExpression.Parameters.Single<ParameterExpression>());
      if (!(propertyPath != (PropertyPath) null))
        return (IEnumerable<PropertyPath>) null;
      return (IEnumerable<PropertyPath>) new PropertyPath[1]
      {
        propertyPath
      };
    }

    private static bool HasDefaultMembersOnly(
      this NewExpression newExpression,
      IEnumerable<PropertyPath> propertyPaths)
    {
      return newExpression.Members == null || !newExpression.Members.Where<MemberInfo>((Func<MemberInfo, int, bool>) ((t, i) => !string.Equals(t.Name, propertyPaths.ElementAt<PropertyPath>(i).Last<PropertyInfo>().Name, StringComparison.Ordinal))).Any<MemberInfo>();
    }

    private static PropertyPath MatchSimplePropertyAccess(
      this Expression parameterExpression,
      Expression propertyAccessExpression)
    {
      PropertyPath propertyPath = parameterExpression.MatchPropertyAccess(propertyAccessExpression);
      return !(propertyPath != (PropertyPath) null) || propertyPath.Count != 1 ? (PropertyPath) null : propertyPath;
    }

    private static PropertyPath MatchComplexPropertyAccess(
      this Expression parameterExpression,
      Expression propertyAccessExpression)
    {
      return parameterExpression.MatchPropertyAccess(propertyAccessExpression);
    }

    private static PropertyPath MatchPropertyAccess(
      this Expression parameterExpression,
      Expression propertyAccessExpression)
    {
      List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
      while (propertyAccessExpression.RemoveConvert() is MemberExpression memberExpression)
      {
        PropertyInfo member = memberExpression.Member as PropertyInfo;
        if (member == (PropertyInfo) null)
          return (PropertyPath) null;
        propertyInfoList.Insert(0, member);
        propertyAccessExpression = memberExpression.Expression;
        if (memberExpression.Expression == parameterExpression)
          return new PropertyPath((IEnumerable<PropertyInfo>) propertyInfoList);
      }
      return (PropertyPath) null;
    }

    public static Expression RemoveConvert(this Expression expression)
    {
      while (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
        expression = ((UnaryExpression) expression).Operand;
      return expression;
    }

    public static bool IsNullConstant(this Expression expression)
    {
      expression = expression.RemoveConvert();
      return expression.NodeType == ExpressionType.Constant && ((ConstantExpression) expression).Value == null;
    }

    public static bool IsStringAddExpression(this Expression expression) => expression is BinaryExpression binaryExpression && !(binaryExpression.Method == (MethodInfo) null) && (binaryExpression.NodeType == ExpressionType.Add && binaryExpression.Method.DeclaringType == typeof (string)) && string.Equals(binaryExpression.Method.Name, "Concat", StringComparison.Ordinal);
  }
}
