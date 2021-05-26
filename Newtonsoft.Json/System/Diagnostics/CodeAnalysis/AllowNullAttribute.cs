// Decompiled with JetBrains decompiler
// Type: System.Diagnostics.CodeAnalysis.AllowNullAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace System.Diagnostics.CodeAnalysis
{
  /// <summary>Specifies that null is allowed as an input even if the corresponding type disallows it.</summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
  internal sealed class AllowNullAttribute : Attribute
  {
  }
}
