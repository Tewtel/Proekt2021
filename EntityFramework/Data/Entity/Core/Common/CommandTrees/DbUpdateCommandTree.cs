// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbUpdateCommandTree
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees.Internal;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>Represents a single-row update operation expressed as a command tree. This class cannot be inherited.  </summary>
  /// <remarks>
  /// Represents a single-row update operation expressed as a canonical command tree.
  /// When the <see cref="P:System.Data.Entity.Core.Common.CommandTrees.DbUpdateCommandTree.Returning" /> property is set, the command returns a reader; otherwise,
  /// it returns a scalar indicating the number of rows affected.
  /// </remarks>
  public sealed class DbUpdateCommandTree : DbModificationCommandTree
  {
    private readonly DbExpression _predicate;
    private readonly DbExpression _returning;
    private readonly ReadOnlyCollection<DbModificationClause> _setClauses;

    internal DbUpdateCommandTree()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbUpdateCommandTree" /> class.
    /// </summary>
    /// <param name="metadata">The model this command will operate on.</param>
    /// <param name="dataSpace">The data space.</param>
    /// <param name="target">The target table for the data manipulation language (DML) operation.</param>
    /// <param name="predicate">A predicate used to determine which members of the target collection should be updated.</param>
    /// <param name="setClauses">The list of update set clauses that define the update operation.</param>
    /// <param name="returning">A <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies a projection of results to be returned, based on the modified rows.</param>
    public DbUpdateCommandTree(
      MetadataWorkspace metadata,
      DataSpace dataSpace,
      DbExpressionBinding target,
      DbExpression predicate,
      ReadOnlyCollection<DbModificationClause> setClauses,
      DbExpression returning)
      : base(metadata, dataSpace, target)
    {
      this._predicate = predicate;
      this._setClauses = setClauses;
      this._returning = returning;
    }

    /// <summary>Gets the list of update set clauses that define the update operation.</summary>
    /// <returns>The list of update set clauses that define the update operation.</returns>
    public IList<DbModificationClause> SetClauses => (IList<DbModificationClause>) this._setClauses;

    /// <summary>
    /// Gets an <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies a projection of results to be returned, based on the modified rows.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies a projection of results to be returned based, on the modified rows. null indicates that no results should be returned from this command.
    /// </returns>
    public DbExpression Returning => this._returning;

    /// <summary>
    /// Gets an <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies the predicate used to determine which members of the target collection should be updated.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies the predicate used to determine which members of the target collection should be updated.
    /// </returns>
    public DbExpression Predicate => this._predicate;

    /// <summary>Gets the kind of this command tree.</summary>
    /// <returns>The kind of this command tree.</returns>
    public override DbCommandTreeKind CommandTreeKind => DbCommandTreeKind.Update;

    internal override bool HasReader => this.Returning != null;

    internal override void DumpStructure(ExpressionDumper dumper)
    {
      base.DumpStructure(dumper);
      if (this.Predicate != null)
        dumper.Dump(this.Predicate, "Predicate");
      dumper.Begin("SetClauses", (Dictionary<string, object>) null);
      foreach (DbModificationClause setClause in (IEnumerable<DbModificationClause>) this.SetClauses)
        setClause?.DumpStructure(dumper);
      dumper.End("SetClauses");
      dumper.Dump(this.Returning, "Returning");
    }

    internal override string PrintTree(ExpressionPrinter printer) => printer.Print(this);
  }
}
