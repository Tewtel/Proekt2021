﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.MetadataType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class MetadataType : MetadataMember
  {
    internal readonly TypeUsage TypeUsage;

    internal MetadataType(string name, TypeUsage typeUsage)
      : base(MetadataMemberClass.Type, name)
    {
      this.TypeUsage = typeUsage;
    }

    internal override string MetadataMemberClassName => MetadataType.TypeClassName;

    internal static string TypeClassName => Strings.LocalizedType;
  }
}
