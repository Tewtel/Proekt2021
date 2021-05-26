// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.CollationSequence
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// A struct describing the collating sequence a function is executing in
  /// </summary>
  public struct CollationSequence
  {
    /// <summary>The name of the collating sequence</summary>
    public string Name;
    /// <summary>The type of collating sequence</summary>
    public CollationTypeEnum Type;
    /// <summary>The text encoding of the collation sequence</summary>
    public CollationEncodingEnum Encoding;
    /// <summary>
    /// Context of the function that requested the collating sequence
    /// </summary>
    internal SQLiteFunction _func;

    /// <summary>
    /// Calls the base collating sequence to compare two strings
    /// </summary>
    /// <param name="s1">The first string to compare</param>
    /// <param name="s2">The second string to compare</param>
    /// <returns>-1 if s1 is less than s2, 0 if s1 is equal to s2, and 1 if s1 is greater than s2</returns>
    public int Compare(string s1, string s2) => this._func._base.ContextCollateCompare(this.Encoding, this._func._context, s1, s2);

    /// <summary>
    /// Calls the base collating sequence to compare two character arrays
    /// </summary>
    /// <param name="c1">The first array to compare</param>
    /// <param name="c2">The second array to compare</param>
    /// <returns>-1 if c1 is less than c2, 0 if c1 is equal to c2, and 1 if c1 is greater than c2</returns>
    public int Compare(char[] c1, char[] c2) => this._func._base.ContextCollateCompare(this.Encoding, this._func._context, c1, c2);
  }
}
