﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ObjectItemNoOpAssemblyLoader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class ObjectItemNoOpAssemblyLoader : ObjectItemAssemblyLoader
  {
    internal ObjectItemNoOpAssemblyLoader(
      Assembly assembly,
      ObjectItemLoadingSessionData sessionData)
      : base(assembly, (AssemblyCacheEntry) new MutableAssemblyCacheEntry(), sessionData)
    {
    }

    internal override void Load()
    {
      if (this.SessionData.KnownAssemblies.Contains(this.SourceAssembly, (object) this.SessionData.ObjectItemAssemblyLoaderFactory, this.SessionData.EdmItemCollection))
        return;
      this.AddToKnownAssemblies();
    }

    protected override void AddToAssembliesLoaded() => throw new NotImplementedException();

    protected override void LoadTypesFromAssembly() => throw new NotImplementedException();
  }
}
