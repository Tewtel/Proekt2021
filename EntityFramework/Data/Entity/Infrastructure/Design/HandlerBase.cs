// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.HandlerBase
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  /// <summary>
  /// Base handler type. Handlers aren't required to use this exact type. Only the
  /// namespace, name, and member signatures need to be the same. This also applies to
  /// handler contracts types
  /// </summary>
  public abstract class HandlerBase : MarshalByRefObject
  {
    /// <summary>
    /// Indicates whether the specified contract is implemented by this handler.
    /// </summary>
    /// <param name="interfaceName">The full name of the contract interface.</param>
    /// <returns><c>True</c> if the contract is implemented, otherwise <c>false</c>.</returns>
    public virtual bool ImplementsContract(string interfaceName)
    {
      Type type;
      try
      {
        type = Type.GetType(interfaceName, true);
      }
      catch
      {
        return false;
      }
      return type.IsAssignableFrom(this.GetType());
    }
  }
}
