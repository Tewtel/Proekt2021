// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MslSerializer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Xml;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class MslSerializer
  {
    public virtual bool Serialize(DbDatabaseMapping databaseMapping, XmlWriter xmlWriter)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDatabaseMapping>(databaseMapping, nameof (databaseMapping));
      System.Data.Entity.Utilities.Check.NotNull<XmlWriter>(xmlWriter, nameof (xmlWriter));
      new MslXmlSchemaWriter(xmlWriter, databaseMapping.Model.SchemaVersion).WriteSchema(databaseMapping);
      return true;
    }
  }
}
