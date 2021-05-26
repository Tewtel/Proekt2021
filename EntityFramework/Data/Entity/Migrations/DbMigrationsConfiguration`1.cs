// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.DbMigrationsConfiguration`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Migrations
{
  /// <summary>
  /// Configuration relating to the use of migrations for a given model.
  /// </summary>
  /// <typeparam name="TContext"> The context representing the model that this configuration applies to. </typeparam>
  public class DbMigrationsConfiguration<TContext> : DbMigrationsConfiguration
    where TContext : DbContext
  {
    static DbMigrationsConfiguration() => DbConfigurationManager.Instance.EnsureLoadedForContext(typeof (TContext));

    /// <summary>
    /// Initializes a new instance of the DbMigrationsConfiguration class.
    /// </summary>
    public DbMigrationsConfiguration()
    {
      this.ContextType = typeof (TContext);
      this.MigrationsAssembly = this.GetType().Assembly();
      this.MigrationsNamespace = this.GetType().Namespace;
    }

    /// <summary>
    /// Runs after upgrading to the latest migration to allow seed data to be updated.
    /// </summary>
    /// <remarks>
    /// Note that the database may already contain seed data when this method runs. This means that
    /// implementations of this method must check whether or not seed data is present and/or up-to-date
    /// and then only make changes if necessary and in a non-destructive way. The
    /// <see cref="M:System.Data.Entity.Migrations.DbSetMigrationsExtensions.AddOrUpdate``1(System.Data.Entity.IDbSet{``0},``0[])" />
    /// can be used to help with this, but for seeding large amounts of data it may be necessary to do less
    /// granular checks if performance is an issue.
    /// If the <see cref="T:System.Data.Entity.MigrateDatabaseToLatestVersion`2" /> database
    /// initializer is being used, then this method will be called each time that the initializer runs.
    /// If one of the <see cref="T:System.Data.Entity.DropCreateDatabaseAlways`1" />, <see cref="T:System.Data.Entity.DropCreateDatabaseIfModelChanges`1" />,
    /// or <see cref="T:System.Data.Entity.CreateDatabaseIfNotExists`1" /> initializers is being used, then this method will not be
    /// called and the Seed method defined in the initializer should be used instead.
    /// </remarks>
    /// <param name="context"> Context to be used for updating seed data. </param>
    protected virtual void Seed(TContext context) => System.Data.Entity.Utilities.Check.NotNull<TContext>(context, nameof (context));

    internal override void OnSeed(DbContext context) => this.Seed((TContext) context);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected new object MemberwiseClone() => base.MemberwiseClone();
  }
}
