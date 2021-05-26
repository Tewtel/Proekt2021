// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ValueCondition
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping
{
  internal class ValueCondition : IEquatable<ValueCondition>
  {
    internal readonly string Description;
    internal readonly bool IsSentinel;
    internal const string IsNullDescription = "NULL";
    internal const string IsNotNullDescription = "NOT NULL";
    internal const string IsOtherDescription = "OTHER";
    internal static readonly ValueCondition IsNull = new ValueCondition("NULL", true);
    internal static readonly ValueCondition IsNotNull = new ValueCondition("NOT NULL", true);
    internal static readonly ValueCondition IsOther = new ValueCondition("OTHER", true);

    private ValueCondition(string description, bool isSentinel)
    {
      this.Description = description;
      this.IsSentinel = isSentinel;
    }

    internal ValueCondition(string description)
      : this(description, false)
    {
    }

    internal bool IsNotNullCondition => this == ValueCondition.IsNotNull;

    public bool Equals(ValueCondition other) => other.IsSentinel == this.IsSentinel && other.Description == this.Description;

    public override int GetHashCode() => this.Description.GetHashCode();

    public override string ToString() => this.Description;
  }
}
