﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.DropCreateDatabaseAlways`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Internal;

namespace System.Data.Entity
{
  /// <summary>
  /// An implementation of IDatabaseInitializer that will always recreate and optionally re-seed the
  /// database the first time that a context is used in the app domain.
  /// To seed the database, create a derived class and override the Seed method.
  /// </summary>
  /// <typeparam name="TContext"> The type of the context. </typeparam>
  public class DropCreateDatabaseAlways<TContext> : IDatabaseInitializer<TContext>
    where TContext : DbContext
  {
    static DropCreateDatabaseAlways() => DbConfigurationManager.Instance.EnsureLoadedForContext(typeof (TContext));

    /// <summary>
    /// Executes the strategy to initialize the database for the given context.
    /// </summary>
    /// <param name="context"> The context. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="context" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    public virtual void InitializeDatabase(TContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<TContext>(context, nameof (context));
      context.Database.Delete();
      context.Database.Create(DatabaseExistenceState.DoesNotExist);
      this.Seed(context);
      context.SaveChanges();
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
