// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalMemberEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Internal.Validation;
using System.Data.Entity.Validation;
using System.Linq;

namespace System.Data.Entity.Internal
{
  internal abstract class InternalMemberEntry
  {
    private readonly InternalEntityEntry _internalEntityEntry;
    private readonly MemberEntryMetadata _memberMetadata;

    protected InternalMemberEntry(
      InternalEntityEntry internalEntityEntry,
      MemberEntryMetadata memberMetadata)
    {
      this._internalEntityEntry = internalEntityEntry;
      this._memberMetadata = memberMetadata;
    }

    public virtual string Name => this._memberMetadata.MemberName;

    public abstract object CurrentValue { get; set; }

    public virtual InternalEntityEntry InternalEntityEntry => this._internalEntityEntry;

    public virtual MemberEntryMetadata EntryMetadata => this._memberMetadata;

    public virtual IEnumerable<DbValidationError> GetValidationErrors()
    {
      ValidationProvider validationProvider = this.InternalEntityEntry.InternalContext.ValidationProvider;
      PropertyValidator propertyValidator = validationProvider.GetPropertyValidator(this._internalEntityEntry, this);
      return propertyValidator == null ? Enumerable.Empty<DbValidationError>() : propertyValidator.Validate(validationProvider.GetEntityValidationContext(this._internalEntityEntry, (IDictionary<object, object>) null), this);
    }

    public abstract DbMemberEntry CreateDbMemberEntry();

    public abstract DbMemberEntry<TEntity, TProperty> CreateDbMemberEntry<TEntity, TProperty>() where TEntity : class;
  }
}
