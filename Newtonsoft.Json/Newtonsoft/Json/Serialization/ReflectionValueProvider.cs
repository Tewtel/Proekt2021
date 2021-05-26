// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionValueProvider
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Get and set values for a <see cref="T:System.Reflection.MemberInfo" /> using reflection.
  /// </summary>
  public class ReflectionValueProvider : IValueProvider
  {
    private readonly MemberInfo _memberInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.ReflectionValueProvider" /> class.
    /// </summary>
    /// <param name="memberInfo">The member info.</param>
    public ReflectionValueProvider(MemberInfo memberInfo)
    {
      ValidationUtils.ArgumentNotNull((object) memberInfo, nameof (memberInfo));
      this._memberInfo = memberInfo;
    }

    /// <summary>Sets the value.</summary>
    /// <param name="target">The target to set the value on.</param>
    /// <param name="value">The value to set on the target.</param>
    public void SetValue(object target, object? value)
    {
      try
      {
        ReflectionUtils.SetMemberValue(this._memberInfo, target, value);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }

    /// <summary>Gets the value.</summary>
    /// <param name="target">The target to get the value from.</param>
    /// <returns>The value.</returns>
    public object? GetValue(object target)
    {
      try
      {
        PropertyInfo memberInfo = this._memberInfo as PropertyInfo;
        if ((object) memberInfo != null && memberInfo.PropertyType.IsByRef)
          throw new InvalidOperationException("Could not create getter for {0}. ByRef return values are not supported.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo));
        return ReflectionUtils.GetMemberValue(this._memberInfo, target);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }
  }
}
