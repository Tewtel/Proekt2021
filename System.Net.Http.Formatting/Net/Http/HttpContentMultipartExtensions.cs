﻿// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpContentMultipartExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http.Formatting.Parsers;
using System.Net.Http.Properties;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Extension methods to read MIME multipart entities from <see cref="T:System.Net.Http.HttpContent" /> instances.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HttpContentMultipartExtensions
  {
    private const int MinBufferSize = 256;
    private const int DefaultBufferSize = 32768;

    /// <summary>Determines whether the specified content is MIME multipart content.</summary>
    /// <returns>true if the specified content is MIME multipart content; otherwise, false.</returns>
    /// <param name="content">The content.</param>
    public static bool IsMimeMultipartContent(this HttpContent content) => content != null ? MimeMultipartBodyPartParser.IsMimeMultipartContent(content) : throw Error.ArgumentNull(nameof (content));

    /// <summary>Determines whether the specified content is MIME multipart content with the specified subtype.</summary>
    /// <returns>true if the specified content is MIME multipart content with the specified subtype; otherwise, false.</returns>
    /// <param name="content">The content.</param>
    /// <param name="subtype">The MIME multipart subtype to match.</param>
    public static bool IsMimeMultipartContent(this HttpContent content, string subtype)
    {
      if (string.IsNullOrWhiteSpace(subtype))
        throw Error.ArgumentNull(nameof (subtype));
      return content.IsMimeMultipartContent() && content.Headers.ContentType.MediaType.Equals("multipart/" + subtype, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Reads all body parts within a MIME multipart message and produces a set of <see cref="T:System.Net.Http.HttpContent" /> instances as a result.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the tasks of getting the collection of <see cref="T:System.Net.Http.HttpContent" /> instances where each instance represents a body part.</returns>
    /// <param name="content">An existing <see cref="T:System.Net.Http.HttpContent" /> instance to use for the object's content.</param>
    public static Task<MultipartMemoryStreamProvider> ReadAsMultipartAsync(
      this HttpContent content)
    {
      return content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider(), 32768);
    }

    /// <summary>Reads all body parts within a MIME multipart message and produces a set of <see cref="T:System.Net.Http.HttpContent" /> instances as a result.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the tasks of getting the collection of <see cref="T:System.Net.Http.HttpContent" /> instances where each instance represents a body part.</returns>
    /// <param name="content">An existing <see cref="T:System.Net.Http.HttpContent" /> instance to use for the object's content.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public static Task<MultipartMemoryStreamProvider> ReadAsMultipartAsync(
      this HttpContent content,
      CancellationToken cancellationToken)
    {
      return content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider(), 32768, cancellationToken);
    }

    /// <summary>Reads all body parts within a MIME multipart message and produces a set of <see cref="T:System.Net.Http.HttpContent" /> instances as a result using the streamProvider instance to determine where the contents of each body part is written.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the tasks of getting the collection of <see cref="T:System.Net.Http.HttpContent" /> instances where each instance represents a body part.</returns>
    /// <param name="content">An existing <see cref="T:System.Net.Http.HttpContent" /> instance to use for the object's content.</param>
    /// <param name="streamProvider">A stream provider providing output streams for where to write body parts as they are parsed.</param>
    /// <typeparam name="T">The type of the MIME multipart.</typeparam>
    public static Task<T> ReadAsMultipartAsync<T>(this HttpContent content, T streamProvider) where T : MultipartStreamProvider => content.ReadAsMultipartAsync<T>(streamProvider, 32768);

    /// <summary>Reads all body parts within a MIME multipart message and produces a set of <see cref="T:System.Net.Http.HttpContent" /> instances as a result using the streamProvider instance to determine where the contents of each body part is written.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the tasks of getting the collection of <see cref="T:System.Net.Http.HttpContent" /> instances where each instance represents a body part.</returns>
    /// <param name="content">An existing <see cref="T:System.Net.Http.HttpContent" /> instance to use for the object's content.</param>
    /// <param name="streamProvider">A stream provider providing output streams for where to write body parts as they are parsed.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    /// <typeparam name="T">The type of the MIME multipart.</typeparam>
    public static Task<T> ReadAsMultipartAsync<T>(
      this HttpContent content,
      T streamProvider,
      CancellationToken cancellationToken)
      where T : MultipartStreamProvider
    {
      return content.ReadAsMultipartAsync<T>(streamProvider, 32768, cancellationToken);
    }

    /// <summary>Reads all body parts within a MIME multipart message and produces a set of <see cref="T:System.Net.Http.HttpContent" /> instances as a result using the streamProvider instance to determine where the contents of each body part is written and bufferSize as read buffer size.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the tasks of getting the collection of <see cref="T:System.Net.Http.HttpContent" /> instances where each instance represents a body part.</returns>
    /// <param name="content">An existing <see cref="T:System.Net.Http.HttpContent" /> instance to use for the object's content.</param>
    /// <param name="streamProvider">A stream provider providing output streams for where to write body parts as they are parsed.</param>
    /// <param name="bufferSize">Size of the buffer used to read the contents.</param>
    /// <typeparam name="T">The type of the MIME multipart.</typeparam>
    public static Task<T> ReadAsMultipartAsync<T>(
      this HttpContent content,
      T streamProvider,
      int bufferSize)
      where T : MultipartStreamProvider
    {
      return content.ReadAsMultipartAsync<T>(streamProvider, bufferSize, CancellationToken.None);
    }

    /// <summary>Reads all body parts within a MIME multipart message and produces a set of <see cref="T:System.Net.Http.HttpContent" /> instances as a result using the streamProvider instance to determine where the contents of each body part is written and bufferSize as read buffer size.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the tasks of getting the collection of <see cref="T:System.Net.Http.HttpContent" /> instances where each instance represents a body part.</returns>
    /// <param name="content">An existing <see cref="T:System.Net.Http.HttpContent" /> instance to use for the object's content.</param>
    /// <param name="streamProvider">A stream provider providing output streams for where to write body parts as they are parsed.</param>
    /// <param name="bufferSize">Size of the buffer used to read the contents.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    /// <typeparam name="T">The type of the MIME multipart.</typeparam>
    public static async Task<T> ReadAsMultipartAsync<T>(
      this HttpContent content,
      T streamProvider,
      int bufferSize,
      CancellationToken cancellationToken)
      where T : MultipartStreamProvider
    {
      if (content == null)
        throw Error.ArgumentNull(nameof (content));
      if ((object) (T) streamProvider == null)
        throw Error.ArgumentNull(nameof (streamProvider));
      if (bufferSize < 256)
        throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (bufferSize), (object) bufferSize, (object) 256);
      Stream contentStream;
      try
      {
        contentStream = await content.ReadAsStreamAsync();
      }
      catch (Exception ex)
      {
        throw new IOException(Resources.ReadAsMimeMultipartErrorReading, ex);
      }
      T obj;
      using (MimeMultipartBodyPartParser parser = new MimeMultipartBodyPartParser(content, (MultipartStreamProvider) streamProvider))
      {
        byte[] data = new byte[bufferSize];
        await HttpContentMultipartExtensions.MultipartReadAsync(new HttpContentMultipartExtensions.MultipartAsyncContext(contentStream, parser, data, (ICollection<HttpContent>) ((T) streamProvider).Contents), cancellationToken);
        await ((T) streamProvider).ExecutePostProcessingAsync(cancellationToken);
        obj = streamProvider;
      }
      return obj;
    }

    private static async Task MultipartReadAsync(
      HttpContentMultipartExtensions.MultipartAsyncContext context,
      CancellationToken cancellationToken)
    {
      while (true)
      {
        int bytesRead;
        try
        {
          bytesRead = await context.ContentStream.ReadAsync(context.Data, 0, context.Data.Length, cancellationToken);
        }
        catch (Exception ex)
        {
          throw new IOException(Resources.ReadAsMimeMultipartErrorReading, ex);
        }
        using (IEnumerator<MimeBodyPart> enumerator = context.MimeParser.ParseBuffer(context.Data, bytesRead).GetEnumerator())
        {
          while (true)
          {
            if (enumerator.MoveNext())
            {
              MimeBodyPart part = enumerator.Current;
              foreach (ArraySegment<byte> segment in part.Segments)
              {
                try
                {
                  await part.WriteSegment(segment, cancellationToken);
                }
                catch (Exception ex)
                {
                  part.Dispose();
                  throw new IOException(Resources.ReadAsMimeMultipartErrorWriting, ex);
                }
              }
              if (!HttpContentMultipartExtensions.CheckIsFinalPart(part, context.Result))
                part = (MimeBodyPart) null;
              else
                goto label_22;
            }
            else
              break;
          }
        }
      }
label_22:;
    }

    private static bool CheckIsFinalPart(MimeBodyPart part, ICollection<HttpContent> result)
    {
      if (!part.IsComplete)
        return false;
      HttpContent completedHttpContent = part.GetCompletedHttpContent();
      if (completedHttpContent != null)
        result.Add(completedHttpContent);
      int num = part.IsFinal ? 1 : 0;
      part.Dispose();
      return num != 0;
    }

    private class MultipartAsyncContext
    {
      public MultipartAsyncContext(
        Stream contentStream,
        MimeMultipartBodyPartParser mimeParser,
        byte[] data,
        ICollection<HttpContent> result)
      {
        this.ContentStream = contentStream;
        this.Result = result;
        this.MimeParser = mimeParser;
        this.Data = data;
      }

      public Stream ContentStream { get; private set; }

      public ICollection<HttpContent> Result { get; private set; }

      public byte[] Data { get; private set; }

      public MimeMultipartBodyPartParser MimeParser { get; private set; }
    }
  }
}
