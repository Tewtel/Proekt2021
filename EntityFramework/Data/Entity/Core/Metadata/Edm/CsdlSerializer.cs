// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.CsdlSerializer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Linq;
using System.Xml;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Serializes an <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmModel" /> that conforms to the restrictions of a single
  /// CSDL schema file to an XML writer. The model to be serialized must contain a single
  /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.EntityContainer" /> .
  /// </summary>
  public class CsdlSerializer
  {
    /// <summary>
    /// Occurs when an error is encountered serializing the model.
    /// </summary>
    public event EventHandler<DataModelErrorEventArgs> OnError;

    /// <summary>
    /// Serialize the <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmModel" /> to the XmlWriter.
    /// </summary>
    /// <param name="model">The EdmModel to serialize.</param>
    /// <param name="xmlWriter"> The XmlWriter to serialize to. </param>
    /// <param name="modelNamespace">The serialized model's namespace.</param>
    /// <returns>true if the model is valid; otherwise, false.</returns>
    public bool Serialize(EdmModel model, XmlWriter xmlWriter, string modelNamespace = null)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmModel>(model, nameof (model));
      System.Data.Entity.Utilities.Check.NotNull<XmlWriter>(xmlWriter, nameof (xmlWriter));
      bool modelIsValid = true;
      Action<DataModelErrorEventArgs> onErrorAction = (Action<DataModelErrorEventArgs>) (e =>
      {
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
      if (!modelIsValid)
        return false;
      new EdmSerializationVisitor(xmlWriter, model.SchemaVersion).Visit(model, modelNamespace);
      return true;
    }
  }
}
