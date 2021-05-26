// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Handlers.ProgressMessageHandler
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Handlers
{
  /// <summary>Generates progress notification for both request entities being uploaded and response entities being downloaded.</summary>
  public class ProgressMessageHandler : DelegatingHandler
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Handlers.ProgressMessageHandler" /> class.</summary>
    public ProgressMessageHandler()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Handlers.ProgressMessageHandler" /> class.</summary>
    /// <param name="innerHandler">The inner message handler.</param>
    public ProgressMessageHandler(HttpMessageHandler innerHandler)
      : base(innerHandler)
    {
    }

    /// <summary>Occurs when event entities are being uploaded.</summary>
    public event EventHandler<HttpProgressEventArgs> HttpSendProgress;

    /// <summary>Occurs when event entities are being downloaded.</summary>
    public event EventHandler<HttpProgressEventArgs> HttpReceiveProgress;

    /// <summary>Sends the specified progress message to an HTTP server for delivery.</summary>
    /// <returns>The sent progress message.</returns>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    protected override async Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      this.AddRequestProgress(request);
      HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
      if (this.HttpReceiveProgress != null && response != null && response.Content != null)
      {
        cancellationToken.ThrowIfCancellationRequested();
        HttpResponseMessage httpResponseMessage = await this.AddResponseProgressAsync(request, response);
      }
      return response;
    }

    /// <summary>Raises the event that handles the request of the progress.</summary>
    /// <param name="request">The request.</param>
    /// <param name="e">The event handler for the request.</param>
    protected internal virtual void OnHttpRequestProgress(
      HttpRequestMessage request,
      HttpProgressEventArgs e)
    {
      if (this.HttpSendProgress == null)
        return;
      this.HttpSendProgress((object) request, e);
    }

    /// <summary>Raises the event that handles the response of the progress.</summary>
    /// <param name="request">The request.</param>
    /// <param name="e">The event handler for the request.</param>
    protected internal virtual void OnHttpResponseProgress(
      HttpRequestMessage request,
      HttpProgressEventArgs e)
    {
      if (this.HttpReceiveProgress == null)
        return;
      this.HttpReceiveProgress((object) request, e);
    }

    private void AddRequestProgress(HttpRequestMessage request)
    {
      if (this.HttpSendProgress == null || request == null || request.Content == null)
        return;
      HttpContent httpContent = (HttpContent) new ProgressContent(request.Content, this, request);
      request.Content = httpContent;
    }

    private async Task<HttpResponseMessage> AddResponseProgressAsync(
      HttpRequestMessage request,
      HttpResponseMessage response)
    {
      ProgressMessageHandler handler = this;
      HttpContent httpContent = (HttpContent) new StreamContent((Stream) new ProgressStream(await response.Content.ReadAsStreamAsync(), handler, request, response));
      response.Content.Headers.CopyTo(httpContent.Headers);
      response.Content = httpContent;
      return response;
    }
  }
}
