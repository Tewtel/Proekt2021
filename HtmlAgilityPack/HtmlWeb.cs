// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlWeb
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace HtmlAgilityPack
{
  /// <summary>A utility class to get HTML document from HTTP.</summary>
  public class HtmlWeb
  {
    private bool _autoDetectEncoding = true;
    private bool _cacheOnly;
    private string _cachePath;
    private bool _fromCache;
    private int _requestDuration;
    private Uri _responseUri;
    private HttpStatusCode _statusCode = HttpStatusCode.OK;
    private int _streamBufferSize = 1024;
    private bool _useCookies;
    private bool _usingCache;
    private bool _usingCacheAndLoad;
    private bool _usingCacheIfExists;
    private string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:x.x.x) Gecko/20041107 Firefox/x.x";
    /// <summary>Occurs after an HTTP request has been executed.</summary>
    public HtmlWeb.PostResponseHandler PostResponse;
    /// <summary>Occurs before an HTML document is handled.</summary>
    public HtmlWeb.PreHandleDocumentHandler PreHandleDocument;
    /// <summary>Occurs before an HTTP request is executed.</summary>
    public HtmlWeb.PreRequestHandler PreRequest;
    private Encoding _encoding;
    private TimeSpan _browserTimeout = TimeSpan.FromSeconds(30.0);
    private TimeSpan _browserDelay = TimeSpan.FromMilliseconds(100.0);

    /// <summary>
    /// Gets or Sets a value indicating if document encoding must be automatically detected.
    /// </summary>
    public bool AutoDetectEncoding
    {
      get => this._autoDetectEncoding;
      set => this._autoDetectEncoding = value;
    }

    /// <summary>
    /// Gets or sets the Encoding used to override the response stream from any web request
    /// </summary>
    public Encoding OverrideEncoding
    {
      get => this._encoding;
      set => this._encoding = value;
    }

    /// <summary>
    /// Gets or Sets a value indicating whether to get document only from the cache.
    /// If this is set to true and document is not found in the cache, nothing will be loaded.
    /// </summary>
    public bool CacheOnly
    {
      get => this._cacheOnly;
      set => this._cacheOnly = !value || this.UsingCache ? value : throw new HtmlWebException("Cache is not enabled. Set UsingCache to true first.");
    }

    /// <summary>
    /// Gets or Sets a value indicating whether to get document from the cache if exists, otherwise from the web
    /// A value indicating whether to get document from the cache if exists, otherwise from the web
    /// </summary>
    public bool UsingCacheIfExists
    {
      get => this._usingCacheIfExists;
      set => this._usingCacheIfExists = value;
    }

    /// <summary>
    /// Gets or Sets the cache path. If null, no caching mechanism will be used.
    /// </summary>
    public string CachePath
    {
      get => this._cachePath;
      set => this._cachePath = value;
    }

    /// <summary>
    /// Gets a value indicating if the last document was retrieved from the cache.
    /// </summary>
    public bool FromCache => this._fromCache;

    /// <summary>Gets the last request duration in milliseconds.</summary>
    public int RequestDuration => this._requestDuration;

    /// <summary>
    /// Gets the URI of the Internet resource that actually responded to the request.
    /// </summary>
    public Uri ResponseUri => this._responseUri;

    /// <summary>Gets the last request status.</summary>
    public HttpStatusCode StatusCode => this._statusCode;

    /// <summary>
    /// Gets or Sets the size of the buffer used for memory operations.
    /// </summary>
    public int StreamBufferSize
    {
      get => this._streamBufferSize;
      set => this._streamBufferSize = this._streamBufferSize > 0 ? value : throw new ArgumentException("Size must be greater than zero.");
    }

    /// <summary>
    /// Gets or Sets a value indicating if cookies will be stored.
    /// </summary>
    public bool UseCookies
    {
      get => this._useCookies;
      set => this._useCookies = value;
    }

    /// <summary>Gets or sets a value indicating whether redirect should be captured instead of the current location.</summary>
    /// <value>True if capture redirect, false if not.</value>
    public bool CaptureRedirect { get; set; }

    /// <summary>
    /// Gets or Sets the User Agent HTTP 1.1 header sent on any webrequest
    /// </summary>
    public string UserAgent
    {
      get => this._userAgent;
      set => this._userAgent = value;
    }

    /// <summary>
    /// Gets or Sets a value indicating whether the caching mechanisms should be used or not.
    /// </summary>
    public bool UsingCache
    {
      get => this._cachePath != null && this._usingCache;
      set => this._usingCache = !value || this._cachePath != null ? value : throw new HtmlWebException("You need to define a CachePath first.");
    }

    /// <summary>
    /// Gets the MIME content type for a given path extension.
    /// </summary>
    /// <param name="extension">The input path extension.</param>
    /// <param name="def">The default content type to return if any error occurs.</param>
    /// <returns>The path extension's MIME content type.</returns>
    public static string GetContentTypeForExtension(string extension, string def)
    {
      if (string.IsNullOrEmpty(extension))
        return def;
      string str = "";
      if (!extension.StartsWith("."))
        extension = "." + extension;
      if (!MimeTypeMap.Mappings.TryGetValue(extension, out str))
        str = def;
      return str;
    }

    /// <summary>
    /// Gets the path extension for a given MIME content type.
    /// </summary>
    /// <param name="contentType">The input MIME content type.</param>
    /// <param name="def">The default path extension to return if any error occurs.</param>
    /// <returns>The MIME content type's path extension.</returns>
    public static string GetExtensionForContentType(string contentType, string def)
    {
      if (string.IsNullOrEmpty(contentType))
        return def;
      if (contentType.StartsWith("."))
        throw new ArgumentException("Requested mime type is not valid: " + contentType);
      string str = "";
      if (!MimeTypeMap.Mappings.TryGetValue(contentType, out str))
        str = def;
      return str;
    }

    /// <summary>
    /// Creates an instance of the given type from the specified Internet resource.
    /// </summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="type">The requested type.</param>
    /// <returns>An newly created instance.</returns>
    public object CreateInstance(string url, Type type) => this.CreateInstance(url, (string) null, (XsltArgumentList) null, type);

    /// <summary>
    /// Gets an HTML document from an Internet resource and saves it to the specified file.
    /// </summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="path">The location of the file where you want to save the document.</param>
    public void Get(string url, string path) => this.Get(url, path, "GET");

    /// <summary>
    /// Gets an HTML document from an Internet resource and saves it to the specified file. - Proxy aware
    /// </summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="path">The location of the file where you want to save the document.</param>
    /// <param name="proxy"></param>
    /// <param name="credentials"></param>
    public void Get(string url, string path, WebProxy proxy, NetworkCredential credentials) => this.Get(url, path, proxy, credentials, "GET");

    /// <summary>
    /// Gets an HTML document from an Internet resource and saves it to the specified file.
    /// </summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="path">The location of the file where you want to save the document.</param>
    /// <param name="method">The HTTP method used to open the connection, such as GET, POST, PUT, or PROPFIND.</param>
    public void Get(string url, string path, string method)
    {
      Uri uri = new Uri(url);
      if (!(uri.Scheme == Uri.UriSchemeHttps) && !(uri.Scheme == Uri.UriSchemeHttp))
        throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
      int num = (int) this.Get(uri, method, path, (HtmlDocument) null, (IWebProxy) null, (ICredentials) null);
    }

    /// <summary>
    /// Gets an HTML document from an Internet resource and saves it to the specified file.  Understands Proxies
    /// </summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="path">The location of the file where you want to save the document.</param>
    /// <param name="credentials"></param>
    /// <param name="method">The HTTP method used to open the connection, such as GET, POST, PUT, or PROPFIND.</param>
    /// <param name="proxy"></param>
    public void Get(
      string url,
      string path,
      WebProxy proxy,
      NetworkCredential credentials,
      string method)
    {
      Uri uri = new Uri(url);
      if (!(uri.Scheme == Uri.UriSchemeHttps) && !(uri.Scheme == Uri.UriSchemeHttp))
        throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
      int num = (int) this.Get(uri, method, path, (HtmlDocument) null, (IWebProxy) proxy, (ICredentials) credentials);
    }

    /// <summary>Gets the cache file path for a specified url.</summary>
    /// <param name="uri">The url fo which to retrieve the cache path. May not be null.</param>
    /// <returns>The cache file path.</returns>
    public string GetCachePath(Uri uri)
    {
      if (uri == (Uri) null)
        throw new ArgumentNullException(nameof (uri));
      if (!this.UsingCache)
        throw new HtmlWebException("Cache is not enabled. Set UsingCache to true first.");
      string str1;
      if (uri.AbsolutePath == "/")
      {
        str1 = Path.Combine(this._cachePath, ".htm");
      }
      else
      {
        string str2 = uri.AbsolutePath;
        foreach (char ch in new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()))
          str2 = str2.Replace(ch.ToString(), "");
        if ((int) uri.AbsolutePath[uri.AbsolutePath.Length - 1] == (int) Path.AltDirectorySeparatorChar)
          str1 = Path.Combine(this._cachePath, (uri.Host + str2.TrimEnd(Path.AltDirectorySeparatorChar)).Replace('/', '\\') + ".htm");
        else
          str1 = Path.Combine(this._cachePath, uri.Host + str2.Replace('/', '\\'));
      }
      return str1;
    }

    /// <summary>Gets an HTML document from an Internet resource.</summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument Load(string url) => this.Load(url, "GET");

    /// <summary>Gets an HTML document from an Internet resource.</summary>
    /// <param name="uri">The requested Uri, such as new Uri("http://Myserver/Mypath/Myfile.asp").</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument Load(Uri uri) => this.Load(uri, "GET");

    /// <summary>Gets an HTML document from an Internet resource.</summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="proxyHost">Host to use for Proxy</param>
    /// <param name="proxyPort">Port the Proxy is on</param>
    /// <param name="userId">User Id for Authentication</param>
    /// <param name="password">Password for Authentication</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument Load(
      string url,
      string proxyHost,
      int proxyPort,
      string userId,
      string password)
    {
      WebProxy proxy = new WebProxy(proxyHost, proxyPort);
      proxy.BypassProxyOnLocal = true;
      NetworkCredential credentials = (NetworkCredential) null;
      if (userId != null && password != null)
      {
        credentials = new NetworkCredential(userId, password);
        CredentialCache credentialCache = new CredentialCache()
        {
          {
            proxy.Address,
            "Basic",
            credentials
          },
          {
            proxy.Address,
            "Digest",
            credentials
          }
        };
      }
      return this.Load(url, "GET", proxy, credentials);
    }

    /// <summary>Gets an HTML document from an Internet resource.</summary>
    /// <param name="uri">The requested Uri, such as new Uri("http://Myserver/Mypath/Myfile.asp").</param>
    /// <param name="proxyHost">Host to use for Proxy</param>
    /// <param name="proxyPort">Port the Proxy is on</param>
    /// <param name="userId">User Id for Authentication</param>
    /// <param name="password">Password for Authentication</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument Load(
      Uri uri,
      string proxyHost,
      int proxyPort,
      string userId,
      string password)
    {
      WebProxy proxy = new WebProxy(proxyHost, proxyPort);
      proxy.BypassProxyOnLocal = true;
      NetworkCredential credentials = (NetworkCredential) null;
      if (userId != null && password != null)
      {
        credentials = new NetworkCredential(userId, password);
        CredentialCache credentialCache = new CredentialCache()
        {
          {
            proxy.Address,
            "Basic",
            credentials
          },
          {
            proxy.Address,
            "Digest",
            credentials
          }
        };
      }
      return this.Load(uri, "GET", proxy, credentials);
    }

    /// <summary>Loads an HTML document from an Internet resource.</summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="method">The HTTP method used to open the connection, such as GET, POST, PUT, or PROPFIND.</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument Load(string url, string method) => this.Load(new Uri(url), method);

    /// <summary>Loads an HTML document from an Internet resource.</summary>
    /// <param name="uri">The requested URL, such as new Uri("http://Myserver/Mypath/Myfile.asp").</param>
    /// <param name="method">The HTTP method used to open the connection, such as GET, POST, PUT, or PROPFIND.</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument Load(Uri uri, string method)
    {
      if (this.UsingCache)
        this._usingCacheAndLoad = true;
      HtmlDocument document;
      if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
      {
        document = this.LoadUrl(uri, method, (WebProxy) null, (NetworkCredential) null);
      }
      else
      {
        if (!(uri.Scheme == Uri.UriSchemeFile))
          throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
        document = new HtmlDocument()
        {
          OptionAutoCloseOnEnd = false
        };
        document.OptionAutoCloseOnEnd = true;
        if (this.OverrideEncoding != null)
          document.Load(uri.OriginalString, this.OverrideEncoding);
        else
          document.DetectEncodingAndLoad(uri.OriginalString, this._autoDetectEncoding);
      }
      if (this.PreHandleDocument != null)
        this.PreHandleDocument(document);
      return document;
    }

    /// <summary>Loads an HTML document from an Internet resource.</summary>
    /// <param name="url">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="method">The HTTP method used to open the connection, such as GET, POST, PUT, or PROPFIND.</param>
    /// <param name="proxy">Proxy to use with this request</param>
    /// <param name="credentials">Credentials to use when authenticating</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument Load(
      string url,
      string method,
      WebProxy proxy,
      NetworkCredential credentials)
    {
      return this.Load(new Uri(url), method, proxy, credentials);
    }

    /// <summary>Loads an HTML document from an Internet resource.</summary>
    /// <param name="uri">The requested Uri, such as new Uri("http://Myserver/Mypath/Myfile.asp").</param>
    /// <param name="method">The HTTP method used to open the connection, such as GET, POST, PUT, or PROPFIND.</param>
    /// <param name="proxy">Proxy to use with this request</param>
    /// <param name="credentials">Credentials to use when authenticating</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument Load(
      Uri uri,
      string method,
      WebProxy proxy,
      NetworkCredential credentials)
    {
      if (this.UsingCache)
        this._usingCacheAndLoad = true;
      HtmlDocument document;
      if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
      {
        document = this.LoadUrl(uri, method, proxy, credentials);
      }
      else
      {
        if (!(uri.Scheme == Uri.UriSchemeFile))
          throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
        document = new HtmlDocument()
        {
          OptionAutoCloseOnEnd = false
        };
        document.OptionAutoCloseOnEnd = true;
        document.DetectEncodingAndLoad(uri.OriginalString, this._autoDetectEncoding);
      }
      if (this.PreHandleDocument != null)
        this.PreHandleDocument(document);
      return document;
    }

    /// <summary>
    /// Loads an HTML document from an Internet resource and saves it to the specified XmlTextWriter.
    /// </summary>
    /// <param name="htmlUrl">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="writer">The XmlTextWriter to which you want to save to.</param>
    public void LoadHtmlAsXml(string htmlUrl, XmlTextWriter writer) => this.Load(htmlUrl).Save((XmlWriter) writer);

    private static void FilePreparePath(string target)
    {
      if (System.IO.File.Exists(target))
      {
        FileAttributes attributes = System.IO.File.GetAttributes(target);
        System.IO.File.SetAttributes(target, attributes & ~FileAttributes.ReadOnly);
      }
      else
      {
        string directoryName = Path.GetDirectoryName(target);
        if (Directory.Exists(directoryName))
          return;
        Directory.CreateDirectory(directoryName);
      }
    }

    private static DateTime RemoveMilliseconds(DateTime t) => new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, 0);

    private static DateTime RemoveMilliseconds(DateTimeOffset? offset)
    {
      DateTimeOffset dateTimeOffset = offset ?? DateTimeOffset.Now;
      return new DateTime(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, dateTimeOffset.Minute, dateTimeOffset.Second, 0);
    }

    private static long SaveStream(
      Stream stream,
      string path,
      DateTime touchDate,
      int streamBufferSize)
    {
      HtmlWeb.FilePreparePath(path);
      long num = 0;
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        using (BinaryReader binaryReader = new BinaryReader(stream))
        {
          using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
          {
            byte[] buffer;
            do
            {
              buffer = binaryReader.ReadBytes(streamBufferSize);
              num += (long) buffer.Length;
              if (buffer.Length != 0)
                binaryWriter.Write(buffer);
            }
            while (buffer.Length != 0);
            binaryWriter.Flush();
          }
        }
      }
      System.IO.File.SetLastWriteTime(path, touchDate);
      return num;
    }

    private HttpStatusCode Get(
      Uri uri,
      string method,
      string path,
      HtmlDocument doc,
      IWebProxy proxy,
      ICredentials creds)
    {
      string str = (string) null;
      bool flag1 = false;
      HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
      request.Method = method;
      request.UserAgent = this.UserAgent;
      if (this.CaptureRedirect)
        request.AllowAutoRedirect = false;
      if (proxy != null)
      {
        if (creds != null)
        {
          proxy.Credentials = creds;
          request.Credentials = creds;
        }
        else
        {
          proxy.Credentials = CredentialCache.DefaultCredentials;
          request.Credentials = CredentialCache.DefaultCredentials;
        }
        request.Proxy = proxy;
      }
      this._fromCache = false;
      this._requestDuration = 0;
      int tickCount = Environment.TickCount;
      if (this.UsingCache)
      {
        str = this.GetCachePath(request.RequestUri);
        if (System.IO.File.Exists(str))
        {
          request.IfModifiedSince = System.IO.File.GetLastAccessTime(str);
          flag1 = true;
        }
      }
      if (this._cacheOnly || this._usingCacheIfExists)
      {
        if (System.IO.File.Exists(str))
        {
          if (path != null)
          {
            IOLibrary.CopyAlways(str, path);
            if (str != null)
              System.IO.File.SetLastWriteTime(path, System.IO.File.GetLastWriteTime(str));
          }
          this._fromCache = true;
          return HttpStatusCode.NotModified;
        }
        if (this._cacheOnly)
          throw new HtmlWebException("File was not found at cache path: '" + str + "'");
      }
      if (this._useCookies)
        request.CookieContainer = new CookieContainer();
      if (this.PreRequest != null && !this.PreRequest(request))
        return HttpStatusCode.ResetContent;
      HttpWebResponse response;
      try
      {
        response = request.GetResponse() as HttpWebResponse;
      }
      catch (WebException ex)
      {
        this._requestDuration = Environment.TickCount - tickCount;
        response = (HttpWebResponse) ex.Response;
        if (response == null)
        {
          if (flag1)
          {
            if (path != null)
            {
              IOLibrary.CopyAlways(str, path);
              System.IO.File.SetLastWriteTime(path, System.IO.File.GetLastWriteTime(str));
            }
            return HttpStatusCode.NotModified;
          }
          throw;
        }
      }
      catch (Exception ex)
      {
        this._requestDuration = Environment.TickCount - tickCount;
        throw;
      }
      if (this.PostResponse != null)
        this.PostResponse(request, response);
      this._requestDuration = Environment.TickCount - tickCount;
      this._responseUri = response.ResponseUri;
      HttpStatusCode statusCode = response.StatusCode;
      bool flag2 = this.IsHtmlContent(response.ContentType);
      bool flag3 = string.IsNullOrEmpty(response.ContentType);
      Encoding encoding = !string.IsNullOrEmpty(flag2 ? response.CharacterSet : response.ContentEncoding) ? Encoding.GetEncoding(flag2 ? response.CharacterSet : response.ContentEncoding) : (Encoding) null;
      if (this.OverrideEncoding != null)
        encoding = this.OverrideEncoding;
      if (this.CaptureRedirect && response.StatusCode == HttpStatusCode.Found)
      {
        string header = response.Headers["Location"];
        Uri result;
        if (!Uri.TryCreate(header, UriKind.Absolute, out result))
          result = new Uri(uri, header);
        return this.Get(result, "GET", path, doc, proxy, creds);
      }
      if (response.StatusCode == HttpStatusCode.NotModified)
      {
        if (!this.UsingCache)
          throw new HtmlWebException("Server has send a NotModifed code, without cache enabled.");
        this._fromCache = true;
        if (path != null)
        {
          IOLibrary.CopyAlways(str, path);
          System.IO.File.SetLastWriteTime(path, System.IO.File.GetLastWriteTime(str));
        }
        return response.StatusCode;
      }
      Stream responseStream = response.GetResponseStream();
      if (responseStream != null)
      {
        if (this.UsingCache)
        {
          HtmlWeb.SaveStream(responseStream, str, HtmlWeb.RemoveMilliseconds(response.LastModified), this._streamBufferSize);
          this.SaveCacheHeaders(request.RequestUri, response);
          if (path != null)
          {
            IOLibrary.CopyAlways(str, path);
            System.IO.File.SetLastWriteTime(path, System.IO.File.GetLastWriteTime(str));
          }
          if (this._usingCacheAndLoad)
            doc.Load(str);
        }
        else
        {
          if (doc != null & flag2)
          {
            if (encoding == null)
              doc.Load(responseStream, true);
            else
              doc.Load(responseStream, encoding);
          }
          if (doc != null & flag3)
          {
            try
            {
              if (encoding == null)
                doc.Load(responseStream, true);
              else
                doc.Load(responseStream, encoding);
            }
            catch
            {
            }
          }
        }
        response.Close();
      }
      return statusCode;
    }

    private string GetCacheHeader(Uri requestUri, string name, string def)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(this.GetCacheHeadersPath(requestUri));
      XmlNode xmlNode = xmlDocument.SelectSingleNode("//h[translate(@n, 'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')='" + name.ToUpperInvariant() + "']");
      return xmlNode == null ? def : xmlNode.Attributes[name].Value;
    }

    private string GetCacheHeadersPath(Uri uri) => this.GetCachePath(uri) + ".h.xml";

    private bool IsCacheHtmlContent(string path) => this.IsHtmlContent(HtmlWeb.GetContentTypeForExtension(Path.GetExtension(path), (string) null));

    private bool IsHtmlContent(string contentType) => contentType.ToLowerInvariant().StartsWith("text/html");

    private bool IsGZipEncoding(string contentEncoding) => contentEncoding.ToLowerInvariant().StartsWith("gzip");

    private HtmlDocument LoadUrl(
      Uri uri,
      string method,
      WebProxy proxy,
      NetworkCredential creds)
    {
      HtmlDocument doc = new HtmlDocument();
      doc.OptionAutoCloseOnEnd = false;
      doc.OptionFixNestedTags = true;
      this._statusCode = this.Get(uri, method, (string) null, doc, (IWebProxy) proxy, (ICredentials) creds);
      if (this._statusCode == HttpStatusCode.NotModified)
        doc.DetectEncodingAndLoad(this.GetCachePath(uri));
      return doc;
    }

    private void SaveCacheHeaders(Uri requestUri, HttpWebResponse resp)
    {
      string cacheHeadersPath = this.GetCacheHeadersPath(requestUri);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<c></c>");
      XmlNode firstChild = xmlDocument.FirstChild;
      foreach (string header in (NameObjectCollectionBase) resp.Headers)
      {
        XmlNode element = (XmlNode) xmlDocument.CreateElement("h");
        XmlAttribute attribute1 = xmlDocument.CreateAttribute("n");
        attribute1.Value = header;
        element.Attributes.Append(attribute1);
        XmlAttribute attribute2 = xmlDocument.CreateAttribute("v");
        attribute2.Value = resp.Headers[header];
        element.Attributes.Append(attribute2);
        firstChild.AppendChild(element);
      }
      xmlDocument.Save(cacheHeadersPath);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    public Task<HtmlDocument> LoadFromWebAsync(string url) => this.LoadFromWebAsync(new Uri(url), (Encoding) null, (NetworkCredential) null);

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      CancellationToken cancellationToken)
    {
      return this.LoadFromWebAsync(new Uri(url), (Encoding) null, (NetworkCredential) null, cancellationToken);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="encoding">The encoding to use while downloading the document</param>
    public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding) => this.LoadFromWebAsync(new Uri(url), encoding, (NetworkCredential) null, CancellationToken.None);

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="encoding">The encoding to use while downloading the document</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      Encoding encoding,
      CancellationToken cancellationToken)
    {
      return this.LoadFromWebAsync(new Uri(url), encoding, (NetworkCredential) null, cancellationToken);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="encoding">The encoding to use while downloading the document</param>
    /// <param name="userName">Username to use for credentials in the web request</param>
    /// <param name="password">Password to use for credentials in the web request</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      Encoding encoding,
      string userName,
      string password)
    {
      return this.LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password), CancellationToken.None);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="encoding">The encoding to use while downloading the document</param>
    /// <param name="userName">Username to use for credentials in the web request</param>
    /// <param name="password">Password to use for credentials in the web request</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      Encoding encoding,
      string userName,
      string password,
      CancellationToken cancellationToken)
    {
      return this.LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password), cancellationToken);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="encoding">The encoding to use while downloading the document</param>
    /// <param name="userName">Username to use for credentials in the web request</param>
    /// <param name="password">Password to use for credentials in the web request</param>
    /// <param name="domain">Domain to use for credentials in the web request</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      Encoding encoding,
      string userName,
      string password,
      string domain)
    {
      return this.LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password, domain), CancellationToken.None);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="encoding">The encoding to use while downloading the document</param>
    /// <param name="userName">Username to use for credentials in the web request</param>
    /// <param name="password">Password to use for credentials in the web request</param>
    /// <param name="domain">Domain to use for credentials in the web request</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      Encoding encoding,
      string userName,
      string password,
      string domain,
      CancellationToken cancellationToken)
    {
      return this.LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password, domain), cancellationToken);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="userName">Username to use for credentials in the web request</param>
    /// <param name="password">Password to use for credentials in the web request</param>
    /// <param name="domain">Domain to use for credentials in the web request</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      string userName,
      string password,
      string domain)
    {
      return this.LoadFromWebAsync(new Uri(url), (Encoding) null, new NetworkCredential(userName, password, domain), CancellationToken.None);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="userName">Username to use for credentials in the web request</param>
    /// <param name="password">Password to use for credentials in the web request</param>
    /// <param name="domain">Domain to use for credentials in the web request</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      string userName,
      string password,
      string domain,
      CancellationToken cancellationToken)
    {
      return this.LoadFromWebAsync(new Uri(url), (Encoding) null, new NetworkCredential(userName, password, domain), cancellationToken);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="userName">Username to use for credentials in the web request</param>
    /// <param name="password">Password to use for credentials in the web request</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      string userName,
      string password)
    {
      return this.LoadFromWebAsync(new Uri(url), (Encoding) null, new NetworkCredential(userName, password), CancellationToken.None);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="userName">Username to use for credentials in the web request</param>
    /// <param name="password">Password to use for credentials in the web request</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      string userName,
      string password,
      CancellationToken cancellationToken)
    {
      return this.LoadFromWebAsync(new Uri(url), (Encoding) null, new NetworkCredential(userName, password), cancellationToken);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="credentials">The credentials to use for authenticating the web request</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      NetworkCredential credentials)
    {
      return this.LoadFromWebAsync(new Uri(url), (Encoding) null, credentials, CancellationToken.None);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="url">Url to the html document</param>
    /// <param name="credentials">The credentials to use for authenticating the web request</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      string url,
      NetworkCredential credentials,
      CancellationToken cancellationToken)
    {
      return this.LoadFromWebAsync(new Uri(url), (Encoding) null, credentials, cancellationToken);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="uri">Url to the html document</param>
    /// <param name="encoding">The encoding to use while downloading the document</param>
    /// <param name="credentials">The credentials to use for authenticating the web request</param>
    public Task<HtmlDocument> LoadFromWebAsync(
      Uri uri,
      Encoding encoding,
      NetworkCredential credentials)
    {
      return this.LoadFromWebAsync(uri, encoding, credentials, CancellationToken.None);
    }

    /// <summary>
    /// Begins the process of downloading an internet resource
    /// </summary>
    /// <param name="uri">Url to the html document</param>
    /// <param name="encoding">The encoding to use while downloading the document</param>
    /// <param name="credentials">The credentials to use for authenticating the web request</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public async Task<HtmlDocument> LoadFromWebAsync(
      Uri uri,
      Encoding encoding,
      NetworkCredential credentials,
      CancellationToken cancellationToken)
    {
      HtmlDocument doc = new HtmlDocument();
      HttpClientHandler httpClientHandler = new HttpClientHandler();
      if (credentials == null)
        httpClientHandler.UseDefaultCredentials = true;
      else
        httpClientHandler.Credentials = (ICredentials) credentials;
      if (this.CaptureRedirect)
        httpClientHandler.AllowAutoRedirect = false;
      HttpClient httpClient = new HttpClient((HttpMessageHandler) httpClientHandler);
      httpClient.DefaultRequestHeaders.Add("User-Agent", this.UserAgent);
      HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
      string html = string.Empty;
      if (encoding != null)
      {
        using (StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false), encoding))
          html = streamReader.ReadToEnd();
      }
      else
        html = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
      if (this.PreHandleDocument != null)
        this.PreHandleDocument(doc);
      if (html != null)
        doc.LoadHtml(html);
      HtmlDocument htmlDocument = doc;
      doc = (HtmlDocument) null;
      return htmlDocument;
    }

    /// <summary>Gets or sets the web browser timeout.</summary>
    public TimeSpan BrowserTimeout
    {
      get => this._browserTimeout;
      set => this._browserTimeout = value;
    }

    /// <summary>Gets or sets the web browser delay.</summary>
    public TimeSpan BrowserDelay
    {
      get => this._browserDelay;
      set => this._browserDelay = value;
    }

    /// <summary>Loads HTML using a WebBrowser and Application.DoEvents.</summary>
    /// <exception cref="T:System.Exception">Thrown when an exception error condition occurs.</exception>
    /// <param name="url">The requested URL, such as "http://html-agility-pack.net/".</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument LoadFromBrowser(string url) => this.LoadFromBrowser(url, (Func<object, bool>) (browser => true));

    internal string WebBrowserOuterHtml(object webBrowser)
    {
      try
      {
        this._responseUri = (Uri) webBrowser.GetType().GetProperty("Url").GetValue(webBrowser, (object[]) null);
      }
      catch
      {
      }
      object obj1 = webBrowser.GetType().GetProperty("Document").GetValue(webBrowser, (object[]) null);
      object obj2 = obj1.GetType().GetMethod("GetElementsByTagName", new Type[1]
      {
        typeof (string)
      }).Invoke(obj1, (object[]) new string[1]{ "HTML" });
      object obj3 = obj2.GetType().GetProperty("Item", new Type[1]
      {
        typeof (int)
      }).GetValue(obj2, new object[1]{ (object) 0 });
      return (string) obj3.GetType().GetProperty("OuterHtml").GetValue(obj3, (object[]) null);
    }

    /// <summary>Loads HTML using a WebBrowser and Application.DoEvents.</summary>
    /// <exception cref="T:System.Exception">Thrown when an exception error condition occurs.</exception>
    /// <param name="url">The requested URL, such as "http://html-agility-pack.net/".</param>
    /// <param name="isBrowserScriptCompleted">(Optional) Check if the browser script has all been run and completed.</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument LoadFromBrowser(
      string url,
      Func<string, bool> isBrowserScriptCompleted = null)
    {
      return this.LoadFromBrowser(url, (Func<object, bool>) (browser => isBrowserScriptCompleted == null || isBrowserScriptCompleted(this.WebBrowserOuterHtml(browser))));
    }

    /// <summary>Loads HTML using a WebBrowser and Application.DoEvents.</summary>
    /// <exception cref="T:System.Exception">Thrown when an exception error condition occurs.</exception>
    /// <param name="url">The requested URL, such as "http://html-agility-pack.net/".</param>
    /// <param name="isBrowserScriptCompleted">(Optional) Check if the browser script has all been run and completed.</param>
    /// <returns>A new HTML document.</returns>
    public HtmlDocument LoadFromBrowser(
      string url,
      Func<object, bool> isBrowserScriptCompleted = null)
    {
      Assembly assembly1 = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (x => x.GetName().Name == "System.Windows.Forms"));
      if (assembly1 == (Assembly) null)
      {
        try
        {
          Assembly assembly2 = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (x => x.GetName().Name == "System"));
          if (assembly2 != (Assembly) null)
            Assembly.LoadFile(assembly2.CodeBase.Replace("System", "System.Windows.Forms").Replace("file:///", ""));
        }
        catch (Exception ex)
        {
        }
        assembly1 = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (x => x.GetName().Name == "System.Windows.Forms"));
        if (assembly1 == (Assembly) null)
          throw new Exception("Oops! No reference to System.Windows.Forms have been loaded. Make sure your project load any type from this assembly to make sure the reference is added to the domain assemblies list. Example: `var webBrowserType = typeof(WebBrowser);`");
      }
      Type type = assembly1.GetType("System.Windows.Forms.WebBrowser");
      ConstructorInfo constructor = type.GetConstructor(new Type[0]);
      MethodInfo method = assembly1.GetType("System.Windows.Forms.Application").GetMethod("DoEvents");
      Uri uri = new Uri(url);
      HtmlDocument htmlDocument = new HtmlDocument();
      string message = "WebBrowser Execution Timeout Expired. The timeout period elapsed prior to completion of the operation. To avoid this error, increase the WebBrowserTimeout value or set it to 0 (unlimited).";
      using (IDisposable disposable = (IDisposable) constructor.Invoke(new object[0]))
      {
        type.GetProperty("ScriptErrorsSuppressed").SetValue((object) disposable, (object) true, (object[]) null);
        type.GetMethod("Navigate", new Type[1]
        {
          typeof (Uri)
        }).Invoke((object) disposable, new object[1]
        {
          (object) uri
        });
        PropertyInfo property1 = type.GetProperty("ReadyState");
        PropertyInfo property2 = type.GetProperty("IsBusy");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        TimeSpan browserTimeout;
        while ((int) property1.GetValue((object) disposable, (object[]) null) != 4 || (bool) property2.GetValue((object) disposable, (object[]) null))
        {
          browserTimeout = this.BrowserTimeout;
          if (browserTimeout.TotalMilliseconds != 0.0)
          {
            double elapsedMilliseconds = (double) stopwatch.ElapsedMilliseconds;
            browserTimeout = this.BrowserTimeout;
            double totalMilliseconds = browserTimeout.TotalMilliseconds;
            if (elapsedMilliseconds > totalMilliseconds)
              throw new Exception(message);
          }
          method.Invoke((object) null, new object[0]);
          Thread.Sleep(this._browserDelay);
        }
        if (isBrowserScriptCompleted != null)
        {
          while (!isBrowserScriptCompleted((object) disposable))
          {
            browserTimeout = this.BrowserTimeout;
            if (browserTimeout.TotalMilliseconds != 0.0)
            {
              double elapsedMilliseconds = (double) stopwatch.ElapsedMilliseconds;
              browserTimeout = this.BrowserTimeout;
              double totalMilliseconds = browserTimeout.TotalMilliseconds;
              if (elapsedMilliseconds > totalMilliseconds)
              {
                this.WebBrowserOuterHtml((object) disposable);
                throw new Exception(message);
              }
            }
            method.Invoke((object) null, new object[0]);
            Thread.Sleep(this._browserDelay);
          }
        }
        string html = this.WebBrowserOuterHtml((object) disposable);
        htmlDocument.LoadHtml(html);
      }
      return htmlDocument;
    }

    /// <summary>
    /// Creates an instance of the given type from the specified Internet resource.
    /// </summary>
    /// <param name="htmlUrl">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="xsltUrl">The URL that specifies the XSLT stylesheet to load.</param>
    /// <param name="xsltArgs">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform.</param>
    /// <param name="type">The requested type.</param>
    /// <returns>An newly created instance.</returns>
    public object CreateInstance(
      string htmlUrl,
      string xsltUrl,
      XsltArgumentList xsltArgs,
      Type type)
    {
      return this.CreateInstance(htmlUrl, xsltUrl, xsltArgs, type, (string) null);
    }

    /// <summary>
    /// Creates an instance of the given type from the specified Internet resource.
    /// </summary>
    /// <param name="htmlUrl">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="xsltUrl">The URL that specifies the XSLT stylesheet to load.</param>
    /// <param name="xsltArgs">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform.</param>
    /// <param name="type">The requested type.</param>
    /// <param name="xmlPath">A file path where the temporary XML before transformation will be saved. Mostly used for debugging purposes.</param>
    /// <returns>An newly created instance.</returns>
    public object CreateInstance(
      string htmlUrl,
      string xsltUrl,
      XsltArgumentList xsltArgs,
      Type type,
      string xmlPath)
    {
      StringWriter stringWriter = new StringWriter();
      XmlTextWriter writer = new XmlTextWriter((TextWriter) stringWriter);
      if (xsltUrl == null)
        this.LoadHtmlAsXml(htmlUrl, writer);
      else if (xmlPath == null)
        this.LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, writer);
      else
        this.LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, writer, xmlPath);
      writer.Flush();
      XmlTextReader xmlTextReader = new XmlTextReader((TextReader) new StringReader(stringWriter.ToString()));
      XmlSerializer xmlSerializer = new XmlSerializer(type);
      try
      {
        return xmlSerializer.Deserialize((XmlReader) xmlTextReader);
      }
      catch (InvalidOperationException ex)
      {
        throw new Exception(ex?.ToString() + ", --- xml:" + stringWriter?.ToString());
      }
    }

    /// <summary>
    /// Loads an HTML document from an Internet resource and saves it to the specified XmlTextWriter, after an XSLT transformation.
    /// </summary>
    /// <param name="htmlUrl">The requested URL, such as "http://Myserver/Mypath/Myfile.asp".</param>
    /// <param name="xsltUrl">The URL that specifies the XSLT stylesheet to load.</param>
    /// <param name="xsltArgs">An XsltArgumentList containing the namespace-qualified arguments used as input to the transform.</param>
    /// <param name="writer">The XmlTextWriter to which you want to save.</param>
    public void LoadHtmlAsXml(
      string htmlUrl,
      string xsltUrl,
      XsltArgumentList xsltArgs,
      XmlTextWriter writer)
    {
      this.LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, writer, (string) null);
    }

    /// <summary>
    /// Loads an HTML document from an Internet resource and saves it to the specified XmlTextWriter, after an XSLT transformation.
    /// </summary>
    /// <param name="htmlUrl">The requested URL, such as "http://Myserver/Mypath/Myfile.asp". May not be null.</param>
    /// <param name="xsltUrl">The URL that specifies the XSLT stylesheet to load.</param>
    /// <param name="xsltArgs">An XsltArgumentList containing the namespace-qualified arguments used as input to the transform.</param>
    /// <param name="writer">The XmlTextWriter to which you want to save.</param>
    /// <param name="xmlPath">A file path where the temporary XML before transformation will be saved. Mostly used for debugging purposes.</param>
    public void LoadHtmlAsXml(
      string htmlUrl,
      string xsltUrl,
      XsltArgumentList xsltArgs,
      XmlTextWriter writer,
      string xmlPath)
    {
      HtmlDocument htmlDocument = htmlUrl != null ? this.Load(htmlUrl) : throw new ArgumentNullException(nameof (htmlUrl));
      if (xmlPath != null)
      {
        XmlTextWriter xmlTextWriter = new XmlTextWriter(xmlPath, htmlDocument.Encoding);
        htmlDocument.Save((XmlWriter) xmlTextWriter);
        xmlTextWriter.Close();
      }
      if (xsltArgs == null)
        xsltArgs = new XsltArgumentList();
      xsltArgs.AddParam("url", "", (object) htmlUrl);
      xsltArgs.AddParam("requestDuration", "", (object) this.RequestDuration);
      xsltArgs.AddParam("fromCache", "", (object) this.FromCache);
      XslCompiledTransform compiledTransform = new XslCompiledTransform();
      compiledTransform.Load(xsltUrl);
      compiledTransform.Transform((IXPathNavigable) htmlDocument, xsltArgs, (XmlWriter) writer);
    }

    /// <summary>
    /// Represents the method that will handle the PostResponse event.
    /// </summary>
    public delegate void PostResponseHandler(HttpWebRequest request, HttpWebResponse response);

    /// <summary>
    /// Represents the method that will handle the PreHandleDocument event.
    /// </summary>
    public delegate void PreHandleDocumentHandler(HtmlDocument document);

    /// <summary>
    /// Represents the method that will handle the PreRequest event.
    /// </summary>
    public delegate bool PreRequestHandler(HttpWebRequest request);
  }
}
