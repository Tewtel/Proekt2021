// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MemberCollection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class MemberCollection : MetadataCollection<EdmMember>
  {
    private readonly StructuralType _declaringType;

    public MemberCollection(StructuralType declaringType)
      : this(declaringType, (IEnumerable<EdmMember>) null)
    {
    }

    public MemberCollection(StructuralType declaringType, IEnumerable<EdmMember> items)
      : base(items)
    {
      this._declaringType = declaringType;
    }

    public override ReadOnlyCollection<EdmMember> AsReadOnly => new ReadOnlyCollection<EdmMember>((IList<EdmMember>) this);

    public override int Count => this.GetBaseTypeMemberCount() + base.Count;

    public override EdmMember this[int index]
    {
      get
      {
        int relativeIndex = this.GetRelativeIndex(index);
        return relativeIndex < 0 ? ((StructuralType) this._declaringType.BaseType).Members[index] : base[relativeIndex];
      }
      set
      {
        int relativeIndex = this.GetRelativeIndex(index);
        if (relativeIndex < 0)
          ((StructuralType) this._declaringType.BaseType).Members.Source[index] = value;
        else
          base[relativeIndex] = value;
      }
    }

    public override void Add(EdmMember member)
    {
      this.ValidateMemberForAdd(member, nameof (member));
      base.Add(member);
      member.ChangeDeclaringTypeWithoutCollectionFixup(this._declaringType);
    }

    public override bool ContainsIdentity(string identity)
    {
      if (base.ContainsIdentity(identity))
        return true;
      EdmType baseType = this._declaringType.BaseType;
      return baseType != null && ((StructuralType) baseType).Members.Contains(identity);
    }

    public override int IndexOf(EdmMember item)
    {
      int num = base.IndexOf(item);
      if (num != -1)
        return num + this.GetBaseTypeMemberCount();
      return this._declaringType.BaseType is StructuralType baseType ? baseType.Members.IndexOf(item) : -1;
    }

    public override void CopyTo(EdmMember[] array, int arrayIndex)
    {
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex));
      int baseTypeMemberCount = this.GetBaseTypeMemberCount();
      if (base.Count + baseTypeMemberCount > array.Length - arrayIndex)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex));
      if (baseTypeMemberCount > 0)
        ((StructuralType) this._declaringType.BaseType).Members.CopyTo(array, arrayIndex);
      base.CopyTo(array, arrayIndex + baseTypeMemberCount);
    }

    public override bool TryGetValue(string identity, bool ignoreCase, out EdmMember item)
    {
      if (!base.TryGetValue(identity, ignoreCase, out item))
        ((StructuralType) this._declaringType.BaseType)?.Members.TryGetValue(identity, ignoreCase, out item);
      return item != null;
    }

    internal ReadOnlyMetadataCollection<T> GetDeclaredOnlyMembers<T>() where T : EdmMember
    {
      MetadataCollection<T> collection = new MetadataCollection<T>();
      for (int index = 0; index < base.Count; ++index)
      {
        if (base[index] is T obj1)
          collection.Add(obj1);
      }
      return new ReadOnlyMetadataCollection<T>(collection);
    }

    private int GetBaseTypeMemberCount() => this._declaringType.BaseType is StructuralType baseType ? baseType.Members.Count : 0;

    private int GetRelativeIndex(int index)
    {
      int baseTypeMemberCount = this.GetBaseTypeMemberCount();
      int count = base.Count;
      if (index < 0 || index >= baseTypeMemberCount + count)
        throw new ArgumentOutOfRangeException(nameof (index));
      return index - baseTypeMemberCount;
    }

    private void ValidateMemberForAdd(EdmMember member, string argumentName)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmMember>(member, argumentName);
      this._declaringType.ValidateMemberForAdd(member);
    }
  }
}
