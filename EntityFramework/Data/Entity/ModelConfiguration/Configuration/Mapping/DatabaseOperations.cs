﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Mapping.DatabaseOperations
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Configuration.Mapping
{
  internal static class DatabaseOperations
  {
    public static void AddTypeConstraint(
      EdmModel database,
      EntityType entityType,
      EntityType principalTable,
      EntityType dependentTable,
      bool isSplitting)
    {
      ForeignKeyBuilder foreignKeyBuilder = new ForeignKeyBuilder(database, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}_TypeConstraint_From_{1}_To_{2}", (object) entityType.Name, (object) principalTable.Name, (object) dependentTable.Name))
      {
        PrincipalTable = principalTable
      };
      dependentTable.AddForeignKey(foreignKeyBuilder);
      if (isSplitting)
        foreignKeyBuilder.SetIsSplitConstraint();
      else
        foreignKeyBuilder.SetIsTypeConstraint();
      foreignKeyBuilder.DependentColumns = dependentTable.Properties.Where<EdmProperty>((Func<EdmProperty, bool>) (c => c.IsPrimaryKeyColumn));
      dependentTable.Properties.Where<EdmProperty>((Func<EdmProperty, bool>) (c => c.IsPrimaryKeyColumn)).Each<EdmProperty>((Action<EdmProperty>) (c => c.RemoveStoreGeneratedIdentityPattern()));
    }
  }
}
