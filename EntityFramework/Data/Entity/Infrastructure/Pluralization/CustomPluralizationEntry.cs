// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Pluralization.CustomPluralizationEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Pluralization
{
  /// <summary>
  /// Represents a custom pluralization term to be used by the <see cref="T:System.Data.Entity.Infrastructure.Pluralization.EnglishPluralizationService" />
  /// </summary>
  public class CustomPluralizationEntry
  {
    /// <summary>Get the singular.</summary>
    public string Singular { get; private set; }

    /// <summary>Get the plural.</summary>
    public string Plural { get; private set; }

    /// <summary>Create a new instance</summary>
    /// <param name="singular">A non null or empty string representing the singular.</param>
    /// <param name="plural">A non null or empty string representing the plural.</param>
    public CustomPluralizationEntry(string singular, string plural)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(singular, nameof (singular));
      System.Data.Entity.Utilities.Check.NotEmpty(plural, nameof (plural));
      this.Singular = singular;
      this.Plural = plural;
    }
  }
}
