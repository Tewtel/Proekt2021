// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Annotations.CompatibilityResult
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Annotations
{
  /// <summary>
  /// Returned by <see cref="M:System.Data.Entity.Infrastructure.Annotations.IMergeableAnnotation.IsCompatibleWith(System.Object)" /> and related methods to indicate whether or
  /// not one object does not conflict with another such that the two can be combined into one.
  /// </summary>
  /// <remarks>
  /// If the two objects are not compatible then information about why they are not compatible is contained
  /// in the <see cref="P:System.Data.Entity.Infrastructure.Annotations.CompatibilityResult.ErrorMessage" /> property.
  /// </remarks>
  public sealed class CompatibilityResult
  {
    private readonly bool _isCompatible;
    private readonly string _errorMessage;

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Annotations.CompatibilityResult" /> instance.
    /// </summary>
    /// <param name="isCompatible">Indicates whether or not the two tested objects are compatible.</param>
    /// <param name="errorMessage">
    /// An error message indicating how the objects are not compatible. Expected to be null if isCompatible is true.
    /// </param>
    public CompatibilityResult(bool isCompatible, string errorMessage)
    {
      this._isCompatible = isCompatible;
      this._errorMessage = errorMessage;
      if (isCompatible)
        return;
      System.Data.Entity.Utilities.Check.NotEmpty(errorMessage, nameof (errorMessage));
    }

    /// <summary>
    /// True if the two tested objects are compatible; otherwise false.
    /// </summary>
    public bool IsCompatible => this._isCompatible;

    /// <summary>
    /// If <see cref="P:System.Data.Entity.Infrastructure.Annotations.CompatibilityResult.IsCompatible" /> is true, then returns an error message indicating how the two tested objects
    /// are incompatible.
    /// </summary>
    public string ErrorMessage => this._errorMessage;

    /// <summary>
    /// Implicit conversion to a bool to allow the result object to be used directly in checks.
    /// </summary>
    /// <param name="result">The object to convert.</param>
    /// <returns>True if the result is compatible; false otherwise.</returns>
    public static implicit operator bool(CompatibilityResult result)
    {
      System.Data.Entity.Utilities.Check.NotNull<CompatibilityResult>(result, nameof (result));
      return result._isCompatible;
    }
  }
}
