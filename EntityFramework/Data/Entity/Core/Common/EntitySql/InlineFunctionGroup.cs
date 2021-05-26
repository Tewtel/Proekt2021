// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.InlineFunctionGroup
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class InlineFunctionGroup : MetadataMember
  {
    internal readonly IList<InlineFunctionInfo> FunctionMetadata;

    internal InlineFunctionGroup(string name, IList<InlineFunctionInfo> functionMetadata)
      : base(MetadataMemberClass.InlineFunctionGroup, name)
    {
      this.FunctionMetadata = functionMetadata;
    }

    internal override string MetadataMemberClassName => InlineFunctionGroup.InlineFunctionGroupClassName;

    internal static string InlineFunctionGroupClassName => Strings.LocalizedInlineFunction;
  }
}
