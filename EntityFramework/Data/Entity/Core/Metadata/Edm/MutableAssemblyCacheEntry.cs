// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MutableAssemblyCacheEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class MutableAssemblyCacheEntry : AssemblyCacheEntry
  {
    private readonly List<EdmType> _typesInAssembly = new List<EdmType>();
    private readonly List<Assembly> _closureAssemblies = new List<Assembly>();

    internal override IList<EdmType> TypesInAssembly => (IList<EdmType>) this._typesInAssembly;

    internal override IList<Assembly> ClosureAssemblies => (IList<Assembly>) this._closureAssemblies;
  }
}
