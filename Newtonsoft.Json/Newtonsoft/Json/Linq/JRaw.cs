// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JRaw
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Newtonsoft.Json.Linq
{
  /// <summary>Represents a raw JSON string.</summary>
  public class JRaw : JValue
  {
    /// <summary>
    /// Asynchronously creates an instance of <see cref="T:Newtonsoft.Json.Linq.JRaw" /> with the content of the reader's current token.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the asynchronous creation. The <see cref="P:System.Threading.Tasks.Task`1.Result" />
    /// property returns an instance of <see cref="T:Newtonsoft.Json.Linq.JRaw" /> with the content of the reader's current token.</returns>
    public static async Task<JRaw> CreateAsync(
      JsonReader reader,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      JRaw jraw;
      using (StringWriter sw = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (JsonTextWriter jsonWriter = new JsonTextWriter((TextWriter) sw))
        {
          await jsonWriter.WriteTokenSyncReadingAsync(reader, cancellationToken).ConfigureAwait(false);
          jraw = new JRaw((object) sw.ToString());
        }
      }
      return jraw;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JRaw" /> class from another <see cref="T:Newtonsoft.Json.Linq.JRaw" /> object.
    /// </summary>
    /// <param name="other">A <see cref="T:Newtonsoft.Json.Linq.JRaw" /> object to copy from.</param>
    public JRaw(JRaw other)
      : base((JValue) other)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JRaw" /> class.
    /// </summary>
    /// <param name="rawJson">The raw json.</param>
    public JRaw(object? rawJson)
      : base(rawJson, JTokenType.Raw)
    {
    }

    /// <summary>
    /// Creates an instance of <see cref="T:Newtonsoft.Json.Linq.JRaw" /> with the content of the reader's current token.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>An instance of <see cref="T:Newtonsoft.Json.Linq.JRaw" /> with the content of the reader's current token.</returns>
    public static JRaw Create(JsonReader reader)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
        {
          jsonTextWriter.WriteToken(reader);
          return new JRaw((object) stringWriter.ToString());
        }
      }
    }

    internal override JToken CloneToken() => (JToken) new JRaw(this);
  }
}
