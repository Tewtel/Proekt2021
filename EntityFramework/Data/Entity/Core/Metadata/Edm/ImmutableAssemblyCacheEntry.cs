﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ImmutableAssemblyCacheEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class ImmutableAssemblyCacheEntry : AssemblyCacheEntry
  {
    private readonly ReadOnlyCollection<EdmType> _typesInAssembly;
    private readonly ReadOnlyCollection<Assembly> _closureAssemblies;

    internal ImmutableAssemblyCacheEntry(MutableAssemblyCacheEntry mutableEntry)
    {
      this._typesInAssembly = new ReadOnlyCollection<EdmType>((IList<EdmType>) new List<EdmType>((IEnumerable<EdmType>) mutableEntry.TypesInAssembly));
      this._closureAssemblies = new ReadOnlyCollection<Assembly>((IList<Assembly>) new List<Assembly>((IEnumerable<Assembly>) mutableEntry.ClosureAssemblies));
    }

    internal override IList<EdmType> TypesInAssembly => (IList<EdmType>) this._typesInAssembly;

    internal override IList<Assembly> ClosureAssemblies => (IList<Assembly>) this._closureAssemblies;
  }
}
