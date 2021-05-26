// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpContentFormDataExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Extension methods to read HTML form URL-encoded datafrom <see cref="T:System.Net.Http.HttpContent" /> instances.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HttpContentFormDataExtensions
  {
    private const string ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";

    /// <summary>Determines whether the specified content is HTML form URL-encoded data.</summary>
    /// <returns>true if the specified content is HTML form URL-encoded data; otherwise, false.</returns>
    /// <param name="content">The content.</param>
    public static bool IsFormData(this HttpContent content)
    {
      if (content == null)
        throw Error.ArgumentNull(nameof (content));
      MediaTypeHeaderValue contentType = content.Headers.ContentType;
      return contentType != null && string.Equals("application/x-www-form-urlencoded", contentType.MediaType, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Asynchronously reads HTML form URL-encoded from an <see cref="T:System.Net.Http.HttpContent" /> instance and stores the results in a <see cref="T:System.Collections.Specialized.NameValueCollection" /> object.</summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="content">The content.</param>
    public static Task<NameValueCollection> ReadAsFormDataAsync(
      this HttpContent content)
    {
      return content.ReadAsFormDataAsync(CancellationToken.None);
    }

    /// <summary>Asynchronously reads HTML form URL-encoded from an <see cref="T:System.Net.Http.HttpContent" /> instance and stores the results in a <see cref="T:System.Collections.Specialized.NameValueCollection" /> object.</summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="content">The content.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public static Task<NameValueCollection> ReadAsFormDataAsync(
      this HttpContent content,
      CancellationToken cancellationToken)
    {
      if (content == null)
        throw Error.ArgumentNull(nameof (content));
      MediaTypeFormatter[] formatters = new MediaTypeFormatter[1]
      {
        (MediaTypeFormatter) new FormUrlEncodedMediaTypeFormatter()
      };
      return HttpContentFormDataExtensions.ReadAsAsyncCore(content, formatters, cancellationToken);
    }

    private static async Task<NameValueCollection> ReadAsAsyncCore(
      HttpContent content,
      MediaTypeFormatter[] formatters,
      CancellationToken cancellationToken)
    {
      FormDataCollection formDataCollection = await content.ReadAsAsync<FormDataCollection>((IEnumerable<MediaTypeFormatter>) formatters, cancellationToken);
      return formDataCollection == null ? (NameValueCollection) null : formDataCollection.ReadAsNameValueCollection();
    }
  }
}
