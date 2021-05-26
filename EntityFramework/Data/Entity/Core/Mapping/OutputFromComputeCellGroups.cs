// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.OutputFromComputeCellGroups
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Data.Entity.Core.Mapping.ViewGeneration.Validation;

namespace System.Data.Entity.Core.Mapping
{
  internal struct OutputFromComputeCellGroups
  {
    internal List<Cell> Cells;
    internal CqlIdentifiers Identifiers;
    internal List<Set<Cell>> CellGroups;
    internal List<ForeignConstraint> ForeignKeyConstraints;
    internal bool Success;
  }
}
