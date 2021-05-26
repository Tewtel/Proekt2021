// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.SimpleType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Class representing a simple type</summary>
  public abstract class SimpleType : EdmType
  {
    internal SimpleType()
    {
    }

    internal SimpleType(string name, string namespaceName, DataSpace dataSpace)
      : base(name, namespaceName, dataSpace)
    {
    }
  }
}
