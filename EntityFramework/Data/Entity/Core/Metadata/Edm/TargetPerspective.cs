// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.TargetPerspective
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class TargetPerspective : Perspective
  {
    internal const DataSpace TargetPerspectiveDataSpace = DataSpace.SSpace;
    private readonly ModelPerspective _modelPerspective;

    internal TargetPerspective(MetadataWorkspace metadataWorkspace)
      : base(metadataWorkspace, DataSpace.SSpace)
    {
      this._modelPerspective = new ModelPerspective(metadataWorkspace);
    }

    internal override bool TryGetTypeByName(string fullName, bool ignoreCase, out TypeUsage usage)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(fullName, nameof (fullName));
      EdmType edmType = (EdmType) null;
      if (!this.MetadataWorkspace.TryGetItem<EdmType>(fullName, ignoreCase, this.TargetDataspace, out edmType))
        return this._modelPerspective.TryGetTypeByName(fullName, ignoreCase, out usage);
      usage = TypeUsage.Create(edmType);
      usage = Helper.GetModelTypeUsage(usage);
      return true;
    }

    internal override bool TryGetEntityContainer(
      string name,
      bool ignoreCase,
      out EntityContainer entityContainer)
    {
      return base.TryGetEntityContainer(name, ignoreCase, out entityContainer) || this._modelPerspective.TryGetEntityContainer(name, ignoreCase, out entityContainer);
    }
  }
}
