// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ModelPerspective
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class ModelPerspective : Perspective
  {
    internal ModelPerspective(MetadataWorkspace metadataWorkspace)
      : base(metadataWorkspace, DataSpace.CSpace)
    {
    }

    internal override bool TryGetTypeByName(
      string fullName,
      bool ignoreCase,
      out TypeUsage typeUsage)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(fullName, nameof (fullName));
      typeUsage = (TypeUsage) null;
      EdmType edmType = (EdmType) null;
      if (this.MetadataWorkspace.TryGetItem<EdmType>(fullName, ignoreCase, this.TargetDataspace, out edmType))
        typeUsage = !Helper.IsPrimitiveType(edmType) ? TypeUsage.Create(edmType) : MetadataWorkspace.GetCanonicalModelTypeUsage(((PrimitiveType) edmType).PrimitiveTypeKind);
      return typeUsage != null;
    }
  }
}
