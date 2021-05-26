// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.MetadataFunctionGroup
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class MetadataFunctionGroup : MetadataMember
  {
    internal readonly IList<EdmFunction> FunctionMetadata;

    internal MetadataFunctionGroup(string name, IList<EdmFunction> functionMetadata)
      : base(MetadataMemberClass.FunctionGroup, name)
    {
      this.FunctionMetadata = functionMetadata;
    }

    internal override string MetadataMemberClassName => MetadataFunctionGroup.FunctionGroupClassName;

    internal static string FunctionGroupClassName => Strings.LocalizedFunction;
  }
}
