// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonLoadSettings
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies the settings used when loading JSON.</summary>
  public class JsonLoadSettings
  {
    private CommentHandling _commentHandling;
    private LineInfoHandling _lineInfoHandling;
    private DuplicatePropertyNameHandling _duplicatePropertyNameHandling;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> class.
    /// </summary>
    public JsonLoadSettings()
    {
      this._lineInfoHandling = LineInfoHandling.Load;
      this._commentHandling = CommentHandling.Ignore;
      this._duplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace;
    }

    /// <summary>
    /// Gets or sets how JSON comments are handled when loading JSON.
    /// The default value is <see cref="F:Newtonsoft.Json.Linq.CommentHandling.Ignore" />.
    /// </summary>
    /// <value>The JSON comment handling.</value>
    public CommentHandling CommentHandling
    {
      get => this._commentHandling;
      set => this._commentHandling = value >= CommentHandling.Ignore && value <= CommentHandling.Load ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    /// <summary>
    /// Gets or sets how JSON line info is handled when loading JSON.
    /// The default value is <see cref="F:Newtonsoft.Json.Linq.LineInfoHandling.Load" />.
    /// </summary>
    /// <value>The JSON line info handling.</value>
    public LineInfoHandling LineInfoHandling
    {
      get => this._lineInfoHandling;
      set => this._lineInfoHandling = value >= LineInfoHandling.Ignore && value <= LineInfoHandling.Load ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    /// <summary>
    /// Gets or sets how duplicate property names in JSON objects are handled when loading JSON.
    /// The default value is <see cref="F:Newtonsoft.Json.Linq.DuplicatePropertyNameHandling.Replace" />.
    /// </summary>
    /// <value>The JSON duplicate property name handling.</value>
    public DuplicatePropertyNameHandling DuplicatePropertyNameHandling
    {
      get => this._duplicatePropertyNameHandling;
      set => this._duplicatePropertyNameHandling = value >= DuplicatePropertyNameHandling.Replace && value <= DuplicatePropertyNameHandling.Error ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }
}
