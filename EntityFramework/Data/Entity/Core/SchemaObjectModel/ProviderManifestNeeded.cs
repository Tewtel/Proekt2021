// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.ProviderManifestNeeded
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal delegate DbProviderManifest ProviderManifestNeeded(
    System.Action<string, ErrorCode, EdmSchemaErrorSeverity> addError);
}
