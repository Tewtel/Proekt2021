// Decompiled with JetBrains decompiler
// Type: System.Diagnostics.CodeAnalysis.DoesNotReturnIfAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace System.Diagnostics.CodeAnalysis
{
  /// <summary>
  /// Specifies that the method will not return if the associated Boolean parameter is passed the specified value.
  /// </summary>
  [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
  internal class DoesNotReturnIfAttribute : Attribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Diagnostics.CodeAnalysis.DoesNotReturnIfAttribute" /> class.
    /// </summary>
    /// <param name="parameterValue">
    /// The condition parameter value. Code after the method will be considered unreachable by diagnostics if the argument to
    /// the associated parameter matches this value.
    /// </param>
    public DoesNotReturnIfAttribute(bool parameterValue) => this.ParameterValue = parameterValue;

    /// <summary>Gets the condition parameter value.</summary>
    public bool ParameterValue { get; }
  }
}
