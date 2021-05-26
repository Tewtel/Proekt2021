// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpClientExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary> Extension methods that aid in making formatted requests using <see cref="T:System.Net.Http.HttpClient" />. </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HttpClientExtensions
  {
    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
      this HttpClient client,
      string requestUri,
      T value)
    {
      return client.PostAsJsonAsync<T>(requestUri, value, CancellationToken.None);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      CancellationToken cancellationToken)
    {
      return client.PostAsync<T>(requestUri, value, (MediaTypeFormatter) new JsonMediaTypeFormatter(), cancellationToken);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value)
    {
      return client.PostAsJsonAsync<T>(requestUri, value, CancellationToken.None);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      CancellationToken cancellationToken)
    {
      return client.PostAsync<T>(requestUri, value, (MediaTypeFormatter) new JsonMediaTypeFormatter(), cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsXmlAsync<T>(
      this HttpClient client,
      string requestUri,
      T value)
    {
      return client.PostAsXmlAsync<T>(requestUri, value, CancellationToken.None);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsXmlAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      CancellationToken cancellationToken)
    {
      return client.PostAsync<T>(requestUri, value, (MediaTypeFormatter) new XmlMediaTypeFormatter(), cancellationToken);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as XML. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsXmlAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value)
    {
      return client.PostAsXmlAsync<T>(requestUri, value, CancellationToken.None);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as XML. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsXmlAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      CancellationToken cancellationToken)
    {
      return client.PostAsync<T>(requestUri, value, (MediaTypeFormatter) new XmlMediaTypeFormatter(), cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter)
    {
      return client.PostAsync<T>(requestUri, value, formatter, CancellationToken.None);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter,
      CancellationToken cancellationToken)
    {
      return client.PostAsync<T>(requestUri, value, formatter, (MediaTypeHeaderValue) null, cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter,
      string mediaType)
    {
      return client.PostAsync<T>(requestUri, value, formatter, mediaType, CancellationToken.None);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter,
      string mediaType,
      CancellationToken cancellationToken)
    {
      return client.PostAsync<T>(requestUri, value, formatter, ObjectContent.BuildHeaderValue(mediaType), cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter,
      MediaTypeHeaderValue mediaType,
      CancellationToken cancellationToken)
    {
      if (client == null)
        throw Error.ArgumentNull(nameof (client));
      ObjectContent<T> objectContent = new ObjectContent<T>(value, formatter, mediaType);
      return client.PostAsync(requestUri, (HttpContent) objectContent, cancellationToken);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter)
    {
      return client.PostAsync<T>(requestUri, value, formatter, CancellationToken.None);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter,
      CancellationToken cancellationToken)
    {
      return client.PostAsync<T>(requestUri, value, formatter, (MediaTypeHeaderValue) null, cancellationToken);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <param name="mediaType">The authoritative value of the request's content's Content-Type header. Can be null in which case the &lt;paramref name="formatter"&gt;formatter's&lt;/paramref&gt; default content type will be used.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter,
      string mediaType)
    {
      return client.PostAsync<T>(requestUri, value, formatter, mediaType, CancellationToken.None);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <param name="mediaType">The authoritative value of the request's content's Content-Type header. Can be null in which case the &lt;paramref name="formatter"&gt;formatter's&lt;/paramref&gt; default content type will be used.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter,
      string mediaType,
      CancellationToken cancellationToken)
    {
      return client.PostAsync<T>(requestUri, value, formatter, ObjectContent.BuildHeaderValue(mediaType), cancellationToken);
    }

    /// <summary> Sends a POST request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <param name="mediaType">The authoritative value of the request's content's Content-Type header. Can be null in which case the &lt;paramref name="formatter"&gt;formatter's&lt;/paramref&gt; default content type will be used.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PostAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter,
      MediaTypeHeaderValue mediaType,
      CancellationToken cancellationToken)
    {
      if (client == null)
        throw Error.ArgumentNull(nameof (client));
      ObjectContent<T> objectContent = new ObjectContent<T>(value, formatter, mediaType);
      return client.PostAsync(requestUri, (HttpContent) objectContent, cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
      this HttpClient client,
      string requestUri,
      T value)
    {
      return client.PutAsJsonAsync<T>(requestUri, value, CancellationToken.None);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      CancellationToken cancellationToken)
    {
      return client.PutAsync<T>(requestUri, value, (MediaTypeFormatter) new JsonMediaTypeFormatter(), cancellationToken);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with the given value serialized as JSON. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value)
    {
      return client.PutAsJsonAsync<T>(requestUri, value, CancellationToken.None);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with the given value serialized as JSON. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      CancellationToken cancellationToken)
    {
      return client.PutAsync<T>(requestUri, value, (MediaTypeFormatter) new JsonMediaTypeFormatter(), cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsXmlAsync<T>(
      this HttpClient client,
      string requestUri,
      T value)
    {
      return client.PutAsXmlAsync<T>(requestUri, value, CancellationToken.None);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsXmlAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      CancellationToken cancellationToken)
    {
      return client.PutAsync<T>(requestUri, value, (MediaTypeFormatter) new XmlMediaTypeFormatter(), cancellationToken);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with the given value serialized as XML. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsXmlAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value)
    {
      return client.PutAsXmlAsync<T>(requestUri, value, CancellationToken.None);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with the given value serialized as XML. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsXmlAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      CancellationToken cancellationToken)
    {
      return client.PutAsync<T>(requestUri, value, (MediaTypeFormatter) new XmlMediaTypeFormatter(), cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter)
    {
      return client.PutAsync<T>(requestUri, value, formatter, CancellationToken.None);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter,
      CancellationToken cancellationToken)
    {
      return client.PutAsync<T>(requestUri, value, formatter, (MediaTypeHeaderValue) null, cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter,
      string mediaType)
    {
      return client.PutAsync<T>(requestUri, value, formatter, mediaType, CancellationToken.None);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter,
      string mediaType,
      CancellationToken cancellationToken)
    {
      return client.PutAsync<T>(requestUri, value, formatter, ObjectContent.BuildHeaderValue(mediaType), cancellationToken);
    }

    /// <typeparam name="T"></typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      string requestUri,
      T value,
      MediaTypeFormatter formatter,
      MediaTypeHeaderValue mediaType,
      CancellationToken cancellationToken)
    {
      if (client == null)
        throw Error.ArgumentNull(nameof (client));
      ObjectContent<T> objectContent = new ObjectContent<T>(value, formatter, mediaType);
      return client.PutAsync(requestUri, (HttpContent) objectContent, cancellationToken);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter)
    {
      return client.PutAsync<T>(requestUri, value, formatter, CancellationToken.None);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter,
      CancellationToken cancellationToken)
    {
      return client.PutAsync<T>(requestUri, value, formatter, (MediaTypeHeaderValue) null, cancellationToken);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <param name="mediaType">The authoritative value of the request's content's Content-Type header. Can be null in which case the &lt;paramref name="formatter"&gt;formatter's&lt;/paramref&gt; default content type will be used.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter,
      string mediaType)
    {
      return client.PutAsync<T>(requestUri, value, formatter, mediaType, CancellationToken.None);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <param name="mediaType">The authoritative value of the request's content's Content-Type header. Can be null in which case the &lt;paramref name="formatter"&gt;formatter's&lt;/paramref&gt; default content type will be used.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter,
      string mediaType,
      CancellationToken cancellationToken)
    {
      return client.PutAsync<T>(requestUri, value, formatter, ObjectContent.BuildHeaderValue(mediaType), cancellationToken);
    }

    /// <summary> Sends a PUT request as an asynchronous operation to the specified Uri with value serialized using the given formatter. </summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="client">The client used to make the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value that will be placed in the request's entity body.</param>
    /// <param name="formatter">The formatter used to serialize the value.</param>
    /// <param name="mediaType">The authoritative value of the request's content's Content-Type header. Can be null in which case the &lt;paramref name="formatter"&gt;formatter's&lt;/paramref&gt; default content type will be used.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The type of value.</typeparam>
    public static Task<HttpResponseMessage> PutAsync<T>(
      this HttpClient client,
      Uri requestUri,
      T value,
      MediaTypeFormatter formatter,
      MediaTypeHeaderValue mediaType,
      CancellationToken cancellationToken)
    {
      if (client == null)
        throw Error.ArgumentNull(nameof (client));
      ObjectContent<T> objectContent = new ObjectContent<T>(value, formatter, mediaType);
      return client.PutAsync(requestUri, (HttpContent) objectContent, cancellationToken);
    }
  }
}
