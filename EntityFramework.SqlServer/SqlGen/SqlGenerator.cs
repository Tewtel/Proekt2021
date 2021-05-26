// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SqlGenerator
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder.Spatial;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Hierarchy;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer.Utilities;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class SqlGenerator : DbExpressionVisitor<ISqlFragment>
  {
    private Stack<SqlSelectStatement> selectStatementStack;
    private Stack<bool> isParentAJoinStack;
    private Dictionary<string, int> allExtentNames;
    private Dictionary<string, int> allColumnNames;
    private readonly SymbolTable symbolTable = new SymbolTable();
    private bool isVarRefSingle;
    private readonly SymbolUsageManager optionalColumnUsageManager = new SymbolUsageManager();
    private readonly Dictionary<string, bool> _candidateParametersToForceNonUnicode = new Dictionary<string, bool>();
    private bool _forceNonUnicode;
    private bool _ignoreForceNonUnicodeFlag;
    private const byte DefaultDecimalPrecision = 18;
    private static readonly char[] _hexDigits = new char[16]
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
    private List<string> _targets;
    private static readonly ISet<string> _canonicalAndStoreStringFunctionsOneArg = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.Ordinal)
    {
      "Edm.Trim",
      "Edm.RTrim",
      "Edm.LTrim",
      "Edm.Left",
      "Edm.Right",
      "Edm.Substring",
      "Edm.ToLower",
      "Edm.ToUpper",
      "Edm.Reverse",
      "SqlServer.RTRIM",
      "SqlServer.LTRIM",
      "SqlServer.LEFT",
      "SqlServer.RIGHT",
      "SqlServer.SUBSTRING",
      "SqlServer.LOWER",
      "SqlServer.UPPER",
      "SqlServer.REVERSE"
    };
    private readonly SqlVersion _sqlVersion;
    private TypeUsage _integerType;
    private StoreItemCollection _storeItemCollection;

    private SqlSelectStatement CurrentSelectStatement => this.selectStatementStack.Peek();

    private bool IsParentAJoin => this.isParentAJoinStack.Count != 0 && this.isParentAJoinStack.Peek();

    internal Dictionary<string, int> AllExtentNames => this.allExtentNames;

    internal Dictionary<string, int> AllColumnNames => this.allColumnNames;

    public List<string> Targets => this._targets;

    internal SqlVersion SqlVersion => this._sqlVersion;

    internal bool IsPreKatmai => SqlVersionUtils.IsPreKatmai(this.SqlVersion);

    internal TypeUsage IntegerType => this._integerType ?? (this._integerType = TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreItemCollection.GetPrimitiveTypes().First<PrimitiveType>((Func<PrimitiveType, bool>) (t => t.PrimitiveTypeKind == PrimitiveTypeKind.Int64))));

    internal virtual StoreItemCollection StoreItemCollection => this._storeItemCollection;

    internal SqlGenerator() => this._sqlVersion = SqlVersion.Sql11;

    internal SqlGenerator(SqlVersion sqlVersion) => this._sqlVersion = sqlVersion;

    internal static string GenerateSql(
      DbCommandTree tree,
      SqlVersion sqlVersion,
      out List<SqlParameter> parameters,
      out CommandType commandType,
      out HashSet<string> paramsToForceNonUnicode)
    {
      commandType = CommandType.Text;
      parameters = (List<SqlParameter>) null;
      paramsToForceNonUnicode = (HashSet<string>) null;
      SqlGenerator sqlGenerator = new SqlGenerator(sqlVersion);
      switch (tree.CommandTreeKind)
      {
        case DbCommandTreeKind.Query:
          return sqlGenerator.GenerateSql((DbQueryCommandTree) tree, out paramsToForceNonUnicode);
        case DbCommandTreeKind.Update:
          return DmlSqlGenerator.GenerateUpdateSql((DbUpdateCommandTree) tree, sqlGenerator, out parameters);
        case DbCommandTreeKind.Insert:
          return DmlSqlGenerator.GenerateInsertSql((DbInsertCommandTree) tree, sqlGenerator, out parameters);
        case DbCommandTreeKind.Delete:
          return DmlSqlGenerator.GenerateDeleteSql((DbDeleteCommandTree) tree, sqlGenerator, out parameters);
        case DbCommandTreeKind.Function:
          return SqlGenerator.GenerateFunctionSql((DbFunctionCommandTree) tree, out commandType);
        default:
          return (string) null;
      }
    }

    private static string GenerateFunctionSql(
      DbFunctionCommandTree tree,
      out CommandType commandType)
    {
      EdmFunction edmFunction = tree.EdmFunction;
      if (string.IsNullOrEmpty(edmFunction.CommandTextAttribute))
      {
        commandType = CommandType.StoredProcedure;
        string name1 = string.IsNullOrEmpty(edmFunction.Schema) ? edmFunction.NamespaceName : edmFunction.Schema;
        string name2 = string.IsNullOrEmpty(edmFunction.StoreFunctionNameAttribute) ? edmFunction.Name : edmFunction.StoreFunctionNameAttribute;
        return SqlGenerator.QuoteIdentifier(name1) + "." + SqlGenerator.QuoteIdentifier(name2);
      }
      commandType = CommandType.Text;
      return edmFunction.CommandTextAttribute;
    }

    internal string GenerateSql(
      DbQueryCommandTree tree,
      out HashSet<string> paramsToForceNonUnicode)
    {
      this._targets = new List<string>();
      DbQueryCommandTree queryCommandTree = tree;
      if (this.SqlVersion == SqlVersion.Sql8 && Sql8ConformanceChecker.NeedsRewrite(tree.Query))
        queryCommandTree = Sql8ExpressionRewriter.Rewrite(tree);
      this._storeItemCollection = (StoreItemCollection) queryCommandTree.MetadataWorkspace.GetItemCollection(DataSpace.SSpace);
      this.selectStatementStack = new Stack<SqlSelectStatement>();
      this.isParentAJoinStack = new Stack<bool>();
      this.allExtentNames = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.allColumnNames = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      ISqlFragment sqlStatement;
      if (BuiltInTypeKind.CollectionType == queryCommandTree.Query.ResultType.EdmType.BuiltInTypeKind)
      {
        SqlSelectStatement sqlSelectStatement = this.VisitExpressionEnsureSqlStatement(queryCommandTree.Query);
        sqlSelectStatement.IsTopMost = true;
        sqlStatement = (ISqlFragment) sqlSelectStatement;
      }
      else
      {
        SqlBuilder sqlBuilder = new SqlBuilder();
        sqlBuilder.Append((object) "SELECT ");
        sqlBuilder.Append((object) queryCommandTree.Query.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        sqlStatement = (ISqlFragment) sqlBuilder;
      }
      if (this.isVarRefSingle)
        throw new NotSupportedException();
      paramsToForceNonUnicode = new HashSet<string>(this._candidateParametersToForceNonUnicode.Where<KeyValuePair<string, bool>>((Func<KeyValuePair<string, bool>, bool>) (p => p.Value)).Select<KeyValuePair<string, bool>, string>((Func<KeyValuePair<string, bool>, string>) (q => q.Key)));
      StringBuilder b = new StringBuilder(1024);
      using (SqlWriter writer = new SqlWriter(b))
        this.WriteSql(writer, sqlStatement);
      return b.ToString();
    }

    internal SqlWriter WriteSql(SqlWriter writer, ISqlFragment sqlStatement)
    {
      sqlStatement.WriteSql(writer, this);
      return writer;
    }

    public override ISqlFragment Visit(DbAndExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbAndExpression>(e, nameof (e));
      return (ISqlFragment) this.VisitBinaryExpression(" AND ", DbExpressionKind.And, e.Left, e.Right);
    }

    public override ISqlFragment Visit(DbApplyExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbApplyExpression>(e, nameof (e));
      List<DbExpressionBinding> expressionBindingList = new List<DbExpressionBinding>();
      expressionBindingList.Add(e.Input);
      expressionBindingList.Add(e.Apply);
      string joinString;
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.CrossApply:
          joinString = "CROSS APPLY";
          break;
        case DbExpressionKind.OuterApply:
          joinString = "OUTER APPLY";
          break;
        default:
          throw new InvalidOperationException(string.Empty);
      }
      return this.VisitJoinExpression((IList<DbExpressionBinding>) expressionBindingList, DbExpressionKind.CrossJoin, joinString, (DbExpression) null);
    }

    public override ISqlFragment Visit(DbArithmeticExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbArithmeticExpression>(e, nameof (e));
      SqlBuilder sqlBuilder;
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Divide:
          sqlBuilder = this.VisitBinaryExpression(" / ", e.ExpressionKind, e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.Minus:
          sqlBuilder = this.VisitBinaryExpression(" - ", e.ExpressionKind, e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.Modulo:
          sqlBuilder = this.VisitBinaryExpression(" % ", e.ExpressionKind, e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.Multiply:
          sqlBuilder = this.VisitBinaryExpression(" * ", e.ExpressionKind, e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.Plus:
          sqlBuilder = this.VisitBinaryExpression(" + ", e.ExpressionKind, e.Arguments[0], e.Arguments[1]);
          break;
        case DbExpressionKind.UnaryMinus:
          sqlBuilder = new SqlBuilder();
          sqlBuilder.Append((object) " -(");
          sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
          sqlBuilder.Append((object) ")");
          break;
        default:
          throw new InvalidOperationException(string.Empty);
      }
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbCaseExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbCaseExpression>(e, nameof (e));
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
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbCastExpression>(e, nameof (e));
      if (e.ResultType.IsSpatialType())
        return e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) " CAST( ");
      sqlBuilder.Append((object) e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      sqlBuilder.Append((object) " AS ");
      sqlBuilder.Append((object) this.GetSqlPrimitiveType(e.ResultType));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbComparisonExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbComparisonExpression>(e, nameof (e));
      if (e.Left.ResultType.IsPrimitiveType(PrimitiveTypeKind.String))
        this._forceNonUnicode = this.CheckIfForceNonUnicodeRequired((DbExpression) e);
      SqlBuilder sqlBuilder;
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Equals:
          sqlBuilder = this.VisitComparisonExpression(" = ", e.Left, e.Right);
          break;
        case DbExpressionKind.GreaterThan:
          sqlBuilder = this.VisitComparisonExpression(" > ", e.Left, e.Right);
          break;
        case DbExpressionKind.GreaterThanOrEquals:
          sqlBuilder = this.VisitComparisonExpression(" >= ", e.Left, e.Right);
          break;
        case DbExpressionKind.LessThan:
          sqlBuilder = this.VisitComparisonExpression(" < ", e.Left, e.Right);
          break;
        case DbExpressionKind.LessThanOrEquals:
          sqlBuilder = this.VisitComparisonExpression(" <= ", e.Left, e.Right);
          break;
        case DbExpressionKind.NotEquals:
          sqlBuilder = this.VisitComparisonExpression(" <> ", e.Left, e.Right);
          break;
        default:
          throw new InvalidOperationException(string.Empty);
      }
      this._forceNonUnicode = false;
      return (ISqlFragment) sqlBuilder;
    }

    private bool CheckIfForceNonUnicodeRequired(DbExpression e)
    {
      if (this._forceNonUnicode)
        throw new NotSupportedException();
      return this.MatchPatternForForcingNonUnicode(e);
    }

    private bool MatchPatternForForcingNonUnicode(DbExpression e)
    {
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Like:
          DbLikeExpression dbLikeExpression = (DbLikeExpression) e;
          return SqlGenerator.MatchSourcePatternForForcingNonUnicode(dbLikeExpression.Argument) && this.MatchTargetPatternForForcingNonUnicode(dbLikeExpression.Pattern) && this.MatchTargetPatternForForcingNonUnicode(dbLikeExpression.Escape);
        case DbExpressionKind.In:
          return SqlGenerator.MatchSourcePatternForForcingNonUnicode(((DbInExpression) e).Item);
        default:
          DbComparisonExpression comparisonExpression = (DbComparisonExpression) e;
          DbExpression left = comparisonExpression.Left;
          DbExpression right = comparisonExpression.Right;
          if (SqlGenerator.MatchSourcePatternForForcingNonUnicode(left) && this.MatchTargetPatternForForcingNonUnicode(right))
            return true;
          return SqlGenerator.MatchSourcePatternForForcingNonUnicode(right) && this.MatchTargetPatternForForcingNonUnicode(left);
      }
    }

    internal bool MatchTargetPatternForForcingNonUnicode(DbExpression expr)
    {
      if (SqlGenerator.IsConstParamOrNullExpressionUnicodeNotSpecified(expr))
        return true;
      if (expr.ExpressionKind == DbExpressionKind.Function)
      {
        DbFunctionExpression functionExpression = (DbFunctionExpression) expr;
        EdmFunction function = functionExpression.Function;
        if (!function.IsCanonicalFunction() && !SqlFunctionCallHandler.IsStoreFunction(function))
          return false;
        string fullName = function.FullName;
        if (SqlGenerator._canonicalAndStoreStringFunctionsOneArg.Contains(fullName))
          return this.MatchTargetPatternForForcingNonUnicode(functionExpression.Arguments[0]);
        if ("Edm.Concat".Equals(fullName, StringComparison.Ordinal))
          return this.MatchTargetPatternForForcingNonUnicode(functionExpression.Arguments[0]) && this.MatchTargetPatternForForcingNonUnicode(functionExpression.Arguments[1]);
        if (("Edm.Replace".Equals(fullName, StringComparison.Ordinal) || "SqlServer.REPLACE".Equals(fullName, StringComparison.Ordinal)) && (this.MatchTargetPatternForForcingNonUnicode(functionExpression.Arguments[0]) && this.MatchTargetPatternForForcingNonUnicode(functionExpression.Arguments[1])))
          return this.MatchTargetPatternForForcingNonUnicode(functionExpression.Arguments[2]);
      }
      return false;
    }

    private static bool MatchSourcePatternForForcingNonUnicode(DbExpression argument)
    {
      bool isUnicode;
      return argument.ExpressionKind == DbExpressionKind.Property && argument.ResultType.TryGetIsUnicode(out isUnicode) && !isUnicode;
    }

    internal static bool IsConstParamOrNullExpressionUnicodeNotSpecified(DbExpression argument)
    {
      DbExpressionKind expressionKind = argument.ExpressionKind;
      TypeUsage resultType = argument.ResultType;
      return resultType.IsPrimitiveType(PrimitiveTypeKind.String) && (expressionKind == DbExpressionKind.Constant || expressionKind == DbExpressionKind.ParameterReference || expressionKind == DbExpressionKind.Null) && !resultType.TryGetFacetValue<bool>("Unicode", out bool _);
    }

    private ISqlFragment VisitConstant(DbConstantExpression e, bool isCastOptional)
    {
      SqlBuilder result1 = new SqlBuilder();
      TypeUsage resultType = e.ResultType;
      PrimitiveTypeKind primitiveTypeKind = resultType.IsPrimitiveType() ? resultType.GetPrimitiveTypeKind() : throw new NotSupportedException();
      switch (primitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          result1.Append((object) " 0x");
          result1.Append((object) SqlGenerator.ByteArrayToBinaryString((byte[]) e.Value));
          result1.Append((object) " ");
          break;
        case PrimitiveTypeKind.Boolean:
          SqlGenerator.WrapWithCastIfNeeded(!isCastOptional, (bool) e.Value ? "1" : "0", "bit", result1);
          break;
        case PrimitiveTypeKind.Byte:
          SqlGenerator.WrapWithCastIfNeeded(!isCastOptional, e.Value.ToString(), "tinyint", result1);
          break;
        case PrimitiveTypeKind.DateTime:
          result1.Append((object) "convert(");
          result1.Append(this.IsPreKatmai ? (object) "datetime" : (object) "datetime2");
          result1.Append((object) ", ");
          result1.Append((object) SqlGenerator.EscapeSingleQuote(((DateTime) e.Value).ToString(this.IsPreKatmai ? "yyyy-MM-dd HH:mm:ss.fff" : "yyyy-MM-dd HH:mm:ss.fffffff", (IFormatProvider) CultureInfo.InvariantCulture), false));
          result1.Append((object) ", 121)");
          break;
        case PrimitiveTypeKind.Decimal:
          string str1 = ((Decimal) e.Value).ToString((IFormatProvider) CultureInfo.InvariantCulture);
          int num1;
          if (-1 == str1.IndexOf('.'))
            num1 = str1.TrimStart('-').Length < 20 ? 1 : 0;
          else
            num1 = 0;
          string str2 = "decimal(" + Math.Max((byte) str1.Length, (byte) 18).ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")";
          string str3 = str1;
          string typeName = str2;
          SqlBuilder result2 = result1;
          SqlGenerator.WrapWithCastIfNeeded(num1 != 0, str3, typeName, result2);
          break;
        case PrimitiveTypeKind.Double:
          double num2 = (double) e.Value;
          SqlGenerator.AssertValidDouble(num2);
          SqlGenerator.WrapWithCastIfNeeded(true, num2.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture), "float(53)", result1);
          break;
        case PrimitiveTypeKind.Guid:
          SqlGenerator.WrapWithCastIfNeeded(true, SqlGenerator.EscapeSingleQuote(e.Value.ToString(), false), "uniqueidentifier", result1);
          break;
        case PrimitiveTypeKind.Single:
          float num3 = (float) e.Value;
          SqlGenerator.AssertValidSingle(num3);
          SqlGenerator.WrapWithCastIfNeeded(true, num3.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture), "real", result1);
          break;
        case PrimitiveTypeKind.Int16:
          SqlGenerator.WrapWithCastIfNeeded(!isCastOptional, e.Value.ToString(), "smallint", result1);
          break;
        case PrimitiveTypeKind.Int32:
          result1.Append((object) e.Value.ToString());
          break;
        case PrimitiveTypeKind.Int64:
          SqlGenerator.WrapWithCastIfNeeded(!isCastOptional, e.Value.ToString(), "bigint", result1);
          break;
        case PrimitiveTypeKind.String:
          bool isUnicode;
          if (!e.ResultType.TryGetIsUnicode(out isUnicode))
            isUnicode = !this._forceNonUnicode;
          result1.Append((object) SqlGenerator.EscapeSingleQuote(e.Value as string, isUnicode));
          break;
        case PrimitiveTypeKind.Time:
          this.AssertKatmaiOrNewer(primitiveTypeKind);
          result1.Append((object) "convert(");
          result1.Append((object) e.ResultType.EdmType.Name);
          result1.Append((object) ", ");
          result1.Append((object) SqlGenerator.EscapeSingleQuote(e.Value.ToString(), false));
          result1.Append((object) ", 121)");
          break;
        case PrimitiveTypeKind.DateTimeOffset:
          this.AssertKatmaiOrNewer(primitiveTypeKind);
          result1.Append((object) "convert(");
          result1.Append((object) e.ResultType.EdmType.Name);
          result1.Append((object) ", ");
          result1.Append((object) SqlGenerator.EscapeSingleQuote(((DateTimeOffset) e.Value).ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz", (IFormatProvider) CultureInfo.InvariantCulture), false));
          result1.Append((object) ", 121)");
          break;
        case PrimitiveTypeKind.Geometry:
          this.AppendSpatialConstant(result1, ((DbGeometry) e.Value).AsSpatialValue());
          break;
        case PrimitiveTypeKind.Geography:
          this.AppendSpatialConstant(result1, ((DbGeography) e.Value).AsSpatialValue());
          break;
        case PrimitiveTypeKind.HierarchyId:
          SqlGenerator.AppendHierarchyConstant(result1, (HierarchyId) e.Value);
          break;
        default:
          throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.NoStoreTypeForEdmType((object) resultType.EdmType.Name, (object) ((PrimitiveType) resultType.EdmType).PrimitiveTypeKind));
      }
      return (ISqlFragment) result1;
    }

    private static void AppendHierarchyConstant(SqlBuilder result, HierarchyId hierarchyId)
    {
      result.Append((object) "cast(");
      result.Append((object) SqlGenerator.EscapeSingleQuote(hierarchyId.ToString(), false));
      result.Append((object) " as hierarchyid)");
    }

    private void AppendSpatialConstant(SqlBuilder result, IDbSpatialValue spatialValue)
    {
      DbFunctionExpression functionExpression = (DbFunctionExpression) null;
      int? coordinateSystemId = spatialValue.CoordinateSystemId;
      if (coordinateSystemId.HasValue)
      {
        string wellKnownText = spatialValue.WellKnownText;
        if (wellKnownText != null)
        {
          functionExpression = spatialValue.IsGeography ? SpatialEdmFunctions.GeographyFromText((DbExpression) wellKnownText, (DbExpression) new int?(coordinateSystemId.Value)) : SpatialEdmFunctions.GeometryFromText((DbExpression) wellKnownText, (DbExpression) new int?(coordinateSystemId.Value));
        }
        else
        {
          byte[] wellKnownBinary = spatialValue.WellKnownBinary;
          if (wellKnownBinary != null)
          {
            functionExpression = spatialValue.IsGeography ? SpatialEdmFunctions.GeographyFromBinary((DbExpression) wellKnownBinary, (DbExpression) new int?(coordinateSystemId.Value)) : SpatialEdmFunctions.GeometryFromBinary((DbExpression) wellKnownBinary, (DbExpression) new int?(coordinateSystemId.Value));
          }
          else
          {
            string gmlString = spatialValue.GmlString;
            if (gmlString != null)
              functionExpression = spatialValue.IsGeography ? SpatialEdmFunctions.GeographyFromGml((DbExpression) gmlString, (DbExpression) new int?(coordinateSystemId.Value)) : SpatialEdmFunctions.GeometryFromGml((DbExpression) gmlString, (DbExpression) new int?(coordinateSystemId.Value));
          }
        }
      }
      if (functionExpression == null)
        throw spatialValue.NotSqlCompatible();
      result.Append((object) SqlFunctionCallHandler.GenerateFunctionCallSql(this, functionExpression));
    }

    private static void AssertValidDouble(double value)
    {
      if (double.IsNaN(value))
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_TypedNaNNotSupported((object) Enum.GetName(typeof (PrimitiveTypeKind), (object) PrimitiveTypeKind.Double)));
      if (double.IsPositiveInfinity(value))
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_TypedPositiveInfinityNotSupported((object) Enum.GetName(typeof (PrimitiveTypeKind), (object) PrimitiveTypeKind.Double), (object) typeof (double).Name));
      if (double.IsNegativeInfinity(value))
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_TypedNegativeInfinityNotSupported((object) Enum.GetName(typeof (PrimitiveTypeKind), (object) PrimitiveTypeKind.Double), (object) typeof (double).Name));
    }

    private static void AssertValidSingle(float value)
    {
      if (float.IsNaN(value))
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_TypedNaNNotSupported((object) Enum.GetName(typeof (PrimitiveTypeKind), (object) PrimitiveTypeKind.Single)));
      if (float.IsPositiveInfinity(value))
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_TypedPositiveInfinityNotSupported((object) Enum.GetName(typeof (PrimitiveTypeKind), (object) PrimitiveTypeKind.Single), (object) typeof (float).Name));
      if (float.IsNegativeInfinity(value))
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_TypedNegativeInfinityNotSupported((object) Enum.GetName(typeof (PrimitiveTypeKind), (object) PrimitiveTypeKind.Single), (object) typeof (float).Name));
    }

    private static void WrapWithCastIfNeeded(
      bool cast,
      string value,
      string typeName,
      SqlBuilder result)
    {
      if (!cast)
      {
        result.Append((object) value);
      }
      else
      {
        result.Append((object) "cast(");
        result.Append((object) value);
        result.Append((object) " as ");
        result.Append((object) typeName);
        result.Append((object) ")");
      }
    }

    public override ISqlFragment Visit(DbConstantExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbConstantExpression>(e, nameof (e));
      return this.VisitConstant(e, false);
    }

    public override ISqlFragment Visit(DbDerefExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbDerefExpression>(e, nameof (e));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbDistinctExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbDistinctExpression>(e, nameof (e));
      SqlSelectStatement sqlSelectStatement = this.VisitExpressionEnsureSqlStatement(e.Argument);
      if (!SqlGenerator.IsCompatible(sqlSelectStatement, e.ExpressionKind))
      {
        TypeUsage elementTypeUsage = e.Argument.ResultType.GetElementTypeUsage();
        Symbol fromSymbol;
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, "distinct", elementTypeUsage, out fromSymbol);
        this.AddFromSymbol(sqlSelectStatement, "distinct", fromSymbol, false);
      }
      sqlSelectStatement.Select.IsDistinct = true;
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbElementExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbElementExpression>(e, nameof (e));
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "(");
      sqlBuilder.Append((object) this.VisitExpressionEnsureSqlStatement(e.Argument));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbExceptExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbExceptExpression>(e, nameof (e));
      return this.VisitSetOpExpression((DbBinaryExpression) e, "EXCEPT");
    }

    public override ISqlFragment Visit(DbExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbExpression>(e, nameof (e));
      throw new InvalidOperationException(string.Empty);
    }

    public override ISqlFragment Visit(DbScanExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbScanExpression>(e, nameof (e));
      string targetTsql = SqlGenerator.GetTargetTSql(e.Target);
      if (this._targets != null)
        this._targets.Add(targetTsql);
      if (this.IsParentAJoin)
      {
        SqlBuilder sqlBuilder = new SqlBuilder();
        sqlBuilder.Append((object) targetTsql);
        return (ISqlFragment) sqlBuilder;
      }
      SqlSelectStatement sqlSelectStatement = new SqlSelectStatement();
      sqlSelectStatement.From.Append((object) targetTsql);
      return (ISqlFragment) sqlSelectStatement;
    }

    internal static string GetTargetTSql(EntitySetBase entitySetBase)
    {
      string metadataPropertyValue1 = entitySetBase.GetMetadataPropertyValue<string>("DefiningQuery");
      if (metadataPropertyValue1 != null)
        return "(" + metadataPropertyValue1 + ")";
      StringBuilder stringBuilder = new StringBuilder(50);
      string metadataPropertyValue2 = entitySetBase.GetMetadataPropertyValue<string>("Schema");
      if (!string.IsNullOrEmpty(metadataPropertyValue2))
      {
        stringBuilder.Append(SqlGenerator.QuoteIdentifier(metadataPropertyValue2));
        stringBuilder.Append(".");
      }
      else
      {
        stringBuilder.Append(SqlGenerator.QuoteIdentifier(entitySetBase.EntityContainer.Name));
        stringBuilder.Append(".");
      }
      string metadataPropertyValue3 = entitySetBase.GetMetadataPropertyValue<string>("Table");
      stringBuilder.Append(string.IsNullOrEmpty(metadataPropertyValue3) ? SqlGenerator.QuoteIdentifier(entitySetBase.Name) : SqlGenerator.QuoteIdentifier(metadataPropertyValue3));
      return stringBuilder.ToString();
    }

    public override ISqlFragment Visit(DbFilterExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbFilterExpression>(e, nameof (e));
      return (ISqlFragment) this.VisitFilterExpression(e.Input, e.Predicate, false);
    }

    public override ISqlFragment Visit(DbFunctionExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbFunctionExpression>(e, nameof (e));
      return SqlFunctionCallHandler.GenerateFunctionCallSql(this, e);
    }

    public override ISqlFragment Visit(DbLambdaExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbLambdaExpression>(expression, nameof (expression));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbEntityRefExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbEntityRefExpression>(e, nameof (e));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbRefKeyExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbRefKeyExpression>(e, nameof (e));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbGroupByExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbGroupByExpression>(e, nameof (e));
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(e.Input.Expression, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      if (!SqlGenerator.IsCompatible(sqlSelectStatement, e.ExpressionKind))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, e.Input.VariableName, fromSymbol);
      this.symbolTable.Add(e.Input.GroupVariableName, fromSymbol);
      RowType edmType = (RowType) ((CollectionType) e.ResultType.EdmType).TypeUsage.EdmType;
      bool flag = SqlGenerator.GroupByAggregatesNeedInnerQuery(e.Aggregates, e.Input.GroupVariableName) || SqlGenerator.GroupByKeysNeedInnerQuery(e.Keys, e.Input.VariableName);
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
        string str1 = "";
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
          EdmProperty current = enumerator.Current;
          string str2 = SqlGenerator.QuoteIdentifier(current.Name);
          List<object> objectList = new List<object>();
          for (int index = 0; index < aggregate.Arguments.Count; ++index)
          {
            ISqlFragment sqlFragment = aggregate.Arguments[index].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
            object obj;
            if (flag)
            {
              string str3 = SqlGenerator.QuoteIdentifier(current.Name + "_" + index.ToString());
              SqlBuilder sqlBuilder = new SqlBuilder();
              sqlBuilder.Append((object) fromSymbol);
              sqlBuilder.Append((object) ".");
              sqlBuilder.Append((object) str3);
              obj = (object) sqlBuilder;
              sqlSelectStatement.Select.Append((object) str1);
              sqlSelectStatement.Select.AppendLine();
              sqlSelectStatement.Select.Append((object) sqlFragment);
              sqlSelectStatement.Select.Append((object) " AS ");
              sqlSelectStatement.Select.Append((object) str3);
            }
            else
              obj = (object) sqlFragment;
            objectList.Add(obj);
          }
          ISqlFragment sqlFragment1 = (ISqlFragment) SqlGenerator.VisitAggregate(aggregate, (IList<object>) objectList);
          selectStatement.Select.Append((object) str1);
          selectStatement.Select.AppendLine();
          selectStatement.Select.Append((object) sqlFragment1);
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

    public override ISqlFragment Visit(DbIntersectExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbIntersectExpression>(e, nameof (e));
      return this.VisitSetOpExpression((DbBinaryExpression) e, "INTERSECT");
    }

    public override ISqlFragment Visit(DbIsEmptyExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbIsEmptyExpression>(e, nameof (e));
      return (ISqlFragment) this.VisitIsEmptyExpression(e, false);
    }

    public override ISqlFragment Visit(DbIsNullExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbIsNullExpression>(e, nameof (e));
      return (ISqlFragment) this.VisitIsNullExpression(e, false);
    }

    public override ISqlFragment Visit(DbIsOfExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbIsOfExpression>(e, nameof (e));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbCrossJoinExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbCrossJoinExpression>(e, nameof (e));
      return this.VisitJoinExpression(e.Inputs, e.ExpressionKind, "CROSS JOIN", (DbExpression) null);
    }

    public override ISqlFragment Visit(DbJoinExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbJoinExpression>(e, nameof (e));
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
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbLikeExpression>(e, nameof (e));
      this._forceNonUnicode = this.CheckIfForceNonUnicodeRequired((DbExpression) e);
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      sqlBuilder.Append((object) " LIKE ");
      sqlBuilder.Append((object) e.Pattern.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      if (e.Escape.ExpressionKind != DbExpressionKind.Null)
      {
        sqlBuilder.Append((object) " ESCAPE ");
        sqlBuilder.Append((object) e.Escape.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      }
      this._forceNonUnicode = false;
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbLimitExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbLimitExpression>(e, nameof (e));
      SqlSelectStatement sqlSelectStatement = this.VisitExpressionEnsureSqlStatement(e.Argument, false, false);
      if (!SqlGenerator.IsCompatible(sqlSelectStatement, e.ExpressionKind))
      {
        TypeUsage elementTypeUsage = e.Argument.ResultType.GetElementTypeUsage();
        Symbol fromSymbol;
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, "top", elementTypeUsage, out fromSymbol);
        this.AddFromSymbol(sqlSelectStatement, "top", fromSymbol, false);
      }
      ISqlFragment topCount = this.HandleCountExpression(e.Limit);
      sqlSelectStatement.Select.Top = new TopClause(topCount, e.WithTies);
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbNewInstanceExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbNewInstanceExpression>(e, nameof (e));
      if (BuiltInTypeKind.CollectionType == e.ResultType.EdmType.BuiltInTypeKind)
        return this.VisitCollectionConstructor(e);
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbNotExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbNotExpression>(e, nameof (e));
      if (e.Argument is DbNotExpression dbNotExpression)
        return dbNotExpression.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
      if (e.Argument is DbIsEmptyExpression e1)
        return (ISqlFragment) this.VisitIsEmptyExpression(e1, true);
      if (e.Argument is DbIsNullExpression e2)
        return (ISqlFragment) this.VisitIsNullExpression(e2, true);
      if (e.Argument is DbComparisonExpression comparisonExpression && comparisonExpression.ExpressionKind == DbExpressionKind.Equals)
      {
        bool forceNonUnicode = this._forceNonUnicode;
        if (comparisonExpression.Left.ResultType.IsPrimitiveType(PrimitiveTypeKind.String))
          this._forceNonUnicode = this.CheckIfForceNonUnicodeRequired((DbExpression) comparisonExpression);
        SqlBuilder sqlBuilder = this.VisitComparisonExpression(" <> ", comparisonExpression.Left, comparisonExpression.Right);
        this._forceNonUnicode = forceNonUnicode;
        return (ISqlFragment) sqlBuilder;
      }
      SqlBuilder sqlBuilder1 = new SqlBuilder();
      sqlBuilder1.Append((object) " NOT (");
      sqlBuilder1.Append((object) e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      sqlBuilder1.Append((object) ")");
      return (ISqlFragment) sqlBuilder1;
    }

    public override ISqlFragment Visit(DbNullExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbNullExpression>(e, nameof (e));
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "CAST(NULL AS ");
      TypeUsage resultType = e.ResultType;
      switch ((resultType.EdmType as PrimitiveType).PrimitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          sqlBuilder.Append((object) "varbinary(1)");
          break;
        case PrimitiveTypeKind.String:
          sqlBuilder.Append((object) "varchar(1)");
          break;
        default:
          sqlBuilder.Append((object) this.GetSqlPrimitiveType(resultType));
          break;
      }
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbOfTypeExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbOfTypeExpression>(e, nameof (e));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbOrExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbOrExpression>(e, nameof (e));
      ISqlFragment sqlFragment = (ISqlFragment) null;
      return this.TryTranslateIntoIn(e, out sqlFragment) ? sqlFragment : (ISqlFragment) this.VisitBinaryExpression(" OR ", e.ExpressionKind, e.Left, e.Right);
    }

    public override ISqlFragment Visit(DbInExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbInExpression>(e, nameof (e));
      if (e.List.Count == 0)
        return this.Visit(DbExpressionBuilder.False);
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (e.Item.ResultType.IsPrimitiveType(PrimitiveTypeKind.String))
        this._forceNonUnicode = this.CheckIfForceNonUnicodeRequired((DbExpression) e);
      sqlBuilder.Append((object) e.Item.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      sqlBuilder.Append((object) " IN (");
      bool flag = true;
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) e.List)
      {
        if (flag)
          flag = false;
        else
          sqlBuilder.Append((object) ", ");
        sqlBuilder.Append((object) dbExpression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      }
      sqlBuilder.Append((object) ")");
      this._forceNonUnicode = false;
      return (ISqlFragment) sqlBuilder;
    }

    internal static IDictionary<DbExpression, IList<DbExpression>> HasBuiltMapForIn(
      DbOrExpression expression)
    {
      Dictionary<DbExpression, IList<DbExpression>> dictionary = new Dictionary<DbExpression, IList<DbExpression>>((IEqualityComparer<DbExpression>) new SqlGenerator.KeyFieldExpressionComparer());
      return !SqlGenerator.HasBuiltMapForIn((DbExpression) expression, (IDictionary<DbExpression, IList<DbExpression>>) dictionary) ? (IDictionary<DbExpression, IList<DbExpression>>) null : (IDictionary<DbExpression, IList<DbExpression>>) dictionary;
    }

    private bool TryTranslateIntoIn(DbOrExpression e, out ISqlFragment sqlFragment)
    {
      IDictionary<DbExpression, IList<DbExpression>> dictionary = SqlGenerator.HasBuiltMapForIn(e);
      if (dictionary == null)
      {
        sqlFragment = (ISqlFragment) null;
        return false;
      }
      SqlBuilder sqlBuilder = new SqlBuilder();
      bool flag1 = true;
      foreach (DbExpression key in (IEnumerable<DbExpression>) dictionary.Keys)
      {
        IList<DbExpression> source1 = dictionary[key];
        if (!flag1)
          sqlBuilder.Append((object) " OR ");
        else
          flag1 = false;
        IEnumerable<DbExpression> source2 = source1.Where<DbExpression>((Func<DbExpression, bool>) (v => v.ExpressionKind != DbExpressionKind.IsNull));
        int num = source2.Count<DbExpression>();
        bool forceNonUnicodeOnQualifyingValues = false;
        bool forceNonUnicodeOnKey = false;
        if (key.ResultType.IsPrimitiveType(PrimitiveTypeKind.String))
        {
          forceNonUnicodeOnQualifyingValues = SqlGenerator.MatchSourcePatternForForcingNonUnicode(key);
          forceNonUnicodeOnKey = !forceNonUnicodeOnQualifyingValues && this.MatchTargetPatternForForcingNonUnicode(key) && source2.All<DbExpression>(new Func<DbExpression, bool>(SqlGenerator.MatchSourcePatternForForcingNonUnicode));
        }
        if (num == 1)
        {
          this.HandleInKey(sqlBuilder, key, forceNonUnicodeOnKey);
          sqlBuilder.Append((object) " = ");
          DbExpression dbExpression = source2.First<DbExpression>();
          this.HandleInValue(sqlBuilder, dbExpression, key.ResultType.EdmType == dbExpression.ResultType.EdmType, forceNonUnicodeOnQualifyingValues);
        }
        if (num > 1)
        {
          this.HandleInKey(sqlBuilder, key, forceNonUnicodeOnKey);
          sqlBuilder.Append((object) " IN (");
          bool flag2 = true;
          foreach (DbExpression dbExpression in source2)
          {
            if (!flag2)
              sqlBuilder.Append((object) ",");
            else
              flag2 = false;
            this.HandleInValue(sqlBuilder, dbExpression, key.ResultType.EdmType == dbExpression.ResultType.EdmType, forceNonUnicodeOnQualifyingValues);
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

    private void HandleInValue(
      SqlBuilder sqlBuilder,
      DbExpression value,
      bool isSameEdmType,
      bool forceNonUnicodeOnQualifyingValues)
    {
      this.ForcingNonUnicode((Action) (() => this.ParenthesizeExpressionWithoutRedundantConstantCasts(value, sqlBuilder, isSameEdmType)), forceNonUnicodeOnQualifyingValues && this.MatchTargetPatternForForcingNonUnicode(value));
    }

    private void HandleInKey(SqlBuilder sqlBuilder, DbExpression key, bool forceNonUnicodeOnKey) => this.ForcingNonUnicode((Action) (() => this.ParenthesizeExpressionIfNeeded(key, sqlBuilder)), forceNonUnicodeOnKey);

    private void ForcingNonUnicode(Action action, bool forceNonUnicode)
    {
      bool flag = false;
      if (forceNonUnicode && !this._forceNonUnicode)
      {
        this._forceNonUnicode = true;
        flag = true;
      }
      action();
      if (!flag)
        return;
      this._forceNonUnicode = false;
    }

    private void ParenthesizeExpressionWithoutRedundantConstantCasts(
      DbExpression value,
      SqlBuilder sqlBuilder,
      bool isSameEdmType)
    {
      if (value.ExpressionKind == DbExpressionKind.Constant)
        sqlBuilder.Append((object) this.VisitConstant((DbConstantExpression) value, isSameEdmType));
      else
        this.ParenthesizeExpressionIfNeeded(value, sqlBuilder);
    }

    internal static bool IsKeyForIn(DbExpression e) => e.ExpressionKind == DbExpressionKind.Property || e.ExpressionKind == DbExpressionKind.VariableReference || e.ExpressionKind == DbExpressionKind.ParameterReference;

    internal static bool TryAddExpressionForIn(
      DbBinaryExpression e,
      IDictionary<DbExpression, IList<DbExpression>> values)
    {
      if (SqlGenerator.IsKeyForIn(e.Left))
      {
        values.Add<DbExpression, DbExpression>(e.Left, e.Right);
        return true;
      }
      if (!SqlGenerator.IsKeyForIn(e.Right))
        return false;
      values.Add<DbExpression, DbExpression>(e.Right, e.Left);
      return true;
    }

    internal static bool HasBuiltMapForIn(
      DbExpression e,
      IDictionary<DbExpression, IList<DbExpression>> values)
    {
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Equals:
          return SqlGenerator.TryAddExpressionForIn((DbBinaryExpression) e, values);
        case DbExpressionKind.IsNull:
          DbExpression dbExpression = ((DbUnaryExpression) e).Argument;
          if (!SqlGenerator.IsKeyForIn(dbExpression))
            return false;
          values.Add<DbExpression, DbExpression>(dbExpression, e);
          return true;
        case DbExpressionKind.Or:
          DbBinaryExpression binaryExpression = (DbBinaryExpression) e;
          return SqlGenerator.HasBuiltMapForIn(binaryExpression.Left, values) && SqlGenerator.HasBuiltMapForIn(binaryExpression.Right, values);
        default:
          return false;
      }
    }

    public override ISqlFragment Visit(DbParameterReferenceExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbParameterReferenceExpression>(e, nameof (e));
      if (!this._ignoreForceNonUnicodeFlag)
      {
        if (!this._forceNonUnicode)
          this._candidateParametersToForceNonUnicode[e.ParameterName] = false;
        else if (!this._candidateParametersToForceNonUnicode.ContainsKey(e.ParameterName))
          this._candidateParametersToForceNonUnicode[e.ParameterName] = true;
      }
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) ("@" + e.ParameterName));
      return (ISqlFragment) sqlBuilder;
    }

    public override ISqlFragment Visit(DbProjectExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbProjectExpression>(e, nameof (e));
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(e.Input.Expression, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      bool aliasesNeedRenaming = false;
      if (!SqlGenerator.IsCompatible(sqlSelectStatement, e.ExpressionKind))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      else if (this.SqlVersion == SqlVersion.Sql8 && !sqlSelectStatement.OrderBy.IsEmpty)
        aliasesNeedRenaming = true;
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, e.Input.VariableName, fromSymbol);
      if (e.Projection is DbNewInstanceExpression projection)
      {
        Dictionary<string, Symbol> newColumns;
        sqlSelectStatement.Select.Append((object) this.VisitNewInstanceExpression(projection, aliasesNeedRenaming, out newColumns));
        if (aliasesNeedRenaming)
          sqlSelectStatement.OutputColumnsRenamed = true;
        sqlSelectStatement.OutputColumns = newColumns;
      }
      else
        sqlSelectStatement.Select.Append((object) e.Projection.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbPropertyExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbPropertyExpression>(e, nameof (e));
      ISqlFragment sqlFragment = e.Instance.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
      if (e.Instance is DbVariableReferenceExpression)
        this.isVarRefSingle = false;
      switch (sqlFragment)
      {
        case JoinSymbol joinSymbol:
          return joinSymbol.IsNestedJoin ? (ISqlFragment) new SymbolPair((Symbol) joinSymbol, joinSymbol.NameToExtent[e.Property.Name]) : (ISqlFragment) joinSymbol.NameToExtent[e.Property.Name];
        case SymbolPair symbolPair:
          if (symbolPair.Column is JoinSymbol column3)
          {
            symbolPair.Column = column3.NameToExtent[e.Property.Name];
            return (ISqlFragment) symbolPair;
          }
          if (symbolPair.Column.Columns.ContainsKey(e.Property.Name))
          {
            SqlBuilder sqlBuilder = new SqlBuilder();
            sqlBuilder.Append((object) symbolPair.Source);
            sqlBuilder.Append((object) ".");
            Symbol column2 = symbolPair.Column.Columns[e.Property.Name];
            this.optionalColumnUsageManager.MarkAsUsed(column2);
            sqlBuilder.Append((object) column2);
            return (ISqlFragment) sqlBuilder;
          }
          break;
      }
      SqlBuilder sqlBuilder1 = new SqlBuilder();
      sqlBuilder1.Append((object) sqlFragment);
      sqlBuilder1.Append((object) ".");
      Symbol key;
      if (sqlFragment is Symbol symbol && symbol.OutputColumns.TryGetValue(e.Property.Name, out key))
      {
        this.optionalColumnUsageManager.MarkAsUsed(key);
        if (symbol.OutputColumnsRenamed)
          sqlBuilder1.Append((object) key);
        else
          sqlBuilder1.Append((object) SqlGenerator.QuoteIdentifier(e.Property.Name));
      }
      else
        sqlBuilder1.Append((object) SqlGenerator.QuoteIdentifier(e.Property.Name));
      return (ISqlFragment) sqlBuilder1;
    }

    public override ISqlFragment Visit(DbQuantifierExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbQuantifierExpression>(e, nameof (e));
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

    public override ISqlFragment Visit(DbRefExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbRefExpression>(e, nameof (e));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbRelationshipNavigationExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbRelationshipNavigationExpression>(e, nameof (e));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbSkipExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbSkipExpression>(e, nameof (e));
      Symbol fromSymbol1;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(e.Input.Expression, e.Input.VariableName, e.Input.VariableType, out fromSymbol1);
      if (!SqlGenerator.IsCompatible(sqlSelectStatement, e.ExpressionKind))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, out fromSymbol1);
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, e.Input.VariableName, fromSymbol1);
      if (this.SqlVersion >= SqlVersion.Sql11)
      {
        sqlSelectStatement.Select.Skip = new SkipClause(this.HandleCountExpression(e.Count));
        if (SqlProviderServices.UseRowNumberOrderingInOffsetQueries)
        {
          sqlSelectStatement.OrderBy.Append((object) "row_number() OVER (ORDER BY ");
          this.AddSortKeys(sqlSelectStatement.OrderBy, e.SortOrder);
          sqlSelectStatement.OrderBy.Append((object) ")");
        }
        else
          this.AddSortKeys(sqlSelectStatement.OrderBy, e.SortOrder);
        this.symbolTable.ExitScope();
        this.selectStatementStack.Pop();
        return (ISqlFragment) sqlSelectStatement;
      }
      List<Symbol> source = this.AddDefaultColumns(sqlSelectStatement);
      sqlSelectStatement.Select.Append((object) "row_number() OVER (ORDER BY ");
      this.AddSortKeys((SqlBuilder) sqlSelectStatement.Select, e.SortOrder);
      sqlSelectStatement.Select.Append((object) ") AS ");
      string row_numberName = "row_number";
      Symbol symbol = new Symbol(row_numberName, this.IntegerType);
      if (source.Any<Symbol>((Func<Symbol, bool>) (c => string.Equals(c.Name, row_numberName, StringComparison.OrdinalIgnoreCase))))
        symbol.NeedsRenaming = true;
      sqlSelectStatement.Select.Append((object) symbol);
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      SqlSelectStatement selectStatement = new SqlSelectStatement();
      selectStatement.From.Append((object) "( ");
      selectStatement.From.Append((object) sqlSelectStatement);
      selectStatement.From.AppendLine();
      selectStatement.From.Append((object) ") ");
      Symbol fromSymbol2 = (Symbol) null;
      if (sqlSelectStatement.FromExtents.Count == 1 && sqlSelectStatement.FromExtents[0] is JoinSymbol fromExtent)
        fromSymbol2 = (Symbol) new JoinSymbol(e.Input.VariableName, e.Input.VariableType, fromExtent.ExtentList)
        {
          IsNestedJoin = true,
          ColumnList = source,
          FlattenedExtentList = fromExtent.FlattenedExtentList
        };
      if (fromSymbol2 == null)
        fromSymbol2 = new Symbol(e.Input.VariableName, e.Input.VariableType, sqlSelectStatement.OutputColumns, false);
      this.selectStatementStack.Push(selectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(selectStatement, e.Input.VariableName, fromSymbol2);
      selectStatement.Where.Append((object) fromSymbol2);
      selectStatement.Where.Append((object) ".");
      selectStatement.Where.Append((object) symbol);
      selectStatement.Where.Append((object) " > ");
      selectStatement.Where.Append((object) this.HandleCountExpression(e.Count));
      this.AddSortKeys(selectStatement.OrderBy, e.SortOrder);
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      return (ISqlFragment) selectStatement;
    }

    public override ISqlFragment Visit(DbSortExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbSortExpression>(e, nameof (e));
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(e.Input.Expression, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      if (!SqlGenerator.IsCompatible(sqlSelectStatement, e.ExpressionKind))
        sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, e.Input.VariableName, e.Input.VariableType, out fromSymbol);
      this.selectStatementStack.Push(sqlSelectStatement);
      this.symbolTable.EnterScope();
      this.AddFromSymbol(sqlSelectStatement, e.Input.VariableName, fromSymbol);
      this.AddSortKeys(sqlSelectStatement.OrderBy, e.SortOrder);
      this.symbolTable.ExitScope();
      this.selectStatementStack.Pop();
      return (ISqlFragment) sqlSelectStatement;
    }

    public override ISqlFragment Visit(DbTreatExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbTreatExpression>(e, nameof (e));
      throw new NotSupportedException();
    }

    public override ISqlFragment Visit(DbUnionAllExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbUnionAllExpression>(e, nameof (e));
      return this.VisitSetOpExpression((DbBinaryExpression) e, "UNION ALL");
    }

    public override ISqlFragment Visit(DbVariableReferenceExpression e)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbVariableReferenceExpression>(e, nameof (e));
      this.isVarRefSingle = !this.isVarRefSingle ? true : throw new NotSupportedException();
      Symbol key = this.symbolTable.Lookup(e.VariableName);
      this.optionalColumnUsageManager.MarkAsUsed(key);
      if (!this.CurrentSelectStatement.FromExtents.Contains(key))
        this.CurrentSelectStatement.OuterExtents[key] = true;
      return (ISqlFragment) key;
    }

    private static SqlBuilder VisitAggregate(
      DbAggregate aggregate,
      IList<object> aggregateArguments)
    {
      SqlBuilder result = new SqlBuilder();
      if (!(aggregate is DbFunctionAggregate functionAggregate1))
        throw new NotSupportedException();
      if (functionAggregate1.Function.IsCanonicalFunction() && string.Equals(functionAggregate1.Function.Name, "BigCount", StringComparison.Ordinal))
        result.Append((object) "COUNT_BIG");
      else
        SqlFunctionCallHandler.WriteFunctionName(result, functionAggregate1.Function);
      result.Append((object) "(");
      DbFunctionAggregate functionAggregate2 = functionAggregate1;
      if (functionAggregate2 != null && functionAggregate2.Distinct)
        result.Append((object) "DISTINCT ");
      string str = string.Empty;
      foreach (object aggregateArgument in (IEnumerable<object>) aggregateArguments)
      {
        result.Append((object) str);
        result.Append(aggregateArgument);
        str = ", ";
      }
      result.Append((object) ")");
      return result;
    }

    internal void ParenthesizeExpressionIfNeeded(DbExpression e, SqlBuilder result)
    {
      if (SqlGenerator.IsComplexExpression(e))
      {
        result.Append((object) "(");
        result.Append((object) e.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        result.Append((object) ")");
      }
      else
        result.Append((object) e.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
    }

    private SqlBuilder VisitBinaryExpression(
      string op,
      DbExpressionKind expressionKind,
      DbExpression left,
      DbExpression right)
    {
      SqlGenerator.RemoveUnnecessaryCasts(ref left, ref right);
      SqlBuilder result = new SqlBuilder();
      bool flag = true;
      foreach (DbExpression e in SqlGenerator.FlattenAssociativeExpression(expressionKind, left, right))
      {
        if (flag)
          flag = false;
        else
          result.Append((object) op);
        this.ParenthesizeExpressionIfNeeded(e, result);
      }
      return result;
    }

    private static IEnumerable<DbExpression> FlattenAssociativeExpression(
      DbExpressionKind kind,
      DbExpression left,
      DbExpression right)
    {
      if (kind != DbExpressionKind.Or && kind != DbExpressionKind.And && (kind != DbExpressionKind.Plus && kind != DbExpressionKind.Multiply))
        return (IEnumerable<DbExpression>) new DbExpression[2]
        {
          left,
          right
        };
      List<DbExpression> argumentList = new List<DbExpression>();
      SqlGenerator.ExtractAssociativeArguments(kind, argumentList, left);
      SqlGenerator.ExtractAssociativeArguments(kind, argumentList, right);
      return (IEnumerable<DbExpression>) argumentList;
    }

    private static void ExtractAssociativeArguments(
      DbExpressionKind expressionKind,
      List<DbExpression> argumentList,
      DbExpression expression)
    {
      IEnumerable<DbExpression> leafNodes = expression.GetLeafNodes(expressionKind, (Func<DbExpression, IEnumerable<DbExpression>>) (exp =>
      {
        if (!(exp is DbBinaryExpression binaryExpression2))
          return (IEnumerable<DbExpression>) ((DbArithmeticExpression) exp).Arguments;
        return (IEnumerable<DbExpression>) new DbExpression[2]
        {
          binaryExpression2.Left,
          binaryExpression2.Right
        };
      }));
      argumentList.AddRange(leafNodes);
    }

    private SqlBuilder VisitComparisonExpression(
      string op,
      DbExpression left,
      DbExpression right)
    {
      SqlGenerator.RemoveUnnecessaryCasts(ref left, ref right);
      SqlBuilder result = new SqlBuilder();
      bool isCastOptional = left.ResultType.EdmType == right.ResultType.EdmType;
      if (left.ExpressionKind == DbExpressionKind.Constant)
        result.Append((object) this.VisitConstant((DbConstantExpression) left, isCastOptional));
      else
        this.ParenthesizeExpressionIfNeeded(left, result);
      result.Append((object) op);
      if (right.ExpressionKind == DbExpressionKind.Constant)
        result.Append((object) this.VisitConstant((DbConstantExpression) right, isCastOptional));
      else
        this.ParenthesizeExpressionIfNeeded(right, result);
      return result;
    }

    private static void RemoveUnnecessaryCasts(ref DbExpression left, ref DbExpression right)
    {
      if (left.ResultType.EdmType != right.ResultType.EdmType)
        return;
      if (left is DbCastExpression dbCastExpression1 && dbCastExpression1.Argument.ResultType.EdmType == left.ResultType.EdmType)
        left = dbCastExpression1.Argument;
      if (!(right is DbCastExpression dbCastExpression2) || dbCastExpression2.Argument.ResultType.EdmType != left.ResultType.EdmType)
        return;
      right = dbCastExpression2.Argument;
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
        SqlGenerator.WrapNonQueryExtent(result, sqlFragment, inputExpression.ExpressionKind);
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
        if (!SqlGenerator.IsCompatible(sqlSelectStatement, DbExpressionKind.Element))
        {
          TypeUsage elementTypeUsage = elementExpression.Argument.ResultType.GetElementTypeUsage();
          Symbol fromSymbol;
          sqlSelectStatement = this.CreateNewSelectStatement(sqlSelectStatement, "element", elementTypeUsage, out fromSymbol);
          this.AddFromSymbol(sqlSelectStatement, "element", fromSymbol, false);
        }
        sqlSelectStatement.Select.Top = new TopClause(1, false);
        return (ISqlFragment) sqlSelectStatement;
      }
      CollectionType edmType = (CollectionType) e.ResultType.EdmType;
      bool flag = BuiltInTypeKind.PrimitiveType == edmType.TypeUsage.EdmType.BuiltInTypeKind;
      SqlBuilder sqlBuilder = new SqlBuilder();
      string str = "";
      if (e.Arguments.Count == 0)
      {
        sqlBuilder.Append((object) " SELECT CAST(null as ");
        sqlBuilder.Append((object) this.GetSqlPrimitiveType(edmType.TypeUsage));
        sqlBuilder.Append((object) ") AS X FROM (SELECT 1) AS Y WHERE 1=0");
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
      if (e.Argument.ExpressionKind == DbExpressionKind.ParameterReference)
        this._ignoreForceNonUnicodeFlag = true;
      sqlBuilder.Append((object) e.Argument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
      this._ignoreForceNonUnicodeFlag = false;
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
      string str = "";
      bool flag = true;
      int count1 = inputs.Count;
      for (int index = 0; index < count1; ++index)
      {
        DbExpressionBinding input = inputs[index];
        if (str.Length != 0)
          result.From.AppendLine();
        result.From.Append((object) (str + " "));
        this.isParentAJoinStack.Push(input.Expression.ExpressionKind == DbExpressionKind.Scan || flag && (SqlGenerator.IsJoinExpression(input.Expression) || SqlGenerator.IsApplyExpression(input.Expression)));
        int count2 = result.FromExtents.Count;
        ISqlFragment fromExtentFragment = input.Expression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this);
        this.isParentAJoinStack.Pop();
        this.ProcessJoinInputResult(fromExtentFragment, result, input, count2);
        str = joinString;
        flag = false;
      }
      if (joinKind == DbExpressionKind.FullOuterJoin || joinKind == DbExpressionKind.InnerJoin || joinKind == DbExpressionKind.LeftOuterJoin)
      {
        result.From.Append((object) " ON ");
        this.isParentAJoinStack.Push(false);
        result.From.Append((object) joinCondition.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        this.isParentAJoinStack.Pop();
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
            if (SqlGenerator.IsJoinExpression(input.Expression) || SqlGenerator.IsApplyExpression(input.Expression))
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
            else
              fromSymbol = new Symbol(input.VariableName, input.VariableType, selectStatement2.OutputColumns, selectStatement2.OutputColumnsRenamed);
          }
          else
            fromSymbol = new Symbol(input.VariableName, input.VariableType, selectStatement2.OutputColumns, selectStatement2.OutputColumnsRenamed);
          result.From.Append((object) " (");
          result.From.Append((object) selectStatement2);
          result.From.Append((object) " )");
        }
        else if (input.Expression is DbScanExpression)
          result.From.Append((object) fromExtentFragment);
        else
          SqlGenerator.WrapNonQueryExtent(result, fromExtentFragment, input.Expression.ExpressionKind);
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

    private ISqlFragment VisitNewInstanceExpression(
      DbNewInstanceExpression e,
      bool aliasesNeedRenaming,
      out Dictionary<string, Symbol> newColumns)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (!(e.ResultType.EdmType is RowType edmType))
        throw new NotSupportedException();
      newColumns = new Dictionary<string, Symbol>(e.Arguments.Count);
      ReadOnlyMetadataCollection<EdmProperty> properties = edmType.Properties;
      string str = "";
      for (int index = 0; index < e.Arguments.Count; ++index)
      {
        DbExpression dbExpression = e.Arguments[index];
        if (BuiltInTypeKind.RowType == dbExpression.ResultType.EdmType.BuiltInTypeKind)
          throw new NotSupportedException();
        EdmProperty edmProperty = properties[index];
        sqlBuilder.Append((object) str);
        sqlBuilder.AppendLine();
        sqlBuilder.Append((object) dbExpression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) this));
        sqlBuilder.Append((object) " AS ");
        if (aliasesNeedRenaming)
        {
          Symbol symbol = new Symbol(edmProperty.Name, edmProperty.TypeUsage);
          symbol.NeedsRenaming = true;
          symbol.NewName = "Internal_" + edmProperty.Name;
          sqlBuilder.Append((object) symbol);
          newColumns.Add(edmProperty.Name, symbol);
        }
        else
          sqlBuilder.Append((object) SqlGenerator.QuoteIdentifier(edmProperty.Name));
        str = ", ";
      }
      return (ISqlFragment) sqlBuilder;
    }

    private ISqlFragment VisitSetOpExpression(
      DbBinaryExpression setOpExpression,
      string separator)
    {
      List<SqlSelectStatement> leafSelectStatements = new List<SqlSelectStatement>();
      this.VisitAndGatherSetOpLeafExpressions(setOpExpression.ExpressionKind, setOpExpression.Left, leafSelectStatements);
      this.VisitAndGatherSetOpLeafExpressions(setOpExpression.ExpressionKind, setOpExpression.Right, leafSelectStatements);
      SqlBuilder sqlBuilder = new SqlBuilder();
      for (int index = 0; index < leafSelectStatements.Count; ++index)
      {
        if (index > 0)
        {
          sqlBuilder.AppendLine();
          sqlBuilder.Append((object) separator);
          sqlBuilder.AppendLine();
        }
        sqlBuilder.Append((object) leafSelectStatements[index]);
      }
      if (!leafSelectStatements[0].OutputColumnsRenamed)
        return (ISqlFragment) sqlBuilder;
      SqlSelectStatement selectStatement = new SqlSelectStatement();
      selectStatement.From.Append((object) "( ");
      selectStatement.From.Append((object) sqlBuilder);
      selectStatement.From.AppendLine();
      selectStatement.From.Append((object) ") ");
      Symbol fromSymbol = new Symbol("X", setOpExpression.Left.ResultType.GetElementTypeUsage(), leafSelectStatements[0].OutputColumns, true);
      this.AddFromSymbol(selectStatement, (string) null, fromSymbol, false);
      return (ISqlFragment) selectStatement;
    }

    private void VisitAndGatherSetOpLeafExpressions(
      DbExpressionKind kind,
      DbExpression expression,
      List<SqlSelectStatement> leafSelectStatements)
    {
      if (this.SqlVersion > SqlVersion.Sql8 && (kind == DbExpressionKind.UnionAll || kind == DbExpressionKind.Intersect) && expression.ExpressionKind == kind)
      {
        DbBinaryExpression binaryExpression = (DbBinaryExpression) expression;
        this.VisitAndGatherSetOpLeafExpressions(kind, binaryExpression.Left, leafSelectStatements);
        this.VisitAndGatherSetOpLeafExpressions(kind, binaryExpression.Right, leafSelectStatements);
      }
      else
        leafSelectStatements.Add(this.VisitExpressionEnsureSqlStatement(expression, true, true));
    }

    private void AddColumns(
      SqlSelectStatement selectStatement,
      Symbol symbol,
      List<Symbol> columnList,
      Dictionary<string, Symbol> columnDictionary)
    {
      if (symbol is JoinSymbol joinSymbol)
      {
        if (!joinSymbol.IsNestedJoin)
        {
          foreach (Symbol extent in joinSymbol.ExtentList)
          {
            if (extent.Type != null && BuiltInTypeKind.PrimitiveType != extent.Type.EdmType.BuiltInTypeKind)
              this.AddColumns(selectStatement, extent, columnList, columnDictionary);
          }
        }
        else
        {
          foreach (Symbol column in joinSymbol.ColumnList)
          {
            OptionalColumn optionalColumn = this.CreateOptionalColumn((Symbol) null, column);
            optionalColumn.Append((object) symbol);
            optionalColumn.Append((object) ".");
            optionalColumn.Append((object) column);
            selectStatement.Select.AddOptionalColumn(optionalColumn);
            if (columnDictionary.ContainsKey(column.Name))
            {
              columnDictionary[column.Name].NeedsRenaming = true;
              column.NeedsRenaming = true;
            }
            else
              columnDictionary[column.Name] = column;
            columnList.Add(column);
          }
        }
      }
      else
      {
        if (symbol.OutputColumnsRenamed)
          selectStatement.OutputColumnsRenamed = true;
        if (selectStatement.OutputColumns == null)
          selectStatement.OutputColumns = new Dictionary<string, Symbol>();
        if (symbol.Type == null || BuiltInTypeKind.PrimitiveType == symbol.Type.EdmType.BuiltInTypeKind)
        {
          this.AddColumn(selectStatement, symbol, columnList, columnDictionary, "X");
        }
        else
        {
          foreach (EdmProperty property in symbol.Type.GetProperties())
            this.AddColumn(selectStatement, symbol, columnList, columnDictionary, property.Name);
        }
      }
    }

    private OptionalColumn CreateOptionalColumn(
      Symbol inputColumnSymbol,
      Symbol column)
    {
      if (!this.optionalColumnUsageManager.ContainsKey(column))
        this.optionalColumnUsageManager.Add(inputColumnSymbol, column);
      return new OptionalColumn(this.optionalColumnUsageManager, column);
    }

    private void AddColumn(
      SqlSelectStatement selectStatement,
      Symbol symbol,
      List<Symbol> columnList,
      Dictionary<string, Symbol> columnDictionary,
      string columnName)
    {
      this.allColumnNames[columnName] = 0;
      Symbol inputColumnSymbol = (Symbol) null;
      symbol.OutputColumns.TryGetValue(columnName, out inputColumnSymbol);
      Symbol column;
      if (!symbol.Columns.TryGetValue(columnName, out column))
      {
        column = inputColumnSymbol == null || !symbol.OutputColumnsRenamed ? new Symbol(columnName, (TypeUsage) null) : inputColumnSymbol;
        symbol.Columns.Add(columnName, column);
      }
      OptionalColumn optionalColumn = this.CreateOptionalColumn(inputColumnSymbol, column);
      optionalColumn.Append((object) symbol);
      optionalColumn.Append((object) ".");
      if (symbol.OutputColumnsRenamed)
        optionalColumn.Append((object) inputColumnSymbol);
      else
        optionalColumn.Append((object) SqlGenerator.QuoteIdentifier(columnName));
      optionalColumn.Append((object) " AS ");
      optionalColumn.Append((object) column);
      selectStatement.Select.AddOptionalColumn(optionalColumn);
      if (!selectStatement.OutputColumns.ContainsKey(columnName))
        selectStatement.OutputColumns.Add(columnName, column);
      if (columnDictionary.ContainsKey(columnName))
      {
        columnDictionary[columnName].NeedsRenaming = true;
        column.NeedsRenaming = true;
      }
      else
        columnDictionary[columnName] = symbol.Columns[columnName];
      columnList.Add(column);
    }

    private List<Symbol> AddDefaultColumns(SqlSelectStatement selectStatement)
    {
      List<Symbol> columnList = new List<Symbol>();
      Dictionary<string, Symbol> columnDictionary = new Dictionary<string, Symbol>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (Symbol fromExtent in selectStatement.FromExtents)
        this.AddColumns(selectStatement, fromExtent, columnList, columnDictionary);
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
      string str = "";
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
        fromSymbol = new Symbol(inputVarName, inputVarType, oldStatement.OutputColumns, oldStatement.OutputColumnsRenamed);
      SqlSelectStatement sqlSelectStatement = new SqlSelectStatement();
      sqlSelectStatement.From.Append((object) "( ");
      sqlSelectStatement.From.Append((object) oldStatement);
      sqlSelectStatement.From.AppendLine();
      sqlSelectStatement.From.Append((object) ") ");
      return sqlSelectStatement;
    }

    private static string EscapeSingleQuote(string s, bool isUnicode) => (isUnicode ? "N'" : "'") + s.Replace("'", "''") + "'";

    private string GetSqlPrimitiveType(TypeUsage type) => SqlGenerator.GenerateSqlForStoreType(this._sqlVersion, this._storeItemCollection.ProviderManifest.GetStoreType(type));

    internal static string GenerateSqlForStoreType(SqlVersion sqlVersion, TypeUsage storeTypeUsage)
    {
      string str = storeTypeUsage.EdmType.Name;
      int maxLength = 0;
      byte precision = 0;
      byte scale = 0;
      PrimitiveTypeKind primitiveTypeKind = ((PrimitiveType) storeTypeUsage.EdmType).PrimitiveTypeKind;
      switch (primitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          if (!storeTypeUsage.MustFacetBeConstant("MaxLength"))
          {
            storeTypeUsage.TryGetMaxLength(out maxLength);
            str = str + "(" + maxLength.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")";
            break;
          }
          break;
        case PrimitiveTypeKind.DateTime:
          str = SqlVersionUtils.IsPreKatmai(sqlVersion) ? "datetime" : "datetime2";
          break;
        case PrimitiveTypeKind.Decimal:
          if (!storeTypeUsage.MustFacetBeConstant("Precision"))
          {
            storeTypeUsage.TryGetPrecision(out precision);
            storeTypeUsage.TryGetScale(out scale);
            str = str + "(" + precision.ToString() + "," + scale.ToString() + ")";
            break;
          }
          break;
        case PrimitiveTypeKind.String:
          if (!storeTypeUsage.MustFacetBeConstant("MaxLength"))
          {
            storeTypeUsage.TryGetMaxLength(out maxLength);
            str = str + "(" + maxLength.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")";
            break;
          }
          break;
        case PrimitiveTypeKind.Time:
          SqlGenerator.AssertKatmaiOrNewer(sqlVersion, primitiveTypeKind);
          str = "time";
          break;
        case PrimitiveTypeKind.DateTimeOffset:
          SqlGenerator.AssertKatmaiOrNewer(sqlVersion, primitiveTypeKind);
          str = "datetimeoffset";
          break;
      }
      return str;
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

    private static bool IsApplyExpression(DbExpression e) => DbExpressionKind.CrossApply == e.ExpressionKind || DbExpressionKind.OuterApply == e.ExpressionKind;

    private static bool IsJoinExpression(DbExpression e) => DbExpressionKind.CrossJoin == e.ExpressionKind || DbExpressionKind.FullOuterJoin == e.ExpressionKind || DbExpressionKind.InnerJoin == e.ExpressionKind || DbExpressionKind.LeftOuterJoin == e.ExpressionKind;

    private static bool IsComplexExpression(DbExpression e)
    {
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.Cast:
        case DbExpressionKind.Constant:
        case DbExpressionKind.ParameterReference:
        case DbExpressionKind.Property:
          return false;
        default:
          return true;
      }
    }

    private static bool IsCompatible(SqlSelectStatement result, DbExpressionKind expressionKind)
    {
      switch (expressionKind)
      {
        case DbExpressionKind.Distinct:
          return result.Select.Top == null && result.Select.Skip == null && result.OrderBy.IsEmpty;
        case DbExpressionKind.Element:
        case DbExpressionKind.Limit:
          return result.Select.Top == null;
        case DbExpressionKind.Filter:
          return result.Select.IsEmpty && result.Where.IsEmpty && (result.GroupBy.IsEmpty && result.Select.Top == null) && result.Select.Skip == null;
        case DbExpressionKind.GroupBy:
          return result.Select.IsEmpty && result.GroupBy.IsEmpty && (result.OrderBy.IsEmpty && result.Select.Top == null) && result.Select.Skip == null && !result.Select.IsDistinct;
        case DbExpressionKind.Project:
          return result.Select.IsEmpty && result.GroupBy.IsEmpty && !result.Select.IsDistinct;
        case DbExpressionKind.Skip:
          return result.Select.IsEmpty && result.Select.Skip == null && (result.GroupBy.IsEmpty && result.OrderBy.IsEmpty) && !result.Select.IsDistinct;
        case DbExpressionKind.Sort:
          return result.Select.IsEmpty && result.GroupBy.IsEmpty && result.OrderBy.IsEmpty && !result.Select.IsDistinct;
        default:
          throw new InvalidOperationException(string.Empty);
      }
    }

    internal static string QuoteIdentifier(string name) => "[" + name.Replace("]", "]]") + "]";

    private SqlSelectStatement VisitExpressionEnsureSqlStatement(DbExpression e) => this.VisitExpressionEnsureSqlStatement(e, true, false);

    private SqlSelectStatement VisitExpressionEnsureSqlStatement(
      DbExpression e,
      bool addDefaultColumns,
      bool markAllDefaultColumnsAsUsed)
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
              inputVarType = e.ResultType.GetElementTypeUsage();
              break;
            default:
              inputVarType = ((CollectionType) e.ResultType.EdmType).TypeUsage;
              break;
          }
          Symbol fromSymbol;
          selectStatement = this.VisitInputExpression(e, inputVarName, inputVarType, out fromSymbol);
          this.AddFromSymbol(selectStatement, inputVarName, fromSymbol);
          this.symbolTable.ExitScope();
          break;
      }
      if (addDefaultColumns && selectStatement.Select.IsEmpty)
      {
        List<Symbol> symbolList = this.AddDefaultColumns(selectStatement);
        if (markAllDefaultColumnsAsUsed)
        {
          foreach (Symbol key in symbolList)
            this.optionalColumnUsageManager.MarkAsUsed(key);
        }
      }
      return selectStatement;
    }

    private SqlSelectStatement VisitFilterExpression(
      DbExpressionBinding input,
      DbExpression predicate,
      bool negatePredicate)
    {
      Symbol fromSymbol;
      SqlSelectStatement sqlSelectStatement = this.VisitInputExpression(input.Expression, input.VariableName, input.VariableType, out fromSymbol);
      if (!SqlGenerator.IsCompatible(sqlSelectStatement, DbExpressionKind.Filter))
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

    private static void WrapNonQueryExtent(
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

    private static string ByteArrayToBinaryString(byte[] binaryArray)
    {
      StringBuilder stringBuilder = new StringBuilder(binaryArray.Length * 2);
      for (int index = 0; index < binaryArray.Length; ++index)
        stringBuilder.Append(SqlGenerator._hexDigits[((int) binaryArray[index] & 240) >> 4]).Append(SqlGenerator._hexDigits[(int) binaryArray[index] & 15]);
      return stringBuilder.ToString();
    }

    private static bool GroupByAggregatesNeedInnerQuery(
      IList<DbAggregate> aggregates,
      string inputVarRefName)
    {
      foreach (DbAggregate aggregate in (IEnumerable<DbAggregate>) aggregates)
      {
        if (SqlGenerator.GroupByAggregateNeedsInnerQuery(aggregate.Arguments[0], inputVarRefName))
          return true;
      }
      return false;
    }

    private static bool GroupByAggregateNeedsInnerQuery(
      DbExpression expression,
      string inputVarRefName)
    {
      return SqlGenerator.GroupByExpressionNeedsInnerQuery(expression, inputVarRefName, true);
    }

    private static bool GroupByKeysNeedInnerQuery(IList<DbExpression> keys, string inputVarRefName)
    {
      foreach (DbExpression key in (IEnumerable<DbExpression>) keys)
      {
        if (SqlGenerator.GroupByKeyNeedsInnerQuery(key, inputVarRefName))
          return true;
      }
      return false;
    }

    private static bool GroupByKeyNeedsInnerQuery(DbExpression expression, string inputVarRefName) => SqlGenerator.GroupByExpressionNeedsInnerQuery(expression, inputVarRefName, false);

    private static bool GroupByExpressionNeedsInnerQuery(
      DbExpression expression,
      string inputVarRefName,
      bool allowConstants)
    {
      if (allowConstants && expression.ExpressionKind == DbExpressionKind.Constant)
        return false;
      if (expression.ExpressionKind == DbExpressionKind.Cast)
        return SqlGenerator.GroupByExpressionNeedsInnerQuery(((DbUnaryExpression) expression).Argument, inputVarRefName, allowConstants);
      if (expression.ExpressionKind == DbExpressionKind.Property)
        return SqlGenerator.GroupByExpressionNeedsInnerQuery(((DbPropertyExpression) expression).Instance, inputVarRefName, allowConstants);
      return expression.ExpressionKind != DbExpressionKind.VariableReference || !(expression as DbVariableReferenceExpression).VariableName.Equals(inputVarRefName);
    }

    private void AssertKatmaiOrNewer(PrimitiveTypeKind primitiveTypeKind) => SqlGenerator.AssertKatmaiOrNewer(this._sqlVersion, primitiveTypeKind);

    private static void AssertKatmaiOrNewer(
      SqlVersion sqlVersion,
      PrimitiveTypeKind primitiveTypeKind)
    {
      if (SqlVersionUtils.IsPreKatmai(sqlVersion))
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_PrimitiveTypeNotSupportedPriorSql10((object) primitiveTypeKind));
    }

    internal void AssertKatmaiOrNewer(DbFunctionExpression e)
    {
      if (this.IsPreKatmai)
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_CanonicalFunctionNotSupportedPriorSql10((object) e.Function.Name));
    }

    internal class KeyFieldExpressionComparer : IEqualityComparer<DbExpression>
    {
      public bool Equals(DbExpression x, DbExpression y)
      {
        if (x.ExpressionKind != y.ExpressionKind)
          return false;
        switch (x.ExpressionKind)
        {
          case DbExpressionKind.Cast:
            DbCastExpression dbCastExpression1 = (DbCastExpression) x;
            DbCastExpression dbCastExpression2 = (DbCastExpression) y;
            return dbCastExpression1.ResultType == dbCastExpression2.ResultType && this.Equals(dbCastExpression1.Argument, dbCastExpression2.Argument);
          case DbExpressionKind.ParameterReference:
            return ((DbParameterReferenceExpression) x).ParameterName == ((DbParameterReferenceExpression) y).ParameterName;
          case DbExpressionKind.Property:
            DbPropertyExpression propertyExpression1 = (DbPropertyExpression) x;
            DbPropertyExpression propertyExpression2 = (DbPropertyExpression) y;
            return propertyExpression1.Property == propertyExpression2.Property && this.Equals(propertyExpression1.Instance, propertyExpression2.Instance);
          case DbExpressionKind.VariableReference:
            return x == y;
          default:
            return false;
        }
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
