// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicityConverter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal static class RelationshipMultiplicityConverter
  {
    internal static string MultiplicityToString(RelationshipMultiplicity multiplicity)
    {
      switch (multiplicity)
      {
        case RelationshipMultiplicity.ZeroOrOne:
          return "0..1";
        case RelationshipMultiplicity.One:
          return "1";
        case RelationshipMultiplicity.Many:
          return "*";
        default:
          return string.Empty;
      }
    }

    internal static bool TryParseMultiplicity(
      string value,
      out RelationshipMultiplicity multiplicity)
    {
      switch (value)
      {
        case "*":
          multiplicity = RelationshipMultiplicity.Many;
          return true;
        case "1":
          multiplicity = RelationshipMultiplicity.One;
          return true;
        case "0..1":
          multiplicity = RelationshipMultiplicity.ZeroOrOne;
          return true;
        default:
          multiplicity = ~RelationshipMultiplicity.ZeroOrOne;
          return false;
      }
    }
  }
}
