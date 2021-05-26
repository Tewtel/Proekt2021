// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.AuthorizerEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>The data associated with a call into the authorizer.</summary>
  public class AuthorizerEventArgs : EventArgs
  {
    /// <summary>
    /// The user-defined native data associated with this event.  Currently,
    /// this will always contain the value of <see cref="F:System.IntPtr.Zero" />.
    /// </summary>
    public readonly IntPtr UserData;
    /// <summary>
    /// The action code responsible for the current call into the authorizer.
    /// </summary>
    public readonly SQLiteAuthorizerActionCode ActionCode;
    /// <summary>
    /// The first string argument for the current call into the authorizer.
    /// The exact value will vary based on the action code, see the
    /// <see cref="T:System.Data.SQLite.SQLiteAuthorizerActionCode" /> enumeration for possible
    /// values.
    /// </summary>
    public readonly string Argument1;
    /// <summary>
    /// The second string argument for the current call into the authorizer.
    /// The exact value will vary based on the action code, see the
    /// <see cref="T:System.Data.SQLite.SQLiteAuthorizerActionCode" /> enumeration for possible
    /// values.
    /// </summary>
    public readonly string Argument2;
    /// <summary>
    /// The database name for the current call into the authorizer, if
    /// applicable.
    /// </summary>
    public readonly string Database;
    /// <summary>
    /// The name of the inner-most trigger or view that is responsible for
    /// the access attempt or a null value if this access attempt is directly
    /// from top-level SQL code.
    /// </summary>
    public readonly string Context;
    /// <summary>
    /// The return code for the current call into the authorizer.
    /// </summary>
    public SQLiteAuthorizerReturnCode ReturnCode;

    /// <summary>
    /// Constructs an instance of this class with default property values.
    /// </summary>
    private AuthorizerEventArgs()
    {
      this.UserData = IntPtr.Zero;
      this.ActionCode = SQLiteAuthorizerActionCode.None;
      this.Argument1 = (string) null;
      this.Argument2 = (string) null;
      this.Database = (string) null;
      this.Context = (string) null;
      this.ReturnCode = SQLiteAuthorizerReturnCode.Ok;
    }

    /// <summary>
    /// Constructs an instance of this class with specific property values.
    /// </summary>
    /// <param name="pUserData">
    /// The user-defined native data associated with this event.
    /// </param>
    /// <param name="actionCode">The authorizer action code.</param>
    /// <param name="argument1">The first authorizer argument.</param>
    /// <param name="argument2">The second authorizer argument.</param>
    /// <param name="database">The database name, if applicable.</param>
    /// <param name="context">
    /// The name of the inner-most trigger or view that is responsible for
    /// the access attempt or a null value if this access attempt is directly
    /// from top-level SQL code.
    /// </param>
    /// <param name="returnCode">The authorizer return code.</param>
    internal AuthorizerEventArgs(
      IntPtr pUserData,
      SQLiteAuthorizerActionCode actionCode,
      string argument1,
      string argument2,
      string database,
      string context,
      SQLiteAuthorizerReturnCode returnCode)
      : this()
    {
      this.UserData = pUserData;
      this.ActionCode = actionCode;
      this.Argument1 = argument1;
      this.Argument2 = argument2;
      this.Database = database;
      this.Context = context;
      this.ReturnCode = returnCode;
    }
  }
}
