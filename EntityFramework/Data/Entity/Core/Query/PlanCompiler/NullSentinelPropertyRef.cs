// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.NullSentinelPropertyRef
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class NullSentinelPropertyRef : PropertyRef
  {
    private static readonly NullSentinelPropertyRef _singleton = new NullSentinelPropertyRef();

    private NullSentinelPropertyRef()
    {
    }

    internal static NullSentinelPropertyRef Instance => NullSentinelPropertyRef._singleton;

    public override string ToString() => "NULLSENTINEL";
  }
}
