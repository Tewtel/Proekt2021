// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.RelationshipEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Objects.Internal;
using System.Data.Entity.Resources;
using System.Diagnostics;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class RelationshipEntry : ObjectStateEntry
  {
    internal RelationshipWrapper _relationshipWrapper;

    internal EntityKey Key0 => this.RelationshipWrapper.Key0;

    internal EntityKey Key1 => this.RelationshipWrapper.Key1;

    internal override BitArray ModifiedProperties => (BitArray) null;

    internal RelationshipEntry(
      ObjectStateManager cache,
      EntityState state,
      RelationshipWrapper relationshipWrapper)
      : base(cache, (EntitySet) null, state)
    {
      this._entitySet = (EntitySetBase) relationshipWrapper.AssociationSet;
      this._relationshipWrapper = relationshipWrapper;
    }

    public override bool IsRelationship
    {
      get
      {
        this.ValidateState();
        return true;
      }
    }

    public override void AcceptChanges()
    {
      this.ValidateState();
      EntityState state = this.State;
      if (state <= EntityState.Added)
      {
        if (state == EntityState.Unchanged || state != EntityState.Added)
          return;
        this._cache.ChangeState(this, EntityState.Added, EntityState.Unchanged);
        this.State = EntityState.Unchanged;
      }
      else if (state == EntityState.Deleted)
      {
        this.DeleteUnnecessaryKeyEntries();
        if (this._cache == null)
          return;
        this._cache.ChangeState(this, EntityState.Deleted, EntityState.Detached);
      }
    }

    public override void Delete() => this.Delete(true);

    public override IEnumerable<string> GetModifiedProperties()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      RelationshipEntry relationshipEntry = this;
      if (num != 0)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      relationshipEntry.ValidateState();
      return false;
    }

    public override void SetModified()
    {
      this.ValidateState();
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationState);
    }

    public override object Entity
    {
      get
      {
        this.ValidateState();
        return (object) null;
      }
    }

    public override EntityKey EntityKey
    {
      get
      {
        this.ValidateState();
        return (EntityKey) null;
      }
      internal set
      {
      }
    }

    public override void SetModifiedProperty(string propertyName)
    {
      this.ValidateState();
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationState);
    }

    public override void RejectPropertyChanges(string propertyName)
    {
      this.ValidateState();
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationState);
    }

    public override bool IsPropertyChanged(string propertyName)
    {
      this.ValidateState();
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationState);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override DbDataRecord OriginalValues
    {
      get
      {
        this.ValidateState();
        if (this.State == EntityState.Added)
          throw new InvalidOperationException(Strings.ObjectStateEntry_OriginalValuesDoesNotExist);
        return (DbDataRecord) new ObjectStateEntryDbDataRecord(this);
      }
    }

    public override OriginalValueRecord GetUpdatableOriginalValues() => throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationValues);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override CurrentValueRecord CurrentValues
    {
      get
      {
        this.ValidateState();
        if (this.State == EntityState.Deleted)
          throw new InvalidOperationException(Strings.ObjectStateEntry_CurrentValuesDoesNotExist);
        return (CurrentValueRecord) new ObjectStateEntryDbUpdatableDataRecord(this);
      }
    }

    public override RelationshipManager RelationshipManager => throw new InvalidOperationException(Strings.ObjectStateEntry_RelationshipAndKeyEntriesDoNotHaveRelationshipManagers);

    public override void ChangeState(EntityState state)
    {
      EntityUtil.CheckValidStateForChangeRelationshipState(state, nameof (state));
      if (this.State == EntityState.Detached && state == EntityState.Detached)
        return;
      this.ValidateState();
      if (this.RelationshipWrapper.Key0 == this.Key0)
        this.ObjectStateManager.ChangeRelationshipState((object) this.Key0, (object) this.Key1, this.RelationshipWrapper.AssociationSet.ElementType.FullName, this.RelationshipWrapper.AssociationEndMembers[1].Name, state);
      else
        this.ObjectStateManager.ChangeRelationshipState((object) this.Key0, (object) this.Key1, this.RelationshipWrapper.AssociationSet.ElementType.FullName, this.RelationshipWrapper.AssociationEndMembers[0].Name, state);
    }

    public override void ApplyCurrentValues(object currentEntity)
    {
      System.Data.Entity.Utilities.Check.NotNull<object>(currentEntity, nameof (currentEntity));
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationValues);
    }

    public override void ApplyOriginalValues(object originalEntity)
    {
      System.Data.Entity.Utilities.Check.NotNull<object>(originalEntity, nameof (originalEntity));
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationValues);
    }

    internal override bool IsKeyEntry => false;

    internal override int GetFieldCount(StateManagerTypeMetadata metadata) => this._relationshipWrapper.AssociationEndMembers.Count;

    internal override DataRecordInfo GetDataRecordInfo(
      StateManagerTypeMetadata metadata,
      object userObject)
    {
      return new DataRecordInfo(TypeUsage.Create((EdmType) ((RelationshipSet) this.EntitySet).ElementType));
    }

    internal override void SetModifiedAll()
    {
      this.ValidateState();
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationState);
    }

    internal override Type GetFieldType(int ordinal, StateManagerTypeMetadata metadata) => typeof (EntityKey);

    internal override string GetCLayerName(int ordinal, StateManagerTypeMetadata metadata)
    {
      RelationshipEntry.ValidateRelationshipRange(ordinal);
      return this._relationshipWrapper.AssociationEndMembers[ordinal].Name;
    }

    internal override int GetOrdinalforCLayerName(string name, StateManagerTypeMetadata metadata)
    {
      ReadOnlyMetadataCollection<AssociationEndMember> associationEndMembers = this._relationshipWrapper.AssociationEndMembers;
      AssociationEndMember associationEndMember;
      return associationEndMembers.TryGetValue(name, false, out associationEndMember) ? associationEndMembers.IndexOf(associationEndMember) : -1;
    }

    internal override void RevertDelete()
    {
      this.State = EntityState.Unchanged;
      this._cache.ChangeState(this, EntityState.Deleted, this.State);
    }

    internal override void EntityMemberChanging(string entityMemberName) => throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationValues);

    internal override void EntityMemberChanged(string entityMemberName) => throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationValues);

    internal override void EntityComplexMemberChanging(
      string entityMemberName,
      object complexObject,
      string complexObjectMemberName)
    {
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationValues);
    }

    internal override void EntityComplexMemberChanged(
      string entityMemberName,
      object complexObject,
      string complexObjectMemberName)
    {
      throw new InvalidOperationException(Strings.ObjectStateEntry_CantModifyRelationValues);
    }

    internal bool IsSameAssociationSetAndRole(
      AssociationSet associationSet,
      AssociationEndMember associationMember,
      EntityKey entityKey)
    {
      if (this._entitySet != associationSet)
        return false;
      return this._relationshipWrapper.AssociationSet.ElementType.AssociationEndMembers[0].Name == associationMember.Name ? entityKey == this.Key0 : entityKey == this.Key1;
    }

    private object GetCurrentRelationValue(int ordinal, bool throwException)
    {
      RelationshipEntry.ValidateRelationshipRange(ordinal);
      this.ValidateState();
      if (this.State == EntityState.Deleted & throwException)
        throw new InvalidOperationException(Strings.ObjectStateEntry_CurrentValuesDoesNotExist);
      return (object) this._relationshipWrapper.GetEntityKey(ordinal);
    }

    private static void ValidateRelationshipRange(int ordinal)
    {
      if (1U < (uint) ordinal)
        throw new ArgumentOutOfRangeException(nameof (ordinal));
    }

    internal object GetCurrentRelationValue(int ordinal) => this.GetCurrentRelationValue(ordinal, true);

    internal RelationshipWrapper RelationshipWrapper
    {
      get => this._relationshipWrapper;
      set => this._relationshipWrapper = value;
    }

    internal override void Reset()
    {
      this._relationshipWrapper = (RelationshipWrapper) null;
      base.Reset();
    }

    internal void ChangeRelatedEnd(EntityKey oldKey, EntityKey newKey)
    {
      if (oldKey.Equals(this.Key0))
      {
        if (oldKey.Equals(this.Key1))
          this.RelationshipWrapper = new RelationshipWrapper(this.RelationshipWrapper.AssociationSet, newKey);
        else
          this.RelationshipWrapper = new RelationshipWrapper(this.RelationshipWrapper, 0, newKey);
      }
      else
        this.RelationshipWrapper = new RelationshipWrapper(this.RelationshipWrapper, 1, newKey);
    }

    internal void DeleteUnnecessaryKeyEntries()
    {
      for (int ordinal = 0; ordinal < 2; ++ordinal)
      {
        EntityKey currentRelationValue = this.GetCurrentRelationValue(ordinal, false) as EntityKey;
        EntityEntry entityEntry = this._cache.GetEntityEntry(currentRelationValue);
        if (entityEntry.IsKeyEntry)
        {
          bool flag = false;
          foreach (RelationshipEntry relationshipEntry in this._cache.FindRelationshipsByKey(currentRelationValue))
          {
            if (relationshipEntry != this)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            this._cache.DeleteKeyEntry(entityEntry);
            break;
          }
        }
      }
    }

    internal void Delete(bool doFixup)
    {
      this.ValidateState();
      if (doFixup)
      {
        if (this.State == EntityState.Deleted)
          return;
        EntityEntry entityEntry1 = this._cache.GetEntityEntry((EntityKey) this.GetCurrentRelationValue(0));
        IEntityWrapper wrappedEntity1 = entityEntry1.WrappedEntity;
        EntityEntry entityEntry2 = this._cache.GetEntityEntry((EntityKey) this.GetCurrentRelationValue(1));
        IEntityWrapper wrappedEntity2 = entityEntry2.WrappedEntity;
        if (wrappedEntity1.Entity != null && wrappedEntity2.Entity != null)
        {
          string name = this._relationshipWrapper.AssociationEndMembers[1].Name;
          string fullName = ((AssociationSet) this._entitySet).ElementType.FullName;
          wrappedEntity1.RelationshipManager.RemoveEntity(name, fullName, wrappedEntity2);
        }
        else
        {
          EntityKey entityKey;
          RelationshipManager relationshipManager;
          if (wrappedEntity1.Entity == null)
          {
            entityKey = entityEntry1.EntityKey;
            relationshipManager = wrappedEntity2.RelationshipManager;
          }
          else
          {
            entityKey = entityEntry2.EntityKey;
            relationshipManager = wrappedEntity1.RelationshipManager;
          }
          AssociationEndMember associationEndMember = this.RelationshipWrapper.GetAssociationEndMember(entityKey);
          ((EntityReference) relationshipManager.GetRelatedEndInternal(associationEndMember.DeclaringType.FullName, associationEndMember.Name)).DetachedEntityKey = (EntityKey) null;
          if (this.State == EntityState.Added)
          {
            this.DeleteUnnecessaryKeyEntries();
            this.DetachRelationshipEntry();
          }
          else
          {
            this._cache.ChangeState(this, this.State, EntityState.Deleted);
            this.State = EntityState.Deleted;
          }
        }
      }
      else
      {
        switch (this.State)
        {
          case EntityState.Unchanged:
            this._cache.ChangeState(this, EntityState.Unchanged, EntityState.Deleted);
            this.State = EntityState.Deleted;
            break;
          case EntityState.Added:
            this.DeleteUnnecessaryKeyEntries();
            this.DetachRelationshipEntry();
            break;
        }
      }
    }

    internal object GetOriginalRelationValue(int ordinal) => this.GetCurrentRelationValue(ordinal, false);

    internal void DetachRelationshipEntry()
    {
      if (this._cache == null)
        return;
      this._cache.ChangeState(this, this.State, EntityState.Detached);
    }

    internal void ChangeRelationshipState(
      EntityEntry targetEntry,
      RelatedEnd relatedEnd,
      EntityState requestedState)
    {
      switch (this.State)
      {
        case EntityState.Unchanged:
          switch (requestedState)
          {
            case EntityState.Detached:
              this.Delete();
              this.AcceptChanges();
              return;
            case EntityState.Unchanged:
              return;
            case EntityState.Detached | EntityState.Unchanged:
              return;
            case EntityState.Added:
              this.ObjectStateManager.ChangeState(this, EntityState.Unchanged, EntityState.Added);
              this.State = EntityState.Added;
              return;
            case EntityState.Deleted:
              this.Delete();
              return;
            default:
              return;
          }
        case EntityState.Added:
          switch (requestedState)
          {
            case EntityState.Detached:
              this.Delete();
              return;
            case EntityState.Unchanged:
              this.AcceptChanges();
              return;
            case EntityState.Detached | EntityState.Unchanged:
              return;
            case EntityState.Added:
              return;
            case EntityState.Deleted:
              this.AcceptChanges();
              this.Delete();
              return;
            default:
              return;
          }
        case EntityState.Deleted:
          switch (requestedState)
          {
            case EntityState.Detached:
              this.AcceptChanges();
              return;
            case EntityState.Unchanged:
              relatedEnd.Add(targetEntry.WrappedEntity, true, false, true, false, true);
              this.ObjectStateManager.ChangeState(this, EntityState.Deleted, EntityState.Unchanged);
              this.State = EntityState.Unchanged;
              return;
            case EntityState.Detached | EntityState.Unchanged:
              return;
            case EntityState.Added:
              relatedEnd.Add(targetEntry.WrappedEntity, true, false, true, false, true);
              this.ObjectStateManager.ChangeState(this, EntityState.Deleted, EntityState.Added);
              this.State = EntityState.Added;
              return;
            default:
              return;
          }
      }
    }

    internal RelationshipEntry GetNextRelationshipEnd(EntityKey entityKey) => !entityKey.Equals(this.Key0) ? this.NextKey1 : this.NextKey0;

    internal void SetNextRelationshipEnd(EntityKey entityKey, RelationshipEntry nextEnd)
    {
      if (entityKey.Equals(this.Key0))
        this.NextKey0 = nextEnd;
      else
        this.NextKey1 = nextEnd;
    }

    internal RelationshipEntry NextKey0 { get; set; }

    internal RelationshipEntry NextKey1 { get; set; }
  }
}
