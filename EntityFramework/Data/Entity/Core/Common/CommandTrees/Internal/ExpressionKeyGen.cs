// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.Internal.ExpressionKeyGen
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Hierarchy;
using System.Data.Entity.Resources;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
  internal sealed class ExpressionKeyGen : DbExpressionVisitor
  {
    private readonly StringBuilder _key = new StringBuilder();
    private static readonly string[] _exprKindNames = ExpressionKeyGen.InitializeExprKindNames();

    internal static bool TryGenerateKey(DbExpression tree, out string key)
    {
      ExpressionKeyGen expressionKeyGen = new ExpressionKeyGen();
      try
      {
        tree.Accept((DbExpressionVisitor) expressionKeyGen);
        key = expressionKeyGen._key.ToString();
        return true;
      }
      catch (NotSupportedException ex)
      {
        key = (string) null;
        return false;
      }
    }

    internal ExpressionKeyGen()
    {
    }

    private static string[] InitializeExprKindNames()
    {
      string[] names = Enum.GetNames(typeof (DbExpressionKind));
      names[10] = "/";
      names[33] = "%";
      names[34] = "*";
      names[44] = "+";
      names[32] = "-";
      names[54] = "-";
      names[13] = "=";
      names[28] = "<";
      names[29] = "<=";
      names[18] = ">";
      names[19] = ">=";
      names[37] = "<>";
      names[46] = ".";
      names[21] = "IJ";
      names[16] = "FOJ";
      names[27] = "LOJ";
      names[6] = "CA";
      names[42] = "OA";
      return names;
    }

    internal string Key => this._key.ToString();

    private void VisitVariableName(string varName)
    {
      this._key.Append('\'');
      this._key.Append(varName.Replace("'", "''"));
      this._key.Append('\'');
    }

    private void VisitBinding(DbExpressionBinding binding)
    {
      this._key.Append("BV");
      this.VisitVariableName(binding.VariableName);
      this._key.Append("=(");
      binding.Expression.Accept((DbExpressionVisitor) this);
      this._key.Append(')');
    }

    private void VisitGroupBinding(DbGroupExpressionBinding groupBinding)
    {
      this._key.Append("GBVV");
      this.VisitVariableName(groupBinding.VariableName);
      this._key.Append(",");
      this.VisitVariableName(groupBinding.GroupVariableName);
      this._key.Append("=(");
      groupBinding.Expression.Accept((DbExpressionVisitor) this);
      this._key.Append(')');
    }

    private void VisitFunction(EdmFunction func, IList<DbExpression> args)
    {
      this._key.Append("FUNC<");
      this._key.Append(func.Identity);
      this._key.Append(">:ARGS(");
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) args)
      {
        this._key.Append('(');
        dbExpression.Accept((DbExpressionVisitor) this);
        this._key.Append(')');
      }
      this._key.Append(')');
    }

    private void VisitExprKind(DbExpressionKind kind)
    {
      this._key.Append('[');
      this._key.Append(ExpressionKeyGen._exprKindNames[(int) kind]);
      this._key.Append(']');
    }

    private void VisitUnary(DbUnaryExpression expr)
    {
      this.VisitExprKind(expr.ExpressionKind);
      this._key.Append('(');
      expr.Argument.Accept((DbExpressionVisitor) this);
      this._key.Append(')');
    }

    private void VisitBinary(DbBinaryExpression expr)
    {
      this.VisitExprKind(expr.ExpressionKind);
      this._key.Append('(');
      expr.Left.Accept((DbExpressionVisitor) this);
      this._key.Append(',');
      expr.Right.Accept((DbExpressionVisitor) this);
      this._key.Append(')');
    }

    private void VisitCastOrTreat(DbUnaryExpression e)
    {
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      e.Argument.Accept((DbExpressionVisitor) this);
      this._key.Append(":");
      this._key.Append(e.ResultType.Identity);
      this._key.Append(')');
    }

    public override void Visit(DbExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(e, nameof (e));
      throw new NotSupportedException(Strings.Cqt_General_UnsupportedExpression((object) e.GetType().FullName));
    }

    public override void Visit(DbConstantExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConstantExpression>(e, nameof (e));
      switch (((PrimitiveType) TypeHelpers.GetPrimitiveTypeUsageForScalar(e.ResultType).EdmType).PrimitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          if (!(e.Value is byte[] numArray2))
            throw new NotSupportedException();
          this._key.Append("'");
          for (int index = 0; index < numArray2.Length; ++index)
            this._key.AppendFormat("{0:X2}", (object) numArray2[index]);
          this._key.Append("'");
          break;
        case PrimitiveTypeKind.Boolean:
        case PrimitiveTypeKind.Byte:
        case PrimitiveTypeKind.Decimal:
        case PrimitiveTypeKind.Double:
        case PrimitiveTypeKind.Guid:
        case PrimitiveTypeKind.Single:
        case PrimitiveTypeKind.SByte:
        case PrimitiveTypeKind.Int16:
        case PrimitiveTypeKind.Int32:
        case PrimitiveTypeKind.Int64:
        case PrimitiveTypeKind.Time:
          this._key.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}", e.Value);
          break;
        case PrimitiveTypeKind.DateTime:
          this._key.Append(((DateTime) e.Value).ToString("o", (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case PrimitiveTypeKind.String:
          if (!(e.Value is string str2))
            throw new NotSupportedException();
          this._key.Append("'");
          this._key.Append(str2.Replace("'", "''"));
          this._key.Append("'");
          break;
        case PrimitiveTypeKind.DateTimeOffset:
          this._key.Append(((DateTimeOffset) e.Value).ToString("o", (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case PrimitiveTypeKind.Geometry:
        case PrimitiveTypeKind.GeometryPoint:
        case PrimitiveTypeKind.GeometryLineString:
        case PrimitiveTypeKind.GeometryPolygon:
        case PrimitiveTypeKind.GeometryMultiPoint:
        case PrimitiveTypeKind.GeometryMultiLineString:
        case PrimitiveTypeKind.GeometryMultiPolygon:
        case PrimitiveTypeKind.GeometryCollection:
          if (!(e.Value is DbGeometry dbGeometry2))
            throw new NotSupportedException();
          this._key.Append(dbGeometry2.AsText());
          break;
        case PrimitiveTypeKind.Geography:
        case PrimitiveTypeKind.GeographyPoint:
        case PrimitiveTypeKind.GeographyLineString:
        case PrimitiveTypeKind.GeographyPolygon:
        case PrimitiveTypeKind.GeographyMultiPoint:
        case PrimitiveTypeKind.GeographyMultiLineString:
        case PrimitiveTypeKind.GeographyMultiPolygon:
        case PrimitiveTypeKind.GeographyCollection:
          if (!(e.Value is DbGeography dbGeography2))
            throw new NotSupportedException();
          this._key.Append(dbGeography2.AsText());
          break;
        case PrimitiveTypeKind.HierarchyId:
          HierarchyId hierarchyId = e.Value as HierarchyId;
          if (!(hierarchyId != (HierarchyId) null))
            throw new NotSupportedException();
          this._key.Append((object) hierarchyId);
          break;
        default:
          throw new NotSupportedException();
      }
      this._key.Append(":");
      this._key.Append(e.ResultType.Identity);
    }

    public override void Visit(DbNullExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNullExpression>(e, nameof (e));
      this._key.Append("NULL:");
      this._key.Append(e.ResultType.Identity);
    }

    public override void Visit(DbVariableReferenceExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbVariableReferenceExpression>(e, nameof (e));
      this._key.Append("Var(");
      this.VisitVariableName(e.VariableName);
      this._key.Append(")");
    }

    public override void Visit(DbParameterReferenceExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbParameterReferenceExpression>(e, nameof (e));
      this._key.Append("@");
      this._key.Append(e.ParameterName);
      this._key.Append(":");
      this._key.Append(e.ResultType.Identity);
    }

    public override void Visit(DbFunctionExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFunctionExpression>(e, nameof (e));
      this.VisitFunction(e.Function, e.Arguments);
    }

    public override void Visit(DbLambdaExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLambdaExpression>(expression, nameof (expression));
      this._key.Append("Lambda(");
      foreach (DbVariableReferenceExpression variable in (IEnumerable<DbVariableReferenceExpression>) expression.Lambda.Variables)
      {
        this._key.Append("(V");
        this.VisitVariableName(variable.VariableName);
        this._key.Append(":");
        this._key.Append(variable.ResultType.Identity);
        this._key.Append(')');
      }
      this._key.Append("=");
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) expression.Arguments)
      {
        this._key.Append('(');
        dbExpression.Accept((DbExpressionVisitor) this);
        this._key.Append(')');
      }
      this._key.Append(")Body(");
      expression.Lambda.Body.Accept((DbExpressionVisitor) this);
      this._key.Append(")");
    }

    public override void Visit(DbPropertyExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbPropertyExpression>(e, nameof (e));
      e.Instance.Accept((DbExpressionVisitor) this);
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append(e.Property.Name);
    }

    public override void Visit(DbComparisonExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbComparisonExpression>(e, nameof (e));
      this.VisitBinary((DbBinaryExpression) e);
    }

    public override void Visit(DbLikeExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLikeExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      e.Argument.Accept((DbExpressionVisitor) this);
      this._key.Append(")(");
      e.Pattern.Accept((DbExpressionVisitor) this);
      this._key.Append(")(");
      if (e.Escape != null)
        e.Escape.Accept((DbExpressionVisitor) this);
      e.Argument.Accept((DbExpressionVisitor) this);
      this._key.Append(')');
    }

    public override void Visit(DbLimitExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLimitExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      if (e.WithTies)
        this._key.Append("WithTies");
      this._key.Append('(');
      e.Argument.Accept((DbExpressionVisitor) this);
      this._key.Append(")(");
      e.Limit.Accept((DbExpressionVisitor) this);
      this._key.Append(')');
    }

    public override void Visit(DbIsNullExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsNullExpression>(e, nameof (e));
      this.VisitUnary((DbUnaryExpression) e);
    }

    public override void Visit(DbArithmeticExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbArithmeticExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) e.Arguments)
      {
        this._key.Append('(');
        dbExpression.Accept((DbExpressionVisitor) this);
        this._key.Append(')');
      }
    }

    public override void Visit(DbAndExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbAndExpression>(e, nameof (e));
      this.VisitBinary((DbBinaryExpression) e);
    }

    public override void Visit(DbOrExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOrExpression>(e, nameof (e));
      this.VisitBinary((DbBinaryExpression) e);
    }

    public override void Visit(DbInExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      e.Item.Accept((DbExpressionVisitor) this);
      this._key.Append(",(");
      bool flag = true;
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) e.List)
      {
        if (flag)
          flag = false;
        else
          this._key.Append(',');
        dbExpression.Accept((DbExpressionVisitor) this);
      }
      this._key.Append("))");
    }

    public override void Visit(DbNotExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNotExpression>(e, nameof (e));
      this.VisitUnary((DbUnaryExpression) e);
    }

    public override void Visit(DbDistinctExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDistinctExpression>(e, nameof (e));
      this.VisitUnary((DbUnaryExpression) e);
    }

    public override void Visit(DbElementExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbElementExpression>(e, nameof (e));
      this.VisitUnary((DbUnaryExpression) e);
    }

    public override void Visit(DbIsEmptyExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsEmptyExpression>(e, nameof (e));
      this.VisitUnary((DbUnaryExpression) e);
    }

    public override void Visit(DbUnionAllExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbUnionAllExpression>(e, nameof (e));
      this.VisitBinary((DbBinaryExpression) e);
    }

    public override void Visit(DbIntersectExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIntersectExpression>(e, nameof (e));
      this.VisitBinary((DbBinaryExpression) e);
    }

    public override void Visit(DbExceptExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExceptExpression>(e, nameof (e));
      this.VisitBinary((DbBinaryExpression) e);
    }

    public override void Visit(DbTreatExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbTreatExpression>(e, nameof (e));
      this.VisitCastOrTreat((DbUnaryExpression) e);
    }

    public override void Visit(DbCastExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCastExpression>(e, nameof (e));
      this.VisitCastOrTreat((DbUnaryExpression) e);
    }

    public override void Visit(DbIsOfExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsOfExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      e.Argument.Accept((DbExpressionVisitor) this);
      this._key.Append(":");
      this._key.Append(e.OfType.EdmType.Identity);
      this._key.Append(')');
    }

    public override void Visit(DbOfTypeExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOfTypeExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      e.Argument.Accept((DbExpressionVisitor) this);
      this._key.Append(":");
      this._key.Append(e.OfType.EdmType.Identity);
      this._key.Append(')');
    }

    public override void Visit(DbCaseExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCaseExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      for (int index = 0; index < e.When.Count; ++index)
      {
        this._key.Append("WHEN:(");
        e.When[index].Accept((DbExpressionVisitor) this);
        this._key.Append(")THEN:(");
        e.Then[index].Accept((DbExpressionVisitor) this);
      }
      this._key.Append("ELSE:(");
      e.Else.Accept((DbExpressionVisitor) this);
      this._key.Append("))");
    }

    public override void Visit(DbNewInstanceExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNewInstanceExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append(':');
      this._key.Append(e.ResultType.EdmType.Identity);
      this._key.Append('(');
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) e.Arguments)
      {
        this._key.Append('(');
        dbExpression.Accept((DbExpressionVisitor) this);
        this._key.Append(')');
      }
      if (e.HasRelatedEntityReferences)
      {
        foreach (DbRelatedEntityRef relatedEntityReference in e.RelatedEntityReferences)
        {
          this._key.Append("RE(A(");
          this._key.Append(relatedEntityReference.SourceEnd.DeclaringType.Identity);
          this._key.Append(")(");
          this._key.Append(relatedEntityReference.SourceEnd.Name);
          this._key.Append("->");
          this._key.Append(relatedEntityReference.TargetEnd.Name);
          this._key.Append(")(");
          relatedEntityReference.TargetEntityReference.Accept((DbExpressionVisitor) this);
          this._key.Append("))");
        }
      }
      this._key.Append(')');
    }

    public override void Visit(DbRefExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append("(ESET(");
      this._key.Append(e.EntitySet.EntityContainer.Name);
      this._key.Append('.');
      this._key.Append(e.EntitySet.Name);
      this._key.Append(")T(");
      this._key.Append(TypeHelpers.GetEdmType<RefType>(e.ResultType).ElementType.FullName);
      this._key.Append(")(");
      e.Argument.Accept((DbExpressionVisitor) this);
      this._key.Append(')');
    }

    public override void Visit(DbRelationshipNavigationExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRelationshipNavigationExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      e.NavigationSource.Accept((DbExpressionVisitor) this);
      this._key.Append(")A(");
      this._key.Append(e.NavigateFrom.DeclaringType.Identity);
      this._key.Append(")(");
      this._key.Append(e.NavigateFrom.Name);
      this._key.Append("->");
      this._key.Append(e.NavigateTo.Name);
      this._key.Append("))");
    }

    public override void Visit(DbDerefExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDerefExpression>(e, nameof (e));
      this.VisitUnary((DbUnaryExpression) e);
    }

    public override void Visit(DbRefKeyExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefKeyExpression>(e, nameof (e));
      this.VisitUnary((DbUnaryExpression) e);
    }

    public override void Visit(DbEntityRefExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbEntityRefExpression>(e, nameof (e));
      this.VisitUnary((DbUnaryExpression) e);
    }

    public override void Visit(DbScanExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbScanExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this._key.Append(e.Target.EntityContainer.Name);
      this._key.Append('.');
      this._key.Append(e.Target.Name);
      this._key.Append(':');
      this._key.Append(e.ResultType.EdmType.Identity);
      this._key.Append(')');
    }

    public override void Visit(DbFilterExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFilterExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this.VisitBinding(e.Input);
      this._key.Append('(');
      e.Predicate.Accept((DbExpressionVisitor) this);
      this._key.Append("))");
    }

    public override void Visit(DbProjectExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbProjectExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this.VisitBinding(e.Input);
      this._key.Append('(');
      e.Projection.Accept((DbExpressionVisitor) this);
      this._key.Append("))");
    }

    public override void Visit(DbCrossJoinExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCrossJoinExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      foreach (DbExpressionBinding input in (IEnumerable<DbExpressionBinding>) e.Inputs)
        this.VisitBinding(input);
      this._key.Append(')');
    }

    public override void Visit(DbJoinExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbJoinExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this.VisitBinding(e.Left);
      this.VisitBinding(e.Right);
      this._key.Append('(');
      e.JoinCondition.Accept((DbExpressionVisitor) this);
      this._key.Append("))");
    }

    public override void Visit(DbApplyExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbApplyExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this.VisitBinding(e.Input);
      this.VisitBinding(e.Apply);
      this._key.Append(')');
    }

    public override void Visit(DbGroupByExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGroupByExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this.VisitGroupBinding(e.Input);
      foreach (DbExpression key in (IEnumerable<DbExpression>) e.Keys)
      {
        this._key.Append("K(");
        key.Accept((DbExpressionVisitor) this);
        this._key.Append(')');
      }
      foreach (DbAggregate aggregate in (IEnumerable<DbAggregate>) e.Aggregates)
      {
        if (aggregate is DbGroupAggregate dbGroupAggregate1)
        {
          this._key.Append("GA(");
          dbGroupAggregate1.Arguments[0].Accept((DbExpressionVisitor) this);
          this._key.Append(')');
        }
        else
        {
          this._key.Append("A:");
          DbFunctionAggregate functionAggregate = (DbFunctionAggregate) aggregate;
          if (functionAggregate.Distinct)
            this._key.Append("D:");
          this.VisitFunction(functionAggregate.Function, functionAggregate.Arguments);
        }
      }
      this._key.Append(')');
    }

    private void VisitSortOrder(IList<DbSortClause> sortOrder)
    {
      this._key.Append("SO(");
      foreach (DbSortClause dbSortClause in (IEnumerable<DbSortClause>) sortOrder)
      {
        this._key.Append(dbSortClause.Ascending ? "ASC(" : "DESC(");
        dbSortClause.Expression.Accept((DbExpressionVisitor) this);
        this._key.Append(')');
        if (!string.IsNullOrEmpty(dbSortClause.Collation))
        {
          this._key.Append(":(");
          this._key.Append(dbSortClause.Collation);
          this._key.Append(')');
        }
      }
      this._key.Append(')');
    }

    public override void Visit(DbSkipExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSkipExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this.VisitBinding(e.Input);
      this.VisitSortOrder(e.SortOrder);
      this._key.Append('(');
      e.Count.Accept((DbExpressionVisitor) this);
      this._key.Append("))");
    }

    public override void Visit(DbSortExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSortExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this.VisitBinding(e.Input);
      this.VisitSortOrder(e.SortOrder);
      this._key.Append(')');
    }

    public override void Visit(DbQuantifierExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbQuantifierExpression>(e, nameof (e));
      this.VisitExprKind(e.ExpressionKind);
      this._key.Append('(');
      this.VisitBinding(e.Input);
      this._key.Append('(');
      e.Predicate.Accept((DbExpressionVisitor) this);
      this._key.Append("))");
    }
  }
}
