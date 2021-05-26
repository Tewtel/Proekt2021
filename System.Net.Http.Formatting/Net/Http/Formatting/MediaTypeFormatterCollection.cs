// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeFormatterCollection
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Xml;
using System.Xml.Linq;

namespace System.Net.Http.Formatting
{
  /// <summary> Collection class that contains <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> instances. </summary>
  public class MediaTypeFormatterCollection : Collection<MediaTypeFormatter>
  {
    private static readonly Type _mediaTypeFormatterType = typeof (MediaTypeFormatter);
    private MediaTypeFormatter[] _writingFormatters;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterCollection" /> class.</summary>
    public MediaTypeFormatterCollection()
      : this(MediaTypeFormatterCollection.CreateDefaultFormatters())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterCollection" /> class.</summary>
    /// <param name="formatters">A collection of <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> instances to place in the collection.</param>
    public MediaTypeFormatterCollection(IEnumerable<MediaTypeFormatter> formatters) => this.VerifyAndSetFormatters(formatters);

    internal event EventHandler Changing;

    /// <summary>Gets the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> to use for XML.</summary>
    /// <returns>The <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" />to use for XML.</returns>
    public XmlMediaTypeFormatter XmlFormatter => this.Items.OfType<XmlMediaTypeFormatter>().FirstOrDefault<XmlMediaTypeFormatter>();

    /// <summary>Gets the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> to use for JSON.</summary>
    /// <returns>The <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> to use for JSON.</returns>
    public JsonMediaTypeFormatter JsonFormatter => this.Items.OfType<JsonMediaTypeFormatter>().FirstOrDefault<JsonMediaTypeFormatter>();

    /// <summary>Gets the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> to use for application/x-www-form-urlencoded data.</summary>
    /// <returns>The <see cref="T:System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter" />to use for application/x-www-form-urlencoded data.</returns>
    public FormUrlEncodedMediaTypeFormatter FormUrlEncodedFormatter => this.Items.OfType<FormUrlEncodedMediaTypeFormatter>().FirstOrDefault<FormUrlEncodedMediaTypeFormatter>();

    internal MediaTypeFormatter[] WritingFormatters
    {
      get
      {
        if (this._writingFormatters == null)
          this._writingFormatters = this.GetWritingFormatters();
        return this._writingFormatters;
      }
    }

    /// <summary>Adds the elements of the specified collection to the end of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterCollection" />.</summary>
    /// <param name="items">The items that should be added to the end of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterCollection" />. The items collection itself cannot be <see cref="null" />, but it can contain elements that are <see cref="null" />.</param>
    public void AddRange(IEnumerable<MediaTypeFormatter> items)
    {
      if (items == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (items));
      foreach (MediaTypeFormatter mediaTypeFormatter in items)
        this.Add(mediaTypeFormatter);
    }

    /// <summary>Inserts the elements of a collection into the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterCollection" /> at the specified index.</summary>
    /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
    /// <param name="items">The items that should be inserted into the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterCollection" />. The items collection itself cannot be <see cref="null" />, but it can contain elements that are <see cref="null" />.</param>
    public void InsertRange(int index, IEnumerable<MediaTypeFormatter> items)
    {
      if (items == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (items));
      foreach (MediaTypeFormatter mediaTypeFormatter in items)
        this.Insert(index++, mediaTypeFormatter);
    }

    /// <summary>Helper to search a collection for a formatter that can read the .NET type in the given mediaType.</summary>
    /// <returns>The formatter that can read the type. Null if no formatter found.</returns>
    /// <param name="type">The .NET type to read</param>
    /// <param name="mediaType">The media type to match on.</param>
    public MediaTypeFormatter FindReader(
      Type type,
      MediaTypeHeaderValue mediaType)
    {
      if (type == (Type) null)
        throw System.Web.Http.Error.ArgumentNull(nameof (type));
      if (mediaType == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (mediaType));
      foreach (MediaTypeFormatter mediaTypeFormatter in (IEnumerable<MediaTypeFormatter>) this.Items)
      {
        if (mediaTypeFormatter != null && mediaTypeFormatter.CanReadType(type))
        {
          foreach (MediaTypeHeaderValue supportedMediaType in mediaTypeFormatter.SupportedMediaTypes)
          {
            if (supportedMediaType != null && supportedMediaType.IsSubsetOf(mediaType))
              return mediaTypeFormatter;
          }
        }
      }
      return (MediaTypeFormatter) null;
    }

