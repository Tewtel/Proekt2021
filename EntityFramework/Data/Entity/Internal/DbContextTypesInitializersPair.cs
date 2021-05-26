// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DbContextTypesInitializersPair
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Internal
{
  internal class DbContextTypesInitializersPair : 
    Tuple<Dictionary<Type, List<string>>, Action<DbContext>>
  {
    public DbContextTypesInitializersPair(
      Dictionary<Type, List<string>> entityTypeToPropertyNameMap,
      Action<DbContext> setsInitializer)
      : base(entityTypeToPropertyNameMap, setsInitializer)
    {
    }

    public Dictionary<Type, List<string>> EntityTypeToPropertyNameMap => this.Item1;

    public Action<DbContext> SetsInitializer => this.Item2;
  }
}
