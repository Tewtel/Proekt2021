// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.RelationshipType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Represents the Relationship type</summary>
  public abstract class RelationshipType : EntityTypeBase
  {
    private ReadOnlyMetadataCollection<RelationshipEndMember> _relationshipEndMembers;

    internal RelationshipType(string name, string namespaceName, DataSpace dataSpace)
      : base(name, namespaceName, dataSpace)
    {
    }

    /// <summary>Gets the list of ends for this relationship type. </summary>
    /// <returns>
    /// A collection of type <see cref="T:System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1" /> that contains the list of Ends for this relationship type.
    /// </returns>
    public ReadOnlyMetadataCollection<RelationshipEndMember> RelationshipEndMembers
    {
      get
      {
        if (this._relationshipEndMembers == null)
          Interlocked.CompareExchange<ReadOnlyMetadataCollection<RelationshipEndMember>>(ref this._relationshipEndMembers, (ReadOnlyMetadataCollection<RelationshipEndMember>) new FilteredReadOnlyMetadataCollection<RelationshipEndMember, EdmMember>(this.Members, new Predicate<EdmMember>(Helper.IsRelationshipEndMember)), (ReadOnlyMetadataCollection<RelationshipEndMember>) null);
        return this._relationshipEndMembers;
      }
    }
  }
}
