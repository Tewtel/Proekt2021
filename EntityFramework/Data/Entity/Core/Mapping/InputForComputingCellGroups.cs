// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.InputForComputingCellGroups
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping.ViewGeneration;

namespace System.Data.Entity.Core.Mapping
{
  internal struct InputForComputingCellGroups : 
    IEquatable<InputForComputingCellGroups>,
    IEqualityComparer<InputForComputingCellGroups>
  {
    internal readonly EntityContainerMapping ContainerMapping;
    internal readonly ConfigViewGenerator Config;

    internal InputForComputingCellGroups(
      EntityContainerMapping containerMapping,
      ConfigViewGenerator config)
    {
      this.ContainerMapping = containerMapping;
      this.Config = config;
    }

    public bool Equals(InputForComputingCellGroups other) => this.ContainerMapping.Equals((object) other.ContainerMapping) && this.Config.Equals((object) other.Config);

    public bool Equals(InputForComputingCellGroups one, InputForComputingCellGroups two)
    {
      if ((System.ValueType) one == (System.ValueType) two)
        return true;
      return (System.ValueType) one != null && (System.ValueType) two != null && one.Equals(two);
    }

    public int GetHashCode(InputForComputingCellGroups value) => value.GetHashCode();

    public override int GetHashCode() => this.ContainerMapping.GetHashCode();

    public override bool Equals(object obj) => obj is InputForComputingCellGroups other && this.Equals(other);

    public static bool operator ==(
      InputForComputingCellGroups input1,
      InputForComputingCellGroups input2)
    {
      return (System.ValueType) input1 == (System.ValueType) input2 || input1.Equals(input2);
    }

    public static bool operator !=(
      InputForComputingCellGroups input1,
      InputForComputingCellGroups input2)
    {
      return !(input1 == input2);
    }
  }
}
