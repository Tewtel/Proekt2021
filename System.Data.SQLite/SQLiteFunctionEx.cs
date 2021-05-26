// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFunctionEx
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Extends SQLiteFunction and allows an inherited class to obtain the collating sequence associated with a function call.
  /// </summary>
  /// <remarks>
  /// User-defined functions can call the GetCollationSequence() method in this class and use it to compare strings and char arrays.
  /// </remarks>
  public class SQLiteFunctionEx : SQLiteFunction
  {
    private bool disposed;

    /// <summary>
    /// Obtains the collating sequence in effect for the given function.
    /// </summary>
    /// <returns></returns>
    protected CollationSequence GetCollationSequence() => this._base.GetCollationSequence((SQLiteFunction) this, this._context);

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteFunctionEx).Name);
    }

    /// <summary>
    /// Cleans up resources (native and managed) associated with the current instance.
    /// </summary>
    /// <param name="disposing">
    /// Zero when being disposed via garbage collection; otherwise, non-zero.
    /// </param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        int num = this.disposed ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
