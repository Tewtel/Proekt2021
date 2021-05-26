// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.SqlGenerator
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.Data.SQLite.EF6
{
  internal sealed class SqlGenerator : DbExpressionVisitor<ISqlFragment>
  {
    private SQLiteProviderManifest _manifest;
    private Stack<SqlSelectStatement> selectStatementStack;
    private Stack<bool> isParentAJoinStack;
    private Dictionary<string, int> allExtentNames;
    private Dictionary<string, int> allColumnNames;
    private SymbolTable symbolTable = new SymbolTable();
    private bool isVarRefSingle;
    private static readonly Dictionary<string, SqlGenerator.FunctionHandler> _builtInFunctionHandlers = SqlGenerator.InitializeBuiltInFunctionHandlers();
    private static readonly Dictionary<string, SqlGenerator.FunctionHandler> _canonicalFunctionHandlers = SqlGenerator.InitializeCanonicalFunctionHandlers();
    private static readonly Dictionary<string, string> _functionNameToOperatorDictionary = SqlGenerator.InitializeFunctionNameToOperatorDictionary();
    private static readonly Dictionary<string, string> _datepartKeywords = SqlGenerator.InitializeDatepartKeywords();
    private static readonly char[] hexDigits = new char[16]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
    };

    private SqlSelectStatement CurrentSelectStatement => this.selectStatementStack.Peek();

    private bool IsParentAJoin => this.isParentAJoinStack.Count != 0 && this.isParentAJoinStack.Peek();

    internal Dictionary<string, int> AllExtentNames => this.allExtentNames;

    internal Dictionary<string, int> AllColumnNames => this.allColumnNames;

    private bool HasBuiltMapForIn(DbExpression e, KeyToListMap<DbExpression, DbExpression> values)
    {
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Equals:
          return this.TryAddExpressionForIn((DbBinaryExpression) e, values);
        case DbExpressionKind.IsNull:
          DbExpression dbExpression = ((DbUnaryExpression) e).Argument;
          if (!this.IsKeyForIn(dbExpression))
            return false;
          values.Add(dbExpression, e);
          return true;
        case DbExpressionKind.Or:
          DbBinaryExpression binaryExpression = e as DbBinaryExpression;
          return this.HasBuiltMapForIn(binaryExpression.Left, values) && this.HasBuiltMapForIn(binaryExpression.Right, values);
        default:
          return false;
      }
    }

    private static Dictionary<string, SqlGenerator.FunctionHandler> InitializeBuiltInFunctionHandlers() => new Dictionary<string, SqlGenerator.FunctionHandler>(7, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "CONCAT",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleConcatFunction)
      },
      {
        "DATEPART",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleDatepartDateFunction)
      },
      {
        "DatePart",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleDatepartDateFunction)
      },
      {
        "GETDATE",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleGetDateFunction)
      },
      {
        "GETUTCDATE",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleGetUtcDateFunction)
      }
    };

    private static Dictionary<string, SqlGenerator.FunctionHandler> InitializeCanonicalFunctionHandlers() => new Dictionary<string, SqlGenerator.FunctionHandler>(16, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "IndexOf",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionIndexOf)
      },
      {
        "Length",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionLength)
      },
      {
        "NewGuid",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionNewGuid)
      },
      {
        "Round",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionRound)
      },
      {
        "ToLower",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionToLower)
      },
      {
        "ToUpper",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionToUpper)
      },
      {
        "Trim",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionTrim)
      },
      {
        "Left",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionLeft)
      },
      {
        "Right",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionRight)
      },
      {
        "Substring",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionSubstring)
      },
      {
        "CurrentDateTime",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleGetDateFunction)
      },
      {
        "CurrentUtcDateTime",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleGetUtcDateFunction)
      },
      {
        "Year",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDatepart)
      },
      {
        "Month",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDatepart)
      },
      {
        "Day",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDatepart)
      },
      {
        "Hour",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDatepart)
      },
      {
        "Minute",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDatepart)
      },
      {
        "Second",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDatepart)
      },
      {
        "DateAdd",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDateAdd)
      },
      {
        "DateDiff",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDateSubtract)
      },
      {
        "DATEADD",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDateAdd)
      },
      {
        "DATEDIFF",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionDateSubtract)
      },
      {
        "Concat",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleConcatFunction)
      },
      {
        "BitwiseAnd",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionBitwise)
      },
      {
        "BitwiseNot",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionBitwise)
      },
      {
        "BitwiseOr",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionBitwise)
      },
      {
        "BitwiseXor",
        new SqlGenerator.FunctionHandler(SqlGenerator.HandleCanonicalFunctionBitwise)
      }
    };

    private static Dictionary<string, string> InitializeDatepartKeywords() => new Dictionary<string, string>(30, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "d",
        "%d"
      },
      {
        "day",
        "%d"
      },
      {
        "dayofyear",
        "%j"
      },
      {
        "dd",
        "%d"
      },
      {
        "dw",
        "%w"
      },
      {
        "dy",
        "%j"
      },
      {
        "hh",
        "%H"
      },
      {
        "hour",
        "%H"
      },
      {
        "m",
        "%m"
      },
      {
        "mi",
        "%M"
      },
      {
        "millisecond",
        "%f"
      },
      {
        "minute",
        "%M"
      },
      {
        "mm",
        "%m"
      },
      {
        "month",
        "%m"
      },
      {
        "ms",
        "%f"
      },
      {
        "n",
        "%M"
      },
      {
        "s",
        "%S"
      },
      {
        "second",
        "%S"
      },
      {
        "ss",
        "%S"
      },
      {
        "week",
        "%W"
      },
      {
        "weekday",
        "%w"
      },
      {
        "wk",
        "%W"
      },
      {
        "ww",
        "%W"
      },
      {
        "y",
        "%Y"
      },
      {
        "year",
        "%Y"
      },
      {
        "yy",
        "%Y"
      },
      {
        "yyyy",
        "%Y"
      }
    };

    private static Dictionary<string, string> InitializeFunctionNameToOperatorDictionary() => new Dictionary<string, string>(5, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "Concat",
        "||"
      },
      {
        "CONCAT",
        "||"
      },
      {
        "BitwiseAnd",
        "&"
      },
      {
        "BitwiseNot",
        "~"
      },
      {
        "BitwiseOr",
        "|"
      },
      {
        "BitwiseXor",
        "^"
      }
    };

    private SqlGenerator(SQLiteProviderManifest manifest) => this._manifest = manifest;

    internal static string GenerateSql(
      SQLiteProviderManifest manifest,
      DbCommandTree tree,
      out List<DbParameter> parameters,
      out CommandType commandType)
    {
      commandType = CommandType.Text;
      switch (tree)
      {
        case DbQueryCommandTree _:
          SqlGenerator sqlGenerator1 = new SqlGenerator(manifest);
          parameters = (List<DbParameter>) null;
          return sqlGenerator1.GenerateSql((DbQueryCommandTree) tree);
        case DbFunctionCommandTree tree1:
          SqlGenerator sqlGenerator2 = new SqlGenerator(manifest);
          parameters = (List<DbParameter>) null;
          return sqlGenerator2.GenerateFunctionSql(tree1, out commandType);
        case DbInsertCommandTree tree2:
          return DmlSqlGenerator.GenerateInsertSql(tree2, out parameters);
        case DbDeleteCommandTree tree3:
          return DmlSqlGenerator.GenerateDeleteSql(tree3, out parameters);
        case DbUpdateCommandTree tree4:
          return DmlSqlGenerator.GenerateUpdateSql(tree4, out parameters);
        default:
          throw new NotSupportedException("Unrecognized command tree type");
      }
    }

    private string GenerateSql(DbQueryCommandTree tree)
    {
      this.selectStatementStack = new Stack<SqlSelectStatement>();
      this.isParentAJoinStack = new Stack<bool>();
      this.allExtentNames = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.allColumnNames = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      ISqlFragment sqlStatement;
      if (MetadataHelpers.IsCollectionType(tree.Query.ResultType))
      {
        SqlSelectStatement sqlSelectStatement = this.VisitExpressionEnsureSqlStatement(tree.Query);
        sqlSelectStatement.IsTopMost = true;
        sqlStatement = (ISqlFragment) sqlSelectStatement;
      }
      else
      {
        SqlBuilder sqlBuilder = new SqlBuilder();
        sqlBuilder.Append((object) "SELECT ");
        sqlBuilder.Append((object) tree.Query.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        sqlStatement = (ISqlFragment) sqlBuilder;
      }
      if (this.isVarRefSingle)
        throw new NotSupportedException();
      return this.WriteSql(sqlStatement);
    }

    private string GenerateFunctionSql(DbFunctionCommandTree tree, out CommandType commandType)
    {
      EdmFunction edmFunction = tree.EdmFunction;
      string str1 = (string) edmFunction.MetadataProperties["CommandTextAttribute"].Value;
      string str2 = (string) edmFunction.MetadataProperties["StoreFunctionNameAttribute"].Value;
      if (string.IsNullOrEmpty(str1))
      {
        commandType = CommandType.StoredProcedure;
        return SqlGenerator.QuoteIdentifier(string.IsNullOrEmpty(str2) ? edmFunction.Name : str2);
      }
      commandType = CommandType.Text;
      return str1;
    }

    private string WriteSql(ISqlFragment sqlStatement)
    {
      StringBuilder b = new StringBuilder(1024);
      using (SqlWriter writer = new SqlWriter(b))
        sqlStatement.WriteSql(writer, this);
      return b.ToString();
    }

    private bool TryTranslateIntoIn(DbOrExpression e, out ISqlFragment sqlFragment)
    {
      KeyToListMap<DbExpression, DbExpression> values = new KeyToListMap<DbExpression, DbExpression>((IEqualityComparer<DbExpression>) SqlGenerator.KeyFieldExpressionComparer.Singleton);
      if (!this.HasBuiltMapForIn((DbExpression) e, values) || values.Keys.Count<DbExpression>() <= 0)
      {
        sqlFragment = (ISqlFragment) null;
        return false;
      }
      SqlBuilder sqlBuilder = new SqlBuilder();
      bool flag1 = true;
      foreach (DbExpression key in values.Keys)
      {
        ReadOnlyCollection<DbExpression> source1 = values.ListForKey(key);
        if (!flag1)
          sqlBuilder.Append((object) " OR ");
        else
          flag1 = false;
        IEnumerable<DbExpression> source2 = source1.Where<DbExpression>((Func<DbExpression, bool>) (v => v.ExpressionKind != DbExpressionKind.IsNull));
        int num = source2.Count<DbExpression>();
        if (num == 1)
        {
          this.ParanthesizeExpressionIfNeeded(key, sqlBuilder);
          sqlBuilder.Append((object) " = ");
          this.ParenthesizeExpressionWithoutRedundantConstantCasts(source2.First<DbExpression>(), sqlBuilder);
        }
        if (num > 1)
        {
          this.ParanthesizeExpressionIfNeeded(key, sqlBuilder);
          sqlBuilder.Append((object) " IN (");
          bool flag2 = true;
          foreach (DbExpression dbExpression in source2)
          {
            if (!flag2)
              sqlBuilder.Append((object) ",");
            else
              flag2 = false;
            this.ParenthesizeExpressionWithoutRedundantConstantCasts(dbExpression, sqlBuilder);
          }
          sqlBuilder.Append((object) ")");
        }
        if (source1.FirstOrDefault<DbExpression>((Func<DbExpression, bool>) (v => v.ExpressionKind == DbExpressionKind.IsNull)) is DbIsNullExpression e3)
        {
          if (num > 0)
            sqlBuilder.Append((object) " OR ");
          sqlBuilder.Append((object) this.VisitIsNullExpression(e3, false));
        }
      }
      sqlFragment = (ISqlFragment) sqlBuilder;
      return true;
    }

    public override ISqlFragment Visit(DbAndExpression e) => (ISqlFragment) this.VisitBinaryExpression(" AND ", e.Left, e.Right);

    public override ISqlFragment Visit(DbApplyExpression e) => throw new NotSupportedException("APPLY joins are not supported");

    public override ISqlFragment Visit(DbArithmeticExpression e)
    {
      SqlBuilder sqlBuilder;
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Divide:
          sqlBuilder = this.VisitBinaryExpression(" / ", e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.Minus:
          sqlBuilder = this.VisitBinaryExpression(" - ", e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.Modulo:
          sqlBuilder = this.VisitBinaryExpression(" % ", e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.Multiply:
          sqlBuilder = this.VisitBinaryExpression(" * ", e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.Plus:
          sqlBuilder = this.VisitBinaryExpression(" + ", e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.UnaryMinus:
          sqlBuilder = new SqlBuilder();
          sqlBuilder.Append((object) " -(");
          sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
          sqlBuilder.Append((object) ")");
          break;
        default:
          throw new InvalidOperationException();
      }
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbCaseExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "CASE");
      for (int index = 0; index < e.When.Count; ++index)
      {
        sqlBuilder.Append((object) " WHEN (");
        sqlBuilder.Append((object) e.When[index].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        sqlBuilder.Append((object) ") THEN ");
        sqlBuilder.Append((object) e.Then[index].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      }
      if (e.Else != null && !(e.Else is DbNullExpression))
      {
        sqlBuilder.Append((object) " ELSE ");
        sqlBuilder.Append((object) e.Else.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      }
      sqlBuilder.Append((object) " END");
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbCastExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbComparisonExpression e)
    {
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Equals:
          return (ISqlFragment) this.VisitBinaryExpression(" = ", e.Left, e.Right);
        case DbExpressionKind.GreaterThan:
          return (ISqlFragment) this.VisitBinaryExpression(" > ", e.Left, e.Right);
        case DbExpressionKind.GreaterThanOrEquals:
          return (ISqlFragment) this.VisitBinaryExpression(" >= ", e.Left, e.Right);
        case DbExpressionKind.LessThan:
          return (ISqlFragment) this.VisitBinaryExpression(" < ", e.Left, e.Right);
        case DbExpressionKind.LessThanOrEquals:
          return (ISqlFragment) this.VisitBinaryExpression(" <= ", e.Left, e.Right);
        case DbExpressionKind.NotEquals:
          return (ISqlFragment) this.VisitBinaryExpression(" <> ", e.Left, e.Right);
        default:
          throw new InvalidOperationException();
      }
    }

    public override ISqlFragment Visit(DbConstantExpression e)
    {
      SqlBuilder builder = new SqlBuilder();
      PrimitiveTypeKind typeKind;
      if (!MetadataHelpers.TryGetPrimitiveTypeKind(e.ResultType, out typeKind))
        throw new NotSupportedException();
      switch (typeKind)
      {
        case PrimitiveTypeKind.Binary:
          SqlGenerator.ToBlobLiteral((byte[]) e.Value, builder);
          break;
        case PrimitiveTypeKind.Boolean:
          builder.Append((bool) e.Value ? (object) "1" : (object) "0");
          break;
        case PrimitiveTypeKind.Byte:
          builder.Append((object) e.Value.ToString());
          break;
        case PrimitiveTypeKind.DateTime:
          bool flag = SqlGenerator.NeedSingleQuotes(this._manifest._dateTimeFormat);
          string s = SQLiteConvert.ToString((DateTime) e.Value, this._manifest._dateTimeFormat, this._manifest._dateTimeKind, this._manifest._dateTimeFormatString);
          if (flag)
          {
            builder.Append((object) SqlGenerator.EscapeSingleQuote(s, false));
            break;
          }
          builder.Append((object) s);
          break;
        case PrimitiveTypeKind.Decimal:
          string str = ((Decimal) e.Value).ToString((IFormatProvider) CultureInfo.InvariantCulture);
          if (-1 == str.IndexOf('.'))
          {
            if (str.TrimStart('-').Length < 20)
            {
              byte length = (byte) str.Length;
              FacetDescription facetDescription;
              if (MetadataHelpers.TryGetTypeFacetDescriptionByName(e.ResultType.EdmType, "precision", out facetDescription) && facetDescription.DefaultValue != null)
                Math.Max(length, (byte) facetDescription.DefaultValue);
              builder.Append((object) str);
              break;
            }
          }
          builder.Append((object) str);
          break;
        case PrimitiveTypeKind.Double:
          builder.Append((object) ((double) e.Value).ToString((IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case PrimitiveTypeKind.Guid:
          object obj = e.Value;
          if (this._manifest._binaryGuid && obj is Guid guid2)
          {
            SqlGenerator.ToBlobLiteral(guid2.ToByteArray(), builder);
            break;
          }
          builder.Append((object) SqlGenerator.EscapeSingleQuote(e.Value.ToString(), false));
          break;
        case PrimitiveTypeKind.Single:
          builder.Append((object) ((float) e.Value).ToString((IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case PrimitiveTypeKind.Int16:
          builder.Append((object) e.Value.ToString());
          break;
        case PrimitiveTypeKind.Int32:
          builder.Append((object) e.Value.ToString());
          break;
        case PrimitiveTypeKind.Int64:
          builder.Append((object) e.Value.ToString());
          break;
        case PrimitiveTypeKind.String:
          bool facetValueOrDefault = MetadataHelpers.GetFacetValueOrDefault<bool>(e.ResultType, MetadataHelpers.UnicodeFacetName, true);
          builder.Append((object) SqlGenerator.EscapeSingleQuote(e.Value as string, facetValueOrDefault));
          break;
        case PrimitiveTypeKind.Time:
          throw new NotSupportedException("time");
        case PrimitiveTypeKind.DateTimeOffset:
          throw new NotSupportedException("datetimeoffset");
        default:
          throw new NotSupportedException();
      }
      return (ISqlFragment) builder;
    }

    public override ISqlFragment Visit(DbDerefExpression e) => throw new NotSupportedException();

    public override ISqlFragment Visit(DbDistinctExpression e)
    {
      SqlSelectStatement sqlSelectStatement = this.VisitExpressionEnsureSqlStatement(e.Argument);
      if (!this.IsCompatible(sqlSelectStatement, e.ExpressionKind))
      {
        TypeUsage elementTypeUsage = MetadataHelpers.GetElementTypeUsage(e.Argument.ResultType);
        Symbol fromSymbol;
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, "DISTINCT", elementTypeUsage, out fromSymbol);
        this.AddFromSymbol(sqlSelectStatement, "DISTINCT", fromSymbol, false);
      }
      sqlSelectStatement.IsDistinct = true;
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbElementExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "(");
      sqlBuilder.Append((object) this.VisitExpressionEnsureSqlStatement(e.Argument));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbExceptExpression e) => this.VisitSetOpExpression(e.Left, e.Right, "EXCEPT");

    public override ISqlFragment Visit(DbExpression e) => throw new InvalidOperationException();

    public override ISqlFragment Visit(DbScanExpression e)
    {
      EntitySetBase target = e.Target;
      if (this.IsParentAJoin)
      {
        SqlBuilder sqlBuilder = new SqlBuilder();
        sqlBuilder.Append((object) SqlGenerator.GetTargetTSql(target));
        return (ISqlFragment) sqlBuilder;
      }
      SqlSelectStatement sqlSelectStatement = new SqlSelectStatement();
      sqlSelectStatement.From.Append((object) SqlGenerator.GetTargetTSql(target));
      return (ISqlFragment) sqlSelectStatement;
    }

    internal static string GetTargetTSql(EntitySetBase entitySetBase)
    {
      StringBuilder stringBuilder = new StringBuilder(50);
      string metadataProperty1 = MetadataHelpers.TryGetValueForMetadataProperty<string>((MetadataItem) entitySetBase, "DefiningQuery");
      if (!string.IsNullOrEmpty(metadataProperty1))
      {
        stringBuilder.Append("(");
        stringBuilder.Append(metadataProperty1);
        stringBuilder.Append(")");
      }
      else
      {
        string metadataProperty2 = MetadataHelpers.TryGetValueForMetadataProperty<string>((MetadataItem) entitySetBase, "Table");
        if (!string.IsNullOrEmpty(metadataProperty2))
          stringBuilder.Append(SqlGenerator.QuoteIdentifier(metadataProperty2));
        else
          stringBuilder.Append(SqlGenerator.QuoteIdentifier(entitySetBase.Name));
      }
      return stringBuilder.ToString();
    }

    public override ISqlFragment Visit(DbFilterExpression e) => (ISqlFragment) this.VisitFilterExpression(e.Input, e.Predicate, false);

    public override ISqlFragment Visit(DbFunctionExpression e)
    {
      if (this.IsSpecialBuiltInFunction(e))
        return this.HandleSpecialBuiltInFunction(e);
      return this.IsSpecialCanonicalFunction(e) ? this.HandleSpecialCanonicalFunction(e) : this.HandleFunctionDefault(e);
    }

    public override ISqlFragment Visit(DbEntityRefExpression e) => throw new NotSupportedException();

    public override ISqlFragment Visit(DbRefKeyExpression e) => throw new NotSupportedException();

    public override ISqlFragment Visit(DbGroupByExpression e)
    {
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(e.Input.Expression, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      if (!this.IsCompatible(sqlSelectStatement, e.ExpressionKind))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, e.Input.VariableName, fromSymbol);
      this.symbolTable.Add(e.Input.GroupVariableName, fromSymbol);
      RowType edmType = MetadataHelpers.GetEdmType<RowType>(MetadataHelpers.GetEdmType<CollectionType>(e.ResultType).TypeUsage);
      bool flag = SqlGenerator.NeedsInnerQuery(e.Aggregates);
      SqlSelectStatement selectStatement;
      if (flag)
      {
        selectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, false, out fromSymbol);
        this.AddFromSymbol(selectStatement, e.Input.VariableName, fromSymbol, false);
      }
      else
        selectStatement = sqlSelectStatement;
      using (IEnumerator<EdmProperty> enumerator = (IEnumerator<EdmProperty>) edmType.Properties.GetEnumerator())
      {
        enumerator.MoveNext();
        string str1 = string.Empty;
        foreach (DbExpression key in (IEnumerable<DbExpression>) e.Keys)
        {
          string str2 = SqlGenerator.QuoteIdentifier(enumerator.Current.Name);
          selectStatement.GroupBy.Append((object) str1);
          ISqlFragment sqlFragment = key.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
          if (!flag)
          {
            selectStatement.Select.Append((object) str1);
            selectStatement.Select.AppendLine();
            selectStatement.Select.Append((object) sqlFragment);
            selectStatement.Select.Append((object) " AS ");
            selectStatement.Select.Append((object) str2);
            selectStatement.GroupBy.Append((object) sqlFragment);
          }
          else
          {
            sqlSelectStatement.Select.Append((object) str1);
            sqlSelectStatement.Select.AppendLine();
            sqlSelectStatement.Select.Append((object) sqlFragment);
            sqlSelectStatement.Select.Append((object) " AS ");
            sqlSelectStatement.Select.Append((object) str2);
            selectStatement.Select.Append((object) str1);
            selectStatement.Select.AppendLine();
            selectStatement.Select.Append((object) fromSymbol);
            selectStatement.Select.Append((object) ".");
            selectStatement.Select.Append((object) str2);
            selectStatement.Select.Append((object) " AS ");
            selectStatement.Select.Append((object) str2);
            selectStatement.GroupBy.Append((object) str2);
          }
          str1 = ", ";
          enumerator.MoveNext();
        }
        foreach (DbAggregate aggregate in (IEnumerable<DbAggregate>) e.Aggregates)
        {
          string str2 = SqlGenerator.QuoteIdentifier(enumerator.Current.Name);
          ISqlFragment sqlFragment1 = aggregate.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
          object aggregateArgument;
          if (flag)
          {
            SqlBuilder sqlBuilder = new SqlBuilder();
            sqlBuilder.Append((object) fromSymbol);
            sqlBuilder.Append((object) ".");
            sqlBuilder.Append((object) str2);
            aggregateArgument = (object) sqlBuilder;
            sqlSelectStatement.Select.Append((object) str1);
            sqlSelectStatement.Select.AppendLine();
            sqlSelectStatement.Select.Append((object) sqlFragment1);
            sqlSelectStatement.Select.Append((object) " AS ");
            sqlSelectStatement.Select.Append((object) str2);
          }
          else
            aggregateArgument = (object) sqlFragment1;
          ISqlFragment sqlFragment2 = (ISqlFragment) this.VisitAggregate(aggregate, aggregateArgument);
          selectStatement.Select.Append((object) str1);
          selectStatement.Select.AppendLine();
          selectStatement.Select.Append((object) sqlFragment2);
          selectStatement.Select.Append((object) " AS ");
          selectStatement.Select.Append((object) str2);
          str1 = ", ";
          enumerator.MoveNext();
        }
      }
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      return (ISqlFragment) selectStatement;
    }

    public override ISqlFragment Visit(DbIntersectExpression e) => this.VisitSetOpExpression(e.Left, e.Right, "INTERSECT");

    public override ISqlFragment Visit(DbIsEmptyExpression e) => (ISqlFragment) this.VisitIsEmptyExpression(e, false);

    public override ISqlFragment Visit(DbIsNullExpression e) => (ISqlFragment) this.VisitIsNullExpression(e, false);

    public override ISqlFragment Visit(DbIsOfExpression e) => throw new NotSupportedException();

    public override ISqlFragment Visit(DbCrossJoinExpression e) => this.VisitJoinExpression(e.Inputs, e.ExpressionKind, "CROSS JOIN", (DbExpression) null);

    public override ISqlFragment Visit(DbJoinExpression e)
    {
      string joinString;
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.FullOuterJoin:
          joinString = "FULL OUTER JOIN";
          break;
        case DbExpressionKind.InnerJoin:
          joinString = "INNER JOIN";
          break;
        case DbExpressionKind.LeftOuterJoin:
          joinString = "LEFT OUTER JOIN";
          break;
        default:
          joinString = (string) null;
          break;
      }
      return this.VisitJoinExpression((IList<DbExpressionBinding>) new List<DbExpressionBinding>(2)
      {
        e.Left,
        e.Right
      }, e.ExpressionKind, joinString, e.JoinCondition);
    }

    public override ISqlFragment Visit(DbLikeExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      sqlBuilder.Append((object) " LIKE ");
      sqlBuilder.Append((object) e.Pattern.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      if (e.Escape.ExpressionKind != DbExpressionKind.Null)
      {
        sqlBuilder.Append((object) " ESCAPE ");
        sqlBuilder.Append((object) e.Escape.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      }
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbLimitExpression e)
    {
      SqlSelectStatement sqlSelectStatement = this.VisitExpressionEnsureSqlStatement(e.Argument, false);
      if (!this.IsCompatible(sqlSelectStatement, e.ExpressionKind))
      {
        TypeUsage elementTypeUsage = MetadataHelpers.GetElementTypeUsage(e.Argument.ResultType);
        Symbol fromSymbol;
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, "top", elementTypeUsage, out fromSymbol);
        this.AddFromSymbol(sqlSelectStatement, "top", fromSymbol, false);
      }
      ISqlFragment topCount = this.HandleCountExpression(e.Limit);
      sqlSelectStatement.Top = new TopClause(topCount, e.WithTies);
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbNewInstanceExpression e) => MetadataHelpers.IsCollectionType(e.ResultType) ? this.VisitCollectionConstructor(e) : throw new NotSupportedException();

    public override ISqlFragment Visit(DbNotExpression e)
    {
      if (e.Argument is DbNotExpression dbNotExpression)
        return dbNotExpression.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
      if (e.Argument is DbIsEmptyExpression e1)
        return (ISqlFragment) this.VisitIsEmptyExpression(e1, true);
      if (e.Argument is DbIsNullExpression e2)
        return (ISqlFragment) this.VisitIsNullExpression(e2, true);
      if (e.Argument is DbComparisonExpression comparisonExpression && comparisonExpression.ExpressionKind == DbExpressionKind.Equals)
        return (ISqlFragment) this.VisitBinaryExpression(" <> ", comparisonExpression.Left, comparisonExpression.Right);
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) " NOT (");
      sqlBuilder.Append((object) e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbNullExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "NULL");
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbOfTypeExpression e) => throw new NotSupportedException();

    public override ISqlFragment Visit(DbOrExpression e)
    {
      ISqlFragment sqlFragment = (ISqlFragment) null;
      return this.TryTranslateIntoIn(e, out sqlFragment) ? sqlFragment : (ISqlFragment) this.VisitBinaryExpression(" OR ", e.Left, e.Right);
    }

    public override ISqlFragment Visit(DbParameterReferenceExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) ("@" + e.ParameterName));
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbProjectExpression e)
    {
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(e.Input.Expression, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      if (!this.IsCompatible(sqlSelectStatement, e.ExpressionKind))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, e.Input.VariableName, fromSymbol);
      if (e.Projection is DbNewInstanceExpression projection)
        sqlSelectStatement.Select.Append((object) this.VisitNewInstanceExpression(projection));
      else
        sqlSelectStatement.Select.Append((object) e.Projection.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbPropertyExpression e)
    {
      ISqlFragment sqlFragment = e.Instance.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
      if (e.Instance is DbVariableReferenceExpression)
        this.isVarRefSingle = false;
      switch (sqlFragment)
      {
        case JoinSymbol joinSymbol:
          return joinSymbol.IsNestedJoin ? (ISqlFragment) new SymbolPair((Symbol) joinSymbol, joinSymbol.NameToExtent[e.Property.Name]) : (ISqlFragment) joinSymbol.NameToExtent[e.Property.Name];
        case SymbolPair symbolPair:
          if (symbolPair.Column is JoinSymbol column2)
          {
            symbolPair.Column = column2.NameToExtent[e.Property.Name];
            return (ISqlFragment) symbolPair;
          }
          if (symbolPair.Column.Columns.ContainsKey(e.Property.Name))
          {
            SqlBuilder sqlBuilder = new SqlBuilder();
            sqlBuilder.Append((object) symbolPair.Source);
            sqlBuilder.Append((object) ".");
            sqlBuilder.Append((object) symbolPair.Column.Columns[e.Property.Name]);
            return (ISqlFragment) sqlBuilder;
          }
          break;
      }
      SqlBuilder sqlBuilder1 = new SqlBuilder();
      sqlBuilder1.Append((object) sqlFragment);
      sqlBuilder1.Append((object) ".");
      sqlBuilder1.Append((object) SqlGenerator.QuoteIdentifier(e.Property.Name));
      return (ISqlFragment) sqlBuilder1;
    }

    public override ISqlFragment Visit(DbQuantifierExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      bool negatePredicate = e.ExpressionKind == DbExpressionKind.All;
      if (e.ExpressionKind == DbExpressionKind.Any)
        sqlBuilder.Append((object) "EXISTS (");
      else
        sqlBuilder.Append((object) "NOT EXISTS (");
      SqlSelectStatement selectStatement = this.VisitFilterExpression(e.Input, e.Predicate, negatePredicate);
      if (selectStatement.Select.IsEmpty)
        this.AddDefaultColumns(selectStatement);
      sqlBuilder.Append((object) selectStatement);
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbRefExpression e) => throw new NotSupportedException();

    public override ISqlFragment Visit(DbRelationshipNavigationExpression e) => throw new NotSupportedException();

    public override ISqlFragment Visit(DbSkipExpression e)
    {
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(e.Input.Expression, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      if (!this.IsCompatible(sqlSelectStatement, e.ExpressionKind))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, e.Input.VariableName, fromSymbol);
      this.AddSortKeys(sqlSelectStatement.OrderBy, e.SortOrder);
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      ISqlFragment skipCount = this.HandleCountExpression(e.Count);
      sqlSelectStatement.Skip = new SkipClause(skipCount);
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbSortExpression e)
    {
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(e.Input.Expression, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      if (!this.IsCompatible(sqlSelectStatement, e.ExpressionKind))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, e.Input.VariableName, fromSymbol);
      this.AddSortKeys(sqlSelectStatement.OrderBy, e.SortOrder);
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbTreatExpression e) => throw new NotSupportedException();

    public override ISqlFragment Visit(DbUnionAllExpression e) => this.VisitSetOpExpression(e.Left, e.Right, "UNION ALL");

    public override ISqlFragment Visit(DbVariableReferenceExpression e)
    {
      this.isVarRefSingle = !this.isVarRefSingle ? true : throw new NotSupportedException();
      Symbol key = this.symbolTable.Lookup(e.VariableName);
      if (!this.CurrentSelectStatement.FromExtents.Contains(key))
        this.CurrentSelectStatement.OuterExtents[key] = true;
      return (ISqlFragment) key;
    }

    private SqlBuilder VisitAggregate(DbAggregate aggregate, object aggregateArgument)
    {
      SqlBuilder result = new SqlBuilder();
      if (!(aggregate is DbFunctionAggregate functionAggregate1))
        throw new NotSupportedException();
      this.WriteFunctionName(result, functionAggregate1.Function);
      result.Append((object) "(");
      DbFunctionAggregate functionAggregate2 = functionAggregate1;
      if (functionAggregate2 != null && functionAggregate2.Distinct)
        result.Append((object) "DISTINCT ");
      result.Append(aggregateArgument);
      result.Append((object) ")");
      return result;
    }

    private SqlBuilder VisitBinaryExpression(
      string op,
      DbExpression left,
      DbExpression right)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (this.IsComplexExpression(left))
        sqlBuilder.Append((object) "(");
      sqlBuilder.Append((object) left.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      if (this.IsComplexExpression(left))
        sqlBuilder.Append((object) ")");
      sqlBuilder.Append((object) op);
      if (this.IsComplexExpression(right))
        sqlBuilder.Append((object) "(");
      sqlBuilder.Append((object) right.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      if (this.IsComplexExpression(right))
        sqlBuilder.Append((object) ")");
      return sqlBuilder;
    }

    private SqlSelectStatement VisitInputExpression(
      DbExpression inputExpression,
      string inputVarName,
      TypeUsage inputVarType,
      out Symbol fromSymbol)
    {
      ISqlFragment sqlFragment = inputExpression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
      if (!(sqlFragment is SqlSelectStatement result))
      {
        result = new SqlSelectStatement();
        this.WrapNonQueryExtent(result, sqlFragment, inputExpression.ExpressionKind);
      }
      if (result.FromExtents.Count == 0)
        fromSymbol = new Symbol(inputVarName, inputVarType);
      else if (result.FromExtents.Count == 1)
      {
        fromSymbol = result.FromExtents[0];
      }
      else
      {
        fromSymbol = (Symbol) new JoinSymbol(inputVarName, inputVarType, result.FromExtents)
        {
          FlattenedExtentList = result.AllJoinExtents
        };
        result.FromExtents.Clear();
        result.FromExtents.Add(fromSymbol);
      }
      return result;
    }

    private SqlBuilder VisitIsEmptyExpression(DbIsEmptyExpression e, bool negate)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (!negate)
        sqlBuilder.Append((object) " NOT");
      sqlBuilder.Append((object) " EXISTS (");
      sqlBuilder.Append((object) this.VisitExpressionEnsureSqlStatement(e.Argument));
      sqlBuilder.AppendLine();
      sqlBuilder.Append((object) ")");
      return sqlBuilder;
    }

    private ISqlFragment VisitCollectionConstructor(DbNewInstanceExpression e)
    {
      if (e.Arguments.Count == 1 && e.Arguments[0].ExpressionKind == DbExpressionKind.Element)
      {
        DbElementExpression elementExpression = e.Arguments[0] as DbElementExpression;
        SqlSelectStatement sqlSelectStatement = this.VisitExpressionEnsureSqlStatement(elementExpression.Argument);
        if (!this.IsCompatible(sqlSelectStatement, DbExpressionKind.Element))
        {
          TypeUsage elementTypeUsage = MetadataHelpers.GetElementTypeUsage(elementExpression.Argument.ResultType);
          Symbol fromSymbol;
          sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, "element", elementTypeUsage, out fromSymbol);
          this.AddFromSymbol(sqlSelectStatement, "element", fromSymbol, false);
        }
        sqlSelectStatement.Top = new TopClause(1, false);
        return (ISqlFragment) sqlSelectStatement;
      }
      bool flag = MetadataHelpers.IsPrimitiveType(MetadataHelpers.GetEdmType<CollectionType>(e.ResultType).TypeUsage);
      SqlBuilder sqlBuilder = new SqlBuilder();
      string str = string.Empty;
      if (e.Arguments.Count == 0)
      {
        sqlBuilder.Append((object) " SELECT NULL");
        sqlBuilder.Append((object) " AS X FROM (SELECT 1) AS Y WHERE 1=0");
      }
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) e.Arguments)
      {
        sqlBuilder.Append((object) str);
        sqlBuilder.Append((object) " SELECT ");
        sqlBuilder.Append((object) dbExpression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        if (flag)
          sqlBuilder.Append((object) " AS X ");
        str = " UNION ALL ";
      }
      return (ISqlFragment) sqlBuilder;
    }

    private SqlBuilder VisitIsNullExpression(DbIsNullExpression e, bool negate)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      if (!negate)
        sqlBuilder.Append((object) " IS NULL");
      else
        sqlBuilder.Append((object) " IS NOT NULL");
      return sqlBuilder;
    }

    private ISqlFragment VisitJoinExpression(
      IList<DbExpressionBinding> inputs,
      DbExpressionKind joinKind,
      string joinString,
      DbExpression joinCondition)
    {
      SqlSelectStatement result;
      if (!this.IsParentAJoin)
      {
        result = new SqlSelectStatement();
        result.AllJoinExtents = new List<Symbol>();
        this.selectStatementStack.Push(result);
      }
      else
        result = this.CurrentSelectStatement;
      this.symbolTable.EnterScope();
      string str = string.Empty;
      bool flag = true;
      int count1 = inputs.Count;
      for (int index = 0; index < count1; ++index)
      {
        DbExpressionBinding input = inputs[index];
        if (str != string.Empty)
          result.From.AppendLine();
        result.From.Append((object) (str + " "));
        this.isParentAJoinStack.Push(input.Expression.ExpressionKind == DbExpressionKind.Scan || flag && (this.IsJoinExpression(input.Expression) || this.IsApplyExpression(input.Expression)));
        int count2 = result.FromExtents.Count;
        ISqlFragment fromExtentFragment = input.Expression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
        this.isParentAJoinStack.Pop();
        this.ProcessJoinInputResult(fromExtentFragment, result, input, count2);
        str = joinString;
        flag = false;
      }
      switch (joinKind)
      {
        case DbExpressionKind.FullOuterJoin:
        case DbExpressionKind.InnerJoin:
        case DbExpressionKind.LeftOuterJoin:
          result.From.Append((object) " ON ");
          this.isParentAJoinStack.Push(false);
          result.From.Append((object) joinCondition.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
          this.isParentAJoinStack.Pop();
          break;
      }
      this.symbolTable.ExitScope();
      if (!this.IsParentAJoin)
        this.selectStatementStack.Pop();
      return (ISqlFragment) result;
    }

    private void ProcessJoinInputResult(
      ISqlFragment fromExtentFragment,
      SqlSelectStatement result,
      DbExpressionBinding input,
      int fromSymbolStart)
    {
      Symbol fromSymbol = (Symbol) null;
      if (result != fromExtentFragment)
      {
        if (fromExtentFragment is SqlSelectStatement selectStatement2)
        {
          if (selectStatement2.Select.IsEmpty)
          {
            List<Symbol> symbolList = this.AddDefaultColumns(selectStatement2);
            if (this.IsJoinExpression(input.Expression) || this.IsApplyExpression(input.Expression))
            {
              List<Symbol> fromExtents = selectStatement2.FromExtents;
              fromSymbol = (Symbol) new JoinSymbol(input.VariableName, input.VariableType, fromExtents)
              {
                IsNestedJoin = true,
                ColumnList = symbolList
              };
            }
            else if (selectStatement2.FromExtents[0] is JoinSymbol fromExtent8)
              fromSymbol = (Symbol) new JoinSymbol(input.VariableName, input.VariableType, fromExtent8.ExtentList)
              {
                IsNestedJoin = true,
                ColumnList = symbolList,
                FlattenedExtentList = fromExtent8.FlattenedExtentList
              };
          }
          result.From.Append((object) " (");
          result.From.Append((object) selectStatement2);
          result.From.Append((object) " )");
        }
        else if (input.Expression is DbScanExpression)
          result.From.Append((object) fromExtentFragment);
        else
          this.WrapNonQueryExtent(result, fromExtentFragment, input.Expression.ExpressionKind);
        if (fromSymbol == null)
          fromSymbol = new Symbol(input.VariableName, input.VariableType);
        this.AddFromSymbol(result, input.VariableName, fromSymbol);
        result.AllJoinExtents.Add(fromSymbol);
      }
      else
      {
        List<Symbol> extents = new List<Symbol>();
        for (int index = fromSymbolStart; index < result.FromExtents.Count; ++index)
          extents.Add(result.FromExtents[index]);
        result.FromExtents.RemoveRange(fromSymbolStart, result.FromExtents.Count - fromSymbolStart);
        Symbol symbol = (Symbol) new JoinSymbol(input.VariableName, input.VariableType, extents);
        result.FromExtents.Add(symbol);
        this.symbolTable.Add(input.VariableName, symbol);
      }
    }

    private ISqlFragment VisitNewInstanceExpression(DbNewInstanceExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (!(e.ResultType.EdmType is RowType edmType))
        throw new NotSupportedException();
      ReadOnlyMetadataCollection<EdmProperty> properties = edmType.Properties;
      string str = string.Empty;
      for (int index = 0; index < e.Arguments.Count; ++index)
      {
        DbExpression dbExpression = e.Arguments[index];
        if (MetadataHelpers.IsRowType(dbExpression.ResultType))
          throw new NotSupportedException();
        EdmProperty edmProperty = properties[index];
        sqlBuilder.Append((object) str);
        sqlBuilder.AppendLine();
        sqlBuilder.Append((object) dbExpression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        sqlBuilder.Append((object) " AS ");
        sqlBuilder.Append((object) SqlGenerator.QuoteIdentifier(edmProperty.Name));
        str = ", ";
      }
      return (ISqlFragment) sqlBuilder;
    }

    private ISqlFragment VisitSetOpExpression(
      DbExpression left,
      DbExpression right,
      string separator)
    {
      SqlSelectStatement sqlSelectStatement1 = this.VisitExpressionEnsureSqlStatement(left);
      bool flag1 = sqlSelectStatement1.HaveOrderByLimitOrOffset();
      SqlSelectStatement sqlSelectStatement2 = this.VisitExpressionEnsureSqlStatement(right);
      bool flag2 = sqlSelectStatement2.HaveOrderByLimitOrOffset();
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (flag1)
        sqlBuilder.Append((object) "SELECT * FROM (");
      sqlBuilder.Append((object) sqlSelectStatement1);
      if (flag1)
        sqlBuilder.Append((object) ") ");
      sqlBuilder.AppendLine();
      sqlBuilder.Append((object) separator);
      sqlBuilder.AppendLine();
      if (flag2)
        sqlBuilder.Append((object) "SELECT * FROM (");
      sqlBuilder.Append((object) sqlSelectStatement2);
      if (flag2)
        sqlBuilder.Append((object) ") ");
      return (ISqlFragment) sqlBuilder;
    }

    private bool IsSpecialBuiltInFunction(DbFunctionExpression e) => SqlGenerator.IsBuiltinFunction(e.Function) && SqlGenerator._builtInFunctionHandlers.ContainsKey(e.Function.Name);

    private bool IsSpecialCanonicalFunction(DbFunctionExpression e) => MetadataHelpers.IsCanonicalFunction(e.Function) && SqlGenerator._canonicalFunctionHandlers.ContainsKey(e.Function.Name);

    private ISqlFragment HandleFunctionDefault(DbFunctionExpression e)
    {
      SqlBuilder result = new SqlBuilder();
      this.WriteFunctionName(result, e.Function);
      this.HandleFunctionArgumentsDefault(e, result);
      return (ISqlFragment) result;
    }

    private ISqlFragment HandleFunctionDefaultGivenName(
      DbFunctionExpression e,
      string functionName)
    {
      SqlBuilder result = new SqlBuilder();
      result.Append((object) functionName);
      this.HandleFunctionArgumentsDefault(e, result);
      return (ISqlFragment) result;
    }

    private void HandleFunctionArgumentsDefault(DbFunctionExpression e, SqlBuilder result)
    {
      bool metadataProperty = MetadataHelpers.TryGetValueForMetadataProperty<bool>((MetadataItem) e.Function, "NiladicFunctionAttribute");
      if (metadataProperty && e.Arguments.Count > 0)
        throw new InvalidOperationException("Niladic functions cannot have parameters");
      if (metadataProperty)
        return;
      result.Append((object) "(");
      string str = string.Empty;
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) e.Arguments)
      {
        result.Append((object) str);
        result.Append((object) dbExpression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        str = ", ";
      }
      result.Append((object) ")");
    }

    private ISqlFragment HandleSpecialBuiltInFunction(DbFunctionExpression e) => this.HandleSpecialFunction(SqlGenerator._builtInFunctionHandlers, e);

    private ISqlFragment HandleSpecialCanonicalFunction(DbFunctionExpression e) => this.HandleSpecialFunction(SqlGenerator._canonicalFunctionHandlers, e);

    private ISqlFragment HandleSpecialFunction(
      Dictionary<string, SqlGenerator.FunctionHandler> handlers,
      DbFunctionExpression e)
    {
      if (!handlers.ContainsKey(e.Function.Name))
        throw new InvalidOperationException("Special handling should be called only for functions in the list of special functions");
      return handlers[e.Function.Name](this, e);
    }

    private ISqlFragment HandleSpecialFunctionToOperator(
      DbFunctionExpression e,
      bool parenthesiseArguments)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (e.Arguments.Count > 1)
      {
        if (parenthesiseArguments)
          sqlBuilder.Append((object) "(");
        sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        if (parenthesiseArguments)
          sqlBuilder.Append((object) ")");
      }
      sqlBuilder.Append((object) " ");
      sqlBuilder.Append((object) SqlGenerator._functionNameToOperatorDictionary[e.Function.Name]);
      sqlBuilder.Append((object) " ");
      if (parenthesiseArguments)
        sqlBuilder.Append((object) "(");
      sqlBuilder.Append((object) e.Arguments[e.Arguments.Count - 1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      if (parenthesiseArguments)
        sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleConcatFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return sqlgen.HandleSpecialFunctionToOperator(e, false);
    }

    private static ISqlFragment HandleCanonicalFunctionBitwise(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return sqlgen.HandleSpecialFunctionToOperator(e, true);
    }

    private static ISqlFragment HandleGetDateFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      switch (sqlgen._manifest._dateTimeFormat)
      {
        case SQLiteDateFormats.Ticks:
          sqlBuilder.Append((object) "(STRFTIME('%s', 'now') * 10000000 + 621355968000000000)");
          break;
        case SQLiteDateFormats.JulianDay:
          sqlBuilder.Append((object) "CAST(STRFTIME('%J', 'now') AS double)");
          break;
        default:
          sqlBuilder.Append((object) "STRFTIME('%Y-%m-%d %H:%M:%S', 'now')");
          break;
      }
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleGetUtcDateFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      switch (sqlgen._manifest._dateTimeFormat)
      {
        case SQLiteDateFormats.Ticks:
          sqlBuilder.Append((object) "(STRFTIME('%s', 'now', 'utc') * 10000000 + 621355968000000000)");
          break;
        case SQLiteDateFormats.JulianDay:
          sqlBuilder.Append((object) "CAST(STRFTIME('%J', 'now', 'utc') AS double)");
          break;
        default:
          sqlBuilder.Append((object) "STRFTIME('%Y-%m-%d %H:%M:%S', 'now', 'utc')");
          break;
      }
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleDatepartDateFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      if (!(e.Arguments[0] is DbConstantExpression constantExpression))
        throw new InvalidOperationException(string.Format("DATEPART argument to function '{0}.{1}' must be a literal string", (object) e.Function.NamespaceName, (object) e.Function.Name));
      if (!(constantExpression.Value is string key))
        throw new InvalidOperationException(string.Format("DATEPART argument to function '{0}.{1}' must be a literal string", (object) e.Function.NamespaceName, (object) e.Function.Name));
      SqlBuilder sqlBuilder = new SqlBuilder();
      string str;
      if (!SqlGenerator._datepartKeywords.TryGetValue(key, out str))
        throw new InvalidOperationException(string.Format("{0}' is not a valid value for DATEPART argument in '{1}.{2}' function", (object) key, (object) e.Function.NamespaceName, (object) e.Function.Name));
      if (str != "%f")
      {
        sqlBuilder.Append((object) "CAST(STRFTIME('");
        sqlBuilder.Append((object) str);
        sqlBuilder.Append((object) "', ");
        if (sqlgen._manifest._dateTimeFormat == SQLiteDateFormats.Ticks)
          sqlBuilder.Append((object) string.Format("(({0} - 621355968000000000) / 10000000.0)", (object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen)));
        else
          sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        sqlBuilder.Append((object) ") AS integer)");
      }
      else
      {
        sqlBuilder.Append((object) "CAST(SUBSTR(STRFTIME('%f', ");
        if (sqlgen._manifest._dateTimeFormat == SQLiteDateFormats.Ticks)
          sqlBuilder.Append((object) string.Format("(({0} - 621355968000000000) / 10000000.0)", (object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen)));
        else
          sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        sqlBuilder.Append((object) "), 4) AS integer)");
      }
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionDateAdd(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      switch (sqlgen._manifest._dateTimeFormat)
      {
        case SQLiteDateFormats.Ticks:
          sqlBuilder.Append((object) string.Format("(STRFTIME('%s', JULIANDAY({1}) + ({0} / 86400.0)) * 10000000 + 621355968000000000)", (object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen), (object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen)));
          break;
        case SQLiteDateFormats.JulianDay:
          sqlBuilder.Append((object) string.Format("CAST(STRFTIME('%J', JULIANDAY({1}) + ({0} / 86400.0)) AS double)", (object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen), (object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen)));
          break;
        default:
          sqlBuilder.Append((object) string.Format("STRFTIME('%Y-%m-%d %H:%M:%S', JULIANDAY({1}) + ({0} / 86400.0))", (object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen), (object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen)));
          break;
      }
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionDateSubtract(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (sqlgen._manifest._dateTimeFormat == SQLiteDateFormats.Ticks)
        sqlBuilder.Append((object) string.Format("CAST((({0} - 621355968000000000) / 10000000.0)  - (({1} - 621355968000000000) / 10000000.0) * 86400.0 AS integer)", (object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen), (object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen)));
      else
        sqlBuilder.Append((object) string.Format("CAST((JULIANDAY({1}) - JULIANDAY({0})) * 86400.0 AS integer)", (object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen), (object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen)));
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionDatepart(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      string str;
      if (!SqlGenerator._datepartKeywords.TryGetValue(e.Function.Name, out str))
        throw new InvalidOperationException(string.Format("{0}' is not a valid value for STRFTIME argument", (object) e.Function.Name));
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "CAST(STRFTIME('");
      sqlBuilder.Append((object) str);
      sqlBuilder.Append((object) "', ");
      if (sqlgen._manifest._dateTimeFormat == SQLiteDateFormats.Ticks)
        sqlBuilder.Append((object) string.Format("(({0} - 621355968000000000) / 10000000.0)", (object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen)));
      else
        sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ") AS integer)");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionIndexOf(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return sqlgen.HandleFunctionDefaultGivenName(e, "CHARINDEX");
    }

    private static ISqlFragment HandleCanonicalFunctionNewGuid(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "RANDOMBLOB(16)");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionLength(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "LENGTH(");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionRound(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "ROUND(");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      if (e.Arguments.Count == 2)
      {
        sqlBuilder.Append((object) ", ");
        sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        sqlBuilder.Append((object) ")");
      }
      else
        sqlBuilder.Append((object) ", 0)");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionTrim(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "TRIM(");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionLeft(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "SUBSTR(");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ", 1, ");
      sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionRight(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "SUBSTR(");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ", -(");
      sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) "), ");
      sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionSubstring(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "SUBSTR(");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ", ");
      sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      if (e.Arguments.Count == 3)
      {
        sqlBuilder.Append((object) ", ");
        sqlBuilder.Append((object) e.Arguments[2].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      }
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionToLower(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return sqlgen.HandleFunctionDefaultGivenName(e, "LOWER");
    }

    private static ISqlFragment HandleCanonicalFunctionToUpper(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return sqlgen.HandleFunctionDefaultGivenName(e, "UPPER");
    }

    private void AddColumns(
      SqlSelectStatement selectStatement,
      Symbol symbol,
      List<Symbol> columnList,
      Dictionary<string, Symbol> columnDictionary,
      ref string separator)
    {
      if (symbol is JoinSymbol joinSymbol)
      {
        if (!joinSymbol.IsNestedJoin)
        {
          foreach (Symbol extent in joinSymbol.ExtentList)
          {
            if (!MetadataHelpers.IsPrimitiveType(extent.Type))
              this.AddColumns(selectStatement, extent, columnList, columnDictionary, ref separator);
          }
        }
        else
        {
          foreach (Symbol column in joinSymbol.ColumnList)
          {
            selectStatement.Select.Append((object) separator);
            selectStatement.Select.Append((object) symbol);
            selectStatement.Select.Append((object) ".");
            selectStatement.Select.Append((object) column);
            if (columnDictionary.ContainsKey(column.Name))
            {
              columnDictionary[column.Name].NeedsRenaming = true;
              column.NeedsRenaming = true;
            }
            else
              columnDictionary[column.Name] = column;
            columnList.Add(column);
            separator = ", ";
          }
        }
      }
      else
      {
        foreach (EdmMember property in (IEnumerable<EdmProperty>) MetadataHelpers.GetProperties(symbol.Type))
        {
          string name = property.Name;
          this.allColumnNames[name] = 0;
          Symbol symbol1;
          if (!symbol.Columns.TryGetValue(name, out symbol1))
          {
            symbol1 = new Symbol(name, (TypeUsage) null);
            symbol.Columns.Add(name, symbol1);
          }
          selectStatement.Select.Append((object) separator);
          selectStatement.Select.Append((object) symbol);
          selectStatement.Select.Append((object) ".");
          selectStatement.Select.Append((object) SqlGenerator.QuoteIdentifier(name));
          selectStatement.Select.Append((object) " AS ");
          selectStatement.Select.Append((object) symbol1);
          if (columnDictionary.ContainsKey(name))
          {
            columnDictionary[name].NeedsRenaming = true;
            symbol1.NeedsRenaming = true;
          }
          else
            columnDictionary[name] = symbol.Columns[name];
          columnList.Add(symbol1);
          separator = ", ";
        }
      }
    }

    private List<Symbol> AddDefaultColumns(SqlSelectStatement selectStatement)
    {
      List<Symbol> columnList = new List<Symbol>();
      Dictionary<string, Symbol> columnDictionary = new Dictionary<string, Symbol>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      string separator = string.Empty;
      if (!selectStatement.Select.IsEmpty)
        separator = ", ";
      foreach (Symbol fromExtent in selectStatement.FromExtents)
        this.AddColumns(selectStatement, fromExtent, columnList, columnDictionary, ref separator);
      return columnList;
    }

    private void AddFromSymbol(
      SqlSelectStatement selectStatement,
      string inputVarName,
      Symbol fromSymbol)
    {
      this.AddFromSymbol(selectStatement, inputVarName, fromSymbol, true);
    }

    private void AddFromSymbol(
      SqlSelectStatement selectStatement,
      string inputVarName,
      Symbol fromSymbol,
      bool addToSymbolTable)
    {
      if (selectStatement.FromExtents.Count == 0 || fromSymbol != selectStatement.FromExtents[0])
      {
        selectStatement.FromExtents.Add(fromSymbol);
        selectStatement.From.Append((object) " AS ");
        selectStatement.From.Append((object) fromSymbol);
        this.allExtentNames[fromSymbol.Name] = 0;
      }
      if (!addToSymbolTable)
        return;
      this.symbolTable.Add(inputVarName, fromSymbol);
    }

    private void AddSortKeys(SqlBuilder orderByClause, IList<DbSortClause> sortKeys)
    {
      string str = string.Empty;
      foreach (DbSortClause sortKey in (IEnumerable<DbSortClause>) sortKeys)
      {
        orderByClause.Append((object) str);
        orderByClause.Append((object) sortKey.Expression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        if (!string.IsNullOrEmpty(sortKey.Collation))
        {
          orderByClause.Append((object) " COLLATE ");
          orderByClause.Append((object) sortKey.Collation);
        }
        orderByClause.Append(sortKey.Ascending ? (object) " ASC" : (object) " DESC");
        str = ", ";
      }
    }

    private SqlSelectStatement CreateNewSelectStatement(
      SqlSelectStatement oldStatement,
      string inputVarName,
      TypeUsage inputVarType,
      out Symbol fromSymbol)
    {
      return this.CreateNewSelectStatement(oldStatement, inputVarName, inputVarType, true, out fromSymbol);
    }

    private SqlSelectStatement CreateNewSelectStatement(
      SqlSelectStatement oldStatement,
      string inputVarName,
      TypeUsage inputVarType,
      bool finalizeOldStatement,
      out Symbol fromSymbol)
    {
      fromSymbol = (Symbol) null;
      if (finalizeOldStatement && oldStatement.Select.IsEmpty)
      {
        List<Symbol> symbolList = this.AddDefaultColumns(oldStatement);
        if (oldStatement.FromExtents[0] is JoinSymbol fromExtent2)
          fromSymbol = (Symbol) new JoinSymbol(inputVarName, inputVarType, fromExtent2.ExtentList)
          {
            IsNestedJoin = true,
            ColumnList = symbolList,
            FlattenedExtentList = fromExtent2.FlattenedExtentList
          };
      }
      if (fromSymbol == null)
        fromSymbol = new Symbol(inputVarName, inputVarType);
      SqlSelectStatement sqlSelectStatement = new SqlSelectStatement();
      sqlSelectStatement.From.Append((object) "( ");
      sqlSelectStatement.From.Append((object) oldStatement);
      sqlSelectStatement.From.AppendLine();
      sqlSelectStatement.From.Append((object) ") ");
      return sqlSelectStatement;
    }

    private static bool NeedSingleQuotes(SQLiteDateFormats format) => format != SQLiteDateFormats.Ticks && format != SQLiteDateFormats.JulianDay && format != SQLiteDateFormats.UnixEpoch;

    private static string EscapeSingleQuote(string s, bool isUnicode) => "'" + s.Replace("'", "''") + "'";

    private string GetSqlPrimitiveType(TypeUsage type)
    {
      PrimitiveType edmType = MetadataHelpers.GetEdmType<PrimitiveType>(type);
      string name = edmType.Name;
      switch (edmType.PrimitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          int facetValueOrDefault1 = MetadataHelpers.GetFacetValueOrDefault<int>(type, MetadataHelpers.MaxLengthFacetName, MetadataHelpers.BinaryMaxMaxLength);
          string str1 = facetValueOrDefault1 != MetadataHelpers.BinaryMaxMaxLength ? facetValueOrDefault1.ToString((IFormatProvider) CultureInfo.InvariantCulture) : "max";
          return (MetadataHelpers.GetFacetValueOrDefault<bool>(type, MetadataHelpers.FixedLengthFacetName, false) ? "binary(" : "varbinary(") + str1 + ")";
        case PrimitiveTypeKind.Boolean:
          return "bit";
        case PrimitiveTypeKind.Byte:
          return "tinyint";
        case PrimitiveTypeKind.DateTime:
          return MetadataHelpers.GetFacetValueOrDefault<bool>(type, MetadataHelpers.PreserveSecondsFacetName, false) ? "datetime" : "smalldatetime";
        case PrimitiveTypeKind.Decimal:
          byte facetValueOrDefault2 = MetadataHelpers.GetFacetValueOrDefault<byte>(type, MetadataHelpers.PrecisionFacetName, (byte) 18);
          byte facetValueOrDefault3 = MetadataHelpers.GetFacetValueOrDefault<byte>(type, MetadataHelpers.ScaleFacetName, (byte) 0);
          return name + "(" + (object) facetValueOrDefault2 + "," + (object) facetValueOrDefault3 + ")";
        case PrimitiveTypeKind.Double:
          return "float";
        case PrimitiveTypeKind.Guid:
          return "uniqueidentifier";
        case PrimitiveTypeKind.Single:
          return "real";
        case PrimitiveTypeKind.Int16:
          return "smallint";
        case PrimitiveTypeKind.Int32:
          return "int";
        case PrimitiveTypeKind.Int64:
          return "bigint";
        case PrimitiveTypeKind.String:
          bool facetValueOrDefault4 = MetadataHelpers.GetFacetValueOrDefault<bool>(type, MetadataHelpers.UnicodeFacetName, true);
          bool facetValueOrDefault5 = MetadataHelpers.GetFacetValueOrDefault<bool>(type, MetadataHelpers.FixedLengthFacetName, false);
          int facetValueOrDefault6 = MetadataHelpers.GetFacetValueOrDefault<int>(type, MetadataHelpers.MaxLengthFacetName, int.MinValue);
          string str2 = facetValueOrDefault6 != int.MinValue ? facetValueOrDefault6.ToString((IFormatProvider) CultureInfo.InvariantCulture) : "max";
          if (facetValueOrDefault4 && !facetValueOrDefault5 && facetValueOrDefault6 > 4000)
            str2 = "max";
          if (!facetValueOrDefault4 && !facetValueOrDefault5 && facetValueOrDefault6 > 8000)
            str2 = "max";
          return facetValueOrDefault5 ? (facetValueOrDefault4 ? "nchar(" : "char(") + str2 + ")" : (facetValueOrDefault4 ? "nvarchar(" : "varchar(") + str2 + ")";
        default:
          throw new NotSupportedException("Unsupported EdmType: " + (object) edmType.PrimitiveTypeKind);
      }
    }

    private ISqlFragment HandleCountExpression(DbExpression e)
    {
      ISqlFragment sqlFragment;
      if (e.ExpressionKind == DbExpressionKind.Constant)
      {
        SqlBuilder sqlBuilder = new SqlBuilder();
        sqlBuilder.Append((object) ((DbConstantExpression) e).Value.ToString());
        sqlFragment = (ISqlFragment) sqlBuilder;
      }
      else
        sqlFragment = e.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
      return sqlFragment;
    }

    private bool IsApplyExpression(DbExpression e) => DbExpressionKind.CrossApply == e.ExpressionKind || DbExpressionKind.OuterApply == e.ExpressionKind;

    private bool IsKeyForIn(DbExpression e) => e.ExpressionKind == DbExpressionKind.Property || e.ExpressionKind == DbExpressionKind.VariableReference || e.ExpressionKind == DbExpressionKind.ParameterReference;

    private bool IsJoinExpression(DbExpression e) => DbExpressionKind.CrossJoin == e.ExpressionKind || DbExpressionKind.FullOuterJoin == e.ExpressionKind || DbExpressionKind.InnerJoin == e.ExpressionKind || DbExpressionKind.LeftOuterJoin == e.ExpressionKind;

    private bool IsComplexExpression(DbExpression e)
    {
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Constant:
        case DbExpressionKind.ParameterReference:
        case DbExpressionKind.Property:
          return false;
        default:
          return true;
      }
    }

    private bool IsCompatible(SqlSelectStatement result, DbExpressionKind expressionKind)
    {
      switch (expressionKind)
      {
        case DbExpressionKind.Distinct:
          return result.Top == null && result.OrderBy.IsEmpty;
        case DbExpressionKind.Element:
        case DbExpressionKind.Limit:
          return result.Top == null;
        case DbExpressionKind.Filter:
          return result.Select.IsEmpty && result.Where.IsEmpty && result.GroupBy.IsEmpty && result.Top == null;
        case DbExpressionKind.GroupBy:
          return result.Select.IsEmpty && result.GroupBy.IsEmpty && result.OrderBy.IsEmpty && result.Top == null;
        case DbExpressionKind.Project:
          return result.Select.IsEmpty && result.GroupBy.IsEmpty;
        case DbExpressionKind.Skip:
          return result.Select.IsEmpty && result.GroupBy.IsEmpty && result.OrderBy.IsEmpty && !result.IsDistinct;
        case DbExpressionKind.Sort:
          return result.Select.IsEmpty && result.GroupBy.IsEmpty && result.OrderBy.IsEmpty;
        default:
          throw new InvalidOperationException();
      }
    }

    private void ParenthesizeExpressionWithoutRedundantConstantCasts(
      DbExpression value,
      SqlBuilder sqlBuilder)
    {
      if (value.ExpressionKind == DbExpressionKind.Constant)
        sqlBuilder.Append((object) this.Visit((DbConstantExpression) value));
      else
        this.ParanthesizeExpressionIfNeeded(value, sqlBuilder);
    }

    private void ParanthesizeExpressionIfNeeded(DbExpression e, SqlBuilder result)
    {
      if (this.IsComplexExpression(e))
      {
        result.Append((object) "(");
        result.Append((object) e.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        result.Append((object) ")");
      }
      else
        result.Append((object) e.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
    }

    internal static string QuoteIdentifier(string name) => "[" + name.Replace("]", "]]") + "]";

    private bool TryAddExpressionForIn(
      DbBinaryExpression e,
      KeyToListMap<DbExpression, DbExpression> values)
    {
      if (this.IsKeyForIn(e.Left))
      {
        values.Add(e.Left, e.Right);
        return true;
      }
      if (!this.IsKeyForIn(e.Right))
        return false;
      values.Add(e.Right, e.Left);
      return true;
    }

    private SqlSelectStatement VisitExpressionEnsureSqlStatement(DbExpression e) => this.VisitExpressionEnsureSqlStatement(e, true);

    private SqlSelectStatement VisitExpressionEnsureSqlStatement(
      DbExpression e,
      bool addDefaultColumns)
    {
      SqlSelectStatement selectStatement;
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Filter:
        case DbExpressionKind.GroupBy:
        case DbExpressionKind.Project:
        case DbExpressionKind.Sort:
          selectStatement = e.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this) as SqlSelectStatement;
          break;
        default:
          string inputVarName = "c";
          this.symbolTable.EnterScope();
          TypeUsage inputVarType;
          switch (e.ExpressionKind)
          {
            case DbExpressionKind.CrossApply:
            case DbExpressionKind.CrossJoin:
            case DbExpressionKind.FullOuterJoin:
            case DbExpressionKind.InnerJoin:
            case DbExpressionKind.LeftOuterJoin:
            case DbExpressionKind.OuterApply:
            case DbExpressionKind.Scan:
              inputVarType = MetadataHelpers.GetElementTypeUsage(e.ResultType);
              break;
            default:
              inputVarType = MetadataHelpers.GetEdmType<CollectionType>(e.ResultType).TypeUsage;
              break;
          }
          Symbol fromSymbol;
          selectStatement = this.VisitInputExpression(e, inputVarName, inputVarType, out fromSymbol);
          this.AddFromSymbol(selectStatement, inputVarName, fromSymbol);
          this.symbolTable.ExitScope();
          break;
      }
      if (addDefaultColumns && selectStatement.Select.IsEmpty)
        this.AddDefaultColumns(selectStatement);
      return selectStatement;
    }

    private SqlSelectStatement VisitFilterExpression(
      DbExpressionBinding input,
      DbExpression predicate,
      bool negatePredicate)
    {
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(input.Expression, input.VariableName, input.VariableType, out fromSymbol);
      if (!this.IsCompatible(sqlSelectStatement, DbExpressionKind.Filter))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, input.VariableName, input.VariableType, out fromSymbol);
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, input.VariableName, fromSymbol);
      if (negatePredicate)
        sqlSelectStatement.Where.Append((object) "NOT (");
      sqlSelectStatement.Where.Append((object) predicate.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      if (negatePredicate)
        sqlSelectStatement.Where.Append((object) ")");
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      return sqlSelectStatement;
    }

    private void WrapNonQueryExtent(
      SqlSelectStatement result,
      ISqlFragment sqlFragment,
      DbExpressionKind expressionKind)
    {
      if (expressionKind == DbExpressionKind.Function)
      {
        result.From.Append((object) sqlFragment);
      }
      else
      {
        result.From.Append((object) " (");
        result.From.Append((object) sqlFragment);
        result.From.Append((object) ")");
      }
    }

    private static bool IsBuiltinFunction(EdmFunction function) => MetadataHelpers.TryGetValueForMetadataProperty<bool>((MetadataItem) function, "BuiltInAttribute");

    private void WriteFunctionName(SqlBuilder result, EdmFunction function)
    {
      string name = MetadataHelpers.TryGetValueForMetadataProperty<string>((MetadataItem) function, "StoreFunctionNameAttribute");
      if (string.IsNullOrEmpty(name))
        name = function.Name;
      if (SqlGenerator.IsBuiltinFunction(function))
      {
        if (function.NamespaceName == "Edm")
        {
          name.ToUpperInvariant();
          result.Append((object) name);
        }
        else
          result.Append((object) name);
      }
      else
        result.Append((object) SqlGenerator.QuoteIdentifier(name));
    }

    private static void ToBlobLiteral(byte[] bytes, SqlBuilder builder)
    {
      if (builder == null)
        throw new ArgumentNullException(nameof (builder));
      if (bytes == null)
      {
        builder.Append((object) "NULL");
      }
      else
      {
        builder.Append((object) " X'");
        for (int index = 0; index < bytes.Length; ++index)
        {
          builder.Append((object) SqlGenerator.hexDigits[((int) bytes[index] & 240) >> 4]);
          builder.Append((object) SqlGenerator.hexDigits[(int) bytes[index] & 15]);
        }
        builder.Append((object) "' ");
      }
    }

    private static bool NeedsInnerQuery(IList<DbAggregate> aggregates)
    {
      foreach (DbAggregate aggregate in (IEnumerable<DbAggregate>) aggregates)
      {
        if (!SqlGenerator.IsPropertyOverVarRef(aggregate.Arguments[0]))
          return true;
      }
      return false;
    }

    private static bool IsPropertyOverVarRef(DbExpression expression) => expression is DbPropertyExpression propertyExpression && propertyExpression.Instance is DbVariableReferenceExpression;

    private delegate ISqlFragment FunctionHandler(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpr);

    private class KeyFieldExpressionComparer : IEqualityComparer<DbExpression>
    {
      internal static readonly SqlGenerator.KeyFieldExpressionComparer Singleton = new SqlGenerator.KeyFieldExpressionComparer();

      private KeyFieldExpressionComparer()
      {
      }

      public bool Equals(DbExpression x, DbExpression y)
      {
        if (x.ExpressionKind == y.ExpressionKind)
        {
          DbExpressionKind expressionKind = x.ExpressionKind;
          if (expressionKind <= DbExpressionKind.ParameterReference)
          {
            switch (expressionKind)
            {
              case DbExpressionKind.Cast:
                DbCastExpression dbCastExpression1 = (DbCastExpression) x;
                DbCastExpression dbCastExpression2 = (DbCastExpression) y;
                return dbCastExpression1.ResultType == dbCastExpression2.ResultType && this.Equals(dbCastExpression1.Argument, dbCastExpression2.Argument);
              case DbExpressionKind.ParameterReference:
                return ((DbParameterReferenceExpression) x).ParameterName == ((DbParameterReferenceExpression) y).ParameterName;
            }
          }
          else if (expressionKind != DbExpressionKind.Property)
          {
            if (expressionKind == DbExpressionKind.VariableReference)
              return x == y;
          }
          else
          {
            DbPropertyExpression propertyExpression1 = (DbPropertyExpression) x;
            DbPropertyExpression propertyExpression2 = (DbPropertyExpression) y;
            if (propertyExpression1.Property == propertyExpression2.Property)
              return this.Equals(propertyExpression1.Instance, propertyExpression2.Instance);
            goto label_12;
          }
          return false;
        }
label_12:
        return false;
      }

      public int GetHashCode(DbExpression obj)
      {
        switch (obj.ExpressionKind)
        {
          case DbExpressionKind.Cast:
            return this.GetHashCode(((DbUnaryExpression) obj).Argument);
          case DbExpressionKind.ParameterReference:
            return ((DbParameterReferenceExpression) obj).ParameterName.GetHashCode() ^ int.MaxValue;
          case DbExpressionKind.Property:
            return ((DbPropertyExpression) obj).Property.GetHashCode();
          case DbExpressionKind.VariableReference:
            return ((DbVariableReferenceExpression) obj).VariableName.GetHashCode();
          default:
            return obj.GetHashCode();
        }
      }
    }
  }
}
