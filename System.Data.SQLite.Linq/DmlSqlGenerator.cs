// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.DmlSqlGenerator
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Common.CommandTrees;
using System.Data.Metadata.Edm;
using System.Globalization;
using System.Text;

namespace System.Data.SQLite.Linq
{
  internal static class DmlSqlGenerator
  {
    private static readonly int s_commandTextBuilderInitialCapacity = 256;

    internal static string GenerateUpdateSql(
      DbUpdateCommandTree tree,
      out List<DbParameter> parameters)
    {
      StringBuilder commandText = new StringBuilder(DmlSqlGenerator.s_commandTextBuilderInitialCapacity);
      DmlSqlGenerator.ExpressionTranslator translator = new DmlSqlGenerator.ExpressionTranslator(commandText, (DbModificationCommandTree) tree, null != tree.Returning, "UpdateFunction");
      commandText.Append("UPDATE ");
      tree.Target.Expression.Accept((DbExpressionVisitor) translator);
      commandText.AppendLine();
      bool flag = true;
      commandText.Append("SET ");
      foreach (DbSetClause setClause in (IEnumerable<DbModificationClause>) tree.SetClauses)
      {
        if (flag)
          flag = false;
        else
          commandText.Append(", ");
        setClause.Property.Accept((DbExpressionVisitor) translator);
        commandText.Append(" = ");
        setClause.Value.Accept((DbExpressionVisitor) translator);
      }
      if (flag)
      {
        DbParameter parameter = (DbParameter) translator.CreateParameter((object) 0, DbType.Int32);
        commandText.Append(parameter.ParameterName);
        commandText.Append(" = 0");
      }
      commandText.AppendLine();
      commandText.Append("WHERE ");
      tree.Predicate.Accept((DbExpressionVisitor) translator);
      commandText.AppendLine(";");
      DmlSqlGenerator.GenerateReturningSql(commandText, (DbModificationCommandTree) tree, translator, tree.Returning, false);
      parameters = translator.Parameters;
      return commandText.ToString();
    }

    internal static string GenerateDeleteSql(
      DbDeleteCommandTree tree,
      out List<DbParameter> parameters)
    {
      StringBuilder commandText = new StringBuilder(DmlSqlGenerator.s_commandTextBuilderInitialCapacity);
      DmlSqlGenerator.ExpressionTranslator expressionTranslator = new DmlSqlGenerator.ExpressionTranslator(commandText, (DbModificationCommandTree) tree, false, "DeleteFunction");
      commandText.Append("DELETE FROM ");
      tree.Target.Expression.Accept((DbExpressionVisitor) expressionTranslator);
      commandText.AppendLine();
      commandText.Append("WHERE ");
      tree.Predicate.Accept((DbExpressionVisitor) expressionTranslator);
      parameters = expressionTranslator.Parameters;
      commandText.AppendLine(";");
      return commandText.ToString();
    }

    internal static string GenerateInsertSql(
      DbInsertCommandTree tree,
      out List<DbParameter> parameters)
    {
      StringBuilder commandText = new StringBuilder(DmlSqlGenerator.s_commandTextBuilderInitialCapacity);
      DmlSqlGenerator.ExpressionTranslator translator = new DmlSqlGenerator.ExpressionTranslator(commandText, (DbModificationCommandTree) tree, null != tree.Returning, "InsertFunction");
      commandText.Append("INSERT INTO ");
      tree.Target.Expression.Accept((DbExpressionVisitor) translator);
      if (tree.SetClauses.Count > 0)
      {
        commandText.Append("(");
        bool flag1 = true;
        foreach (DbSetClause setClause in (IEnumerable<DbModificationClause>) tree.SetClauses)
        {
          if (flag1)
            flag1 = false;
          else
            commandText.Append(", ");
          setClause.Property.Accept((DbExpressionVisitor) translator);
        }
        commandText.AppendLine(")");
        bool flag2 = true;
        commandText.Append(" VALUES (");
        foreach (DbSetClause setClause in (IEnumerable<DbModificationClause>) tree.SetClauses)
        {
          if (flag2)
            flag2 = false;
          else
            commandText.Append(", ");
          setClause.Value.Accept((DbExpressionVisitor) translator);
          translator.RegisterMemberValue(setClause.Property, setClause.Value);
        }
        commandText.AppendLine(");");
      }
      else
        commandText.AppendLine(" DEFAULT VALUES;");
      DmlSqlGenerator.GenerateReturningSql(commandText, (DbModificationCommandTree) tree, translator, tree.Returning, true);
      parameters = translator.Parameters;
      return commandText.ToString();
    }

