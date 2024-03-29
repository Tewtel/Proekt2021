﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EdmSchemaError
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;
using System.Globalization;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// This class encapsulates the error information for a schema error that was encountered.
  /// </summary>
  [Serializable]
  public sealed class EdmSchemaError : EdmError
  {
    private int _errorCode;
    private EdmSchemaErrorSeverity _severity;
    private string _schemaLocation;
    private int _line = -1;
    private int _column = -1;
    private string _stackTrace = string.Empty;

    /// <summary>Constructs a EdmSchemaError object.</summary>
    /// <param name="message"> The explanation of the error. </param>
    /// <param name="errorCode"> The code associated with this error. </param>
    /// <param name="severity"> The severity of the error. </param>
    public EdmSchemaError(string message, int errorCode, EdmSchemaErrorSeverity severity)
      : this(message, errorCode, severity, (Exception) null)
    {
    }

    internal EdmSchemaError(
      string message,
      int errorCode,
      EdmSchemaErrorSeverity severity,
      Exception exception)
      : base(message)
    {
      this.Initialize(errorCode, severity, (string) null, -1, -1, exception);
    }

    internal EdmSchemaError(
      string message,
      int errorCode,
      EdmSchemaErrorSeverity severity,
      string schemaLocation,
      int line,
      int column)
      : this(message, errorCode, severity, schemaLocation, line, column, (Exception) null)
    {
    }

    internal EdmSchemaError(
      string message,
      int errorCode,
      EdmSchemaErrorSeverity severity,
      string schemaLocation,
      int line,
      int column,
      Exception exception)
      : base(message)
    {
      if (severity < EdmSchemaErrorSeverity.Warning || severity > EdmSchemaErrorSeverity.Error)
        throw new ArgumentOutOfRangeException(nameof (severity), (object) severity, Strings.ArgumentOutOfRange((object) severity));
      this.Initialize(errorCode, severity, schemaLocation, line, column, exception);
    }

    private void Initialize(
      int errorCode,
      EdmSchemaErrorSeverity severity,
      string schemaLocation,
      int line,
      int column,
      Exception exception)
    {
      this._errorCode = errorCode >= 0 ? errorCode : throw new ArgumentOutOfRangeException(nameof (errorCode), (object) errorCode, Strings.ArgumentOutOfRangeExpectedPostiveNumber((object) errorCode));
      this._severity = severity;
      this._schemaLocation = schemaLocation;
      this._line = line;
      this._column = column;
      if (exception == null)
        return;
      this._stackTrace = exception.StackTrace;
    }

    /// <summary>Returns the error message.</summary>
    /// <returns>The error message.</returns>
    public override string ToString()
    {
      string str1;
      switch (this.Severity)
      {
        case EdmSchemaErrorSeverity.Warning:
          str1 = Strings.GeneratorErrorSeverityWarning;
          break;
        case EdmSchemaErrorSeverity.Error:
          str1 = Strings.GeneratorErrorSeverityError;
          break;
        default:
          str1 = Strings.GeneratorErrorSeverityUnknown;
          break;
      }
      string str2;
      if (string.IsNullOrEmpty(this.SchemaName) && this.Line < 0 && this.Column < 0)
        str2 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} {1:0000}: {2}", (object) str1, (object) this.ErrorCode, (object) this.Message);
      else
        str2 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}({1},{2}) : {3} {4:0000}: {5}", this.SchemaName == null ? (object) Strings.SourceUriUnknown : (object) this.SchemaName, (object) this.Line, (object) this.Column, (object) str1, (object) this.ErrorCode, (object) this.Message);
      return str2;
    }

    /// <summary>Gets the error code.</summary>
    /// <returns>The error code.</returns>
    public int ErrorCode => this._errorCode;

    /// <summary>Gets the severity level of the error.</summary>
    /// <returns>
    /// One of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmSchemaErrorSeverity" /> values. The default is
    /// <see cref="F:System.Data.Entity.Core.Metadata.Edm.EdmSchemaErrorSeverity.Warning" />
    /// .
    /// </returns>
    public EdmSchemaErrorSeverity Severity
    {
      get => this._severity;
      set => this._severity = value;
    }

    /// <summary>Gets the line number where the error occurred.</summary>
    /// <returns>The line number where the error occurred.</returns>
    public int Line => this._line;

    /// <summary>Gets the column where the error occurred.</summary>
    /// <returns>The column where the error occurred.</returns>
    public int Column => this._column;

    /// <summary>Gets the location of the schema that contains the error. This string also includes the name of the schema at the end.</summary>
    /// <returns>The location of the schema that contains the error.</returns>
    public string SchemaLocation => this._schemaLocation;

    /// <summary>Gets the name of the schema that contains the error.</summary>
    /// <returns>The name of the schema that contains the error.</returns>
    public string SchemaName => EdmSchemaError.GetNameFromSchemaLocation(this.SchemaLocation);

    /// <summary>Gets a string representation of the stack trace at the time the error occurred.</summary>
    /// <returns>A string representation of the stack trace at the time the error occurred.</returns>
    public string StackTrace => this._stackTrace;

    private static string GetNameFromSchemaLocation(string schemaLocation)
    {
      if (string.IsNullOrEmpty(schemaLocation))
        return schemaLocation;
      int num = Math.Max(schemaLocation.LastIndexOf('/'), schemaLocation.LastIndexOf('\\'));
      int startIndex = num + 1;
      if (num < 0)
        return schemaLocation;
      return startIndex >= schemaLocation.Length ? string.Empty : schemaLocation.Substring(startIndex);
    }
  }
}
