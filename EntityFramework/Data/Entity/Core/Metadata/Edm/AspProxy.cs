// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.AspProxy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.SchemaObjectModel;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;
using System.Security;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class AspProxy
  {
    private const string BUILD_MANAGER_TYPE_NAME = "System.Web.Compilation.BuildManager";
    private const string AspNetAssemblyName = "System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
    private static readonly byte[] _systemWebPublicKeyToken = ScalarType.ConvertToByteArray("b03f5f7f11d50a3a");
    private Assembly _webAssembly;
    private bool _triedLoadingWebAssembly;

    internal bool IsAspNetEnvironment()
    {
      if (!this.TryInitializeWebAssembly())
        return false;
      try
      {
        return this.InternalMapWebPath("~") != null;
      }
      catch (SecurityException ex)
      {
        return false;
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          return false;
        throw;
      }
    }

    public bool TryInitializeWebAssembly()
    {
      if (this._webAssembly != (Assembly) null)
        return true;
      if (this._triedLoadingWebAssembly)
        return false;
      this._triedLoadingWebAssembly = true;
      if (!AspProxy.IsSystemWebLoaded())
        return false;
      try
      {
        this._webAssembly = Assembly.Load("System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        return this._webAssembly != (Assembly) null;
      }
      catch (Exception ex)
      {
        if (!ex.IsCatchableExceptionType())
          throw;
      }
      return false;
    }

    public static bool IsSystemWebLoaded()
    {
      try
      {
        return ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Any<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == "System.Web" && a.GetName().GetPublicKeyToken() != null && ((IEnumerable<byte>) a.GetName().GetPublicKeyToken()).SequenceEqual<byte>((IEnumerable<byte>) AspProxy._systemWebPublicKeyToken)));
      }
      catch
      {
      }
      return false;
    }

    private void InitializeWebAssembly()
    {
      if (!this.TryInitializeWebAssembly())
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext);
    }

    internal string MapWebPath(string path)
    {
      path = this.InternalMapWebPath(path);
      return path != null ? path : throw new InvalidOperationException(System.Data.Entity.Resources.Strings.InvalidUseOfWebPath((object) "~"));
    }

    internal string InternalMapWebPath(string path)
    {
      this.InitializeWebAssembly();
      try
      {
        return (string) this._webAssembly.GetType("System.Web.Hosting.HostingEnvironment", true).GetDeclaredMethod("MapPath", typeof (string)).Invoke((object) null, new object[1]
        {
          (object) path
        });
      }
      catch (TargetException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
      catch (ArgumentException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
      catch (TargetInvocationException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
      catch (TargetParameterCountException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
      catch (MethodAccessException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
      catch (MemberAccessException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
      catch (TypeLoadException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
    }

    internal bool HasBuildManagerType() => this.TryGetBuildManagerType(out Type _);

    private bool TryGetBuildManagerType(out Type buildManager)
    {
      this.InitializeWebAssembly();
      buildManager = this._webAssembly.GetType("System.Web.Compilation.BuildManager", false);
      return buildManager != (Type) null;
    }

    internal IEnumerable<Assembly> GetBuildManagerReferencedAssemblies()
    {
      MethodInfo assembliesMethod = this.GetReferencedAssembliesMethod();
      if (assembliesMethod == (MethodInfo) null)
        return (IEnumerable<Assembly>) new List<Assembly>();
      try
      {
        ICollection source = (ICollection) assembliesMethod.Invoke((object) null, (object[]) null);
        return source == null ? (IEnumerable<Assembly>) new List<Assembly>() : source.Cast<Assembly>();
      }
      catch (TargetException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
      catch (TargetInvocationException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
      catch (MethodAccessException ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToDetermineApplicationContext, (Exception) ex);
      }
    }

    internal MethodInfo GetReferencedAssembliesMethod()
    {
      Type buildManager;
      if (!this.TryGetBuildManagerType(out buildManager))
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.UnableToFindReflectedType((object) "System.Web.Compilation.BuildManager", (object) "System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"));
      return buildManager.GetDeclaredMethod("GetReferencedAssemblies");
    }
  }
}
