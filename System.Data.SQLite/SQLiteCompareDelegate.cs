// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteCompareDelegate
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This <see cref="T:System.Delegate" /> type is used with the
  /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Compare(System.String,System.String)" /> method.
  /// </summary>
  /// <param name="param0">
  /// This is always the string literal "Compare".
  /// </param>
  /// <param name="param1">The first string to compare.</param>
  /// <param name="param2">The second strnig to compare.</param>
  /// <returns>
  /// A positive integer if the <paramref name="param1" /> parameter is
  /// greater than the <paramref name="param2" /> parameter, a negative
  /// integer if the <paramref name="param1" /> parameter is less than
  /// the <paramref name="param2" /> parameter, or zero if they are
  /// equal.
  /// </returns>
  public delegate int SQLiteCompareDelegate(string param0, string param1, string param2);
}
