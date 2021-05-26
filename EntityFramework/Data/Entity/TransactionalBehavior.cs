// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.TransactionalBehavior
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity
{
  /// <summary>
  /// Controls the transaction creation behavior while executing a database command or query.
  /// </summary>
  public enum TransactionalBehavior
  {
    /// <summary>
    /// If no transaction is present then a new transaction will be used for the operation.
    /// </summary>
    EnsureTransaction,
    /// <summary>
    /// If an existing transaction is present then use it, otherwise execute the command or query without a transaction.
    /// </summary>
    DoNotEnsureTransaction,
  }
}
