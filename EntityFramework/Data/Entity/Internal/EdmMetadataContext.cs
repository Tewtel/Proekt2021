// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.EdmMetadataContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal
{
  internal class EdmMetadataContext : DbContext
  {
    public const string TableName = "EdmMetadata";

    static EdmMetadataContext() => Database.SetInitializer<EdmMetadataContext>((IDatabaseInitializer<EdmMetadataContext>) null);

    public EdmMetadataContext(DbConnection existingConnection)
      : base(existingConnection, false)
    {
    }

    public virtual IDbSet<EdmMetadata> Metadata { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) => EdmMetadataContext.ConfigureEdmMetadata(modelBuilder.ModelConfiguration);

    public static void ConfigureEdmMetadata(System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration) => modelConfiguration.Entity(typeof (EdmMetadata)).ToTable("EdmMetadata");
  }
}
