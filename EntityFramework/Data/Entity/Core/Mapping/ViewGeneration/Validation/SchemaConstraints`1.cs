// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Validation.SchemaConstraints`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Validation
{
  internal class SchemaConstraints<TKeyConstraint> : InternalBase where TKeyConstraint : InternalBase
  {
    private readonly List<TKeyConstraint> m_keyConstraints;

    internal SchemaConstraints() => this.m_keyConstraints = new List<TKeyConstraint>();

    internal IEnumerable<TKeyConstraint> KeyConstraints => (IEnumerable<TKeyConstraint>) this.m_keyConstraints;

    internal void Add(TKeyConstraint constraint) => this.m_keyConstraints.Add(constraint);

    private static void ConstraintsToBuilder<Constraint>(
      IEnumerable<Constraint> constraints,
      StringBuilder builder)
      where Constraint : InternalBase
    {
      foreach (Constraint constraint in constraints)
      {
        constraint.ToCompactString(builder);
        builder.Append(Environment.NewLine);
      }
    }

    internal override void ToCompactString(StringBuilder builder) => SchemaConstraints<TKeyConstraint>.ConstraintsToBuilder<TKeyConstraint>((IEnumerable<TKeyConstraint>) this.m_keyConstraints, builder);
  }
}
