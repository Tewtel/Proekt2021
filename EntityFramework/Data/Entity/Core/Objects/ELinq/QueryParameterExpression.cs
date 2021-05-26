// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ELinq.QueryParameterExpression
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Core.Objects.ELinq
{
  internal sealed class QueryParameterExpression : Expression
  {
    private readonly DbParameterReferenceExpression _parameterReference;
    private readonly Type _type;
    private readonly Expression _funcletizedExpression;
    private readonly IEnumerable<ParameterExpression> _compiledQueryParameters;
    private Delegate _cachedDelegate;

    internal QueryParameterExpression(
      DbParameterReferenceExpression parameterReference,
      Expression funcletizedExpression,
      IEnumerable<ParameterExpression> compiledQueryParameters)
    {
      this._compiledQueryParameters = compiledQueryParameters ?? Enumerable.Empty<ParameterExpression>();
      this._parameterReference = parameterReference;
      this._type = funcletizedExpression.Type;
      this._funcletizedExpression = funcletizedExpression;
      this._cachedDelegate = (Delegate) null;
    }

    internal object EvaluateParameter(object[] arguments)
    {
      if ((object) this._cachedDelegate == null)
      {
        if (this._funcletizedExpression.NodeType == ExpressionType.Constant)
          return ((ConstantExpression) this._funcletizedExpression).Value;
        ConstantExpression constantExpression;
        if (QueryParameterExpression.TryEvaluatePath(this._funcletizedExpression, out constantExpression))
          return constantExpression.Value;
      }
      try
      {
        if ((object) this._cachedDelegate == null)
          this._cachedDelegate = Expression.Lambda(TypeSystem.GetDelegateType(this._compiledQueryParameters.Select<ParameterExpression, Type>((Func<ParameterExpression, Type>) (p => p.Type)), this._type), this._funcletizedExpression, this._compiledQueryParameters).Compile();
        return this._cachedDelegate.DynamicInvoke(arguments);
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    internal QueryParameterExpression EscapeParameterForLike(
      Expression<Func<string, Tuple<string, bool>>> method)
    {
      return new QueryParameterExpression(this._parameterReference, (Expression) Expression.Property((Expression) Expression.Invoke((Expression) method, this._funcletizedExpression), "Item1"), this._compiledQueryParameters);
    }

    internal DbParameterReferenceExpression ParameterReference => this._parameterReference;

    public override Type Type => this._type;

    public override ExpressionType NodeType => ~ExpressionType.Add;

    private static bool TryEvaluatePath(
      Expression expression,
      out ConstantExpression constantExpression)
    {
      memberExpression2 = expression as MemberExpression;
      constantExpression = (ConstantExpression) null;
      if (memberExpression2 != null)
      {
        Stack<MemberExpression> memberExpressionStack = new Stack<MemberExpression>();
        memberExpressionStack.Push(memberExpression2);
        while (memberExpression2.Expression is MemberExpression memberExpression2)
          memberExpressionStack.Push(memberExpression2);
        MemberExpression me1 = memberExpressionStack.Pop();
        object memberValue;
        if (me1.Expression is ConstantExpression && QueryParameterExpression.TryGetFieldOrPropertyValue(me1, ((ConstantExpression) me1.Expression).Value, out memberValue))
        {
          if (memberExpressionStack.Count > 0)
          {
            foreach (MemberExpression me2 in memberExpressionStack)
            {
              if (!QueryParameterExpression.TryGetFieldOrPropertyValue(me2, memberValue, out memberValue))
                return false;
            }
          }
          constantExpression = Expression.Constant(memberValue, expression.Type);
          return true;
        }
      }
      return false;
    }

    private static bool TryGetFieldOrPropertyValue(
      MemberExpression me,
      object instance,
      out object memberValue)
    {
      bool flag = false;
      memberValue = (object) null;
      try
      {
        if (me.Member.MemberType == MemberTypes.Field)
        {
          memberValue = ((FieldInfo) me.Member).GetValue(instance);
          flag = true;
        }
        else if (me.Member.MemberType == MemberTypes.Property)
        {
          memberValue = ((PropertyInfo) me.Member).GetValue(instance, (object[]) null);
          flag = true;
        }
        return flag;
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }
  }
}
