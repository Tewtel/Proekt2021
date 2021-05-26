// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionAttributeProvider
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Provides methods to get attributes from a <see cref="T:System.Type" />, <see cref="T:System.Reflection.MemberInfo" />, <see cref="T:System.Reflection.ParameterInfo" /> or <see cref="T:System.Reflection.Assembly" />.
  /// </summary>
  public class ReflectionAttributeProvider : IAttributeProvider
  {
    private readonly object _attributeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.ReflectionAttributeProvider" /> class.
    /// </summary>
    /// <param name="attributeProvider">The instance to get attributes for. This parameter should be a <see cref="T:System.Type" />, <see cref="T:System.Reflection.MemberInfo" />, <see cref="T:System.Reflection.ParameterInfo" /> or <see cref="T:System.Reflection.Assembly" />.</param>
    public ReflectionAttributeProvider(object attributeProvider)
    {
      ValidationUtils.ArgumentNotNull(attributeProvider, nameof (attributeProvider));
      this._attributeProvider = attributeProvider;
    }

    /// <summary>
    /// Returns a collection of all of the attributes, or an empty collection if there are no attributes.
    /// </summary>
    /// <param name="inherit">When <c>true</c>, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>A collection of <see cref="T:System.Attribute" />s, or an empty collection.</returns>
    public IList<Attribute> GetAttributes(bool inherit) => (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, (Type) null, inherit);

    /// <summary>
    /// Returns a collection of attributes, identified by type, or an empty collection if there are no attributes.
    /// </summary>
    /// <param name="attributeType">The type of the attributes.</param>
    /// <param name="inherit">When <c>true</c>, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>A collection of <see cref="T:System.Attribute" />s, or an empty collection.</returns>
    public IList<Attribute> GetAttributes(Type attributeType, bool inherit) => (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
  }
}
