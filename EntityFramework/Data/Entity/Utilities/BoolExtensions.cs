// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.BoolExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Utilities
{
  internal static class BoolExtensions
  {
    internal static bool? Not(this bool? operand) => !operand.HasValue ? new bool?() : new bool?(!operand.Value);

    internal static bool? And(this bool? left, bool? right) => !left.HasValue || !right.HasValue ? (left.HasValue || right.HasValue ? (!left.HasValue ? (right.Value ? new bool?() : new bool?(false)) : (left.Value ? new bool?() : new bool?(false))) : new bool?()) : new bool?(left.Value && right.Value);

    internal static bool? Or(this bool? left, bool? right) => !left.HasValue || !right.HasValue ? (left.HasValue || right.HasValue ? (!left.HasValue ? (right.Value ? new bool?(true) : new bool?()) : (left.Value ? new bool?(true) : new bool?())) : new bool?()) : new bool?(left.Value || right.Value);
  }
}
