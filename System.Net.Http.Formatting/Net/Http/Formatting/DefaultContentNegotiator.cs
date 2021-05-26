// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.DefaultContentNegotiator
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace System.Net.Http.Formatting
{
  /// <summary> The default implementation of <see cref="T:System.Net.Http.Formatting.IContentNegotiator" />, which is used to select a <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> for an <see cref="T:System.Net.Http.HttpRequestMessage" /> or <see cref="T:System.Net.Http.HttpResponseMessage" />. </summary>
  public class DefaultContentNegotiator : IContentNegotiator
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.DefaultContentNegotiator" /> class.</summary>
    public DefaultContentNegotiator()
      : this(false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.DefaultContentNegotiator" /> class.</summary>
    /// <param name="excludeMatchOnTypeOnly">true to exclude formatters that match only on the object type; otherwise, false.</param>
    public DefaultContentNegotiator(bool excludeMatchOnTypeOnly) => this.ExcludeMatchOnTypeOnly = excludeMatchOnTypeOnly;

    /// <summary>If true, exclude formatters that match only on the object type; otherwise, false.</summary>
    /// <returns>Returns a <see cref="T:System.Boolean" />.</returns>
    public bool ExcludeMatchOnTypeOnly { get; private set; }

    /// <summary>Performs content negotiating by selecting the most appropriate <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> out of the passed in <paramref name="formatters" /> for the given <paramref name="request" /> that can serialize an object of the given <paramref name="type" />.</summary>
    /// <returns>The result of the negotiation containing the most appropriate <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> instance, or null if there is no appropriate formatter.</returns>
    /// <param name="type">The type to be serialized.</param>
    /// <param name="request">The request.</param>
    /// <param name="formatters">The set of <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> objects from which to choose.</param>
    public virtual ContentNegotiationResult Negotiate(
      Type type,
      HttpRequestMessage request,
      IEnumerable<MediaTypeFormatter> formatters)
    {
      if (type == (Type) null)
        throw System.Web.Http.Error.ArgumentNull(nameof (type));
      if (request == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (request));
      if (formatters == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (formatters));
      MediaTypeFormatterMatch typeFormatterMatch = this.SelectResponseMediaTypeFormatter((ICollection<MediaTypeFormatterMatch>) this.ComputeFormatterMatches(type, request, formatters));
      if (typeFormatterMatch == null)
        return (ContentNegotiationResult) null;
      Encoding encoding = this.SelectResponseCharacterEncoding(request, typeFormatterMatch.Formatter);
      if (encoding != null)
        typeFormatterMatch.MediaType.CharSet = encoding.WebName;
      MediaTypeHeaderValue mediaType = typeFormatterMatch.MediaType;
      return new ContentNegotiationResult(typeFormatterMatch.Formatter.GetPerRequestFormatterInstance(type, request, mediaType), mediaType);
    }

    /// <summary>Determines how well each formatter matches an HTTP request.</summary>
    /// <returns>Returns a collection of <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> objects that represent all of the matches.</returns>
    /// <param name="type">The type to be serialized.</param>
    /// <param name="request">The request.</param>
    /// <param name="formatters">The set of <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> objects from which to choose.</param>
    protected virtual Collection<MediaTypeFormatterMatch> ComputeFormatterMatches(
      Type type,
      HttpRequestMessage request,
      IEnumerable<MediaTypeFormatter> formatters)
    {
      if (type == (Type) null)
        throw System.Web.Http.Error.ArgumentNull(nameof (type));
      if (request == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (request));
      if (formatters == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (formatters));
      IEnumerable<MediaTypeWithQualityHeaderValue> sortedAcceptValues = (IEnumerable<MediaTypeWithQualityHeaderValue>) null;
      ListWrapperCollection<MediaTypeFormatterMatch> wrapperCollection = new ListWrapperCollection<MediaTypeFormatterMatch>();
      foreach (MediaTypeFormatter writingFormatter in DefaultContentNegotiator.GetWritingFormatters(formatters))
      {
        if (writingFormatter.CanWriteType(type))
        {
          MediaTypeFormatterMatch typeFormatterMatch1;
          if ((typeFormatterMatch1 = this.MatchMediaTypeMapping(request, writingFormatter)) != null)
          {
            wrapperCollection.Add(typeFormatterMatch1);
          }
          else
          {
            if (sortedAcceptValues == null)
              sortedAcceptValues = this.SortMediaTypeWithQualityHeaderValuesByQFactor((ICollection<MediaTypeWithQualityHeaderValue>) request.Headers.Accept);
            MediaTypeFormatterMatch typeFormatterMatch2;
            if ((typeFormatterMatch2 = this.MatchAcceptHeader(sortedAcceptValues, writingFormatter)) != null)
            {
              wrapperCollection.Add(typeFormatterMatch2);
            }
            else
            {
              MediaTypeFormatterMatch typeFormatterMatch3;
              if ((typeFormatterMatch3 = this.MatchRequestMediaType(request, writingFormatter)) != null)
              {
                wrapperCollection.Add(typeFormatterMatch3);
              }
              else
              {
                MediaTypeFormatterMatch typeFormatterMatch4;
                if (this.ShouldMatchOnType(sortedAcceptValues) && (typeFormatterMatch4 = this.MatchType(type, writingFormatter)) != null)
                  wrapperCollection.Add(typeFormatterMatch4);
              }
            }
          }
        }
      }
      return (Collection<MediaTypeFormatterMatch>) wrapperCollection;
    }

    /// <summary>Select the best match among the candidate matches found.</summary>
    /// <returns>Returns the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> object that represents the best match. </returns>
    /// <param name="matches">The collection of matches.</param>
    protected virtual MediaTypeFormatterMatch SelectResponseMediaTypeFormatter(
      ICollection<MediaTypeFormatterMatch> matches)
    {
      List<MediaTypeFormatterMatch> typeFormatterMatchList = matches != null ? matches.AsList<MediaTypeFormatterMatch>() : throw System.Web.Http.Error.ArgumentNull(nameof (matches));
      MediaTypeFormatterMatch typeFormatterMatch1 = (MediaTypeFormatterMatch) null;
      MediaTypeFormatterMatch typeFormatterMatch2 = (MediaTypeFormatterMatch) null;
      MediaTypeFormatterMatch typeFormatterMatch3 = (MediaTypeFormatterMatch) null;
      MediaTypeFormatterMatch typeFormatterMatch4 = (MediaTypeFormatterMatch) null;
      MediaTypeFormatterMatch current1 = (MediaTypeFormatterMatch) null;
      MediaTypeFormatterMatch typeFormatterMatch5 = (MediaTypeFormatterMatch) null;
      for (int index = 0; index < typeFormatterMatchList.Count; ++index)
      {
        MediaTypeFormatterMatch potentialReplacement = typeFormatterMatchList[index];
        switch (potentialReplacement.Ranking)
        {
          case MediaTypeFormatterMatchRanking.MatchOnCanWriteType:
            if (typeFormatterMatch1 == null)
            {
              typeFormatterMatch1 = potentialReplacement;
              break;
            }
            break;
          case MediaTypeFormatterMatchRanking.MatchOnRequestAcceptHeaderLiteral:
            typeFormatterMatch2 = this.UpdateBestMatch(typeFormatterMatch2, potentialReplacement);
            break;
          case MediaTypeFormatterMatchRanking.MatchOnRequestAcceptHeaderSubtypeMediaRange:
            typeFormatterMatch3 = this.UpdateBestMatch(typeFormatterMatch3, potentialReplacement);
            break;
          case MediaTypeFormatterMatchRanking.MatchOnRequestAcceptHeaderAllMediaRange:
            typeFormatterMatch4 = this.UpdateBestMatch(typeFormatterMatch4, potentialReplacement);
            break;
          case MediaTypeFormatterMatchRanking.MatchOnRequestWithMediaTypeMapping:
            current1 = this.UpdateBestMatch(current1, potentialReplacement);
            break;
          case MediaTypeFormatterMatchRanking.MatchOnRequestMediaType:
            if (typeFormatterMatch5 == null)
            {
              typeFormatterMatch5 = potentialReplacement;
              break;
            }
            break;
        }
      }
      if (current1 != null && this.UpdateBestMatch(this.UpdateBestMatch(this.UpdateBestMatch(current1, typeFormatterMatch2), typeFormatterMatch3), typeFormatterMatch4) != current1)
        current1 = (MediaTypeFormatterMatch) null;
      MediaTypeFormatterMatch current2 = (MediaTypeFormatterMatch) null;
      if (current1 != null)
        current2 = current1;
      else if (typeFormatterMatch2 != null || typeFormatterMatch3 != null || typeFormatterMatch4 != null)
        current2 = this.UpdateBestMatch(this.UpdateBestMatch(this.UpdateBestMatch(current2, typeFormatterMatch2), typeFormatterMatch3), typeFormatterMatch4);
      else if (typeFormatterMatch5 != null)
        current2 = typeFormatterMatch5;
      else if (typeFormatterMatch1 != null)
        current2 = typeFormatterMatch1;
      return current2;
    }

    /// <summary>Determines the best character encoding for writing the response.</summary>
    /// <returns>Returns the <see cref="T:System.Text.Encoding" /> that is the best match.</returns>
    /// <param name="request">The request.</param>
    /// <param name="formatter">The selected media formatter.</param>
    protected virtual Encoding SelectResponseCharacterEncoding(
      HttpRequestMessage request,
      MediaTypeFormatter formatter)
    {
      if (request == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (request));
      List<Encoding> encodingList = formatter != null ? formatter.SupportedEncodingsInternal : throw System.Web.Http.Error.ArgumentNull(nameof (formatter));
      if (encodingList.Count <= 0)
        return (Encoding) null;
      foreach (StringWithQualityHeaderValue qualityHeaderValue in this.SortStringWithQualityHeaderValuesByQFactor((ICollection<StringWithQualityHeaderValue>) request.Headers.AcceptCharset))
      {
        for (int index = 0; index < encodingList.Count; ++index)
        {
          Encoding encoding = encodingList[index];
          if (encoding != null)
          {
            double? quality = qualityHeaderValue.Quality;
            double num = 0.0;
            if (!(quality.GetValueOrDefault() == num & quality.HasValue) && (qualityHeaderValue.Value.Equals(encoding.WebName, StringComparison.OrdinalIgnoreCase) || qualityHeaderValue.Value.Equals("*", StringComparison.OrdinalIgnoreCase)))
              return encoding;
          }
        }
      }
      return formatter.SelectCharacterEncoding(request.Content != null ? request.Content.Headers : (HttpContentHeaders) null);
    }

    /// <summary>Matches a request against the <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" /> objects in a media-type formatter.</summary>
    /// <returns>Returns a <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> object that indicates the quality of the match, or null if there is no match.</returns>
    /// <param name="request">The request to match.</param>
    /// <param name="formatter">The media-type formatter.</param>
    protected virtual MediaTypeFormatterMatch MatchMediaTypeMapping(
      HttpRequestMessage request,
      MediaTypeFormatter formatter)
    {
      if (request == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (request));
      List<MediaTypeMapping> mediaTypeMappingList = formatter != null ? formatter.MediaTypeMappingsInternal : throw System.Web.Http.Error.ArgumentNull(nameof (formatter));
      for (int index = 0; index < mediaTypeMappingList.Count; ++index)
      {
        MediaTypeMapping mediaTypeMapping = mediaTypeMappingList[index];
        double num;
        if (mediaTypeMapping != null && (num = mediaTypeMapping.TryMatchMediaType(request)) > 0.0)
          return new MediaTypeFormatterMatch(formatter, mediaTypeMapping.MediaType, new double?(num), MediaTypeFormatterMatchRanking.MatchOnRequestWithMediaTypeMapping);
      }
      return (MediaTypeFormatterMatch) null;
    }

    /// <summary>Matches a set of Accept header fields against the media types that a formatter supports.</summary>
    /// <returns>Returns a <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> object that indicates the quality of the match, or null if there is no match.</returns>
    /// <param name="sortedAcceptValues">A list of Accept header values, sorted in descending order of q factor. You can create this list by calling the <see cref="M:System.Net.Http.Formatting.DefaultContentNegotiator.SortStringWithQualityHeaderValuesByQFactor(System.Collections.Generic.ICollection{System.Net.Http.Headers.StringWithQualityHeaderValue})" /> method.</param>
    /// <param name="formatter">The formatter to match against.</param>
    protected virtual MediaTypeFormatterMatch MatchAcceptHeader(
      IEnumerable<MediaTypeWithQualityHeaderValue> sortedAcceptValues,
      MediaTypeFormatter formatter)
    {
      if (sortedAcceptValues == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (sortedAcceptValues));
      if (formatter == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (formatter));
      foreach (MediaTypeWithQualityHeaderValue sortedAcceptValue in sortedAcceptValues)
      {
        List<MediaTypeHeaderValue> mediaTypesInternal = formatter.SupportedMediaTypesInternal;
        for (int index = 0; index < mediaTypesInternal.Count; ++index)
        {
          MediaTypeHeaderValue mediaTypeHeaderValue = mediaTypesInternal[index];
          if (mediaTypeHeaderValue != null)
          {
            double? quality = sortedAcceptValue.Quality;
            double num = 0.0;
            MediaTypeHeaderValueRange mediaType2Range;
            if (!(quality.GetValueOrDefault() == num & quality.HasValue) && mediaTypeHeaderValue.IsSubsetOf((MediaTypeHeaderValue) sortedAcceptValue, out mediaType2Range))
            {
              MediaTypeFormatterMatchRanking ranking;
              switch (mediaType2Range)
              {
                case MediaTypeHeaderValueRange.SubtypeMediaRange:
                  ranking = MediaTypeFormatterMatchRanking.MatchOnRequestAcceptHeaderSubtypeMediaRange;
                  break;
                case MediaTypeHeaderValueRange.AllMediaRange:
                  ranking = MediaTypeFormatterMatchRanking.MatchOnRequestAcceptHeaderAllMediaRange;
                  break;
                default:
                  ranking = MediaTypeFormatterMatchRanking.MatchOnRequestAcceptHeaderLiteral;
                  break;
              }
              return new MediaTypeFormatterMatch(formatter, mediaTypeHeaderValue, sortedAcceptValue.Quality, ranking);
            }
          }
        }
      }
      return (MediaTypeFormatterMatch) null;
    }

    /// <summary>Match the content type of a request against the media types that a formatter supports.</summary>
    /// <returns>Returns a <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> object that indicates the quality of the match, or null if there is no match.</returns>
    /// <param name="request">The request to match.</param>
    /// <param name="formatter">The formatter to match against.</param>
    protected virtual MediaTypeFormatterMatch MatchRequestMediaType(
      HttpRequestMessage request,
      MediaTypeFormatter formatter)
    {
      if (request == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (request));
      if (formatter == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (formatter));
      if (request.Content != null)
      {
        MediaTypeHeaderValue contentType = request.Content.Headers.ContentType;
        if (contentType != null)
        {
          List<MediaTypeHeaderValue> mediaTypesInternal = formatter.SupportedMediaTypesInternal;
          for (int index = 0; index < mediaTypesInternal.Count; ++index)
          {
            MediaTypeHeaderValue mediaTypeHeaderValue = mediaTypesInternal[index];
            if (mediaTypeHeaderValue != null && mediaTypeHeaderValue.IsSubsetOf(contentType))
              return new MediaTypeFormatterMatch(formatter, mediaTypeHeaderValue, new double?(1.0), MediaTypeFormatterMatchRanking.MatchOnRequestMediaType);
          }
        }
      }
      return (MediaTypeFormatterMatch) null;
    }

    /// <summary> Determine whether to match on type or not. This is used to determine whether to generate a 406 response or use the default media type formatter in case there is no match against anything in the request. If ExcludeMatchOnTypeOnly is true  then we don't match on type unless there are no accept headers. </summary>
    /// <returns>True if not ExcludeMatchOnTypeOnly and accept headers with a q-factor bigger than 0.0 are present.</returns>
    /// <param name="sortedAcceptValues">The sorted accept header values to match.</param>
    protected virtual bool ShouldMatchOnType(
      IEnumerable<MediaTypeWithQualityHeaderValue> sortedAcceptValues)
    {
      if (sortedAcceptValues == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (sortedAcceptValues));
      return !this.ExcludeMatchOnTypeOnly || !sortedAcceptValues.Any<MediaTypeWithQualityHeaderValue>();
    }

    /// <summary>Selects the first supported media type of a formatter.</summary>
    /// <returns>Returns a <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> with <see cref="P:System.Net.Http.Formatting.MediaTypeFormatterMatch.Ranking" /> set to MatchOnCanWriteType, or null if there is no match. A <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> indicating the quality of the match or null is no match.</returns>
    /// <param name="type">The type to match.</param>
    /// <param name="formatter">The formatter to match against.</param>
    protected virtual MediaTypeFormatterMatch MatchType(
      Type type,
      MediaTypeFormatter formatter)
    {
      if (type == (Type) null)
        throw System.Web.Http.Error.ArgumentNull(nameof (type));
      if (formatter == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (formatter));
      MediaTypeHeaderValue mediaType = (MediaTypeHeaderValue) null;
      List<MediaTypeHeaderValue> mediaTypesInternal = formatter.SupportedMediaTypesInternal;
      if (mediaTypesInternal.Count > 0)
        mediaType = mediaTypesInternal[0];
      return new MediaTypeFormatterMatch(formatter, mediaType, new double?(1.0), MediaTypeFormatterMatchRanking.MatchOnCanWriteType);
    }

    /// <summary>Sorts Accept header values in descending order of q factor.</summary>
    /// <returns>Returns the sorted list of MediaTypeWithQualityHeaderValue objects.</returns>
    /// <param name="headerValues">A collection of StringWithQualityHeaderValue objects, representing the header fields.</param>
    protected virtual IEnumerable<MediaTypeWithQualityHeaderValue> SortMediaTypeWithQualityHeaderValuesByQFactor(
      ICollection<MediaTypeWithQualityHeaderValue> headerValues)
    {
      if (headerValues == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (headerValues));
      return headerValues.Count > 1 ? (IEnumerable<MediaTypeWithQualityHeaderValue>) headerValues.OrderByDescending<MediaTypeWithQualityHeaderValue, MediaTypeWithQualityHeaderValue>((Func<MediaTypeWithQualityHeaderValue, MediaTypeWithQualityHeaderValue>) (m => m), (IComparer<MediaTypeWithQualityHeaderValue>) MediaTypeWithQualityHeaderValueComparer.QualityComparer).ToArray<MediaTypeWithQualityHeaderValue>() : (IEnumerable<MediaTypeWithQualityHeaderValue>) headerValues;
    }

    /// <summary>Sorts a list of Accept-Charset, Accept-Encoding, Accept-Language or related header values in descending order or q factor.</summary>
    /// <returns>Returns the sorted list of StringWithQualityHeaderValue objects.</returns>
    /// <param name="headerValues">A collection of StringWithQualityHeaderValue objects, representing the header fields.</param>
    protected virtual IEnumerable<StringWithQualityHeaderValue> SortStringWithQualityHeaderValuesByQFactor(
      ICollection<StringWithQualityHeaderValue> headerValues)
    {
      if (headerValues == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (headerValues));
      return headerValues.Count > 1 ? (IEnumerable<StringWithQualityHeaderValue>) headerValues.OrderByDescending<StringWithQualityHeaderValue, StringWithQualityHeaderValue>((Func<StringWithQualityHeaderValue, StringWithQualityHeaderValue>) (m => m), (IComparer<StringWithQualityHeaderValue>) StringWithQualityHeaderValueComparer.QualityComparer).ToArray<StringWithQualityHeaderValue>() : (IEnumerable<StringWithQualityHeaderValue>) headerValues;
    }

    /// <summary>Evaluates whether a match is better than the current match.</summary>
    /// <returns>Returns whichever <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> object is a better match.</returns>
    /// <param name="current">The current match.</param>
    /// <param name="potentialReplacement">The match to evaluate against the current match.</param>
    protected virtual MediaTypeFormatterMatch UpdateBestMatch(
      MediaTypeFormatterMatch current,
      MediaTypeFormatterMatch potentialReplacement)
    {
      return potentialReplacement == null || current != null && potentialReplacement.Quality <= current.Quality ? current : potentialReplacement;
    }

    private static MediaTypeFormatter[] GetWritingFormatters(
      IEnumerable<MediaTypeFormatter> formatters)
    {
      return formatters is MediaTypeFormatterCollection formatterCollection ? formatterCollection.WritingFormatters : formatters.AsArray<MediaTypeFormatter>();
    }
  }
}
