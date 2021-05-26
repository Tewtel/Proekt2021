// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpClientFactory
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Properties;

namespace System.Net.Http
{
  /// <summary>Represents the factory for creating new instance of <see cref="T:System.Net.Http.HttpClient" />.</summary>
  public static class HttpClientFactory
  {
    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.HttpClient" />.</summary>
    /// <returns>A new instance of the <see cref="T:System.Net.Http.HttpClient" />.</returns>
    /// <param name="handlers">The list of HTTP handler that delegates the processing of HTTP response messages to another handler.</param>
    public static HttpClient Create(params DelegatingHandler[] handlers) => HttpClientFactory.Create((HttpMessageHandler) new HttpClientHandler(), handlers);

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.HttpClient" />.</summary>
    /// <returns>A new instance of the <see cref="T:System.Net.Http.HttpClient" />.</returns>
    /// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
    /// <param name="handlers">The list of HTTP handler that delegates the processing of HTTP response messages to another handler.</param>
    public static HttpClient Create(
      HttpMessageHandler innerHandler,
      params DelegatingHandler[] handlers)
    {
      return new HttpClient(HttpClientFactory.CreatePipeline(innerHandler, (IEnumerable<DelegatingHandler>) handlers));
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.HttpClient" /> which should be pipelined.</summary>
    /// <returns>A new instance of the <see cref="T:System.Net.Http.HttpClient" /> which should be pipelined.</returns>
    /// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
    /// <param name="handlers">The list of HTTP handler that delegates the processing of HTTP response messages to another handler.</param>
    public static HttpMessageHandler CreatePipeline(
      HttpMessageHandler innerHandler,
      IEnumerable<DelegatingHandler> handlers)
    {
      if (innerHandler == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (innerHandler));
      if (handlers == null)
        return innerHandler;
      HttpMessageHandler httpMessageHandler = innerHandler;
      foreach (DelegatingHandler delegatingHandler in handlers.Reverse<DelegatingHandler>())
      {
        if (delegatingHandler == null)
          throw System.Web.Http.Error.Argument(nameof (handlers), Resources.DelegatingHandlerArrayContainsNullItem, (object) typeof (DelegatingHandler).Name);
        delegatingHandler.InnerHandler = delegatingHandler.InnerHandler == null ? httpMessageHandler : throw System.Web.Http.Error.Argument(nameof (handlers), Resources.DelegatingHandlerArrayHasNonNullInnerHandler, (object) typeof (DelegatingHandler).Name, (object) "InnerHandler", (object) delegatingHandler.GetType().Name);
        httpMessageHandler = (HttpMessageHandler) delegatingHandler;
      }
      return httpMessageHandler;
    }
  }
}
