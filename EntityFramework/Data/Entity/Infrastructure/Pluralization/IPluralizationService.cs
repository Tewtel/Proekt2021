// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Pluralization.IPluralizationService
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Pluralization
{
  /// <summary>
  /// Pluralization services to be used by the EF runtime implement this interface.
  /// By default the <see cref="T:System.Data.Entity.Infrastructure.Pluralization.EnglishPluralizationService" /> is used, but the pluralization service to use
  /// can be set in a class derived from <see cref="T:System.Data.Entity.DbConfiguration" />.
  /// </summary>
  public interface IPluralizationService
  {
    /// <summary>Pluralize a word using the service.</summary>
    /// <param name="word">The word to pluralize.</param>
    /// <returns>The pluralized word </returns>
    string Pluralize(string word);

    /// <summary>Singularize a word using the service.</summary>
    /// <param name="word">The word to singularize.</param>
    /// <returns>The singularized word.</returns>
    string Singularize(string word);
  }
}
