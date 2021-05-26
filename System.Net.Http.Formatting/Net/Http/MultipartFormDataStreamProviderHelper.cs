// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartFormDataStreamProviderHelper
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  internal static class MultipartFormDataStreamProviderHelper
  {
    public static bool IsFileContent(HttpContent parent, HttpContentHeaders headers)
    {
      if (parent == null)
        throw Error.ArgumentNull(nameof (parent));
      ContentDispositionHeaderValue dispositionHeaderValue = headers != null ? headers.ContentDisposition : throw Error.ArgumentNull(nameof (headers));
      if (dispositionHeaderValue == null)
        throw Error.InvalidOperation(Resources.MultipartFormDataStreamProviderNoContentDisposition, (object) "Content-Disposition");
      return !string.IsNullOrEmpty(dispositionHeaderValue.FileName);
    }

    public static async Task ReadFormDataAsync(
      Collection<HttpContent> contents,
      NameValueCollection formData,
      CancellationToken cancellationToken)
    {
      foreach (HttpContent content in contents)
      {
        ContentDispositionHeaderValue contentDisposition = content.Headers.ContentDisposition;
        if (string.IsNullOrEmpty(contentDisposition.FileName))
        {
          string formFieldName = FormattingUtilities.UnquoteToken(contentDisposition.Name) ?? string.Empty;
          cancellationToken.ThrowIfCancellationRequested();
          formData.Add(formFieldName, await content.ReadAsStringAsync());
          formFieldName = (string) null;
        }
      }
    }
  }
}
