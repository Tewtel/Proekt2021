// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.SsdlSerializer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Linq;
using System.Xml;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Serializes the storage (database) section of an <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmModel" /> to XML.
  /// </summary>
  public class SsdlSerializer
  {
    /// <summary>
    /// Occurs when an error is encountered serializing the model.
    /// </summary>
    public event EventHandler<DataModelErrorEventArgs> OnError;

    /// <summary>
    /// Serialize the <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmModel" /> to the <see cref="T:System.Xml.XmlWriter" />
    /// </summary>
    /// <param name="dbDatabase"> The EdmModel to serialize </param>
    /// <param name="provider"> Provider information on the Schema element </param>
    /// <param name="providerManifestToken"> ProviderManifestToken information on the Schema element </param>
    /// <param name="xmlWriter"> The XmlWriter to serialize to </param>
    /// <param name="serializeDefaultNullability">A value indicating whether to serialize Nullable attributes when they are set to the default value.</param>
    /// <returns> true if model can be serialized, otherwise false </returns>
    public virtual bool Serialize(
      EdmModel dbDatabase,
      string provider,
      string providerManifestToken,
      XmlWriter xmlWriter,
      bool serializeDefaultNullability = true)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmModel>(dbDatabase, nameof (dbDatabase));
      System.Data.Entity.Utilities.Check.NotEmpty(provider, nameof (provider));
      System.Data.Entity.Utilities.Check.NotEmpty(providerManifestToken, nameof (providerManifestToken));
      System.Data.Entity.Utilities.Check.NotNull<XmlWriter>(xmlWriter, nameof (xmlWriter));
      if (!this.ValidateModel(dbDatabase))
        return false;
      SsdlSerializer.CreateVisitor(xmlWriter, dbDatabase, serializeDefaultNullability).Visit(dbDatabase, provider, providerManifestToken);
      return true;
    }

    /// <summary>
    /// Serialize the <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmModel" /> to the <see cref="T:System.Xml.XmlWriter" />
    /// </summary>
    /// <param name="dbDatabase"> The EdmModel to serialize </param>
    /// <param name="namespaceName"> Namespace name on the Schema element </param>
    /// <param name="provider"> Provider information on the Schema element </param>
    /// <param name="providerManifestToken"> ProviderManifestToken information on the Schema element </param>
    /// <param name="xmlWriter"> The XmlWriter to serialize to </param>
    /// <param name="serializeDefaultNullability">A value indicating whether to serialize Nullable attributes when they are set to the default value.</param>
    /// <returns> true if model can be serialized, otherwise false </returns>
    public virtual bool Serialize(
      EdmModel dbDatabase,
      string namespaceName,
      string provider,
      string providerManifestToken,
      XmlWriter xmlWriter,
      bool serializeDefaultNullability = true)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmModel>(dbDatabase, nameof (dbDatabase));
      System.Data.Entity.Utilities.Check.NotEmpty(namespaceName, nameof (namespaceName));
      System.Data.Entity.Utilities.Check.NotEmpty(provider, nameof (provider));
      System.Data.Entity.Utilities.Check.NotEmpty(providerManifestToken, nameof (providerManifestToken));
      System.Data.Entity.Utilities.Check.NotNull<XmlWriter>(xmlWriter, nameof (xmlWriter));
      if (!this.ValidateModel(dbDatabase))
        return false;
      SsdlSerializer.CreateVisitor(xmlWriter, dbDatabase, serializeDefaultNullability).Visit(dbDatabase, namespaceName, provider, providerManifestToken);
      return true;
    }

    private bool ValidateModel(EdmModel model)
    {
      bool modelIsValid = true;
      Action<DataModelErrorEventArgs> onErrorAction = (Action<DataModelErrorEventArgs>) (e =>
      {
        MetadataItem instance = e.Item;
        if (instance != null && MetadataItemHelper.IsInvalid(instance))
          return;
        modelIsValid = false;
        if (this.OnError == null)
          return;
        this.OnError((object) this, e);
      });
      if (model.NamespaceNames.Count<string>() > 1 || model.Containers.Count<EntityContainer>() != 1)
        onErrorAction(new DataModelErrorEventArgs()
        {
          ErrorMessage = System.Data.Entity.Resources.Strings.Serializer_OneNamespaceAndOneContainer
        });
      DataModelValidator dataModelValidator = new DataModelValidator();
      dataModelValidator.OnError += (EventHandler<DataModelErrorEventArgs>) ((_, e) => onErrorAction(e));
      dataModelValidator.Validate(model, true);
      return modelIsValid;
    }

    private static EdmSerializationVisitor CreateVisitor(
      XmlWriter xmlWriter,
      EdmModel dbDatabase,
      bool serializeDefaultNullability)
    {
      return new EdmSerializationVisitor(xmlWriter, dbDatabase.SchemaVersion, serializeDefaultNullability);
    }
  }
}
