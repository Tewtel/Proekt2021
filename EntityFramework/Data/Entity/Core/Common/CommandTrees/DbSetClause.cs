// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbSetClause
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees.Internal;
using System.Data.Entity.Core.Common.Utils;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>Specifies the clause in a modification operation that sets the value of a property. This class cannot be inherited. </summary>
  public sealed class DbSetClause : DbModificationClause
  {
    private readonly DbExpression _prop;
    private readonly DbExpression _val;

    internal DbSetClause(DbExpression targetProperty, DbExpression sourceValue)
    {
      this._prop = targetProperty;
      this._val = sourceValue;
    }

    /// <summary>
    /// Gets an <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies the property that should be updated.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies the property that should be updated.
    /// </returns>
    public DbExpression Property => this._prop;

    /// <summary>
    /// Gets an <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies the new value with which to update the property.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that specifies the new value with which to update the property.
    /// </returns>
    public DbExpression Value => this._val;

    internal override void DumpStructure(ExpressionDumper dumper)
    {
      dumper.Begin(nameof (DbSetClause));
      if (this.Property != null)
        dumper.Dump(this.Property, "Property");
      if (this.Value != null)
        dumper.Dump(this.Value, "Value");
      dumper.End(nameof (DbSetClause));
    }

    internal override TreeNode Print(DbExpressionVisitor<TreeNode> visitor)
    {
      TreeNode treeNode = new TreeNode(nameof (DbSetClause), new TreeNode[0]);
      if (this.Property != null)
        treeNode.Children.Add(new TreeNode("Property", new TreeNode[1]
        {
          this.Property.Accept<TreeNode>(visitor)
        }));
      if (this.Value != null)
        treeNode.Children.Add(new TreeNode("Value", new TreeNode[1]
        {
          this.Value.Accept<TreeNode>(visitor)
        }));
      return treeNode;
    }
  }
}
