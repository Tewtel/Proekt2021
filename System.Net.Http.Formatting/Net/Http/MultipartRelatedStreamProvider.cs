// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartRelatedStreamProvider
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace System.Net.Http
{
  /// <summary>Represents the provider for the multipart related multistream.</summary>
  public class MultipartRelatedStreamProvider : MultipartStreamProvider
  {
    private const string RelatedSubType = "related";
    private const string ContentID = "Content-ID";
    private const string StartParameter = "Start";
    private HttpContent _rootContent;
    private HttpContent _parent;

    /// <summary>Gets the root content of the <see cref="T:System.Net.Http.MultipartRelatedStreamProvider" />.</summary>
    /// <returns>The root content of the <see cref="T:System.Net.Http.MultipartRelatedStreamProvider" />.</returns>
    public HttpContent RootContent
    {
      get
      {
        if (this._rootContent == null)
          this._rootContent = MultipartRelatedStreamProvider.FindRootContent(this._parent, (IEnumerable<HttpContent>) this.Contents);
        return this._rootContent;
      }
    }

    /// <summary>Gets the related stream for the provider.</summary>
    /// <returns>The content headers.</returns>
    /// <param name="parent">The parent content.</param>
    /// <param name="headers">The http content headers.</param>
    public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
    {
      if (parent == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (parent));
      if (headers == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (headers));
      if (this._parent == null)
        this._parent = parent;
      return (Stream) new MemoryStream();
    }

    private static HttpContent FindRootContent(
      HttpContent parent,
      IEnumerable<HttpContent> children)
    {
      NameValueHeaderValue relatedParameter = MultipartRelatedStreamProvider.FindMultipartRelatedParameter(parent, "Start");
      if (relatedParameter == null)
        return children.FirstOrDefault<HttpContent>();
      string startValue = FormattingUtilities.UnquoteToken(relatedParameter.Value);
      IEnumerable<string> values;
      return children.FirstOrDefault<HttpContent>((Func<HttpContent, bool>) (content => content.Headers.TryGetValues("Content-ID", out values) && string.Equals(FormattingUtilities.UnquoteToken(values.ElementAt<string>(0)), startValue, StringComparison.OrdinalIgnoreCase)));
    }

    private static NameValueHeaderValue FindMultipartRelatedParameter(
      HttpContent content,
      string parameterName)
    {
      if (content == null)
        return (NameValueHeaderValue) null;
      MediaTypeHeaderValue contentType = content.Headers.ContentType;
      return contentType == null || !content.IsMimeMultipartContent("related") ? (NameValueHeaderValue) null : contentType.Parameters.FirstOrDefault<NameValueHeaderValue>((Func<NameValueHeaderValue, bool>) (nvp => string.Equals(nvp.Name, parameterName, StringComparison.OrdinalIgnoreCase)));
    }
  }
}
