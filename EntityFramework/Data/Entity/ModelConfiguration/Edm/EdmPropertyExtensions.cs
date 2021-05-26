// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.EdmPropertyExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class EdmPropertyExtensions
  {
    private const string OrderAnnotation = "Order";
    private const string PreferredNameAnnotation = "PreferredName";
    private const string UnpreferredUniqueNameAnnotation = "UnpreferredUniqueName";

    public static void CopyFrom(this EdmProperty column, EdmProperty other)
    {
      column.IsFixedLength = other.IsFixedLength;
      column.IsMaxLength = other.IsMaxLength;
      column.IsUnicode = other.IsUnicode;
      column.MaxLength = other.MaxLength;
      column.Precision = other.Precision;
      column.Scale = other.Scale;
    }

    public static EdmProperty Clone(this EdmProperty tableColumn)
    {
      EdmProperty columnMetadata = new EdmProperty(tableColumn.Name, tableColumn.TypeUsage)
      {
        Nullable = tableColumn.Nullable,
        StoreGeneratedPattern = tableColumn.StoreGeneratedPattern,
        IsFixedLength = tableColumn.IsFixedLength,
        IsMaxLength = tableColumn.IsMaxLength,
        IsUnicode = tableColumn.IsUnicode,
        MaxLength = tableColumn.MaxLength,
        Precision = tableColumn.Precision,
        Scale = tableColumn.Scale
      };
      tableColumn.Annotations.Each<MetadataProperty>((Action<MetadataProperty>) (a => columnMetadata.GetMetadataProperties().Add(a)));
      return columnMetadata;
    }

    public static int? GetOrder(this EdmProperty tableColumn) => (int?) tableColumn.Annotations.GetAnnotation("Order");

    public static void SetOrder(this EdmProperty tableColumn, int order) => tableColumn.GetMetadataProperties().SetAnnotation("Order", (object) order);

    public static string GetPreferredName(this EdmProperty tableColumn) => (string) tableColumn.Annotations.GetAnnotation("PreferredName");

    public static void SetPreferredName(this EdmProperty tableColumn, string name) => tableColumn.GetMetadataProperties().SetAnnotation("PreferredName", (object) name);

    public static string GetUnpreferredUniqueName(this EdmProperty tableColumn) => (string) tableColumn.Annotations.GetAnnotation("UnpreferredUniqueName");

    public static void SetUnpreferredUniqueName(this EdmProperty tableColumn, string name) => tableColumn.GetMetadataProperties().SetAnnotation("UnpreferredUniqueName", (object) name);

    public static void RemoveStoreGeneratedIdentityPattern(this EdmProperty tableColumn)
    {
      if (tableColumn.StoreGeneratedPattern != StoreGeneratedPattern.Identity)
        return;
      tableColumn.StoreGeneratedPattern = StoreGeneratedPattern.None;
    }

    public static bool HasStoreGeneratedPattern(this EdmProperty property)
    {
      StoreGeneratedPattern? generatedPattern1 = property.GetStoreGeneratedPattern();
      if (!generatedPattern1.HasValue)
        return false;
      StoreGeneratedPattern? nullable = generatedPattern1;
      StoreGeneratedPattern generatedPattern2 = StoreGeneratedPattern.None;
      return !(nullable.GetValueOrDefault() == generatedPattern2 & nullable.HasValue);
    }

    public static StoreGeneratedPattern? GetStoreGeneratedPattern(
      this EdmProperty property)
    {
      MetadataProperty metadataProperty;
      return property.MetadataProperties.TryGetValue("http://schemas.microsoft.com/ado/2009/02/edm/annotation:StoreGeneratedPattern", false, out metadataProperty) ? (StoreGeneratedPattern?) Enum.Parse(typeof (StoreGeneratedPattern), (string) metadataProperty.Value) : new StoreGeneratedPattern?();
    }

    public static void SetStoreGeneratedPattern(
      this EdmProperty property,
      StoreGeneratedPattern storeGeneratedPattern)
    {
      MetadataProperty metadataProperty;
      if (!property.MetadataProperties.TryGetValue("http://schemas.microsoft.com/ado/2009/02/edm/annotation:StoreGeneratedPattern", false, out metadataProperty))
        property.MetadataProperties.Source.Add(new MetadataProperty("http://schemas.microsoft.com/ado/2009/02/edm/annotation:StoreGeneratedPattern", TypeUsage.Create((EdmType) EdmProviderManifest.Instance.GetPrimitiveType(PrimitiveTypeKind.String)), (object) storeGeneratedPattern.ToString()));
      else
        metadataProperty.Value = (object) storeGeneratedPattern.ToString();
    }

    public static object GetConfiguration(this EdmProperty property) => property.Annotations.GetConfiguration();

    public static void SetConfiguration(this EdmProperty property, object configuration) => property.GetMetadataProperties().SetConfiguration(configuration);

    public static List<EdmPropertyPath> ToPropertyPathList(
      this EdmProperty property)
    {
      return property.ToPropertyPathList(new List<EdmProperty>());
    }

    public static List<EdmPropertyPath> ToPropertyPathList(
      this EdmProperty property,
      List<EdmProperty> currentPath)
    {
      List<EdmPropertyPath> propertyPaths = new List<EdmPropertyPath>();
      EdmPropertyExtensions.IncludePropertyPath(propertyPaths, currentPath, property);
      return propertyPaths;
    }

    private static void IncludePropertyPath(
      List<EdmPropertyPath> propertyPaths,
      List<EdmProperty> currentPath,
      EdmProperty property)
    {
      currentPath.Add(property);
      if (property.IsUnderlyingPrimitiveType)
        propertyPaths.Add(new EdmPropertyPath((IEnumerable<EdmProperty>) currentPath));
      else if (property.IsComplexType)
      {
        foreach (EdmProperty property1 in property.ComplexType.Properties)
          EdmPropertyExtensions.IncludePropertyPath(propertyPaths, currentPath, property1);
      }
      currentPath.Remove(property);
    }
  }
}
