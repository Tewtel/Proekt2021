// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.CommitEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Event arguments raised when a transaction is being committed
  /// </summary>
  public class CommitEventArgs : EventArgs
  {
    /// <summary>
    /// Set to true to abort the transaction and trigger a rollback
    /// </summary>
    public bool AbortTransaction;

    internal CommitEventArgs()
    {
    }
  }
}
