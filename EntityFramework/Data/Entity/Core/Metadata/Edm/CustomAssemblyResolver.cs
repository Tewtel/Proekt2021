// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.CustomAssemblyResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class CustomAssemblyResolver : MetadataArtifactAssemblyResolver
  {
    private readonly Func<AssemblyName, Assembly> _referenceResolver;
    private readonly Func<IEnumerable<Assembly>> _wildcardAssemblyEnumerator;

    internal CustomAssemblyResolver(
      Func<IEnumerable<Assembly>> wildcardAssemblyEnumerator,
      Func<AssemblyName, Assembly> referenceResolver)
    {
      this._wildcardAssemblyEnumerator = wildcardAssemblyEnumerator;
      this._referenceResolver = referenceResolver;
    }

    internal override bool TryResolveAssemblyReference(
      AssemblyName referenceName,
      out Assembly assembly)
    {
      assembly = this._referenceResolver(referenceName);
      return assembly != (Assembly) null;
    }

    internal override IEnumerable<Assembly> GetWildcardAssemblies() => this._wildcardAssemblyEnumerator() ?? throw new InvalidOperationException(Strings.WildcardEnumeratorReturnedNull);
  }
}
