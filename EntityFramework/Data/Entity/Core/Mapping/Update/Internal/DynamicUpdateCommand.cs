// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.Update.Internal.DynamicUpdateCommand
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.Mapping.Update.Internal
{
  internal class DynamicUpdateCommand : UpdateCommand
  {
    private readonly ModificationOperator _operator;
    private readonly TableChangeProcessor _processor;
    private readonly List<KeyValuePair<int, DbSetClause>> _inputIdentifiers;
    private readonly Dictionary<int, string> _outputIdentifiers;
    private readonly DbModificationCommandTree _modificationCommandTree;

    internal DynamicUpdateCommand(
      TableChangeProcessor processor,
      UpdateTranslator translator,
      ModificationOperator modificationOperator,
      PropagatorResult originalValues,
      PropagatorResult currentValues,
      DbModificationCommandTree tree,
      Dictionary<int, string> outputIdentifiers)
      : base(translator, originalValues, currentValues)
    {
      this._processor = processor;
      this._operator = modificationOperator;
      this._modificationCommandTree = tree;
      this._outputIdentifiers = outputIdentifiers;
      if (ModificationOperator.Insert != modificationOperator && modificationOperator != ModificationOperator.Update)
        return;
      this._inputIdentifiers = new List<KeyValuePair<int, DbSetClause>>(2);
      foreach (KeyValuePair<EdmMember, PropagatorResult> pairEnumeration in Helper.PairEnumerations<EdmMember, PropagatorResult>(TypeHelpers.GetAllStructuralMembers((EdmType) this.CurrentValues.StructuralType), (IEnumerable<PropagatorResult>) this.CurrentValues.GetMemberValues()))
      {
        int identifier = pairEnumeration.Value.Identifier;
        DbSetClause setter;
        if (-1 != identifier && DynamicUpdateCommand.TryGetSetterExpression(tree, pairEnumeration.Key, modificationOperator, out setter))
        {
          foreach (int principal in translator.KeyManager.GetPrincipals(identifier))
            this._inputIdentifiers.Add(new KeyValuePair<int, DbSetClause>(principal, setter));
        }
      }
    }

    private static bool TryGetSetterExpression(
      DbModificationCommandTree tree,
      EdmMember member,
      ModificationOperator op,
      out DbSetClause setter)
    {
      foreach (DbSetClause dbSetClause in ModificationOperator.Insert != op ? (IEnumerable<DbModificationClause>) ((DbUpdateCommandTree) tree).SetClauses : (IEnumerable<DbModificationClause>) ((DbInsertCommandTree) tree).SetClauses)
      {
        if (((DbPropertyExpression) dbSetClause.Property).Property.EdmEquals((MetadataItem) member))
        {
          setter = dbSetClause;
          return true;
        }
      }
      setter = (DbSetClause) null;
      return false;
    }

    internal override long Execute(
      Dictionary<int, object> identifierValues,
      List<KeyValuePair<PropagatorResult, object>> generatedValues)
    {
      using (DbCommand command = this.CreateCommand(identifierValues))
      {
        EntityConnection connection = this.Translator.Connection;
        command.Transaction = connection.CurrentTransaction == null ? (DbTransaction) null : connection.CurrentTransaction.StoreTransaction;
        command.Connection = connection.StoreConnection;
        if (this.Translator.CommandTimeout.HasValue)
          command.CommandTimeout = this.Translator.CommandTimeout.Value;
        int num;
        if (this._modificationCommandTree.HasReader)
        {
          num = 0;
          using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
          {
            if (reader.Read())
            {
              ++num;
              IBaseList<EdmMember> structuralMembers = TypeHelpers.GetAllStructuralMembers((EdmType) this.CurrentValues.StructuralType);
              for (int index = 0; index < reader.FieldCount; ++index)
              {
                string name = reader.GetName(index);
                EdmMember edmMember = structuralMembers[name];
                object obj = !Helper.IsSpatialType(edmMember.TypeUsage) || reader.IsDBNull(index) ? reader.GetValue(index) : SpatialHelpers.GetSpatialValue(this.Translator.MetadataWorkspace, reader, edmMember.TypeUsage, index);
                PropagatorResult memberValue = this.CurrentValues.GetMemberValue(structuralMembers.IndexOf(edmMember));
                generatedValues.Add(new KeyValuePair<PropagatorResult, object>(memberValue, obj));
                int identifier = memberValue.Identifier;
                if (-1 != identifier)
                  identifierValues.Add(identifier, obj);
              }
            }
            CommandHelper.ConsumeReader(reader);
          }
        }
        else
          num = command.ExecuteNonQuery();
        return (long) num;
      }
    }

    internal override async Task<long> ExecuteAsync(
      Dictionary<int, object> identifierValues,
      List<KeyValuePair<PropagatorResult, object>> generatedValues,
      CancellationToken cancellationToken)
    {
      DynamicUpdateCommand dynamicUpdateCommand = this;
      cancellationToken.ThrowIfCancellationRequested();
      long num;
      using (DbCommand command = dynamicUpdateCommand.CreateCommand(identifierValues))
      {
        EntityConnection connection = dynamicUpdateCommand.Translator.Connection;
        command.Transaction = connection.CurrentTransaction == null ? (DbTransaction) null : connection.CurrentTransaction.StoreTransaction;
        command.Connection = connection.StoreConnection;
        if (dynamicUpdateCommand.Translator.CommandTimeout.HasValue)
          command.CommandTimeout = dynamicUpdateCommand.Translator.CommandTimeout.Value;
        int rowsAffected;
        if (dynamicUpdateCommand._modificationCommandTree.HasReader)
        {
          rowsAffected = 0;
          using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken).WithCurrentCulture<DbDataReader>())
          {
            if (await reader.ReadAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              ++rowsAffected;
              IBaseList<EdmMember> members = TypeHelpers.GetAllStructuralMembers((EdmType) dynamicUpdateCommand.CurrentValues.StructuralType);
              for (int ordinal = 0; ordinal < reader.FieldCount; ++ordinal)
              {
                EdmMember member = members[reader.GetName(ordinal)];
                bool flag = Helper.IsSpatialType(member.TypeUsage);
                if (flag)
                  flag = !await reader.IsDBNullAsync(ordinal, cancellationToken).WithCurrentCulture<bool>();
                object obj;
                if (flag)
                  obj = await SpatialHelpers.GetSpatialValueAsync(dynamicUpdateCommand.Translator.MetadataWorkspace, reader, member.TypeUsage, ordinal, cancellationToken).WithCurrentCulture<object>();
                else
                  obj = await reader.GetFieldValueAsync<object>(ordinal, cancellationToken).WithCurrentCulture<object>();
                int ordinal1 = members.IndexOf(member);
                PropagatorResult memberValue = dynamicUpdateCommand.CurrentValues.GetMemberValue(ordinal1);
                generatedValues.Add(new KeyValuePair<PropagatorResult, object>(memberValue, obj));
                int identifier = memberValue.Identifier;
                if (-1 != identifier)
                  identifierValues.Add(identifier, obj);
                member = (EdmMember) null;
              }
              members = (IBaseList<EdmMember>) null;
            }
            await CommandHelper.ConsumeReaderAsync(reader, cancellationToken).WithCurrentCulture();
          }
        }
        else
          rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken).WithCurrentCulture<int>();
        num = (long) rowsAffected;
      }
      return num;
    }

    protected virtual DbCommand CreateCommand(Dictionary<int, object> identifierValues)
    {
      DbModificationCommandTree modificationCommandTree = this._modificationCommandTree;
      if (this._inputIdentifiers != null)
      {
        Dictionary<DbSetClause, DbSetClause> clauseMappings = new Dictionary<DbSetClause, DbSetClause>();
        for (int index = 0; index < this._inputIdentifiers.Count; ++index)
        {
          KeyValuePair<int, DbSetClause> inputIdentifier = this._inputIdentifiers[index];
          object obj;
          if (identifierValues.TryGetValue(inputIdentifier.Key, out obj))
          {
            DbSetClause dbSetClause = new DbSetClause(inputIdentifier.Value.Property, (DbExpression) DbExpressionBuilder.Constant(obj));
            clauseMappings[inputIdentifier.Value] = dbSetClause;
            this._inputIdentifiers[index] = new KeyValuePair<int, DbSetClause>(inputIdentifier.Key, dbSetClause);
          }
        }
        modificationCommandTree = DynamicUpdateCommand.RebuildCommandTree(modificationCommandTree, clauseMappings);
      }
      return this.Translator.CreateCommand(modificationCommandTree);
    }

    private static DbModificationCommandTree RebuildCommandTree(
      DbModificationCommandTree originalTree,
      Dictionary<DbSetClause, DbSetClause> clauseMappings)
    {
      if (clauseMappings.Count == 0)
        return originalTree;
      DbModificationCommandTree modificationCommandTree;
      if (originalTree.CommandTreeKind == DbCommandTreeKind.Insert)
      {
        DbInsertCommandTree insertCommandTree = (DbInsertCommandTree) originalTree;
        modificationCommandTree = (DbModificationCommandTree) new DbInsertCommandTree(insertCommandTree.MetadataWorkspace, insertCommandTree.DataSpace, insertCommandTree.Target, new ReadOnlyCollection<DbModificationClause>((IList<DbModificationClause>) DynamicUpdateCommand.ReplaceClauses(insertCommandTree.SetClauses, clauseMappings)), insertCommandTree.Returning);
      }
      else
      {
        DbUpdateCommandTree updateCommandTree = (DbUpdateCommandTree) originalTree;
        modificationCommandTree = (DbModificationCommandTree) new DbUpdateCommandTree(updateCommandTree.MetadataWorkspace, updateCommandTree.DataSpace, updateCommandTree.Target, updateCommandTree.Predicate, new ReadOnlyCollection<DbModificationClause>((IList<DbModificationClause>) DynamicUpdateCommand.ReplaceClauses(updateCommandTree.SetClauses, clauseMappings)), updateCommandTree.Returning);
      }
      return modificationCommandTree;
    }

    private static List<DbModificationClause> ReplaceClauses(
      IList<DbModificationClause> originalClauses,
      Dictionary<DbSetClause, DbSetClause> mappings)
    {
      List<DbModificationClause> modificationClauseList = new List<DbModificationClause>(originalClauses.Count);
      for (int index = 0; index < originalClauses.Count; ++index)
      {
        DbSetClause dbSetClause;
        if (mappings.TryGetValue((DbSetClause) originalClauses[index], out dbSetClause))
          modificationClauseList.Add((DbModificationClause) dbSetClause);
        else
          modificationClauseList.Add(originalClauses[index]);
      }
      return modificationClauseList;
    }

    internal ModificationOperator Operator => this._operator;

    internal override EntitySet Table => this._processor.Table;

    internal override IEnumerable<int> InputIdentifiers
    {
      get
      {
        if (this._inputIdentifiers != null)
        {
          foreach (KeyValuePair<int, DbSetClause> inputIdentifier in this._inputIdentifiers)
            yield return inputIdentifier.Key;
        }
      }
    }

    internal override IEnumerable<int> OutputIdentifiers => this._outputIdentifiers == null ? Enumerable.Empty<int>() : (IEnumerable<int>) this._outputIdentifiers.Keys;

    internal override UpdateCommandKind Kind => UpdateCommandKind.Dynamic;

    internal override IList<IEntityStateEntry> GetStateEntries(
      UpdateTranslator translator)
    {
      List<IEntityStateEntry> entityStateEntryList = new List<IEntityStateEntry>(2);
      if (this.OriginalValues != null)
      {
        foreach (IEntityStateEntry allStateEntry in SourceInterpreter.GetAllStateEntries(this.OriginalValues, translator, this.Table))
          entityStateEntryList.Add(allStateEntry);
      }
      if (this.CurrentValues != null)
      {
        foreach (IEntityStateEntry allStateEntry in SourceInterpreter.GetAllStateEntries(this.CurrentValues, translator, this.Table))
          entityStateEntryList.Add(allStateEntry);
      }
      return (IList<IEntityStateEntry>) entityStateEntryList;
    }

    internal override int CompareToType(UpdateCommand otherCommand)
    {
      DynamicUpdateCommand dynamicUpdateCommand = (DynamicUpdateCommand) otherCommand;
      int num1 = (int) (this.Operator - dynamicUpdateCommand.Operator);
      if (num1 != 0)
        return num1;
      int num2 = StringComparer.Ordinal.Compare(this._processor.Table.Name, dynamicUpdateCommand._processor.Table.Name);
      if (num2 != 0)
        return num2;
      int num3 = StringComparer.Ordinal.Compare(this._processor.Table.EntityContainer.Name, dynamicUpdateCommand._processor.Table.EntityContainer.Name);
      if (num3 != 0)
        return num3;
      PropagatorResult propagatorResult1 = this.Operator == ModificationOperator.Delete ? this.OriginalValues : this.CurrentValues;
      PropagatorResult propagatorResult2 = dynamicUpdateCommand.Operator == ModificationOperator.Delete ? dynamicUpdateCommand.OriginalValues : dynamicUpdateCommand.CurrentValues;
      for (int index = 0; index < this._processor.KeyOrdinals.Length; ++index)
      {
        int keyOrdinal = this._processor.KeyOrdinals[index];
        object simpleValue1 = propagatorResult1.GetMemberValue(keyOrdinal).GetSimpleValue();
        object simpleValue2 = propagatorResult2.GetMemberValue(keyOrdinal).GetSimpleValue();
        num3 = ByValueComparer.Default.Compare(simpleValue1, simpleValue2);
        if (num3 != 0)
          return num3;
      }
      for (int index = 0; index < this._processor.KeyOrdinals.Length; ++index)
      {
        int keyOrdinal = this._processor.KeyOrdinals[index];
        num3 = propagatorResult1.GetMemberValue(keyOrdinal).Identifier - propagatorResult2.GetMemberValue(keyOrdinal).Identifier;
        if (num3 != 0)
          return num3;
      }
      return num3;
    }
  }
}
