// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.SafeLink`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class SafeLink<TParent> where TParent : class
  {
    private TParent _value;

    public TParent Value => this._value;

    internal static IEnumerable<TChild> BindChildren<TChild>(
      TParent parent,
      Func<TChild, SafeLink<TParent>> getLink,
      IEnumerable<TChild> children)
    {
      foreach (TChild child in children)
        SafeLink<TParent>.BindChild<TChild>(parent, getLink, child);
      return children;
    }

    internal static TChild BindChild<TChild>(
      TParent parent,
      Func<TChild, SafeLink<TParent>> getLink,
      TChild child)
    {
      getLink(child)._value = parent;
      return child;
    }
  }
}
