// Decompiled with JetBrains decompiler
// Type: System.Linq.Expressions.EntityExpressionVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal abstract class EntityExpressionVisitor
  {
    internal const ExpressionType CustomExpression = ~ExpressionType.Add;

    internal virtual Expression Visit(Expression exp)
    {
      if (exp == null)
        return exp;
      switch (exp.NodeType)
      {
        case ~ExpressionType.Add:
          return this.VisitExtension(exp);
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.And:
        case ExpressionType.AndAlso:
        case ExpressionType.ArrayIndex:
        case ExpressionType.Coalesce:
        case ExpressionType.Divide:
        case ExpressionType.ExclusiveOr:
        case ExpressionType.LeftShift:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
        case ExpressionType.Or:
        case ExpressionType.OrElse:
        case ExpressionType.Power:
        case ExpressionType.RightShift:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
          return this.VisitBinary((BinaryExpression) exp);
        case ExpressionType.ArrayLength:
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
        case ExpressionType.Negate:
        case ExpressionType.UnaryPlus:
        case ExpressionType.NegateChecked:
        case ExpressionType.Not:
        case ExpressionType.Quote:
        case ExpressionType.TypeAs:
          return this.VisitUnary((UnaryExpression) exp);
        case ExpressionType.Call:
          return this.VisitMethodCall((MethodCallExpression) exp);
        case ExpressionType.Conditional:
          return this.VisitConditional((ConditionalExpression) exp);
        case ExpressionType.Constant:
          return this.VisitConstant((ConstantExpression) exp);
        case ExpressionType.Equal:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.NotEqual:
          return this.VisitComparison((BinaryExpression) exp);
        case ExpressionType.Invoke:
          return this.VisitInvocation((InvocationExpression) exp);
        case ExpressionType.Lambda:
          return this.VisitLambda((LambdaExpression) exp);
        case ExpressionType.ListInit:
          return this.VisitListInit((ListInitExpression) exp);
        case ExpressionType.MemberAccess:
          return this.VisitMemberAccess((MemberExpression) exp);
        case ExpressionType.MemberInit:
          return this.VisitMemberInit((MemberInitExpression) exp);
        case ExpressionType.New:
          return (Expression) this.VisitNew((NewExpression) exp);
        case ExpressionType.NewArrayInit:
        case ExpressionType.NewArrayBounds:
          return this.VisitNewArray((NewArrayExpression) exp);
        case ExpressionType.Parameter:
          return this.VisitParameter((ParameterExpression) exp);
        case ExpressionType.TypeIs:
          return this.VisitTypeIs((TypeBinaryExpression) exp);
        default:
          throw System.Linq.Expressions.Internal.Error.UnhandledExpressionType(exp.NodeType);
      }
    }

    internal virtual MemberBinding VisitBinding(MemberBinding binding)
    {
      switch (binding.BindingType)
      {
        case MemberBindingType.Assignment:
          return (MemberBinding) this.VisitMemberAssignment((MemberAssignment) binding);
        case MemberBindingType.MemberBinding:
          return (MemberBinding) this.VisitMemberMemberBinding((MemberMemberBinding) binding);
        case MemberBindingType.ListBinding:
          return (MemberBinding) this.VisitMemberListBinding((MemberListBinding) binding);
        default:
          throw System.Linq.Expressions.Internal.Error.UnhandledBindingType(binding.BindingType);
      }
    }

    internal virtual ElementInit VisitElementInitializer(ElementInit initializer)
    {
      ReadOnlyCollection<Expression> readOnlyCollection = this.VisitExpressionList(initializer.Arguments);
      return readOnlyCollection != initializer.Arguments ? Expression.ElementInit(initializer.AddMethod, (IEnumerable<Expression>) readOnlyCollection) : initializer;
    }

    internal virtual Expression VisitUnary(UnaryExpression u)
    {
      Expression operand = this.Visit(u.Operand);
      return operand != u.Operand ? (Expression) Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method) : (Expression) u;
    }

    internal virtual Expression VisitBinary(BinaryExpression b)
    {
      Expression left = this.Visit(b.Left);
      Expression right = this.Visit(b.Right);
      Expression expression = this.Visit((Expression) b.Conversion);
      if (left == b.Left && right == b.Right && expression == b.Conversion)
        return (Expression) b;
      return b.NodeType == ExpressionType.Coalesce && b.Conversion != null ? (Expression) Expression.Coalesce(left, right, expression as LambdaExpression) : (Expression) Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
    }

    internal virtual Expression VisitComparison(BinaryExpression expression) => this.VisitBinary(EntityExpressionVisitor.RemoveUnnecessaryConverts(expression));

    internal virtual Expression VisitTypeIs(TypeBinaryExpression b)
    {
      Expression expression = this.Visit(b.Expression);
      return expression != b.Expression ? (Expression) Expression.TypeIs(expression, b.TypeOperand) : (Expression) b;
    }

    internal virtual Expression VisitConstant(ConstantExpression c) => (Expression) c;

    internal virtual Expression VisitConditional(ConditionalExpression c)
    {
      Expression test = this.Visit(c.Test);
      Expression ifTrue = this.Visit(c.IfTrue);
      Expression ifFalse = this.Visit(c.IfFalse);
      return test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse ? (Expression) Expression.Condition(test, ifTrue, ifFalse) : (Expression) c;
    }

    internal virtual Expression VisitParameter(ParameterExpression p) => (Expression) p;

    internal virtual Expression VisitMemberAccess(MemberExpression m)
    {
      Expression expression = this.Visit(m.Expression);
      return expression != m.Expression ? (Expression) Expression.MakeMemberAccess(expression, m.Member) : (Expression) m;
    }

    internal virtual Expression VisitMethodCall(MethodCallExpression m)
    {
      Expression instance = this.Visit(m.Object);
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(m.Arguments);
      return instance != m.Object || arguments != m.Arguments ? (Expression) Expression.Call(instance, m.Method, arguments) : (Expression) m;
    }

    internal virtual ReadOnlyCollection<Expression> VisitExpressionList(
      ReadOnlyCollection<Expression> original)
    {
      List<Expression> sequence = (List<Expression>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        Expression expression = this.Visit(original[index1]);
        if (sequence != null)
          sequence.Add(expression);
        else if (expression != original[index1])
        {
          sequence = new List<Expression>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            sequence.Add(original[index2]);
          sequence.Add(expression);
        }
      }
      return sequence != null ? sequence.ToReadOnlyCollection<Expression>() : original;
    }

    internal virtual MemberAssignment VisitMemberAssignment(
      MemberAssignment assignment)
    {
      Expression expression = this.Visit(assignment.Expression);
      return expression != assignment.Expression ? Expression.Bind(assignment.Member, expression) : assignment;
    }

    internal virtual MemberMemberBinding VisitMemberMemberBinding(
      MemberMemberBinding binding)
    {
      IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
      return bindings != binding.Bindings ? Expression.MemberBind(binding.Member, bindings) : binding;
    }

    internal virtual MemberListBinding VisitMemberListBinding(
      MemberListBinding binding)
    {
      IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
      return initializers != binding.Initializers ? Expression.ListBind(binding.Member, initializers) : binding;
    }

    internal virtual IEnumerable<MemberBinding> VisitBindingList(
      ReadOnlyCollection<MemberBinding> original)
    {
      List<MemberBinding> memberBindingList = (List<MemberBinding>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        MemberBinding memberBinding = this.VisitBinding(original[index1]);
        if (memberBindingList != null)
          memberBindingList.Add(memberBinding);
        else if (memberBinding != original[index1])
        {
          memberBindingList = new List<MemberBinding>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            memberBindingList.Add(original[index2]);
          memberBindingList.Add(memberBinding);
        }
      }
      return (IEnumerable<MemberBinding>) memberBindingList ?? (IEnumerable<MemberBinding>) original;
    }

    internal virtual IEnumerable<ElementInit> VisitElementInitializerList(
      ReadOnlyCollection<ElementInit> original)
    {
      List<ElementInit> elementInitList = (List<ElementInit>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        ElementInit elementInit = this.VisitElementInitializer(original[index1]);
        if (elementInitList != null)
          elementInitList.Add(elementInit);
        else if (elementInit != original[index1])
        {
          elementInitList = new List<ElementInit>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            elementInitList.Add(original[index2]);
          elementInitList.Add(elementInit);
        }
      }
      return (IEnumerable<ElementInit>) elementInitList ?? (IEnumerable<ElementInit>) original;
    }

    internal virtual Expression VisitLambda(LambdaExpression lambda)
    {
      Expression body = this.Visit(lambda.Body);
      return body != lambda.Body ? (Expression) Expression.Lambda(lambda.Type, body, (IEnumerable<ParameterExpression>) lambda.Parameters) : (Expression) lambda;
    }

    internal virtual NewExpression VisitNew(NewExpression nex)
    {
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(nex.Arguments);
      if (arguments == nex.Arguments)
        return nex;
      return nex.Members != null ? Expression.New(nex.Constructor, arguments, (IEnumerable<MemberInfo>) nex.Members) : Expression.New(nex.Constructor, arguments);
    }

    internal virtual Expression VisitMemberInit(MemberInitExpression init)
    {
      NewExpression newExpression = this.VisitNew(init.NewExpression);
      IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
      return newExpression != init.NewExpression || bindings != init.Bindings ? (Expression) Expression.MemberInit(newExpression, bindings) : (Expression) init;
    }

    internal virtual Expression VisitListInit(ListInitExpression init)
    {
      NewExpression newExpression = this.VisitNew(init.NewExpression);
      IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
      return newExpression != init.NewExpression || initializers != init.Initializers ? (Expression) Expression.ListInit(newExpression, initializers) : (Expression) init;
    }

    internal virtual Expression VisitNewArray(NewArrayExpression na)
    {
      IEnumerable<Expression> expressions = (IEnumerable<Expression>) this.VisitExpressionList(na.Expressions);
      if (expressions == na.Expressions)
        return (Expression) na;
      return na.NodeType == ExpressionType.NewArrayInit ? (Expression) Expression.NewArrayInit(na.Type.GetElementType(), expressions) : (Expression) Expression.NewArrayBounds(na.Type.GetElementType(), expressions);
    }

    internal virtual Expression VisitInvocation(InvocationExpression iv)
    {
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(iv.Arguments);
      Expression expression = this.Visit(iv.Expression);
      return arguments != iv.Arguments || expression != iv.Expression ? (Expression) Expression.Invoke(expression, arguments) : (Expression) iv;
    }

    internal virtual Expression VisitExtension(Expression ext) => ext;

    internal static Expression Visit(
      Expression exp,
      Func<Expression, Func<Expression, Expression>, Expression> visit)
    {
      return new EntityExpressionVisitor.BasicExpressionVisitor(visit).Visit(exp);
    }

    private static BinaryExpression RemoveUnnecessaryConverts(
      BinaryExpression expression)
    {
      if (expression.Method != (MethodInfo) null || expression.Left.Type != expression.Right.Type)
        return expression;
      switch (expression.Left.NodeType)
      {
        case ExpressionType.Constant:
          ConstantExpression left1 = (ConstantExpression) expression.Left;
          if (expression.Right.NodeType == ExpressionType.Convert)
          {
            UnaryExpression right = (UnaryExpression) expression.Right;
            if (EntityExpressionVisitor.TryConvertConstant(ref left1, right.Operand.Type))
              return EntityExpressionVisitor.MakeBinaryExpression(expression.NodeType, (Expression) left1, right.Operand);
            break;
          }
          break;
        case ExpressionType.Convert:
          UnaryExpression left2 = (UnaryExpression) expression.Left;
          switch (expression.Right.NodeType)
          {
            case ExpressionType.Constant:
              ConstantExpression right1 = (ConstantExpression) expression.Right;
              if (EntityExpressionVisitor.TryConvertConstant(ref right1, left2.Operand.Type))
                return EntityExpressionVisitor.MakeBinaryExpression(expression.NodeType, left2.Operand, (Expression) right1);
              break;
            case ExpressionType.Convert:
              UnaryExpression right2 = (UnaryExpression) expression.Right;
              if (EntityExpressionVisitor.CanRemoveConverts(left2, right2))
                return EntityExpressionVisitor.MakeBinaryExpression(expression.NodeType, left2.Operand, right2.Operand);
              break;
          }
          break;
      }
      return expression;
    }

    private static bool CanRemoveConverts(UnaryExpression leftConvert, UnaryExpression rightConvert)
    {
      if (leftConvert.Method != (MethodInfo) null || rightConvert.Method != (MethodInfo) null || Type.GetTypeCode(leftConvert.Type) != TypeCode.Int32)
        return false;
      switch (Type.GetTypeCode(leftConvert.Operand.Type))
      {
        case TypeCode.Byte:
        case TypeCode.Int16:
          return leftConvert.Operand.Type == rightConvert.Operand.Type;
        default:
          return false;
      }
    }

    private static bool TryConvertConstant(ref ConstantExpression constant, Type type)
    {
      if (Type.GetTypeCode(constant.Type) != TypeCode.Int32)
        return false;
      int num = (int) constant.Value;
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Byte:
          if (num >= 0 && num <= (int) byte.MaxValue)
          {
            constant = Expression.Constant((object) (byte) num);
            return true;
          }
          break;
        case TypeCode.Int16:
          if (num >= (int) short.MinValue && num <= (int) short.MaxValue)
          {
            constant = Expression.Constant((object) (short) num);
            return true;
          }
          break;
      }
      return false;
    }

    private static BinaryExpression MakeBinaryExpression(
      ExpressionType expressionType,
      Expression left,
      Expression right)
    {
      if (left.Type.IsEnum)
        left = (Expression) Expression.Convert(left, left.Type.GetEnumUnderlyingType());
      if (right.Type.IsEnum)
        right = (Expression) Expression.Convert(right, right.Type.GetEnumUnderlyingType());
      return Expression.MakeBinary(expressionType, left, right);
    }

    private sealed class BasicExpressionVisitor : EntityExpressionVisitor
    {
      private readonly Func<Expression, Func<Expression, Expression>, Expression> _visit;

      internal BasicExpressionVisitor(
        Func<Expression, Func<Expression, Expression>, Expression> visit)
      {
        this._visit = visit ?? (Func<Expression, Func<Expression, Expression>, Expression>) ((exp, baseVisit) => baseVisit(exp));
      }

      internal override Expression Visit(Expression exp) => this._visit(exp, new Func<Expression, Expression>(((EntityExpressionVisitor) this).Visit));
    }
  }
}
