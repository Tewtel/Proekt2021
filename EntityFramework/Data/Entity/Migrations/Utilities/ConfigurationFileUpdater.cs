// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Utilities.ConfigurationFileUpdater
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Utilities;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace System.Data.Entity.Migrations.Utilities
{
  internal class ConfigurationFileUpdater
  {
    private static readonly XNamespace _asm = (XNamespace) "urn:schemas-microsoft-com:asm.v1";
    private static readonly XElement _dependentAssemblyElement;

    static ConfigurationFileUpdater()
    {
      AssemblyName name = typeof (ConfigurationFileUpdater).Assembly().GetName();
      ConfigurationFileUpdater._dependentAssemblyElement = new XElement(ConfigurationFileUpdater._asm + "dependentAssembly", new object[2]
      {
        (object) new XElement(ConfigurationFileUpdater._asm + "assemblyIdentity", new object[3]
        {
          (object) new XAttribute((XName) "name", (object) "EntityFramework"),
          (object) new XAttribute((XName) "culture", (object) "neutral"),
          (object) new XAttribute((XName) "publicKeyToken", (object) "b77a5c561934e089")
        }),
        (object) new XElement(ConfigurationFileUpdater._asm + "codeBase", new object[2]
        {
          (object) new XAttribute((XName) "version", (object) name.Version.ToString()),
          (object) new XAttribute((XName) "href", (object) name.CodeBase)
        })
      });
    }

    public virtual string Update(string configurationFile)
    {
      int num = string.IsNullOrWhiteSpace(configurationFile) ? 0 : (File.Exists(configurationFile) ? 1 : 0);
      XDocument container = num != 0 ? XDocument.Load(configurationFile) : new XDocument();
      container.GetOrAddElement((XName) "configuration").GetOrAddElement((XName) "runtime").GetOrAddElement(ConfigurationFileUpdater._asm + "assemblyBinding").Add((object) ConfigurationFileUpdater._dependentAssemblyElement);
      string str = Path.GetTempFileName();
      if (num != 0)
      {
        File.Delete(str);
        str = Path.Combine(Path.GetDirectoryName(configurationFile), Path.GetFileName(str));
      }
      container.Save(str);
      return str;
    }
  }
}
