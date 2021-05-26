// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.DbFunctionAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity
{
  /// <summary>
  /// Indicates that the given method is a proxy for an EDM function.
  /// </summary>
  /// <remarks>
  /// Note that this class was called EdmFunctionAttribute in some previous versions of Entity Framework.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class DbFunctionAttribute : Attribute
  {
    private readonly string _namespaceName;
    private readonly string _functionName;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.DbFunctionAttribute" /> class.
    /// </summary>
    /// <param name="namespaceName">The namespace of the mapped-to function.</param>
    /// <param name="functionName">The name of the mapped-to function.</param>
    public DbFunctionAttribute(string namespaceName, string functionName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(namespaceName, nameof (namespaceName));
      System.Data.Entity.Utilities.Check.NotEmpty(functionName, nameof (functionName));
      this._namespaceName = namespaceName;
      this._functionName = functionName;
    }

    /// <summary>The namespace of the mapped-to function.</summary>
    /// <returns>The namespace of the mapped-to function.</returns>
    public string NamespaceName => this._namespaceName;

    /// <summary>The name of the mapped-to function.</summary>
    /// <returns>The name of the mapped-to function.</returns>
    public string FunctionName => this._functionName;
  }
}
