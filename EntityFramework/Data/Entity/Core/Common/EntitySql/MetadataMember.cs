﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.MetadataMember
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal abstract class MetadataMember : ExpressionResolution
  {
    internal readonly MetadataMemberClass MetadataMemberClass;
    internal readonly string Name;

    protected MetadataMember(MetadataMemberClass @class, string name)
      : base(ExpressionResolutionClass.MetadataMember)
    {
      this.MetadataMemberClass = @class;
      this.Name = name;
    }

    internal override string ExpressionClassName => MetadataMember.MetadataMemberExpressionClassName;

    internal static string MetadataMemberExpressionClassName => Strings.LocalizedMetadataMemberExpression;

    internal abstract string MetadataMemberClassName { get; }

    internal static IEqualityComparer<MetadataMember> CreateMetadataMemberNameEqualityComparer(
      StringComparer stringComparer)
    {
      return (IEqualityComparer<MetadataMember>) new MetadataMember.MetadataMemberNameEqualityComparer(stringComparer);
    }

    private sealed class MetadataMemberNameEqualityComparer : IEqualityComparer<MetadataMember>
    {
      private readonly StringComparer _stringComparer;

      internal MetadataMemberNameEqualityComparer(StringComparer stringComparer) => this._stringComparer = stringComparer;

      bool IEqualityComparer<MetadataMember>.Equals(
        MetadataMember x,
        MetadataMember y)
      {
        return this._stringComparer.Equals(x.Name, y.Name);
      }

      int IEqualityComparer<MetadataMember>.GetHashCode(
        MetadataMember obj)
      {
        return this._stringComparer.GetHashCode(obj.Name);
      }
    }
  }
}
