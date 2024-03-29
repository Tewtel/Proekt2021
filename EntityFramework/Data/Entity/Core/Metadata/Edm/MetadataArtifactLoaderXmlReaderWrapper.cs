﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MetadataArtifactLoaderXmlReaderWrapper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Xml;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class MetadataArtifactLoaderXmlReaderWrapper : MetadataArtifactLoader, IComparable
  {
    private readonly XmlReader _reader;
    private readonly string _resourceUri;

    public MetadataArtifactLoaderXmlReaderWrapper(XmlReader xmlReader)
    {
      this._reader = xmlReader;
      this._resourceUri = xmlReader.BaseURI;
    }

    public override string Path => string.IsNullOrEmpty(this._resourceUri) ? string.Empty : this._resourceUri;

    public int CompareTo(object obj) => obj is MetadataArtifactLoaderXmlReaderWrapper xmlReaderWrapper && this._reader == xmlReaderWrapper._reader ? 0 : -1;

    public override bool Equals(object obj) => this.CompareTo(obj) == 0;

    public override int GetHashCode() => this._reader.GetHashCode();

    public override List<string> GetPaths(DataSpace spaceToGet)
    {
      List<string> stringList = new List<string>();
      if (MetadataArtifactLoader.IsArtifactOfDataSpace(this.Path, spaceToGet))
        stringList.Add(this.Path);
      return stringList;
    }

    public override List<string> GetPaths() => new List<string>((IEnumerable<string>) new string[1]
    {
      this.Path
    });

    public override List<XmlReader> GetReaders(
      Dictionary<MetadataArtifactLoader, XmlReader> sourceDictionary)
    {
      List<XmlReader> xmlReaderList = new List<XmlReader>();
      xmlReaderList.Add(this._reader);
      if (sourceDictionary == null)
        return xmlReaderList;
      sourceDictionary.Add((MetadataArtifactLoader) this, this._reader);
      return xmlReaderList;
    }

    public override List<XmlReader> CreateReaders(DataSpace spaceToGet)
    {
      List<XmlReader> xmlReaderList = new List<XmlReader>();
      if (MetadataArtifactLoader.IsArtifactOfDataSpace(this.Path, spaceToGet))
        xmlReaderList.Add(this._reader);
      return xmlReaderList;
    }
  }
}
