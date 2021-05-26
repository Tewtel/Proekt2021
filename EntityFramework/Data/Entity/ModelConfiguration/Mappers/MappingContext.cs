// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Mappers.MappingContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Utilities;

namespace System.Data.Entity.ModelConfiguration.Mappers
{
  internal sealed class MappingContext
  {
    private readonly System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration _modelConfiguration;
    private readonly ConventionsConfiguration _conventionsConfiguration;
    private readonly EdmModel _model;
    private readonly AttributeProvider _attributeProvider;
    private readonly DbModelBuilderVersion _modelBuilderVersion;

    public MappingContext(
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration,
      ConventionsConfiguration conventionsConfiguration,
      EdmModel model,
      DbModelBuilderVersion modelBuilderVersion = DbModelBuilderVersion.Latest,
      AttributeProvider attributeProvider = null)
    {
      this._modelConfiguration = modelConfiguration;
      this._conventionsConfiguration = conventionsConfiguration;
      this._model = model;
      this._modelBuilderVersion = modelBuilderVersion;
      this._attributeProvider = attributeProvider ?? new AttributeProvider();
    }

    public System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration ModelConfiguration => this._modelConfiguration;

    public ConventionsConfiguration ConventionsConfiguration => this._conventionsConfiguration;

    public EdmModel Model => this._model;

    public AttributeProvider AttributeProvider => this._attributeProvider;

    public DbModelBuilderVersion ModelBuilderVersion => this._modelBuilderVersion;
  }
}