    private static string GenerateMemberTSql(EdmMember member) => SqlGenerator.QuoteIdentifier(member.Name);

    private static bool IsIntegerPrimaryKey(
      EntitySetBase table,
      out ReadOnlyMetadataCollection<EdmMember> keyMembers,
      out EdmMember primaryKeyMember)
    {
      keyMembers = table.ElementType.KeyMembers;
      if (keyMembers.Count == 1)
      {
        EdmMember edmMember = keyMembers[0];
        PrimitiveTypeKind typeKind;
        if (MetadataHelpers.TryGetPrimitiveTypeKind(edmMember.TypeUsage, out typeKind) && typeKind == PrimitiveTypeKind.Int64)
        {
          primaryKeyMember = edmMember;
          return true;
        }
      }
      primaryKeyMember = (EdmMember) null;
      return false;
    }

    private static bool DoAllKeyMembersHaveValues(
      DmlSqlGenerator.ExpressionTranslator translator,
      ReadOnlyMetadataCollection<EdmMember> keyMembers,
      out EdmMember missingKeyMember)
    {
      foreach (EdmMember keyMember in keyMembers)
      {
        if (!translator.MemberValues.ContainsKey(keyMember))
        {
          missingKeyMember = keyMember;
          return false;
        }
      }
      missingKeyMember = (EdmMember) null;
      return true;
    }

    private static void GenerateReturningSql(
      StringBuilder commandText,
      DbModificationCommandTree tree,
      DmlSqlGenerator.ExpressionTranslator translator,
      DbExpression returning,
      bool wasInsert)
    {
      if (returning == null)
        return;
      commandText.Append("SELECT ");
      returning.Accept((DbExpressionVisitor) translator);
      commandText.AppendLine();
      commandText.Append("FROM ");
      tree.Target.Expression.Accept((DbExpressionVisitor) translator);
      commandText.AppendLine();
      commandText.Append("WHERE last_rows_affected() > 0");
      EntitySetBase target = ((DbScanExpression) tree.Target.Expression).Target;
      ReadOnlyMetadataCollection<EdmMember> keyMembers;
      EdmMember primaryKeyMember;
      if (DmlSqlGenerator.IsIntegerPrimaryKey(target, out keyMembers, out primaryKeyMember))
      {
        commandText.Append(" AND ");
        commandText.Append(DmlSqlGenerator.GenerateMemberTSql(primaryKeyMember));
        commandText.Append(" = ");
        DbParameter dbParameter;
        if (translator.MemberValues.TryGetValue(primaryKeyMember, out dbParameter))
        {
          commandText.Append(dbParameter.ParameterName);
        }
        else
        {
          if (!wasInsert)
            throw new NotSupportedException(string.Format("Missing value for INSERT key member '{0}' in table '{1}'.", primaryKeyMember != null ? (object) primaryKeyMember.Name : (object) "<unknown>", (object) target.Name));
          commandText.AppendLine("last_insert_rowid()");
        }
      }
      else
      {
        EdmMember missingKeyMember;
        if (DmlSqlGenerator.DoAllKeyMembersHaveValues(translator, keyMembers, out missingKeyMember))
        {
          foreach (EdmMember edmMember in keyMembers)
          {
            commandText.Append(" AND ");
            commandText.Append(DmlSqlGenerator.GenerateMemberTSql(edmMember));
            commandText.Append(" = ");
            DbParameter dbParameter;
            if (!translator.MemberValues.TryGetValue(edmMember, out dbParameter))
              throw new NotSupportedException(string.Format("Missing value for {0} key member '{1}' in table '{2}' (internal).", wasInsert ? (object) "INSERT" : (object) "UPDATE", edmMember != null ? (object) edmMember.Name : (object) "<unknown>", (object) target.Name));
            commandText.Append(dbParameter.ParameterName);
          }
        }
        else
        {
          if (!wasInsert)
            throw new NotSupportedException(string.Format("Missing value for UPDATE key member '{0}' in table '{1}'.", missingKeyMember != null ? (object) missingKeyMember.Name : (object) "<unknown>", (object) target.Name));
          commandText.Append(" AND ");
          commandText.Append(SqlGenerator.QuoteIdentifier("rowid"));
          commandText.Append(" = ");
          commandText.AppendLine("last_insert_rowid()");
        }
      }
      commandText.AppendLine(";");
    }

