// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.NavigationEntryMetadata
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Internal
{
  internal class NavigationEntryMetadata : MemberEntryMetadata
  {
    private readonly bool _isCollection;

    public NavigationEntryMetadata(
      Type declaringType,
      Type propertyType,
      string propertyName,
      bool isCollection)
      : base(declaringType, propertyType, propertyName)
    {
      this._isCollection = isCollection;
    }

    public override MemberEntryType MemberEntryType => !this._isCollection ? MemberEntryType.ReferenceNavigationProperty : MemberEntryType.CollectionNavigationProperty;

    public override Type MemberType => !this._isCollection ? this.ElementType : DbHelpers.CollectionType(this.ElementType);

    public override InternalMemberEntry CreateMemberEntry(
      InternalEntityEntry internalEntityEntry,
      InternalPropertyEntry parentPropertyEntry)
    {
      return !this._isCollection ? (InternalMemberEntry) new InternalReferenceEntry(internalEntityEntry, this) : (InternalMemberEntry) new InternalCollectionEntry(internalEntityEntry, this);
    }
  }
}
