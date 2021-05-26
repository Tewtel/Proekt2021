// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Infrastructure.VersionedModel
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Xml.Linq;

namespace System.Data.Entity.Migrations.Infrastructure
{
  internal class VersionedModel
  {
    private readonly XDocument _model;
    private readonly string _version;

    public VersionedModel(XDocument model, string version = null)
    {
      this._model = model;
      this._version = version;
    }

    public XDocument Model => this._model;

    public string Version => this._version;
  }
}
