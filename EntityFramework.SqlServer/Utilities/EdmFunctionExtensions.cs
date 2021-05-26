﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.EdmFunctionExtensions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal static class EdmFunctionExtensions
  {
    internal static bool IsCSpace(this EdmFunction function)
    {
      MetadataProperty metadataProperty = function.MetadataProperties.FirstOrDefault<MetadataProperty>((Func<MetadataProperty, bool>) (p => p.Name == "DataSpace"));
      return metadataProperty != null && (DataSpace) metadataProperty.Value == DataSpace.CSpace;
    }

    internal static bool IsCanonicalFunction(this EdmFunction function) => function.IsCSpace() && function.NamespaceName == "Edm";
  }
}
