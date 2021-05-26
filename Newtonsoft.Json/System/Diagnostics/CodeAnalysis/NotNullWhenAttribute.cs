// Decompiled with JetBrains decompiler
// Type: System.Diagnostics.CodeAnalysis.NotNullWhenAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace System.Diagnostics.CodeAnalysis
{
  /// <summary>Specifies that when a method returns <see cref="P:System.Diagnostics.CodeAnalysis.NotNullWhenAttribute.ReturnValue" />, the parameter will not be null even if the corresponding type allows it.</summary>
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
  internal sealed class NotNullWhenAttribute : Attribute
  {
    /// <summary>Initializes the attribute with the specified return value condition.</summary>
    /// <param name="returnValue">
    /// The return value condition. If the method returns this value, the associated parameter will not be null.
    /// </param>
    public NotNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;

    /// <summary>Gets the return value condition.</summary>
    public bool ReturnValue { get; }
  }
}
