// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Internal.NonClosingDelegatingStream
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;

namespace System.Net.Http.Internal
{
  internal class NonClosingDelegatingStream : DelegatingStream
  {
    public NonClosingDelegatingStream(Stream innerStream)
      : base(innerStream)
    {
    }

    public override void Close()
    {
    }
  }
}
