// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.StructuralType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Represents the Structural Type</summary>
  public abstract class StructuralType : EdmType
  {
    private readonly MemberCollection _members;
    private readonly ReadOnlyMetadataCollection<EdmMember> _readOnlyMembers;

    internal StructuralType()
    {
      this._members = new MemberCollection(this);
      this._readOnlyMembers = this._members.AsReadOnlyMetadataCollection();
    }

    internal StructuralType(string name, string namespaceName, DataSpace dataSpace)
      : base(name, namespaceName, dataSpace)
    {
      this._members = new MemberCollection(this);
      this._readOnlyMembers = this._members.AsReadOnlyMetadataCollection();
    }

    /// <summary>Gets the list of members on this type.</summary>
    /// <returns>
    /// A collection of type <see cref="T:System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1" /> that contains a set of members on this type.
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.EdmMember, true)]
    public ReadOnlyMetadataCollection<EdmMember> Members => this._readOnlyMembers;

    internal ReadOnlyMetadataCollection<T> GetDeclaredOnlyMembers<T>() where T : EdmMember => this._members.GetDeclaredOnlyMembers<T>();

    internal override void SetReadOnly()
    {
      if (this.IsReadOnly)
        return;
      base.SetReadOnly();
      this.Members.Source.SetReadOnly();
    }

    internal abstract void ValidateMemberForAdd(EdmMember member);

    /// <summary>Adds a member to this type</summary>
    /// <param name="member"> The member to add </param>
    public void AddMember(EdmMember member) => this.AddMember(member, false);

    internal void AddMember(EdmMember member, bool forceAdd)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmMember>(member, nameof (member));
      if (!forceAdd)
        Util.ThrowIfReadOnly((MetadataItem) this);
      if (this.DataSpace != member.TypeUsage.EdmType.DataSpace && this.BuiltInTypeKind != BuiltInTypeKind.RowType)
        throw new ArgumentException(Strings.AttemptToAddEdmMemberFromWrongDataSpace((object) member.Name, (object) this.Name, (object) member.TypeUsage.EdmType.DataSpace, (object) this.DataSpace), nameof (member));
      if (BuiltInTypeKind.RowType == this.BuiltInTypeKind)
      {
        if (this._members.Count == 0)
          this.DataSpace = member.TypeUsage.EdmType.DataSpace;
        else if (this.DataSpace != ~DataSpace.OSpace && member.TypeUsage.EdmType.DataSpace != this.DataSpace)
          this.DataSpace = ~DataSpace.OSpace;
      }
      if (this._members.IsReadOnly & forceAdd)
      {
        this._members.ResetReadOnly();
        this._members.Add(member);
        this._members.SetReadOnly();
      }
      else
        this._members.Add(member);
    }

    /// <summary>Removes a member from this type.</summary>
    /// <param name="member">The member to remove.</param>
    public virtual void RemoveMember(EdmMember member)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmMember>(member, nameof (member));
      Util.ThrowIfReadOnly((MetadataItem) this);
      this._members.Remove(member);
    }

    internal virtual bool HasMember(EdmMember member) => this._members.Contains(member);

    internal virtual void NotifyItemIdentityChanged(EdmMember item, string initialIdentity) => this._members.HandleIdentityChange(item, initialIdentity);
  }
}
