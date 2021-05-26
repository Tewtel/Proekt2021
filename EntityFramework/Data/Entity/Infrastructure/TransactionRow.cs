// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.TransactionRow
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure
{
  /// <summary>Rrepresents a transaction</summary>
  public class TransactionRow
  {
    /// <summary>A unique id assigned to a transaction object.</summary>
    public Guid Id { get; set; }

    /// <summary>The local time when the transaction was started.</summary>
    public DateTime CreationTime { get; set; }

    /// <inheritdoc />
    public override bool Equals(object obj) => obj is TransactionRow transactionRow && this.Id == transactionRow.Id;

    /// <inheritdoc />
    public override int GetHashCode() => this.Id.GetHashCode();
  }
}
