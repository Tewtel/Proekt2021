﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.XDocumentExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.Edm;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace System.Data.Entity.Utilities
{
  internal static class XDocumentExtensions
  {
    public static StorageMappingItemCollection GetStorageMappingItemCollection(
      this XDocument model,
      out DbProviderInfo providerInfo)
    {
      EdmItemCollection edmCollection = new EdmItemCollection((IEnumerable<XmlReader>) new XmlReader[1]
      {
        model.Descendants(EdmXNames.Csdl.SchemaNames).Single<XElement>().CreateReader()
      });
      XElement element = model.Descendants(EdmXNames.Ssdl.SchemaNames).Single<XElement>();
      providerInfo = new DbProviderInfo(element.ProviderAttribute(), element.ProviderManifestTokenAttribute());
      StoreItemCollection storeCollection = new StoreItemCollection((IEnumerable<XmlReader>) new XmlReader[1]
      {
        element.CreateReader()
      });
      return new StorageMappingItemCollection(edmCollection, storeCollection, (IEnumerable<XmlReader>) new XmlReader[1]
      {
        new XElement(model.Descendants(EdmXNames.Msl.MappingNames).Single<XElement>()).CreateReader()
      });
    }
  }
}
