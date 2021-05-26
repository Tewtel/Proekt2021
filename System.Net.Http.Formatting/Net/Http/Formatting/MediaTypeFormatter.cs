// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeFormatter
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary> Base class to handle serializing and deserializing strongly-typed objects using <see cref="T:System.Net.Http.ObjectContent" />. </summary>
  public abstract class MediaTypeFormatter
  {
    private const int DefaultMinHttpCollectionKeys = 1;
    private const int DefaultMaxHttpCollectionKeys = 1000;
    private const string IWellKnownComparerTypeName = "System.IWellKnownStringEqualityComparer, mscorlib, Version=4.0.0.0, PublicKeyToken=b77a5c561934e089";
    private static readonly ConcurrentDictionary<Type, Type> _delegatingEnumerableCache = new ConcurrentDictionary<Type, Type>();
    private static ConcurrentDictionary<Type, ConstructorInfo> _delegatingEnumerableConstructorCache = new ConcurrentDictionary<Type, ConstructorInfo>();
    private static Lazy<int> _defaultMaxHttpCollectionKeys = new Lazy<int>(new Func<int>(MediaTypeFormatter.InitializeDefaultCollectionKeySize), true);
    private static int _maxHttpCollectionKeys = -1;
    private readonly List<MediaTypeHeaderValue> _supportedMediaTypes;
    private readonly List<Encoding> _supportedEncodings;
    private readonly List<MediaTypeMapping> _mediaTypeMappings;
    private IRequiredMemberSelector _requiredMemberSelector;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> class.</summary>
    protected MediaTypeFormatter()
    {
      this._supportedMediaTypes = new List<MediaTypeHeaderValue>();
      this.SupportedMediaTypes = (Collection<MediaTypeHeaderValue>) new MediaTypeFormatter.MediaTypeHeaderValueCollection((IList<MediaTypeHeaderValue>) this._supportedMediaTypes);
      this._supportedEncodings = new List<Encoding>();
      this.SupportedEncodings = new Collection<Encoding>((IList<Encoding>) this._supportedEncodings);
      this._mediaTypeMappings = new List<MediaTypeMapping>();
      this.MediaTypeMappings = new Collection<MediaTypeMapping>((IList<MediaTypeMapping>) this._mediaTypeMappings);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> class.</summary>
    /// <param name="formatter">The <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> instance to copy settings from.</param>
    protected MediaTypeFormatter(MediaTypeFormatter formatter)
    {
      this._supportedMediaTypes = formatter != null ? formatter._supportedMediaTypes : throw Error.ArgumentNull(nameof (formatter));
      this.SupportedMediaTypes = formatter.SupportedMediaTypes;
      this._supportedEncodings = formatter._supportedEncodings;
      this.SupportedEncodings = formatter.SupportedEncodings;
      this._mediaTypeMappings = formatter._mediaTypeMappings;
      this.MediaTypeMappings = formatter.MediaTypeMappings;
      this._requiredMemberSelector = formatter._requiredMemberSelector;
    }

    /// <summary>Gets or sets the maximum number of keys stored in a T: <see cref="System.Collections.Specialized.NameValueCollection" />.</summary>
    /// <returns>The maximum number of keys.</returns>
    public static int MaxHttpCollectionKeys
    {
      get
      {
        if (MediaTypeFormatter._maxHttpCollectionKeys < 0)
          MediaTypeFormatter._maxHttpCollectionKeys = MediaTypeFormatter._defaultMaxHttpCollectionKeys.Value;
        return MediaTypeFormatter._maxHttpCollectionKeys;
      }
      set => MediaTypeFormatter._maxHttpCollectionKeys = value >= 1 ? value : throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (value), (object) value, (object) 1);
    }

    /// <summary>Gets the mutable collection of media types supported bythis <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" />.</summary>
    /// <returns>The collection of <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> objects.</returns>
    public Collection<MediaTypeHeaderValue> SupportedMediaTypes { get; private set; }

    internal List<MediaTypeHeaderValue> SupportedMediaTypesInternal => this._supportedMediaTypes;

    /// <summary>Gets the mutable collection of character encodings supported bythis <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" />.</summary>
    /// <returns>The collection of <see cref="T:System.Text.Encoding" /> objects.</returns>
    public Collection<Encoding> SupportedEncodings { get; private set; }

    internal List<Encoding> SupportedEncodingsInternal => this._supportedEncodings;

    /// <summary>Gets the mutable collection of <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" /> objects that match HTTP requests to media types.</summary>
    /// <returns>The <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" /> collection.</returns>
    public Collection<MediaTypeMapping> MediaTypeMappings { get; private set; }

    internal List<MediaTypeMapping> MediaTypeMappingsInternal => this._mediaTypeMappings;

    /// <summary>Gets or sets the <see cref="T:System.Net.Http.Formatting.IRequiredMemberSelector" /> instance used to determine required members.</summary>
    /// <returns>The <see cref="T:System.Net.Http.Formatting.IRequiredMemberSelector" /> instance.</returns>
    public virtual IRequiredMemberSelector RequiredMemberSelector
    {
      get => this._requiredMemberSelector;
      set => this._requiredMemberSelector = value;
    }

    internal virtual bool CanWriteAnyTypes => true;

    /// <summary>Asynchronously deserializes an object of the specified type.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> whose result will be an object of the given type.</returns>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <param name="readStream">The <see cref="T:System.IO.Stream" /> to read.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. It may be null.</param>
    /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
    /// <exception cref="T:System.NotSupportedException">Derived types need to support reading.</exception>
    public virtual Task<object> ReadFromStreamAsync(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger)
    {
      throw Error.NotSupported(Resources.MediaTypeFormatterCannotRead, (object) this.GetType().Name);
    }

    /// <summary>Asynchronously deserializes an object of the specified type.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> whose result will be an object of the given type.</returns>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <param name="readStream">The <see cref="T:System.IO.Stream" /> to read.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. It may be null.</param>
    /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public virtual Task<object> ReadFromStreamAsync(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger,
      CancellationToken cancellationToken)
    {
      return cancellationToken.IsCancellationRequested ? TaskHelpers.Canceled<object>() : this.ReadFromStreamAsync(type, readStream, content, formatterLogger);
    }

    /// <summary>Asynchronously writes an object of the specified type.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that will perform the write.</returns>
    /// <param name="type">The type of the object to write.</param>
    /// <param name="value">The object value to write.  It may be null.</param>
    /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> if available. It may be null.</param>
    /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" /> if available. It may be null.</param>
    /// <exception cref="T:System.NotSupportedException">Derived types need to support writing.</exception>
    public virtual Task WriteToStreamAsync(
      Type type,
      object value,
      Stream writeStream,
      HttpContent content,
      TransportContext transportContext)
    {
      return this.WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
    }

    /// <summary>Asynchronously writes an object of the specified type.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that will perform the write.</returns>
    /// <param name="type">The type of the object to write.</param>
    /// <param name="value">The object value to write.  It may be null.</param>
    /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> if available. It may be null.</param>
    /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" /> if available. It may be null.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    /// <exception cref="T:System.NotSupportedException">Derived types need to support writing.</exception>
    public virtual Task WriteToStreamAsync(
      Type type,
      object value,
      Stream writeStream,
      HttpContent content,
      TransportContext transportContext,
      CancellationToken cancellationToken)
    {
      throw Error.NotSupported(Resources.MediaTypeFormatterCannotWrite, (object) this.GetType().Name);
    }

    private static bool TryGetDelegatingType(Type interfaceType, ref Type type)
    {
      if (type != (Type) null && type.IsInterface() && type.IsGenericType())
      {
        Type genericInterface = type.ExtractGenericInterface(interfaceType);
        if (genericInterface != (Type) null)
        {
          type = MediaTypeFormatter.GetOrAddDelegatingType(type, genericInterface);
          return true;
        }
      }
      return false;
    }

    private static int InitializeDefaultCollectionKeySize() => int.MaxValue;

    internal static bool TryGetDelegatingTypeForIEnumerableGenericOrSame(ref Type type) => MediaTypeFormatter.TryGetDelegatingType(FormattingUtilities.EnumerableInterfaceGenericType, ref type);

    internal static bool TryGetDelegatingTypeForIQueryableGenericOrSame(ref Type type) => MediaTypeFormatter.TryGetDelegatingType(FormattingUtilities.QueryableInterfaceGenericType, ref type);

    internal static ConstructorInfo GetTypeRemappingConstructor(Type type)
    {
      ConstructorInfo constructorInfo;
      MediaTypeFormatter._delegatingEnumerableConstructorCache.TryGetValue(type, out constructorInfo);
      return constructorInfo;
    }

    /// <summary>Determines the best character encoding for reading or writing an HTTP entity body, given a set of content headers.</summary>
    /// <returns>The encoding that is the best match.</returns>
    /// <param name="contentHeaders">The content headers.</param>
    public Encoding SelectCharacterEncoding(HttpContentHeaders contentHeaders)
    {
      Encoding encoding = (Encoding) null;
      if (contentHeaders != null && contentHeaders.ContentType != null)
      {
        string charSet = contentHeaders.ContentType.CharSet;
        if (!string.IsNullOrWhiteSpace(charSet))
        {
          for (int index = 0; index < this._supportedEncodings.Count; ++index)
          {
            Encoding supportedEncoding = this._supportedEncodings[index];
            if (charSet.Equals(supportedEncoding.WebName, StringComparison.OrdinalIgnoreCase))
            {
              encoding = supportedEncoding;
              break;
            }
          }
        }
      }
      if (encoding == null && this._supportedEncodings.Count > 0)
        encoding = this._supportedEncodings[0];
      return encoding != null ? encoding : throw Error.InvalidOperation(Resources.MediaTypeFormatterNoEncoding, (object) this.GetType().Name);
    }

    /// <summary> Sets the default headers for content that will be formatted using this formatter. This method is called from the <see cref="T:System.Net.Http.ObjectContent" /> constructor. This implementation sets the Content-Type header to the value of mediaType if it is not null. If it is null it sets the Content-Type to the default media type of this formatter. If the Content-Type does not specify a charset it will set it using this formatters configured <see cref="T:System.Text.Encoding" />. </summary>
    /// <param name="type">The type of the object being serialized. See <see cref="T:System.Net.Http.ObjectContent" />.</param>
    /// <param name="headers">The content headers that should be configured.</param>
    /// <param name="mediaType">The authoritative media type. Can be null.</param>
    public virtual void SetDefaultContentHeaders(
      Type type,
      HttpContentHeaders headers,
      MediaTypeHeaderValue mediaType)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (headers == null)
        throw Error.ArgumentNull(nameof (headers));
      if (mediaType != null)
        headers.ContentType = mediaType.Clone<MediaTypeHeaderValue>();
      if (headers.ContentType == null)
      {
        MediaTypeHeaderValue mediaTypeHeaderValue = (MediaTypeHeaderValue) null;
        if (this._supportedMediaTypes.Count > 0)
          mediaTypeHeaderValue = this._supportedMediaTypes[0];
        if (mediaTypeHeaderValue != null)
          headers.ContentType = mediaTypeHeaderValue.Clone<MediaTypeHeaderValue>();
      }
      if (headers.ContentType == null || headers.ContentType.CharSet != null)
        return;
      Encoding encoding = (Encoding) null;
      if (this._supportedEncodings.Count > 0)
        encoding = this._supportedEncodings[0];
      if (encoding == null)
        return;
      headers.ContentType.CharSet = encoding.WebName;
    }

    /// <summary>Returns a specialized instance of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> that can format a response for the given parameters.</summary>
    /// <returns>Returns <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" />.</returns>
    /// <param name="type">The type to format.</param>
    /// <param name="request">The request.</param>
    /// <param name="mediaType">The media type.</param>
    public virtual MediaTypeFormatter GetPerRequestFormatterInstance(
      Type type,
      HttpRequestMessage request,
      MediaTypeHeaderValue mediaType)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (request == null)
        throw Error.ArgumentNull(nameof (request));
      return this;
    }

    /// <summary>Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserializean object of the specified type.</summary>
    /// <returns>true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserialize the type; otherwise, false.</returns>
    /// <param name="type">The type to deserialize.</param>
    public abstract bool CanReadType(Type type);

    /// <summary>Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serializean object of the specified type.</summary>
    /// <returns>true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serialize the type; otherwise, false.</returns>
    /// <param name="type">The type to serialize.</param>
    public abstract bool CanWriteType(Type type);

    private static Type GetOrAddDelegatingType(Type type, Type genericType) => MediaTypeFormatter._delegatingEnumerableCache.GetOrAdd(type, (Func<Type, Type>) (typeToRemap =>
    {
      Type genericArgument = genericType.GetGenericArguments()[0];
      Type key = FormattingUtilities.DelegatingEnumerableGenericType.MakeGenericType(genericArgument);
      ConstructorInfo constructor = key.GetConstructor(new Type[1]
      {
        FormattingUtilities.EnumerableInterfaceGenericType.MakeGenericType(genericArgument)
      });
      MediaTypeFormatter._delegatingEnumerableConstructorCache.TryAdd(key, constructor);
      return key;
    }));

    /// <summary>Gets the default value for the specified type.</summary>
    /// <returns>The default value.</returns>
    /// <param name="type">The type for which to get the default value.</param>
    public static object GetDefaultValueForType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      return type.IsValueType() ? Activator.CreateInstance(type) : (object) null;
    }

    internal class MediaTypeHeaderValueCollection : Collection<MediaTypeHeaderValue>
    {
      private static readonly Type _mediaTypeHeaderValueType = typeof (MediaTypeHeaderValue);

      internal MediaTypeHeaderValueCollection(IList<MediaTypeHeaderValue> list)
        : base(list)
      {
      }

      protected override void InsertItem(int index, MediaTypeHeaderValue item)
      {
        MediaTypeFormatter.MediaTypeHeaderValueCollection.ValidateMediaType(item);
        base.InsertItem(index, item);
      }

      protected override void SetItem(int index, MediaTypeHeaderValue item)
      {
        MediaTypeFormatter.MediaTypeHeaderValueCollection.ValidateMediaType(item);
        base.SetItem(index, item);
      }

      private static void ValidateMediaType(MediaTypeHeaderValue item)
      {
        ParsedMediaTypeHeaderValue mediaTypeHeaderValue = item != null ? new ParsedMediaTypeHeaderValue(item) : throw Error.ArgumentNull(nameof (item));
        if (mediaTypeHeaderValue.IsAllMediaRange || mediaTypeHeaderValue.IsSubtypeMediaRange)
          throw Error.Argument(nameof (item), Resources.CannotUseMediaRangeForSupportedMediaType, (object) MediaTypeFormatter.MediaTypeHeaderValueCollection._mediaTypeHeaderValueType.Name, (object) item.MediaType);
      }
    }
  }
}
