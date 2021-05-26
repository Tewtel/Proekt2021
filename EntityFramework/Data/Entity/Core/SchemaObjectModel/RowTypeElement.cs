// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.RowTypeElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Text;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class RowTypeElement : ModelFunctionTypeElement
  {
    private readonly SchemaElementLookUpTable<RowTypePropertyElement> _properties = new SchemaElementLookUpTable<RowTypePropertyElement>();

    internal RowTypeElement(SchemaElement parentElement)
      : base(parentElement)
    {
    }

    protected override bool HandleElement(XmlReader reader)
    {
      if (!this.CanHandleElement(reader, "Property"))
        return false;
      this.HandlePropertyElement(reader);
      return true;
    }

    protected void HandlePropertyElement(XmlReader reader)
    {
      RowTypePropertyElement type = new RowTypePropertyElement((SchemaElement) this);
      type.Parse(reader);
      this._properties.Add(type, true, new Func<object, string>(Strings.DuplicateEntityContainerMemberName));
    }

    internal SchemaElementLookUpTable<RowTypePropertyElement> Properties => this._properties;

    internal override void ResolveTopLevelNames()
    {
      foreach (SchemaElement property in this._properties)
        property.ResolveTopLevelNames();
    }

    internal override void WriteIdentity(StringBuilder builder)
    {
      builder.Append("Row[");
      bool flag = true;
      foreach (RowTypePropertyElement property in this._properties)
      {
        if (flag)
          flag = !flag;
        else
          builder.Append(", ");
        StringBuilder builder1 = builder;
        property.WriteIdentity(builder1);
      }
      builder.Append("]");
    }

    internal override TypeUsage GetTypeUsage()
    {
      if (this._typeUsage == null)
      {
        List<EdmProperty> edmPropertyList = new List<EdmProperty>();
        foreach (RowTypePropertyElement property in this._properties)
        {
          EdmProperty edmProperty = new EdmProperty(property.FQName, property.GetTypeUsage());
          edmProperty.AddMetadataProperties((IEnumerable<MetadataProperty>) property.OtherContent);
          edmPropertyList.Add(edmProperty);
        }
        RowType rowType = new RowType((IEnumerable<EdmProperty>) edmPropertyList);
        if (this.Schema.DataModel == SchemaDataModelOption.EntityDataModel)
          rowType.DataSpace = DataSpace.CSpace;
        else
          rowType.DataSpace = DataSpace.SSpace;
        rowType.AddMetadataProperties((IEnumerable<MetadataProperty>) this.OtherContent);
        this._typeUsage = TypeUsage.Create((EdmType) rowType);
      }
      return this._typeUsage;
    }

    internal override bool ResolveNameAndSetTypeUsage(
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      bool flag = true;
      if (this._typeUsage == null)
      {
        foreach (ModelFunctionTypeElement property in this._properties)
        {
          if (!property.ResolveNameAndSetTypeUsage(convertedItemCache, newGlobalItems))
            flag = false;
        }
      }
      return flag;
    }

    internal override void Validate()
    {
      foreach (SchemaElement property in this._properties)
        property.Validate();
      if (this._properties.Count != 0)
        return;
      this.AddError(ErrorCode.RowTypeWithoutProperty, EdmSchemaErrorSeverity.Error, (object) Strings.RowTypeWithoutProperty);
    }
  }
}
