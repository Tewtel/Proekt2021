// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.SafeLinkCollection`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class SafeLinkCollection<TParent, TChild> : ReadOnlyMetadataCollection<TChild>
    where TParent : class
    where TChild : MetadataItem
  {
    public SafeLinkCollection(
      TParent parent,
      Func<TChild, SafeLink<TParent>> getLink,
      MetadataCollection<TChild> children)
      : base((MetadataCollection<TChild>) SafeLink<TParent>.BindChildren<TChild>(parent, getLink, (IEnumerable<TChild>) children))
    {
    }
  }
}
