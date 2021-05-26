// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.CodeFirstOSpaceTypeFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class CodeFirstOSpaceTypeFactory : OSpaceTypeFactory
  {
    private readonly List<Action> _referenceResolutions = new List<Action>();
    private readonly Dictionary<EdmType, EdmType> _cspaceToOspace = new Dictionary<EdmType, EdmType>();
    private readonly Dictionary<string, EdmType> _loadedTypes = new Dictionary<string, EdmType>();

    public override List<Action> ReferenceResolutions => this._referenceResolutions;

    public override void LogLoadMessage(string message, EdmType relatedType)
    {
    }

    public override void LogError(string errorMessage, EdmType relatedType) => throw new MetadataException(Strings.InvalidSchemaEncountered((object) errorMessage));

    public override void TrackClosure(Type type)
    {
    }

    public override Dictionary<EdmType, EdmType> CspaceToOspace => this._cspaceToOspace;

    public override Dictionary<string, EdmType> LoadedTypes => this._loadedTypes;

    public override void AddToTypesInAssembly(EdmType type)
    {
    }
  }
}