    private class ExpressionTranslator : DbExpressionVisitor
    {
      private readonly StringBuilder _commandText;
      private readonly DbModificationCommandTree _commandTree;
      private readonly List<DbParameter> _parameters;
      private readonly Dictionary<EdmMember, DbParameter> _memberValues;
      private int parameterNameCount;
      private string _kind;

      internal ExpressionTranslator(
        StringBuilder commandText,
        DbModificationCommandTree commandTree,
        bool preserveMemberValues,
        string kind)
      {
        this._kind = kind;
        this._commandText = commandText;
        this._commandTree = commandTree;
        this._parameters = new List<DbParameter>();
        this._memberValues = preserveMemberValues ? new Dictionary<EdmMember, DbParameter>() : (Dictionary<EdmMember, DbParameter>) null;
      }

      internal List<DbParameter> Parameters => this._parameters;

      internal Dictionary<EdmMember, DbParameter> MemberValues => this._memberValues;

      internal SQLiteParameter CreateParameter(object value, TypeUsage type)
      {
        DbType dbType = MetadataHelpers.GetDbType(MetadataHelpers.GetPrimitiveTypeKind(type));
        return this.CreateParameter(value, dbType);
      }

      internal SQLiteParameter CreateParameter(object value, DbType dbType)
      {
        string parameterName = "@p" + this.parameterNameCount.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        ++this.parameterNameCount;
        SQLiteParameter sqLiteParameter = new SQLiteParameter(parameterName, value);
        sqLiteParameter.DbType = dbType;
        this._parameters.Add((DbParameter) sqLiteParameter);
        return sqLiteParameter;
      }

      public override void Visit(DbApplyExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionBindingPre(expression.Input);
        if (expression.Apply != null)
          this.VisitExpression(expression.Apply.Expression);
        this.VisitExpressionBindingPost(expression.Input);
      }

      public override void Visit(DbArithmeticExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionList(expression.Arguments);
      }

      public override void Visit(DbCaseExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionList(expression.When);
        this.VisitExpressionList(expression.Then);
        this.VisitExpression(expression.Else);
      }

      public override void Visit(DbCastExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbCrossJoinExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        foreach (DbExpressionBinding input in (IEnumerable<DbExpressionBinding>) expression.Inputs)
          this.VisitExpressionBindingPre(input);
        foreach (DbExpressionBinding input in (IEnumerable<DbExpressionBinding>) expression.Inputs)
          this.VisitExpressionBindingPost(input);
      }

      public override void Visit(DbDerefExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbDistinctExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbElementExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbEntityRefExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbExceptExpression expression) => this.VisitBinary((DbBinaryExpression) expression);

      protected virtual void VisitBinary(DbBinaryExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpression(expression.Left);
        this.VisitExpression(expression.Right);
      }

