// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityClient.NameValuePair
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.EntityClient
{
  internal sealed class NameValuePair
  {
    private NameValuePair _next;

    internal NameValuePair Next
    {
      get => this._next;
      set => this._next = this._next == null && value != null ? value : throw new InvalidOperationException(Strings.ADP_InternalProviderError((object) 1014));
    }
  }
}
