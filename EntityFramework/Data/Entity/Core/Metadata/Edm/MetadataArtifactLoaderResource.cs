// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MetadataArtifactLoaderResource
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.SchemaObjectModel;
using System.Data.Entity.Resources;
using System.IO;
using System.Reflection;
using System.Xml;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class MetadataArtifactLoaderResource : MetadataArtifactLoader, IComparable
  {
    private readonly bool _alreadyLoaded;
    private readonly Assembly _assembly;
    private readonly string _resourceName;

    internal MetadataArtifactLoaderResource(
      Assembly assembly,
      string resourceName,
      ICollection<string> uriRegistry)
    {
      this._assembly = assembly;
      this._resourceName = resourceName;
      string resPath = MetadataArtifactLoaderCompositeResource.CreateResPath(this._assembly, this._resourceName);
      this._alreadyLoaded = uriRegistry.Contains(resPath);
      if (this._alreadyLoaded)
        return;
      uriRegistry.Add(resPath);
    }

    public override string Path => MetadataArtifactLoaderCompositeResource.CreateResPath(this._assembly, this._resourceName);

    public int CompareTo(object obj) => obj is MetadataArtifactLoaderResource artifactLoaderResource ? string.Compare(this.Path, artifactLoaderResource.Path, StringComparison.OrdinalIgnoreCase) : -1;

    public override bool Equals(object obj) => this.CompareTo(obj) == 0;

    public override int GetHashCode() => this.Path.GetHashCode();

    public override List<string> GetPaths(DataSpace spaceToGet)
    {
      List<string> stringList = new List<string>();
      if (!this._alreadyLoaded && MetadataArtifactLoader.IsArtifactOfDataSpace(this.Path, spaceToGet))
        stringList.Add(this.Path);
      return stringList;
    }

    public override List<string> GetPaths()
    {
      List<string> stringList = new List<string>();
      if (!this._alreadyLoaded)
        stringList.Add(this.Path);
      return stringList;
    }

    public override List<XmlReader> GetReaders(
      Dictionary<MetadataArtifactLoader, XmlReader> sourceDictionary)
    {
      List<XmlReader> xmlReaderList = new List<XmlReader>();
      if (!this._alreadyLoaded)
      {
        XmlReader reader = this.CreateReader();
        xmlReaderList.Add(reader);
        sourceDictionary?.Add((MetadataArtifactLoader) this, reader);
      }
      return xmlReaderList;
    }

    private XmlReader CreateReader()
    {
      Stream input = this.LoadResource();
      XmlReaderSettings xmlReaderSettings = Schema.CreateEdmStandardXmlReaderSettings();
      xmlReaderSettings.CloseInput = true;
      xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
      XmlReaderSettings settings = xmlReaderSettings;
      return XmlReader.Create(input, settings);
    }

    public override List<XmlReader> CreateReaders(DataSpace spaceToGet)
    {
      List<XmlReader> xmlReaderList = new List<XmlReader>();
      if (!this._alreadyLoaded && MetadataArtifactLoader.IsArtifactOfDataSpace(this.Path, spaceToGet))
      {
        XmlReader reader = this.CreateReader();
        xmlReaderList.Add(reader);
      }
      return xmlReaderList;
    }

    private Stream LoadResource()
    {
      Stream resourceStream;
      if (this.TryCreateResourceStream(out resourceStream))
        return resourceStream;
      throw new System.Data.Entity.Core.MetadataException(Strings.UnableToLoadResource);
    }

    private bool TryCreateResourceStream(out Stream resourceStream)
    {
      resourceStream = this._assembly.GetManifestResourceStream(this._resourceName);
      return resourceStream != null;
    }
  }
}
