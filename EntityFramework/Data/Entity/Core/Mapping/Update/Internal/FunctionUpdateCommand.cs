// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.Update.Internal.FunctionUpdateCommand
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Internal;
using System.Data.Entity.Spatial;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.Mapping.Update.Internal
{
  internal class FunctionUpdateCommand : UpdateCommand
  {
    private readonly ReadOnlyCollection<System.Data.Entity.Core.IEntityStateEntry> _stateEntries;
    private readonly DbCommand _dbCommand;
    private List<KeyValuePair<int, DbParameter>> _inputIdentifiers;
    private Dictionary<int, string> _outputIdentifiers;
    private DbParameter _rowsAffectedParameter;

    internal FunctionUpdateCommand(
      ModificationFunctionMapping functionMapping,
      UpdateTranslator translator,
      ReadOnlyCollection<System.Data.Entity.Core.IEntityStateEntry> stateEntries,
      ExtractedStateEntry stateEntry)
      : this(translator, stateEntries, stateEntry, translator.GenerateCommandDefinition(functionMapping).CreateCommand())
    {
    }

    protected FunctionUpdateCommand(
      UpdateTranslator translator,
      ReadOnlyCollection<System.Data.Entity.Core.IEntityStateEntry> stateEntries,
      ExtractedStateEntry stateEntry,
      DbCommand dbCommand)
      : base(translator, stateEntry.Original, stateEntry.Current)
    {
      this._stateEntries = stateEntries;
      this._dbCommand = (DbCommand) new InterceptableDbCommand(dbCommand, translator.InterceptionContext);
    }

    protected virtual List<KeyValuePair<string, PropagatorResult>> ResultColumns { get; set; }

    internal override IEnumerable<int> InputIdentifiers
    {
      get
      {
        if (this._inputIdentifiers != null)
        {
          foreach (KeyValuePair<int, DbParameter> inputIdentifier in this._inputIdentifiers)
            yield return inputIdentifier.Key;
        }
      }
    }

    internal override IEnumerable<int> OutputIdentifiers => this._outputIdentifiers == null ? Enumerable.Empty<int>() : (IEnumerable<int>) this._outputIdentifiers.Keys;

    internal override UpdateCommandKind Kind => UpdateCommandKind.Function;

    internal override IList<System.Data.Entity.Core.IEntityStateEntry> GetStateEntries(
      UpdateTranslator translator)
    {
      return (IList<System.Data.Entity.Core.IEntityStateEntry>) this._stateEntries;
    }

    internal void SetParameterValue(
      PropagatorResult result,
      ModificationFunctionParameterBinding parameterBinding,
      UpdateTranslator translator)
    {
      DbParameter parameter = this._dbCommand.Parameters[parameterBinding.Parameter.Name];
      TypeUsage typeUsage = parameterBinding.Parameter.TypeUsage;
      object principalValue = translator.KeyManager.GetPrincipalValue(result);
      translator.SetParameterValue(parameter, typeUsage, principalValue);
      int identifier = result.Identifier;
      if (-1 == identifier)
        return;
      if (this._inputIdentifiers == null)
        this._inputIdentifiers = new List<KeyValuePair<int, DbParameter>>(2);
      foreach (int principal in translator.KeyManager.GetPrincipals(identifier))
        this._inputIdentifiers.Add(new KeyValuePair<int, DbParameter>(principal, parameter));
    }

    internal void RegisterRowsAffectedParameter(FunctionParameter rowsAffectedParameter)
    {
      if (rowsAffectedParameter == null)
        return;
      this._rowsAffectedParameter = this._dbCommand.Parameters[rowsAffectedParameter.Name];
    }

    internal void AddResultColumn(
      UpdateTranslator translator,
      string columnName,
      PropagatorResult result)
    {
      if (this.ResultColumns == null)
        this.ResultColumns = new List<KeyValuePair<string, PropagatorResult>>(2);
      this.ResultColumns.Add(new KeyValuePair<string, PropagatorResult>(columnName, result));
      int identifier = result.Identifier;
      if (-1 == identifier)
        return;
      if (translator.KeyManager.HasPrincipals(identifier))
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.Update_GeneratedDependent((object) columnName));
      this.AddOutputIdentifier(columnName, identifier);
    }

    private void AddOutputIdentifier(string columnName, int identifier)
    {
      if (this._outputIdentifiers == null)
        this._outputIdentifiers = new Dictionary<int, string>(2);
      this._outputIdentifiers[identifier] = columnName;
    }

    internal virtual void SetInputIdentifiers(Dictionary<int, object> identifierValues)
    {
      if (this._inputIdentifiers == null)
        return;
      foreach (KeyValuePair<int, DbParameter> inputIdentifier in this._inputIdentifiers)
      {
        object obj;
        if (identifierValues.TryGetValue(inputIdentifier.Key, out obj))
          inputIdentifier.Value.Value = obj;
      }
    }

    internal override long Execute(
      Dictionary<int, object> identifierValues,
      List<KeyValuePair<PropagatorResult, object>> generatedValues)
    {
      EntityConnection connection = this.Translator.Connection;
      this._dbCommand.Transaction = connection.CurrentTransaction == null ? (DbTransaction) null : connection.CurrentTransaction.StoreTransaction;
      this._dbCommand.Connection = connection.StoreConnection;
      int? commandTimeout = this.Translator.CommandTimeout;
      if (commandTimeout.HasValue)
      {
        DbCommand dbCommand = this._dbCommand;
        commandTimeout = this.Translator.CommandTimeout;
        int num = commandTimeout.Value;
        dbCommand.CommandTimeout = num;
      }
      this.SetInputIdentifiers(identifierValues);
      long rowsAffected;
      if (this.ResultColumns != null)
      {
        rowsAffected = 0L;
        IBaseList<EdmMember> structuralMembers = TypeHelpers.GetAllStructuralMembers((EdmType) this.CurrentValues.StructuralType);
        using (DbDataReader reader = this._dbCommand.ExecuteReader(CommandBehavior.SequentialAccess))
        {
          if (reader.Read())
          {
            ++rowsAffected;
            foreach (KeyValuePair<int, PropagatorResult> keyValuePair in (IEnumerable<KeyValuePair<int, PropagatorResult>>) this.ResultColumns.Select<KeyValuePair<string, PropagatorResult>, KeyValuePair<int, PropagatorResult>>((Func<KeyValuePair<string, PropagatorResult>, KeyValuePair<int, PropagatorResult>>) (r => new KeyValuePair<int, PropagatorResult>(this.GetColumnOrdinal(this.Translator, reader, r.Key), r.Value))).OrderBy<KeyValuePair<int, PropagatorResult>, int>((Func<KeyValuePair<int, PropagatorResult>, int>) (r => r.Key)))
            {
              int key1 = keyValuePair.Key;
              if (key1 != -1)
              {
                TypeUsage typeUsage = structuralMembers[keyValuePair.Value.RecordOrdinal].TypeUsage;
                object obj = !Helper.IsSpatialType(typeUsage) || reader.IsDBNull(key1) ? reader.GetValue(key1) : SpatialHelpers.GetSpatialValue(this.Translator.MetadataWorkspace, reader, typeUsage, key1);
                PropagatorResult key2 = keyValuePair.Value;
                generatedValues.Add(new KeyValuePair<PropagatorResult, object>(key2, obj));
                int identifier = key2.Identifier;
                if (-1 != identifier)
                  identifierValues.Add(identifier, obj);
              }
              else
                break;
            }
          }
          CommandHelper.ConsumeReader(reader);
        }
      }
      else
        rowsAffected = (long) this._dbCommand.ExecuteNonQuery();
      return this.GetRowsAffected(rowsAffected, this.Translator);
    }

    internal override async Task<long> ExecuteAsync(
      Dictionary<int, object> identifierValues,
      List<KeyValuePair<PropagatorResult, object>> generatedValues,
      CancellationToken cancellationToken)
    {
      FunctionUpdateCommand functionUpdateCommand = this;
      cancellationToken.ThrowIfCancellationRequested();
      EntityConnection connection = functionUpdateCommand.Translator.Connection;
      functionUpdateCommand._dbCommand.Transaction = connection.CurrentTransaction == null ? (DbTransaction) null : connection.CurrentTransaction.StoreTransaction;
      functionUpdateCommand._dbCommand.Connection = connection.StoreConnection;
      int? commandTimeout = functionUpdateCommand.Translator.CommandTimeout;
      if (commandTimeout.HasValue)
      {
        DbCommand dbCommand = functionUpdateCommand._dbCommand;
        commandTimeout = functionUpdateCommand.Translator.CommandTimeout;
        int num = commandTimeout.Value;
        dbCommand.CommandTimeout = num;
      }
      functionUpdateCommand.SetInputIdentifiers(identifierValues);
      long rowsAffected;
      if (functionUpdateCommand.ResultColumns != null)
      {
        rowsAffected = 0L;
        IBaseList<EdmMember> members = TypeHelpers.GetAllStructuralMembers((EdmType) functionUpdateCommand.CurrentValues.StructuralType);
        using (DbDataReader reader = await functionUpdateCommand._dbCommand.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken).WithCurrentCulture<DbDataReader>())
        {
          if (await reader.ReadAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            ++rowsAffected;
            foreach (KeyValuePair<int, PropagatorResult> keyValuePair in (IEnumerable<KeyValuePair<int, PropagatorResult>>) functionUpdateCommand.ResultColumns.Select<KeyValuePair<string, PropagatorResult>, KeyValuePair<int, PropagatorResult>>((Func<KeyValuePair<string, PropagatorResult>, KeyValuePair<int, PropagatorResult>>) (r => new KeyValuePair<int, PropagatorResult>(this.GetColumnOrdinal(this.Translator, reader, r.Key), r.Value))).OrderBy<KeyValuePair<int, PropagatorResult>, int>((Func<KeyValuePair<int, PropagatorResult>, int>) (r => r.Key)))
            {
              KeyValuePair<int, PropagatorResult> resultColumn = keyValuePair;
              int columnOrdinal = resultColumn.Key;
              TypeUsage columnType = members[resultColumn.Value.RecordOrdinal].TypeUsage;
              bool flag = Helper.IsSpatialType(columnType);
              if (flag)
                flag = !await reader.IsDBNullAsync(columnOrdinal, cancellationToken).WithCurrentCulture<bool>();
              System.Data.Entity.Utilities.TaskExtensions.CultureAwaiter<object> cultureAwaiter;
              object obj;
              if (flag)
              {
                cultureAwaiter = SpatialHelpers.GetSpatialValueAsync(functionUpdateCommand.Translator.MetadataWorkspace, reader, columnType, columnOrdinal, cancellationToken).WithCurrentCulture<object>();
                obj = await cultureAwaiter;
              }
              else
              {
                cultureAwaiter = reader.GetFieldValueAsync<object>(columnOrdinal, cancellationToken).WithCurrentCulture<object>();
                obj = await cultureAwaiter;
              }
              PropagatorResult key = resultColumn.Value;
              generatedValues.Add(new KeyValuePair<PropagatorResult, object>(key, obj));
              int identifier = key.Identifier;
              if (-1 != identifier)
                identifierValues.Add(identifier, obj);
              columnType = (TypeUsage) null;
              resultColumn = new KeyValuePair<int, PropagatorResult>();
            }
          }
          await CommandHelper.ConsumeReaderAsync(reader, cancellationToken).WithCurrentCulture();
        }
        members = (IBaseList<EdmMember>) null;
      }
      else
        rowsAffected = (long) await functionUpdateCommand._dbCommand.ExecuteNonQueryAsync(cancellationToken).WithCurrentCulture<int>();
      return functionUpdateCommand.GetRowsAffected(rowsAffected, functionUpdateCommand.Translator);
    }

    protected virtual long GetRowsAffected(long rowsAffected, UpdateTranslator translator)
    {
      if (this._rowsAffectedParameter != null)
      {
        if (DBNull.Value.Equals(this._rowsAffectedParameter.Value))
        {
          rowsAffected = 0L;
        }
        else
        {
          try
          {
            rowsAffected = Convert.ToInt64(this._rowsAffectedParameter.Value, (IFormatProvider) CultureInfo.InvariantCulture);
          }
          catch (Exception ex)
          {
            if (ex.RequiresContext())
              throw new UpdateException(System.Data.Entity.Resources.Strings.Update_UnableToConvertRowsAffectedParameter((object) this._rowsAffectedParameter.ParameterName, (object) typeof (long).FullName), ex, this.GetStateEntries(translator).Cast<ObjectStateEntry>().Distinct<ObjectStateEntry>());
            throw;
          }
        }
      }
      return rowsAffected;
    }

    private int GetColumnOrdinal(
      UpdateTranslator translator,
      DbDataReader reader,
      string columnName)
    {
      try
      {
        return reader.GetOrdinal(columnName);
      }
      catch (IndexOutOfRangeException ex)
      {
        throw new UpdateException(System.Data.Entity.Resources.Strings.Update_MissingResultColumn((object) columnName), (Exception) null, this.GetStateEntries(translator).Cast<ObjectStateEntry>().Distinct<ObjectStateEntry>());
      }
    }

    private static ModificationOperator GetModificationOperator(
      EntityState state)
    {
      if (state <= EntityState.Added)
      {
        if (state != EntityState.Unchanged && state == EntityState.Added)
          return ModificationOperator.Insert;
      }
      else if (state == EntityState.Deleted)
        return ModificationOperator.Delete;
      return ModificationOperator.Update;
    }

    internal override int CompareToType(UpdateCommand otherCommand)
    {
      FunctionUpdateCommand functionUpdateCommand = (FunctionUpdateCommand) otherCommand;
      System.Data.Entity.Core.IEntityStateEntry stateEntry1 = this._stateEntries[0];
      System.Data.Entity.Core.IEntityStateEntry stateEntry2 = functionUpdateCommand._stateEntries[0];
      int num1 = (int) (FunctionUpdateCommand.GetModificationOperator(stateEntry1.State) - FunctionUpdateCommand.GetModificationOperator(stateEntry2.State));
      if (num1 != 0)
        return num1;
      int num2 = StringComparer.Ordinal.Compare(stateEntry1.EntitySet.Name, stateEntry2.EntitySet.Name);
      if (num2 != 0)
        return num2;
      int num3 = StringComparer.Ordinal.Compare(stateEntry1.EntitySet.EntityContainer.Name, stateEntry2.EntitySet.EntityContainer.Name);
      if (num3 != 0)
        return num3;
      int num4 = this._inputIdentifiers == null ? 0 : this._inputIdentifiers.Count;
      int num5 = functionUpdateCommand._inputIdentifiers == null ? 0 : functionUpdateCommand._inputIdentifiers.Count;
      int num6 = num4 - num5;
      if (num6 != 0)
        return num6;
      for (int index = 0; index < num4; ++index)
      {
        DbParameter dbParameter1 = this._inputIdentifiers[index].Value;
        DbParameter dbParameter2 = functionUpdateCommand._inputIdentifiers[index].Value;
        num6 = ByValueComparer.Default.Compare(dbParameter1.Value, dbParameter2.Value);
        if (num6 != 0)
          return num6;
      }
      for (int index = 0; index < num4; ++index)
      {
        KeyValuePair<int, DbParameter> inputIdentifier = this._inputIdentifiers[index];
        int key1 = inputIdentifier.Key;
        inputIdentifier = functionUpdateCommand._inputIdentifiers[index];
        int key2 = inputIdentifier.Key;
        num6 = key1 - key2;
        if (num6 != 0)
          return num6;
      }
      return num6;
    }
  }
}
