﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.RuleProcessingContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class RuleProcessingContext
  {
    private readonly Command m_command;

    internal Command Command => this.m_command;

    internal virtual void PreProcess(Node node)
    {
    }

    internal virtual void PreProcessSubTree(Node node)
    {
    }

    internal virtual void PostProcess(Node node, Rule rule)
    {
    }

    internal virtual void PostProcessSubTree(Node node)
    {
    }

    internal virtual int GetHashCode(Node node) => node.GetHashCode();

    internal RuleProcessingContext(Command command) => this.m_command = command;
  }
}