    /// <summary>Helper to search a collection for a formatter that can write the .NET type in the given mediaType.</summary>
    /// <returns>The formatter that can write the type. Null if no formatter found.</returns>
    /// <param name="type">The .NET type to read</param>
    /// <param name="mediaType">The media type to match on.</param>
    public MediaTypeFormatter FindWriter(
      Type type,
      MediaTypeHeaderValue mediaType)
    {
      if (type == (Type) null)
        throw System.Web.Http.Error.ArgumentNull(nameof (type));
      if (mediaType == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (mediaType));
      foreach (MediaTypeFormatter mediaTypeFormatter in (IEnumerable<MediaTypeFormatter>) this.Items)
      {
        if (mediaTypeFormatter != null && mediaTypeFormatter.CanWriteType(type))
        {
          foreach (MediaTypeHeaderValue supportedMediaType in mediaTypeFormatter.SupportedMediaTypes)
          {
            if (supportedMediaType != null && supportedMediaType.IsSubsetOf(mediaType))
              return mediaTypeFormatter;
          }
        }
      }
      return (MediaTypeFormatter) null;
    }

    /// <summary>Returns true if the type is one of those loosely defined types that should be excluded from validation.</summary>
    /// <returns>true if the type should be excluded; otherwise, false.</returns>
    /// <param name="type">The .NET <see cref="T:System.Type" /> to validate.</param>
    public static bool IsTypeExcludedFromValidation(Type type) => typeof (XmlNode).IsAssignableFrom(type) || typeof (FormDataCollection).IsAssignableFrom(type) || (FormattingUtilities.IsJTokenType(type) || typeof (XObject).IsAssignableFrom(type)) || typeof (Type).IsAssignableFrom(type) || type == typeof (byte[]);

    /// <summary>Removes all items in the collection.</summary>
    protected override void ClearItems()
    {
      this.OnChanging();
      base.ClearItems();
    }

    /// <summary>Inserts the specified item at the specified index in the collection.</summary>
    /// <param name="index">The index to insert at.</param>
    /// <param name="item">The item to insert.</param>
    protected override void InsertItem(int index, MediaTypeFormatter item)
    {
      this.OnChanging();
      base.InsertItem(index, item);
    }

    /// <summary>Removes the item at the specified index.</summary>
    /// <param name="index">The index of the item to remove.</param>
    protected override void RemoveItem(int index)
    {
      this.OnChanging();
      base.RemoveItem(index);
    }

    /// <summary>Assigns the item at the specified index in the collection.</summary>
    /// <param name="index">The index to insert at.</param>
    /// <param name="item">The item to assign.</param>
    protected override void SetItem(int index, MediaTypeFormatter item)
    {
      this.OnChanging();
      base.SetItem(index, item);
    }

    private void OnChanging()
    {
      if (this.Changing != null)
        this.Changing((object) this, EventArgs.Empty);
      this._writingFormatters = (MediaTypeFormatter[]) null;
    }

    private MediaTypeFormatter[] GetWritingFormatters() => this.Items.Where<MediaTypeFormatter>((Func<MediaTypeFormatter, bool>) (formatter => formatter != null && formatter.CanWriteAnyTypes)).ToArray<MediaTypeFormatter>();

    private static IEnumerable<MediaTypeFormatter> CreateDefaultFormatters() => (IEnumerable<MediaTypeFormatter>) new MediaTypeFormatter[3]
    {
      (MediaTypeFormatter) new JsonMediaTypeFormatter(),
      (MediaTypeFormatter) new XmlMediaTypeFormatter(),
      (MediaTypeFormatter) new FormUrlEncodedMediaTypeFormatter()
    };

    private void VerifyAndSetFormatters(IEnumerable<MediaTypeFormatter> formatters)
    {
      if (formatters == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (formatters));
      foreach (MediaTypeFormatter formatter in formatters)
      {
        if (formatter == null)
          throw System.Web.Http.Error.Argument(nameof (formatters), Resources.CannotHaveNullInList, (object) MediaTypeFormatterCollection._mediaTypeFormatterType.Name);
        this.Add(formatter);
      }
    }
  }
}
