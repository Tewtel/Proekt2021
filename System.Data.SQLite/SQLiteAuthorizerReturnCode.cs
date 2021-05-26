// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteAuthorizerReturnCode
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// The return code for the current call into the authorizer.
  /// </summary>
  public enum SQLiteAuthorizerReturnCode
  {
    /// <summary>The action will be allowed.</summary>
    Ok,
    /// <summary>
    /// The overall action will be disallowed and an error message will be
    /// returned from the query preparation method.
    /// </summary>
    Deny,
    /// <summary>
    /// The specific action will be disallowed; however, the overall action
    /// will continue.  The exact effects of this return code vary depending
    /// on the specific action, please refer to the SQLite core library
    /// documentation for futher details.
    /// </summary>
    Ignore,
  }
}
