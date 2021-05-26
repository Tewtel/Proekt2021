// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Data.Entity.Core.Query.InternalTrees;
using System.Data.Entity.Core.Query.PlanCompiler;
using System.Data.Entity.Core.Query.ResultAssembly;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Internal;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.EntityClient.Internal
{
  internal class EntityCommandDefinition : DbCommandDefinition
  {
    private readonly List<DbCommandDefinition> _mappedCommandDefinitions;
    private readonly EntityCommandDefinition.IColumnMapGenerator[] _columnMapGenerators;
    private readonly ReadOnlyCollection<EntityParameter> _parameters;
    private readonly Set<EntitySet> _entitySets;
    private readonly BridgeDataReaderFactory _bridgeDataReaderFactory;
    private readonly ColumnMapFactory _columnMapFactory;
    private readonly DbProviderServices _storeProviderServices;

    internal EntityCommandDefinition()
    {
    }

    internal EntityCommandDefinition(
      DbProviderFactory storeProviderFactory,
      DbCommandTree commandTree,
      DbInterceptionContext interceptionContext,
      IDbDependencyResolver resolver = null,
      BridgeDataReaderFactory bridgeDataReaderFactory = null,
      ColumnMapFactory columnMapFactory = null)
    {
      this._bridgeDataReaderFactory = bridgeDataReaderFactory ?? new BridgeDataReaderFactory();
      this._columnMapFactory = columnMapFactory ?? new ColumnMapFactory();
      this._storeProviderServices = (resolver != null ? resolver.GetService<DbProviderServices>((object) storeProviderFactory.GetProviderInvariantName()) : (DbProviderServices) null) ?? storeProviderFactory.GetProviderServices();
      try
      {
        if (commandTree.CommandTreeKind == DbCommandTreeKind.Query)
        {
          List<ProviderCommandInfo> providerCommands = new List<ProviderCommandInfo>();
          ColumnMap resultColumnMap;
          int columnCount;
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Compile(commandTree, out providerCommands, out resultColumnMap, out columnCount, out this._entitySets);
          this._columnMapGenerators = new EntityCommandDefinition.IColumnMapGenerator[1]
          {
            (EntityCommandDefinition.IColumnMapGenerator) new EntityCommandDefinition.ConstantColumnMapGenerator(resultColumnMap, columnCount)
          };
          this._mappedCommandDefinitions = new List<DbCommandDefinition>(providerCommands.Count);
          foreach (ProviderCommandInfo providerCommandInfo in providerCommands)
            this._mappedCommandDefinitions.Add(this._storeProviderServices.CreateCommandDefinition(providerCommandInfo.CommandTree, interceptionContext) ?? throw new ProviderIncompatibleException(System.Data.Entity.Resources.Strings.ProviderReturnedNullForCreateCommandDefinition));
        }
        else
        {
          DbFunctionCommandTree functionCommandTree = (DbFunctionCommandTree) commandTree;
          FunctionImportMappingNonComposable targetFunctionMapping = EntityCommandDefinition.GetTargetFunctionMapping(functionCommandTree);
          IList<FunctionParameter> returnParameters = (IList<FunctionParameter>) functionCommandTree.EdmFunction.ReturnParameters;
          int length = returnParameters.Count > 1 ? returnParameters.Count : 1;
          this._columnMapGenerators = new EntityCommandDefinition.IColumnMapGenerator[length];
          TypeUsage storeResultType = this.DetermineStoreResultType(targetFunctionMapping, 0, out this._columnMapGenerators[0]);
          for (int resultSetIndex = 1; resultSetIndex < length; ++resultSetIndex)
            this.DetermineStoreResultType(targetFunctionMapping, resultSetIndex, out this._columnMapGenerators[resultSetIndex]);
          List<KeyValuePair<string, TypeUsage>> keyValuePairList = new List<KeyValuePair<string, TypeUsage>>();
          foreach (KeyValuePair<string, TypeUsage> parameter in functionCommandTree.Parameters)
            keyValuePairList.Add(parameter);
          this._mappedCommandDefinitions = new List<DbCommandDefinition>(1)
          {
            this._storeProviderServices.CreateCommandDefinition((DbCommandTree) new DbFunctionCommandTree(functionCommandTree.MetadataWorkspace, DataSpace.SSpace, targetFunctionMapping.TargetFunction, storeResultType, (IEnumerable<KeyValuePair<string, TypeUsage>>) keyValuePairList))
          };
          if (targetFunctionMapping.FunctionImport.EntitySets.FirstOrDefault<EntitySet>() != null)
          {
            this._entitySets = new Set<EntitySet>();
            this._entitySets.Add(targetFunctionMapping.FunctionImport.EntitySets.FirstOrDefault<EntitySet>());
            this._entitySets.MakeReadOnly();
          }
        }
        List<EntityParameter> entityParameterList = new List<EntityParameter>();
        foreach (KeyValuePair<string, TypeUsage> parameter in commandTree.Parameters)
        {
          EntityParameter fromQueryParameter = EntityCommandDefinition.CreateEntityParameterFromQueryParameter(parameter);
          entityParameterList.Add(fromQueryParameter);
        }
        this._parameters = new ReadOnlyCollection<EntityParameter>((IList<EntityParameter>) entityParameterList);
      }
      catch (EntityCommandCompilationException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new EntityCommandCompilationException(System.Data.Entity.Resources.Strings.EntityClient_CommandDefinitionPreparationFailed, ex);
        throw;
      }
    }

    protected EntityCommandDefinition(
      BridgeDataReaderFactory factory = null,
      ColumnMapFactory columnMapFactory = null,
      List<DbCommandDefinition> mappedCommandDefinitions = null)
    {
      this._bridgeDataReaderFactory = factory ?? new BridgeDataReaderFactory();
      this._columnMapFactory = columnMapFactory ?? new ColumnMapFactory();
      this._mappedCommandDefinitions = mappedCommandDefinitions;
    }

    private TypeUsage DetermineStoreResultType(
      FunctionImportMappingNonComposable mapping,
      int resultSetIndex,
      out EntityCommandDefinition.IColumnMapGenerator columnMapGenerator)
    {
      EdmFunction functionImport = mapping.FunctionImport;
      StructuralType returnType;
      TypeUsage type;
      if (MetadataHelper.TryGetFunctionImportReturnType<StructuralType>(functionImport, resultSetIndex, out returnType))
      {
        EntityCommandDefinition.ValidateEdmResultType((EdmType) returnType, functionImport);
        EntitySet entitySet = functionImport.EntitySets.Count > resultSetIndex ? functionImport.EntitySets[resultSetIndex] : (EntitySet) null;
        columnMapGenerator = (EntityCommandDefinition.IColumnMapGenerator) new EntityCommandDefinition.FunctionColumnMapGenerator(mapping, resultSetIndex, entitySet, returnType, this._columnMapFactory);
        type = mapping.GetExpectedTargetResultType(resultSetIndex);
      }
      else
      {
        FunctionParameter returnParameter = MetadataHelper.GetReturnParameter(functionImport, resultSetIndex);
        if (returnParameter != null && returnParameter.TypeUsage != null)
        {
          type = returnParameter.TypeUsage;
          ScalarColumnMap scalarColumnMap = new ScalarColumnMap(((CollectionType) type.EdmType).TypeUsage, string.Empty, 0, 0);
          SimpleCollectionColumnMap collectionColumnMap = new SimpleCollectionColumnMap(type, string.Empty, (ColumnMap) scalarColumnMap, (SimpleColumnMap[]) null, (SimpleColumnMap[]) null);
          columnMapGenerator = (EntityCommandDefinition.IColumnMapGenerator) new EntityCommandDefinition.ConstantColumnMapGenerator((ColumnMap) collectionColumnMap, 1);
        }
        else
        {
          type = (TypeUsage) null;
          columnMapGenerator = (EntityCommandDefinition.IColumnMapGenerator) new EntityCommandDefinition.ConstantColumnMapGenerator((ColumnMap) null, 0);
        }
      }
      return type;
    }

    private static void ValidateEdmResultType(EdmType resultType, EdmFunction functionImport)
    {
      if (!Helper.IsComplexType(resultType))
        return;
      ComplexType complexType = resultType as ComplexType;
      foreach (EdmProperty property in complexType.Properties)
      {
        if (property.TypeUsage.EdmType.BuiltInTypeKind == BuiltInTypeKind.ComplexType)
          throw new NotSupportedException(System.Data.Entity.Resources.Strings.ComplexTypeAsReturnTypeAndNestedComplexProperty((object) property.Name, (object) complexType.Name, (object) functionImport.FullName));
      }
    }

    private static FunctionImportMappingNonComposable GetTargetFunctionMapping(
      DbFunctionCommandTree functionCommandTree)
    {
      FunctionImportMapping targetFunctionMapping;
      if (!functionCommandTree.MetadataWorkspace.TryGetFunctionImportMapping(functionCommandTree.EdmFunction, out targetFunctionMapping))
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.EntityClient_UnmappedFunctionImport((object) functionCommandTree.EdmFunction.FullName));
      return (FunctionImportMappingNonComposable) targetFunctionMapping;
    }

    internal virtual IEnumerable<EntityParameter> Parameters => (IEnumerable<EntityParameter>) this._parameters;

    internal virtual Set<EntitySet> EntitySets => this._entitySets;

    public override DbCommand CreateCommand() => (DbCommand) new EntityCommand(this, new DbInterceptionContext());

    internal ColumnMap CreateColumnMap(DbDataReader storeDataReader) => this.CreateColumnMap(storeDataReader, 0);

    internal virtual ColumnMap CreateColumnMap(
      DbDataReader storeDataReader,
      int resultSetIndex)
    {
      return this._columnMapGenerators[resultSetIndex].CreateColumnMap(storeDataReader);
    }

    private static EntityParameter CreateEntityParameterFromQueryParameter(
      KeyValuePair<string, TypeUsage> queryParameter)
    {
      EntityParameter parameter = new EntityParameter();
      parameter.ParameterName = queryParameter.Key;
      EntityCommandDefinition.PopulateParameterFromTypeUsage(parameter, queryParameter.Value, false);
      return parameter;
    }

    internal static void PopulateParameterFromTypeUsage(
      EntityParameter parameter,
      TypeUsage type,
      bool isOutParam)
    {
      if (type != null)
      {
        if (Helper.IsEnumType(type.EdmType))
        {
          type = TypeUsage.Create((EdmType) Helper.GetUnderlyingEdmTypeForEnumType(type.EdmType));
        }
        else
        {
          PrimitiveTypeKind spatialType;
          if (Helper.IsSpatialType(type, out spatialType))
            parameter.EdmType = (EdmType) EdmProviderManifest.Instance.GetPrimitiveType(spatialType);
        }
      }
      DbCommandDefinition.PopulateParameterFromTypeUsage((DbParameter) parameter, type, isOutParam);
    }

    internal virtual DbDataReader Execute(
      EntityCommand entityCommand,
      CommandBehavior behavior)
    {
      DbDataReader dbDataReader1 = CommandBehavior.SequentialAccess == (behavior & CommandBehavior.SequentialAccess) ? this.ExecuteStoreCommands(entityCommand, behavior & ~CommandBehavior.SequentialAccess) : throw new InvalidOperationException(System.Data.Entity.Resources.Strings.ADP_MustUseSequentialAccess);
      DbDataReader dbDataReader2 = (DbDataReader) null;
      if (dbDataReader1 != null)
      {
        try
        {
          ColumnMap columnMap = this.CreateColumnMap(dbDataReader1, 0);
          if (columnMap == null)
          {
            CommandHelper.ConsumeReader(dbDataReader1);
            dbDataReader2 = dbDataReader1;
          }
          else
          {
            MetadataWorkspace metadataWorkspace = entityCommand.Connection.GetMetadataWorkspace();
            IEnumerable<ColumnMap> resultColumnMaps = this.GetNextResultColumnMaps(dbDataReader1);
            dbDataReader2 = this._bridgeDataReaderFactory.Create(dbDataReader1, columnMap, metadataWorkspace, resultColumnMaps);
          }
        }
        catch
        {
          dbDataReader1.Dispose();
          throw;
        }
      }
      return dbDataReader2;
    }

    internal virtual async Task<DbDataReader> ExecuteAsync(
      EntityCommand entityCommand,
      CommandBehavior behavior,
      CancellationToken cancellationToken)
    {
      if (CommandBehavior.SequentialAccess != (behavior & CommandBehavior.SequentialAccess))
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.ADP_MustUseSequentialAccess);
      cancellationToken.ThrowIfCancellationRequested();
      DbDataReader storeDataReader = await this.ExecuteStoreCommandsAsync(entityCommand, behavior & ~CommandBehavior.SequentialAccess, cancellationToken).WithCurrentCulture<DbDataReader>();
      DbDataReader dbDataReader;
      if (storeDataReader != null)
      {
        try
        {
          ColumnMap columnMap = this.CreateColumnMap(storeDataReader, 0);
          if (columnMap == null)
          {
            await CommandHelper.ConsumeReaderAsync(storeDataReader, cancellationToken).WithCurrentCulture();
            dbDataReader = storeDataReader;
          }
          else
          {
            MetadataWorkspace metadataWorkspace = entityCommand.Connection.GetMetadataWorkspace();
            IEnumerable<ColumnMap> resultColumnMaps = this.GetNextResultColumnMaps(storeDataReader);
            dbDataReader = this._bridgeDataReaderFactory.Create(storeDataReader, columnMap, metadataWorkspace, resultColumnMaps);
          }
        }
        catch
        {
          storeDataReader.Dispose();
          throw;
        }
      }
      return dbDataReader;
    }

    private IEnumerable<ColumnMap> GetNextResultColumnMaps(
      DbDataReader storeDataReader)
    {
      for (int i = 1; i < this._columnMapGenerators.Length; ++i)
        yield return this.CreateColumnMap(storeDataReader, i);
    }

    internal virtual DbDataReader ExecuteStoreCommands(
      EntityCommand entityCommand,
      CommandBehavior behavior)
    {
      DbCommand dbCommand = this.PrepareEntityCommandBeforeExecution(entityCommand);
      try
      {
        return dbCommand.ExecuteReader(behavior);
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new EntityCommandExecutionException(System.Data.Entity.Resources.Strings.EntityClient_CommandDefinitionExecutionFailed, ex);
        throw;
      }
    }

    internal virtual async Task<DbDataReader> ExecuteStoreCommandsAsync(
      EntityCommand entityCommand,
      CommandBehavior behavior,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      DbCommand dbCommand = this.PrepareEntityCommandBeforeExecution(entityCommand);
      DbDataReader dbDataReader;
      try
      {
        dbDataReader = await dbCommand.ExecuteReaderAsync(behavior, cancellationToken).WithCurrentCulture<DbDataReader>();
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new EntityCommandExecutionException(System.Data.Entity.Resources.Strings.EntityClient_CommandDefinitionExecutionFailed, ex);
        throw;
      }
      return dbDataReader;
    }

    private DbCommand PrepareEntityCommandBeforeExecution(EntityCommand entityCommand)
    {
      if (1 != this._mappedCommandDefinitions.Count)
        throw new NotSupportedException("MARS");
      EntityTransaction entityTransaction = entityCommand.ValidateAndGetEntityTransaction();
      InterceptableDbCommand interceptableDbCommand = new InterceptableDbCommand(this._mappedCommandDefinitions[0].CreateCommand(), entityCommand.InterceptionContext);
      CommandHelper.SetStoreProviderCommandState(entityCommand, entityTransaction, (DbCommand) interceptableDbCommand);
      bool flag = false;
      if (interceptableDbCommand.Parameters != null)
      {
        foreach (DbParameter parameter in interceptableDbCommand.Parameters)
        {
          int index = entityCommand.Parameters.IndexOf(parameter.ParameterName);
          if (-1 != index)
          {
            EntityCommandDefinition.SyncParameterProperties(entityCommand.Parameters[index], parameter, this._storeProviderServices);
            if (parameter.Direction != ParameterDirection.Input)
              flag = true;
          }
        }
      }
      if (flag)
        entityCommand.SetStoreProviderCommand((DbCommand) interceptableDbCommand);
      return (DbCommand) interceptableDbCommand;
    }

    private static void SyncParameterProperties(
      EntityParameter entityParameter,
      DbParameter storeParameter,
      DbProviderServices storeProviderServices)
    {
      IDbDataParameter dbDataParameter = (IDbDataParameter) storeParameter;
      TypeUsage typeUsageForScalar = TypeHelpers.GetPrimitiveTypeUsageForScalar(entityParameter.GetTypeUsage());
      storeProviderServices.SetParameterValue(storeParameter, typeUsageForScalar, entityParameter.Value);
      if (entityParameter.IsDirectionSpecified)
        storeParameter.Direction = entityParameter.Direction;
      if (entityParameter.IsIsNullableSpecified)
        storeParameter.IsNullable = entityParameter.IsNullable;
      if (entityParameter.IsSizeSpecified)
        storeParameter.Size = entityParameter.Size;
      if (entityParameter.IsPrecisionSpecified)
        dbDataParameter.Precision = entityParameter.Precision;
      if (!entityParameter.IsScaleSpecified)
        return;
      dbDataParameter.Scale = entityParameter.Scale;
    }

    internal virtual string ToTraceString()
    {
      if (this._mappedCommandDefinitions == null)
        return string.Empty;
      if (this._mappedCommandDefinitions.Count == 1)
        return this._mappedCommandDefinitions[0].CreateCommand().CommandText;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (DbCommandDefinition commandDefinition in this._mappedCommandDefinitions)
      {
        DbCommand command = commandDefinition.CreateCommand();
        stringBuilder.Append(command.CommandText);
      }
      return stringBuilder.ToString();
    }

    private interface IColumnMapGenerator
    {
      ColumnMap CreateColumnMap(DbDataReader reader);
    }

    private sealed class ConstantColumnMapGenerator : EntityCommandDefinition.IColumnMapGenerator
    {
      private readonly ColumnMap _columnMap;
      private readonly int _fieldsRequired;

      internal ConstantColumnMapGenerator(ColumnMap columnMap, int fieldsRequired)
      {
        this._columnMap = columnMap;
        this._fieldsRequired = fieldsRequired;
      }

      ColumnMap EntityCommandDefinition.IColumnMapGenerator.CreateColumnMap(
        DbDataReader reader)
      {
        if (reader != null && reader.FieldCount < this._fieldsRequired)
          throw new EntityCommandExecutionException(System.Data.Entity.Resources.Strings.EntityClient_TooFewColumns);
        return this._columnMap;
      }
    }

    private sealed class FunctionColumnMapGenerator : EntityCommandDefinition.IColumnMapGenerator
    {
      private readonly FunctionImportMappingNonComposable _mapping;
      private readonly EntitySet _entitySet;
      private readonly StructuralType _baseStructuralType;
      private readonly int _resultSetIndex;
      private readonly ColumnMapFactory _columnMapFactory;

      internal FunctionColumnMapGenerator(
        FunctionImportMappingNonComposable mapping,
        int resultSetIndex,
        EntitySet entitySet,
        StructuralType baseStructuralType,
        ColumnMapFactory columnMapFactory)
      {
        this._mapping = mapping;
        this._entitySet = entitySet;
        this._baseStructuralType = baseStructuralType;
        this._resultSetIndex = resultSetIndex;
        this._columnMapFactory = columnMapFactory;
      }

      ColumnMap EntityCommandDefinition.IColumnMapGenerator.CreateColumnMap(
        DbDataReader reader)
      {
        return (ColumnMap) this._columnMapFactory.CreateFunctionImportStructuralTypeColumnMap(reader, this._mapping, this._resultSetIndex, this._entitySet, this._baseStructuralType);
      }
    }
  }
}
