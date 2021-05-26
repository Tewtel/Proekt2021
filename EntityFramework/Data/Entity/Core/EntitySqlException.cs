// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntitySqlException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.EntitySql;
using System.Data.Entity.Resources;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace System.Data.Entity.Core
{
  /// <summary>
  /// Represents an eSQL Query compilation exception;
  /// The class of exceptional conditions that may cause this exception to be raised are mainly:
  /// 1) Syntax Errors: raised during query text parsing and when a given query does not conform to eSQL formal grammar;
  /// 2) Semantic Errors: raised when semantic rules of eSQL language are not met such as metadata or schema information
  /// not accurate or not present, type validation errors, scoping rule violations, user of undefined variables, etc.
  /// For more information, see eSQL Language Spec.
  /// </summary>
  [Serializable]
  public sealed class EntitySqlException : EntityException
  {
    private const int HResultInvalidQuery = -2146232006;
    private readonly string _errorDescription;
    private readonly string _errorContext;

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.EntitySqlException" />.
    /// </summary>
    public EntitySqlException()
      : this(Strings.GeneralQueryError)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.EntitySqlException" /> with a specialized error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntitySqlException(string message)
      : base(message)
    {
      this.HResult = -2146232006;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.EntitySqlException" /> class that uses a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that caused the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public EntitySqlException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.HResult = -2146232006;
    }

    private EntitySqlException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._errorDescription = info.GetString(nameof (ErrorDescription));
      this._errorContext = info.GetString(nameof (ErrorContext));
      this.Line = info.GetInt32(nameof (Line));
      this.Column = info.GetInt32(nameof (Column));
    }

    internal static EntitySqlException Create(
      System.Data.Entity.Core.Common.EntitySql.ErrorContext errCtx,
      string errorMessage,
      Exception innerException)
    {
      return EntitySqlException.Create(errCtx.CommandText, errorMessage, errCtx.InputPosition, errCtx.ErrorContextInfo, errCtx.UseContextInfoAsResourceIdentifier, innerException);
    }

    internal static EntitySqlException Create(
      string commandText,
      string errorDescription,
      int errorPosition,
      string errorContextInfo,
      bool loadErrorContextInfoFromResource,
      Exception innerException)
    {
      int lineNumber;
      int columnNumber;
      string errorContext = EntitySqlException.FormatErrorContext(commandText, errorPosition, errorContextInfo, loadErrorContextInfoFromResource, out lineNumber, out columnNumber);
      return new EntitySqlException(EntitySqlException.FormatQueryError(errorDescription, errorContext), errorDescription, errorContext, lineNumber, columnNumber, innerException);
    }

    private EntitySqlException(
      string message,
      string errorDescription,
      string errorContext,
      int line,
      int column,
      Exception innerException)
      : base(message, innerException)
    {
      this._errorDescription = errorDescription;
      this._errorContext = errorContext;
      this.Line = line;
      this.Column = column;
      this.HResult = -2146232006;
    }

    /// <summary>Gets a description of the error.</summary>
    /// <returns>A string that describes the error.</returns>
    public string ErrorDescription => this._errorDescription ?? string.Empty;

    /// <summary>Gets the approximate context where the error occurred, if available.</summary>
    /// <returns>A string that describes the approximate context where the error occurred, if available.</returns>
    public string ErrorContext => this._errorContext ?? string.Empty;

    /// <summary>Gets the approximate line number where the error occurred.</summary>
    /// <returns>An integer that describes the line number where the error occurred.</returns>
    public int Line { get; }

    /// <summary>Gets the approximate column number where the error occurred.</summary>
    /// <returns>An integer that describes the column number where the error occurred.</returns>
    public int Column { get; }

    internal static string GetGenericErrorMessage(string commandText, int position)
    {
      int lineNumber = 0;
      int columnNumber = 0;
      return EntitySqlException.FormatErrorContext(commandText, position, "GenericSyntaxError", true, out lineNumber, out columnNumber);
    }

    internal static string FormatErrorContext(
      string commandText,
      int errorPosition,
      string errorContextInfo,
      bool loadErrorContextInfoFromResource,
      out int lineNumber,
      out int columnNumber)
    {
      if (loadErrorContextInfoFromResource)
        errorContextInfo = !string.IsNullOrEmpty(errorContextInfo) ? EntityRes.GetString(errorContextInfo) : string.Empty;
      StringBuilder stringBuilder1 = new StringBuilder(commandText.Length);
      for (int index = 0; index < commandText.Length; ++index)
      {
        char c = commandText[index];
        if (CqlLexer.IsNewLine(c))
          c = '\n';
        else if ((char.IsControl(c) || char.IsWhiteSpace(c)) && '\r' != c)
          c = ' ';
        stringBuilder1.Append(c);
      }
      commandText = stringBuilder1.ToString().TrimEnd('\n');
      string[] strArray = commandText.Split(new char[1]
      {
        '\n'
      }, StringSplitOptions.None);
      lineNumber = 0;
      columnNumber = errorPosition;
      while (lineNumber < strArray.Length && columnNumber > strArray[lineNumber].Length)
      {
        columnNumber -= strArray[lineNumber].Length + 1;
        ++lineNumber;
      }
      ++lineNumber;
      ++columnNumber;
      StringBuilder stringBuilder2 = new StringBuilder();
      if (!string.IsNullOrEmpty(errorContextInfo))
        stringBuilder2.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, "{0}, ", (object) errorContextInfo);
      if (errorPosition >= 0)
        stringBuilder2.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, "{0} {1}, {2} {3}", (object) Strings.LocalizedLine, (object) lineNumber, (object) Strings.LocalizedColumn, (object) columnNumber);
      return stringBuilder2.ToString();
    }

    private static string FormatQueryError(string errorMessage, string errorContext)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(errorMessage);
      if (!string.IsNullOrEmpty(errorContext))
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, " {0} {1}", (object) Strings.LocalizedNear, (object) errorContext);
      return stringBuilder.Append(".").ToString();
    }

    /// <summary>
    /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
    /// </summary>
    /// <param name="info"> The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
    /// <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("ErrorDescription", (object) this._errorDescription);
      info.AddValue("ErrorContext", (object) this._errorContext);
      info.AddValue("Line", this.Line);
      info.AddValue("Column", this.Column);
    }
  }
}
