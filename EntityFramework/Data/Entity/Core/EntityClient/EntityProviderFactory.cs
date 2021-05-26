// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityClient.EntityProviderFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.EntityClient.Internal;
using System.Security;
using System.Security.Permissions;

namespace System.Data.Entity.Core.EntityClient
{
  /// <summary>
  /// Class representing a provider factory for the entity client provider
  /// </summary>
  public sealed class EntityProviderFactory : DbProviderFactory, IServiceProvider
  {
    /// <summary>
    /// A singleton object for the entity client provider factory object.
    /// This remains a public field (not property) because DbProviderFactory expects a field.
    /// </summary>
    public static readonly EntityProviderFactory Instance = new EntityProviderFactory();

    private EntityProviderFactory()
    {
    }

    /// <summary>
    /// Returns a new instance of the provider's class that implements the
    /// <see cref="T:System.Data.Entity.Core.EntityClient.EntityCommand" />
    /// class.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="T:System.Data.Entity.Core.EntityClient.EntityCommand" />.
    /// </returns>
    public override DbCommand CreateCommand() => (DbCommand) new EntityCommand();

    /// <summary>
    /// Throws a <see cref="T:System.NotSupportedException" />. This method is currently not supported.
    /// </summary>
    /// <returns>This method is currently not supported.</returns>
    public override DbCommandBuilder CreateCommandBuilder() => throw new NotSupportedException();

    /// <summary>
    /// Returns a new instance of the provider's class that implements the
    /// <see cref="T:System.Data.Entity.Core.EntityClient.EntityConnection" />
    /// class.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="T:System.Data.Entity.Core.EntityClient.EntityConnection" />.
    /// </returns>
    public override DbConnection CreateConnection() => (DbConnection) new EntityConnection();

    /// <summary>
    /// Returns a new instance of the provider's class that implements the
    /// <see cref="T:System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder" />
    /// class.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="T:System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder" />.
    /// </returns>
    public override DbConnectionStringBuilder CreateConnectionStringBuilder() => (DbConnectionStringBuilder) new EntityConnectionStringBuilder();

    /// <summary>
    /// Throws a <see cref="T:System.NotSupportedException" />. This method is currently not supported.
    /// </summary>
    /// <returns>This method is currently not supported.</returns>
    public override DbDataAdapter CreateDataAdapter() => throw new NotSupportedException();

    /// <summary>
    /// Returns a new instance of the provider's class that implements the
    /// <see cref="T:System.Data.Entity.Core.EntityClient.EntityParameter" />
    /// class.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="T:System.Data.Entity.Core.EntityClient.EntityParameter" />.
    /// </returns>
    public override DbParameter CreateParameter() => (DbParameter) new EntityParameter();

    /// <summary>
    /// Throws a <see cref="T:System.NotSupportedException" />. This method is currently not supported.
    /// </summary>
    /// <param name="state">This method is currently not supported.</param>
    /// <returns>This method is currently not supported.</returns>
    public override CodeAccessPermission CreatePermission(PermissionState state) => throw new NotSupportedException();

    /// <summary>
    /// Returns the requested <see cref="T:System.IServiceProvider" /> class.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="T:System.IServiceProvider" />. The supported types are
    /// <see cref="T:System.Data.Entity.Core.Common.DbProviderServices" />
    /// ,
    /// <see cref="T:System.Data.Entity.Core.Common.DbCommandDefinitionBuilder" />
    /// , and
    /// <see cref="T:System.Data.IEntityAdapter" />
    /// . Returns null (or Nothing in Visual Basic) for every other type.
    /// </returns>
    /// <param name="serviceType">
    /// The <see cref="T:System.Type" /> to return.
    /// </param>
    object IServiceProvider.GetService(Type serviceType) => !(serviceType == typeof (DbProviderServices)) ? (object) null : (object) EntityProviderServices.Instance;
  }
}
