// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.LeafOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class LeafOp : RulePatternOp
  {
    internal static readonly LeafOp Instance = new LeafOp();
    internal static readonly LeafOp Pattern = LeafOp.Instance;

    internal override int Arity => 0;

    private LeafOp()
      : base(OpType.Leaf)
    {
    }
  }
}
