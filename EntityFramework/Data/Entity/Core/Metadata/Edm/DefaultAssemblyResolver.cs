// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.DefaultAssemblyResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class DefaultAssemblyResolver : MetadataArtifactAssemblyResolver
  {
    internal override bool TryResolveAssemblyReference(
      AssemblyName referenceName,
      out Assembly assembly)
    {
      assembly = this.ResolveAssembly(referenceName);
      return assembly != (Assembly) null;
    }

    internal override IEnumerable<Assembly> GetWildcardAssemblies() => DefaultAssemblyResolver.GetAllDiscoverableAssemblies();

    internal virtual Assembly ResolveAssembly(AssemblyName referenceName)
    {
      Assembly assembly = (Assembly) null;
      foreach (Assembly nonSystemAssembly in DefaultAssemblyResolver.GetAlreadyLoadedNonSystemAssemblies())
      {
        if (AssemblyName.ReferenceMatchesDefinition(referenceName, new AssemblyName(nonSystemAssembly.FullName)))
          return nonSystemAssembly;
      }
      if (assembly == (Assembly) null)
      {
        assembly = MetadataAssemblyHelper.SafeLoadReferencedAssembly(referenceName);
        if (assembly != (Assembly) null)
          return assembly;
      }
      DefaultAssemblyResolver.TryFindWildcardAssemblyMatch(referenceName, out assembly);
      return assembly;
    }

    private static bool TryFindWildcardAssemblyMatch(
      AssemblyName referenceName,
      out Assembly assembly)
    {
      foreach (Assembly discoverableAssembly in DefaultAssemblyResolver.GetAllDiscoverableAssemblies())
      {
        if (AssemblyName.ReferenceMatchesDefinition(referenceName, new AssemblyName(discoverableAssembly.FullName)))
        {
          assembly = discoverableAssembly;
          return true;
        }
      }
      assembly = (Assembly) null;
      return false;
    }

    private static IEnumerable<Assembly> GetAlreadyLoadedNonSystemAssemblies() => ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (a => a != (Assembly) null && !MetadataAssemblyHelper.ShouldFilterAssembly(a)));

    private static IEnumerable<Assembly> GetAllDiscoverableAssemblies()
    {
      Assembly entryAssembly = Assembly.GetEntryAssembly();
      HashSet<Assembly> source = new HashSet<Assembly>((IEqualityComparer<Assembly>) DefaultAssemblyResolver.AssemblyComparer.Instance);
      foreach (Assembly nonSystemAssembly in DefaultAssemblyResolver.GetAlreadyLoadedNonSystemAssemblies())
        source.Add(nonSystemAssembly);
      AspProxy aspProxy = new AspProxy();
      if (!aspProxy.IsAspNetEnvironment())
      {
        if (entryAssembly == (Assembly) null)
          return (IEnumerable<Assembly>) source;
        source.Add(entryAssembly);
        foreach (Assembly referencedAssembly in MetadataAssemblyHelper.GetNonSystemReferencedAssemblies(entryAssembly))
          source.Add(referencedAssembly);
        return (IEnumerable<Assembly>) source;
      }
      if (aspProxy.HasBuildManagerType())
      {
        IEnumerable<Assembly> referencedAssemblies = aspProxy.GetBuildManagerReferencedAssemblies();
        if (referencedAssemblies != null)
        {
          foreach (Assembly assembly in referencedAssemblies)
          {
            if (!MetadataAssemblyHelper.ShouldFilterAssembly(assembly))
              source.Add(assembly);
          }
        }
      }
      return source.Where<Assembly>((Func<Assembly, bool>) (a => a != (Assembly) null));
    }

    internal sealed class AssemblyComparer : IEqualityComparer<Assembly>
    {
      private static readonly DefaultAssemblyResolver.AssemblyComparer _instance = new DefaultAssemblyResolver.AssemblyComparer();

      private AssemblyComparer()
      {
      }

      public static DefaultAssemblyResolver.AssemblyComparer Instance => DefaultAssemblyResolver.AssemblyComparer._instance;

      public bool Equals(Assembly x, Assembly y)
      {
        AssemblyName assemblyName1 = new AssemblyName(x.FullName);
        AssemblyName assemblyName2 = new AssemblyName(y.FullName);
        if ((object) x == (object) y)
          return true;
        return AssemblyName.ReferenceMatchesDefinition(assemblyName1, assemblyName2) && AssemblyName.ReferenceMatchesDefinition(assemblyName2, assemblyName1);
      }

      public int GetHashCode(Assembly assembly) => assembly.FullName.GetHashCode();
    }
  }
}
