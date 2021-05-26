// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbCompiledModel
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Internal;
using System.Data.Entity.Utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// An immutable representation of an Entity Data Model (EDM) model that can be used to create an
  /// <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" /> or can be passed to the constructor of a <see cref="T:System.Data.Entity.DbContext" />.
  /// For increased performance, instances of this type should be cached and re-used to construct contexts.
  /// </summary>
  public class DbCompiledModel
  {
    private static readonly ConcurrentDictionary<Type, Func<EntityConnection, ObjectContext>> _contextConstructors = new ConcurrentDictionary<Type, Func<EntityConnection, ObjectContext>>();
    private static readonly Func<EntityConnection, ObjectContext> _objectContextConstructor = (Func<EntityConnection, ObjectContext>) (c => new ObjectContext(c));
    private readonly ICachedMetadataWorkspace _workspace;
    private readonly DbModelBuilder _cachedModelBuilder;
    private readonly string _defaultSchema;

    internal DbCompiledModel()
    {
    }

    internal DbCompiledModel(
      CodeFirstCachedMetadataWorkspace workspace,
      DbModelBuilder cachedModelBuilder)
    {
      this._workspace = (ICachedMetadataWorkspace) workspace;
      this._cachedModelBuilder = cachedModelBuilder;
      this._defaultSchema = cachedModelBuilder.ModelConfiguration.DefaultSchema;
    }

    internal DbCompiledModel(CodeFirstCachedMetadataWorkspace workspace, string defaultSchema)
    {
      this._workspace = (ICachedMetadataWorkspace) workspace;
      this._defaultSchema = defaultSchema;
    }

    internal virtual DbModelBuilder CachedModelBuilder => this._cachedModelBuilder;

    internal virtual DbProviderInfo ProviderInfo => this._workspace.ProviderInfo;

    internal string DefaultSchema => this._defaultSchema;

    /// <summary>
    /// Creates an instance of ObjectContext or class derived from ObjectContext.  Note that an instance
    /// of DbContext can be created instead by using the appropriate DbContext constructor.
    /// If a derived ObjectContext is used, then it must have a public constructor with a single
    /// EntityConnection parameter.
    /// The connection passed is used by the ObjectContext created, but is not owned by the context.  The caller
    /// must dispose of the connection once the context has been disposed.
    /// </summary>
    /// <typeparam name="TContext"> The type of context to create. </typeparam>
    /// <param name="existingConnection"> An existing connection to a database for use by the context. </param>
    /// <returns>The context.</returns>
    public TContext CreateObjectContext<TContext>(DbConnection existingConnection) where TContext : ObjectContext
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(existingConnection, nameof (existingConnection));
      EntityConnection entityConnection = new EntityConnection(this._workspace.GetMetadataWorkspace(existingConnection), existingConnection);
      TContext context = (TContext) DbCompiledModel.GetConstructorDelegate<TContext>()(entityConnection);
      context.ContextOwnsConnection = true;
      if (string.IsNullOrEmpty(context.DefaultContainerName))
        context.DefaultContainerName = this._workspace.DefaultContainerName;
      foreach (Assembly assembly in this._workspace.Assemblies)
        context.MetadataWorkspace.LoadFromAssembly(assembly);
      return context;
    }

    internal static Func<EntityConnection, ObjectContext> GetConstructorDelegate<TContext>() where TContext : ObjectContext
    {
      if (typeof (TContext) == typeof (ObjectContext))
        return DbCompiledModel._objectContextConstructor;
      Func<EntityConnection, ObjectContext> func;
      if (!DbCompiledModel._contextConstructors.TryGetValue(typeof (TContext), out func))
      {
        ConstructorInfo declaredConstructor = typeof (TContext).GetDeclaredConstructor((Func<ConstructorInfo, bool>) (c => c.IsPublic), new Type[1]
        {
          typeof (EntityConnection)
        }, new Type[1]{ typeof (DbConnection) }, new Type[1]
        {
          typeof (IDbConnection)
        }, new Type[1]{ typeof (IDisposable) }, new Type[1]
        {
          typeof (Component)
        }, new Type[1]{ typeof (MarshalByRefObject) }, new Type[1]
        {
          typeof (object)
        });
        ParameterExpression parameterExpression;
        func = !(declaredConstructor == (ConstructorInfo) null) ? Expression.Lambda<Func<EntityConnection, ObjectContext>>((Expression) Expression.New(declaredConstructor, (Expression) parameterExpression), parameterExpression).Compile() : throw System.Data.Entity.Resources.Error.DbModelBuilder_MissingRequiredCtor((object) typeof (TContext).Name);
        DbCompiledModel._contextConstructors.TryAdd(typeof (TContext), func);
      }
      return func;
    }
  }
}
