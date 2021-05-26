// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteInvokeDelegate
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This <see cref="T:System.Delegate" /> type is used with the
  /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Invoke(System.Object[])" /> method.
  /// </summary>
  /// <param name="param0">This is always the string literal "Invoke".</param>
  /// <param name="args">The arguments for the scalar function.</param>
  /// <returns>The result of the scalar function.</returns>
  public delegate object SQLiteInvokeDelegate(string param0, object[] args);
}
