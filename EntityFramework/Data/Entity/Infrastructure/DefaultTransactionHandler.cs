// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DefaultTransactionHandler
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Infrastructure
{
  internal class DefaultTransactionHandler : TransactionHandler
  {
    public override string BuildDatabaseInitializationScript() => string.Empty;

    public override void Committed(
      DbTransaction transaction,
      DbTransactionInterceptionContext interceptionContext)
    {
      if (interceptionContext.Exception == null || interceptionContext.Connection == null || !this.MatchesParentContext(interceptionContext.Connection, (DbInterceptionContext) interceptionContext))
        return;
      interceptionContext.Exception = (Exception) new CommitFailedException(Strings.CommitFailed, interceptionContext.Exception);
    }
  }
}
