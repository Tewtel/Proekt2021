// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.CreateDatabaseIfNotExists`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Internal;
using System.Data.Entity.Resources;

namespace System.Data.Entity
{
  /// <summary>
  /// An implementation of IDatabaseInitializer that will recreate and optionally re-seed the
  /// database only if the database does not exist.
  /// To seed the database, create a derived class and override the Seed method.
  /// </summary>
  /// <typeparam name="TContext"> The type of the context. </typeparam>
  public class CreateDatabaseIfNotExists<TContext> : IDatabaseInitializer<TContext>
    where TContext : DbContext
  {
    static CreateDatabaseIfNotExists() => DbConfigurationManager.Instance.EnsureLoadedForContext(typeof (TContext));

    /// <summary>
    /// Executes the strategy to initialize the database for the given context.
    /// </summary>
    /// <param name="context"> The context. </param>
    public virtual void InitializeDatabase(TContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<TContext>(context, nameof (context));
      DatabaseExistenceState existenceState = new DatabaseTableChecker().AnyModelTableExists(context.InternalContext);
      if (existenceState == DatabaseExistenceState.Exists)
      {
        if (!context.Database.CompatibleWithModel(false, existenceState))
          throw Error.DatabaseInitializationStrategy_ModelMismatch((object) context.GetType().Name);
      }
      else
      {
        context.Database.Create(existenceState);
        this.Seed(context);
        context.SaveChanges();
      }
    }

    /// <summary>
    /// A method that should be overridden to actually add data to the context for seeding.
    /// The default implementation does nothing.
    /// </summary>
    /// <param name="context"> The context to seed. </param>
    protected virtual void Seed(TContext context)
    {
    }
  }
}
