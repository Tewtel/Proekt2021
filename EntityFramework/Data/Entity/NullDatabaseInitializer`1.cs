// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.NullDatabaseInitializer`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity
{
  /// <summary>
  /// An implementation of <see cref="T:System.Data.Entity.IDatabaseInitializer`1" /> that does nothing. Using this
  /// initializer disables database initialization for the given context type. Passing an instance
  /// of this class to <see cref="M:System.Data.Entity.Database.SetInitializer``1(System.Data.Entity.IDatabaseInitializer{``0})" /> is equivalent to passing null.
  /// When <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" /> is being used to resolve initializers an instance of
  /// this class must be used to disable initialization.
  /// </summary>
  /// <typeparam name="TContext">The type of the context.</typeparam>
  public class NullDatabaseInitializer<TContext> : IDatabaseInitializer<TContext>
    where TContext : DbContext
  {
    /// <inheritdoc />
    public virtual void InitializeDatabase(TContext context) => System.Data.Entity.Utilities.Check.NotNull<TContext>(context, nameof (context));
  }
}
