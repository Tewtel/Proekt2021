// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.EdmFunctionAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>
  /// Indicates that the given method is a proxy for an EDM function.
  /// </summary>
  /// <remarks>
  /// Note that this attribute has been replaced by the <see cref="T:System.Data.Entity.DbFunctionAttribute" /> starting with EF6.
  /// </remarks>
  [Obsolete("This attribute has been replaced by System.Data.Entity.DbFunctionAttribute.")]
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public sealed class EdmFunctionAttribute : DbFunctionAttribute
  {
    /// <summary>Creates a new DbFunctionAttribute instance.</summary>
    /// <param name="namespaceName"> The namespace name of the EDM function represented by the attributed method. </param>
    /// <param name="functionName"> The function name of the EDM function represented by the attributed method. </param>
    public EdmFunctionAttribute(string namespaceName, string functionName)
      : base(namespaceName, functionName)
    {
    }
  }
}
