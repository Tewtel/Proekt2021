﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.BuiltInExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class BuiltInExpr : Node
  {
    internal readonly BuiltInKind Kind;
    internal readonly string Name;
    internal readonly int ArgCount;
    internal readonly Node Arg1;
    internal readonly Node Arg2;
    internal readonly Node Arg3;
    internal readonly Node Arg4;

    private BuiltInExpr(BuiltInKind kind, string name)
    {
      this.Kind = kind;
      this.Name = name.ToUpperInvariant();
    }

    internal BuiltInExpr(BuiltInKind kind, string name, Node arg1)
      : this(kind, name)
    {
      this.ArgCount = 1;
      this.Arg1 = arg1;
    }

    internal BuiltInExpr(BuiltInKind kind, string name, Node arg1, Node arg2)
      : this(kind, name)
    {
      this.ArgCount = 2;
      this.Arg1 = arg1;
      this.Arg2 = arg2;
    }

    internal BuiltInExpr(BuiltInKind kind, string name, Node arg1, Node arg2, Node arg3)
      : this(kind, name)
    {
      this.ArgCount = 3;
      this.Arg1 = arg1;
      this.Arg2 = arg2;
      this.Arg3 = arg3;
    }

    internal BuiltInExpr(
      BuiltInKind kind,
      string name,
      Node arg1,
      Node arg2,
      Node arg3,
      Node arg4)
      : this(kind, name)
    {
      this.ArgCount = 4;
      this.Arg1 = arg1;
      this.Arg2 = arg2;
      this.Arg3 = arg3;
      this.Arg4 = arg4;
    }
  }
}