      public override void Visit(DbExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        throw new NotSupportedException("DbExpression");
      }

      public override void Visit(DbFilterExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionBindingPre(expression.Input);
        this.VisitExpression(expression.Predicate);
        this.VisitExpressionBindingPost(expression.Input);
      }

      public override void Visit(DbFunctionExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionList(expression.Arguments);
      }

      public override void Visit(DbGroupByExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitGroupExpressionBindingPre(expression.Input);
        this.VisitExpressionList(expression.Keys);
        this.VisitGroupExpressionBindingMid(expression.Input);
        this.VisitAggregateList(expression.Aggregates);
        this.VisitGroupExpressionBindingPost(expression.Input);
      }

      public override void Visit(DbIntersectExpression expression) => this.VisitBinary((DbBinaryExpression) expression);

      public override void Visit(DbIsEmptyExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbIsOfExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbJoinExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionBindingPre(expression.Left);
        this.VisitExpressionBindingPre(expression.Right);
        this.VisitExpression(expression.JoinCondition);
        this.VisitExpressionBindingPost(expression.Left);
        this.VisitExpressionBindingPost(expression.Right);
      }

      public override void Visit(DbLikeExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpression(expression.Argument);
        this.VisitExpression(expression.Pattern);
        this.VisitExpression(expression.Escape);
      }

      public override void Visit(DbLimitExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpression(expression.Argument);
        this.VisitExpression(expression.Limit);
      }

      public override void Visit(DbOfTypeExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbParameterReferenceExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
      }

      public override void Visit(DbProjectExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionBindingPre(expression.Input);
        this.VisitExpression(expression.Projection);
        this.VisitExpressionBindingPost(expression.Input);
      }

      public override void Visit(DbQuantifierExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionBindingPre(expression.Input);
        this.VisitExpression(expression.Predicate);
        this.VisitExpressionBindingPost(expression.Input);
      }

      public override void Visit(DbRefExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbRefKeyExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbRelationshipNavigationExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpression(expression.NavigationSource);
      }

      public override void Visit(DbSkipExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionBindingPre(expression.Input);
        foreach (DbSortClause dbSortClause in (IEnumerable<DbSortClause>) expression.SortOrder)
          this.VisitExpression(dbSortClause.Expression);
        this.VisitExpressionBindingPost(expression.Input);
        this.VisitExpression(expression.Count);
      }

      public override void Visit(DbSortExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpressionBindingPre(expression.Input);
        for (int index = 0; index < expression.SortOrder.Count; ++index)
          this.VisitExpression(expression.SortOrder[index].Expression);
        this.VisitExpressionBindingPost(expression.Input);
      }

