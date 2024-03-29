﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.ForeignKeyBuilderExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class ForeignKeyBuilderExtensions
  {
    private const string IsTypeConstraint = "IsTypeConstraint";
    private const string IsSplitConstraint = "IsSplitConstraint";
    private const string AssociationType = "AssociationType";
    private const string PreferredNameAnnotation = "PreferredName";

    public static string GetPreferredName(this ForeignKeyBuilder fk) => (string) fk.Annotations.GetAnnotation("PreferredName");

    public static void SetPreferredName(this ForeignKeyBuilder fk, string name) => fk.GetMetadataProperties().SetAnnotation("PreferredName", (object) name);

    public static bool GetIsTypeConstraint(this ForeignKeyBuilder fk)
    {
      object annotation = fk.Annotations.GetAnnotation("IsTypeConstraint");
      return annotation != null && (bool) annotation;
    }

    public static void SetIsTypeConstraint(this ForeignKeyBuilder fk) => fk.GetMetadataProperties().SetAnnotation("IsTypeConstraint", (object) true);

    public static void SetIsSplitConstraint(this ForeignKeyBuilder fk) => fk.GetMetadataProperties().SetAnnotation("IsSplitConstraint", (object) true);

    public static System.Data.Entity.Core.Metadata.Edm.AssociationType GetAssociationType(
      this ForeignKeyBuilder fk)
    {
      return fk.Annotations.GetAnnotation("AssociationType") as System.Data.Entity.Core.Metadata.Edm.AssociationType;
    }

    public static void SetAssociationType(
      this ForeignKeyBuilder fk,
      System.Data.Entity.Core.Metadata.Edm.AssociationType associationType)
    {
      fk.GetMetadataProperties().SetAnnotation("AssociationType", (object) associationType);
    }
  }
}
