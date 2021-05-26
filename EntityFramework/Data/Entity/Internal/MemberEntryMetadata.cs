// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.MemberEntryMetadata
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Internal
{
  internal abstract class MemberEntryMetadata
  {
    private readonly Type _declaringType;
    private readonly Type _elementType;
    private readonly string _memberName;

    protected MemberEntryMetadata(Type declaringType, Type elementType, string memberName)
    {
      this._declaringType = declaringType;
      this._elementType = elementType;
      this._memberName = memberName;
    }

    public abstract InternalMemberEntry CreateMemberEntry(
      InternalEntityEntry internalEntityEntry,
      InternalPropertyEntry parentPropertyEntry);

    public abstract MemberEntryType MemberEntryType { get; }

    public string MemberName => this._memberName;

    public Type DeclaringType => this._declaringType;

    public Type ElementType => this._elementType;

    public abstract Type MemberType { get; }
  }
}
