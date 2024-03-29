﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalNavigationEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Internal
{
  internal abstract class InternalNavigationEntry : InternalMemberEntry
  {
    private IRelatedEnd _relatedEnd;
    private Func<object, object> _getter;
    private bool _triedToGetGetter;
    private Action<object, object> _setter;
    private bool _triedToGetSetter;

    protected InternalNavigationEntry(
      InternalEntityEntry internalEntityEntry,
      NavigationEntryMetadata navigationMetadata)
      : base(internalEntityEntry, (MemberEntryMetadata) navigationMetadata)
    {
    }

    public virtual void Load()
    {
      this.ValidateNotDetached(nameof (Load));
      this._relatedEnd.Load();
    }

    public virtual Task LoadAsync(CancellationToken cancellationToken)
    {
      this.ValidateNotDetached(nameof (LoadAsync));
      return this._relatedEnd.LoadAsync(cancellationToken);
    }

    public virtual bool IsLoaded
    {
      get
      {
        this.ValidateNotDetached(nameof (IsLoaded));
        return this._relatedEnd.IsLoaded;
      }
      set
      {
        this.ValidateNotDetached(nameof (IsLoaded));
        this._relatedEnd.IsLoaded = value;
      }
    }

    public virtual IQueryable Query()
    {
      this.ValidateNotDetached(nameof (Query));
      return (IQueryable) this._relatedEnd.CreateSourceQuery();
    }

    protected IRelatedEnd RelatedEnd
    {
      get
      {
        if (this._relatedEnd == null && !this.InternalEntityEntry.IsDetached)
          this._relatedEnd = this.InternalEntityEntry.GetRelatedEnd(this.Name);
        return this._relatedEnd;
      }
    }

    public override object CurrentValue
    {
      get
      {
        if (this.Getter != null)
          return this.Getter(this.InternalEntityEntry.Entity);
        this.ValidateNotDetached(nameof (CurrentValue));
        return this.GetNavigationPropertyFromRelatedEnd(this.InternalEntityEntry.Entity);
      }
    }

    protected Func<object, object> Getter
    {
      get
      {
        if (!this._triedToGetGetter)
        {
          DbHelpers.GetPropertyGetters(this.InternalEntityEntry.EntityType).TryGetValue(this.Name, out this._getter);
          this._triedToGetGetter = true;
        }
        return this._getter;
      }
    }

    protected Action<object, object> Setter
    {
      get
      {
        if (!this._triedToGetSetter)
        {
          DbHelpers.GetPropertySetters(this.InternalEntityEntry.EntityType).TryGetValue(this.Name, out this._setter);
          this._triedToGetSetter = true;
        }
        return this._setter;
      }
    }

    protected abstract object GetNavigationPropertyFromRelatedEnd(object entity);

    private void ValidateNotDetached(string method)
    {
      if (this._relatedEnd != null)
        return;
      this._relatedEnd = !this.InternalEntityEntry.IsDetached ? this.InternalEntityEntry.GetRelatedEnd(this.Name) : throw System.Data.Entity.Resources.Error.DbPropertyEntry_NotSupportedForDetached((object) method, (object) this.Name, (object) this.InternalEntityEntry.EntityType.Name);
    }
  }
}
