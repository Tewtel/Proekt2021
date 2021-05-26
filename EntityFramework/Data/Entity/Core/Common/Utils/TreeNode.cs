// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.TreeNode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Text;

namespace System.Data.Entity.Core.Common.Utils
{
  internal class TreeNode
  {
    private readonly StringBuilder _text;
    private readonly List<TreeNode> _children = new List<TreeNode>();

    internal TreeNode() => this._text = new StringBuilder();

    internal TreeNode(string text, params TreeNode[] children)
    {
      this._text = !string.IsNullOrEmpty(text) ? new StringBuilder(text) : new StringBuilder();
      if (children == null)
        return;
      this._children.AddRange((IEnumerable<TreeNode>) children);
    }

    internal TreeNode(string text, List<TreeNode> children)
      : this(text)
    {
      if (children == null)
        return;
      this._children.AddRange((IEnumerable<TreeNode>) children);
    }

    internal StringBuilder Text => this._text;

    internal IList<TreeNode> Children => (IList<TreeNode>) this._children;

    internal int Position { get; set; }
  }
}
