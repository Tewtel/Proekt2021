// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.ForeignKeyConstraint
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class ForeignKeyConstraint
  {
    private readonly ExtentPair m_extentPair;
    private readonly List<string> m_parentKeys;
    private readonly List<string> m_childKeys;
    private readonly ReferentialConstraint m_constraint;
    private Dictionary<string, string> m_keyMap;

    internal List<string> ParentKeys => this.m_parentKeys;

    internal List<string> ChildKeys => this.m_childKeys;

    internal ExtentPair Pair => this.m_extentPair;

    internal RelationshipMultiplicity ChildMultiplicity => this.m_constraint.ToRole.RelationshipMultiplicity;

    internal bool GetParentProperty(string childPropertyName, out string parentPropertyName)
    {
      this.BuildKeyMap();
      return this.m_keyMap.TryGetValue(childPropertyName, out parentPropertyName);
    }

    internal ForeignKeyConstraint(RelationshipSet relationshipSet, ReferentialConstraint constraint)
    {
      AssociationSet associationSet = relationshipSet as AssociationSet;
      AssociationEndMember fromRole = constraint.FromRole as AssociationEndMember;
      AssociationEndMember toRole = constraint.ToRole as AssociationEndMember;
      if (associationSet == null || fromRole == null || toRole == null)
        throw new NotSupportedException();
      this.m_constraint = constraint;
      this.m_extentPair = new ExtentPair((EntitySetBase) MetadataHelper.GetEntitySetAtEnd(associationSet, fromRole), (EntitySetBase) MetadataHelper.GetEntitySetAtEnd(associationSet, toRole));
      this.m_childKeys = new List<string>();
      foreach (EdmMember toProperty in constraint.ToProperties)
        this.m_childKeys.Add(toProperty.Name);
      this.m_parentKeys = new List<string>();
      foreach (EdmMember fromProperty in constraint.FromProperties)
        this.m_parentKeys.Add(fromProperty.Name);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(fromRole.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne || RelationshipMultiplicity.One == fromRole.RelationshipMultiplicity, "from-end of relationship constraint cannot have multiplicity greater than 1");
    }

    private void BuildKeyMap()
    {
      if (this.m_keyMap != null)
        return;
      this.m_keyMap = new Dictionary<string, string>();
      IEnumerator<EdmProperty> enumerator1 = (IEnumerator<EdmProperty>) this.m_constraint.FromProperties.GetEnumerator();
      IEnumerator<EdmProperty> enumerator2 = (IEnumerator<EdmProperty>) this.m_constraint.ToProperties.GetEnumerator();
      while (true)
      {
        int num = !enumerator1.MoveNext() ? 1 : 0;
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(num == (!enumerator2.MoveNext() ? 1 : 0), "key count mismatch");
        if (num == 0)
          this.m_keyMap[enumerator2.Current.Name] = enumerator1.Current.Name;
        else
          break;
      }
    }
  }
}