      public override void Visit(DbTreatExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

      public override void Visit(DbUnionAllExpression expression) => this.VisitBinary((DbBinaryExpression) expression);

      public override void Visit(DbVariableReferenceExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
      }

      public virtual void VisitAggregate(DbAggregate aggregate)
      {
        if (aggregate == null)
          throw new ArgumentException(nameof (aggregate));
        this.VisitExpressionList(aggregate.Arguments);
      }

      public virtual void VisitAggregateList(IList<DbAggregate> aggregates)
      {
        if (aggregates == null)
          throw new ArgumentException(nameof (aggregates));
        for (int index = 0; index < aggregates.Count; ++index)
          this.VisitAggregate(aggregates[index]);
      }

      public virtual void VisitExpression(DbExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        expression.Accept((DbExpressionVisitor) this);
      }

      protected virtual void VisitExpressionBindingPost(DbExpressionBinding binding)
      {
      }

      protected virtual void VisitExpressionBindingPre(DbExpressionBinding binding)
      {
        if (binding == null)
          throw new ArgumentException(nameof (binding));
        this.VisitExpression(binding.Expression);
      }

      public virtual void VisitExpressionList(IList<DbExpression> expressionList)
      {
        if (expressionList == null)
          throw new ArgumentException(nameof (expressionList));
        for (int index = 0; index < expressionList.Count; ++index)
          this.VisitExpression(expressionList[index]);
      }

      protected virtual void VisitGroupExpressionBindingMid(DbGroupExpressionBinding binding)
      {
      }

      protected virtual void VisitGroupExpressionBindingPost(DbGroupExpressionBinding binding)
      {
      }

      protected virtual void VisitGroupExpressionBindingPre(DbGroupExpressionBinding binding)
      {
        if (binding == null)
          throw new ArgumentException(nameof (binding));
        this.VisitExpression(binding.Expression);
      }

      protected virtual void VisitLambdaFunctionPost(EdmFunction function, DbExpression body)
      {
      }

      protected virtual void VisitLambdaFunctionPre(EdmFunction function, DbExpression body)
      {
        if (function == null)
          throw new ArgumentException(nameof (function));
        if (body == null)
          throw new ArgumentException(nameof (body));
      }

      protected virtual void VisitUnaryExpression(DbUnaryExpression expression)
      {
        if (expression == null)
          throw new ArgumentException(nameof (expression));
        this.VisitExpression(expression.Argument);
      }

      public override void Visit(DbAndExpression expression) => this.VisitBinary((DbBinaryExpression) expression, " AND ");

      public override void Visit(DbOrExpression expression) => this.VisitBinary((DbBinaryExpression) expression, " OR ");

      public override void Visit(DbComparisonExpression expression)
      {
        this.VisitBinary((DbBinaryExpression) expression, " = ");
        this.RegisterMemberValue(expression.Left, expression.Right);
      }

      internal void RegisterMemberValue(DbExpression propertyExpression, DbExpression value)
      {
        if (this._memberValues == null)
          return;
        EdmMember property = ((DbPropertyExpression) propertyExpression).Property;
        if (value.ExpressionKind == DbExpressionKind.Null)
          return;
        this._memberValues[property] = this._parameters[this._parameters.Count - 1];
      }

      public override void Visit(DbIsNullExpression expression)
      {
        expression.Argument.Accept((DbExpressionVisitor) this);
        this._commandText.Append(" IS NULL");
      }

      public override void Visit(DbNotExpression expression)
      {
        this._commandText.Append("NOT (");
        expression.Accept((DbExpressionVisitor) this);
        this._commandText.Append(")");
      }

      public override void Visit(DbConstantExpression expression) => this._commandText.Append(this.CreateParameter(expression.Value, expression.ResultType).ParameterName);

      public override void Visit(DbScanExpression expression)
      {
        if (MetadataHelpers.TryGetValueForMetadataProperty<string>((MetadataItem) expression.Target, "DefiningQuery") != null)
          throw new NotSupportedException(string.Format("Unable to update the EntitySet '{0}' because it has a DefiningQuery and no <{1}> element exists in the <ModificationFunctionMapping> element to support the current operation.", (object) expression.Target.Name, (object) this._kind));
        this._commandText.Append(SqlGenerator.GetTargetTSql(expression.Target));
      }

      public override void Visit(DbPropertyExpression expression) => this._commandText.Append(DmlSqlGenerator.GenerateMemberTSql(expression.Property));

      public override void Visit(DbNullExpression expression) => this._commandText.Append("NULL");

      public override void Visit(DbNewInstanceExpression expression)
      {
        bool flag = true;
        foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) expression.Arguments)
        {
          if (flag)
            flag = false;
          else
            this._commandText.Append(", ");
          dbExpression.Accept((DbExpressionVisitor) this);
        }
      }

      private void VisitBinary(DbBinaryExpression expression, string separator)
      {
        this._commandText.Append("(");
        expression.Left.Accept((DbExpressionVisitor) this);
        this._commandText.Append(separator);
        expression.Right.Accept((DbExpressionVisitor) this);
        this._commandText.Append(")");
      }
    }
  }
}
