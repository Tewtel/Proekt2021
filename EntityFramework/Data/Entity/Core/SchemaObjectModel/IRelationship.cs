﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.IRelationship
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal interface IRelationship
  {
    string Name { get; }

    string FQName { get; }

    IList<IRelationshipEnd> Ends { get; }

    IList<ReferentialConstraint> Constraints { get; }

    bool TryGetEnd(string roleName, out IRelationshipEnd end);

    RelationshipKind RelationshipKind { get; }

    bool IsForeignKey { get; }
  }
}
