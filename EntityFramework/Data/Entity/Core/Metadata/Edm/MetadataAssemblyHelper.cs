// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MetadataAssemblyHelper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.SchemaObjectModel;
using System.IO;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal static class MetadataAssemblyHelper
  {
    private const string EcmaPublicKey = "b77a5c561934e089";
    private const string MicrosoftPublicKey = "b03f5f7f11d50a3a";
    private static readonly byte[] _ecmaPublicKeyToken = ScalarType.ConvertToByteArray("b77a5c561934e089");
    private static readonly byte[] _msPublicKeyToken = ScalarType.ConvertToByteArray("b03f5f7f11d50a3a");
    private static readonly Memoizer<Assembly, bool> _filterAssemblyCacheByAssembly = new Memoizer<Assembly, bool>(new Func<Assembly, bool>(MetadataAssemblyHelper.ComputeShouldFilterAssembly), (IEqualityComparer<Assembly>) EqualityComparer<Assembly>.Default);

    internal static Assembly SafeLoadReferencedAssembly(AssemblyName assemblyName)
    {
      Assembly assembly = (Assembly) null;
      try
      {
        assembly = Assembly.Load(assemblyName);
      }
      catch (FileNotFoundException ex)
      {
      }
      catch (FileLoadException ex)
      {
      }
      return assembly;
    }

    private static bool ComputeShouldFilterAssembly(Assembly assembly) => MetadataAssemblyHelper.ShouldFilterAssembly(new AssemblyName(assembly.FullName));

    internal static bool ShouldFilterAssembly(Assembly assembly) => MetadataAssemblyHelper._filterAssemblyCacheByAssembly.Evaluate(assembly);

    private static bool ShouldFilterAssembly(AssemblyName assemblyName) => MetadataAssemblyHelper.ArePublicKeyTokensEqual(assemblyName.GetPublicKeyToken(), MetadataAssemblyHelper._ecmaPublicKeyToken) || MetadataAssemblyHelper.ArePublicKeyTokensEqual(assemblyName.GetPublicKeyToken(), MetadataAssemblyHelper._msPublicKeyToken);

    private static bool ArePublicKeyTokensEqual(byte[] left, byte[] right)
    {
      if (left.Length != right.Length)
        return false;
      for (int index = 0; index < left.Length; ++index)
      {
        if ((int) left[index] != (int) right[index])
          return false;
      }
      return true;
    }

    internal static IEnumerable<Assembly> GetNonSystemReferencedAssemblies(
      Assembly assembly)
    {
      AssemblyName[] assemblyNameArray = assembly.GetReferencedAssemblies();
      for (int index = 0; index < assemblyNameArray.Length; ++index)
      {
        AssemblyName assemblyName = assemblyNameArray[index];
        if (!MetadataAssemblyHelper.ShouldFilterAssembly(assemblyName))
        {
          Assembly assembly1 = MetadataAssemblyHelper.SafeLoadReferencedAssembly(assemblyName);
          if (assembly1 != (Assembly) null)
            yield return assembly1;
        }
      }
      assemblyNameArray = (AssemblyName[]) null;
    }
  }
}
