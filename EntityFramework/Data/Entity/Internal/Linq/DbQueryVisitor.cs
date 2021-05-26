// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.DbQueryVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Internal.Linq
{
  internal class DbQueryVisitor : ExpressionVisitor
  {
    private const BindingFlags SetAccessBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private static readonly ConcurrentDictionary<Type, Func<ObjectQuery, object>> _wrapperFactories = new ConcurrentDictionary<Type, Func<ObjectQuery, object>>();

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
      System.Data.Entity.Utilities.Check.NotNull<MethodCallExpression>(node, nameof (node));
      if (typeof (DbContext).IsAssignableFrom(node.Method.DeclaringType))
      {
        DbContext dbContext = (DbContext) null;
        if (node.Object is MemberExpression memberExpression2)
          dbContext = DbQueryVisitor.GetContextFromConstantExpression(memberExpression2.Expression, memberExpression2.Member);
        else if (node.Object is ConstantExpression constantExpression4)
          dbContext = constantExpression4.Value as DbContext;
        if (dbContext != null && !node.Method.GetCustomAttributes<DbFunctionAttribute>(false).Any<DbFunctionAttribute>() && node.Method.GetParameters().Length == 0)
        {
          Expression objectQueryConstant = DbQueryVisitor.CreateObjectQueryConstant(node.Method.Invoke((object) dbContext, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, (object[]) null, (CultureInfo) null));
          if (objectQueryConstant != null)
            return objectQueryConstant;
        }
      }
      return base.VisitMethodCall(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
      System.Data.Entity.Utilities.Check.NotNull<MemberExpression>(node, nameof (node));
      PropertyInfo member = node.Member as PropertyInfo;
      MemberExpression expression = node.Expression as MemberExpression;
      if (member != (PropertyInfo) null && expression != null && (typeof (IQueryable).IsAssignableFrom(member.PropertyType) && typeof (DbContext).IsAssignableFrom(node.Member.DeclaringType)))
      {
        DbContext constantExpression = DbQueryVisitor.GetContextFromConstantExpression(expression.Expression, expression.Member);
        if (constantExpression != null)
        {
          Expression objectQueryConstant = DbQueryVisitor.CreateObjectQueryConstant(member.GetValue((object) constantExpression, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, (object[]) null, (CultureInfo) null));
          if (objectQueryConstant != null)
            return objectQueryConstant;
        }
      }
      return base.VisitMember(node);
    }

    private static DbContext GetContextFromConstantExpression(
      Expression expression,
      MemberInfo member)
    {
      if (expression == null)
        return DbQueryVisitor.GetContextFromMember(member, (object) null);
      object expressionValue = DbQueryVisitor.GetExpressionValue(expression);
      return expressionValue != null ? DbQueryVisitor.GetContextFromMember(member, expressionValue) : (DbContext) null;
    }

    private static object GetExpressionValue(Expression expression)
    {
      switch (expression)
      {
        case ConstantExpression constantExpression:
          return constantExpression.Value;
        case MemberExpression memberExpression:
          FieldInfo member1 = memberExpression.Member as FieldInfo;
          if (member1 != (FieldInfo) null)
          {
            object expressionValue = DbQueryVisitor.GetExpressionValue(memberExpression.Expression);
            if (expressionValue != null)
              return member1.GetValue(expressionValue);
          }
          PropertyInfo member2 = memberExpression.Member as PropertyInfo;
          if (member2 != (PropertyInfo) null)
          {
            object expressionValue = DbQueryVisitor.GetExpressionValue(memberExpression.Expression);
            if (expressionValue != null)
              return member2.GetValue(expressionValue, (object[]) null);
            break;
          }
          break;
      }
      return (object) null;
    }

    private static DbContext GetContextFromMember(MemberInfo member, object value)
    {
      FieldInfo fieldInfo = member as FieldInfo;
      if (fieldInfo != (FieldInfo) null)
        return fieldInfo.GetValue(value) as DbContext;
      PropertyInfo propertyInfo = member as PropertyInfo;
      return propertyInfo != (PropertyInfo) null ? propertyInfo.GetValue(value, (object[]) null) as DbContext : (DbContext) null;
    }

    private static Expression CreateObjectQueryConstant(object dbQuery)
    {
      ObjectQuery objectQuery = DbQueryVisitor.ExtractObjectQuery(dbQuery);
      if (objectQuery == null)
        return (Expression) null;
      Type key = ((IEnumerable<Type>) objectQuery.GetType().GetGenericArguments()).Single<Type>();
      Func<ObjectQuery, object> func;
      if (!DbQueryVisitor._wrapperFactories.TryGetValue(key, out func))
      {
        func = (Func<ObjectQuery, object>) Delegate.CreateDelegate(typeof (Func<ObjectQuery, object>), typeof (ReplacementDbQueryWrapper<>).MakeGenericType(key).GetDeclaredMethod("Create", typeof (ObjectQuery)));
        DbQueryVisitor._wrapperFactories.TryAdd(key, func);
      }
      object obj = func(objectQuery);
      return (Expression) Expression.Property((Expression) Expression.Constant(obj, obj.GetType()), "Query");
    }

    private static ObjectQuery ExtractObjectQuery(object dbQuery) => dbQuery is IInternalQueryAdapter internalQueryAdapter ? internalQueryAdapter.InternalQuery.ObjectQuery : (ObjectQuery) null;
  }
}
