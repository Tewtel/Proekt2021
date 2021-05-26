// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpContentExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Specifies extension methods to allow strongly typed objects to be read from HttpContent instances.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HttpContentExtensions
  {
    private static MediaTypeFormatterCollection _defaultMediaTypeFormatterCollection;

    private static MediaTypeFormatterCollection DefaultMediaTypeFormatterCollection
    {
      get
      {
        if (HttpContentExtensions._defaultMediaTypeFormatterCollection == null)
          HttpContentExtensions._defaultMediaTypeFormatterCollection = new MediaTypeFormatterCollection();
        return HttpContentExtensions._defaultMediaTypeFormatterCollection;
      }
    }

    /// <summary> Returns a Task that will yield an object of the specified type from the content instance. </summary>
    /// <returns>A Task that will yield an object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="type">The type of the object to read.</param>
    public static Task<object> ReadAsAsync(this HttpContent content, Type type) => content.ReadAsAsync(type, (IEnumerable<MediaTypeFormatter>) HttpContentExtensions.DefaultMediaTypeFormatterCollection);

    /// <summary>Returns a Task that will yield an object of the specified type from the content instance using one of the provided formatters to deserialize the content.</summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public static Task<object> ReadAsAsync(
      this HttpContent content,
      Type type,
      CancellationToken cancellationToken)
    {
      return content.ReadAsAsync(type, (IEnumerable<MediaTypeFormatter>) HttpContentExtensions.DefaultMediaTypeFormatterCollection, cancellationToken);
    }

    /// <summary> Returns a Task that will yield an object of the specified type from the content instance using one of the provided formatters to deserialize the content. </summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="formatters">The collection of MediaTypeFormatter instances to use.</param>
    public static Task<object> ReadAsAsync(
      this HttpContent content,
      Type type,
      IEnumerable<MediaTypeFormatter> formatters)
    {
      return HttpContentExtensions.ReadAsAsync<object>(content, type, formatters, (IFormatterLogger) null);
    }

    /// <summary>Returns a Task that will yield an object of the specified type from the content instance using one of the provided formatters to deserialize the content.</summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="formatters">The collection of MediaTypeFormatter instances to use.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public static Task<object> ReadAsAsync(
      this HttpContent content,
      Type type,
      IEnumerable<MediaTypeFormatter> formatters,
      CancellationToken cancellationToken)
    {
      return HttpContentExtensions.ReadAsAsync<object>(content, type, formatters, (IFormatterLogger) null, cancellationToken);
    }

    /// <summary> Returns a Task that will yield an object of the specified type from the content instance using one of the provided formatters to deserialize the content. </summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="formatters">The collection of MediaTypeFormatter instances to use.</param>
    /// <param name="formatterLogger">The IFormatterLogger to log events to.</param>
    public static Task<object> ReadAsAsync(
      this HttpContent content,
      Type type,
      IEnumerable<MediaTypeFormatter> formatters,
      IFormatterLogger formatterLogger)
    {
      return HttpContentExtensions.ReadAsAsync<object>(content, type, formatters, formatterLogger);
    }

    /// <summary>Returns a Task that will yield an object of the specified type from the content instance using one of the provided formatters to deserialize the content.</summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="formatters">The collection of MediaTypeFormatter instances to use.</param>
    /// <param name="formatterLogger">The IFormatterLogger to log events to.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public static Task<object> ReadAsAsync(
      this HttpContent content,
      Type type,
      IEnumerable<MediaTypeFormatter> formatters,
      IFormatterLogger formatterLogger,
      CancellationToken cancellationToken)
    {
      return HttpContentExtensions.ReadAsAsync<object>(content, type, formatters, formatterLogger, cancellationToken);
    }

    /// <summary> Returns a Task that will yield an object of the specified type &lt;typeparamref name="T" /&gt; from the content instance. </summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <typeparam name="T">The type of the object to read.</typeparam>
    public static Task<T> ReadAsAsync<T>(this HttpContent content) => content.ReadAsAsync<T>((IEnumerable<MediaTypeFormatter>) HttpContentExtensions.DefaultMediaTypeFormatterCollection);

    /// <summary>Returns a Task that will yield an object of the specified type from the content instance.</summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    /// <typeparam name="T">The type of the object to read.</typeparam>
    public static Task<T> ReadAsAsync<T>(
      this HttpContent content,
      CancellationToken cancellationToken)
    {
      return content.ReadAsAsync<T>((IEnumerable<MediaTypeFormatter>) HttpContentExtensions.DefaultMediaTypeFormatterCollection, cancellationToken);
    }

    /// <summary> Returns a Task that will yield an object of the specified type &lt;typeparamref name="T" /&gt; from the content instance. </summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="formatters">The collection of MediaTyepFormatter instances to use.</param>
    /// <typeparam name="T">The type of the object to read.</typeparam>
    public static Task<T> ReadAsAsync<T>(
      this HttpContent content,
      IEnumerable<MediaTypeFormatter> formatters)
    {
      return HttpContentExtensions.ReadAsAsync<T>(content, typeof (T), formatters, (IFormatterLogger) null);
    }

    /// <summary>Returns a Task that will yield an object of the specified type from the content instance.</summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="formatters">The collection of MediaTypeFormatter instances to use.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    /// <typeparam name="T">The type of the object to read.</typeparam>
    public static Task<T> ReadAsAsync<T>(
      this HttpContent content,
      IEnumerable<MediaTypeFormatter> formatters,
      CancellationToken cancellationToken)
    {
      return HttpContentExtensions.ReadAsAsync<T>(content, typeof (T), formatters, (IFormatterLogger) null, cancellationToken);
    }

    /// <summary> Returns a Task that will yield an object of the specified type &lt;typeparamref name="T" /&gt; from the content instance. </summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="formatters">The collection of MediaTypeFormatter instances to use.</param>
    /// <param name="formatterLogger">The IFormatterLogger to log events to.</param>
    /// <typeparam name="T">The type of the object to read.</typeparam>
    public static Task<T> ReadAsAsync<T>(
      this HttpContent content,
      IEnumerable<MediaTypeFormatter> formatters,
      IFormatterLogger formatterLogger)
    {
      return HttpContentExtensions.ReadAsAsync<T>(content, typeof (T), formatters, formatterLogger);
    }

    /// <summary>Returns a Task that will yield an object of the specified type from the content instance.</summary>
    /// <returns>An object instance of the specified type.</returns>
    /// <param name="content">The HttpContent instance from which to read.</param>
    /// <param name="formatters">The collection of MediaTypeFormatter instances to use.</param>
    /// <param name="formatterLogger">The IFormatterLogger to log events to.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    /// <typeparam name="T">The type of the object to read.</typeparam>
    public static Task<T> ReadAsAsync<T>(
      this HttpContent content,
      IEnumerable<MediaTypeFormatter> formatters,
      IFormatterLogger formatterLogger,
      CancellationToken cancellationToken)
    {
      return HttpContentExtensions.ReadAsAsync<T>(content, typeof (T), formatters, formatterLogger, cancellationToken);
    }

    private static Task<T> ReadAsAsync<T>(
      HttpContent content,
      Type type,
      IEnumerable<MediaTypeFormatter> formatters,
      IFormatterLogger formatterLogger)
    {
      return HttpContentExtensions.ReadAsAsync<T>(content, type, formatters, formatterLogger, CancellationToken.None);
    }

    private static Task<T> ReadAsAsync<T>(
      HttpContent content,
      Type type,
      IEnumerable<MediaTypeFormatter> formatters,
      IFormatterLogger formatterLogger,
      CancellationToken cancellationToken)
    {
      if (content == null)
        throw Error.ArgumentNull(nameof (content));
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (formatters == null)
        throw Error.ArgumentNull(nameof (formatters));
      if (content is ObjectContent objectContent && objectContent.Value != null && type.IsAssignableFrom(objectContent.Value.GetType()))
        return Task.FromResult<T>((T) objectContent.Value);
      MediaTypeHeaderValue mediaType = content.Headers.ContentType ?? MediaTypeConstants.ApplicationOctetStreamMediaType;
      MediaTypeFormatter reader = new MediaTypeFormatterCollection(formatters).FindReader(type, mediaType);
      if (reader != null)
        return HttpContentExtensions.ReadAsAsyncCore<T>(content, type, formatterLogger, reader, cancellationToken);
      long? contentLength = content.Headers.ContentLength;
      long num = 0;
      if (contentLength.GetValueOrDefault() == num & contentLength.HasValue)
        return Task.FromResult<T>((T) MediaTypeFormatter.GetDefaultValueForType(type));
      throw new UnsupportedMediaTypeException(Error.Format(Resources.NoReadSerializerAvailable, (object) type.Name, (object) mediaType.MediaType), mediaType);
    }

    private static async Task<T> ReadAsAsyncCore<T>(
      HttpContent content,
      Type type,
      IFormatterLogger formatterLogger,
      MediaTypeFormatter formatter,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return (T) await formatter.ReadFromStreamAsync(type, await content.ReadAsStreamAsync(), content, formatterLogger, cancellationToken);
    }
  }
}
