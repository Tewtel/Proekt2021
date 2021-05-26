// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFinalDelegate
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This <see cref="T:System.Delegate" /> type is used with the
  /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Final(System.Object)" /> method.
  /// </summary>
  /// <param name="param0">This is always the string literal "Final".</param>
  /// <param name="contextData">
  /// A placeholder for implementers to store contextual data pertaining
  /// to the current context.
  /// </param>
  /// <returns>The result of the aggregate function.</returns>
  public delegate object SQLiteFinalDelegate(string param0, object contextData);
}
