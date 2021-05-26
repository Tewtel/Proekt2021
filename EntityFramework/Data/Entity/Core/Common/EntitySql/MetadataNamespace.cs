// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.MetadataNamespace
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class MetadataNamespace : MetadataMember
  {
    internal MetadataNamespace(string name)
      : base(MetadataMemberClass.Namespace, name)
    {
    }

    internal override string MetadataMemberClassName => MetadataNamespace.NamespaceClassName;

    internal static string NamespaceClassName => Strings.LocalizedNamespace;
  }
}
