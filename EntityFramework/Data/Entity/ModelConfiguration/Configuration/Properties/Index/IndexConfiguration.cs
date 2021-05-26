// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Index.IndexConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Edm;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Index
{
  internal class IndexConfiguration : PropertyConfiguration
  {
    private bool? _isUnique;
    private bool? _isClustered;
    private string _name;

    public IndexConfiguration()
    {
    }

    internal IndexConfiguration(IndexConfiguration source)
    {
      this._isUnique = source._isUnique;
      this._isClustered = source._isClustered;
      this._name = source._name;
    }

    public bool? IsUnique
    {
      get => this._isUnique;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<bool>(value, nameof (value));
        this._isUnique = value;
      }
    }

    public bool? IsClustered
    {
      get => this._isClustered;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<bool>(value, nameof (value));
        this._isClustered = value;
      }
    }

    public string Name
    {
      get => this._name;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<string>(value, nameof (value));
        this._name = value;
      }
    }

    internal virtual IndexConfiguration Clone() => new IndexConfiguration(this);

    internal void Configure(EdmProperty edmProperty, int indexOrder) => IndexConfiguration.AddAnnotationWithMerge((MetadataItem) edmProperty, new IndexAnnotation(new IndexAttribute(this._name, indexOrder, this._isClustered, this._isUnique)));

    internal void Configure(EntityType entityType) => IndexConfiguration.AddAnnotationWithMerge((MetadataItem) entityType, new IndexAnnotation(new IndexAttribute(this._name, this._isClustered, this._isUnique)));

    private static void AddAnnotationWithMerge(
      MetadataItem metadataItem,
      IndexAnnotation newAnnotation)
    {
      object annotation = metadataItem.Annotations.GetAnnotation("http://schemas.microsoft.com/ado/2013/11/edm/customannotation:Index");
      if (annotation != null)
        newAnnotation = (IndexAnnotation) ((IndexAnnotation) annotation).MergeWith((object) newAnnotation);
      metadataItem.AddAnnotation("http://schemas.microsoft.com/ado/2013/11/edm/customannotation:Index", (object) newAnnotation);
    }
  }
}
