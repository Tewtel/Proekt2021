// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbQueryCommandTree
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees.Internal;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>Represents a query operation expressed as a command tree. This class cannot be inherited.  </summary>
  public sealed class DbQueryCommandTree : DbCommandTree
  {
    private readonly DbExpression _query;
    private ReadOnlyCollection<DbParameterReferenceExpression> _parameters;

    /// <summary>
    /// Constructs a new DbQueryCommandTree that uses the specified metadata workspace.
    /// </summary>
    /// <param name="metadata"> The metadata workspace that the command tree should use. </param>
    /// <param name="dataSpace"> The logical 'space' that metadata in the expressions used in this command tree must belong to. </param>
    /// <param name="query">
    /// A <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the logic of the query.
    /// </param>
    /// <param name="validate"> When set to false the validation of the tree is turned off. </param>
    /// <param name="useDatabaseNullSemantics">A boolean that indicates whether database null semantics are exhibited when comparing
    /// two operands, both of which are potentially nullable.</param>
    /// <param name="disableFilterOverProjectionSimplificationForCustomFunctions">A boolean that indicates whether
    /// filter over projection simplification should be used.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="metadata" />
    /// or
    /// <paramref name="query" />
    /// is null
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="dataSpace" />
    /// does not represent a valid data space
    /// </exception>
    public DbQueryCommandTree(
      MetadataWorkspace metadata,
      DataSpace dataSpace,
      DbExpression query,
      bool validate,
      bool useDatabaseNullSemantics,
      bool disableFilterOverProjectionSimplificationForCustomFunctions)
      : base(metadata, dataSpace, useDatabaseNullSemantics, disableFilterOverProjectionSimplificationForCustomFunctions)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(query, nameof (query));
      if (validate)
      {
        DbExpressionValidator expressionValidator = new DbExpressionValidator(metadata, dataSpace);
        expressionValidator.ValidateExpression(query, nameof (query));
        this._parameters = new ReadOnlyCollection<DbParameterReferenceExpression>((IList<DbParameterReferenceExpression>) expressionValidator.Parameters.Select<KeyValuePair<string, DbParameterReferenceExpression>, DbParameterReferenceExpression>((Func<KeyValuePair<string, DbParameterReferenceExpression>, DbParameterReferenceExpression>) (paramInfo => paramInfo.Value)).ToList<DbParameterReferenceExpression>());
      }
      this._query = query;
    }

    /// <summary>
    /// Constructs a new DbQueryCommandTree that uses the specified metadata workspace.
    /// </summary>
    /// <param name="metadata"> The metadata workspace that the command tree should use. </param>
    /// <param name="dataSpace"> The logical 'space' that metadata in the expressions used in this command tree must belong to. </param>
    /// <param name="query">
    /// A <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the logic of the query.
    /// </param>
    /// <param name="validate"> When set to false the validation of the tree is turned off. </param>
    /// <param name="useDatabaseNullSemantics">A boolean that indicates whether database null semantics are exhibited when comparing
    /// two operands, both of which are potentially nullable.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="metadata" />
    /// or
    /// <paramref name="query" />
    /// is null
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="dataSpace" />
    /// does not represent a valid data space
    /// </exception>
    public DbQueryCommandTree(
      MetadataWorkspace metadata,
      DataSpace dataSpace,
      DbExpression query,
      bool validate,
      bool useDatabaseNullSemantics)
      : this(metadata, dataSpace, query, validate, useDatabaseNullSemantics, false)
    {
    }

    /// <summary>
    /// Constructs a new DbQueryCommandTree that uses the specified metadata workspace, using database null semantics.
    /// </summary>
    /// <param name="metadata"> The metadata workspace that the command tree should use. </param>
    /// <param name="dataSpace"> The logical 'space' that metadata in the expressions used in this command tree must belong to. </param>
    /// <param name="query">
    /// A <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the logic of the query.
    /// </param>
    /// <param name="validate"> When set to false the validation of the tree is turned off. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="metadata" />
    /// or
    /// <paramref name="query" />
    /// is null
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="dataSpace" />
    /// does not represent a valid data space
    /// </exception>
    public DbQueryCommandTree(
      MetadataWorkspace metadata,
      DataSpace dataSpace,
      DbExpression query,
      bool validate)
      : this(metadata, dataSpace, query, validate, true, false)
    {
    }

    /// <summary>
    /// Constructs a new DbQueryCommandTree that uses the specified metadata workspace, using database null semantics.
    /// </summary>
    /// <param name="metadata"> The metadata workspace that the command tree should use. </param>
    /// <param name="dataSpace"> The logical 'space' that metadata in the expressions used in this command tree must belong to. </param>
    /// <param name="query">
    /// A <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the logic of the query.
    /// </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="metadata" />
    /// or
    /// <paramref name="query" />
    /// is null
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="dataSpace" />
    /// does not represent a valid data space
    /// </exception>
    public DbQueryCommandTree(MetadataWorkspace metadata, DataSpace dataSpace, DbExpression query)
      : this(metadata, dataSpace, query, true, true, false)
    {
    }

    /// <summary>
    /// Gets an <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the logic of the query operation.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the logic of the query operation.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">The expression is null.</exception>
    /// <exception cref="T:System.ArgumentException">The expression is associated with a different command tree.</exception>
    public DbExpression Query => this._query;

    /// <summary>Gets the kind of this command tree.</summary>
    /// <returns>The kind of this command tree.</returns>
    public override DbCommandTreeKind CommandTreeKind => DbCommandTreeKind.Query;

    internal override IEnumerable<KeyValuePair<string, TypeUsage>> GetParameters()
    {
      if (this._parameters == null)
        this._parameters = ParameterRetriever.GetParameters((DbCommandTree) this);
      return this._parameters.Select<DbParameterReferenceExpression, KeyValuePair<string, TypeUsage>>((Func<DbParameterReferenceExpression, KeyValuePair<string, TypeUsage>>) (p => new KeyValuePair<string, TypeUsage>(p.ParameterName, p.ResultType)));
    }

    internal override void DumpStructure(ExpressionDumper dumper)
    {
      if (this.Query == null)
        return;
      dumper.Dump(this.Query, "Query");
    }

    internal override string PrintTree(ExpressionPrinter printer) => printer.Print(this);

    internal static DbQueryCommandTree FromValidExpression(
      MetadataWorkspace metadata,
      DataSpace dataSpace,
      DbExpression query,
      bool useDatabaseNullSemantics,
      bool disableFilterOverProjectionSimplificationForCustomFunctions)
    {
      return new DbQueryCommandTree(metadata, dataSpace, query, false, useDatabaseNullSemantics, disableFilterOverProjectionSimplificationForCustomFunctions);
    }
  }
}
