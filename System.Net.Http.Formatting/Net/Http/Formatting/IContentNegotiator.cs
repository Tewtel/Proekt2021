// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.IContentNegotiator
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;

namespace System.Net.Http.Formatting
{
  /// <summary> Performs content negotiation.  This is the process of selecting a response writer (formatter) in compliance with header values in the request. </summary>
  public interface IContentNegotiator
  {
    /// <summary> Performs content negotiating by selecting the most appropriate <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> out of the passed in formatters for the given request that can serialize an object of the given type. </summary>
    /// <returns>The result of the negotiation containing the most appropriate <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> instance, or null if there is no appropriate formatter.</returns>
    /// <param name="type">The type to be serialized.</param>
    /// <param name="request">Request message, which contains the header values used to perform negotiation.</param>
    /// <param name="formatters">The set of <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> objects from which to choose.</param>
    ContentNegotiationResult Negotiate(
      Type type,
      HttpRequestMessage request,
      IEnumerable<MediaTypeFormatter> formatters);
  }
}
