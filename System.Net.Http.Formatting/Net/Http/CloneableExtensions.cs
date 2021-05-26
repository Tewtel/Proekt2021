// Decompiled with JetBrains decompiler
// Type: System.Net.Http.CloneableExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

namespace System.Net.Http
{
  internal static class CloneableExtensions
  {
    internal static T Clone<T>(this T value) where T : ICloneable => (T) value.Clone();
  }
}
