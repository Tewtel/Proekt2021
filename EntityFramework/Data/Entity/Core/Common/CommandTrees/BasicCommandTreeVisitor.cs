// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.BasicCommandTreeVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>
  /// An abstract base type for types that implement the IExpressionVisitor interface to derive from.
  /// </summary>
  public abstract class BasicCommandTreeVisitor : BasicExpressionVisitor
  {
    /// <summary>Implements the visitor pattern for the set clause.</summary>
    /// <param name="setClause">The set clause.</param>
    protected virtual void VisitSetClause(DbSetClause setClause)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSetClause>(setClause, nameof (setClause));
      this.VisitExpression(setClause.Property);
      this.VisitExpression(setClause.Value);
    }

    /// <summary>Implements the visitor pattern for the modification clause.</summary>
    /// <param name="modificationClause">The modification clause.</param>
    protected virtual void VisitModificationClause(DbModificationClause modificationClause)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbModificationClause>(modificationClause, nameof (modificationClause));
      this.VisitSetClause((DbSetClause) modificationClause);
    }

    /// <summary>Implements the visitor pattern for the collection of modification clauses.</summary>
    /// <param name="modificationClauses">The modification clauses.</param>
    protected virtual void VisitModificationClauses(IList<DbModificationClause> modificationClauses)
    {
      System.Data.Entity.Utilities.Check.NotNull<IList<DbModificationClause>>(modificationClauses, nameof (modificationClauses));
      for (int index = 0; index < modificationClauses.Count; ++index)
        this.VisitModificationClause(modificationClauses[index]);
    }

    /// <summary>Implements the visitor pattern for the command tree.</summary>
    /// <param name="commandTree">The command tree.</param>
    public virtual void VisitCommandTree(DbCommandTree commandTree)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommandTree>(commandTree, nameof (commandTree));
      switch (commandTree.CommandTreeKind)
      {
        case DbCommandTreeKind.Query:
          this.VisitQueryCommandTree((DbQueryCommandTree) commandTree);
          break;
        case DbCommandTreeKind.Update:
          this.VisitUpdateCommandTree((DbUpdateCommandTree) commandTree);
          break;
        case DbCommandTreeKind.Insert:
          this.VisitInsertCommandTree((DbInsertCommandTree) commandTree);
          break;
        case DbCommandTreeKind.Delete:
          this.VisitDeleteCommandTree((DbDeleteCommandTree) commandTree);
          break;
        case DbCommandTreeKind.Function:
          this.VisitFunctionCommandTree((DbFunctionCommandTree) commandTree);
          break;
        default:
          throw new NotSupportedException();
      }
    }

    /// <summary>Implements the visitor pattern for the delete command tree.</summary>
    /// <param name="deleteTree">The delete command tree.</param>
    protected virtual void VisitDeleteCommandTree(DbDeleteCommandTree deleteTree)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDeleteCommandTree>(deleteTree, nameof (deleteTree));
      this.VisitExpressionBindingPre(deleteTree.Target);
      this.VisitExpression(deleteTree.Predicate);
      this.VisitExpressionBindingPost(deleteTree.Target);
    }

    /// <summary>Implements the visitor pattern for the function command tree.</summary>
    /// <param name="functionTree">The function command tree.</param>
    protected virtual void VisitFunctionCommandTree(DbFunctionCommandTree functionTree) => System.Data.Entity.Utilities.Check.NotNull<DbFunctionCommandTree>(functionTree, nameof (functionTree));

    /// <summary>Implements the visitor pattern for the insert command tree.</summary>
    /// <param name="insertTree">The insert command tree.</param>
    protected virtual void VisitInsertCommandTree(DbInsertCommandTree insertTree)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInsertCommandTree>(insertTree, nameof (insertTree));
      this.VisitExpressionBindingPre(insertTree.Target);
      this.VisitModificationClauses(insertTree.SetClauses);
      if (insertTree.Returning != null)
        this.VisitExpression(insertTree.Returning);
      this.VisitExpressionBindingPost(insertTree.Target);
    }

    /// <summary>Implements the visitor pattern for the query command tree.</summary>
    /// <param name="queryTree">The query command tree.</param>
    protected virtual void VisitQueryCommandTree(DbQueryCommandTree queryTree)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbQueryCommandTree>(queryTree, nameof (queryTree));
      this.VisitExpression(queryTree.Query);
    }

    /// <summary>Implements the visitor pattern for the update command tree.</summary>
    /// <param name="updateTree">The update command tree.</param>
    protected virtual void VisitUpdateCommandTree(DbUpdateCommandTree updateTree)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbUpdateCommandTree>(updateTree, nameof (updateTree));
      this.VisitExpressionBindingPre(updateTree.Target);
      this.VisitModificationClauses(updateTree.SetClauses);
      this.VisitExpression(updateTree.Predicate);
      if (updateTree.Returning != null)
        this.VisitExpression(updateTree.Returning);
      this.VisitExpressionBindingPost(updateTree.Target);
    }
  }
}
