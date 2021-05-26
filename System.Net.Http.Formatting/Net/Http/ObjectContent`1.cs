// Decompiled with JetBrains decompiler
// Type: System.Net.Http.ObjectContent`1
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace System.Net.Http
{
  /// <summary> Generic form of <see cref="T:System.Net.Http.ObjectContent" />. </summary>
  /// <typeparam name="T">The type of object this  class will contain.</typeparam>
  public class ObjectContent<T> : ObjectContent
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.ObjectContent`1" /> class.</summary>
    /// <param name="value">The value of the object this instance will contain.</param>
    /// <param name="formatter">The formatter to use when serializing the value.</param>
    public ObjectContent(T value, MediaTypeFormatter formatter)
      : this(value, formatter, (MediaTypeHeaderValue) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.ObjectContent`1" /> class.</summary>
    /// <param name="value">The value of the object this instance will contain.</param>
    /// <param name="formatter">The formatter to use when serializing the value.</param>
    /// <param name="mediaType">The authoritative value of the Content-Type header.</param>
    public ObjectContent(T value, MediaTypeFormatter formatter, string mediaType)
      : this(value, formatter, ObjectContent.BuildHeaderValue(mediaType))
    {
    }

    /// <summary> Initializes a new instance of the &lt;see cref="T:System.Net.Http.ObjectContent`1" /&gt; class. </summary>
    /// <param name="value">The value of the object this instance will contain.</param>
    /// <param name="formatter">The formatter to use when serializing the value.</param>
    /// <param name="mediaType">The authoritative value of the Content-Type header. Can be null, in which case the default content type of the formatter will be used.</param>
    public ObjectContent(T value, MediaTypeFormatter formatter, MediaTypeHeaderValue mediaType)
      : base(typeof (T), (object) value, formatter, mediaType)
    {
    }
  }
}
