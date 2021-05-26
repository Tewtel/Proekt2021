// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Infrastructure.MigrationAssembly
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Migrations.Infrastructure
{
  internal class MigrationAssembly
  {
    private readonly IList<IMigrationMetadata> _migrations;

    public static string CreateMigrationId(string migrationName) => UtcNowGenerator.UtcNowAsMigrationIdTimestamp() + "_" + migrationName;

    public static string CreateBootstrapMigrationId() => new string('0', 15) + "_" + System.Data.Entity.Resources.Strings.BootstrapMigration;

    protected MigrationAssembly()
    {
    }

    public MigrationAssembly(Assembly migrationsAssembly, string migrationsNamespace) => this._migrations = (IList<IMigrationMetadata>) migrationsAssembly.GetAccessibleTypes().Where<Type>((Func<Type, bool>) (t => t.IsSubclassOf(typeof (DbMigration)) && typeof (IMigrationMetadata).IsAssignableFrom(t) && (t.GetPublicConstructor() != (ConstructorInfo) null && !t.IsAbstract()) && !t.IsGenericType() && t.Namespace == migrationsNamespace)).Select<Type, IMigrationMetadata>((Func<Type, IMigrationMetadata>) (t => (IMigrationMetadata) Activator.CreateInstance(t))).Where<IMigrationMetadata>((Func<IMigrationMetadata, bool>) (mm => !string.IsNullOrWhiteSpace(mm.Id) && mm.Id.IsValidMigrationId())).OrderBy<IMigrationMetadata, string>((Func<IMigrationMetadata, string>) (mm => mm.Id)).ToList<IMigrationMetadata>();

    public virtual IEnumerable<string> MigrationIds => (IEnumerable<string>) this._migrations.Select<IMigrationMetadata, string>((Func<IMigrationMetadata, string>) (t => t.Id)).ToList<string>();

    public virtual string UniquifyName(string migrationName) => this._migrations.Select<IMigrationMetadata, string>((Func<IMigrationMetadata, string>) (m => m.GetType().Name)).Uniquify(migrationName);

    public virtual DbMigration GetMigration(string migrationId)
    {
      DbMigration dbMigration = (DbMigration) this._migrations.SingleOrDefault<IMigrationMetadata>((Func<IMigrationMetadata, bool>) (m => m.Id.StartsWith(migrationId, StringComparison.Ordinal)));
      dbMigration?.Reset();
      return dbMigration;
    }
  }
}
