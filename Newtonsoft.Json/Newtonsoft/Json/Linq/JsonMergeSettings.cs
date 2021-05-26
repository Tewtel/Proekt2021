// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonMergeSettings
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies the settings used when merging JSON.</summary>
  public class JsonMergeSettings
  {
    private MergeArrayHandling _mergeArrayHandling;
    private MergeNullValueHandling _mergeNullValueHandling;
    private StringComparison _propertyNameComparison;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JsonMergeSettings" /> class.
    /// </summary>
    public JsonMergeSettings() => this._propertyNameComparison = StringComparison.Ordinal;

    /// <summary>
    /// Gets or sets the method used when merging JSON arrays.
    /// </summary>
    /// <value>The method used when merging JSON arrays.</value>
    public MergeArrayHandling MergeArrayHandling
    {
      get => this._mergeArrayHandling;
      set => this._mergeArrayHandling = value >= MergeArrayHandling.Concat && value <= MergeArrayHandling.Merge ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    /// <summary>Gets or sets how null value properties are merged.</summary>
    /// <value>How null value properties are merged.</value>
    public MergeNullValueHandling MergeNullValueHandling
    {
      get => this._mergeNullValueHandling;
      set => this._mergeNullValueHandling = value >= MergeNullValueHandling.Ignore && value <= MergeNullValueHandling.Merge ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    /// <summary>
    /// Gets or sets the comparison used to match property names while merging.
    /// The exact property name will be searched for first and if no matching property is found then
    /// the <see cref="T:System.StringComparison" /> will be used to match a property.
    /// </summary>
    /// <value>The comparison used to match property names while merging.</value>
    public StringComparison PropertyNameComparison
    {
      get => this._propertyNameComparison;
      set => this._propertyNameComparison = value >= StringComparison.CurrentCulture && value <= StringComparison.OrdinalIgnoreCase ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }
}
